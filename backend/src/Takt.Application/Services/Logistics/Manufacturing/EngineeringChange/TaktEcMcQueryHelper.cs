// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMcQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：部管课主表/子表可见明细：采购类型 F 且仓库非 C003；按控制范围去重键 EcGrainKey（新物料写入编码组）
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
/// 部管课列表可见设变明细条件
/// </summary>
internal static class TaktEcMcQueryHelper
{
    /// <summary>
    /// 新采购类型为 F、新品仓库非 C003。同去重键明细全部保留供编码组汇总
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x => x.EcNewPurchaseType == purchaseTypeF);
        exp = exp.And(x => x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003);
        return exp.ToExpression();
    }

    /// <summary>
    /// 部管执行列表可见：F 且非 C003。同一 EcGrainKey（按成品/按机种/按设变）只留 Id 最大的一行。
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcMc, bool>> VisibleExecExpression()
    {
        var purchaseTypeF = TaktEcScopeConstants.PurchaseTypeExternal;
        var warehouseC003 = TaktEcScopeConstants.NewWarehousePcbaGate;
        var internalScope = TaktEcScopeConstants.Internal;
        var technicalScope = TaktEcScopeConstants.Technical;
        return x =>
            (x.EcImplScope == internalScope
                || x.EcImplScope == technicalScope
                || (x.EcNewPurchaseType == purchaseTypeF && (x.EcNewWarehouse == null || x.EcNewWarehouse != warehouseC003)))
            && !SqlFunc.Subqueryable<TaktEcMc>()
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
