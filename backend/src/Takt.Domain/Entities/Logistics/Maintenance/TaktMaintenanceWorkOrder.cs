// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrder.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单实体，承接通知单或直接创建，执行领料、报工、完工与成本汇总
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Maintenance;

/// <summary>
/// 维护工单实体（由通知单转入或直接创建；执行领料、报工、完工；材料/人工成本汇总于头表 TotalCost 等字段）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
/// </summary>
[SugarTable("takt_logistics_maintenance_work_order", "维护工单表")]
[SugarIndex("ix_maintenance_work_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_maintenance_work_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_equipment_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EquipmentId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_notification_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceNotificationId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WorkOrderStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktMaintenanceWorkOrder : TaktApprovalEntityBase
{

    /// <summary>
    /// 维护工单号（前端表单选择编码规则后自动通过 TaktNumbering 维护工单号规则生成并展示，非手输；单据类型菜单：维护工单）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "维护工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_notification_id", ColumnDescription = "来源维护通知单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "notification_code", ColumnDescription = "来源通知单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? NotificationCode { get; set; }

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_id", ColumnDescription = "设备ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_code", ColumnDescription = "设备编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = false)]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_name", ColumnDescription = "设备名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_category", ColumnDescription = "维护类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int MaintenanceCategory { get; set; } = 2;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_type", ColumnDescription = "维护类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MaintenanceType { get; set; } = 1;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_status", ColumnDescription = "工单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int WorkOrderStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int Priority { get; set; } = 2;

    /// <summary>
    /// 工作中心
    /// </summary>
    [SugarColumn(ColumnName = "work_center", ColumnDescription = "工作中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenter { get; set; }

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    [SugarColumn(ColumnName = "assigned_technician", ColumnDescription = "指派技师", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? AssignedTechnician { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_company", ColumnDescription = "维护单位", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaintenanceCompany { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_time", ColumnDescription = "实际开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_time", ColumnDescription = "实际结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    [SugarColumn(ColumnName = "fault_description", ColumnDescription = "故障描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? FaultDescription { get; set; }

    /// <summary>
    /// 维护内容
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_content", ColumnDescription = "维护内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceContent { get; set; }

    /// <summary>
    /// 处理方案
    /// </summary>
    [SugarColumn(ColumnName = "solution", ColumnDescription = "处理方案", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Solution { get; set; }

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_id", ColumnDescription = "结算成本中心ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_code", ColumnDescription = "结算成本中心编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? CostCenterCode { get; set; }

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_id", ColumnDescription = "成本要素ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_code", ColumnDescription = "成本要素编码", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? CostElementCode { get; set; }

    /// <summary>
    /// 材料成本合计
    /// </summary>
    [SugarColumn(ColumnName = "total_material_cost", ColumnDescription = "材料成本合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalMaterialCost { get; set; } = 0;

    /// <summary>
    /// 人工成本合计
    /// </summary>
    [SugarColumn(ColumnName = "total_labor_cost", ColumnDescription = "人工成本合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalLaborCost { get; set; } = 0;

    /// <summary>
    /// 其他成本合计
    /// </summary>
    [SugarColumn(ColumnName = "total_other_cost", ColumnDescription = "其他成本合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalOtherCost { get; set; } = 0;

    /// <summary>
    /// 总成本
    /// </summary>
    [SugarColumn(ColumnName = "total_cost", ColumnDescription = "总成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalCost { get; set; } = 0;

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    [SugarColumn(ColumnName = "settlement_status", ColumnDescription = "结算状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SettlementStatus { get; set; } = 0;

    /// <summary>
    /// 结算时间
    /// </summary>
    [SugarColumn(ColumnName = "settlement_time", ColumnDescription = "结算时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    [SugarColumn(ColumnName = "completed_at", ColumnDescription = "完工时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "accepted_by_employee_id", ColumnDescription = "验收人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AcceptedByEmployeeId { get; set; }
    /// <summary>
    /// 验收人名称（冗余：按 AcceptedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "accepted_by_employee_name", ColumnDescription = "验收人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AcceptedByEmployeeName { get; set; }

    /// <summary>
    /// 验收时间
    /// </summary>
    [SugarColumn(ColumnName = "accepted_at", ColumnDescription = "验收时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_result", ColumnDescription = "维护结果", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    [SugarColumn(ColumnName = "next_maintenance_date", ColumnDescription = "下次维护日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_cycle_days", ColumnDescription = "维护周期（天）", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_images", ColumnDescription = "维护图片", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceImages { get; set; }

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_documents", ColumnDescription = "维护文档", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceDocuments { get; set; }

    /// <summary>
    /// 验收总结
    /// </summary>
    [SugarColumn(ColumnName = "accepted_summary", ColumnDescription = "验收总结", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? AcceptedSummary { get; set; }

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_history_archived", ColumnDescription = "是否已归档履历", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsHistoryArchived { get; set; } = 0;

    /// <summary>
    /// 来源维护通知单
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaintenanceNotificationId))]
    public TaktMaintenanceNotification? MaintenanceNotification { get; set; }

    /// <summary>
    /// 设备（主数据）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EquipmentId))]
    public TaktEquipment? Equipment { get; set; }

    /// <summary>
    /// 领料明细
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaintenanceWorkOrderMaterial.MaintenanceWorkOrderId))]
    public List<TaktMaintenanceWorkOrderMaterial>? Materials { get; set; }

    /// <summary>
    /// 报工明细
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktMaintenanceWorkOrderLabor.MaintenanceWorkOrderId))]
    public List<TaktMaintenanceWorkOrderLabor>? Labors { get; set; }

    /// <summary>
    /// 归档后的维护履历（一工单一条）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(Id), nameof(TaktMaintenanceHistory.MaintenanceWorkOrderId))]
    public TaktMaintenanceHistory? MaintenanceHistory { get; set; }
}
