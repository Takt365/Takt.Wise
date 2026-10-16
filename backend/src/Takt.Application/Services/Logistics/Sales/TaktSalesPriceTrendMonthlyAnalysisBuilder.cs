// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceTrendMonthlyAnalysisBuilder.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移转置分析构建器（列=ValidFrom 有效起始日，同月多笔不合并）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格推移转置分析构建器（列按 ValidFrom，不按自然月合并）
/// </summary>
public class TaktSalesPriceTrendMonthlyAnalysisBuilder : TaktServiceBase, ITaktSalesPriceTrendMonthlyAnalysisBuilder
{
    /// <summary>
    /// 物料/客户名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktSalesCondition> _salesConditionRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionItem> _salesConditionItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktCustomer> _customerRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRepository">销售价格仓储</param>
    /// <param name="salesConditionItemRepository">销售条件行仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="customerRepository">客户仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceTrendMonthlyAnalysisBuilder(
        ITaktCompanyRepository<TaktSalesCondition> salesConditionRepository,
        ITaktCompanyRepository<TaktSalesConditionItem> salesConditionItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktCustomer> customerRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionRepository = salesConditionRepository;
        _salesConditionItemRepository = salesConditionItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// 构建销售价格推移转置分析全量结果（列=ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    public async Task<TaktSalesPriceTrendAnalysisBuilt> BuildAsync(
        TaktSalesPriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var customerFilter = string.IsNullOrWhiteSpace(queryDto.CustomerCode) ? null : queryDto.CustomerCode.Trim();
        var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
        var priceTypeFilter = string.IsNullOrWhiteSpace(queryDto.PriceType) ? null : queryDto.PriceType.Trim();
        var (rangeStart, rangeEndExclusive) = ResolveSalesPriceTrendDateBounds(queryDto);
        var masterExp = BuildSalesPriceTrendMasterExpression(
            plantCode,
            customerFilter,
            materialFilter,
            priceTypeFilter,
            rangeStart,
            rangeEndExclusive);
        var masters = (await _salesConditionRepository.GetListAsync(masterExp))
            .Where(m => string.Equals(m.PlantCode?.Trim(), plantCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (masters.Count == 0)
        {
            return TaktSalesPriceTrendAnalysisBuilt.Empty();
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masters.Select(m => m.Id).ToList();
        var itemExp = BuildSalesPriceTrendItemExpression(plantCode, masterIds);
        var items = await _salesConditionItemRepository.GetListAsync(itemExp);
        if (items.Count == 0)
        {
            return TaktSalesPriceTrendAnalysisBuilt.Empty();
        }
        var sourceRows = items
            .Where(i => masterById.ContainsKey(i.SalesConditionId)
                && string.Equals(i.PlantCode?.Trim(), plantCode, StringComparison.OrdinalIgnoreCase))
            .Select(i => new SalesPriceTrendSourceRow
            {
                Master = masterById[i.SalesConditionId],
                Item = i,
            })
            .ToList();
        var periodOrder = sourceRows
            .Select(r => r.Master.ValidFrom.Date)
            .Distinct()
            .OrderBy(d => d)
            .Select(d => d.ToString("yyyy-MM-dd"))
            .ToList();
        if (periodOrder.Count > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
        {
            throw new ArgumentException($"有效起始日列不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 列");
        }
        var allRows = sourceRows
            .GroupBy(
                r => new SalesPriceTrendRowKey(
                    r.Master.PlantCode.Trim(),
                    r.Master.MaterialCode.Trim(),
                    r.Master.CustomerCode?.Trim() ?? string.Empty),
                SalesPriceTrendRowKeyComparer.Instance)
            .Select(g => BuildSalesPriceTrendRow(g.Key, g.ToList(), periodOrder))
            .ToList();
        await FillSalesPriceTrendDisplayNamesAsync(plantCode, allRows);
        var filtered = FilterSalesPriceTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderSalesPriceTrendRows(filtered);
        return new TaktSalesPriceTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = periodOrder.Count >= 2 ? periodOrder[^2] : periodOrder.FirstOrDefault(),
            ComparePeriod = periodOrder.LastOrDefault(),
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 构建销售价格推移主表筛选条件（OnlyEnabled 已无 PriceStatus，忽略）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="customerFilter">客户编码</param>
    /// <param name="materialFilter">物料编码</param>
    /// <param name="priceType">价格类型（如 PR00）</param>
    /// <param name="validFromStart">有效起始日区间起（含）</param>
    /// <param name="validFromEndExclusive">有效起始日区间止（不含）</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktSalesCondition, bool>> BuildSalesPriceTrendMasterExpression(
        string plantCode,
        string? customerFilter,
        string? materialFilter,
        string? priceType,
        DateTime validFromStart,
        DateTime validFromEndExclusive)
    {
        var exp = Expressionable.Create<TaktSalesCondition>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ValidFrom >= validFromStart
            && x.ValidFrom < validFromEndExclusive);
        if (!string.IsNullOrWhiteSpace(customerFilter))
        {
            var customer = customerFilter.Trim();
            exp = exp.And(x => x.CustomerCode == customer);
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
    /// 构建销售价格推移明细筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="masterIds">主表 ID 列表</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktSalesConditionItem, bool>> BuildSalesPriceTrendItemExpression(
        string plantCode,
        IReadOnlyList<long> masterIds)
    {
        var exp = Expressionable.Create<TaktSalesConditionItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && masterIds.Contains(x.SalesConditionId)
            && x.IsObsolete == 0);
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析销售价格推移查询的有效起始日闭开区间（查询栏年月 → ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起（含）与止（不含）</returns>
    private static (DateTime RangeStart, DateTime RangeEndExclusive) ResolveSalesPriceTrendDateBounds(
        TaktSalesPriceTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizeSalesConditionPeriodBounds(
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
    private static (DateTime? Start, DateTime? End) NormalizeSalesConditionPeriodBounds(
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
    /// 构建单行销售价格推移（列=该键下各 ValidFrom；同日多条取均价）
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键明细</param>
    /// <param name="periodOrder">有效起始日列顺序 yyyy-MM-dd</param>
    /// <returns>转置行</returns>
    private static TaktSalesPriceTrendDto BuildSalesPriceTrendRow(
        SalesPriceTrendRowKey key,
        IReadOnlyList<SalesPriceTrendSourceRow> groupRows,
        IReadOnlyList<string> periodOrder)
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
                        UnitPrice: RoundSalesConditionUnitPrice(
                            TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(avgRaw, perUnit)),
                        Unit: unit,
                        Currency: currency);
                },
                StringComparer.Ordinal);
        var periodUnitPrices = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var periodPriceSourcePeriods = new Dictionary<string, string>(StringComparer.Ordinal);
        var row = new TaktSalesPriceTrendDto
        {
            PlantCode = key.PlantCode,
            MaterialCode = key.MaterialCode,
            CustomerCode = key.CustomerCode,
            CurrencyCode = priceByDate.Values
                .Select(v => v.Currency)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty,
            Unit = priceByDate.Values
                .Select(v => v.Unit)
                .FirstOrDefault(u => !string.IsNullOrWhiteSpace(u)) ?? string.Empty,
            Trend = "none",
            PeriodUnitPrices = periodUnitPrices,
            PeriodPriceSourcePeriods = periodPriceSourcePeriods,
        };
        foreach (var period in periodOrder)
        {
            if (!priceByDate.TryGetValue(period, out var point))
            {
                continue;
            }
            periodUnitPrices[period] = point.UnitPrice;
            periodPriceSourcePeriods[period] = period;
            if (!string.IsNullOrWhiteSpace(point.Unit))
            {
                row.Unit = point.Unit;
            }
        }
        ApplySalesConditionValidFromTrend(row);
        return row;
    }

    /// <summary>
    /// 按该行最近两次有效起始日计算涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    private static void ApplySalesConditionValidFromTrend(TaktSalesPriceTrendDto row)
    {
        var dates = row.PeriodUnitPrices.Keys
            .OrderBy(k => k, StringComparer.Ordinal)
            .ToList();
        if (dates.Count < 2)
        {
            row.Trend = "none";
            return;
        }
        var comparePeriod = dates[^1];
        var basePeriod = dates[^2];
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        if (!row.PeriodUnitPrices.TryGetValue(basePeriod, out var basePrice)
            || !row.PeriodUnitPrices.TryGetValue(comparePeriod, out var comparePrice))
        {
            row.Trend = "none";
            return;
        }
        row.VarianceAmount = RoundSalesConditionUnitPrice(comparePrice - basePrice);
        if (basePrice != 0m)
        {
            row.VariancePercent = Math.Round(
                row.VarianceAmount.Value / basePrice,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (comparePrice > basePrice)
        {
            row.Trend = "up";
        }
        else if (comparePrice < basePrice)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码：空/all/leading=不按涨跌码过滤；up/down/flat/none/changed</param>
    /// <returns>筛选后行</returns>
    private static List<TaktSalesPriceTrendDto> FilterSalesPriceTrendRows(
        IReadOnlyList<TaktSalesPriceTrendDto> rows,
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
    private static List<TaktSalesPriceTrendDto> OrderSalesPriceTrendRows(
        IReadOnlyList<TaktSalesPriceTrendDto> rows)
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
            .ThenBy(r => r.CustomerCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 回填物料描述 / 客户名称
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="rows">推移行</param>
    private async Task FillSalesPriceTrendDisplayNamesAsync(
        string plantCode,
        IReadOnlyList<TaktSalesPriceTrendDto> rows)
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
        var customerCodes = rows
            .Select(r => r.CustomerCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var materialNames = await LoadMaterialNameLookupAsync(plantCode, materialCodes);
        var customerNames = await LoadCustomerNameLookupAsync(plantCode, customerCodes);
        foreach (var row in rows)
        {
            if (materialNames.TryGetValue(row.MaterialCode, out var materialDescription))
            {
                row.MaterialDescription = materialDescription;
            }
            if (customerNames.TryGetValue(row.CustomerCode, out var customerName))
            {
                row.CustomerName = customerName;
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
    /// 加载工厂客户名称字典
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="customerCodes">客户编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadCustomerNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> customerCodes)
    {
        if (customerCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var codes = customerCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var customers = await _customerRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && batch.Contains(x.CustomerCode));
            foreach (var group in customers
                .Where(c => !string.IsNullOrWhiteSpace(c.CustomerCode))
                .GroupBy(c => c.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.CustomerName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }

    /// <summary>
    /// 单价四舍五入至 5 位
    /// </summary>
    /// <param name="value">单价</param>
    /// <returns>四舍五入后单价</returns>
    private static decimal RoundSalesConditionUnitPrice(decimal value) =>
        Math.Round(value, 5, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 销售价格推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="MaterialCode">物料编码</param>
    /// <param name="CustomerCode">客户编码</param>
    private sealed record SalesPriceTrendRowKey(string PlantCode, string MaterialCode, string CustomerCode);

    /// <summary>
    /// 销售价格推移行键比较器
    /// </summary>
    private sealed class SalesPriceTrendRowKeyComparer : IEqualityComparer<SalesPriceTrendRowKey>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static SalesPriceTrendRowKeyComparer Instance { get; } = new();

        /// <summary>
        /// 判断两行键是否相等（工厂/物料/客户，忽略大小写）
        /// </summary>
        /// <param name="x">左值</param>
        /// <param name="y">右值</param>
        /// <returns>是否相等</returns>
        public bool Equals(SalesPriceTrendRowKey? x, SalesPriceTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.MaterialCode, y.MaterialCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.CustomerCode, y.CustomerCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算行键哈希（工厂/物料/客户大写）
        /// </summary>
        /// <param name="obj">行键</param>
        /// <returns>哈希码</returns>
        public int GetHashCode(SalesPriceTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.MaterialCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.CustomerCode));
    }

    /// <summary>
    /// 销售价格推移源行（主表 + 明细）
    /// </summary>
    private sealed class SalesPriceTrendSourceRow
    {
        /// <summary>
        /// 销售价格主表
        /// </summary>
        public TaktSalesCondition Master { get; init; } = null!;

        /// <summary>
        /// 销售条件行
        /// </summary>
        public TaktSalesConditionItem Item { get; init; } = null!;
    }
}
