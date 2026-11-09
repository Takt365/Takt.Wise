// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastVersionHelper.cs
// 创建时间：2026-11-02
// 创建人：Takt365(Cursor AI)
// 功能描述：销售预测明细版本计算（同一财年月份 1～20 行；增减相对上一版本；主表汇总各月最新版本）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Logistics.Manufacturing.Mds;

/// <summary>
/// 销售预测版本计算（主表下的明细行，不是列）
/// </summary>
public static class TaktSalesForecastVersionHelper
{
    /// <summary>
    /// 同一财年月份允许的最大版本号
    /// </summary>
    public const int MaxVersionNo = 20;

    /// <summary>
    /// 校验版本号与计划月份
    /// </summary>
    /// <param name="versionNo">版本号</param>
    /// <param name="planMonth">计划月份</param>
    /// <exception cref="TaktBusinessException">超出 1～20 或月份不在 1～12</exception>
    public static void EnsureVersion(int versionNo, int planMonth)
    {
        if (versionNo < 1 || versionNo > MaxVersionNo)
        {
            throw new TaktBusinessException($"销售预测版本号必须为 1～{MaxVersionNo}");
        }

        if (planMonth < 1 || planMonth > 12)
        {
            throw new TaktBusinessException("销售预测计划月份必须为 1～12");
        }
    }

    /// <summary>
    /// 按财年+月份重算增减与金额。版本 1 的增减等于本版本数量，其后为本版本减上一版本
    /// </summary>
    /// <param name="rows">同一销售预测下的未作废明细</param>
    public static void ApplyDeltas(IEnumerable<TaktSalesForecastItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        foreach (var group in rows.Where(x => x.IsObsolete == 0).GroupBy(x => $"{x.FiscalYear}|{x.PlanMonth}"))
        {
            decimal previous = 0;
            foreach (var row in group.OrderBy(x => x.VersionNo).ThenBy(x => x.LineNumber))
            {
                EnsureVersion(row.VersionNo, row.PlanMonth);
                row.PlanQuantityDelta = row.PlanQuantity - previous;
                previous = row.PlanQuantity;
                row.EstimatedAmount = decimal.Round(row.PlanQuantity * row.EstimatedUnitPrice, 2, MidpointRounding.AwayFromZero);
            }
        }
    }

    /// <summary>
    /// 各月取最大版本号的数量与金额，作为主表合计
    /// </summary>
    /// <param name="rows">同一销售预测下的未作废明细</param>
    /// <returns>数量合计、金额合计</returns>
    public static (decimal Quantity, decimal Amount) SumLatestVersions(IEnumerable<TaktSalesForecastItem> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        decimal quantity = 0;
        decimal amount = 0;
        foreach (var group in rows.Where(x => x.IsObsolete == 0).GroupBy(x => $"{x.FiscalYear}|{x.PlanMonth}"))
        {
            var latest = group.OrderBy(x => x.VersionNo).ThenBy(x => x.LineNumber).Last();
            quantity += latest.PlanQuantity;
            amount += latest.EstimatedAmount;
        }

        return (quantity, amount);
    }
}
