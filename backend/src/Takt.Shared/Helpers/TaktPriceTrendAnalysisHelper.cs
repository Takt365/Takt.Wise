// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPriceTrendAnalysisHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料价格月度波动分析（按主表生效区间解析每月有效单价；支持缺月回填最近价与价格日期）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 价格趋势分析单条有效价记录（主表生效区间 + 明细单价）
/// </summary>
public sealed class TaktPriceTrendEntry
{
    /// <summary>
    /// 生效开始日期（价格日期）
    /// </summary>
    public DateTime EffectiveStartDate { get; init; }

    /// <summary>
    /// 生效结束日期（空表示长期有效）
    /// </summary>
    public DateTime? EffectiveEndDate { get; init; }

    /// <summary>
    /// 原始单价（按价格单位 PerUnit 计价）
    /// </summary>
    public decimal RawPrice { get; init; }

    /// <summary>
    /// 价格单位（1/100/1000 等）
    /// </summary>
    public int PerUnit { get; init; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string Unit { get; init; } = string.Empty;

    /// <summary>
    /// 关联方编码（供应商/客户等，可选）
    /// </summary>
    public string? ReferenceCode { get; init; }
}

/// <summary>
/// 月度价格趋势点
/// </summary>
public sealed class TaktPriceTrendMonthPoint
{
    /// <summary>
    /// 月份 yyyy-MM
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 当月是否存在可展示单价（含缺月回填）
    /// </summary>
    public bool HasPrice { get; set; }

    /// <summary>
    /// 折算单价（RawPrice / PerUnit；PerUnit 为 0 时等于 RawPrice）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 原始单价
    /// </summary>
    public decimal RawPrice { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public int PerUnit { get; set; }

    /// <summary>
    /// 计量单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 关联方编码（当月选中记录；多源时为最新生效记录）
    /// </summary>
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 环比涨跌幅（%）；相对上一有效月份
    /// </summary>
    public decimal? ChangePercent { get; set; }

    /// <summary>
    /// 当月参与汇总的有效价记录数（回填月为 0）
    /// </summary>
    public int SourceRecordCount { get; set; }

    /// <summary>
    /// 单价来源月 yyyy-MM（当月有价=本月；缺月回填=最近有价月）
    /// </summary>
    public string? SourceYearMonth { get; set; }

    /// <summary>
    /// 最近价格日期 yyyy-MM-dd（选中记录 ValidFrom / EffectiveStartDate；回填悬停 * 用）
    /// </summary>
    public string? SourcePriceDate { get; set; }

    /// <summary>
    /// 是否缺月回填（来源月 ≠ 展示月）
    /// </summary>
    public bool IsCarriedForward =>
        HasPrice
        && !string.IsNullOrWhiteSpace(SourceYearMonth)
        && !string.Equals(SourceYearMonth, YearMonth, StringComparison.Ordinal);
}

/// <summary>
/// 物料价格月度波动分析辅助
/// </summary>
public static class TaktPriceTrendAnalysisHelper
{
    /// <summary>
    /// 默认分析起始年（2016-01-01）
    /// </summary>
    public const int DefaultStartYear = 2016;

    /// <summary>
    /// 最大分析月数（20 年）。价格点列、缺月回填等内部扫描用；页面可选区间见 AnalysisPeriodMaxMonths
    /// </summary>
    public const int MaxTrendMonths = 240;

    /// <summary>
    /// 推移分析页面可选区间（含起止），与前端 TAKT_ANALYSIS_PERIOD_MAX_MONTHS 一致
    /// </summary>
    public const int AnalysisPeriodMaxMonths = 12;

    /// <summary>
    /// 缺月回填时向前扫描历史月的上限（与展示列无关）
    /// </summary>
    private const int CarryForwardSeedMaxMonths = 36;

    /// <summary>
    /// 解析分析日期区间（默认 2016-01-01 至当前月末）
    /// </summary>
    /// <param name="dateStart">开始日期</param>
    /// <param name="dateEnd">结束日期</param>
    /// <returns>闭区间起止日期</returns>
    /// <exception cref="ArgumentException">区间非法或超出月数上限</exception>
    public static (DateTime Start, DateTime End) ResolveTrendDateRange(DateTime? dateStart, DateTime? dateEnd)
    {
        var start = dateStart?.Date ?? new DateTime(DefaultStartYear, 1, 1);
        var endAnchor = dateEnd?.Date ?? DateTime.Now.Date;
        var end = new DateTime(endAnchor.Year, endAnchor.Month, 1).AddMonths(1).AddDays(-1);
        if (end < start)
        {
            throw new ArgumentException("结束日期不能早于开始日期");
        }
        var monthCount = ((end.Year - start.Year) * 12) + end.Month - start.Month + 1;
        if (monthCount > MaxTrendMonths)
        {
            throw new ArgumentException($"分析区间不得超过 {MaxTrendMonths} 个月");
        }
        return (start, end);
    }

