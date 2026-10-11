// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktQualityInspectionTrendAnalysisCore.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC/IPQC/FQC 检验月推移转置分析共用构建逻辑
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 检验单推移源数据快照
/// </summary>
public sealed class TaktInspectionTrendOrderSnapshot
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; init; } = string.Empty;

    /// <summary>
    /// 维度编码（供应商/工序/客户）
    /// </summary>
    public string DimensionCode { get; init; } = string.Empty;

    /// <summary>
    /// 维度名称
    /// </summary>
    public string DimensionName { get; init; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; init; }

    /// <summary>
    /// 总抽样数量
    /// </summary>
    public int TotalSampleQuantity { get; init; }

    /// <summary>
    /// 总合格数量
    /// </summary>
    public int TotalQualifiedQuantity { get; init; }

    /// <summary>
    /// 总不合格数量
    /// </summary>
    public int TotalUnqualifiedQuantity { get; init; }
}

/// <summary>
/// 检验月推移内存构建结果
/// </summary>
/// <typeparam name="TRow">行 DTO 类型</typeparam>
public sealed class QualityInspectionMonthlyTrendBuilt<TRow> where TRow : TaktQualityInspectionMonthlyTrendDto
{
    /// <summary>
    /// 过滤并排序后的全量行
    /// </summary>
    public List<TRow> OrderedRows { get; init; } = new();

    /// <summary>
    /// 期间列顺序
    /// </summary>
    public List<string> PeriodOrder { get; init; } = new();

    /// <summary>
    /// 基准期间
    /// </summary>
    public string? BasePeriod { get; init; }

    /// <summary>
    /// 对比期间
    /// </summary>
    public string? ComparePeriod { get; init; }

    /// <summary>
    /// 上升行数
    /// </summary>
    public int UpCount { get; init; }

    /// <summary>
    /// 下降行数
    /// </summary>
    public int DownCount { get; init; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; init; }

    /// <summary>
    /// 无法比较行数
    /// </summary>
    public int NoneCount { get; init; }

    /// <summary>
    /// 空结果
    /// </summary>
    /// <returns>空构建结果</returns>
    public static QualityInspectionMonthlyTrendBuilt<TRow> Empty() => new();
}

