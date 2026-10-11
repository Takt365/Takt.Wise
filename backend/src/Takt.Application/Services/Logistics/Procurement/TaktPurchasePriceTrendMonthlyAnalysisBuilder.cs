// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendMonthlyAnalysisBuilder.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移转置分析构建器（列=各行第 n 笔 ValidFrom；相邻逐比；期间涨跌=首笔对末笔）
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
/// 采购价格推移转置分析构建器（列=各行第 n 笔 ValidFrom，不对齐全表日期；相邻逐比；期间涨跌=首笔对末笔）
/// </summary>
public class TaktPurchasePriceTrendMonthlyAnalysisBuilder : TaktServiceBase, ITaktPurchasePriceTrendMonthlyAnalysisBuilder
{
    /// <summary>
    /// 物料/供应商名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecord> _purchaseInfoRecordRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordItem> _purchaseInfoRecordItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;

    public TaktPurchasePriceTrendMonthlyAnalysisBuilder(
        ITaktCompanyRepository<TaktPurchaseInfoRecord> purchaseInfoRecordRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecordItem> purchaseInfoRecordItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoRecordRepository = purchaseInfoRecordRepository;
        _purchaseInfoRecordItemRepository = purchaseInfoRecordItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _supplierRepository = supplierRepository;
    }

    /// <summary>
    /// 构建采购价格推移转置分析全量结果（列=第 n 笔）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    public async Task<TaktPurchasePriceTrendAnalysisBuilt> BuildAsync(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var supplierFilter = string.IsNullOrWhiteSpace(queryDto.SupplierCode) ? null : queryDto.SupplierCode.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var priceTypeFilter = string.IsNullOrWhiteSpace(queryDto.PriceType) ? null : queryDto.PriceType.Trim();
        var (rangeStart, rangeEndExclusive) = ResolvePurchasePriceTrendDateBounds(queryDto);
        var masterExp = BuildPurchasePriceTrendMasterExpression(
            plantCode,
            supplierFilter,
            materialFilter,
            priceTypeFilter,
            rangeStart,
            rangeEndExclusive);
        var masters = await _purchaseInfoRecordRepository.GetListAsync(masterExp);
        if (masters.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masters.Select(m => m.Id).ToList();
        var itemExp = BuildPurchasePriceTrendItemExpression(masterIds);
        var items = await _purchaseInfoRecordItemRepository.GetListAsync(itemExp);
        if (items.Count == 0)
        {
            return TaktPurchasePriceTrendAnalysisBuilt.Empty();
        }
        var sourceRows = items
            .Where(i => masterById.ContainsKey(i.PurchaseInfoRecordId))
            .Select(i => new PurchasePriceTrendSourceRow
            {
                Master = masterById[i.PurchaseInfoRecordId],
                Item = i,
            })
            .ToList();
        var allRows = sourceRows
            .GroupBy(
                r => new PurchasePriceTrendRowKey(
                    r.Master.PlantCode.Trim(),
                    r.Master.MaterialCode.Trim(),
                    r.Master.SupplierCode.Trim()),
                PurchasePriceTrendRowKeyComparer.Instance)
            .Select(g => BuildPurchasePriceTrendRow(g.Key, g.ToList()))
            .ToList();
        await FillPurchasePriceTrendDisplayNamesAsync(plantCode, allRows);
        var maxPoints = allRows.Count == 0 ? 0 : allRows.Max(r => r.PeriodPoints.Count);
        if (maxPoints > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
        {
            throw new ArgumentException($"单行有效起始日不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 笔");
        }
        var periodOrder = Enumerable.Range(1, maxPoints).Select(i => i.ToString()).ToList();
        var filtered = FilterPurchasePriceTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderPurchasePriceTrendRows(filtered);
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
        };
    }

