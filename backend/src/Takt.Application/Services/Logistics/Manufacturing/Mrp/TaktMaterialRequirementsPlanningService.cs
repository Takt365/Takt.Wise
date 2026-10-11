// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划MRP头应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Domain.Entities.Logistics.Manufacturing.Mrp;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mrp;

/// <summary>
/// 物料需求计划MRP头应用服务
/// </summary>
public class TaktMaterialRequirementsPlanningService : TaktServiceBase, ITaktMaterialRequirementsPlanningService
{
    private readonly ITaktApprovalRepository<TaktMaterialRequirementsPlanning> _materialRequirementsPlanningRepository;
    private readonly ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> _materialRequirementsPlanningItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRequirementsPlanningRepository">物料需求计划MRP头仓储</param>
    /// <param name="materialRequirementsPlanningItemRepository">MaterialRequirementsPlanningItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialRequirementsPlanningService(
        ITaktApprovalRepository<TaktMaterialRequirementsPlanning> materialRequirementsPlanningRepository,
        ITaktCompanyRepository<TaktMaterialRequirementsPlanningItem> materialRequirementsPlanningItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialRequirementsPlanningRepository = materialRequirementsPlanningRepository;
        _materialRequirementsPlanningItemRepository = materialRequirementsPlanningItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料需求计划MRP头列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialRequirementsPlanningDto>> GetMaterialRequirementsPlanningListAsync(TaktMaterialRequirementsPlanningQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaterialRequirementsPlanningDto>.Create(
                new List<TaktMaterialRequirementsPlanningDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialRequirementsPlanningRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialRequirementsPlanningDto>.Create(
            data.Adapt<List<TaktMaterialRequirementsPlanningDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningDto?> GetMaterialRequirementsPlanningByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaterialRequirementsPlanningDto>();
        await FillMaterialRequirementsPlanningDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料需求计划MRP头选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialRequirementsPlanningOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialRequirementsPlanningRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RunStatus == 1,
            x => x.MaterialRequirementsPlanningCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialRequirementsPlanningCode,
            DictLabel = e.MaterialRequirementsPlanningCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料需求计划MRP头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningDto> CreateMaterialRequirementsPlanningAsync(TaktMaterialRequirementsPlanningCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialRequirementsPlanning>();
        var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialRequirementsPlanningCode == entity.MaterialRequirementsPlanningCode
                && x.PlanDate == entity.PlanDate);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP头的PlantCode、MaterialRequirementsPlanningCode、PlanDate已存在");
        }
        entity = await _materialRequirementsPlanningRepository.CreateAsync(entity);
                await SaveMaterialRequirementsPlanningChildrenAsync(entity, dto);
        return await GetMaterialRequirementsPlanningByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialRequirementsPlanningDto>();
    }

    /// <summary>
    /// 更新物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningDto> UpdateMaterialRequirementsPlanningAsync(long id, TaktMaterialRequirementsPlanningUpdateDto dto)
    {
        var entity = await _materialRequirementsPlanningRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP头不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique = await _uniqueValidator.IsUniqueAsync(
            _materialRequirementsPlanningRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialRequirementsPlanningCode == entity.MaterialRequirementsPlanningCode
                && x.PlanDate == entity.PlanDate,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique)
        {
            throw new TaktBusinessException("物料需求计划MRP头的PlantCode、MaterialRequirementsPlanningCode、PlanDate已存在");
        }
        await _materialRequirementsPlanningRepository.UpdateAsync(entity);
                await SaveMaterialRequirementsPlanningChildrenAsync(entity, dto);
        return await GetMaterialRequirementsPlanningByIdAsync(id) ?? throw new TaktBusinessException("物料需求计划MRP头不存在");
    }

    /// <summary>
    /// 删除物料需求计划MRP头
    /// </summary>
    /// <param name="id">物料需求计划MRP头ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningByIdAsync(long id)
    {
        var entity = await _materialRequirementsPlanningRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP头不存在或已删除");
        }
        await _materialRequirementsPlanningItemRepository.DeleteAsync(x => x.MaterialRequirementsPlanningId == entity.Id);
        var deleted = await _materialRequirementsPlanningRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料需求计划MRP头不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料需求计划MRP头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialRequirementsPlanningBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialRequirementsPlanningByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料需求计划MRP头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialRequirementsPlanningDto> UpdateMaterialRequirementsPlanningStatusAsync(TaktMaterialRequirementsPlanningStatusDto dto)
    {
        var entity = await _materialRequirementsPlanningRepository.GetByIdAsync(dto.MaterialRequirementsPlanningId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料需求计划MRP头不存在");
        }
        entity.RunStatus = dto.RunStatus;
        await _materialRequirementsPlanningRepository.UpdateAsync(entity);
        return await GetMaterialRequirementsPlanningByIdAsync(dto.MaterialRequirementsPlanningId) ?? throw new TaktBusinessException("物料需求计划MRP头不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialRequirementsPlanningTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialRequirementsPlanningTemplateDto>(
            sheetName ?? "物料需求计划MRP头导入模板",
            fileName ?? "物料需求计划MRP头导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料需求计划MRP头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialRequirementsPlanningAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialRequirementsPlanningImportDto>(fileStream, sheetName ?? "物料需求计划MRP头导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialRequirementsPlanning>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialRequirementsPlanningCode}|{entity.PlanDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialRequirementsPlanningCode、PlanDate）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialRequirementsPlanningRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialRequirementsPlanningCode == entity.MaterialRequirementsPlanningCode
                        && x.PlanDate == entity.PlanDate);
                if (!isUnique_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_unique)
                {
                    throw new TaktBusinessException("物料需求计划MRP头的PlantCode、MaterialRequirementsPlanningCode、PlanDate已存在");
                }
                await _materialRequirementsPlanningRepository.CreateAsync(entity);
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
    /// 导出物料需求计划MRP头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialRequirementsPlanningAsync(TaktMaterialRequirementsPlanningQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaterialRequirementsPlanningQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialRequirementsPlanningExportDto>(),
                sheetName ?? "物料需求计划MRP头数据",
                fileName ?? "物料需求计划MRP头导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _materialRequirementsPlanningRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialRequirementsPlanningExportDto>(),
                sheetName ?? "物料需求计划MRP头数据",
                fileName ?? "物料需求计划MRP头导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialRequirementsPlanningExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料需求计划MRP头数据",
            fileName ?? "物料需求计划MRP头导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废物料需求计划MRP明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="materialRequirementsPlanningId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMaterialRequirementsPlanningItemsObsoleteAsync(long materialRequirementsPlanningId)
    {
        if (materialRequirementsPlanningId <= 0)
        {
            return;
        }
        var rows = await _materialRequirementsPlanningItemRepository.GetListAsync(
            x => x.MaterialRequirementsPlanningId == materialRequirementsPlanningId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _materialRequirementsPlanningItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充物料需求计划MRP头详情（加载 OneToMany 子表：物料需求计划MRP明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaterialRequirementsPlanningDetailsAsync(TaktMaterialRequirementsPlanningDto dto, TaktMaterialRequirementsPlanning entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料需求计划MRP明细 → dto.Items（含作废行）
        var items = await _materialRequirementsPlanningItemRepository.GetListAsync(x => x.MaterialRequirementsPlanningId == entity.Id);
        dto.Items = items.Adapt<List<TaktMaterialRequirementsPlanningItemDto>>();
    }

    /// <summary>
    /// 保存物料需求计划MRP头子表级联（物料需求计划MRP明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaterialRequirementsPlanningChildrenAsync(TaktMaterialRequirementsPlanning entity, TaktMaterialRequirementsPlanningCreateDto dto)
    {
        // 物料需求计划MRP明细（Items）
        List<TaktMaterialRequirementsPlanningItemUpdateDto>? itemsForSave;
        if (dto is TaktMaterialRequirementsPlanningUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktMaterialRequirementsPlanningItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkMaterialRequirementsPlanningItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _materialRequirementsPlanningItemRepository.GetListAsync(x => x.MaterialRequirementsPlanningId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaterialRequirementsPlanningItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.MaterialRequirementsPlanningId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.MaterialRequirementsPlanningCode = entity.MaterialRequirementsPlanningCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("物料需求计划MRP明细第{i + 1}项与本次提交的其他项重复（CompanyCode、MaterialRequirementsPlanningId、LineNumber）");
                }
                if (childDto.MaterialRequirementsPlanningItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaterialRequirementsPlanningItemId, out var target))
                    {
                        throw new TaktBusinessException("物料需求计划MRP明细不存在（MaterialRequirementsPlanningItemId={childDto.MaterialRequirementsPlanningItemId}）");
                    }
                    if (target.MaterialRequirementsPlanningId != entity.Id)
                    {
                        throw new TaktBusinessException("物料需求计划MRP明细不属于当前主表（MaterialRequirementsPlanningItemId={childDto.MaterialRequirementsPlanningItemId}）");
                    }
                    submittedIds.Add(childDto.MaterialRequirementsPlanningItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialRequirementsPlanningItemRepository,
                        x => x.MaterialRequirementsPlanningId == x.MaterialRequirementsPlanningId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.MaterialRequirementsPlanningItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique)
                    {
                        throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MaterialRequirementsPlanningItemId;
                    target.MaterialRequirementsPlanningId = entity.Id;
                    target.IsObsolete = 0;
                    await _materialRequirementsPlanningItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialRequirementsPlanningItemRepository,
                        x => x.MaterialRequirementsPlanningId == x.MaterialRequirementsPlanningId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique)
                    {
                        throw new TaktBusinessException("物料需求计划MRP明细的MaterialRequirementsPlanningId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktMaterialRequirementsPlanningItem>();
                    child.Id = 0;
                    child.MaterialRequirementsPlanningId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _materialRequirementsPlanningItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.Id.ToString();
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
                await _materialRequirementsPlanningItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料需求计划MRP头查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialRequirementsPlanning, bool>> QueryExpression(TaktMaterialRequirementsPlanningQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialRequirementsPlanning>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || (x.MpsCode != null && x.MpsCode.Contains(keywords))
                || (x.MdsCode != null && x.MdsCode.Contains(keywords))
                || (x.PlannerGroupName != null && x.PlannerGroupName.Contains(keywords))
                || (x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(keywords))
                || (x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(keywords))
                || (x.PlanDescription != null && x.PlanDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialRequirementsPlanningCode))
        {
            var materialRequirementsPlanningCode = queryDto.MaterialRequirementsPlanningCode;
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(materialRequirementsPlanningCode));
        }

        if (queryDto?.MasterProductionScheduleId.HasValue == true)
        {
            var masterProductionScheduleId = queryDto.MasterProductionScheduleId.Value;
            exp = exp.And(x => x.MasterProductionScheduleId == masterProductionScheduleId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MpsCode))
        {
            var mpsCode = queryDto.MpsCode;
            exp = exp.And(x => x.MpsCode != null && x.MpsCode.Contains(mpsCode));
        }

        if (queryDto?.MasterDemandScheduleId.HasValue == true)
        {
            var masterDemandScheduleId = queryDto.MasterDemandScheduleId.Value;
            exp = exp.And(x => x.MasterDemandScheduleId == masterDemandScheduleId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MdsCode))
        {
            var mdsCode = queryDto.MdsCode;
            exp = exp.And(x => x.MdsCode != null && x.MdsCode.Contains(mdsCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupCode))
        {
            var plannerGroupCode = queryDto.PlannerGroupCode;
            exp = exp.And(x => x.PlannerGroupCode == plannerGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupName))
        {
            var planBy = queryDto.PlannerGroupName;
            exp = exp.And(x => x.PlannerGroupName != null && x.PlannerGroupName.Contains(planBy));
        }

        if (queryDto?.RunStatus.HasValue == true)
        {
            var runStatus = queryDto.RunStatus.Value;
            exp = exp.And(x => x.RunStatus == runStatus);
        }

        if (queryDto?.ProductionPlanId.HasValue == true)
        {
            var productionPlanId = queryDto.ProductionPlanId.Value;
            exp = exp.And(x => x.ProductionPlanId == productionPlanId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionPlanCode))
        {
            var productionPlanCode = queryDto.ProductionPlanCode;
            exp = exp.And(x => x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(productionPlanCode));
        }

        if (queryDto?.PurchasePlanId.HasValue == true)
        {
            var purchasePlanId = queryDto.PurchasePlanId.Value;
            exp = exp.And(x => x.PurchasePlanId == purchasePlanId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasePlanCode))
        {
            var purchasePlanCode = queryDto.PurchasePlanCode;
            exp = exp.And(x => x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(purchasePlanCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanDescription))
        {
            var planDescription = queryDto.PlanDescription;
            exp = exp.And(x => x.PlanDescription != null && x.PlanDescription.Contains(planDescription));
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

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            var planDateStart = queryDto.PlanDateStart.Value;
            exp = exp.And(x => x.PlanDate >= planDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            var planDateEnd = queryDto.PlanDateEnd.Value;
            exp = exp.And(x => x.PlanDate <= planDateEnd);
        }

        if (queryDto?.PlanPeriodStartStart.HasValue == true)
        {
            var planPeriodStartStart = queryDto.PlanPeriodStartStart.Value;
            exp = exp.And(x => x.PlanPeriodStart >= planPeriodStartStart);
        }

        if (queryDto?.PlanPeriodStartEnd.HasValue == true)
        {
            var planPeriodStartEnd = queryDto.PlanPeriodStartEnd.Value;
            exp = exp.And(x => x.PlanPeriodStart <= planPeriodStartEnd);
        }

        if (queryDto?.PlanPeriodEndStart.HasValue == true)
        {
            var planPeriodEndStart = queryDto.PlanPeriodEndStart.Value;
            exp = exp.And(x => x.PlanPeriodEnd >= planPeriodEndStart);
        }

        if (queryDto?.PlanPeriodEndEnd.HasValue == true)
        {
            var planPeriodEndEnd = queryDto.PlanPeriodEndEnd.Value;
            exp = exp.And(x => x.PlanPeriodEnd <= planPeriodEndEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaterialRequirementsPlanningQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialRequirementsPlanningCode))
        {
            return true;
        }
        if (queryDto.MasterProductionScheduleId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MpsCode))
        {
            return true;
        }
        if (queryDto.MasterDemandScheduleId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MdsCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupName))
        {
            return true;
        }
        if (queryDto.RunStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ProductionPlanId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionPlanCode))
        {
            return true;
        }
        if (queryDto.PurchasePlanId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasePlanCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanDescription))
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
        if (queryDto.PlanDateStart.HasValue || queryDto.PlanDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanPeriodStartStart.HasValue || queryDto.PlanPeriodStartEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanPeriodEndStart.HasValue || queryDto.PlanPeriodEndEnd.HasValue)
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
