// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktYoYStatHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：同比（YoY）增长率计算
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 同比统计辅助（与前端 calcYoYPercent 语义一致）
/// </summary>
public static class TaktYoYStatHelper
{
    /// <summary>
    /// 计算同比增长率（%）；基期为 0 时：当期 &gt; 0 返回 100，否则 0
    /// </summary>
    /// <param name="current">当期值</param>
    /// <param name="baseline">基期值</param>
    /// <returns>同比增长率(%)</returns>
    public static decimal CalculateYoYPercent(decimal current, decimal baseline)
    {
        if (baseline <= 0)
        {
            return current > 0 ? 100m : 0m;
        }
        var rate = (current - baseline) / baseline * 100m;
        return Math.Round(rate, 2, MidpointRounding.AwayFromZero);
    }
}
