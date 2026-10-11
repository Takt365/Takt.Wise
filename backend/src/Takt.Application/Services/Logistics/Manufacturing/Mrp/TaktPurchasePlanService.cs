// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlanService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：采购计划应用服务实现
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
/// 采购计划应用服务
/// </summary>
public class TaktPurchasePlanService : TaktServiceBase, ITaktPurchasePlanService
{
    private readonly ITaktApprovalRepository<TaktPurchasePlan> _purchasePlanRepository;
    private readonly ITaktCompanyRepository<TaktPurchasePlanItem> _purchasePlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePlanRepository">采购计划仓储</param>
    /// <param name="purchasePlanItemRepository">PurchasePlanItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePlanService(
        ITaktApprovalRepository<TaktPurchasePlan> purchasePlanRepository,
        ITaktCompanyRepository<TaktPurchasePlanItem> purchasePlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchasePlanRepository = purchasePlanRepository;
        _purchasePlanItemRepository = purchasePlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购计划列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchasePlanDto>> GetPurchasePlanListAsync(TaktPurchasePlanQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchasePlanDto>.Create(
                new List<TaktPurchasePlanDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchasePlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchasePlanDto>.Create(
            data.Adapt<List<TaktPurchasePlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanDto?> GetPurchasePlanByIdAsync(long id)
    {
        var entity = await _purchasePlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchasePlanDto>();
        await FillPurchasePlanDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购计划选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePlanOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchasePlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PlanStatus == 1,
            x => x.PurchasePlanCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchasePlanCode,
            DictLabel = e.PurchasePlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanDto> CreatePurchasePlanAsync(TaktPurchasePlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchasePlan>();
        var isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchasePlanCode == entity.PurchasePlanCode
                && x.PlanDate == entity.PlanDate);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique)
        {
            throw new TaktBusinessException("采购计划的PlantCode、PurchasePlanCode、PlanDate已存在");
        }
        entity = await _purchasePlanRepository.CreateAsync(entity);
                await SavePurchasePlanChildrenAsync(entity, dto);
        return await GetPurchasePlanByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchasePlanDto>();
    }

    /// <summary>
    /// 更新采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanDto> UpdatePurchasePlanAsync(long id, TaktPurchasePlanUpdateDto dto)
    {
        var entity = await _purchasePlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _purchasePlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchasePlanCode == entity.PurchasePlanCode
                && x.PlanDate == entity.PlanDate,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique)
        {
            throw new TaktBusinessException("采购计划的PlantCode、PurchasePlanCode、PlanDate已存在");
        }
        await _purchasePlanRepository.UpdateAsync(entity);
                await SavePurchasePlanChildrenAsync(entity, dto);
        return await GetPurchasePlanByIdAsync(id) ?? throw new TaktBusinessException("采购计划不存在");
    }

    /// <summary>
    /// 删除采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePlanByIdAsync(long id)
    {
        var entity = await _purchasePlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划不存在或已删除");
        }
        await _purchasePlanItemRepository.DeleteAsync(x => x.PurchasePlanId == entity.Id);
        var deleted = await _purchasePlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购计划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchasePlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchasePlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购计划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchasePlanDto> UpdatePurchasePlanStatusAsync(TaktPurchasePlanStatusDto dto)
    {
        var entity = await _purchasePlanRepository.GetByIdAsync(dto.PurchasePlanId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购计划不存在");
        }
        entity.PlanStatus = dto.PlanStatus;
        await _purchasePlanRepository.UpdateAsync(entity);
        return await GetPurchasePlanByIdAsync(dto.PurchasePlanId) ?? throw new TaktBusinessException("采购计划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchasePlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchasePlanTemplateDto>(
            sheetName ?? "采购计划导入模板",
            fileName ?? "采购计划导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchasePlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchasePlanImportDto>(fileStream, sheetName ?? "采购计划导入模板");
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
                var entity = rows[i].Adapt<TaktPurchasePlan>();
                var importKey = $"{entity.PlantCode}|{entity.PurchasePlanCode}|{entity.PlanDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchasePlanCode、PlanDate）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchasePlanRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchasePlanCode == entity.PurchasePlanCode
                        && x.PlanDate == entity.PlanDate);
                if (!isUnique_ix_takt_logistics_manufacturing_mrp_purchase_plan_unique)
                {
                    throw new TaktBusinessException("采购计划的PlantCode、PurchasePlanCode、PlanDate已存在");
                }
                await _purchasePlanRepository.CreateAsync(entity);
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
    /// 导出采购计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePlanAsync(TaktPurchasePlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchasePlanQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePlanExportDto>(),
                sheetName ?? "采购计划数据",
                fileName ?? "采购计划导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchasePlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchasePlanExportDto>(),
                sheetName ?? "采购计划数据",
                fileName ?? "采购计划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchasePlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购计划数据",
            fileName ?? "采购计划导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购计划明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchasePlanId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchasePlanItemsObsoleteAsync(long purchasePlanId)
    {
        if (purchasePlanId <= 0)
        {
            return;
        }
        var rows = await _purchasePlanItemRepository.GetListAsync(
            x => x.PurchasePlanId == purchasePlanId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchasePlanItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购计划详情（加载 OneToMany 子表：采购计划明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchasePlanDetailsAsync(TaktPurchasePlanDto dto, TaktPurchasePlan entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购计划明细 → dto.Items（含作废行）
        var items = await _purchasePlanItemRepository.GetListAsync(x => x.PurchasePlanId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchasePlanItemDto>>();
    }

    /// <summary>
    /// 保存采购计划子表级联（采购计划明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchasePlanChildrenAsync(TaktPurchasePlan entity, TaktPurchasePlanCreateDto dto)
    {
        // 采购计划明细（Items）
        List<TaktPurchasePlanItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchasePlanUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchasePlanItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchasePlanItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchasePlanItemRepository.GetListAsync(x => x.PurchasePlanId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchasePlanItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchasePlanId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchasePlanCode = entity.PurchasePlanCode;
                childDto.ProductionPlanCode = entity.ProductionPlanCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购计划明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchasePlanId、LineNumber）");
                }
                if (childDto.PurchasePlanItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchasePlanItemId, out var target))
                    {
                        throw new TaktBusinessException("采购计划明细不存在（PurchasePlanItemId={childDto.PurchasePlanItemId}）");
                    }
                    if (target.PurchasePlanId != entity.Id)
                    {
                        throw new TaktBusinessException("采购计划明细不属于当前主表（PurchasePlanItemId={childDto.PurchasePlanItemId}）");
                    }
                    submittedIds.Add(childDto.PurchasePlanItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_purchase_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchasePlanItemRepository,
                        x => x.PurchasePlanId == x.PurchasePlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.PurchasePlanItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_mrp_purchase_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("采购计划明细的PurchasePlanId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PurchasePlanItemId;
                    target.PurchasePlanId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchasePlanItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_mrp_purchase_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchasePlanItemRepository,
                        x => x.PurchasePlanId == x.PurchasePlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_mrp_purchase_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("采购计划明细的PurchasePlanId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktPurchasePlanItem>();
                    child.Id = 0;
                    child.PurchasePlanId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchasePlanItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchasePlanCode) ? entity.PurchasePlanCode : entity.Id.ToString();
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
                await _purchasePlanItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购计划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchasePlan, bool>> QueryExpression(TaktPurchasePlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchasePlan>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(keywords))
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || (x.ProductionPlanCode != null && x.ProductionPlanCode.Contains(keywords))
                || (x.PurchaseGroupCode != null && x.PurchaseGroupCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasePlanCode))
        {
            var purchasePlanCode = queryDto.PurchasePlanCode;
            exp = exp.And(x => x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(purchasePlanCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroupCode))
        {
            var purchaseGroupCode = queryDto.PurchaseGroupCode;
            exp = exp.And(x => x.PurchaseGroupCode != null && x.PurchaseGroupCode.Contains(purchaseGroupCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupCode))
        {
            var plannerGroupCode = queryDto.PlannerGroupCode;
            exp = exp.And(x => x.PlannerGroupCode == plannerGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupName))
        {
            var plannerGroupName = queryDto.PlannerGroupName;
            exp = exp.And(x => x.PlannerGroupName != null && x.PlannerGroupName.Contains(plannerGroupName));
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
    private static bool HasAnyListQueryFilter(TaktPurchasePlanQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasePlanCode))
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
        if (queryDto.ProductionPlanId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionPlanCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroupCode))
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
