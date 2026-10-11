// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：生产计划应用服务实现
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
/// 生产计划应用服务
/// </summary>
public class TaktProductionPlanService : TaktServiceBase, ITaktProductionPlanService
{
    private readonly ITaktApprovalRepository<TaktProductionPlan> _productionPlanRepository;
    private readonly ITaktCompanyRepository<TaktProductionPlanItem> _productionPlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionPlanRepository">生产计划仓储</param>
    /// <param name="productionPlanItemRepository">ProductionPlanItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProductionPlanService(
        ITaktApprovalRepository<TaktProductionPlan> productionPlanRepository,
        ITaktCompanyRepository<TaktProductionPlanItem> productionPlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _productionPlanRepository = productionPlanRepository;
        _productionPlanItemRepository = productionPlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取生产计划列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProductionPlanDto>> GetProductionPlanListAsync(TaktProductionPlanQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktProductionPlanDto>.Create(
                new List<TaktProductionPlanDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _productionPlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProductionPlanDto>.Create(
            data.Adapt<List<TaktProductionPlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取生产计划
    /// </summary>
    /// <param name="id">生产计划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanDto?> GetProductionPlanByIdAsync(long id)
    {
        var entity = await _productionPlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktProductionPlanDto>();
        await FillProductionPlanDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取生产计划选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProductionPlanOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _productionPlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PlanStatus == 1,
            x => x.ProductionPlanCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProductionPlanCode,
            DictLabel = e.ProductionPlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建生产计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanDto> CreateProductionPlanAsync(TaktProductionPlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktProductionPlan>();
        var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _productionPlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductionPlanCode == entity.ProductionPlanCode
                && x.PlanDate == entity.PlanDate);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique)
        {
            throw new TaktBusinessException("生产计划的PlantCode、ProductionPlanCode、PlanDate已存在");
        }
        entity = await _productionPlanRepository.CreateAsync(entity);
                await SaveProductionPlanChildrenAsync(entity, dto);
        return await GetProductionPlanByIdAsync(entity.Id) ?? entity.Adapt<TaktProductionPlanDto>();
    }

    /// <summary>
    /// 更新生产计划
    /// </summary>
    /// <param name="id">生产计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanDto> UpdateProductionPlanAsync(long id, TaktProductionPlanUpdateDto dto)
    {
        var entity = await _productionPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _productionPlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProductionPlanCode == entity.ProductionPlanCode
                && x.PlanDate == entity.PlanDate,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique)
        {
            throw new TaktBusinessException("生产计划的PlantCode、ProductionPlanCode、PlanDate已存在");
        }
        await _productionPlanRepository.UpdateAsync(entity);
                await SaveProductionPlanChildrenAsync(entity, dto);
        return await GetProductionPlanByIdAsync(id) ?? throw new TaktBusinessException("生产计划不存在");
    }

    /// <summary>
    /// 删除生产计划
    /// </summary>
    /// <param name="id">生产计划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionPlanByIdAsync(long id)
    {
        var entity = await _productionPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划不存在或已删除");
        }
        await _productionPlanItemRepository.DeleteAsync(x => x.ProductionPlanId == entity.Id);
        var deleted = await _productionPlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("生产计划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除生产计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProductionPlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProductionPlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新生产计划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProductionPlanDto> UpdateProductionPlanStatusAsync(TaktProductionPlanStatusDto dto)
    {
        var entity = await _productionPlanRepository.GetByIdAsync(dto.ProductionPlanId);
        if (entity == null)
        {
            throw new TaktBusinessException("生产计划不存在");
        }
        entity.PlanStatus = dto.PlanStatus;
        await _productionPlanRepository.UpdateAsync(entity);
        return await GetProductionPlanByIdAsync(dto.ProductionPlanId) ?? throw new TaktBusinessException("生产计划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProductionPlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProductionPlanTemplateDto>(
            sheetName ?? "生产计划导入模板",
            fileName ?? "生产计划导入模板.xlsx");
    }

    /// <summary>
    /// 导入生产计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProductionPlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProductionPlanImportDto>(fileStream, sheetName ?? "生产计划导入模板");
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
                var entity = rows[i].Adapt<TaktProductionPlan>();
                var importKey = $"{entity.PlantCode}|{entity.ProductionPlanCode}|{entity.PlanDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProductionPlanCode、PlanDate）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique = await _uniqueValidator.IsUniqueAsync(
                    _productionPlanRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProductionPlanCode == entity.ProductionPlanCode
                        && x.PlanDate == entity.PlanDate);
                if (!isUnique_ix_takt_logistics_manufacturing_mrp_production_plan_unique)
                {
                    throw new TaktBusinessException("生产计划的PlantCode、ProductionPlanCode、PlanDate已存在");
                }
                await _productionPlanRepository.CreateAsync(entity);
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
    /// 导出生产计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProductionPlanAsync(TaktProductionPlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktProductionPlanQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionPlanExportDto>(),
                sheetName ?? "生产计划数据",
                fileName ?? "生产计划导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _productionPlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProductionPlanExportDto>(),
                sheetName ?? "生产计划数据",
                fileName ?? "生产计划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProductionPlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "生产计划数据",
            fileName ?? "生产计划导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废生产计划明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="productionPlanId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkProductionPlanItemsObsoleteAsync(long productionPlanId)
    {
        if (productionPlanId <= 0)
        {
            return;
        }
        var rows = await _productionPlanItemRepository.GetListAsync(
            x => x.ProductionPlanId == productionPlanId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _productionPlanItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充生产计划详情（加载 OneToMany 子表：生产计划明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillProductionPlanDetailsAsync(TaktProductionPlanDto dto, TaktProductionPlan entity)
    {
        if (dto == null)
        {
            return;
        }
        // 生产计划明细 → dto.Items（含作废行）
        var items = await _productionPlanItemRepository.GetListAsync(x => x.ProductionPlanId == entity.Id);
        dto.Items = items.Adapt<List<TaktProductionPlanItemDto>>();
    }

    /// <summary>
    /// 保存生产计划子表级联（生产计划明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveProductionPlanChildrenAsync(TaktProductionPlan entity, TaktProductionPlanCreateDto dto)
    {
        // 生产计划明细（Items）
        List<TaktProductionPlanItemUpdateDto>? itemsForSave;
        if (dto is TaktProductionPlanUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktProductionPlanItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkProductionPlanItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _productionPlanItemRepository.GetListAsync(x => x.ProductionPlanId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktProductionPlanItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.ProductionPlanId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ProductionPlanCode = entity.ProductionPlanCode;
                childDto.SalesForecastCode = entity.SalesForecastCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("生产计划明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ProductionPlanId、LineNumber）");
                }
                if (childDto.ProductionPlanItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ProductionPlanItemId, out var target))
                    {
                        throw new TaktBusinessException("生产计划明细不存在（ProductionPlanItemId={childDto.ProductionPlanItemId}）");
                    }
                    if (target.ProductionPlanId != entity.Id)
                    {
                        throw new TaktBusinessException("生产计划明细不属于当前主表（ProductionPlanItemId={childDto.ProductionPlanItemId}）");
                    }
                    submittedIds.Add(childDto.ProductionPlanItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _productionPlanItemRepository,
                        x => x.ProductionPlanId == x.ProductionPlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.ProductionPlanItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("生产计划明细的ProductionPlanId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.ProductionPlanItemId;
                    target.ProductionPlanId = entity.Id;
                    target.IsObsolete = 0;
                    await _productionPlanItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _productionPlanItemRepository,
                        x => x.ProductionPlanId == x.ProductionPlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_mrp_production_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("生产计划明细的ProductionPlanId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktProductionPlanItem>();
                    child.Id = 0;
                    child.ProductionPlanId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _productionPlanItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ProductionPlanCode) ? entity.ProductionPlanCode : entity.Id.ToString();
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
                await _productionPlanItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建生产计划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProductionPlan, bool>> QueryExpression(TaktProductionPlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProductionPlan>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(keywords))
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || (x.SalesForecastCode != null && x.SalesForecastCode.Contains(keywords))
                || (x.PlannerGroupName != null && x.PlannerGroupName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionPlanCode))
        {
            var productionPlanCode = queryDto.ProductionPlanCode;
            exp = exp.And(x => x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(productionPlanCode));
        }

        if (queryDto?.MaterialRequirementsPlanningId.HasValue == true)
        {
            var materialRequirementsPlanningId = queryDto.MaterialRequirementsPlanningId.Value;
            exp = exp.And(x => x.MaterialRequirementsPlanningId == materialRequirementsPlanningId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialRequirementsPlanningCode))
        {
            var materialRequirementsPlanningCode = queryDto.MaterialRequirementsPlanningCode;
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(materialRequirementsPlanningCode));
        }

        if (queryDto?.SalesForecastId.HasValue == true)
        {
            var salesForecastId = queryDto.SalesForecastId.Value;
            exp = exp.And(x => x.SalesForecastId == salesForecastId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesForecastCode))
        {
            var salesForecastCode = queryDto.SalesForecastCode;
            exp = exp.And(x => x.SalesForecastCode != null && x.SalesForecastCode.Contains(salesForecastCode));
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

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            var totalQuantity = queryDto.TotalQuantity.Value;
            exp = exp.And(x => x.TotalQuantity == totalQuantity);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            var totalAmount = queryDto.TotalAmount.Value;
            exp = exp.And(x => x.TotalAmount == totalAmount);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.ConvertedAmount.HasValue == true)
        {
            var convertedAmount = queryDto.ConvertedAmount.Value;
            exp = exp.And(x => x.ConvertedAmount == convertedAmount);
        }

        if (queryDto?.PlanStatus.HasValue == true)
        {
            var planStatus = queryDto.PlanStatus.Value;
            exp = exp.And(x => x.PlanStatus == planStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            var convertedStatus = queryDto.ConvertedStatus.Value;
            exp = exp.And(x => x.ConvertedStatus == convertedStatus);
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
    private static bool HasAnyListQueryFilter(TaktProductionPlanQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionPlanCode))
        {
            return true;
        }
        if (queryDto.MaterialRequirementsPlanningId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialRequirementsPlanningCode))
        {
            return true;
        }
        if (queryDto.SalesForecastId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesForecastCode))
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
        if (queryDto.TotalQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TotalAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.PlanStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedStatus.HasValue)
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
