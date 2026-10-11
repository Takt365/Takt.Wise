// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktAssyOutputProdDateEditLockHelper.cs
// 功能描述：组立日报生产日期编辑锁定（生产日期所属月份的下月 cutoff 日之后不可新增/修改）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 组立日报生产日期编辑截止判定（例：6 月数据在 7 月 5 日含当日可改，7 月 6 日起锁定）
/// </summary>
public static class TaktAssyOutputProdDateEditLockHelper
{
    /// <summary>
    /// 默认编辑截止日：生产日期次月的第几天（含当日仍可编辑）
    /// </summary>
    public const int DefaultCutoffDayOfNextMonth = 5;

    /// <summary>
    /// 解析生产日期对应的编辑截止日（生产日期所属月份的下月 cutoff 日）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    /// <param name="cutoffDayOfNextMonth">次月截止日（1～28）</param>
    /// <returns>编辑截止日（仅日期部分）</returns>
    public static DateTime ResolveEditDeadlineDate(DateTime prodDate, int cutoffDayOfNextMonth = DefaultCutoffDayOfNextMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(cutoffDayOfNextMonth, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cutoffDayOfNextMonth, 28);
        var prodDateOnly = prodDate.Date;
        var nextMonthFirst = new DateTime(prodDateOnly.Year, prodDateOnly.Month, 1).AddMonths(1);
        return new DateTime(nextMonthFirst.Year, nextMonthFirst.Month, cutoffDayOfNextMonth);
    }

    /// <summary>
    /// 生产日期是否已锁定（不可新增/修改/删除）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    /// <param name="referenceDate">参考日期（通常为当前业务日）</param>
    /// <param name="cutoffDayOfNextMonth">次月截止日（含当日仍可编辑）</param>
    /// <returns>已锁定时为 true</returns>
    public static bool IsProdDateLocked(
        DateTime prodDate,
        DateTime referenceDate,
        int cutoffDayOfNextMonth = DefaultCutoffDayOfNextMonth)
    {
        var deadline = ResolveEditDeadlineDate(prodDate, cutoffDayOfNextMonth);
        return referenceDate.Date > deadline.Date;
    }

    /// <summary>
    /// 解析生产日期可选范围（每月 cutoff 日之后仅当月1日至今日；5 日含之前允许上月1日至今日）
    /// </summary>
    /// <param name="referenceDate">参考日期</param>
    /// <param name="cutoffDayOfNextMonth">次月截止日</param>
    /// <returns>最小/最大可选生产日期</returns>
    public static (DateTime MinDate, DateTime MaxDate) ResolveSelectableProdDateRange(
        DateTime referenceDate,
        int cutoffDayOfNextMonth = DefaultCutoffDayOfNextMonth)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(cutoffDayOfNextMonth, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cutoffDayOfNextMonth, 28);
        var refDate = referenceDate.Date;
        var maxDate = refDate;
        var minDate = refDate.Day > cutoffDayOfNextMonth
            ? new DateTime(refDate.Year, refDate.Month, 1)
            : new DateTime(refDate.Year, refDate.Month, 1).AddMonths(-1);
        return (minDate, maxDate);
    }

    /// <summary>
    /// 生产日期是否可在新增/修改时选择（未锁定且在允许月份范围内）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    /// <param name="referenceDate">参考日期</param>
    /// <param name="cutoffDayOfNextMonth">次月截止日</param>
    /// <returns>可选时为 true</returns>
    public static bool IsProdDateSelectable(
        DateTime prodDate,
        DateTime referenceDate,
        int cutoffDayOfNextMonth = DefaultCutoffDayOfNextMonth)
    {
        if (IsProdDateLocked(prodDate, referenceDate, cutoffDayOfNextMonth))
        {
            return false;
        }
        var (minDate, maxDate) = ResolveSelectableProdDateRange(referenceDate, cutoffDayOfNextMonth);
        var prodDateOnly = prodDate.Date;
        return prodDateOnly >= minDate && prodDateOnly <= maxDate;
    }
}
