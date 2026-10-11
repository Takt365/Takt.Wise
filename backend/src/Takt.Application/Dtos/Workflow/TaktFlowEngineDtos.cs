// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowEngineDtos.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程引擎运行时 DTO（发起/审批/待办/详情等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;

namespace Takt.Application.Dtos.Workflow;

/// <summary>
/// 发起流程请求
/// </summary>
public class TaktFlowStartDto
{
    /// <summary>
    /// 流程键
    /// </summary>
    [Required]
    public string ProcessKey { get; set; } = string.Empty;
    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 表单 JSON
    /// </summary>
    public string? FrmData { get; set; }
    /// <summary>
    /// 业务主键
    /// </summary>
    public string? BusinessKey { get; set; }
    /// <summary>
    /// 业务类型
    /// </summary>
    public string? BusinessType { get; set; }
}

/// <summary>
/// 办结任务请求
/// </summary>
public class TaktFlowCompleteTaskDto
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 实例编码
    /// </summary>
    public string? InstanceCode { get; set; }
    /// <summary>
    /// 是否通过
    /// </summary>
    public bool Approved { get; set; }
    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; }
    /// <summary>
    /// 驳回到指定节点 ID
    /// </summary>
    public string? NodeRejectStep { get; set; }
    /// <summary>
    /// 更新后的表单 JSON
    /// </summary>
    public string? FrmData { get; set; }
}

/// <summary>
/// 转办请求
/// </summary>
public class TaktFlowTransferDto
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 实例编码
    /// </summary>
    public string? InstanceCode { get; set; }
    /// <summary>
    /// 目标用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }
    /// <summary>
    /// 目标用户姓名
    /// </summary>
    public string? ToUserName { get; set; }
    /// <summary>
    /// 转办说明
    /// </summary>
    public string? Comment { get; set; }
}

/// <summary>
/// 加签人项
/// </summary>
public class TaktFlowAddApproverItemDto
{
    /// <summary>
    /// 加签人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApproverUserId { get; set; }
    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string? ApproverUserName { get; set; }
}

/// <summary>
/// 加签请求
/// </summary>
public class TaktFlowAddApproversDto
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 实例编码
    /// </summary>
    public string? InstanceCode { get; set; }
    /// <summary>
    /// 加签人列表
    /// </summary>
    public List<TaktFlowAddApproverItemDto> Approvers { get; set; } = new();
    /// <summary>
    /// 加签方式（sequential / all / one）
    /// </summary>
    public string ApproveType { get; set; } = "sequential";
    /// <summary>
    /// 完成后回到加签节点
    /// </summary>
    public bool ReturnToSignNode { get; set; }
    /// <summary>
    /// 加签原因
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// 减签请求
/// </summary>
public class TaktFlowReduceApprovalDto
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 实例编码
    /// </summary>
    public string? InstanceCode { get; set; }
    /// <summary>
    /// 加签记录 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowAddSignId { get; set; }
}

/// <summary>
/// 实例操作请求（挂起/恢复/终止/撤回/撤销审批）
/// </summary>
public class TaktFlowInstanceOperateDto
{
    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 原因说明
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// 待办/已办查询（分页与关键词见 TaktPagedQuery）
/// </summary>
public class TaktFlowTodoQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 实例编码
    /// </summary>
    public string? InstanceCode { get; set; }
    /// <summary>
    /// 流程键
    /// </summary>
    public string? ProcessKey { get; set; }
    /// <summary>
    /// 流程名称
    /// </summary>
    public string? ProcessName { get; set; }
    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 流程定义 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProcessDefinitionId { get; set; }
    /// <summary>
    /// 当前节点/任务名称
    /// </summary>
    public string? TaskName { get; set; }
    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; }
    /// <summary>
    /// 发起时间（范围起）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }
    /// <summary>
    /// 发起时间（范围止）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }
}

/// <summary>
/// 待办列表项
/// </summary>
public class TaktFlowTodoItemDto
{
    /// <summary>
    /// 流程实例 ID（对应 takt_workflow_instance 主键 Id）
    /// </summary>
    [AdaptMember("Id")]
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
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 当前节点名称（任务名称或实例当前活动名）
    /// </summary>
    public string? TaskName { get; set; }
    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; }
    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 任务 ID（适配 TaktFlowTask Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }
}

