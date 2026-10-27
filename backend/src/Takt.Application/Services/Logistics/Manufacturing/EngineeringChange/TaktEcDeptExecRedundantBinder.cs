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
    /// <param name="lineNumberForCreate">新建行号；更新时传 null 不改行号，也不改执行行已有控制范围</param>
    /// <param name="masterControlScope">技术课主表控制范围；仅新建执行行时按部门解析后写入</param>
    public static void Apply(object exec, TaktEcDetail detail, int? lineNumberForCreate, int masterControlScope)
    {
        ArgumentNullException.ThrowIfNull(exec);
        ArgumentNullException.ThrowIfNull(detail);
        var assignControlScope = lineNumberForCreate.HasValue;
        switch (exec)
        {
            case TaktEcPur pur:
                ApplyIdentity(pur, detail, lineNumberForCreate);
                ApplyEcImplScope(pur, detail, assignControlScope, masterControlScope);
                pur.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                pur.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                pur.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                pur.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                pur.EcRootMaterialDescription = ResolveRootMaterialDescription(pur.EcImplScope, pur.EcControlScope, detail.EcRootMaterialDescription);
                pur.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                pur.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                pur.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                // EcOldPartDisposition：采购自行维护，不从明细/生管带入（与生管去重粒不同）
                break;
            case TaktEcIqc iqc:
                ApplyIdentity(iqc, detail, lineNumberForCreate);
                ApplyEcImplScope(iqc, detail, assignControlScope, masterControlScope);
                iqc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                iqc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                iqc.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                iqc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                iqc.EcRootMaterialDescription = ResolveRootMaterialDescription(iqc.EcImplScope, iqc.EcControlScope, detail.EcRootMaterialDescription);
                iqc.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                iqc.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                iqc.EcNewRequiresInspection = detail.EcNewRequiresInspection;
                break;
            case TaktEcMc mc:
                ApplyIdentity(mc, detail, lineNumberForCreate);
                ApplyEcImplScope(mc, detail, assignControlScope, masterControlScope);
                mc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mc.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                mc.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                mc.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                mc.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                mc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mc.EcRootMaterialDescription = ResolveRootMaterialDescription(mc.EcImplScope, mc.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcMtl mtl:
                ApplyIdentity(mtl, detail, lineNumberForCreate);
                ApplyEcImplScope(mtl, detail, assignControlScope, masterControlScope);
                mtl.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                mtl.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                mtl.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mtl.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                mtl.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                mtl.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                mtl.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                mtl.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mtl.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mtl.EcRootMaterialDescription = ResolveRootMaterialDescription(mtl.EcImplScope, mtl.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcMfg2 mfg2:
                ApplyIdentity(mfg2, detail, lineNumberForCreate);
                ApplyEcImplScope(mfg2, detail, assignControlScope, masterControlScope);
                mfg2.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                mfg2.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                mfg2.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mfg2.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mfg2.EcNewMaterialCode = detail.EcNewMaterialCode ?? string.Empty;
                mfg2.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcNewMaterialCode]);
                mfg2.EcNewMaterialDescription = detail.EcNewMaterialDescription ?? string.Empty;
                mfg2.EcParentMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcParentMaterialCode]);
                mfg2.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mfg2.EcRootMaterialDescription = ResolveRootMaterialDescription(mfg2.EcImplScope, mfg2.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcPc pc:
                ApplyIdentity(pc, detail, lineNumberForCreate);
                ApplyEcImplScope(pc, detail, assignControlScope, masterControlScope);
                pc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                pc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                pc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                pc.EcRootMaterialDescription = ResolveRootMaterialDescription(pc.EcImplScope, pc.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcMfg1 mfg1:
                ApplyIdentity(mfg1, detail, lineNumberForCreate);
                ApplyEcImplScope(mfg1, detail, assignControlScope, masterControlScope);
                mfg1.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mfg1.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mfg1.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mfg1.EcRootMaterialDescription = ResolveRootMaterialDescription(mfg1.EcImplScope, mfg1.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcQc qc:
                ApplyIdentity(qc, detail, lineNumberForCreate);
                ApplyEcImplScope(qc, detail, assignControlScope, masterControlScope);
                qc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                qc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                qc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                qc.EcRootMaterialDescription = ResolveRootMaterialDescription(qc.EcImplScope, qc.EcControlScope, detail.EcRootMaterialDescription);
                break;
            case TaktEcMe me:
                ApplyIdentity(me, detail, lineNumberForCreate);
                ApplyEcImplScope(me, detail, assignControlScope, masterControlScope);
                me.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                me.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                me.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                me.EcRootMaterialDescription = ResolveRootMaterialDescription(me.EcImplScope, me.EcControlScope, detail.EcRootMaterialDescription);
                break;
            default:
                if (exec is ITaktEcDeptExecEntity identity)
                {
                    ApplyIdentity(identity, detail, lineNumberForCreate);
                }
                break;
        }
        if (assignControlScope && exec is ITaktEcDeptExecEntity keyed && !string.IsNullOrWhiteSpace(keyed.DeptCode))
        {
            keyed.EcGrainKey = TaktEcScopeRowGrain.BuildExecGrainKey(detail, keyed.DeptCode, masterControlScope);
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
        if (lineNumberForCreate.HasValue)
        {
            exec.LineNumber = lineNumberForCreate.Value;
        }
    }

    /// <summary>
    /// 同步实施范围。新建执行行时按主表控制范围解析本部门控制范围；更新不覆盖执行行已有控制范围。
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="detail">设变明细</param>
    /// <param name="assignControlScope">新建时写入控制范围</param>
    /// <param name="masterControlScope">技术课主表控制范围</param>
    private static void ApplyEcImplScope(object exec, TaktEcDetail detail, bool assignControlScope, int masterControlScope)
    {
        switch (exec)
        {
            case TaktEcPur pur:
                pur.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    pur.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Pur, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcIqc iqc:
                iqc.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    iqc.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Iqc, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcMc mc:
                mc.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    mc.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mc, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcMtl mtl:
                mtl.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    mtl.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mtl, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcMfg2 mfg2:
                mfg2.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    mfg2.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mfg2, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcPc pc:
                pc.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    pc.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Pc, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcMfg1 mfg1:
                mfg1.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    mfg1.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Mfg1, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcQc qc:
                qc.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    qc.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Qc, detail.EcImplScope, masterControlScope);
                }
                break;
            case TaktEcMe me:
                me.EcImplScope = detail.EcImplScope;
                if (assignControlScope)
                {
                    me.EcControlScope = TaktEcControlScopeConstants.ResolveForDept(TaktEcDeptCodes.Me, detail.EcImplScope, masterControlScope);
                }
                break;
        }
    }

    /// <summary>
    /// 根物料描述仅在全仕向且按成品时保留；按机种、按设变等机种组或物料组为空
    /// </summary>
    /// <param name="ecImplScope">实施范围</param>
    /// <param name="ecControlScope">部门实施范围</param>
    /// <param name="description">明细根物料描述</param>
    /// <returns>写入执行行的描述</returns>
    private static string ResolveRootMaterialDescription(int ecImplScope, int ecControlScope, string? description)
    {
        if (ecImplScope == TaktEcScopeConstants.AllDestination
            && ecControlScope == TaktEcControlScopeConstants.ByFinishedProduct)
        {
            return description ?? string.Empty;
        }
        return string.Empty;
    }
}
