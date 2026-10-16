// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcDeptScopeConstants.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门实施范围 logistics_manufacturing_ec_dept_scope_category（按成品/按机种/按设变）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 制二/制一/品管实施范围（字典 logistics_manufacturing_ec_dept_scope_category）。
/// 主表只记录这三课；其余部门按固定规则解析，与 TaktEcScopeConstants 的全仕向/部管/内部/技术独立。
/// </summary>
public static class TaktEcDeptScopeConstants
{
    /// <summary>
    /// 按成品
    /// </summary>
    public const int ByFinishedProduct = 1;

    /// <summary>
    /// 按机种
    /// </summary>
    public const int ByModel = 2;

    /// <summary>
    /// 按设变
    /// </summary>
    public const int ByEngineeringChange = 3;

    /// <summary>
    /// 部门实施范围显示名（按成品 / 按机种 / 按设变）
    /// </summary>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <returns>显示名</returns>
    public static string ResolveLabel(int ecDeptScope)
    {
        return ecDeptScope switch
        {
            ByFinishedProduct => "按成品",
            ByModel => "按机种",
            ByEngineeringChange => "按设变",
            _ => "按机种",
        };
    }

    /// <summary>
    /// 主表部门实施范围：全仕向保留按成品/按机种/按设变；部管固定按机种；内部/技术固定按设变
    /// </summary>
    /// <param name="ecScope">实施范围</param>
    /// <param name="ecDeptScope">表单提交的部门实施范围</param>
    /// <returns>主表部门实施范围</returns>
    public static int Normalize(int ecScope, int ecDeptScope)
    {
        if (ecScope == TaktEcScopeConstants.MaterialControl)
        {
            return ByModel;
        }
        if (ecScope == TaktEcScopeConstants.Internal
            || ecScope == TaktEcScopeConstants.Technical)
        {
            return ByEngineeringChange;
        }
        if (ecDeptScope is ByFinishedProduct or ByModel or ByEngineeringChange)
        {
            return ecDeptScope;
        }
        return ByModel;
    }

    /// <summary>
    /// 执行部门去重粒度。全仕向时生管与制二/制一/品管跟随主表。部管时生管按机种。采购/受检/部管/SMT 固定按设变，制技固定按机种。内部/技术按设变
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="ecScope">实施范围</param>
    /// <param name="masterDeptScope">主表制二/制一/品管实施范围</param>
    /// <returns>该执行部门的部门实施范围</returns>
    public static int ResolveForDept(string deptCode, int ecScope, int masterDeptScope)
    {
        if (deptCode == TaktEcDeptCodes.Te)
        {
            return ByModel;
        }
        if (ecScope == TaktEcScopeConstants.MaterialControl && deptCode == TaktEcDeptCodes.Pmc)
        {
            return ByModel;
        }
        if (ecScope != TaktEcScopeConstants.AllDestination)
        {
            return ByEngineeringChange;
        }
        if (deptCode is TaktEcDeptCodes.Pcba or TaktEcDeptCodes.Assy or TaktEcDeptCodes.Qa or TaktEcDeptCodes.Pmc)
        {
            return Normalize(ecScope, masterDeptScope);
        }
        return ByEngineeringChange;
    }
}
