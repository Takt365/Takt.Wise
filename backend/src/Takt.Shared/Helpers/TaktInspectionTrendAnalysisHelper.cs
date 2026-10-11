// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktInspectionTrendAnalysisHelper.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量检验月推移分析（不良率环比、期间列、涨跌筛选）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 检验月推移单月聚合
/// </summary>
public sealed class TaktInspectionTrendMonthAggregate
{
    /// <summary>
    /// 检验单数
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// 抽样数量合计
    /// </summary>
    public int SampleQty { get; set; }

    /// <summary>
    /// 合格数量合计
    /// </summary>
    public int QualifiedQty { get; set; }

    /// <summary>
    /// 不合格数量合计
    /// </summary>
    public int UnqualifiedQty { get; set; }

    /// <summary>
    /// 不良率（0~1；抽样为 0 时为 null）
    /// </summary>
    public decimal? DefectRate => SampleQty > 0
        ? Math.Round((decimal)UnqualifiedQty / SampleQty, 4, MidpointRounding.AwayFromZero)
        : null;
}

/// <summary>
/// 质量检验月推移分析辅助
/// </summary>
public static class TaktInspectionTrendAnalysisHelper
{
    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    public static (DateTime? Start, DateTime? End) NormalizePeriodBounds(
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
    /// 解析分析日期区间与期间列（默认 2016-01 至当前月）
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>区间起止与期间列顺序</returns>
    /// <exception cref="ArgumentException">区间非法或超出月数上限</exception>
    public static (DateTime RangeStart, DateTime RangeEnd, List<string> PeriodOrder) ResolveTrendRange(
        DateTime? periodDateStart,
        DateTime? periodDateEnd)
    {
        var (periodStart, periodEnd) = NormalizePeriodBounds(periodDateStart, periodDateEnd);
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
            var rangeStart = startMonth;
            var rangeEnd = endMonth.AddMonths(1).AddDays(-1);
            return (rangeStart, rangeEnd, BuildConsecutivePeriodOrder(startMonth, endMonth));
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        var start = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
        var endMonthFirst = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
        return (resolvedStart, resolvedEnd, BuildConsecutivePeriodOrder(start, endMonthFirst));
    }

    /// <summary>
    /// 构建连续 yyyy-MM 期间列
    /// </summary>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>期间列顺序</returns>
    public static List<string> BuildConsecutivePeriodOrder(DateTime periodStart, DateTime periodEnd)
    {
        var order = new List<string>();
        for (var cursor = periodStart; cursor <= periodEnd; cursor = cursor.AddMonths(1))
        {
            order.Add(cursor.ToString("yyyy-MM"));
        }
        return order;
    }

    /// <summary>
    /// 解析关注期间；缺省取期间末月
    /// </summary>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>关注期间或 null</returns>
    public static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 检验日期转 yyyy-MM 期间键
    /// </summary>
    /// <param name="inspectionDate">检验日期</param>
    /// <returns>期间键或 null</returns>
    public static string? ToPeriodKey(DateTime? inspectionDate)
    {
        if (!inspectionDate.HasValue)
        {
            return null;
        }
        return inspectionDate.Value.ToString("yyyy-MM");
    }

    /// <summary>
    /// 按关注月对不良率字典计算环比涨跌
    /// </summary>
    /// <param name="periodDefectRates">各月不良率</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <param name="basePeriod">输出基准月</param>
    /// <param name="comparePeriod">输出对比月</param>
    /// <param name="varianceAmount">输出环比差额（比率差）</param>
    /// <param name="variancePercent">输出环比变动率（小数比率）</param>
    /// <returns>涨跌码 none/up/down/flat</returns>
    public static string ApplyFocusTrend(
        IReadOnlyDictionary<string, decimal?> periodDefectRates,
        string? focusPeriod,
        out string? basePeriod,
        out string? comparePeriod,
        out decimal? varianceAmount,
        out decimal? variancePercent)
    {
        basePeriod = null;
        comparePeriod = null;
        varianceAmount = null;
        variancePercent = null;
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return "none";
        }
        comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return "none";
        }
        basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        if (!periodDefectRates.TryGetValue(basePeriod, out var baseRate)
            || !periodDefectRates.TryGetValue(comparePeriod, out var compareRate)
            || !baseRate.HasValue
            || !compareRate.HasValue)
        {
            return "none";
        }
        varianceAmount = Math.Round(compareRate.Value - baseRate.Value, 4, MidpointRounding.AwayFromZero);
        if (baseRate.Value != 0m)
        {
            variancePercent = Math.Round(
                varianceAmount.Value / baseRate.Value,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareRate.Value > baseRate.Value)
        {
            return "up";
        }
        if (compareRate.Value < baseRate.Value)
        {
            return "down";
        }
        return "flat";
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <param name="trendSelector">取行涨跌码</param>
    /// <typeparam name="T">行类型</typeparam>
    /// <returns>筛选后行</returns>
    public static List<T> FilterTrendRows<T>(
        IReadOnlyList<T> rows,
        string? trendFilter,
        Func<T, string?> trendSelector)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r =>
            {
                var trend = trendSelector(r);
                return trend is "up" or "down";
            }).ToList();
        }
        return rows.Where(r => string.Equals(trendSelector(r), filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <param name="trendSelector">取行涨跌码</param>
    /// <param name="varianceSelector">取行环比差额</param>
    /// <param name="primaryKeySelector">主排序键</param>
    /// <typeparam name="T">行类型</typeparam>
    /// <returns>排序后行</returns>
    public static List<T> OrderTrendRows<T>(
        IReadOnlyList<T> rows,
        Func<T, string?> trendSelector,
        Func<T, decimal?> varianceSelector,
        Func<T, string> primaryKeySelector)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        return rows
            .OrderBy(r => TrendRank(trendSelector(r)))
            .ThenByDescending(r => Math.Abs(varianceSelector(r) ?? 0m))
            .ThenBy(r => primaryKeySelector(r), StringComparer.Ordinal)
            .ToList();
    }
}
