// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptExecRedundantBinder.cs
// 创建时间：2026-09-02
// 创建人：Takt365(Cursor AI)
// 功能描述：从来源设变明细同步各部门执行行冗余字段（采购/受检/部管/制一/品管/制二字段集不同）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细 → 部门执行行冗余字段绑定（新增/更新/来源导入共用）
/// </summary>
internal static class TaktEcDeptExecRedundantBinder
{
    /// <summary>
    /// 按实体类型拷贝明细冗余字段；新建时写入行号。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号；更新时传 null 不改行号</param>
    public static void Apply(object exec, TaktEcDetail detail, int? lineNumberForCreate)
    {
        ArgumentNullException.ThrowIfNull(exec);
        ArgumentNullException.ThrowIfNull(detail);
        switch (exec)
        {
            case TaktEcKoubai koubai:
                ApplyIdentity(koubai, detail, lineNumberForCreate);
                ApplyEcScope(koubai, detail);
                koubai.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                koubai.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                koubai.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                koubai.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                koubai.EcRootMaterialDescription = ResolveRootMaterialDescription(koubai.EcScope, koubai.EcDeptScope, detail.EcRootMaterialDescription);
                koubai.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                koubai.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                koubai.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                koubai.EcOldPartDisposition = detail.EcOldPartDisposition;
                break;
            case TaktEcUkeken ukeken:
                ApplyIdentity(ukeken, detail, lineNumberForCreate);
                ApplyEcScope(ukeken, detail);
                ukeken.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                ukeken.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                ukeken.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                ukeken.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                ukeken.EcRootMaterialDescription = ResolveRootMaterialDescription(ukeken.EcScope, ukeken.EcDeptScope, detail.EcRootMaterialDescription);
                ukeken.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                ukeken.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                ukeken.EcNewRequiresInspection = detail.EcNewRequiresInspection;
                break;
            case TaktEcBukan bukan:
                ApplyIdentity(bukan, detail, lineNumberForCreate);
                ApplyEcScope(bukan, detail);
                bukan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                bukan.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                bukan.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                bukan.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                bukan.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                bukan.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                bukan.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                bukan.EcRootMaterialDescription = ResolveRootMaterialDescription(bukan.EcScope, bukan.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcEmtl smt:
                ApplyIdentity(smt, detail, lineNumberForCreate);
                ApplyEcScope(smt, detail);
                smt.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                smt.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                smt.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                smt.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                smt.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                smt.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                smt.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                smt.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                smt.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                smt.EcRootMaterialDescription = ResolveRootMaterialDescription(smt.EcScope, smt.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcSeizounika seizounika:
                ApplyIdentity(seizounika, detail, lineNumberForCreate);
                ApplyEcScope(seizounika, detail);
                seizounika.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                seizounika.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                seizounika.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizounika.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                seizounika.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                seizounika.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcNewMaterialCode]);
                seizounika.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                seizounika.EcParentMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcParentMaterialCode]);
                seizounika.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                seizounika.EcRootMaterialDescription = ResolveRootMaterialDescription(seizounika.EcScope, seizounika.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcSeikan seikan:
                ApplyIdentity(seikan, detail, lineNumberForCreate);
                ApplyEcScope(seikan, detail);
                seikan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seikan.EcModelCode = detail.EcModelCode ?? string.Empty;
                seikan.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                seikan.EcRootMaterialDescription = ResolveRootMaterialDescription(seikan.EcScope, seikan.EcDeptScope, detail.EcRootMaterialDescription);
                seikan.EcOldPartDisposition = detail.EcOldPartDisposition;
                break;
            case TaktEcSeizouikka seizouikka:
                ApplyIdentity(seizouikka, detail, lineNumberForCreate);
                ApplyEcScope(seizouikka, detail);
                seizouikka.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizouikka.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                seizouikka.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                seizouikka.EcRootMaterialDescription = ResolveRootMaterialDescription(seizouikka.EcScope, seizouikka.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcHinkan hinkan:
                ApplyIdentity(hinkan, detail, lineNumberForCreate);
                ApplyEcScope(hinkan, detail);
                hinkan.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                hinkan.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                hinkan.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                hinkan.EcRootMaterialDescription = ResolveRootMaterialDescription(hinkan.EcScope, hinkan.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcSeizougijutsu seizougijutsu:
                ApplyIdentity(seizougijutsu, detail, lineNumberForCreate);
                ApplyEcScope(seizougijutsu, detail);
                seizougijutsu.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                seizougijutsu.EcModelCode = detail.EcModelCode ?? string.Empty;
                seizougijutsu.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                seizougijutsu.EcRootMaterialDescription = ResolveRootMaterialDescription(seizougijutsu.EcScope, seizougijutsu.EcDeptScope, detail.EcRootMaterialDescription);
                break;
            default:
                if (exec is ITaktEcDeptExecEntity identity)
                {
                    ApplyIdentity(identity, detail, lineNumberForCreate);
                }
                break;
        }
    }

    /// <summary>
    /// 同步设变单号与新建行号
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="lineNumberForCreate">新建行号</param>
    private static void ApplyIdentity(ITaktEcDeptExecEntity exec, TaktEcDetail detail, int? lineNumberForCreate)
    {
        exec.EcCode = detail.EcCode;
        exec.EcDetailId = detail.Id;
        if (lineNumberForCreate.HasValue)
        {
            exec.LineNumber = lineNumberForCreate.Value;
        }
    }

    /// <summary>
    /// 同步实施范围，并按部门写入部门实施范围（采购/受检/部管/SMT 固定按设变，制技固定按机种）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    private static void ApplyEcScope(object exec, TaktEcDetail detail)
    {
        switch (exec)
        {
            case TaktEcKoubai koubai:
                koubai.EcScope = detail.EcScope;
                koubai.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Mp, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcUkeken ukeken:
                ukeken.EcScope = detail.EcScope;
                ukeken.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Iqc, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcBukan bukan:
                bukan.EcScope = detail.EcScope;
                bukan.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Mc, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcEmtl smt:
                smt.EcScope = detail.EcScope;
                smt.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Smt, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcSeizounika seizounika:
                seizounika.EcScope = detail.EcScope;
                seizounika.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Pcba, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcSeikan seikan:
                seikan.EcScope = detail.EcScope;
                seikan.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Pmc, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcSeizouikka seizouikka:
                seizouikka.EcScope = detail.EcScope;
                seizouikka.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Assy, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcHinkan hinkan:
                hinkan.EcScope = detail.EcScope;
                hinkan.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Qa, detail.EcScope, detail.EcDeptScope);
                break;
            case TaktEcSeizougijutsu seizougijutsu:
                seizougijutsu.EcScope = detail.EcScope;
                seizougijutsu.EcDeptScope = TaktEcDeptScopeConstants.ResolveForDept(TaktEcDeptCodes.Te, detail.EcScope, detail.EcDeptScope);
                break;
        }
    }

    /// <summary>
    /// 根物料描述仅在全仕向且按成品时保留；按机种、按设变等机种组或物料组为空
    /// </summary>
    /// <param name="ecScope">实施范围</param>
    /// <param name="ecDeptScope">部门实施范围</param>
    /// <param name="description">明细根物料描述</param>
    /// <returns>写入执行行的描述</returns>
    private static string ResolveRootMaterialDescription(int ecScope, int ecDeptScope, string? description)
    {
        if (ecScope == TaktEcScopeConstants.AllDestination
            && ecDeptScope == TaktEcDeptScopeConstants.ByFinishedProduct)
        {
            return description ?? string.Empty;
        }
        return string.Empty;
    }
}
