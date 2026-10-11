// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintTrendService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉月度推移转置分析服务（与客诉 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Domain.Entities.Logistics.Quality.Complaint;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 顾客投诉月度推移转置分析服务（读客诉主表；与 TaktCustomerComplaintService 分离）
/// </summary>
public class TaktCustomerComplaintTrendService : TaktServiceBase, ITaktCustomerComplaintTrendService
{
    private readonly ITaktCompanyRepository<TaktCustomerComplaint> _customerComplaintRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintRepository">客诉主仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerComplaintTrendService(
        ITaktCompanyRepository<TaktCustomerComplaint> customerComplaintRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerComplaintRepository = customerComplaintRepository;
    }

/// <inheritdoc />
    public async Task<TaktCustomerComplaintMonthlyTrendResultDto> GetCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildCustomerComplaintMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktCustomerComplaintMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktCustomerComplaintMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            CustomerCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildCustomerComplaintMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "customerCode", "customerName" };
        var columnLabels = new List<string> { "工厂代码", "客户编码", "客户名称" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
                ["basePeriod"] = row.BasePeriod,
                ["comparePeriod"] = row.ComparePeriod,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent.HasValue
                    ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                ["trend"] = row.Trend,
            };
            foreach (var period in built.PeriodOrder)
            {
                dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var count) ? count : 0;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "顾客投诉推移表",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "顾客投诉推移"));
    }

    /// <summary>
    /// 构建顾客投诉月度推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析构建结果</returns>
    private async Task<CustomerComplaintMonthlyTrendAnalysisBuilt> BuildCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var exp = BuildCustomerComplaintTrendExpression(queryDto, plantCode);
        var complaints = await _customerComplaintRepository.GetListAsync(exp);
        if (complaints.Count == 0)
        {
            return CustomerComplaintMonthlyTrendAnalysisBuilt.Empty();
        }
        var (rangeStart, rangeEnd, periodOrder) = ResolveCustomerComplaintTrendRange(queryDto);
        var focusPeriod = ResolveCustomerComplaintFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var allRows = complaints
            .GroupBy(
                c => new CustomerComplaintTrendRowKey(
                    c.PlantCode.Trim(),
                    c.CustomerCode?.Trim() ?? string.Empty),
                CustomerComplaintTrendRowKeyComparer.Instance)
            .Select(g => BuildCustomerComplaintMonthlyTrendRow(
                g.Key,
                g.ToList(),
                periodOrder,
                focusPeriod,
                rangeStart,
                rangeEnd))
            .ToList();
        var filtered = FilterCustomerComplaintTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderCustomerComplaintTrendRows(filtered);
        return new CustomerComplaintMonthlyTrendAnalysisBuilt
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
    /// 构建顾客投诉推移筛选条件
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktCustomerComplaint, bool>> BuildCustomerComplaintTrendExpression(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto,
        string plantCode)
    {
        var (rangeStart, rangeEnd, _) = ResolveCustomerComplaintTrendRange(queryDto);
        var exp = Expressionable.Create<TaktCustomerComplaint>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ComplaintDate >= rangeStart
            && x.ComplaintDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode.Trim();
            exp = exp.And(x => x.CustomerCode == customerCode);
        }
        if (queryDto.ComplaintType.HasValue)
        {
            exp = exp.And(x => x.ComplaintType == queryDto.ComplaintType.Value);
        }
        if (queryDto.ComplaintLevel.HasValue)
        {
            exp = exp.And(x => x.ComplaintLevel == queryDto.ComplaintLevel.Value);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析顾客投诉推移分析日期区间与期间列
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起止与期间列顺序</returns>
    private static (DateTime RangeStart, DateTime RangeEnd, List<string> PeriodOrder) ResolveCustomerComplaintTrendRange(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizeCustomerComplaintPeriodBounds(
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
            var rangeStart = startMonth;
            var rangeEnd = endMonth.AddMonths(1).AddDays(-1);
            var periodOrder = BuildCustomerComplaintConsecutivePeriodOrder(startMonth, endMonth);
            return (rangeStart, rangeEnd, periodOrder);
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        var start = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
        var endMonthFirst = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
        return (resolvedStart, resolvedEnd, BuildCustomerComplaintConsecutivePeriodOrder(start, endMonthFirst));
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizeCustomerComplaintPeriodBounds(
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
    /// 构建连续 yyyy-MM 期间列
    /// </summary>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>期间列顺序</returns>
    private static List<string> BuildCustomerComplaintConsecutivePeriodOrder(DateTime periodStart, DateTime periodEnd)
    {
        var order = new List<string>();
        for (var cursor = periodStart; cursor <= periodEnd; cursor = cursor.AddMonths(1))
        {
            order.Add(cursor.ToString("yyyy-MM"));
        }
        return order;
    }

    /// <summary>
    /// 解析关注期间
    /// </summary>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>关注期间 yyyy-MM</returns>
    private static string? ResolveCustomerComplaintFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 构建单行顾客投诉月推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键投诉记录</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="rangeStart">分析区间起</param>
    /// <param name="rangeEnd">分析区间止</param>
    /// <returns>转置行</returns>
    private static TaktCustomerComplaintMonthlyTrendDto BuildCustomerComplaintMonthlyTrendRow(
        CustomerComplaintTrendRowKey key,
        IReadOnlyList<TaktCustomerComplaint> groupRows,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var row = new TaktCustomerComplaintMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            CustomerCode = key.CustomerCode,
            CustomerName = groupRows
                .Select(r => r.CustomerName1?.Trim())
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty,
            Trend = "none",
        };
        foreach (var period in periodOrder)
        {
            if (!DateTime.TryParseExact(
                    period + "-01",
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out var monthStart))
            {
                continue;
            }
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);
            if (monthEnd < rangeStart || monthStart > rangeEnd)
            {
                continue;
            }
            var count = groupRows.Count(r =>
                r.ComplaintDate >= monthStart && r.ComplaintDate <= monthEnd);
            if (count > 0)
            {
                row.PeriodValues[period] = count;
            }
        }
        ApplyCustomerComplaintFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 按关注月计算环比涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyCustomerComplaintFocusTrend(
        TaktCustomerComplaintMonthlyTrendDto row,
        string? focusPeriod)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.PeriodValues.TryGetValue(basePeriod, out var baseCount);
        row.PeriodValues.TryGetValue(comparePeriod, out var compareCount);
        row.VarianceAmount = compareCount - baseCount;
        if (baseCount != 0)
        {
            row.VariancePercent = Math.Round(
                (decimal)row.VarianceAmount.Value / baseCount,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareCount > baseCount)
        {
            row.Trend = "up";
        }
        else if (compareCount < baseCount)
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
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktCustomerComplaintMonthlyTrendDto> FilterCustomerComplaintTrendRows(
        IReadOnlyList<TaktCustomerComplaintMonthlyTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
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
    private static List<TaktCustomerComplaintMonthlyTrendDto> OrderCustomerComplaintTrendRows(
        IReadOnlyList<TaktCustomerComplaintMonthlyTrendDto> rows)
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
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0))
            .ThenBy(r => r.CustomerCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 顾客投诉推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="CustomerCode">客户编码</param>
    private sealed record CustomerComplaintTrendRowKey(string PlantCode, string CustomerCode);

    /// <summary>
    /// 顾客投诉推移行键比较器
    /// </summary>
    private sealed class CustomerComplaintTrendRowKeyComparer : IEqualityComparer<CustomerComplaintTrendRowKey>
    {
        /// <summary>单例</summary>
        public static CustomerComplaintTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(CustomerComplaintTrendRowKey? x, CustomerComplaintTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return x == y;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.Ordinal)
                && string.Equals(x.CustomerCode, y.CustomerCode, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public int GetHashCode(CustomerComplaintTrendRowKey obj) =>
            HashCode.Combine(obj.PlantCode, obj.CustomerCode);
    }

    /// <summary>
    /// 顾客投诉月度推移分析构建结果
    /// </summary>
    private sealed class CustomerComplaintMonthlyTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktCustomerComplaintMonthlyTrendDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>环比基准期间</summary>
        public string? BasePeriod { get; init; }

        /// <summary>环比对比期间</summary>
        public string? ComparePeriod { get; init; }

        /// <summary>上涨行数</summary>
        public int UpCount { get; init; }

        /// <summary>下跌行数</summary>
        public int DownCount { get; init; }

        /// <summary>持平行数</summary>
        public int FlatCount { get; init; }

        /// <summary>无法比较行数</summary>
        public int NoneCount { get; init; }

        /// <summary>空结果</summary>
        public static CustomerComplaintMonthlyTrendAnalysisBuilt Empty() => new();
    }
}
