// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseOrderService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购订单应用服务
/// </summary>
public class TaktPurchaseOrderService : TaktServiceBase, ITaktPurchaseOrderService
{
    private readonly ITaktCompanyRepository<TaktPurchaseOrder> _purchaseOrderRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderRepository">采购订单仓储</param>
    /// <param name="purchaseOrderItemRepository">PurchaseOrderItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseOrderService(
        ITaktCompanyRepository<TaktPurchaseOrder> purchaseOrderRepository,
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取采购订单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseOrderDto>> GetPurchaseOrderListAsync(TaktPurchaseOrderQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseOrderDto>.Create(
                new List<TaktPurchaseOrderDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseOrderDto>.Create(
            data.Adapt<List<TaktPurchaseOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto?> GetPurchaseOrderByIdAsync(long id)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseOrderDto>();
        await FillPurchaseOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseOrderOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.PurchaseOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseOrderCode,
            DictLabel = e.PurchaseOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> CreatePurchaseOrderAsync(TaktPurchaseOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseOrder>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.PurchaseOrderCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.PurchaseOrderCode))
        {
            throw new TaktBusinessException("采购订单编码不能为空");
        }
        var isUnique_ix_takt_logistics_procurement_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseOrderCode == entity.PurchaseOrderCode
                && x.SupplierCode == entity.SupplierCode
                && x.OrderDate == entity.OrderDate);
        if (!isUnique_ix_takt_logistics_procurement_purchase_order_po_unique)
        {
            throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
        }
        entity = await _purchaseOrderRepository.CreateAsync(entity);
                await SavePurchaseOrderChildrenAsync(entity, dto);
        return await GetPurchaseOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseOrderDto>();
    }

