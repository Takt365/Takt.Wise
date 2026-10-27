// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptEntityHelper.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行表实体读取辅助（转置/视图/批次，公共字段走 ITaktEcDeptExecEntity）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行实体读取辅助
/// </summary>
public static class TaktEcDeptEntityHelper
{
    /// <summary>
    /// 转为部门执行接口（公共字段单链路）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>部门执行接口</returns>
    /// <exception cref="ArgumentException">非部门执行实体</exception>
    private static ITaktEcDeptExecEntity AsExec(object exec)
    {
        ArgumentNullException.ThrowIfNull(exec);
        return exec as ITaktEcDeptExecEntity
            ?? throw new ArgumentException("不支持的部门执行实体类型", nameof(exec));
    }

    /// <summary>
    /// 按部门编码在列表中查找执行实体
    /// </summary>
    /// <param name="deptList">部门执行实体列表</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>执行实体</returns>
    public static object? FindByDeptCode(IReadOnlyList<object> deptList, string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        if (deptCode == TaktEcDeptCodes.Mfg2)
        {
            // 看板「制二」桶：SMT(D0625) 与 制造2课(D0620/Pcba) 按表类型择优，不依赖行上 DeptCode 字面量
            var activeElectronic = deptList.OfType<TaktEcMtl>().FirstOrDefault(x => x.IsObsolete == 0);
            if (activeElectronic != null)
            {
                return activeElectronic;
            }
            var activePcba = deptList.OfType<TaktEcMfg2>().FirstOrDefault(x => x.IsObsolete == 0);
            if (activePcba != null)
            {
                return activePcba;
            }
            return (object?)deptList.OfType<TaktEcMtl>().FirstOrDefault()
                ?? deptList.OfType<TaktEcMfg2>().FirstOrDefault();
        }
        if (deptCode == TaktEcDeptCodes.Mtl)
        {
            return deptList.OfType<TaktEcMtl>().FirstOrDefault(x => x.IsObsolete == 0)
                ?? deptList.OfType<TaktEcMtl>().FirstOrDefault();
        }
        return deptList.FirstOrDefault(x => GetDeptCode(x) == deptCode);
    }

    /// <summary>
    /// 读取部门编码
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>部门编码</returns>
    public static string GetDeptCode(object exec) => AsExec(exec).DeptCode;

    /// <summary>
    /// 读取是否实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否实施</returns>
    public static int GetIsImplemented(object exec) => AsExec(exec).IsImplemented;

    /// <summary>
    /// 写入是否实施
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isImplemented">是否实施</param>
    public static void SetIsImplemented(object exec, int isImplemented)
    {
        if (exec is TaktEcMe)
        {
            return;
        }
        AsExec(exec).IsImplemented = isImplemented;
    }

    /// <summary>
    /// 写入执行内容（仅当目标为空或强制覆盖时）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="content">执行内容</param>
    /// <param name="overwrite">是否覆盖已有内容</param>
    public static void SetExecContent(object exec, string? content, bool overwrite = false)
    {
        if (exec is TaktEcMe)
        {
            return;
        }
        var entity = AsExec(exec);
        if (overwrite || string.IsNullOrWhiteSpace(entity.ExecContent))
        {
            entity.ExecContent = content;
        }
    }

    /// <summary>
    /// 读取执行内容
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>执行内容</returns>
    public static string? GetExecContent(object exec) =>
        exec is TaktEcMe ? null : AsExec(exec).ExecContent;

    /// <summary>
    /// 读取是否作废
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否作废</returns>
    public static int GetIsObsolete(object exec) => AsExec(exec).IsObsolete;

    /// <summary>
    /// 部门执行行是否已有输入（实施=是，或执行内容非空；制技以担当/SOP日期/更新SOP为准）
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>是否有输入</returns>
    public static bool HasDeptInput(object exec)
    {
        if (GetIsObsolete(exec) == 1)
        {
            return false;
        }
        if (exec is TaktEcMe te)
        {
            return te.SopDate.HasValue
                || te.IsSopUpdated == 1
                || !string.IsNullOrWhiteSpace(te.TechLeader)
                || !string.IsNullOrWhiteSpace(te.SopLeader);
        }
        if (GetIsImplemented(exec) == 1)
        {
            return true;
        }
        return !string.IsNullOrWhiteSpace(GetExecContent(exec));
    }

    /// <summary>
    /// 解析转置单元格完成日期
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>完成日期</returns>
    public static DateTime? ResolveTransposedCompletedDate(object exec)
    {
        var deptCode = GetDeptCode(exec);
        DateTime? scheduledDate = null;
        DateTime? purchaseOrderIssueDate = null;
        DateTime? inspectionDate = null;
        DateTime? outboundDate = null;
        DateTime? productionDate = null;
        DateTime? updatedAt = null;
        var createdAt = DateTime.MinValue;
        switch (exec)
        {
            case TaktEcPc e:
                scheduledDate = e.ScheduledDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcPur e:
                purchaseOrderIssueDate = e.PurchaseOrderIssueDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcIqc e:
                inspectionDate = e.InspectionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcMc e:
                outboundDate = e.OutboundDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcMtl e:
                outboundDate = e.OutboundDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcMfg2 e:
                productionDate = e.ProductionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcMfg1 e:
                productionDate = e.ProductionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcQc e:
                inspectionDate = e.InspectionDate;
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
            case TaktEcMe e:
                updatedAt = e.UpdatedAt;
                createdAt = e.CreatedAt;
                break;
        }
        return TaktEcExecTransposedHelper.ResolveCompletedDate(
            deptCode,
            scheduledDate,
            purchaseOrderIssueDate,
            inspectionDate,
            outboundDate,
            productionDate,
            updatedAt,
            createdAt);
    }

    /// <summary>
    /// 解析品管检样日期（批次转置）
    /// </summary>
    /// <param name="qaDept">品管执行实体</param>
    /// <returns>检样日期</returns>
    public static DateTime? ResolveSampleInspectionDate(object? qaDept)
    {
        if (qaDept is not TaktEcQc qa || qa.IsImplemented != 1)
        {
            return null;
        }
        if (qa.UpdatedAt.HasValue)
        {
            return qa.UpdatedAt.Value.Date;
        }
        return qa.CreatedAt.Date;
    }

    /// <summary>
    /// 读取设变单号
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <returns>设变单号</returns>
    public static string GetEcCode(object exec) => AsExec(exec).EcCode;


    /// <summary>
    /// 判断是否匹配是否实施筛选
    /// </summary>
    /// <param name="exec">部门执行实体</param>
    /// <param name="isImplemented">是否实施</param>
    /// <returns>是否匹配</returns>
    public static bool MatchesIsImplemented(object exec, int isImplemented) =>
        GetIsImplemented(exec) == isImplemented;
}
