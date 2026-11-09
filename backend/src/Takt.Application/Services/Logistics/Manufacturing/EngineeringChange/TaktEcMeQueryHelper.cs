// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMeQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：制技课主表/子表可见明细：按控制范围去重键 EcGrainKey（保留最大 Id）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 制技课列表可见设变明细条件
/// </summary>
internal static class TaktEcMeQueryHelper
{
    /// <summary>
    /// 制技课明细可见条件（同设变下保留全部；执行行按 EcGrainKey 去重）
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        return x => x.IsObsolete == 0;
    }

    /// <summary>
    /// 制技执行列表可见：同一 EcGrainKey（按成品/按机种/按设变）只留 Id 最大的一行
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcMe, bool>> VisibleExecExpression()
    {
        return x => !SqlFunc.Subqueryable<TaktEcMe>()
            .Where(s =>
                s.TenantCode == x.TenantCode
                && s.CompanyCode == x.CompanyCode
                && s.IsDeleted == 0
                && s.IsObsolete == 0
                && s.EcCode == x.EcCode
                && SqlFunc.ToString(s.EcGrainKey) == SqlFunc.ToString(x.EcGrainKey)
                && s.Id > x.Id)
            .Any();
    }
}
