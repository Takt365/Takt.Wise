// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowTask.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程用户任务实体，记录待办、办理人及会签配置
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程用户任务实体
/// </summary>
[SugarTable("takt_workflow_task", "流程用户任务表")]
[SugarIndex("ix_flow_task_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_task_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_flow_task_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_task_assignee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AssigneeUserId), OrderByType.Asc, nameof(TaskStatus), OrderByType.Asc, false)]
public class TaktFlowTask : TaktCompanyEntityBase
{
    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = false)]
    public long InstanceId { get; set; }
    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    [SugarColumn(ColumnName = "task_definition_key", ColumnDescription = "任务定义键", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string TaskDefinitionKey { get; set; } = string.Empty;
    /// <summary>
    /// 任务名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? TaskName { get; set; }
    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "assignee_user_id", ColumnDescription = "办理人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long AssigneeUserId { get; set; }
    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "assignee_user_name", ColumnDescription = "办理人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? AssigneeUserName { get; set; }
    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [SugarColumn(ColumnName = "owner_user_id", ColumnDescription = "任务所有者ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? OwnerUserId { get; set; }
    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "owner_user_name", ColumnDescription = "任务所有者姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? OwnerUserName { get; set; }
    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    [SugarColumn(ColumnName = "sign_type", ColumnDescription = "会签类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SignType { get; set; } = 1;
    /// <summary>
    /// 优先级
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "50")]
    public int Priority { get; set; } = 50;
    /// <summary>
    /// 到期时间
    /// </summary>
    [SugarColumn(ColumnName = "due_date", ColumnDescription = "到期时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// 认领时间
    /// </summary>
    [SugarColumn(ColumnName = "claim_time", ColumnDescription = "认领时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClaimTime { get; set; }
    /// <summary>
    /// 办结时间
    /// </summary>
    [SugarColumn(ColumnName = "completed_at", ColumnDescription = "办结时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CompletedAt { get; set; }
    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_add_sign", ColumnDescription = "是否加签", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsAddSign { get; set; }
    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "add_sign_id", ColumnDescription = "加签记录ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? AddSignId { get; set; }
    /// <summary>
    /// 审批意见
    /// </summary>
    [SugarColumn(ColumnName = "comment", ColumnDescription = "审批意见", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Comment { get; set; }
    /// <summary>
    /// 多实例序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "task_status", ColumnDescription = "任务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TaskStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 所属流程实例
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
    public TaktFlowInstance? Instance { get; set; }
}