    /// <summary>
    /// 折算为每 1 计量单位的单价
    /// </summary>
    /// <param name="rawPrice">原始单价</param>
    /// <param name="perUnit">价格单位</param>
    /// <returns>折算单价</returns>
    public static decimal NormalizeUnitPrice(decimal rawPrice, int perUnit)
    {
        if (perUnit <= 0)
        {
            return rawPrice;
        }
        return Math.Round(rawPrice / perUnit, 5, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 构建月度价格波动序列（每月取生效开始日最新的一条；同日起点多条取均价）。
    /// carryForwardMissingMonths=true 时：展示期内缺月沿用最近有价月；并向前扫描历史月作回填种子。
    /// </summary>
    /// <param name="entries">有效价记录</param>
    /// <param name="rangeStart">区间开始</param>
    /// <param name="rangeEnd">区间结束</param>
    /// <param name="carryForwardMissingMonths">缺月是否回填最近价</param>
    /// <returns>按月排序的趋势点（仅含展示区间月份）</returns>
    public static List<TaktPriceTrendMonthPoint> BuildMonthlyTrendPoints(
        IReadOnlyList<TaktPriceTrendEntry> entries,
        DateTime rangeStart,
        DateTime rangeEnd,
        bool carryForwardMissingMonths = false)
    {
        ArgumentNullException.ThrowIfNull(entries);
        var displayStart = new DateTime(rangeStart.Year, rangeStart.Month, 1);
        var endMonth = new DateTime(rangeEnd.Year, rangeEnd.Month, 1);
        decimal? lastUnitPrice = null;
        decimal lastRawPrice = 0m;
        var lastPerUnit = 0;
        var lastUnit = string.Empty;
        string? lastReference = null;
        string? lastSourceYm = null;
        string? lastSourcePriceDate = null;

        if (carryForwardMissingMonths && entries.Count > 0)
        {
            SeedCarryForwardBeforeRange(
                entries,
                displayStart,
                ref lastUnitPrice,
                ref lastRawPrice,
                ref lastPerUnit,
                ref lastUnit,
                ref lastReference,
                ref lastSourceYm,
                ref lastSourcePriceDate);
        }

        var points = new List<TaktPriceTrendMonthPoint>();
        var cursor = displayStart;
        while (cursor <= endMonth)
        {
            var yearMonth = cursor.ToString("yyyy-MM");
            var monthStart = cursor;
            var monthEnd = cursor.AddMonths(1).AddTicks(-1);
            var native = ResolveNativeMonthPrice(entries, monthStart, monthEnd);
            if (native != null)
            {
                decimal? changePercent = null;
                if (lastUnitPrice.HasValue)
                {
                    changePercent = TaktYoYStatHelper.CalculateYoYPercent(native.Value.UnitPrice, lastUnitPrice.Value);
                }
                lastUnitPrice = native.Value.UnitPrice;
                lastRawPrice = native.Value.RawPrice;
                lastPerUnit = native.Value.PerUnit;
                lastUnit = native.Value.Unit;
                lastReference = native.Value.ReferenceCode;
                lastSourceYm = yearMonth;
                lastSourcePriceDate = native.Value.SourcePriceDate;
                points.Add(new TaktPriceTrendMonthPoint
                {
                    YearMonth = yearMonth,
                    HasPrice = true,
                    UnitPrice = native.Value.UnitPrice,
                    RawPrice = native.Value.RawPrice,
                    PerUnit = native.Value.PerUnit,
                    Unit = native.Value.Unit,
                    ReferenceCode = native.Value.ReferenceCode,
                    ChangePercent = changePercent,
                    SourceRecordCount = native.Value.SourceRecordCount,
                    SourceYearMonth = yearMonth,
                    SourcePriceDate = native.Value.SourcePriceDate,
                });
            }
            else if (carryForwardMissingMonths
                && lastUnitPrice.HasValue
                && !string.IsNullOrWhiteSpace(lastSourceYm))
            {
                points.Add(new TaktPriceTrendMonthPoint
                {
                    YearMonth = yearMonth,
                    HasPrice = true,
                    UnitPrice = lastUnitPrice.Value,
                    RawPrice = lastRawPrice,
                    PerUnit = lastPerUnit,
                    Unit = lastUnit,
                    ReferenceCode = lastReference,
                    ChangePercent = null,
                    SourceRecordCount = 0,
                    SourceYearMonth = lastSourceYm,
                    SourcePriceDate = lastSourcePriceDate,
                });
            }
            else
            {
                points.Add(new TaktPriceTrendMonthPoint
                {
                    YearMonth = yearMonth,
                    HasPrice = false,
                });
            }
            cursor = cursor.AddMonths(1);
        }
        return points;
    }

    /// <summary>
    /// 缺月回填时写入 PeriodPriceSourcePeriods 的展示值：回填用最近价格日期，当月有价用展示月。
    /// </summary>
    /// <param name="point">趋势点</param>
    /// <returns>来源标识（yyyy-MM-dd 或 yyyy-MM）</returns>
    public static string ResolvePeriodPriceSourceLabel(TaktPriceTrendMonthPoint point)
    {
        ArgumentNullException.ThrowIfNull(point);
        if (!point.HasPrice)
        {
            return point.YearMonth;
        }
        if (point.IsCarriedForward)
        {
            if (!string.IsNullOrWhiteSpace(point.SourcePriceDate))
            {
                return point.SourcePriceDate!;
            }
            return string.IsNullOrWhiteSpace(point.SourceYearMonth) ? point.YearMonth : point.SourceYearMonth!;
        }
        return point.YearMonth;
    }

    /// <summary>
    /// 展示期首月之前：向前扫描历史月，建立最近有价种子（受 MaxTrendMonths 上限）
    /// </summary>
    private static void SeedCarryForwardBeforeRange(
        IReadOnlyList<TaktPriceTrendEntry> entries,
        DateTime displayStart,
        ref decimal? lastUnitPrice,
        ref decimal lastRawPrice,
        ref int lastPerUnit,
        ref string lastUnit,
        ref string? lastReference,
        ref string? lastSourceYm,
        ref string? lastSourcePriceDate)
    {
        var earliest = entries.Min(e => e.EffectiveStartDate.Date);
        var seedCursor = new DateTime(earliest.Year, earliest.Month, 1);
        var beforeDisplay = displayStart.AddMonths(-1);
        if (beforeDisplay < seedCursor)
        {
            return;
        }
        var minSeed = displayStart.AddMonths(1 - CarryForwardSeedMaxMonths);
        if (seedCursor < minSeed)
        {
            seedCursor = minSeed;
        }
        while (seedCursor <= beforeDisplay)
        {
            var monthStart = seedCursor;
            var monthEnd = seedCursor.AddMonths(1).AddTicks(-1);
            var native = ResolveNativeMonthPrice(entries, monthStart, monthEnd);
            if (native != null)
            {
                lastUnitPrice = native.Value.UnitPrice;
                lastRawPrice = native.Value.RawPrice;
                lastPerUnit = native.Value.PerUnit;
                lastUnit = native.Value.Unit;
                lastReference = native.Value.ReferenceCode;
                lastSourceYm = seedCursor.ToString("yyyy-MM");
                lastSourcePriceDate = native.Value.SourcePriceDate;
            }
            seedCursor = seedCursor.AddMonths(1);
        }
    }

    /// <summary>
    /// 解析自然月内原生有效价（无回填）
    /// </summary>
    private static (decimal UnitPrice, decimal RawPrice, int PerUnit, string Unit, string? ReferenceCode, int SourceRecordCount, string SourcePriceDate)?
        ResolveNativeMonthPrice(
            IReadOnlyList<TaktPriceTrendEntry> entries,
            DateTime monthStart,
            DateTime monthEnd)
    {
        var monthEntries = entries
            .Where(x => IsEffectiveInMonth(x, monthStart, monthEnd))
            .ToList();
        if (monthEntries.Count == 0)
        {
            return null;
        }
        var latestStart = monthEntries.Max(x => x.EffectiveStartDate.Date);
        var selected = monthEntries
            .Where(x => x.EffectiveStartDate.Date == latestStart)
            .ToList();
        var avgRaw = selected.Average(x => x.RawPrice);
        var perUnit = selected[0].PerUnit;
        var unit = selected[0].Unit ?? string.Empty;
        var unitPrice = NormalizeUnitPrice(avgRaw, perUnit);
        var sourcePriceDate = latestStart.ToString("yyyy-MM-dd");
        return (unitPrice, avgRaw, perUnit, unit, selected[0].ReferenceCode, selected.Count, sourcePriceDate);
    }

    /// <summary>
    /// 判断价格在自然月内是否有效
    /// </summary>
    /// <param name="entry">价记录</param>
    /// <param name="monthStart">月初</param>
    /// <param name="monthEnd">月末</param>
    /// <returns>是否有效</returns>
    private static bool IsEffectiveInMonth(TaktPriceTrendEntry entry, DateTime monthStart, DateTime monthEnd)
    {
        if (entry.EffectiveStartDate > monthEnd)
        {
            return false;
        }
        if (entry.EffectiveEndDate.HasValue && entry.EffectiveEndDate.Value < monthStart)
        {
            return false;
        }
        return true;
    }
}
