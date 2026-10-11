// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Workflow
// 文件名称：TaktSignalRWorkflowPushModels.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流 SignalR 实时推送模型（方案变更、实例推进、待办计数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;

namespace Takt.Shared.Models.Workflow;

/// <summary>
/// 流程定义变更推送模型
/// </summary>
public class TaktSignalRFlowSchemeChangedPush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

    /// <summary>
    /// 流程标识
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（create / update / delete / status）
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// 操作人用户名
    /// </summary>
    public string? OperatorUserName { get; set; }

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangedAt { get; set; }
}

/// <summary>
/// 流程实例推进推送模型
/// </summary>
public class TaktSignalRFlowInstanceProgressedPush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 实例编码
    /// </summary>
    public string InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态（int，与 TaktFlowInstanceStatus 一致）
    /// </summary>
    public int InstanceStatus { get; set; }

    /// <summary>
    /// 动作类型（start / complete / reject / transfer 等）
    /// </summary>
    public string ActionType { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; }

    /// <summary>
    /// 发起人用户名
    /// </summary>
    public string? StartUserName { get; set; }

    /// <summary>
    /// 推进时间
    /// </summary>
    public DateTime ProgressedAt { get; set; }
}

/// <summary>
/// 待办数量推送模型
/// </summary>
public class TaktSignalRFlowTodoCountPush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 待办数量
    /// </summary>
    public int TodoCount { get; set; }

    /// <summary>
    /// 统计时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
