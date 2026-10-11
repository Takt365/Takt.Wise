// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktApprovalConstants.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：审批工作流常量；TaktApprovalEntityBase 及镜像业务状态字段共用字典类型码
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 审批工作流常量（与 TaktApprovalStatus、字典 sys_approval_status 对齐）
/// </summary>
public static class TaktApprovalConstants
{
    /// <summary>
    /// 审批业务状态字典类型码（TaktApprovalEntityBase.ApprovalStatus；LeaveStatus/OvertimeStatus/ExpenseStatus/CountersignStatus 等镜像字段共用）
    /// </summary>
    public const string SysApprovalStatusDictType = "sys_approval_status";
}
