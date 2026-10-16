// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcUkekenQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：受检课主表/子表可见明细：新品检验为 1 按设变去重（设变单号+新物料，保留最大 Id）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 受检课列表可见设变明细条件
/// </summary>
internal static class TaktEcUkekenQueryHelper
{
    /// <summary>
    /// 新品检验为 1，且同设变单号+新物料仅保留最大 Id 一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewRequiresInspection == 1);
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcNewMaterialCode == x.EcNewMaterialCode
                    && s.EcNewRequiresInspection == 1
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 受检执行列表可见：新品检验为 1，按设变单号+新物料去重，保留最大 Id
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcUkeken, bool>> VisibleExecExpression()
    {
        return x =>
            x.EcNewRequiresInspection == 1
            && !SqlFunc.Subqueryable<TaktEcUkeken>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcNewRequiresInspection == 1
                    && s.EcCode == x.EcCode
                    && s.EcNewMaterialCode == x.EcNewMaterialCode
                    && s.Id > x.Id)
                .Any();
    }
}
