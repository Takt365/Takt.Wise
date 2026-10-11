// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktYearShardTableHelper.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：按年分表物理表名与年份解析（{base}_{yyyy}）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;

namespace Takt.Shared.Helpers;

/// <summary>
/// 按年分表命名与年份解析工具（无状态）
/// </summary>
public static class TaktYearShardTableHelper
{
    private static readonly Regex BaseTablePattern = new(@"^takt_[a-z0-9_]+$", RegexOptions.Compiled);

    /// <summary>
    /// 生成年分表名：{baseTable}_{year}
    /// </summary>
    /// <param name="baseTableName">基表名（无年份后缀，须 takt_ 开头）</param>
    /// <param name="year">年份</param>
    /// <returns>物理分表名</returns>
    /// <exception cref="ArgumentException">表名或年份非法</exception>
    public static string BuildYearTableName(string baseTableName, int year)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseTableName);
        if (year < 1970 || year > 2100)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "年份无效");
        }
        var baseName = baseTableName.Trim().ToLowerInvariant();
        if (baseName.EndsWith($"_{year}", StringComparison.Ordinal))
        {
            return baseName;
        }
        if (!BaseTablePattern.IsMatch(baseName))
        {
            throw new ArgumentException($"非法基表名: {baseTableName}");
        }
        var name = $"{baseName}_{year}";
        if (name.Length > 128)
        {
            throw new ArgumentException($"年分表名过长: {name}");
        }
        if (!BaseTablePattern.IsMatch(name))
        {
            throw new ArgumentException($"非法年分表名: {name}");
        }
        return name;
    }

    /// <summary>
    /// 从日期区间解析涉及的年份列表（升序）；无区间时返回默认年
    /// </summary>
    /// <param name="start">起始（含）</param>
    /// <param name="end">截止（含）</param>
    /// <param name="defaultYear">无区间时默认年（通常为当年）</param>
    /// <returns>年份列表</returns>
    public static IReadOnlyList<int> ResolveYears(DateTime? start, DateTime? end, int? defaultYear = null)
    {
        if (!start.HasValue && !end.HasValue)
        {
            return new[] { defaultYear ?? DateTime.Now.Year };
        }
        var y0 = (start ?? end)!.Value.Year;
        var y1 = (end ?? start)!.Value.Year;
        if (y0 > y1)
        {
            (y0, y1) = (y1, y0);
        }
        if (y1 - y0 > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(end), "跨年查询跨度不能超过 20 年");
        }
        var years = new List<int>(y1 - y0 + 1);
        for (var y = y0; y <= y1; y++)
        {
            years.Add(y);
        }
        return years;
    }

    /// <summary>
    /// 取日期所属业务年
    /// </summary>
    /// <param name="date">业务日期</param>
    /// <returns>年份</returns>
    public static int ResolveYear(DateTime date) => date.Year;

    /// <summary>
    /// 要求日期区间落在同一自然年（列表/分页按年分表路由）
    /// </summary>
    /// <param name="start">起始（含）</param>
    /// <param name="end">截止（含）</param>
    /// <param name="defaultYear">无区间时默认年</param>
    /// <returns>唯一年份</returns>
    /// <exception cref="ArgumentException">跨年或不合法</exception>
    public static int RequireSingleYear(DateTime? start, DateTime? end, int? defaultYear = null)
    {
        var years = ResolveYears(start, end, defaultYear);
        if (years.Count != 1)
        {
            throw new ArgumentException("按年分表查询时，日期起止须在同一自然年");
        }
        return years[0];
    }

    /// <summary>
    /// 从 yyyy-MM 期间解析自然年
    /// </summary>
    /// <param name="period">期间（yyyy-MM）</param>
    /// <returns>年份</returns>
    /// <exception cref="ArgumentException">期间为空或年份非法</exception>
    public static int ParseYyyyMmPeriodYear(string period)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(period);
        var s = period.Trim();
        if (s.Length < 4 || !int.TryParse(s.AsSpan(0, 4), out var year) || year < 1970 || year > 2100)
        {
            throw new ArgumentException("期间年份无效", nameof(period));
        }
        return year;
    }

    /// <summary>
    /// 从 yyyy-MM 期间区间解析涉及的年份列表（升序）；无区间时返回默认年
    /// </summary>
    /// <param name="start">起始期间（含，yyyy-MM）</param>
    /// <param name="end">截止期间（含，yyyy-MM）</param>
    /// <param name="defaultYear">无区间时默认年</param>
    /// <returns>年份列表</returns>
    public static IReadOnlyList<int> ResolveYearsFromYyyyMmPeriod(string? start, string? end, int? defaultYear = null)
    {
        if (string.IsNullOrWhiteSpace(start) && string.IsNullOrWhiteSpace(end))
        {
            return new[] { defaultYear ?? DateTime.Now.Year };
        }
        var y0 = ParseYyyyMmPeriodYear((start ?? end)!);
        var y1 = ParseYyyyMmPeriodYear((end ?? start)!);
        if (y0 > y1)
        {
            (y0, y1) = (y1, y0);
        }
        if (y1 - y0 > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(end), "跨年查询跨度不能超过 20 年");
        }
        var years = new List<int>(y1 - y0 + 1);
        for (var y = y0; y <= y1; y++)
        {
            years.Add(y);
        }
        return years;
    }

    /// <summary>
    /// 要求 yyyy-MM 期间起止落在同一自然年（列表/分页按年分表路由）
    /// </summary>
    /// <param name="start">起始期间（含，yyyy-MM）</param>
    /// <param name="end">截止期间（含，yyyy-MM）</param>
    /// <param name="defaultYear">无区间时默认年</param>
    /// <returns>唯一年份</returns>
    /// <exception cref="ArgumentException">跨年或不合法</exception>
    public static int RequireSingleYearFromYyyyMmPeriod(string? start, string? end, int? defaultYear = null)
    {
        var years = ResolveYearsFromYyyyMmPeriod(start, end, defaultYear);
        if (years.Count != 1)
        {
            throw new ArgumentException("按年分表查询时，期间起止须在同一自然年");
        }
        return years[0];
    }
}
