// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktQualityStatHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：质量检验合格率计算
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 质量检验统计辅助（合格率 = 合格数量 / 抽样数量 × 100%）
/// </summary>
public static class TaktQualityStatHelper
{
    /// <summary>
    /// 计算合格率（百分比，保留 2 位小数；抽样数量为 0 时返回 0）
    /// </summary>
    /// <param name="qualifiedQty">合格数量</param>
    /// <param name="sampleQty">抽样数量</param>
    /// <returns>合格率(%)</returns>
    public static decimal CalculatePassRatePercent(decimal qualifiedQty, decimal sampleQty)
    {
        if (sampleQty <= 0)
        {
            return 0;
        }
        var rate = qualifiedQty / sampleQty * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }
}
