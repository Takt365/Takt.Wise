// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowProcessModels.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程设计树 JSON 反序列化模型（与前端 takt-flow-tree 字段名一致，反序列化用驼峰契约）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 审批人项
/// </summary>
public class TaktFlowNodeApproveItem
{
    /// <summary>
    /// 目标 ID（用户/角色/部门）
    /// </summary>
    public string? TargetId { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string? Name { get; set; }
}

/// <summary>
/// 条件项
/// </summary>
public class TaktFlowConditionItem
{
    /// <summary>
    /// 表单字段标识
    /// </summary>
    public string? FormId { get; set; }
    /// <summary>
    /// 显示名
    /// </summary>
    public string? ShowName { get; set; }
    /// <summary>
    /// 比较符（1&lt; 2&gt; 3== 4&gt;= 5&lt;=）
    /// </summary>
    public string? OptType { get; set; }
    /// <summary>
    /// 比较值
    /// </summary>
    public string? Zdy1 { get; set; }
    /// <summary>
    /// 比较值 2
    /// </summary>
    public string? Zdy2 { get; set; }
}

/// <summary>
/// 流程树节点
/// </summary>
public class TaktFlowTreeNode
{
    /// <summary>
    /// 节点 ID
    /// </summary>
    public string NodeId { get; set; } = string.Empty;
    /// <summary>
    /// 节点名称
    /// </summary>
    public string? NodeName { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string? NodeDisplayName { get; set; }
    /// <summary>
    /// 节点类型（1 发起人 2 网关 3 条件 4 审批 6 抄送 7 并行）
    /// </summary>
    public int NodeType { get; set; }
    /// <summary>
    /// 子节点
    /// </summary>
    public TaktFlowTreeNode? ChildNode { get; set; }
    /// <summary>
    /// 条件分支子节点
    /// </summary>
    public List<TaktFlowTreeNode>? ConditionNodes { get; set; }
    /// <summary>
    /// 并行分支
    /// </summary>
    public List<TaktFlowTreeNode>? ParallelNodes { get; set; }
    /// <summary>
    /// 是否动态条件网关
    /// </summary>
    public bool? IsDynamicCondition { get; set; }
    /// <summary>
    /// 是否条件并行
    /// </summary>
    public bool? IsParallel { get; set; }
    /// <summary>
    /// 审批人列表
    /// </summary>
    public List<TaktFlowNodeApproveItem>? NodeApproveList { get; set; }
    /// <summary>
    /// 审批人类型（1 指定 2 主管 3 角色 4 部门 5 发起人 6 层层）
    /// </summary>
    public int? SetType { get; set; }
    /// <summary>
    /// 会签类型（1 或签 2 会签）
    /// </summary>
    public int? SignType { get; set; }
    /// <summary>
    /// 主管层级
    /// </summary>
    public int? DirectorLevel { get; set; }
    /// <summary>
    /// 条件优先级
    /// </summary>
    public int? PriorityLevel { get; set; }
    /// <summary>
    /// 条件列表
    /// </summary>
    public List<TaktFlowConditionItem>? ConditionList { get; set; }
    /// <summary>
    /// 是否默认分支
    /// </summary>
    public int? IsDefault { get; set; }
    /// <summary>
    /// 抄送标记
    /// </summary>
    public int? CcFlag { get; set; }
}

/// <summary>
/// 解析后的审批人
/// </summary>
public class TaktFlowResolvedApprover
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    public string? UserName { get; set; }
}
