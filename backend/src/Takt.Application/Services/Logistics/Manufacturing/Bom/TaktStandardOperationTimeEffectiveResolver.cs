// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeEffectiveResolver.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：按生产日期解析物料有效标准工时（每工作中心取最新生效版本，汇总标准工时/标准点数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 标准工时有效期解析（组立/PCBA 日报派生计算共用）
/// </summary>
internal static class TaktStandardOperationTimeEffectiveResolver
{
    /// <summary>
    /// 按工作中心选取生产日期当日有效的最新版本（EffectiveDate 最大且 &lt;= 生产日期）
    /// </summary>
    /// <param name="rows">已按租户/公司/物料/工厂/审批/有效期过滤的候选行</param>
    /// <returns>每个工作中心一条有效记录</returns>
    public static List<TaktStandardOperationTime> SelectLatestPerWorkCenter(IEnumerable<TaktStandardOperationTime> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        return rows
            .GroupBy(x => (x.WorkCenter ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group => group.OrderByDescending(x => x.EffectiveDate).First())
            .ToList();
    }

    /// <summary>
    /// 汇总标准工时得到标准工时（分钟）；单行优先 ConvertedMinutes，为 0 时取 StandardMinutes
    /// </summary>
    /// <param name="operationTimes">标准工时列表</param>
    /// <returns>标准工时(分钟)</returns>
    public static decimal CalculateStdMinutes(IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        if (operationTimes.Count == 0)
        {
            return 0;
        }
        decimal total = 0;
        foreach (var row in operationTimes)
        {
            total += row.ConvertedMinutes > 0 ? row.ConvertedMinutes : row.StandardMinutes;
        }
        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 汇总标准工时得到标准点数；每行取 StandardShorts 累加
    /// </summary>
    /// <param name="operationTimes">标准工时列表（通常为每工作中心最新有效版本）</param>
    /// <returns>标准点数</returns>
    public static int CalculateStdShorts(IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        if (operationTimes.Count == 0)
        {
            return 0;
        }
        var total = 0;
        foreach (var row in operationTimes)
        {
            total = checked(total + row.StandardShorts);
        }
        return total;
    }
}
