// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcIqcQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：受检课主表/子表可见明细：新品检验为 1；按设变单号去重一行（需检验新物料写入编码组）
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
internal static class TaktEcIqcQueryHelper
{
    /// <summary>
    /// 新品检验为 1（同设变明细全部保留供编码组汇总）
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewRequiresInspection == 1);
        return exp.ToExpression();
    }

    /// <summary>
    /// 受检执行列表可见：新品检验为 1，按设变单号去重，保留最大 Id
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcIqc, bool>> VisibleExecExpression()
    {
        return x =>
            x.EcNewRequiresInspection == 1
            && !SqlFunc.Subqueryable<TaktEcIqc>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcNewRequiresInspection == 1
                    && s.EcCode == x.EcCode
                    && s.Id > x.Id)
                .Any();
    }
}