    /// <summary>
    /// 更新采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> UpdatePurchaseOrderAsync(long id, TaktPurchaseOrderUpdateDto dto)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseOrderCode == entity.PurchaseOrderCode
                && x.SupplierCode == entity.SupplierCode
                && x.OrderDate == entity.OrderDate,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_order_po_unique)
        {
            throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
        }
        await _purchaseOrderRepository.UpdateAsync(entity);
                await SavePurchaseOrderChildrenAsync(entity, dto);
        return await GetPurchaseOrderByIdAsync(id) ?? throw new TaktBusinessException("采购订单不存在");
    }

    /// <summary>
    /// 删除采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderByIdAsync(long id)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在或已删除");
        }
        await _purchaseOrderItemRepository.DeleteAsync(x => x.PurchaseOrderId == entity.Id);
        var deleted = await _purchaseOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderDto> UpdatePurchaseOrderStatusAsync(TaktPurchaseOrderStatusDto dto)
    {
        var entity = await _purchaseOrderRepository.GetByIdAsync(dto.PurchaseOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _purchaseOrderRepository.UpdateAsync(entity);
        return await GetPurchaseOrderByIdAsync(dto.PurchaseOrderId) ?? throw new TaktBusinessException("采购订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseOrderTemplateDto>(
            sheetName ?? "采购订单导入模板",
            fileName ?? "采购订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseOrderImportDto>(fileStream, sheetName ?? "采购订单导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseOrder>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseOrderCode}|{entity.SupplierCode}|{entity.OrderDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseOrderCode、SupplierCode、OrderDate）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_order_po_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseOrderCode == entity.PurchaseOrderCode
                        && x.SupplierCode == entity.SupplierCode
                        && x.OrderDate == entity.OrderDate);
                if (!isUnique_ix_takt_logistics_procurement_purchase_order_po_unique)
                {
                    throw new TaktBusinessException("采购订单的PlantCode、PurchaseOrderCode、SupplierCode、OrderDate已存在");
                }
                await _purchaseOrderRepository.CreateAsync(entity);
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
    /// 导出采购订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseOrderAsync(TaktPurchaseOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseOrderQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseOrderExportDto>(),
                sheetName ?? "采购订单数据",
                fileName ?? "采购订单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseOrderExportDto>(),
                sheetName ?? "采购订单数据",
                fileName ?? "采购订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购订单数据",
            fileName ?? "采购订单导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取采购订单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>采购订单统计</returns>
    public async Task<TaktPurchaseOrderStatDto> GetPurchaseOrderStatAsync(TaktProcurementStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.OrderDateStart,
            queryDto.OrderDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPurchaseOrder, bool>> orderPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.OrderDate >= start
            && x.OrderDate <= end;
        var monthOrderCount = await _purchaseOrderRepository.CountAsync(orderPredicate);
        // 采购金额取明细 purchase_amount（未作废行）；主表 total_amount 同步源常为 0
        Expression<Func<TaktPurchaseOrderItem, bool>> itemPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktPurchaseOrder>()
                .Where(o =>
                    o.Id == x.PurchaseOrderId
                    && o.TenantCode == tenantCode
                    && o.CompanyCode == companyCode
                    && o.OrderDate >= start
                    && o.OrderDate <= end
                    && o.IsDeleted == 0)
                .Any();
        var monthTotalAmount = await _purchaseOrderItemRepository.SumAsync(x => x.PurchaseAmount, itemPredicate);
        if (monthTotalAmount == 0 && monthOrderCount > 0)
        {
            // 回退：明细无金额时用主表 total_amount（元）
            monthTotalAmount = await _purchaseOrderRepository.SumAsync(x => x.TotalAmount, orderPredicate);
        }
        var compareOrderCount = 0;
        if (queryDto.CompareOrderDateStart.HasValue && queryDto.CompareOrderDateEnd.HasValue)
        {
            var compareStart = queryDto.CompareOrderDateStart.Value;
            var compareEnd = queryDto.CompareOrderDateEnd.Value;
            Expression<Func<TaktPurchaseOrder, bool>> comparePredicate = x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.OrderDate >= compareStart
                && x.OrderDate <= compareEnd;
            compareOrderCount = await _purchaseOrderRepository.CountAsync(comparePredicate);
        }
        return new TaktPurchaseOrderStatDto
        {
            StatMonth = statMonth,
            MonthOrderCount = monthOrderCount,
            MonthTotalAmount = monthTotalAmount,
            CompareOrderCount = compareOrderCount,
            OrderCountYoYPercent = TaktYoYStatHelper.CalculateYoYPercent(monthOrderCount, compareOrderCount),
        };
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购订单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseOrderItemsObsoleteAsync(long purchaseOrderId)
    {
        if (purchaseOrderId <= 0)
        {
            return;
        }
        var rows = await _purchaseOrderItemRepository.GetListAsync(
            x => x.PurchaseOrderId == purchaseOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseOrderItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购订单详情（加载 OneToMany 子表：采购订单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseOrderDetailsAsync(TaktPurchaseOrderDto dto, TaktPurchaseOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购订单明细 → dto.Items（含作废行）
        var items = await _purchaseOrderItemRepository.GetListAsync(x => x.PurchaseOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseOrderItemDto>>();
    }

    /// <summary>
    /// 保存采购订单子表级联（采购订单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseOrderChildrenAsync(TaktPurchaseOrder entity, TaktPurchaseOrderCreateDto dto)
    {
        // 采购订单明细（Items）
        List<TaktPurchaseOrderItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseOrderUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseOrderItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseOrderItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseOrderItemRepository.GetListAsync(x => x.PurchaseOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseOrderItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseOrderId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseOrderCode = entity.PurchaseOrderCode;
                childDto.TaxCode = entity.TaxCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购订单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseOrderId、LineNumber）");
                }
                if (childDto.PurchaseOrderItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseOrderItemId, out var target))
                    {
                        throw new TaktBusinessException("采购订单明细不存在（PurchaseOrderItemId={childDto.PurchaseOrderItemId}）");
                    }
                    if (target.PurchaseOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("采购订单明细不属于当前主表（PurchaseOrderItemId={childDto.PurchaseOrderItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseOrderItemId);
                    var isUniqueUpdate_ix_takt_logistics_procurement_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseOrderItemRepository,
                        x => x.PurchaseOrderId == x.PurchaseOrderId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.PurchaseOrderItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_procurement_purchase_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("采购订单明细的PurchaseOrderId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseOrderItemId;
                    target.PurchaseOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseOrderItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_procurement_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseOrderItemRepository,
                        x => x.PurchaseOrderId == x.PurchaseOrderId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_procurement_purchase_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("采购订单明细的PurchaseOrderId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktPurchaseOrderItem>();
                    child.Id = 0;
                    child.PurchaseOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseOrderItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseOrderCode) ? entity.PurchaseOrderCode : entity.Id.ToString();
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
                await _purchaseOrderItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseOrder, bool>> QueryExpression(TaktPurchaseOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseOrder>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.PurchaseOrderType != null && x.PurchaseOrderType.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.PricingConditionCode != null && x.PricingConditionCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.DeliveryAddress != null && x.DeliveryAddress.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
        }

        if (queryDto?.PurchaseRequestId.HasValue == true)
        {
            var purchaseRequestId = queryDto.PurchaseRequestId.Value;
            exp = exp.And(x => x.PurchaseRequestId == purchaseRequestId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseRequestCode))
        {
            var purchaseRequestCode = queryDto.PurchaseRequestCode;
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(purchaseRequestCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName1))
        {
            var supplierName1 = queryDto.SupplierName1;
            exp = exp.And(x => x.SupplierName1 != null && x.SupplierName1.Contains(supplierName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderType))
        {
            var purchaseOrderType = queryDto.PurchaseOrderType;
            exp = exp.And(x => x.PurchaseOrderType != null && x.PurchaseOrderType.Contains(purchaseOrderType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentTerms))
        {
            var paymentTerms = queryDto.PaymentTerms;
            exp = exp.And(x => x.PaymentTerms != null && x.PaymentTerms.Contains(paymentTerms));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingProcedure))
        {
            var pricingProcedure = queryDto.PricingProcedure;
            exp = exp.And(x => x.PricingProcedure != null && x.PricingProcedure.Contains(pricingProcedure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingConditionCode))
        {
            var pricingConditionCode = queryDto.PricingConditionCode;
            exp = exp.And(x => x.PricingConditionCode != null && x.PricingConditionCode.Contains(pricingConditionCode));
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

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            var discountAmount = queryDto.DiscountAmount.Value;
            exp = exp.And(x => x.DiscountAmount == discountAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (queryDto?.ExchangeRate.HasValue == true)
        {
            var exchangeRate = queryDto.ExchangeRate.Value;
            exp = exp.And(x => x.ExchangeRate == exchangeRate);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            var taxRate = queryDto.TaxRate.Value;
            exp = exp.And(x => x.TaxRate == taxRate);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (queryDto?.ActualAmount.HasValue == true)
        {
            var actualAmount = queryDto.ActualAmount.Value;
            exp = exp.And(x => x.ActualAmount == actualAmount);
        }

        if (queryDto?.ReceivedQuantity.HasValue == true)
        {
            var receivedQuantity = queryDto.ReceivedQuantity.Value;
            exp = exp.And(x => x.ReceivedQuantity == receivedQuantity);
        }

        if (queryDto?.ReceivedAmount.HasValue == true)
        {
            var receivedAmount = queryDto.ReceivedAmount.Value;
            exp = exp.And(x => x.ReceivedAmount == receivedAmount);
        }

        if (queryDto?.PaidAmount.HasValue == true)
        {
            var paidAmount = queryDto.PaidAmount.Value;
            exp = exp.And(x => x.PaidAmount == paidAmount);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            var paymentMethod = queryDto.PaymentMethod.Value;
            exp = exp.And(x => x.PaymentMethod == paymentMethod);
        }

        if (queryDto?.DeliveryMethod.HasValue == true)
        {
            var deliveryMethod = queryDto.DeliveryMethod.Value;
            exp = exp.And(x => x.DeliveryMethod == deliveryMethod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeliveryAddress))
        {
            var deliveryAddress = queryDto.DeliveryAddress;
            exp = exp.And(x => x.DeliveryAddress != null && x.DeliveryAddress.Contains(deliveryAddress));
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            var orderStatus = queryDto.OrderStatus.Value;
            exp = exp.And(x => x.OrderStatus == orderStatus);
        }

        if (queryDto?.DeliveryStatus.HasValue == true)
        {
            var deliveryStatus = queryDto.DeliveryStatus.Value;
            exp = exp.And(x => x.DeliveryStatus == deliveryStatus);
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

        if (queryDto?.OrderDateStart.HasValue == true)
        {
            var orderDateStart = queryDto.OrderDateStart.Value;
            exp = exp.And(x => x.OrderDate >= orderDateStart);
        }

        if (queryDto?.OrderDateEnd.HasValue == true)
        {
            var orderDateEnd = queryDto.OrderDateEnd.Value;
            exp = exp.And(x => x.OrderDate <= orderDateEnd);
        }

        if (queryDto?.RequiredArrivalDateStart.HasValue == true)
        {
            var requiredArrivalDateStart = queryDto.RequiredArrivalDateStart.Value;
            exp = exp.And(x => x.RequiredArrivalDate >= requiredArrivalDateStart);
        }

        if (queryDto?.RequiredArrivalDateEnd.HasValue == true)
        {
            var requiredArrivalDateEnd = queryDto.RequiredArrivalDateEnd.Value;
            exp = exp.And(x => x.RequiredArrivalDate <= requiredArrivalDateEnd);
        }

        if (queryDto?.ActualArrivalDateStart.HasValue == true)
        {
            var actualArrivalDateStart = queryDto.ActualArrivalDateStart.Value;
            exp = exp.And(x => x.ActualArrivalDate >= actualArrivalDateStart);
        }

        if (queryDto?.ActualArrivalDateEnd.HasValue == true)
        {
            var actualArrivalDateEnd = queryDto.ActualArrivalDateEnd.Value;
            exp = exp.And(x => x.ActualArrivalDate <= actualArrivalDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseOrderQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
        {
            return true;
        }
        if (queryDto.PurchaseRequestId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseRequestCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentTerms))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingConditionCode))
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
        if (queryDto.DiscountAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (queryDto.ExchangeRate.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.TaxRate.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ActualAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ReceivedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ReceivedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.PaidAmount.HasValue)
        {
            return true;
        }
        if (queryDto.PaymentMethod.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryMethod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeliveryAddress))
        {
            return true;
        }
        if (queryDto.OrderStatus.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryStatus.HasValue)
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
        if (queryDto.OrderDateStart.HasValue || queryDto.OrderDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.RequiredArrivalDateStart.HasValue || queryDto.RequiredArrivalDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualArrivalDateStart.HasValue || queryDto.ActualArrivalDateEnd.HasValue)
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
