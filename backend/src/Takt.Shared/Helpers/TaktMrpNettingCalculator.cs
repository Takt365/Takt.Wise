// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktMrpNettingCalculator.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：MRP 净需求计算纯函数（毛需求、库存、计划接收 → 净需求与 POH）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// MRP 净需求计算（无状态纯函数）
/// </summary>
public static class TaktMrpNettingCalculator
{
    /// <summary>
    /// 计算净需求与运算后预计可用库存
    /// </summary>
    /// <param name="grossRequirement">毛需求</param>
    /// <param name="onHandQuantity">现有库存</param>
    /// <param name="scheduledReceipts">计划接收</param>
    /// <returns>净需求、预计可用库存</returns>
    /// <exception cref="ArgumentException">数量为负时抛出</exception>
    public static (decimal NetRequirement, decimal ProjectedOnHand) Calculate(
        decimal grossRequirement,
        decimal onHandQuantity,
        decimal scheduledReceipts)
    {
        if (grossRequirement < 0 || onHandQuantity < 0 || scheduledReceipts < 0)
        {
            throw new ArgumentException("MRP 净需求计算参数不能为负");
        }

        var available = onHandQuantity + scheduledReceipts;
        var net = grossRequirement > available ? grossRequirement - available : 0m;
        var projectedOnHand = available > grossRequirement ? available - grossRequirement : 0m;
        return (net, projectedOnHand);
    }
}
