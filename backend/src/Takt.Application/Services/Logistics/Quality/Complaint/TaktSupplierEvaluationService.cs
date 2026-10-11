// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Domain.Entities.Logistics.Quality.Complaint;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核应用服务
/// </summary>
public class TaktSupplierEvaluationService : TaktServiceBase, ITaktSupplierEvaluationService
{
    private readonly ITaktCompanyRepository<TaktSupplierEvaluation> _supplierEvaluationRepository;
    private readonly ITaktCompanyRepository<TaktSupplierEvaluationItem> _supplierEvaluationItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierEvaluationRepository">供应商评价考核仓储</param>
    /// <param name="supplierEvaluationItemRepository">SupplierEvaluationItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierEvaluationService(
        ITaktCompanyRepository<TaktSupplierEvaluation> supplierEvaluationRepository,
        ITaktCompanyRepository<TaktSupplierEvaluationItem> supplierEvaluationItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierEvaluationRepository = supplierEvaluationRepository;
        _supplierEvaluationItemRepository = supplierEvaluationItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取供应商评价考核列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSupplierEvaluationDto>> GetSupplierEvaluationListAsync(TaktSupplierEvaluationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSupplierEvaluationDto>.Create(
                new List<TaktSupplierEvaluationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _supplierEvaluationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSupplierEvaluationDto>.Create(
            data.Adapt<List<TaktSupplierEvaluationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto?> GetSupplierEvaluationByIdAsync(long id)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSupplierEvaluationDto>();
        await FillSupplierEvaluationDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取供应商评价考核选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSupplierEvaluationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _supplierEvaluationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EvaluationStatus == 1,
            x => x.SupplierEvaluationCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SupplierEvaluationCode,
            DictLabel = e.SupplierEvaluationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建供应商评价考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> CreateSupplierEvaluationAsync(TaktSupplierEvaluationCreateDto dto)
    {
        var entity = dto.Adapt<TaktSupplierEvaluation>();
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SupplierEvaluationCode == entity.SupplierEvaluationCode);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
        {
            throw new TaktBusinessException("供应商评价考核的PlantCode、SupplierEvaluationCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _supplierEvaluationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierId == entity.SupplierId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SupplierId, maxSort);
        }
        entity = await _supplierEvaluationRepository.CreateAsync(entity);
                await SaveSupplierEvaluationChildrenAsync(entity, dto);
        return await GetSupplierEvaluationByIdAsync(entity.Id) ?? entity.Adapt<TaktSupplierEvaluationDto>();
    }

    /// <summary>
    /// 更新供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationAsync(long id, TaktSupplierEvaluationUpdateDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SupplierEvaluationCode == entity.SupplierEvaluationCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
        {
            throw new TaktBusinessException("供应商评价考核的PlantCode、SupplierEvaluationCode已存在");
        }
        await _supplierEvaluationRepository.UpdateAsync(entity);
                await SaveSupplierEvaluationChildrenAsync(entity, dto);
        return await GetSupplierEvaluationByIdAsync(id) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 删除供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationByIdAsync(long id)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在或已删除");
        }
        await _supplierEvaluationItemRepository.DeleteAsync(x => x.EvaluationId == entity.Id);
        var deleted = await _supplierEvaluationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("供应商评价考核不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除供应商评价考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSupplierEvaluationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新供应商评价考核状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationStatusAsync(TaktSupplierEvaluationStatusDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(dto.SupplierEvaluationId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        entity.EvaluationStatus = dto.EvaluationStatus;
        await _supplierEvaluationRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationByIdAsync(dto.SupplierEvaluationId) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 更新供应商评价考核排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationSortAsync(TaktSupplierEvaluationSortDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(dto.SupplierEvaluationId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _supplierEvaluationRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationByIdAsync(dto.SupplierEvaluationId) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSupplierEvaluationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSupplierEvaluationTemplateDto>(
            sheetName ?? "供应商评价考核导入模板",
            fileName ?? "供应商评价考核导入模板.xlsx");
    }

    /// <summary>
    /// 导入供应商评价考核
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSupplierEvaluationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSupplierEvaluationImportDto>(fileStream, sheetName ?? "供应商评价考核导入模板");
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
                var entity = rows[i].Adapt<TaktSupplierEvaluation>();
                var importKey = $"{entity.PlantCode}|{entity.SupplierEvaluationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SupplierEvaluationCode）");
                }
                var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierEvaluationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SupplierEvaluationCode == entity.SupplierEvaluationCode);
                if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
                {
                    throw new TaktBusinessException("供应商评价考核的PlantCode、SupplierEvaluationCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _supplierEvaluationRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierId == entity.SupplierId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SupplierId, maxSort);
                }
                await _supplierEvaluationRepository.CreateAsync(entity);
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
    /// 导出供应商评价考核
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSupplierEvaluationAsync(TaktSupplierEvaluationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSupplierEvaluationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierEvaluationExportDto>(),
                sheetName ?? "供应商评价考核数据",
                fileName ?? "供应商评价考核导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _supplierEvaluationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierEvaluationExportDto>(),
                sheetName ?? "供应商评价考核数据",
                fileName ?? "供应商评价考核导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSupplierEvaluationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "供应商评价考核数据",
            fileName ?? "供应商评价考核导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废供应商评价考核项目明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="evaluationId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSupplierEvaluationItemsObsoleteAsync(long evaluationId)
    {
        if (evaluationId <= 0)
        {
            return;
        }
        var rows = await _supplierEvaluationItemRepository.GetListAsync(
            x => x.EvaluationId == evaluationId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _supplierEvaluationItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充供应商评价考核详情（加载 OneToMany 子表：供应商评价考核项目明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSupplierEvaluationDetailsAsync(TaktSupplierEvaluationDto dto, TaktSupplierEvaluation entity)
    {
        if (dto == null)
        {
            return;
        }
        // 供应商评价考核项目明细 → dto.Items（含作废行）
        var items = await _supplierEvaluationItemRepository.GetListAsync(x => x.EvaluationId == entity.Id);
        dto.Items = items.Adapt<List<TaktSupplierEvaluationItemDto>>();
    }

    /// <summary>
    /// 保存供应商评价考核子表级联（供应商评价考核项目明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSupplierEvaluationChildrenAsync(TaktSupplierEvaluation entity, TaktSupplierEvaluationCreateDto dto)
    {
        // 供应商评价考核项目明细（Items）
        List<TaktSupplierEvaluationItemUpdateDto>? itemsForSave;
        if (dto is TaktSupplierEvaluationUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSupplierEvaluationItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSupplierEvaluationItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _supplierEvaluationItemRepository.GetListAsync(x => x.EvaluationId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSupplierEvaluationItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.EvaluationId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SupplierEvaluationCode = entity.SupplierEvaluationCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("供应商评价考核项目明细第{i + 1}项与本次提交的其他项重复（CompanyCode、EvaluationId、LineNumber）");
                }
                if (childDto.SupplierEvaluationItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SupplierEvaluationItemId, out var target))
                    {
                        throw new TaktBusinessException("供应商评价考核项目明细不存在（SupplierEvaluationItemId={childDto.SupplierEvaluationItemId}）");
                    }
                    if (target.EvaluationId != entity.Id)
                    {
                        throw new TaktBusinessException("供应商评价考核项目明细不属于当前主表（SupplierEvaluationItemId={childDto.SupplierEvaluationItemId}）");
                    }
                    submittedIds.Add(childDto.SupplierEvaluationItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _supplierEvaluationItemRepository,
                        x => x.EvaluationId == x.EvaluationId
                && x.LineNumber == x.LineNumber,
                        childDto.SupplierEvaluationItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
                    {
                        throw new TaktBusinessException("供应商评价考核项目明细的EvaluationId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SupplierEvaluationItemId;
                    target.EvaluationId = entity.Id;
                    target.IsObsolete = 0;
                    await _supplierEvaluationItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _supplierEvaluationItemRepository,
                        x => x.EvaluationId == x.EvaluationId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
                    {
                        throw new TaktBusinessException("供应商评价考核项目明细的EvaluationId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSupplierEvaluationItem>();
                    child.Id = 0;
                    child.EvaluationId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _supplierEvaluationItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SupplierEvaluationCode) ? entity.SupplierEvaluationCode : entity.Id.ToString();
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
                await _supplierEvaluationItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建供应商评价考核查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSupplierEvaluation, bool>> QueryExpression(TaktSupplierEvaluationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSupplierEvaluation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.EvaluatorByGroupName != null && x.EvaluatorByGroupName.Contains(keywords))
                || (x.EvaluationDeptName != null && x.EvaluationDeptName.Contains(keywords))
                || (x.MainStrengths != null && x.MainStrengths.Contains(keywords))
                || (x.MainIssues != null && x.MainIssues.Contains(keywords))
                || (x.ImprovementRequirements != null && x.ImprovementRequirements.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierEvaluationCode))
        {
            var supplierEvaluationCode = queryDto.SupplierEvaluationCode;
            exp = exp.And(x => x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(supplierEvaluationCode));
        }

        if (queryDto?.SupplierId.HasValue == true)
        {
            var supplierId = queryDto.SupplierId.Value;
            exp = exp.And(x => x.SupplierId == supplierId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName1))
        {
            var supplierName1 = queryDto.SupplierName1;
            exp = exp.And(x => x.SupplierName1 != null && x.SupplierName1.Contains(supplierName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (queryDto?.EvaluationPeriod.HasValue == true)
        {
            var evaluationPeriod = queryDto.EvaluationPeriod.Value;
            exp = exp.And(x => x.EvaluationPeriod == evaluationPeriod);
        }

        if (queryDto?.EvaluationType.HasValue == true)
        {
            var evaluationType = queryDto.EvaluationType.Value;
            exp = exp.And(x => x.EvaluationType == evaluationType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EvaluatorByGroupName))
        {
            var evaluatorBy = queryDto.EvaluatorByGroupName;
            exp = exp.And(x => x.EvaluatorByGroupName != null && x.EvaluatorByGroupName.Contains(evaluatorBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EvaluationDeptName))
        {
            var evaluationDept = queryDto.EvaluationDeptName;
            exp = exp.And(x => x.EvaluationDeptName != null && x.EvaluationDeptName.Contains(evaluationDept));
        }

        if (queryDto?.OverallRating.HasValue == true)
        {
            var overallRating = queryDto.OverallRating.Value;
            exp = exp.And(x => x.OverallRating == overallRating);
        }

        if (queryDto?.TotalScore.HasValue == true)
        {
            var totalScore = queryDto.TotalScore.Value;
            exp = exp.And(x => x.TotalScore == totalScore);
        }

        if (queryDto?.QualityScore.HasValue == true)
        {
            var qualityScore = queryDto.QualityScore.Value;
            exp = exp.And(x => x.QualityScore == qualityScore);
        }

        if (queryDto?.DeliveryScore.HasValue == true)
        {
            var deliveryScore = queryDto.DeliveryScore.Value;
            exp = exp.And(x => x.DeliveryScore == deliveryScore);
        }

        if (queryDto?.PriceScore.HasValue == true)
        {
            var priceScore = queryDto.PriceScore.Value;
            exp = exp.And(x => x.PriceScore == priceScore);
        }

        if (queryDto?.ServiceScore.HasValue == true)
        {
            var serviceScore = queryDto.ServiceScore.Value;
            exp = exp.And(x => x.ServiceScore == serviceScore);
        }

        if (queryDto?.TechnicalScore.HasValue == true)
        {
            var technicalScore = queryDto.TechnicalScore.Value;
            exp = exp.And(x => x.TechnicalScore == technicalScore);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MainStrengths))
        {
            var mainStrengths = queryDto.MainStrengths;
            exp = exp.And(x => x.MainStrengths != null && x.MainStrengths.Contains(mainStrengths));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MainIssues))
        {
            var mainIssues = queryDto.MainIssues;
            exp = exp.And(x => x.MainIssues != null && x.MainIssues.Contains(mainIssues));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ImprovementRequirements))
        {
            var improvementRequirements = queryDto.ImprovementRequirements;
            exp = exp.And(x => x.ImprovementRequirements != null && x.ImprovementRequirements.Contains(improvementRequirements));
        }

        if (queryDto?.EvaluationConclusion.HasValue == true)
        {
            var evaluationConclusion = queryDto.EvaluationConclusion.Value;
            exp = exp.And(x => x.EvaluationConclusion == evaluationConclusion);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Attachments))
        {
            var attachments = queryDto.Attachments;
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(attachments));
        }

        if (queryDto?.EvaluationStatus.HasValue == true)
        {
            var evaluationStatus = queryDto.EvaluationStatus.Value;
            exp = exp.And(x => x.EvaluationStatus == evaluationStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.RectificationStatus.HasValue == true)
        {
            var rectificationStatus = queryDto.RectificationStatus.Value;
            exp = exp.And(x => x.RectificationStatus == rectificationStatus);
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

        if (queryDto?.EvaluationDateStart.HasValue == true)
        {
            var evaluationDateStart = queryDto.EvaluationDateStart.Value;
            exp = exp.And(x => x.EvaluationDate >= evaluationDateStart);
        }

        if (queryDto?.EvaluationDateEnd.HasValue == true)
        {
            var evaluationDateEnd = queryDto.EvaluationDateEnd.Value;
            exp = exp.And(x => x.EvaluationDate <= evaluationDateEnd);
        }

        if (queryDto?.RectificationDeadlineStart.HasValue == true)
        {
            var rectificationDeadlineStart = queryDto.RectificationDeadlineStart.Value;
            exp = exp.And(x => x.RectificationDeadline >= rectificationDeadlineStart);
        }

        if (queryDto?.RectificationDeadlineEnd.HasValue == true)
        {
            var rectificationDeadlineEnd = queryDto.RectificationDeadlineEnd.Value;
            exp = exp.And(x => x.RectificationDeadline <= rectificationDeadlineEnd);
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
    private static bool HasAnyListQueryFilter(TaktSupplierEvaluationQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierEvaluationCode))
        {
            return true;
        }
        if (queryDto.SupplierId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (queryDto.EvaluationPeriod.HasValue)
        {
            return true;
        }
        if (queryDto.EvaluationType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EvaluatorByGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EvaluationDeptName))
        {
            return true;
        }
        if (queryDto.OverallRating.HasValue)
        {
            return true;
        }
        if (queryDto.TotalScore.HasValue)
        {
            return true;
        }
        if (queryDto.QualityScore.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryScore.HasValue)
        {
            return true;
        }
        if (queryDto.PriceScore.HasValue)
        {
            return true;
        }
        if (queryDto.ServiceScore.HasValue)
        {
            return true;
        }
        if (queryDto.TechnicalScore.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainStrengths))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainIssues))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ImprovementRequirements))
        {
            return true;
        }
        if (queryDto.EvaluationConclusion.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Attachments))
        {
            return true;
        }
        if (queryDto.EvaluationStatus.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.RectificationStatus.HasValue)
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
        if (queryDto.EvaluationDateStart.HasValue || queryDto.EvaluationDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.RectificationDeadlineStart.HasValue || queryDto.RectificationDeadlineEnd.HasValue)
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
