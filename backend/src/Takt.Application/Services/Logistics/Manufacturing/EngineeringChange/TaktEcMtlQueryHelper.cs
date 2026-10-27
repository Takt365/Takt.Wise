// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMtlQueryHelper.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：SMT主表/子表可见明细：F+C003；同一设变、机种、新物料只显示一行
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
/// SMT列表可见设变明细条件（F+C003）
/// </summary>
internal static class TaktEcMtlQueryHelper
{
    /// <summary>
    /// 新采购类型 F、新品仓库 C003。同设变单号+机种的明细全部保留，不按机种收成一行
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewPurchaseType == purchaseTypeF);
        exp = exp.And(x => x.EcNewWarehouse != null && x.EcNewWarehouse == warehouseC003);
        return exp.ToExpression();
    }

    /// <summary>
    /// SMT 执行列表可见：F+C003。同一设变单号、机种、新物料只留 Id 最大的一行。
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcMtl, bool>> VisibleExecExpression()
    {
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var internalScope = TaktEcScopeConstants.Internal;
        var technicalScope = TaktEcScopeConstants.Technical;
        return x =>
            (x.EcImplScope == internalScope
                || x.EcImplScope == technicalScope
                || (x.EcNewPurchaseType == purchaseTypeF && x.EcNewWarehouse != null && x.EcNewWarehouse == warehouseC003))
            && !SqlFunc.Subqueryable<TaktEcMtl>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && SqlFunc.ToString(s.EcModelCodeGroup) == SqlFunc.ToString(x.EcModelCodeGroup)
                    && s.EcNewMaterialCode == x.EcNewMaterialCode
                    && s.Id > x.Id)
                .Any();
    }
}
