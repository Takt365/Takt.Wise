// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktWorkingDayHelper.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：工作日判定（工厂日历优先，无日历时周一至周五回退）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 工作日辅助（纯静态；与假日字典 humanresource_attendance_holiday_working_day_type 语义对齐：0=非工作日）
/// </summary>
public static class TaktWorkingDayHelper
{
    /// <summary>
    /// 解析指定自然月内的工作日列表（升序、去重）
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="calendarRows">工厂日历行（日期 + IsWorkingDay + 工厂）</param>
    /// <returns>该月工作日日期列表</returns>
    public static IReadOnlyList<DateTime> ResolveWorkingDaysInMonth(
        int year,
        int month,
        IEnumerable<(DateTime CalendarDate, int IsWorkingDay, string RelatedPlant)>? calendarRows)
    {
        if (year < 1 || month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "年月无效");
        }
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);
        var rows = (calendarRows ?? Enumerable.Empty<(DateTime, int, string)>())
            .Where(r => r.CalendarDate.Date >= monthStart && r.CalendarDate.Date <= monthEnd)
            .ToList();
        if (rows.Count > 0)
        {
            var primaryPlant = rows
                .GroupBy(r => (r.RelatedPlant ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.Key)
                .First();
            return rows
                .Where(r => string.Equals(
                    (r.RelatedPlant ?? string.Empty).Trim(),
                    primaryPlant,
                    StringComparison.OrdinalIgnoreCase))
                .Where(r => r.IsWorkingDay != 0)
                .Select(r => r.CalendarDate.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }
        var fallback = new List<DateTime>();
        for (var day = monthStart; day <= monthEnd; day = day.AddDays(1))
        {
            if (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                continue;
            }
            fallback.Add(day);
        }
        return fallback;
    }

    /// <summary>
    /// 判断指定日期是否为当月第 N 个工作日
    /// </summary>
    /// <param name="asOfDate">判定日</param>
    /// <param name="nth">第几个工作日（从 1 起）</param>
    /// <param name="workingDaysInMonth">当月工作日列表（升序）</param>
    /// <returns>是否匹配</returns>
    public static bool IsNthWorkingDayOfMonth(
        DateTime asOfDate,
        int nth,
        IReadOnlyList<DateTime> workingDaysInMonth)
    {
        if (nth < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(nth), "工作日序号须 ≥ 1");
        }
        ArgumentNullException.ThrowIfNull(workingDaysInMonth);
        if (workingDaysInMonth.Count < nth)
        {
            return false;
        }
        return workingDaysInMonth[nth - 1].Date == asOfDate.Date;
    }
}