/// <summary>
/// 流转历史项（前端 history）
/// </summary>
public class TaktFlowHistoryItemDto
{
    /// <summary>
    /// 源节点名称
    /// </summary>
    public string FromNodeName { get; set; } = string.Empty;
    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string ToNodeName { get; set; } = string.Empty;
    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string TransitionUserName { get; set; } = string.Empty;
    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime TransitionTime { get; set; }
    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; }
}

/// <summary>
/// 未处理加签项
/// </summary>
public class TaktFlowPendingAddApproverDto
{
    /// <summary>
    /// 加签记录 ID（适配 TaktFlowAddSign Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowAddSignId { get; set; }
    /// <summary>
    /// 加签人姓名
    /// </summary>
    public string ApproverUserName { get; set; } = string.Empty;
}

/// <summary>
/// 流程实例详情（前端 FlowInstanceDetail）
/// </summary>
public class TaktFlowInstanceDetailDto
{
    /// <summary>
    /// 流程实例 ID（对应 takt_workflow_instance 主键 Id）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }
    /// <summary>
    /// 实例编码
    /// </summary>
    public string InstanceCode { get; set; } = string.Empty;
    /// <summary>
    /// 流程定义 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProcessDefinitionId { get; set; }
    /// <summary>
    /// 流程键
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;
    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus InstanceStatus { get; set; }
    /// <summary>
    /// 当前节点 ID
    /// </summary>
    public string? CurrentActivityId { get; set; }
    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; }
    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StartUserId { get; set; }
    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 表单 JSON
    /// </summary>
    public string? FrmData { get; set; }
    /// <summary>
    /// 流转历史
    /// </summary>
    public List<TaktFlowHistoryItemDto> History { get; set; } = new();
    /// <summary>
    /// 未处理加签
    /// </summary>
    public List<TaktFlowPendingAddApproverDto> PendingAddApprovers { get; set; } = new();
    /// <summary>
    /// 当前用户是否可审批（含减签）
    /// </summary>
    public bool CanVerify { get; set; }
}

/// <summary>
/// 我的/已办列表项（前端 FlowInstance 列表形态）
/// </summary>
public class TaktFlowInstanceListItemDto
{
    /// <summary>
    /// 流程实例 ID（对应 takt_workflow_instance 主键 Id）
    /// </summary>
    [AdaptMember("Id")]
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
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus InstanceStatus { get; set; }
    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; }
    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StartUserId { get; set; }
    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; }
    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 表单 JSON
    /// </summary>
    public string? FrmData { get; set; }
}

/// <summary>
/// 我的流程查询扩展
/// </summary>
public class TaktFlowMyInstanceQueryDto : TaktFlowInstanceQueryDto
{
    /// <summary>
    /// 仅我发起
    /// </summary>
    public bool MyStartedOnly { get; set; }
}

/// <summary>
/// 按业务表提交审批请求（配置驱动，无需业务模块专用 C# 方法）
/// </summary>
public class TaktFlowSubmitByTableDto
{
    /// <summary>
    /// 表单 RelatedTableName（审批实体物理表名）
    /// </summary>
    [Required]
    public string RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EntityId { get; set; }

    /// <summary>
    /// 可选流程键；未传时取绑定表单且已发布的方案
    /// </summary>
    public string? ProcessKey { get; set; }
}

/// <summary>
/// 可发起流程方案摘要（我的申请 / 发起弹窗）
/// </summary>
public class TaktFlowStartableSchemeDto
{
    /// <summary>
    /// 流程键
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int IsDatasource { get; set; }

    /// <summary>
    /// 关联物理表名（IsDatasource=1 时有值）
    /// </summary>
    public string? RelatedTableName { get; set; }
}

/// <summary>
/// 当前用户待办数量
/// </summary>
public class TaktFlowTodoCountDto
{
    /// <summary>
    /// 待办数量
    /// </summary>
    public int TodoCount { get; set; }
}
