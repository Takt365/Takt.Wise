// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkanQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：品管课主表/子表可见明细：设变单号+机种编码组+根物料编码组去重
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 品管课列表可见设变明细条件
/// </summary>
internal static class TaktEcHinkanQueryHelper
{
    /// <summary>
    /// 品管课明细可见条件（设变+机种+根物料编码去重）
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression() =>
        TaktEcExecModelRootMaterialDedup.VisibleDetailExpression();

    /// <summary>
    /// 品管执行列表可见：同一设变单号、机种编码组、根物料编码组只留 Id 最大的一行。机种只看编码组。
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcHinkan, bool>> VisibleExecExpression()
    {
        return x => !SqlFunc.Subqueryable<TaktEcHinkan>()
            .Where(s =>
                s.TenantCode == x.TenantCode
                && s.CompanyCode == x.CompanyCode
                && s.IsDeleted == 0
                && s.IsObsolete == 0
                && s.EcCode == x.EcCode
                && SqlFunc.ToString(s.EcModelCodeGroup) == SqlFunc.ToString(x.EcModelCodeGroup)
                && SqlFunc.ToString(s.EcRootMaterialCodeGroup) == SqlFunc.ToString(x.EcRootMaterialCodeGroup)
                && s.Id > x.Id)
            .Any();
    }
}
