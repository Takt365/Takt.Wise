// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSmtRouteHelper.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：制造二课双表路由（F+C003→SMT/D0625；非 F→Seizounika/D0620；其余作废两侧）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 制造二课执行表路由目标
/// </summary>
internal enum TaktEcSmtRouteTarget
{
    /// <summary>
    /// 两侧均不作废目标（F 且非 C003）
    /// </summary>
    None = 0,
    /// <summary>
    /// SMT 执行表（F+C003）
    /// </summary>
    Smt = 1,
    /// <summary>
    /// 制二非 F 表
    /// </summary>
    Seizounika = 2
}

/// <summary>
/// 制造二课双表路由（F+C003→SMT/D0625；非 F→Seizounika/D0620）
/// </summary>
internal static class TaktEcSmtRouteHelper
{
    /// <summary>
    /// 按明细采购类型与仓库解析目标表
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <returns>路由目标</returns>
    public static TaktEcSmtRouteTarget Resolve(TaktEcDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (TaktEcScopeConstants.IsPcbaC003ExternalGroup(detail.EcNewPurchaseType, detail.EcNewWarehouse))
        {
            return TaktEcSmtRouteTarget.Smt;
        }
        if (TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(detail.EcNewPurchaseType))
        {
            return TaktEcSmtRouteTarget.Seizounika;
        }
        return TaktEcSmtRouteTarget.None;
    }

    /// <summary>
    /// 按采购类型与仓库解析目标表
    /// </summary>
    /// <param name="purchaseType">新采购类型</param>
    /// <param name="newWarehouse">新品仓库</param>
    /// <returns>路由目标</returns>
    public static TaktEcSmtRouteTarget Resolve(string? purchaseType, string? newWarehouse)
    {
        if (TaktEcScopeConstants.IsPcbaC003ExternalGroup(purchaseType, newWarehouse))
        {
            return TaktEcSmtRouteTarget.Smt;
        }
        if (TaktEcScopeConstants.IsPcbaOtherPurchaseGroup(purchaseType))
        {
            return TaktEcSmtRouteTarget.Seizounika;
        }
        return TaktEcSmtRouteTarget.None;
    }
}
