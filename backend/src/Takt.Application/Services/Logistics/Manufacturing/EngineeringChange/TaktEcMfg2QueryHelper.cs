// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMfg2QueryHelper.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：制二课主表/子表可见明细：采购类型非 F，再按设变单号+机种+根物料编码去重
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
/// 制二课列表可见设变明细条件（采购类型非 F）
/// </summary>
internal static class TaktEcMfg2QueryHelper
{
    /// <summary>
    /// 新采购类型非 F，且同设变单号+机种+根物料编码仅保留最大 Id 一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var byEc = TaktEcControlScopeConstants.ByEngineeringChange;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewPurchaseType != purchaseTypeF);
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && (SqlFunc.Subqueryable<TaktEcMfg2>()
                            .Where(e => e.EcCode == x.EcCode && e.IsDeleted == 0 && e.IsObsolete == 0 && e.EcControlScope == byEc)
                            .Any()
                        || (!SqlFunc.Subqueryable<TaktEcMfg2>()
                                .Where(e => e.EcCode == x.EcCode && e.IsDeleted == 0 && e.IsObsolete == 0)
                                .Any()
                            && SqlFunc.Subqueryable<TaktEcTe>()
                                .Where(g => g.Id == x.EcTeId && g.IsDeleted == 0 && g.EcControlScope == byEc)
                                .Any())
                        || (s.EcModelCode == x.EcModelCode && s.EcRootMaterialCode == x.EcRootMaterialCode))
                    && s.EcNewPurchaseType != purchaseTypeF
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 制二执行列表可见：同一设变单号、机种编码组、根物料编码组只留 Id 最大的一行。机种只看编码组。
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcMfg2, bool>> VisibleExecExpression()
    {
        return x => !SqlFunc.Subqueryable<TaktEcMfg2>()
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
