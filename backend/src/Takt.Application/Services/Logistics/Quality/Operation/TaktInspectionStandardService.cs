// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：检验标准应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 检验标准应用服务
/// </summary>
public class TaktInspectionStandardService : TaktServiceBase, ITaktInspectionStandardService
{
    private readonly ITaktCompanyRepository<TaktInspectionStandard> _inspectionStandardRepository;
    private readonly ITaktCompanyRepository<TaktInspectionStandardItem> _inspectionStandardItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inspectionStandardRepository">检验标准仓储</param>
    /// <param name="inspectionStandardItemRepository">InspectionStandardItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktInspectionStandardService(
        ITaktCompanyRepository<TaktInspectionStandard> inspectionStandardRepository,
        ITaktCompanyRepository<TaktInspectionStandardItem> inspectionStandardItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _inspectionStandardRepository = inspectionStandardRepository;
        _inspectionStandardItemRepository = inspectionStandardItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取检验标准列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktInspectionStandardDto>> GetInspectionStandardListAsync(TaktInspectionStandardQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktInspectionStandardDto>.Create(
                new List<TaktInspectionStandardDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _inspectionStandardRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktInspectionStandardDto>.Create(
            data.Adapt<List<TaktInspectionStandardDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardDto?> GetInspectionStandardByIdAsync(long id)
    {
        var entity = await _inspectionStandardRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktInspectionStandardDto>();
        await FillInspectionStandardDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取检验标准选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetInspectionStandardOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _inspectionStandardRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StandardStatus == 1,
            x => x.StandardName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.StandardCode,
            DictLabel = e.StandardName ?? e.StandardCode,
        }).ToList();
    }

    /// <summary>
    /// 创建检验标准
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardDto> CreateInspectionStandardAsync(TaktInspectionStandardCreateDto dto)
    {
        var entity = dto.Adapt<TaktInspectionStandard>();
        var isUnique_ix_takt_logistics_quality_inspection_standard_is_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardRepository,
            x => x.PlantCode == entity.PlantCode
                && x.StandardCode == entity.StandardCode);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_is_unique)
        {
            throw new TaktBusinessException("检验标准的PlantCode、StandardCode已存在");
        }
        entity = await _inspectionStandardRepository.CreateAsync(entity);
                await SaveInspectionStandardChildrenAsync(entity, dto);
        return await GetInspectionStandardByIdAsync(entity.Id) ?? entity.Adapt<TaktInspectionStandardDto>();
    }

    /// <summary>
    /// 更新检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardDto> UpdateInspectionStandardAsync(long id, TaktInspectionStandardUpdateDto dto)
    {
        var entity = await _inspectionStandardRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_inspection_standard_is_unique = await _uniqueValidator.IsUniqueAsync(
            _inspectionStandardRepository,
            x => x.PlantCode == entity.PlantCode
                && x.StandardCode == entity.StandardCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_inspection_standard_is_unique)
        {
            throw new TaktBusinessException("检验标准的PlantCode、StandardCode已存在");
        }
        await _inspectionStandardRepository.UpdateAsync(entity);
                await SaveInspectionStandardChildrenAsync(entity, dto);
        return await GetInspectionStandardByIdAsync(id) ?? throw new TaktBusinessException("检验标准不存在");
    }

    /// <summary>
    /// 删除检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardByIdAsync(long id)
    {
        var entity = await _inspectionStandardRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准不存在或已删除");
        }
        await _inspectionStandardItemRepository.DeleteAsync(x => x.InspectionStandardId == entity.Id);
        var deleted = await _inspectionStandardRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("检验标准不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除检验标准
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteInspectionStandardBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteInspectionStandardByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新检验标准状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktInspectionStandardDto> UpdateInspectionStandardStatusAsync(TaktInspectionStandardStatusDto dto)
    {
        var entity = await _inspectionStandardRepository.GetByIdAsync(dto.InspectionStandardId);
        if (entity == null)
        {
            throw new TaktBusinessException("检验标准不存在");
        }
        entity.StandardStatus = dto.StandardStatus;
        await _inspectionStandardRepository.UpdateAsync(entity);
        return await GetInspectionStandardByIdAsync(dto.InspectionStandardId) ?? throw new TaktBusinessException("检验标准不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetInspectionStandardTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktInspectionStandardTemplateDto>(
            sheetName ?? "检验标准导入模板",
            fileName ?? "检验标准导入模板.xlsx");
    }

    /// <summary>
    /// 导入检验标准
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportInspectionStandardAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktInspectionStandardImportDto>(fileStream, sheetName ?? "检验标准导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktInspectionStandard>();
                var importKey = $"{entity.PlantCode}|{entity.StandardCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、StandardCode）");
                }
                var isUnique_ix_takt_logistics_quality_inspection_standard_is_unique = await _uniqueValidator.IsUniqueAsync(
                    _inspectionStandardRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.StandardCode == entity.StandardCode);
                if (!isUnique_ix_takt_logistics_quality_inspection_standard_is_unique)
                {
                    throw new TaktBusinessException("检验标准的PlantCode、StandardCode已存在");
                }
                await _inspectionStandardRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出检验标准
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportInspectionStandardAsync(TaktInspectionStandardQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktInspectionStandardQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInspectionStandardExportDto>(),
                sheetName ?? "检验标准数据",
                fileName ?? "检验标准导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _inspectionStandardRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktInspectionStandardExportDto>(),
                sheetName ?? "检验标准数据",
                fileName ?? "检验标准导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktInspectionStandardExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "检验标准数据",
            fileName ?? "检验标准导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废检验标准明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="inspectionStandardId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkInspectionStandardItemsObsoleteAsync(long inspectionStandardId)
    {
        if (inspectionStandardId <= 0)
        {
            return;
        }
        var rows = await _inspectionStandardItemRepository.GetListAsync(
            x => x.InspectionStandardId == inspectionStandardId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _inspectionStandardItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充检验标准详情（加载 OneToMany 子表：检验标准明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillInspectionStandardDetailsAsync(TaktInspectionStandardDto dto, TaktInspectionStandard entity)
    {
        if (dto == null)
        {
            return;
        }
        // 检验标准明细 → dto.Items（含作废行）
        var items = await _inspectionStandardItemRepository.GetListAsync(x => x.InspectionStandardId == entity.Id);
        dto.Items = items.Adapt<List<TaktInspectionStandardItemDto>>();
    }

    /// <summary>
    /// 保存检验标准子表级联（检验标准明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveInspectionStandardChildrenAsync(TaktInspectionStandard entity, TaktInspectionStandardCreateDto dto)
    {
        // 检验标准明细（Items）
        List<TaktInspectionStandardItemUpdateDto>? itemsForSave;
        if (dto is TaktInspectionStandardUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktInspectionStandardItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkInspectionStandardItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _inspectionStandardItemRepository.GetListAsync(x => x.InspectionStandardId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktInspectionStandardItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.InspectionStandardId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("检验标准明细第{i + 1}项与本次提交的其他项重复（CompanyCode、InspectionStandardId、LineNumber）");
                }
                if (childDto.InspectionStandardItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.InspectionStandardItemId, out var target))
                    {
                        throw new TaktBusinessException("检验标准明细不存在（InspectionStandardItemId={childDto.InspectionStandardItemId}）");
                    }
                    if (target.InspectionStandardId != entity.Id)
                    {
                        throw new TaktBusinessException("检验标准明细不属于当前主表（InspectionStandardItemId={childDto.InspectionStandardItemId}）");
                    }
                    submittedIds.Add(childDto.InspectionStandardItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
                        _inspectionStandardItemRepository,
                        x => x.InspectionStandardId == x.InspectionStandardId
                && x.LineNumber == x.LineNumber
                && x.ItemCode == x.ItemCode
                && x.ItemType == x.ItemType,
                        childDto.InspectionStandardItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_inspection_standard_item_unique)
                    {
                        throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.InspectionStandardItemId;
                    target.InspectionStandardId = entity.Id;
                    target.IsObsolete = 0;
                    await _inspectionStandardItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_inspection_standard_item_unique = await _uniqueValidator.IsUniqueAsync(
                        _inspectionStandardItemRepository,
                        x => x.InspectionStandardId == x.InspectionStandardId
                && x.LineNumber == x.LineNumber
                && x.ItemCode == x.ItemCode
                && x.ItemType == x.ItemType);
                    if (!isUniqueCreate_ix_takt_logistics_quality_inspection_standard_item_unique)
                    {
                        throw new TaktBusinessException("检验标准明细的InspectionStandardId、LineNumber、ItemCode、ItemType已存在");
                    }
                    var child = childDto.Adapt<TaktInspectionStandardItem>();
                    child.Id = 0;
                    child.InspectionStandardId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _inspectionStandardItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.StandardCode) ? entity.StandardCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _inspectionStandardItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建检验标准查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktInspectionStandard, bool>> QueryExpression(TaktInspectionStandardQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktInspectionStandard>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.StandardCode != null && x.StandardCode.Contains(keywords))
                || (x.StandardName != null && x.StandardName.Contains(keywords))
                || (x.MaterialCategoryCode != null && x.MaterialCategoryCode.Contains(keywords))
                || (x.MaterialCategoryName != null && x.MaterialCategoryName.Contains(keywords))
                || (x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(keywords))
                || (x.SamplingSchemeName != null && x.SamplingSchemeName.Contains(keywords))
                || (x.StandardDescription != null && x.StandardDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardCode))
        {
            var standardCode = queryDto.StandardCode;
            exp = exp.And(x => x.StandardCode != null && x.StandardCode.Contains(standardCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardName))
        {
            var standardName = queryDto.StandardName;
            exp = exp.And(x => x.StandardName != null && x.StandardName.Contains(standardName));
        }

        if (queryDto?.InspectionType.HasValue == true)
        {
            var inspectionType = queryDto.InspectionType.Value;
            exp = exp.And(x => x.InspectionType == inspectionType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCategoryCode))
        {
            var materialCategoryCode = queryDto.MaterialCategoryCode;
            exp = exp.And(x => x.MaterialCategoryCode != null && x.MaterialCategoryCode.Contains(materialCategoryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCategoryName))
        {
            var materialCategoryName = queryDto.MaterialCategoryName;
            exp = exp.And(x => x.MaterialCategoryName != null && x.MaterialCategoryName.Contains(materialCategoryName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SamplingSchemeCode))
        {
            var samplingSchemeCode = queryDto.SamplingSchemeCode;
            exp = exp.And(x => x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(samplingSchemeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SamplingSchemeName))
        {
            var samplingSchemeName = queryDto.SamplingSchemeName;
            exp = exp.And(x => x.SamplingSchemeName != null && x.SamplingSchemeName.Contains(samplingSchemeName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardDescription))
        {
            var standardDescription = queryDto.StandardDescription;
            exp = exp.And(x => x.StandardDescription != null && x.StandardDescription.Contains(standardDescription));
        }

        if (queryDto?.StandardStatus.HasValue == true)
        {
            var standardStatus = queryDto.StandardStatus.Value;
            exp = exp.And(x => x.StandardStatus == standardStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktInspectionStandardQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardName))
        {
            return true;
        }
        if (queryDto.InspectionType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCategoryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCategoryName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SamplingSchemeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SamplingSchemeName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardDescription))
        {
            return true;
        }
        if (queryDto.StandardStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
