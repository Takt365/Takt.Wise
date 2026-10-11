// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine.Business
// 文件名称：TaktApprovalFlowCompletedContext.cs
// 创建时间：2026-06-18
// 创建人：Takt365(Cursor AI)
// 功能描述：审批通过后业务回写上下文（与 TaktApprovalFlowDataGateway 同源租户/公司/操作人）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Workflow.FlowEngine.Business;

/// <summary>
/// 审批通过后业务回写上下文
/// </summary>
public sealed class TaktApprovalFlowCompletedContext
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public required string TenantCode { get; init; }

    /// <summary>
    /// 公司编码
    /// </summary>
    public required string CompanyCode { get; init; }

    /// <summary>
    /// 审批业务单据主键
    /// </summary>
    public long EntityId { get; init; }

    /// <summary>
    /// 操作人用户 ID（与通用审批补丁 updated_by 一致，取自流程实例发起人）
    /// </summary>
    public long OperatorUserId { get; init; }
}
