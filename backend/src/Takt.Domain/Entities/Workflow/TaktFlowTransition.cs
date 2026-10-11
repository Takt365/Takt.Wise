// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowTransition.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程流转历史实体，供实例详情轨迹展示
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程流转历史实体
/// </summary>
[SugarTable("takt_workflow_transition", "流程流转历史表")]
[SugarIndex("ix_flow_transition_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_transition_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_flow_transition_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktFlowTransition : TaktCompanyEntityBase
{
    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = false)]
    public long InstanceId { get; set; }
    /// <summary>
    /// 节点 ID（设计器 nodeId）
    /// </summary>
    [SugarColumn(ColumnName = "activity_id", ColumnDescription = "节点ID", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? ActivityId { get; set; }
    /// <summary>
    /// 节点名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "activity_name", ColumnDescription = "节点名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ActivityName { get; set; }
    /// <summary>
    /// 节点类型（设计器 nodeType 语义；运行时文本）
    /// </summary>
    [SugarColumn(ColumnName = "activity_type", ColumnDescription = "节点类型", ColumnDataType = "varchar", Length = 32, IsNullable = true)]
    public string? ActivityType { get; set; }
    /// <summary>
    /// 源节点 ID（设计器 nodeId）
    /// </summary>
    [SugarColumn(ColumnName = "from_node_id", ColumnDescription = "源节点ID", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? FromNodeId { get; set; }
    /// <summary>
    /// 源节点名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "from_node_name", ColumnDescription = "源节点名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FromNodeName { get; set; }
    /// <summary>
    /// 目标节点 ID（设计器 nodeId）
    /// </summary>
    [SugarColumn(ColumnName = "to_node_id", ColumnDescription = "目标节点ID", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? ToNodeId { get; set; }
    /// <summary>
    /// 目标节点名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "to_node_name", ColumnDescription = "目标节点名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ToNodeName { get; set; }
    /// <summary>
    /// 操作人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "transition_user_id", ColumnDescription = "操作人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long TransitionUserId { get; set; }
    /// <summary>
    /// 操作人姓名（冗余：按 TransitionUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "transition_user_name", ColumnDescription = "操作人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? TransitionUserName { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "transition_time", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime TransitionTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 历时毫秒
    /// </summary>
    [SugarColumn(ColumnName = "duration_ms", ColumnDescription = "历时毫秒", ColumnDataType = "bigint", IsNullable = true)]
    public long? DurationMs { get; set; }
    /// <summary>
    /// 操作意见
    /// </summary>
    [SugarColumn(ColumnName = "transition_comment", ColumnDescription = "操作意见", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? TransitionComment { get; set; }
    /// <summary>
    /// 动作类型（字典 sys_flow_action_type；0=发起 1=通过 2=驳回 3=撤回 4=转办 5=加签 6=减签 7=挂起 8=恢复 9=终止 10=抄送）
    /// </summary>
    [SugarColumn(ColumnName = "action_type", ColumnDescription = "动作类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ActionType { get; set; } = 0;
    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 所属流程实例
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
    public TaktFlowInstance? Instance { get; set; }
}
