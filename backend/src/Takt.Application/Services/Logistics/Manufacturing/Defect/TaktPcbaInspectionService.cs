// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查日报应用服务
/// </summary>
public class TaktPcbaInspectionService : TaktServiceBase, ITaktPcbaInspectionService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspection> _pcbaInspectionRepository;
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionRepository">PCBA检查日报仓储</param>
    /// <param name="pcbaInspectionDetailRepository">PcbaInspectionDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaInspectionService(
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionRepository = pcbaInspectionRepository;
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA检查日报列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaInspectionDto>> GetPcbaInspectionListAsync(TaktPcbaInspectionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPcbaInspectionDto>.Create(
                new List<TaktPcbaInspectionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaInspectionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaInspectionDto>.Create(
            data.Adapt<List<TaktPcbaInspectionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto?> GetPcbaInspectionByIdAsync(long id)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPcbaInspectionDto>();
        await FillPcbaInspectionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取PCBA检查日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaInspectionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaInspectionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA检查日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto> CreatePcbaInspectionAsync(TaktPcbaInspectionCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaInspection>();
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionRepository,
            x => x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
        {
            throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
        }
        entity = await _pcbaInspectionRepository.CreateAsync(entity);
                await SavePcbaInspectionChildrenAsync(entity, dto);
        return await GetPcbaInspectionByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaInspectionDto>();
    }

    /// <summary>
    /// 更新PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto> UpdatePcbaInspectionAsync(long id, TaktPcbaInspectionUpdateDto dto)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionRepository,
            x => x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
        {
            throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
        }
        await _pcbaInspectionRepository.UpdateAsync(entity);
                await SavePcbaInspectionChildrenAsync(entity, dto);
        return await GetPcbaInspectionByIdAsync(id) ?? throw new TaktBusinessException("PCBA检查日报不存在");
    }

    /// <summary>
    /// 删除PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionByIdAsync(long id)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在或已删除");
        }
        await _pcbaInspectionDetailRepository.DeleteAsync(x => x.PcbaInspectionId == entity.Id);
        var deleted = await _pcbaInspectionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA检查日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA检查日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaInspectionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaInspectionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaInspectionTemplateDto>(
            sheetName ?? "PCBA检查日报导入模板",
            fileName ?? "PCBA检查日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA检查日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaInspectionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaInspectionImportDto>(fileStream, sheetName ?? "PCBA检查日报导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaInspection>();
                var importKey = $"{entity.ProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaInspectionRepository,
                    x => x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
                {
                    throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
                }
                await _pcbaInspectionRepository.CreateAsync(entity);
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
    /// 导出PCBA检查日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaInspectionAsync(TaktPcbaInspectionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPcbaInspectionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionExportDto>(),
                sheetName ?? "PCBA检查日报数据",
                fileName ?? "PCBA检查日报导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _pcbaInspectionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionExportDto>(),
                sheetName ?? "PCBA检查日报数据",
                fileName ?? "PCBA检查日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaInspectionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA检查日报数据",
            fileName ?? "PCBA检查日报导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废PCBA检查明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="pcbaInspectionId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPcbaInspectionDetailsObsoleteAsync(long pcbaInspectionId)
    {
        if (pcbaInspectionId <= 0)
        {
            return;
        }
        var rows = await _pcbaInspectionDetailRepository.GetListAsync(
            x => x.PcbaInspectionId == pcbaInspectionId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _pcbaInspectionDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充PCBA检查日报详情（加载 OneToMany 子表：PCBA检查明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPcbaInspectionDetailsAsync(TaktPcbaInspectionDto dto, TaktPcbaInspection entity)
    {
        if (dto == null)
        {
            return;
        }
        // PCBA检查明细 → dto.PcbaInspectionDetails（含作废行）
        var pcbainspectiondetails = await _pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == entity.Id);
        dto.PcbaInspectionDetails = pcbainspectiondetails.Adapt<List<TaktPcbaInspectionDetailDto>>();
    }

    /// <summary>
    /// 保存PCBA检查日报子表级联（PCBA检查明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePcbaInspectionChildrenAsync(TaktPcbaInspection entity, TaktPcbaInspectionCreateDto dto)
    {
        // PCBA检查明细（PcbaInspectionDetails）
        List<TaktPcbaInspectionDetailUpdateDto>? pcbaInspectionDetailsForSave;
        if (dto is TaktPcbaInspectionUpdateDto updateDtoForPcbaInspectionDetails && updateDtoForPcbaInspectionDetails.PcbaInspectionDetails != null)
        {
            pcbaInspectionDetailsForSave = updateDtoForPcbaInspectionDetails.PcbaInspectionDetails;
        }
        else if (dto.PcbaInspectionDetails != null)
        {
            pcbaInspectionDetailsForSave = dto.PcbaInspectionDetails.Adapt<List<TaktPcbaInspectionDetailUpdateDto>>();
        }
        else
        {
            pcbaInspectionDetailsForSave = null;
        }
        if (pcbaInspectionDetailsForSave is not { Count: > 0 })
        {
            await MarkPcbaInspectionDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPcbaInspectionDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < pcbaInspectionDetailsForSave.Count; i++)
            {
                var childDto = pcbaInspectionDetailsForSave[i];
                childDto.PcbaInspectionId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ProdOrderCode = entity.ProdOrderCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("PCBA检查明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PcbaInspectionId、LineNumber）");
                }
                if (childDto.PcbaInspectionDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PcbaInspectionDetailId, out var target))
                    {
                        throw new TaktBusinessException("PCBA检查明细不存在（PcbaInspectionDetailId={childDto.PcbaInspectionDetailId}）");
                    }
                    if (target.PcbaInspectionId != entity.Id)
                    {
                        throw new TaktBusinessException("PCBA检查明细不属于当前主表（PcbaInspectionDetailId={childDto.PcbaInspectionDetailId}）");
                    }
                    submittedIds.Add(childDto.PcbaInspectionDetailId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaInspectionDetailRepository,
                        x => x.PcbaInspectionId == x.PcbaInspectionId
                && x.LineNumber == x.LineNumber,
                        childDto.PcbaInspectionDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PcbaInspectionDetailId;
                    target.PcbaInspectionId = entity.Id;
                    target.IsObsolete = 0;
                    await _pcbaInspectionDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaInspectionDetailRepository,
                        x => x.PcbaInspectionId == x.PcbaInspectionId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktPcbaInspectionDetail>();
                    child.Id = 0;
                    child.PcbaInspectionId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _pcbaInspectionDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ProdOrderCode) ? entity.ProdOrderCode : entity.Id.ToString();
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
                await _pcbaInspectionDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA检查日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaInspection, bool>> QueryExpression(TaktPcbaInspectionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaInspection>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdCategory))
        {
            var prodCategory = queryDto.ProdCategory;
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(prodCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderType))
        {
            var prodOrderType = queryDto.ProdOrderType;
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(prodOrderType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            var prodOrderQty = queryDto.ProdOrderQty.Value;
            exp = exp.And(x => x.ProdOrderQty == prodOrderQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
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
    private static bool HasAnyListQueryFilter(TaktPcbaInspectionQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ProdCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
        {
            return true;
        }
        if (queryDto.ProdOrderQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
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

    // ========================================
    // 扩展方法（数据看板）
    // ========================================

    /// <summary>
    /// 获取 PCBA 检查不良统计（数据看板 defect-stat；主表无 ProdDate，按明细实装日期区间）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 检查统计</returns>
    public async Task<TaktPcbaInspectionStatDto> GetPcbaInspectionStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        // 主表无 ProdDate；用明细 B/T 面实装日期落入区间，并校验主表未删除
        Expression<Func<TaktPcbaInspectionDetail, bool>> detailPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && (
                (x.TSideAssemblyDate != null && x.TSideAssemblyDate >= start && x.TSideAssemblyDate <= end)
                || (x.BSideAssemblyDate != null && x.BSideAssemblyDate >= start && x.BSideAssemblyDate <= end))
            && SqlFunc.Subqueryable<TaktPcbaInspection>()
                .Where(h =>
                    h.Id == x.PcbaInspectionId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.IsDeleted == 0)
                .Any();
        var details = await _pcbaInspectionDetailRepository.GetListAsync(detailPredicate);
        var teams = details
            .GroupBy(d => d.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var baseQty = g.Sum(d => d.InspectionQty);
                var defectQty = g.Sum(d => d.DefectQty);
                var goodQty = Math.Max(0m, baseQty - defectQty);
                return new TaktDefectStatTeamItemDto
                {
                    TeamCode = g.Key,
                    BaseQty = baseQty,
                    GoodQty = goodQty,
                    DefectQty = defectQty,
                    DefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(defectQty, baseQty),
                    YieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(goodQty, baseQty),
                };
            })
            .ToList();
        var monthBaseQty = teams.Sum(t => t.BaseQty);
        var monthDefectQty = teams.Sum(t => t.DefectQty);
        var monthGoodQty = Math.Max(0m, monthBaseQty - monthDefectQty);
        return new TaktPcbaInspectionStatDto
        {
            StatMonth = statMonth,
            MonthBaseQty = monthBaseQty,
            MonthGoodQty = monthGoodQty,
            MonthDefectQty = monthDefectQty,
            MonthDefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(monthDefectQty, monthBaseQty),
            MonthYieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(monthGoodQty, monthBaseQty),
            Teams = teams,
        };
    }
}