    /// <summary>
    /// 构建采购价格推移主表筛选条件（OnlyEnabled 已无 PriceStatus，忽略）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="supplierFilter">供应商编码包含</param>
    /// <param name="materialFilter">物料编码包含</param>
    /// <param name="priceType">价格类型（如 PB00）</param>
    /// <param name="validFromStart">有效起始日区间起（含）</param>
    /// <param name="validFromEndExclusive">有效起始日区间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktPurchaseInfoRecord, bool>> BuildPurchasePriceTrendMasterExpression(
        string plantCode,
        string? supplierFilter,
        string? materialFilter,
        string? priceType,
        DateTime validFromStart,
        DateTime validFromEndExclusive)
    {
        var exp = Expressionable.Create<TaktPurchaseInfoRecord>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ValidFrom >= validFromStart
            && x.ValidFrom < validFromEndExclusive);
        if (!string.IsNullOrWhiteSpace(supplierFilter))
        {
            var supplier = supplierFilter.Trim();
            exp = exp.And(x => x.SupplierCode == supplier);
        }
        if (!string.IsNullOrWhiteSpace(materialFilter))
        {
            var material = materialFilter.Trim();
            exp = exp.And(x => x.MaterialCode == material);
        }
        if (!string.IsNullOrWhiteSpace(priceType))
        {
            exp = exp.And(x => x.PriceType == priceType);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 构建采购价格推移明细筛选条件（跳过作废行）
    /// </summary>
    /// <param name="masterIds">主表 ID 列表</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktPurchaseInfoRecordItem, bool>> BuildPurchasePriceTrendItemExpression(
        IReadOnlyList<long> masterIds)
    {
        var exp = Expressionable.Create<TaktPurchaseInfoRecordItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && masterIds.Contains(x.PurchaseInfoRecordId)
            && x.IsObsolete == 0);
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析采购价格推移查询的有效起始日闭开区间（查询栏年月 → ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起（含）与止（不含）</returns>
    private static (DateTime RangeStart, DateTime RangeEndExclusive) ResolvePurchasePriceTrendDateBounds(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizePurchaseInfoRecordPeriodBounds(
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
    private static (DateTime? Start, DateTime? End) NormalizePurchaseInfoRecordPeriodBounds(
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
    /// 构建单行采购价格推移（该行 ValidFrom 自早到晚成第 n 笔；同日多条取均价；相邻逐比）
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键明细</param>
    /// <returns>转置行</returns>
    private static TaktPurchasePriceTrendDto BuildPurchasePriceTrendRow(
        PurchasePriceTrendRowKey key,
        IReadOnlyList<PurchasePriceTrendSourceRow> groupRows)
    {
        var priceByDate = groupRows
            .GroupBy(r => r.Master.ValidFrom.Date)
            .ToDictionary(
                g => g.Key.ToString("yyyy-MM-dd"),
                g =>
                {
                    var avgRaw = g.Average(x => x.Item.Price);
                    var perUnit = g.Select(x => x.Item.PriceUnit).FirstOrDefault();
                    var unit = g.Select(x => x.Item.UnitOfMeasure)
                        .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u)) ?? string.Empty;
                    var currency = g.Select(x => x.Item.ConditionCurrencyCode?.Trim())
                        .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty;
                    return (
                        UnitPrice: RoundPurchasePriceUnitPrice(
                            TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(avgRaw, perUnit)),
                        Unit: unit,
                        Currency: currency);
                },
                StringComparer.Ordinal);
        var periodUnitPrices = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var periodPriceSourcePeriods = new Dictionary<string, string>(StringComparer.Ordinal);
        var periodPoints = new List<TaktPurchasePriceTrendPointDto>();
        var row = new TaktPurchasePriceTrendDto
        {
            PlantCode = key.PlantCode,
            MaterialCode = key.MaterialCode,
            SupplierCode = key.SupplierCode,
            CurrencyCode = priceByDate.Values
                .Select(v => v.Currency)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty,
            Unit = priceByDate.Values
                .Select(v => v.Unit)
                .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u)) ?? string.Empty,
            Trend = "none",
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
            PeriodPoints = periodPoints,
            PeriodAdjacents = new Dictionary<string, TaktPurchasePriceTrendAdjacentDto>(StringComparer.Ordinal),
        };
        foreach (var pair in priceByDate.OrderBy(kv => kv.Key, StringComparer.Ordinal))
        {
            periodUnitPrices[pair.Key] = pair.Value.UnitPrice;
            periodPriceSourcePeriods[pair.Key] = pair.Key;
            periodPoints.Add(new TaktPurchasePriceTrendPointDto
            {
                ValidFrom = pair.Key,
                UnitPrice = pair.Value.UnitPrice,
            });
            if (!string.IsNullOrWhiteSpace(pair.Value.Unit))
            {
                row.Unit = pair.Value.Unit;
            }
        }
        ApplyPurchaseInfoRecordValidFromTrend(row);
        return row;
    }

    /// <summary>
    /// 回填物料描述与供应商名称
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="rows">推移行</param>
    private async Task FillPurchasePriceTrendDisplayNamesAsync(
        string plantCode,
        List<TaktPurchasePriceTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }
        var materialCodes = rows
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
            if (materialNames.TryGetValue(row.MaterialCode, out var materialDescription))
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
    /// 加载指定工厂下存在于供应商主数据的编码集合（用于级联交叉过滤）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">候选供应商编码</param>
    /// <returns>该工厂主数据中存在的编码；主数据无该厂记录时返回空集（调用方回退为本表全集）</returns>
    private async Task<HashSet<string>> LoadPlantScopedSupplierCodesAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(plantCode) || supplierCodes.Count == 0)
        {
            return result;
        }
        var plant = plantCode.Trim();
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = await _supplierRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && batch.Contains(x.SupplierCode));
            foreach (var code in suppliers
                .Select(s => s.SupplierCode?.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))!)
            {
                result.Add(code!);
            }
        }
        return result;
    }

    /// <summary>
    /// 加载供应商名称字典（优先同工厂主数据）
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
            // 同厂无命中时回退公司级编码（价目可能先于主数据 PlantCode 对齐）
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
    /// 按该行第 n 笔有效起始日自早到晚：相邻逐比；期间涨跌=第 1 笔对末笔
    /// </summary>
    /// <param name="row">转置行</param>
    private static void ApplyPurchaseInfoRecordValidFromTrend(TaktPurchasePriceTrendDto row)
    {
        var points = row.PeriodPoints ?? new List<TaktPurchasePriceTrendPointDto>();
        row.PeriodPoints = points;
        row.PeriodAdjacents ??= new Dictionary<string, TaktPurchasePriceTrendAdjacentDto>(StringComparer.Ordinal);
        if (points.Count < 2)
        {
            row.Trend = "none";
            return;
        }
        for (var i = 1; i < points.Count; i++)
        {
            var previous = points[i - 1];
            var current = points[i];
            var adjacent = ComputePurchaseInfoRecordVariance(previous.UnitPrice, current.UnitPrice);
            current.AdjacentTrend = adjacent.Trend;
            current.AdjacentVarianceAmount = adjacent.Amount;
            current.AdjacentVariancePercent = adjacent.Percent;
            current.AdjacentBasePeriod = previous.ValidFrom;
            row.PeriodAdjacents[current.ValidFrom] = new TaktPurchasePriceTrendAdjacentDto
            {
                BasePeriod = previous.ValidFrom,
                ComparePeriod = current.ValidFrom,
                Trend = adjacent.Trend,
                VarianceAmount = adjacent.Amount,
                VariancePercent = adjacent.Percent,
            };
        }
        var first = points[0];
        var last = points[^1];
        var overall = ComputePurchaseInfoRecordVariance(first.UnitPrice, last.UnitPrice);
        row.BasePeriod = first.ValidFrom;
        row.ComparePeriod = last.ValidFrom;
        row.VarianceAmount = overall.Amount;
        row.VariancePercent = overall.Percent;
        row.Trend = overall.Trend;
    }

    /// <summary>
    /// 计算对比单价相对基准单价的差额、变动率与涨跌
    /// </summary>
    /// <param name="basePrice">基准单价</param>
    /// <param name="comparePrice">对比单价</param>
    /// <returns>差额、变动率、涨跌码</returns>
    private static (decimal Amount, decimal? Percent, string Trend) ComputePurchaseInfoRecordVariance(
        decimal basePrice,
        decimal comparePrice)
    {
        var amount = RoundPurchasePriceUnitPrice(comparePrice - basePrice);
        decimal? percent = null;
        if (basePrice != 0m)
        {
            percent = Math.Round(amount / basePrice, 4, MidpointRounding.AwayFromZero);
        }
        var trend = comparePrice > basePrice
            ? "up"
            : comparePrice < basePrice
                ? "down"
                : "flat";
        return (amount, percent, trend);
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码：空/all/leading=不按涨跌码过滤；up/down/flat/none/changed</param>
    /// <returns>筛选后行</returns>
    private static List<TaktPurchasePriceTrendDto> FilterPurchasePriceTrendRows(
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
    private static List<TaktPurchasePriceTrendDto> OrderPurchasePriceTrendRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
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
    private static decimal RoundPurchasePriceUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 采购价格推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="SupplierCode">供应商编码</param>
    private sealed record PurchasePriceTrendRowKey(string PlantCode, string MaterialCode, string SupplierCode);

    /// <summary>
    /// 采购价格推移行键比较器
    /// </summary>
    private sealed class PurchasePriceTrendRowKeyComparer : IEqualityComparer<PurchasePriceTrendRowKey>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static PurchasePriceTrendRowKeyComparer Instance { get; } = new();

        /// <summary>
        /// 判断两行键是否相等（工厂/物料/供应商，忽略大小写）
        /// </summary>
        /// <param name="x">左值</param>
        /// <param name="y">右值</param>
        /// <returns>是否相等</returns>
        public bool Equals(PurchasePriceTrendRowKey? x, PurchasePriceTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.MaterialCode, y.MaterialCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.SupplierCode, y.SupplierCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算行键哈希（工厂/物料/供应商，忽略大小写）
        /// </summary>
        /// <param name="obj">行键</param>
        /// <returns>哈希码</returns>
        public int GetHashCode(PurchasePriceTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MaterialCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.SupplierCode));
    }

    /// <summary>
    /// 采购价格推移源行（主表 + 明细）
    /// </summary>
    private sealed class PurchasePriceTrendSourceRow
    {
        /// <summary>
        /// 采购价格主表
        /// </summary>
        public TaktPurchaseInfoRecord Master { get; init; } = null!;

        /// <summary>
        /// 采购信息记录行
        /// </summary>
        public TaktPurchaseInfoRecordItem Item { get; init; } = null!;
    }
}
