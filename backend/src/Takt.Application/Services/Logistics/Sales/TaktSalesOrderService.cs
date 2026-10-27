// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesOrderService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售订单应用服务
/// </summary>
public class TaktSalesOrderService : TaktServiceBase, ITaktSalesOrderService
{
    private readonly ITaktCompanyRepository<TaktSalesOrder> _salesOrderRepository;
    private readonly ITaktCompanyRepository<TaktSalesOrderItem> _salesOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderRepository">销售订单仓储</param>
    /// <param name="salesOrderItemRepository">SalesOrderItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesOrderService(
        ITaktCompanyRepository<TaktSalesOrder> salesOrderRepository,
        ITaktCompanyRepository<TaktSalesOrderItem> salesOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取销售订单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesOrderDto>> GetSalesOrderListAsync(TaktSalesOrderQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesOrderDto>.Create(
                new List<TaktSalesOrderDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesOrderDto>.Create(
            data.Adapt<List<TaktSalesOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto?> GetSalesOrderByIdAsync(long id)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesOrderDto>();
        await FillSalesOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesOrderOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.SalesOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesOrderCode,
            DictLabel = e.SalesOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> CreateSalesOrderAsync(TaktSalesOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesOrder>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _salesOrderRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.SalesOrderCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.SalesOrderCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.SalesOrderCode))
        {
            throw new TaktBusinessException("销售订单编码不能为空");
        }
        var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesOrderCode == entity.SalesOrderCode);
        if (!isUnique_ix_takt_logistics_sales_order_so_unique)
        {
            throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
        }
        entity = await _salesOrderRepository.CreateAsync(entity);
                await SaveSalesOrderChildrenAsync(entity, dto);
        return await GetSalesOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesOrderDto>();
    }

