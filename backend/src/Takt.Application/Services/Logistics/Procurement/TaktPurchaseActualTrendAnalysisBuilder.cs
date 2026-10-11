// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseActualTrendAnalysisBuilder.cs
// 创建时间：2026-09-19
// 创建人：Takt365(Cursor AI)
// 功能描述：采购实绩推移构建器（列=过账年月；源=采购发票；单价=金额/数量；H 贷方取反）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购实绩推移转置分析构建器（按发票过账日期划月；单价取金额/数量；借/贷标识 H 取反）
/// </summary>
public class TaktPurchaseActualTrendAnalysisBuilder : TaktServiceBase, ITaktPurchaseActualTrendAnalysisBuilder
{
    /// <summary>
    /// 物料/供应商名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchaseInvoice> _invoiceRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInvoiceItem> _itemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="invoiceRepository">采购发票主表仓储</param>
    /// <param name="itemRepository">采购发票行项目仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseActualTrendAnalysisBuilder(
        ITaktCompanyRepository<TaktPurchaseInvoice> invoiceRepository,
        ITaktCompanyRepository<TaktPurchaseInvoiceItem> itemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _invoiceRepository = invoiceRepository;
        _itemRepository = itemRepository;
        _materialPlantRepository = materialPlantRepository;
        _supplierRepository = supplierRepository;
    }

    /// <summary>
    /// 构建采购实绩推移转置分析全量结果（列=过账年月；金额浮动=单价差×当月数量）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    public async Task<TaktPurchasePriceTrendAnalysisBuilt> BuildAsync(TaktPurchasePriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var supplierFilter = string.IsNullOrWhiteSpace(queryDto.SupplierCode) ? null : queryDto.SupplierCode.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var (rangeStart, rangeEndExclusive) = ResolveActualTrendDateBounds(queryDto);
        var periodOrder = BuildPeriodMonthKeys(rangeStart, rangeEndExclusive);
        if (periodOrder.Count > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
        {
            throw new ArgumentException($"分析区间不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 个月");
        }
        var invoices = await _invoiceRepository.GetListAsync(
            BuildInvoiceExpression(rangeStart, rangeEndExclusive));
        if (invoices.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var headerById = invoices.ToDictionary(
            d => d.Id,
            d => (PostingDate: d.PostingDate, SupplierCode: d.SupplierCode?.Trim() ?? string.Empty, Currency: d.CurrencyCode?.Trim() ?? string.Empty));
        var invoiceIds = headerById.Keys.ToList();
        var items = await LoadInvoiceItemsAsync(invoiceIds, plantCode, materialFilter);
        if (items.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var facts = items
            .Where(i => headerById.ContainsKey(i.PurchaseInvoiceId))
            .Select(i =>
            {
                var header = headerById[i.PurchaseInvoiceId];
                var signed = ApplyDebitCreditSign(i);
                var plant = ResolveItemPlantCode(i);
                return new PurchaseActualTrendFact(
                    plant,
                    i.MaterialCode!.Trim(),
                    header.SupplierCode,
                    header.PostingDate,
                    signed.Quantity,
                    signed.Amount,
                    i.OrderUnit?.Trim() ?? string.Empty,
                    header.Currency);
            })
            .Where(f => !string.IsNullOrWhiteSpace(f.PlantCode)
                && string.Equals(f.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && (supplierFilter == null || string.Equals(f.SupplierCode, supplierFilter, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        if (facts.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var allRows = facts
            .GroupBy(
                r => new PurchaseActualTrendRowKey(r.PlantCode, r.MaterialCode, string.Empty),
                PurchaseActualTrendRowKeyComparer.Instance)
            .Select(g => BuildActualTrendRow(g.Key, g.ToList(), periodOrder))
            .Where(r => (r.PeriodPoints?.Count ?? 0) > 0)
            .ToList();
        await FillActualTrendDisplayNamesAsync(plantCode, allRows);
        var filtered = FilterActualTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderActualTrendRows(filtered);
        var monthSummaries = BuildActualMonthSummaries(ordered, periodOrder);
        return new TaktPurchasePriceTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = periodOrder.FirstOrDefault(),
            ComparePeriod = periodOrder.LastOrDefault(),
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
            MonthSummaries = monthSummaries,
            TotalAmount = monthSummaries.Sum(s => s.Amount),
            TotalUpAmount = monthSummaries.Sum(s => s.UpAmount),
            TotalDownAmount = monthSummaries.Sum(s => s.DownAmount),
        };
    }

    /// <summary>
    /// 解析行项目工厂：优先 PlantCode（公司基类）；空时回退 ValuationArea（Length=4）
    /// </summary>
    /// <param name="item">发票行</param>
    /// <returns>工厂代码</returns>
    private static string ResolveItemPlantCode(TaktPurchaseInvoiceItem item)
    {
        var plant = item.PlantCode?.Trim() ?? string.Empty;
        if (!string.IsNullOrEmpty(plant))
        {
            return plant;
        }
        var valuation = item.ValuationArea?.Trim() ?? string.Empty;
        return valuation.Length == 4 ? valuation : string.Empty;
    }

    /// <summary>
    /// 构建采购发票主表过账日期筛选
    /// </summary>
    /// <param name="postingDateStart">过账日期区间起（含）</param>
    /// <param name="postingDateEndExclusive">过账日期区间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktPurchaseInvoice, bool>> BuildInvoiceExpression(
        DateTime postingDateStart,
        DateTime postingDateEndExclusive)
    {
        var exp = Expressionable.Create<TaktPurchaseInvoice>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PostingDate >= postingDateStart
            && x.PostingDate < postingDateEndExclusive);
        return exp.ToExpression();
    }

    /// <summary>
    /// 分批加载采购发票行（未作废、物料非空；工厂在内存侧再过滤以兼容 ValuationArea）
    /// </summary>
    /// <param name="invoiceIds">发票主键</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialFilter">物料编码</param>
    /// <returns>行项目</returns>
    private async Task<List<TaktPurchaseInvoiceItem>> LoadInvoiceItemsAsync(
        IReadOnlyList<long> invoiceIds,
        string plantCode,
        string? materialFilter)
    {
        var result = new List<TaktPurchaseInvoiceItem>();
        if (invoiceIds.Count == 0)
        {
            return result;
        }
        for (var offset = 0; offset < invoiceIds.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = invoiceIds.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var exp = Expressionable.Create<TaktPurchaseInvoiceItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.IsObsolete == 0
                && batch.Contains(x.PurchaseInvoiceId)
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty);
            if (!string.IsNullOrWhiteSpace(materialFilter))
            {
                var material = materialFilter.Trim();
                exp = exp.And(x => x.MaterialCode == material);
            }
            var page = await _itemRepository.GetListAsync(exp.ToExpression());
            if (page.Count > 0)
            {
                // 优先 PlantCode；空则 ValuationArea==plant（4 位）
                result.AddRange(page.Where(i =>
                {
                    var p = ResolveItemPlantCode(i);
                    return string.Equals(p, plantCode, StringComparison.OrdinalIgnoreCase);
                }));
            }
        }
        return result;
    }

    /// <summary>
    /// 借/贷标识 H（贷方）对数量与金额取反；其余保持原值
    /// </summary>
    /// <param name="item">发票行</param>
    /// <returns>带符号数量与金额</returns>
    private static (decimal Quantity, decimal Amount) ApplyDebitCreditSign(TaktPurchaseInvoiceItem item)
    {
        var qty = item.Quantity ?? 0m;
        var amount = item.Amount ?? 0m;
        var indicator = item.DebitCreditIndicator?.Trim() ?? string.Empty;
        if (string.Equals(indicator, "H", StringComparison.OrdinalIgnoreCase))
        {
            return (-qty, -amount);
        }
        return (qty, amount);
    }

    /// <summary>
    /// 解析采购实绩推移查询的过账日期闭开区间
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起（含）与止（不含）</returns>
    private static (DateTime RangeStart, DateTime RangeEndExclusive) ResolveActualTrendDateBounds(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizeActualPeriodBounds(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);
        if (periodStart.HasValue || periodEnd.HasValue)
        {
            var startMonth = periodStart ?? periodEnd!.Value;
            var endMonth = periodEnd ?? periodStart!.Value;
            if (startMonth > endMonth)
            {
                (startMonth, endMonth) = (endMonth, startMonth);
            }
            var monthCount = ((endMonth.Year - startMonth.Year) * 12) + endMonth.Month - startMonth.Month + 1;
            if (monthCount > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
            {
                throw new ArgumentException($"分析区间不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 个月");
            }
            return (startMonth, endMonth.AddMonths(1));
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        return (resolvedStart, resolvedEnd.Date.AddDays(1));
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizeActualPeriodBounds(
        DateTime? periodDateStart,
        DateTime? periodDateEnd)
    {
        DateTime? start = periodDateStart.HasValue
            ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
            : null;
        DateTime? end = periodDateEnd.HasValue
            ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
            : null;
        if (start.HasValue && end.HasValue && start > end)
        {
            (start, end) = (end, start);
        }
        return (start, end);
    }

    /// <summary>
    /// 生成期间内连续年月键（yyyy-MM）
    /// </summary>
    /// <param name="rangeStart">区间起（月初，含）</param>
    /// <param name="rangeEndExclusive">区间止（不含）</param>
    /// <returns>年月列表</returns>
    private static List<string> BuildPeriodMonthKeys(DateTime rangeStart, DateTime rangeEndExclusive)
    {
        var keys = new List<string>();
        for (var cursor = rangeStart; cursor < rangeEndExclusive; cursor = cursor.AddMonths(1))
        {
            keys.Add(cursor.ToString("yyyy-MM"));
        }
        return keys;
    }

    /// <summary>
    /// 构建单行采购实绩推移
    /// </summary>
    /// <param name="key">行键（工厂+物料）</param>
    /// <param name="groupRows">同物料明细</param>
    /// <param name="periodOrder">期间年月</param>
    /// <returns>转置行</returns>
    private static TaktPurchasePriceTrendDto BuildActualTrendRow(
        PurchaseActualTrendRowKey key,
        IReadOnlyList<PurchaseActualTrendFact> groupRows,
        IReadOnlyList<string> periodOrder)
    {
        var byMonth = groupRows
            .GroupBy(r => r.PostingDate.ToString("yyyy-MM"), StringComparer.Ordinal)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var quantity = g.Sum(x => x.Quantity);
                    var amount = g.Sum(x => x.Amount);
                    var unitPrice = quantity != 0m
                        ? RoundActualUnitPrice(amount / quantity)
                        : 0m;
                    var unit = g.Select(x => x.Unit)
                        .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u)) ?? string.Empty;
                    var currency = g.Select(x => x.Currency)
                        .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty;
                    return (Quantity: quantity, Amount: amount, UnitPrice: unitPrice, Unit: unit, Currency: currency);
                },
                StringComparer.Ordinal);
        var suppliers = groupRows
            .Select(x => x.SupplierCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var periodUnitPrices = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var periodPriceSourcePeriods = new Dictionary<string, string>(StringComparer.Ordinal);
        var periodPoints = new List<TaktPurchasePriceTrendPointDto>();
        var row = new TaktPurchasePriceTrendDto
        {
            PlantCode = key.PlantCode,
            MaterialCode = key.MaterialCode,
            SupplierCode = suppliers.Count == 1 ? suppliers[0] : string.Empty,
            MaterialDescription = string.Empty,
            CurrencyCode = byMonth.Values
                .Select(v => v.Currency)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty,
            Unit = byMonth.Values
                .Select(v => v.Unit)
                .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u)) ?? string.Empty,
            Trend = "none",
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
            PeriodPoints = periodPoints,
            PeriodAdjacents = new Dictionary<string, TaktPurchasePriceTrendAdjacentDto>(StringComparer.Ordinal),
        };
        foreach (var yearMonth in periodOrder)
        {
            if (!byMonth.TryGetValue(yearMonth, out var month))
            {
                continue;
            }
            // H/S 完全抵消（净数量与净金额均为 0）：无实绩，不展示、不参与涨跌
            if (month.Quantity == 0m && month.Amount == 0m)
            {
                continue;
            }
            periodUnitPrices[yearMonth] = month.UnitPrice;
            periodPriceSourcePeriods[yearMonth] = yearMonth;
            periodPoints.Add(new TaktPurchasePriceTrendPointDto
            {
                ValidFrom = yearMonth,
                UnitPrice = month.UnitPrice,
                Quantity = month.Quantity,
                Amount = month.Amount,
                HasPurchase = true,
            });
            if (!string.IsNullOrWhiteSpace(month.Unit))
            {
                row.Unit = month.Unit;
            }
        }
        ApplyMonthlyPriceImpact(row);
        return row;
    }

    /// <summary>
    /// 回填物料描述与供应商名称
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="rows">推移行</param>
    private async Task FillActualTrendDisplayNamesAsync(string plantCode, List<TaktPurchasePriceTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }
        var materialCodes = rows
            .Where(r => string.IsNullOrWhiteSpace(r.MaterialDescription))
            .Select(r => r.MaterialCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var supplierCodes = rows
            .Select(r => r.SupplierCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var materialNames = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var supplierNames = await LoadSupplierNameLookupAsync(plantCode, supplierCodes);
        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row.MaterialDescription)
                && materialNames.TryGetValue(row.MaterialCode, out var materialDescription))
            {
                row.MaterialDescription = materialDescription;
            }
            if (supplierNames.TryGetValue(row.SupplierCode, out var supplierName))
            {
                row.SupplierName = supplierName;
            }
        }
    }

    /// <summary>
    /// 加载工厂物料描述字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialCodes">物料编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadMaterialNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = materialCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var plants = await _materialPlantRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.MaterialCode));
            foreach (var group in plants
                .Where(p => !string.IsNullOrWhiteSpace(p.MaterialCode))
                .GroupBy(p => p.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.MaterialDescription)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 加载供应商名称字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">供应商编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadSupplierNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        if (supplierCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = string.IsNullOrEmpty(plant)
                ? await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode))
                : await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && batch.Contains(x.SupplierCode));
            if (suppliers.Count == 0 && !string.IsNullOrEmpty(plant))
            {
                suppliers = await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode));
            }
            foreach (var group in suppliers
                .Where(s => !string.IsNullOrWhiteSpace(s.SupplierCode))
                .GroupBy(s => s.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.SupplierName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 相邻月金额浮动=单价差×当月数量
    /// </summary>
    /// <param name="row">转置行</param>
    private static void ApplyMonthlyPriceImpact(TaktPurchasePriceTrendDto row)
    {
        var points = row.PeriodPoints ?? new List<TaktPurchasePriceTrendPointDto>();
        row.PeriodPoints = points;
        row.PeriodAdjacents ??= new Dictionary<string, TaktPurchasePriceTrendAdjacentDto>(StringComparer.Ordinal);
        TaktPurchasePriceTrendPointDto? previous = null;
        var comparableCount = 0;
        foreach (var current in points)
        {
            if (!current.HasPurchase || current.Quantity == 0m)
            {
                continue;
            }
            if (previous == null)
            {
                previous = current;
                continue;
            }
            var unitDelta = RoundActualUnitPrice(current.UnitPrice - previous.UnitPrice);
            var impact = unitDelta == 0m
                ? 0m
                : RoundActualImpactAmount(unitDelta * current.Quantity);
            current.AdjacentTrend = impact > 0m ? "up" : impact < 0m ? "down" : "flat";
            current.AdjacentVarianceAmount = impact;
            current.AdjacentVariancePercent = unitDelta == 0m || previous.UnitPrice == 0m
                ? (unitDelta == 0m ? 0m : null)
                : Math.Round(unitDelta / previous.UnitPrice, 4, MidpointRounding.AwayFromZero);
            current.AdjacentBasePeriod = previous.ValidFrom;
            row.PeriodAdjacents[current.ValidFrom] = new TaktPurchasePriceTrendAdjacentDto
            {
                BasePeriod = previous.ValidFrom,
                ComparePeriod = current.ValidFrom,
                Trend = current.AdjacentTrend,
                VarianceAmount = impact,
                VariancePercent = current.AdjacentVariancePercent,
            };
            comparableCount++;
            previous = current;
        }
        if (comparableCount == 0)
        {
            row.Trend = "none";
            return;
        }
        var first = points.FirstOrDefault(p => p.HasPurchase && p.Quantity != 0m);
        var last = points.LastOrDefault(p => p.HasPurchase && p.Quantity != 0m);
        if (first == null || last == null || ReferenceEquals(first, last))
        {
            row.Trend = "none";
            return;
        }
        var periodUnitDelta = RoundActualUnitPrice(last.UnitPrice - first.UnitPrice);
        var periodImpact = periodUnitDelta == 0m
            ? 0m
            : RoundActualImpactAmount(periodUnitDelta * last.Quantity);
        row.BasePeriod = first.ValidFrom;
        row.ComparePeriod = last.ValidFrom;
        row.VarianceAmount = periodImpact;
        row.VariancePercent = periodUnitDelta == 0m || first.UnitPrice == 0m
            ? (periodUnitDelta == 0m ? 0m : null)
            : Math.Round(periodUnitDelta / first.UnitPrice, 4, MidpointRounding.AwayFromZero);
        row.Trend = periodImpact > 0m ? "up" : periodImpact < 0m ? "down" : "flat";
    }

    /// <summary>
    /// 按筛选后全量行汇总各月金额、涨价合计、跌价合计
    /// </summary>
    /// <param name="rows">筛选后行</param>
    /// <param name="periodOrder">期间年月</param>
    /// <returns>各月汇总</returns>
    private static List<TaktPurchasePriceTrendMonthSummaryDto> BuildActualMonthSummaries(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        var map = periodOrder.ToDictionary(
            key => key,
            _ => (Amount: 0m, Up: 0m, Down: 0m),
            StringComparer.Ordinal);
        foreach (var row in rows)
        {
            foreach (var point in row.PeriodPoints ?? new List<TaktPurchasePriceTrendPointDto>())
            {
                if (!point.HasPurchase || !map.ContainsKey(point.ValidFrom))
                {
                    continue;
                }
                var current = map[point.ValidFrom];
                current.Amount += point.Amount;
                var impact = point.AdjacentVarianceAmount ?? 0m;
                if (impact > 0m)
                {
                    current.Up += impact;
                }
                else if (impact < 0m)
                {
                    current.Down += -impact;
                }
                map[point.ValidFrom] = current;
            }
        }
        return periodOrder.Select(yearMonth =>
        {
            var month = map[yearMonth];
            return new TaktPurchasePriceTrendMonthSummaryDto
            {
                Period = yearMonth,
                Amount = RoundActualImpactAmount(month.Amount),
                UpAmount = RoundActualImpactAmount(month.Up),
                DownAmount = RoundActualImpactAmount(month.Down),
            };
        }).ToList();
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktPurchasePriceTrendDto> FilterActualTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter is "all" or "leading")
        {
            return rows.ToList();
        }
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <returns>排序后行</returns>
    private static List<TaktPurchasePriceTrendDto> OrderActualTrendRows(IReadOnlyList<TaktPurchasePriceTrendDto> rows)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        return rows
            .OrderBy(r => TrendRank(r.Trend))
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
            .ThenBy(r => r.MaterialCode, StringComparer.Ordinal)
            .ThenBy(r => r.SupplierCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    /// <param name="value">单价</param>
    /// <returns>四舍五入后单价</returns>
    private static decimal RoundActualUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 金额四舍五入至 3 位
    /// </summary>
    /// <param name="value">金额</param>
    /// <returns>四舍五入后金额</returns>
    private static decimal RoundActualImpactAmount(decimal value) =>
        Math.Round(value, 3, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 采购实绩推移事实行
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="SupplierCode">供应商编码</param>
    /// <param name="PostingDate">过账日期</param>
    /// <param name="Quantity">带符号数量</param>
    /// <param name="Amount">带符号金额</param>
    /// <param name="Unit">订单单位</param>
    /// <param name="Currency">币种</param>
    private sealed record PurchaseActualTrendFact(
        string PlantCode,
        string MaterialCode,
        string SupplierCode,
        DateTime PostingDate,
        decimal Quantity,
        decimal Amount,
        string Unit,
        string Currency);

    /// <summary>
    /// 采购实绩推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="SupplierCode">供应商编码</param>
    private sealed record PurchaseActualTrendRowKey(string PlantCode, string MaterialCode, string SupplierCode);

    /// <summary>
    /// 采购实绩推移行键比较器
    /// </summary>
    private sealed class PurchaseActualTrendRowKeyComparer : IEqualityComparer<PurchaseActualTrendRowKey>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static PurchaseActualTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(PurchaseActualTrendRowKey? x, PurchaseActualTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.MaterialCode, y.MaterialCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.SupplierCode, y.SupplierCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public int GetHashCode(PurchaseActualTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MaterialCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.SupplierCode));
    }
}
