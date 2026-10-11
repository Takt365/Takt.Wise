// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDefectStatHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计不良率/直行率计算
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 不良统计辅助（不良率 = 不良数量 / 分母 × 100%；直行率 = 良品 / 分母 × 100%）
/// </summary>
public static class TaktDefectStatHelper
{
    /// <summary>
    /// 计算不良率（百分比，保留 2 位小数；分母为 0 时返回 0）
    /// </summary>
    /// <param name="defectQty">不良数量</param>
    /// <param name="baseQty">统计分母</param>
    /// <returns>不良率(%)</returns>
    public static decimal CalculateDefectRatePercent(decimal defectQty, decimal baseQty)
    {
        if (baseQty <= 0)
        {
            return 0;
        }
        var rate = defectQty / baseQty * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 计算直行率（百分比，保留 2 位小数；分母为 0 时返回 0）
    /// </summary>
    /// <param name="goodQty">良品/无不良数量</param>
    /// <param name="baseQty">统计分母</param>
    /// <returns>直行率(%)</returns>
    public static decimal CalculateYieldRatePercent(decimal goodQty, decimal baseQty)
    {
        if (baseQty <= 0)
        {
            return 0;
        }
        var rate = goodQty / baseQty * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 停产状态：进行中（字典 logistics_manufacturing_prod_status=1）
    /// </summary>
    public const int ProdStatusInProgress = 1;

    /// <summary>
    /// 停产状态：已完成（字典 logistics_manufacturing_prod_status=2）
    /// </summary>
    public const int ProdStatusCompleted = 2;

    /// <summary>
    /// 解析批次停产状态：批次工单总数量与累计生实实绩完全相等且大于 0 时为已完成，否则为进行中
    /// </summary>
    /// <param name="batchOrderQty">批次工单总数量</param>
    /// <param name="prodActualQty">累计生实实绩</param>
    /// <returns>字典 logistics_manufacturing_prod_status 值</returns>
    public static int ResolveBatchProdStatus(decimal batchOrderQty, decimal prodActualQty)
    {
        return batchOrderQty > 0 && prodActualQty == batchOrderQty
            ? ProdStatusCompleted
            : ProdStatusInProgress;
    }

    /// <summary>
    /// 解析工单状态：工单数量与累计生实实绩完全相等且大于 0 时为已完成，否则为进行中
    /// </summary>
    /// <param name="prodOrderQty">工单数量</param>
    /// <param name="prodActualQty">累计生实实绩</param>
    /// <returns>字典 logistics_manufacturing_prod_status 值</returns>
    public static int ResolveOrderProdStatus(decimal prodOrderQty, decimal prodActualQty)
    {
        return prodOrderQty > 0 && prodActualQty == prodOrderQty
            ? ProdStatusCompleted
            : ProdStatusInProgress;
    }
}
