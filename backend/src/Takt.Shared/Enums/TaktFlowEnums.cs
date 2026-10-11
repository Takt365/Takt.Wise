// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktFlowEnums.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流引擎固定枚举（运行时态）；字典字段实体存 int
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Enums;

/// <summary>
/// 流程定义挂起状态（字典 sys_flow_suspension_state；1=激活 2=挂起）
/// </summary>
public enum TaktFlowSuspensionState
{
    /// <summary>
    /// 激活
    /// </summary>
    Active = 1,
    /// <summary>
    /// 挂起
    /// </summary>
    Suspended = 2
}

/// <summary>
/// 流程实例状态（字典 sys_flow_status；与 TaktFlowInstance.InstanceStatus 一致）
/// </summary>
public enum TaktFlowInstanceStatus
{
    /// <summary>
    /// 运行中
    /// </summary>
    Running = 0,
    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 1,
    /// <summary>
    /// 已驳回
    /// </summary>
    Rejected = 2,
    /// <summary>
    /// 已挂起
    /// </summary>
    Suspended = 3,
    /// <summary>
    /// 已终止
    /// </summary>
    Terminated = 4,
    /// <summary>
    /// 草稿
    /// </summary>
    Draft = 5
}

/// <summary>
/// 用户任务状态（字典 sys_flow_task_status）
/// </summary>
public enum TaktFlowTaskStatus
{
    /// <summary>
    /// 待办
    /// </summary>
    Pending = 0,
    /// <summary>
    /// 已完成
    /// </summary>
    Completed = 1,
    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 2
}

/// <summary>
/// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
/// </summary>
public enum TaktFlowSignType
{
    /// <summary>
    /// 或签
    /// </summary>
    Any = 1,
    /// <summary>
    /// 会签
    /// </summary>
    All = 2
}

/// <summary>
/// 流程变量类型（字典 sys_flow_variable_type）
/// </summary>
public enum TaktFlowVariableType
{
    /// <summary>
    /// 字符串
    /// </summary>
    String = 0,
    /// <summary>
    /// 长整型
    /// </summary>
    Long = 1,
    /// <summary>
    /// 双精度
    /// </summary>
    Double = 2,
    /// <summary>
    /// 布尔
    /// </summary>
    Boolean = 3,
    /// <summary>
    /// JSON
    /// </summary>
    Json = 4
}

/// <summary>
/// 流程流转动作类型（字典 sys_flow_action_type）
/// </summary>
public enum TaktFlowActionType
{
    /// <summary>
    /// 发起
    /// </summary>
    Start = 0,
    /// <summary>
    /// 通过
    /// </summary>
    Approve = 1,
    /// <summary>
    /// 驳回
    /// </summary>
    Reject = 2,
    /// <summary>
    /// 撤回
    /// </summary>
    Revoke = 3,
    /// <summary>
    /// 转办
    /// </summary>
    Transfer = 4,
    /// <summary>
    /// 加签
    /// </summary>
    AddSign = 5,
    /// <summary>
    /// 减签
    /// </summary>
    ReduceSign = 6,
    /// <summary>
    /// 挂起
    /// </summary>
    Suspend = 7,
    /// <summary>
    /// 恢复
    /// </summary>
    Resume = 8,
    /// <summary>
    /// 终止
    /// </summary>
    Terminate = 9,
    /// <summary>
    /// 抄送
    /// </summary>
    Copy = 10
}