    /// <summary>
    /// 更新销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> UpdateSalesOrderAsync(long id, TaktSalesOrderUpdateDto dto)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesOrderCode == entity.SalesOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_order_so_unique)
        {
            throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
        }
        await _salesOrderRepository.UpdateAsync(entity);
                await SaveSalesOrderChildrenAsync(entity, dto);
        return await GetSalesOrderByIdAsync(id) ?? throw new TaktBusinessException("销售订单不存在");
    }

    /// <summary>
    /// 删除销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderByIdAsync(long id)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在或已删除");
        }
        await _salesOrderItemRepository.DeleteAsync(x => x.SalesOrderId == entity.Id);
        var deleted = await _salesOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderDto> UpdateSalesOrderStatusAsync(TaktSalesOrderStatusDto dto)
    {
        var entity = await _salesOrderRepository.GetByIdAsync(dto.SalesOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _salesOrderRepository.UpdateAsync(entity);
        return await GetSalesOrderByIdAsync(dto.SalesOrderId) ?? throw new TaktBusinessException("销售订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesOrderTemplateDto>(
            sheetName ?? "销售订单导入模板",
            fileName ?? "销售订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesOrderImportDto>(fileStream, sheetName ?? "销售订单导入模板");
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
                var entity = rows[i].Adapt<TaktSalesOrder>();
                var importKey = $"{entity.PlantCode}|{entity.SalesOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesOrderCode）");
                }
                var isUnique_ix_takt_logistics_sales_order_so_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesOrderCode == entity.SalesOrderCode);
                if (!isUnique_ix_takt_logistics_sales_order_so_unique)
                {
                    throw new TaktBusinessException("销售订单的PlantCode、SalesOrderCode已存在");
                }
                await _salesOrderRepository.CreateAsync(entity);
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
    /// 导出销售订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesOrderAsync(TaktSalesOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesOrderQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesOrderExportDto>(),
                sheetName ?? "销售订单数据",
                fileName ?? "销售订单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesOrderExportDto>(),
                sheetName ?? "销售订单数据",
                fileName ?? "销售订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售订单数据",
            fileName ?? "销售订单导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取销售订单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>销售订单统计</returns>
    public async Task<TaktSalesOrderStatDto> GetSalesOrderStatAsync(TaktSalesStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.OrderDateStart,
            queryDto.OrderDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktSalesOrder, bool>> orderPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.OrderDate >= start
            && x.OrderDate <= end;
        var monthOrderCount = await _salesOrderRepository.CountAsync(orderPredicate);
        Expression<Func<TaktSalesOrderItem, bool>> itemPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktSalesOrder>()
                .Where(o =>
                    o.Id == x.SalesOrderId
                    && o.TenantCode == tenantCode
                    && o.CompanyCode == companyCode
                    && o.OrderDate >= start
                    && o.OrderDate <= end
                    && o.IsDeleted == 0)
                .Any();
        var monthTotalAmount = await _salesOrderItemRepository.SumAsync(x => x.SalesAmount, itemPredicate);
        if (monthTotalAmount == 0 && monthOrderCount > 0)
        {
            monthTotalAmount = await _salesOrderRepository.SumAsync(x => x.TotalAmount, orderPredicate);
        }
        var compareOrderCount = 0;
        if (queryDto.CompareOrderDateStart.HasValue && queryDto.CompareOrderDateEnd.HasValue)
        {
            var compareStart = queryDto.CompareOrderDateStart.Value;
            var compareEnd = queryDto.CompareOrderDateEnd.Value;
            Expression<Func<TaktSalesOrder, bool>> comparePredicate = x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.OrderDate >= compareStart
                && x.OrderDate <= compareEnd;
            compareOrderCount = await _salesOrderRepository.CountAsync(comparePredicate);
        }
        return new TaktSalesOrderStatDto
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
    /// 将指定主表下全部未作废销售订单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesOrderItemsObsoleteAsync(long salesOrderId)
    {
        if (salesOrderId <= 0)
        {
            return;
        }
        var rows = await _salesOrderItemRepository.GetListAsync(
            x => x.SalesOrderId == salesOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesOrderItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售订单详情（加载 OneToMany 子表：销售订单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesOrderDetailsAsync(TaktSalesOrderDto dto, TaktSalesOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售订单明细 → dto.Items（含作废行）
        var items = await _salesOrderItemRepository.GetListAsync(x => x.SalesOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesOrderItemDto>>();
    }

    /// <summary>
    /// 保存销售订单子表级联（销售订单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesOrderChildrenAsync(TaktSalesOrder entity, TaktSalesOrderCreateDto dto)
    {
        // 销售订单明细（Items）
        List<TaktSalesOrderItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesOrderUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesOrderItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesOrderItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesOrderItemRepository.GetListAsync(x => x.SalesOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesOrderItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesOrderId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesOrderCode = entity.SalesOrderCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售订单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesOrderId、LineNumber）");
                }
                if (childDto.SalesOrderItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesOrderItemId, out var target))
                    {
                        throw new TaktBusinessException("销售订单明细不存在（SalesOrderItemId={childDto.SalesOrderItemId}）");
                    }
                    if (target.SalesOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("销售订单明细不属于当前主表（SalesOrderItemId={childDto.SalesOrderItemId}）");
                    }
                    submittedIds.Add(childDto.SalesOrderItemId);
                    var isUniqueUpdate_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesOrderItemRepository,
                        x => x.SalesOrderId == x.SalesOrderId
                && x.LineNumber == x.LineNumber,
                        childDto.SalesOrderItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_sales_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("销售订单明细的SalesOrderId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesOrderItemId;
                    target.SalesOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesOrderItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesOrderItemRepository,
                        x => x.SalesOrderId == x.SalesOrderId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_sales_order_item_order_line_unique)
                    {
                        throw new TaktBusinessException("销售订单明细的SalesOrderId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSalesOrderItem>();
                    child.Id = 0;
                    child.SalesOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesOrderItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesOrderCode) ? entity.SalesOrderCode : entity.Id.ToString();
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
                await _salesOrderItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesOrder, bool>> QueryExpression(TaktSalesOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesOrder>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesOrderCode != null && x.SalesOrderCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
                || (x.SalesGroup != null && x.SalesGroup.Contains(keywords))
                || (x.SalesOrderType != null && x.SalesOrderType.Contains(keywords))
                || (x.OrderReason != null && x.OrderReason.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.PricingConditionCode != null && x.PricingConditionCode.Contains(keywords))
                || (x.InvoiceType != null && x.InvoiceType.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrderCode))
        {
            var salesOrderCode = queryDto.SalesOrderCode;
            exp = exp.And(x => x.SalesOrderCode != null && x.SalesOrderCode.Contains(salesOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerName1))
        {
            var customerName1 = queryDto.CustomerName1;
            exp = exp.And(x => x.CustomerName1 != null && x.CustomerName1.Contains(customerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroup))
        {
            var salesGroup = queryDto.SalesGroup;
            exp = exp.And(x => x.SalesGroup != null && x.SalesGroup.Contains(salesGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrderType))
        {
            var salesOrderType = queryDto.SalesOrderType;
            exp = exp.And(x => x.SalesOrderType != null && x.SalesOrderType.Contains(salesOrderType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrderReason))
        {
            var orderReason = queryDto.OrderReason;
            exp = exp.And(x => x.OrderReason != null && x.OrderReason.Contains(orderReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrganization))
        {
            var salesOrganization = queryDto.SalesOrganization;
            exp = exp.And(x => x.SalesOrganization != null && x.SalesOrganization.Contains(salesOrganization));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.InvoiceType))
        {
            var invoiceType = queryDto.InvoiceType;
            exp = exp.And(x => x.InvoiceType != null && x.InvoiceType.Contains(invoiceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
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

        if (queryDto?.ShippedQuantity.HasValue == true)
        {
            var shippedQuantity = queryDto.ShippedQuantity.Value;
            exp = exp.And(x => x.ShippedQuantity == shippedQuantity);
        }

        if (queryDto?.ShippedAmount.HasValue == true)
        {
            var shippedAmount = queryDto.ShippedAmount.Value;
            exp = exp.And(x => x.ShippedAmount == shippedAmount);
        }

        if (queryDto?.ReceivedAmount.HasValue == true)
        {
            var receivedAmount = queryDto.ReceivedAmount.Value;
            exp = exp.And(x => x.ReceivedAmount == receivedAmount);
        }

        if (queryDto?.DeliveryMethod.HasValue == true)
        {
            var deliveryMethod = queryDto.DeliveryMethod.Value;
            exp = exp.And(x => x.DeliveryMethod == deliveryMethod);
        }

        if (queryDto?.PaymentMethod.HasValue == true)
        {
            var paymentMethod = queryDto.PaymentMethod.Value;
            exp = exp.And(x => x.PaymentMethod == paymentMethod);
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

        if (queryDto?.RequiredDeliveryDateStart.HasValue == true)
        {
            var requiredDeliveryDateStart = queryDto.RequiredDeliveryDateStart.Value;
            exp = exp.And(x => x.RequiredDeliveryDate >= requiredDeliveryDateStart);
        }

        if (queryDto?.RequiredDeliveryDateEnd.HasValue == true)
        {
            var requiredDeliveryDateEnd = queryDto.RequiredDeliveryDateEnd.Value;
            exp = exp.And(x => x.RequiredDeliveryDate <= requiredDeliveryDateEnd);
        }

        if (queryDto?.ActualDeliveryDateStart.HasValue == true)
        {
            var actualDeliveryDateStart = queryDto.ActualDeliveryDateStart.Value;
            exp = exp.And(x => x.ActualDeliveryDate >= actualDeliveryDateStart);
        }

        if (queryDto?.ActualDeliveryDateEnd.HasValue == true)
        {
            var actualDeliveryDateEnd = queryDto.ActualDeliveryDateEnd.Value;
            exp = exp.And(x => x.ActualDeliveryDate <= actualDeliveryDateEnd);
        }

        if (queryDto?.PurchaseOrderDateStart.HasValue == true)
        {
            var purchaseOrderDateStart = queryDto.PurchaseOrderDateStart.Value;
            exp = exp.And(x => x.PurchaseOrderDate >= purchaseOrderDateStart);
        }

        if (queryDto?.PurchaseOrderDateEnd.HasValue == true)
        {
            var purchaseOrderDateEnd = queryDto.PurchaseOrderDateEnd.Value;
            exp = exp.And(x => x.PurchaseOrderDate <= purchaseOrderDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesOrderQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrderType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrderReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrganization))
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
        if (!string.IsNullOrWhiteSpace(queryDto.InvoiceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
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
        if (queryDto.ShippedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ShippedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ReceivedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryMethod.HasValue)
        {
            return true;
        }
        if (queryDto.PaymentMethod.HasValue)
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
        if (queryDto.RequiredDeliveryDateStart.HasValue || queryDto.RequiredDeliveryDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualDeliveryDateStart.HasValue || queryDto.ActualDeliveryDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseOrderDateStart.HasValue || queryDto.PurchaseOrderDateEnd.HasValue)
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