/// <summary>
/// 检验月推移转置分析共用构建器
/// </summary>
public static class TaktQualityInspectionTrendAnalysisCore
{
    /// <summary>
    /// 构建检验月推移转置分析全量结果
    /// </summary>
    /// <param name="sources">检验单快照</param>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <param name="focusPeriodInput">关注期间</param>
    /// <param name="trendFilter">涨跌筛选</param>
    /// <param name="createRow">创建行 DTO</param>
    /// <param name="primaryKeySelector">主排序键</param>
    /// <typeparam name="TRow">行 DTO 类型</typeparam>
    /// <returns>内存构建结果</returns>
    public static QualityInspectionMonthlyTrendBuilt<TRow> Build<TRow>(
        IReadOnlyList<TaktInspectionTrendOrderSnapshot> sources,
        DateTime? periodDateStart,
        DateTime? periodDateEnd,
        string? focusPeriodInput,
        string? trendFilter,
        Func<string, string, string, string, IReadOnlyDictionary<string, TaktInspectionTrendMonthAggregate>, string?, TRow> createRow,
        Func<TRow, string> primaryKeySelector)
        where TRow : TaktQualityInspectionMonthlyTrendDto
    {
        if (sources.Count == 0)
        {
            return QualityInspectionMonthlyTrendBuilt<TRow>.Empty();
        }
        var (_, rangeEnd, periodOrder) = TaktInspectionTrendAnalysisHelper.ResolveTrendRange(
            periodDateStart,
            periodDateEnd);
        var focusPeriod = TaktInspectionTrendAnalysisHelper.ResolveFocusPeriod(focusPeriodInput, periodOrder);
        var validSources = sources
            .Where(s => s.InspectionDate.HasValue && s.InspectionDate.Value <= rangeEnd)
            .ToList();
        if (validSources.Count == 0)
        {
            return QualityInspectionMonthlyTrendBuilt<TRow>.Empty();
        }
        var allRows = validSources
            .GroupBy(
                s => new InspectionTrendRowKey(s.PlantCode.Trim(), s.DimensionCode.Trim()),
                InspectionTrendRowKeyComparer.Instance)
            .Select(g =>
            {
                var plantCode = g.Key.PlantCode;
                var dimensionCode = g.Key.DimensionCode;
                var dimensionName = g.Select(x => x.DimensionName?.Trim())
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty;
                var monthly = BuildMonthlyAggregates(g, periodOrder);
                return createRow(plantCode, dimensionCode, dimensionName, dimensionCode, monthly, focusPeriod);
            })
            .ToList();
        var filtered = TaktInspectionTrendAnalysisHelper.FilterTrendRows(
            allRows,
            trendFilter,
            r => r.Trend);
        var ordered = TaktInspectionTrendAnalysisHelper.OrderTrendRows(
            filtered,
            r => r.Trend,
            r => r.VarianceAmount,
            primaryKeySelector);
        return new QualityInspectionMonthlyTrendBuilt<TRow>
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 填充行期间字典并计算环比
    /// </summary>
    /// <param name="row">行 DTO</param>
    /// <param name="monthly">各月聚合</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <typeparam name="TRow">行 DTO 类型</typeparam>
    public static void FillPeriodMetrics<TRow>(
        TRow row,
        IReadOnlyDictionary<string, TaktInspectionTrendMonthAggregate> monthly,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
        where TRow : TaktQualityInspectionMonthlyTrendDto
    {
        foreach (var period in periodOrder)
        {
            if (!monthly.TryGetValue(period, out var agg))
            {
                continue;
            }
            if (agg.SampleQty > 0)
            {
                row.PeriodDefectRates[period] = agg.DefectRate;
            }
            row.PeriodOrderCounts[period] = agg.OrderCount;
            row.PeriodSampleQuantities[period] = agg.SampleQty;
            row.PeriodUnqualifiedQuantities[period] = agg.UnqualifiedQty;
        }
        row.Trend = TaktInspectionTrendAnalysisHelper.ApplyFocusTrend(
            row.PeriodDefectRates,
            focusPeriod,
            out var basePeriod,
            out var comparePeriod,
            out var varianceAmount,
            out var variancePercent);
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.VarianceAmount = varianceAmount;
        row.VariancePercent = variancePercent;
    }

    /// <summary>
    /// 按维度分组聚合各月指标
    /// </summary>
    /// <param name="orders">同维度检验单</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间→聚合</returns>
    private static Dictionary<string, TaktInspectionTrendMonthAggregate> BuildMonthlyAggregates(
        IEnumerable<TaktInspectionTrendOrderSnapshot> orders,
        IReadOnlyList<string> periodOrder)
    {
        var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        var map = new Dictionary<string, TaktInspectionTrendMonthAggregate>(StringComparer.Ordinal);
        foreach (var order in orders)
        {
            var periodKey = TaktInspectionTrendAnalysisHelper.ToPeriodKey(order.InspectionDate);
            if (string.IsNullOrWhiteSpace(periodKey) || !periodSet.Contains(periodKey))
            {
                continue;
            }
            if (!map.TryGetValue(periodKey, out var agg))
            {
                agg = new TaktInspectionTrendMonthAggregate();
                map[periodKey] = agg;
            }
            agg.OrderCount = checked(agg.OrderCount + 1);
            agg.SampleQty = checked(agg.SampleQty + order.TotalSampleQuantity);
            agg.QualifiedQty = checked(agg.QualifiedQty + order.TotalQualifiedQuantity);
            agg.UnqualifiedQty = checked(agg.UnqualifiedQty + order.TotalUnqualifiedQuantity);
        }
        return map;
    }

    /// <summary>
    /// 检验推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="DimensionCode">维度编码</param>
    private sealed record InspectionTrendRowKey(string PlantCode, string DimensionCode);

    /// <summary>
    /// 检验推移行键比较器
    /// </summary>
    private sealed class InspectionTrendRowKeyComparer : IEqualityComparer<InspectionTrendRowKey>
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static InspectionTrendRowKeyComparer Instance { get; } = new();

        /// <summary>
        /// 月生产推移行键比较器
        /// </summary>
        /// <summary>
        /// 单例
        /// </summary>
        /// <summary>
        /// 判断两行键是否相等（工厂/机种/产出类别，忽略大小写）
        /// </summary>
        /// <param name="x">左值</param>
        /// <param name="y">右值</param>
        /// <returns>是否相等</returns>
        public bool Equals(InspectionTrendRowKey? x, InspectionTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return false;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.DimensionCode, y.DimensionCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算行键哈希（工厂/机种/产出类别大写）
        /// </summary>
        /// <param name="obj">行键</param>
        /// <returns>哈希码</returns>
        public int GetHashCode(InspectionTrendRowKey obj) =>
            HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.PlantCode),
                StringComparer.OrdinalIgnoreCase.GetHashCode(obj.DimensionCode));
    }
}
