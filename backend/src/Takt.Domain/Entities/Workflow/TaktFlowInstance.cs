// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Workflow
// 文件名称：TaktFlowInstance.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例实体，承载运行时状态、表单数据及业务关联
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Workflow;

/// <summary>
/// 流程实例实体
/// </summary>
[SugarTable("takt_workflow_instance", "流程实例表")]
[SugarIndex("ix_flow_instance_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_instance_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(InstanceCode), OrderByType.Asc, true)]
[SugarIndex("ix_flow_instance_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_flow_instance_definition", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProcessDefinitionId), OrderByType.Asc, false)]
[SugarIndex("ix_flow_instance_starter", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartUserId), OrderByType.Asc, false)]
[SugarIndex("ix_flow_instance_business", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BusinessType), OrderByType.Asc, nameof(BusinessKey), OrderByType.Asc, false)]
public class TaktFlowInstance : TaktCompanyEntityBase
{
    /// <summary>
    /// 实例编码（对外业务单号）
    /// </summary>
    [SugarColumn(ColumnName = "instance_code", ColumnDescription = "实例编码", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string InstanceCode { get; set; } = string.Empty;
    /// <summary>
    /// 流程定义 ID（选项 TaktFlowSchemes/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "process_definition_id", ColumnDescription = "流程定义ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ProcessDefinitionId { get; set; }
    /// <summary>
    /// 流程键（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "process_key", ColumnDescription = "流程键", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string ProcessKey { get; set; } = string.Empty;
    /// <summary>
    /// 流程名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "process_name", ColumnDescription = "流程名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ProcessName { get; set; } = string.Empty;
    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    [SugarColumn(ColumnName = "definition_version", ColumnDescription = "定义版本号", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DefinitionVersion { get; set; } = 1;
    /// <summary>
    /// 申请标题
    /// </summary>
    [SugarColumn(ColumnName = "process_title", ColumnDescription = "申请标题", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ProcessTitle { get; set; }
    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    [SugarColumn(ColumnName = "current_activity_id", ColumnDescription = "当前节点ID", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? CurrentActivityId { get; set; }
    /// <summary>
    /// 当前节点名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "current_activity_name", ColumnDescription = "当前节点名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CurrentActivityName { get; set; }
    /// <summary>
    /// 发起人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "start_user_id", ColumnDescription = "发起人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long StartUserId { get; set; }
    /// <summary>
    /// 发起人姓名（冗余：按 StartUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "start_user_name", ColumnDescription = "发起人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? StartUserName { get; set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? StartTime { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 历时毫秒
    /// </summary>
    [SugarColumn(ColumnName = "duration_ms", ColumnDescription = "历时毫秒", ColumnDataType = "bigint", IsNullable = true)]
    public long? DurationMs { get; set; }
    /// <summary>
    /// 业务主键（业务单据 Id 字符串，与 BusinessType 联合回写）
    /// </summary>
    [SugarColumn(ColumnName = "business_key", ColumnDescription = "业务主键", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? BusinessKey { get; set; }
    /// <summary>
    /// 业务类型（业务模块约定标识，默认与 ProcessKey 一致）
    /// </summary>
    [SugarColumn(ColumnName = "business_type", ColumnDescription = "业务类型", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? BusinessType { get; set; }
    /// <summary>
    /// 父流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "super_instance_id", ColumnDescription = "父流程实例ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? SuperInstanceId { get; set; }
    /// <summary>
    /// 终止原因（实例终止时填写）
    /// </summary>
    [SugarColumn(ColumnName = "delete_reason", ColumnDescription = "终止原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? DeleteReason { get; set; }
    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable 表）
    /// </summary>
    [SugarColumn(ColumnName = "frm_data", ColumnDescription = "表单数据", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? FrmData { get; set; }
    /// <summary>
    /// 关联表单 ID（选项 TaktFlowForms/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "form_id", ColumnDescription = "表单ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? FormId { get; set; }
    /// <summary>
    /// 关联表单编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "form_code", ColumnDescription = "表单编码", ColumnDataType = "varchar", Length = 64, IsNullable = true)]
    public string? FormCode { get; set; }
    /// <summary>
    /// 流程设计快照（启动时复制 ProcessContent，避免定义变更影响在途实例）
    /// </summary>
    [SugarColumn(ColumnName = "process_content_snapshot", ColumnDescription = "流程设计快照", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ProcessContentSnapshot { get; set; }
    /// <summary>
    /// 实例状态（字典 sys_flow_status；0=运行中 1=已完成 2=已驳回 3=已挂起 4=已终止 5=草稿）
    /// </summary>
    [SugarColumn(ColumnName = "instance_status", ColumnDescription = "实例状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InstanceStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 流程定义
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessDefinitionId))]
    public TaktFlowScheme? ProcessDefinition { get; set; }
    /// <summary>
    /// 待办任务
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFlowTask.InstanceId))]
    public List<TaktFlowTask>? Tasks { get; set; }
    /// <summary>
    /// 流转历史
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFlowTransition.InstanceId))]
    public List<TaktFlowTransition>? HistoricActivities { get; set; }
    /// <summary>
    /// 流程变量
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFlowVariable.InstanceId))]
    public List<TaktFlowVariable>? Variables { get; set; }
    /// <summary>
    /// 加签记录
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFlowAddSign.InstanceId))]
    public List<TaktFlowAddSign>? AddSigns { get; set; }
}
