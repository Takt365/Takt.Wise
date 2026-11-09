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
                pur.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                pur.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                pur.EcOldMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcOldMaterialCode]);
                pur.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                // EcOldPartBalance：采购自行维护（ntext 多行格式同生管），不从明细/生管带入（与生管去重粒不同）
                break;
            case TaktEcIqc iqc:
                ApplyIdentity(iqc, detail, lineNumberForCreate);
                ApplyEcImplScope(iqc, detail, assignControlScope, masterControlScope);
                iqc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                iqc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                iqc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                iqc.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                iqc.EcNewRequiresInspection = detail.EcNewRequiresInspection;
                break;
            case TaktEcMc mc:
                ApplyIdentity(mc, detail, lineNumberForCreate);
                ApplyEcImplScope(mc, detail, assignControlScope, masterControlScope);
                mc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mc.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                mc.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                mc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mc.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcMtl mtl:
                ApplyIdentity(mtl, detail, lineNumberForCreate);
                ApplyEcImplScope(mtl, detail, assignControlScope, masterControlScope);
                mtl.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                mtl.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                mtl.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mtl.EcNewPurchaseType = detail.EcNewPurchaseType ?? string.Empty;
                mtl.EcNewWarehouse = detail.EcNewWarehouse ?? string.Empty;
                mtl.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mtl.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mtl.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcMfg2 mfg2:
                ApplyIdentity(mfg2, detail, lineNumberForCreate);
                ApplyEcImplScope(mfg2, detail, assignControlScope, masterControlScope);
                mfg2.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mfg2.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mfg2.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mfg2.EcParentMaterialCode = detail.EcParentMaterialCode ?? string.Empty;
                mfg2.EcParentMaterialDescription = detail.EcParentMaterialDescription ?? string.Empty;
                mfg2.EcOldMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcOldMaterialCode]);
                mfg2.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcPc pc:
                ApplyIdentity(pc, detail, lineNumberForCreate);
                ApplyEcImplScope(pc, detail, assignControlScope, masterControlScope);
                pc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                pc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                pc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                pc.EcOldMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcOldMaterialCode]);
                pc.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcMfg1 mfg1:
                ApplyIdentity(mfg1, detail, lineNumberForCreate);
                ApplyEcImplScope(mfg1, detail, assignControlScope, masterControlScope);
                mfg1.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                mfg1.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                mfg1.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                mfg1.EcOldMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcOldMaterialCode]);
                mfg1.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcQc qc:
                ApplyIdentity(qc, detail, lineNumberForCreate);
                ApplyEcImplScope(qc, detail, assignControlScope, masterControlScope);
                qc.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                qc.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                qc.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
                qc.EcOldMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcOldMaterialCode]);
                qc.EcNewMaterialCodeGroup = TaktEcRootMaterialCodeGroup.JoinMaterialCodes([detail.EcNewMaterialCode]);
                break;
            case TaktEcMe me:
                ApplyIdentity(me, detail, lineNumberForCreate);
                ApplyEcImplScope(me, detail, assignControlScope, masterControlScope);
                me.DiscontinuedStatus = detail.DiscontinuedStatus ?? string.Empty;
                me.EcModelCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcModelCode]);
                me.EcRootMaterialCodeGroup = TaktEcRootMaterialCodeGroup.Join([detail.EcRootMaterialCode]);
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

}
