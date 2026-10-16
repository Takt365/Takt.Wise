// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotification.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护通知单实体，记录设备异常发现、故障描述，可转维护工单
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Maintenance;

/// <summary>
/// 维护通知单实体（流程起点：发现异常 → 开通知单 → 转/建维护工单）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
/// </summary>
[SugarTable("takt_logistics_maintenance_notification", "维护通知单表")]
[SugarIndex("ix_maintenance_notification_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_maintenance_notification_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_notification_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(NotificationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_maintenance_notification_equipment_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EquipmentId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_notification_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(NotificationStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_notification_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktMaintenanceNotification : TaktApprovalEntityBase
{

    /// <summary>
    /// 通知单号（前端表单选择编码规则后自动通过 TaktNumbering 通知单号规则生成并展示，非手输；单据类型菜单：维护通知）
    /// </summary>
    [SugarColumn(ColumnName = "notification_code", ColumnDescription = "通知单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_id", ColumnDescription = "设备ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int Priority { get; set; } = 2;

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "notification_status", ColumnDescription = "通知单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NotificationStatus { get; set; } = 0;

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    [SugarColumn(ColumnName = "fault_description", ColumnDescription = "异常描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    [SugarColumn(ColumnName = "discovered_at", ColumnDescription = "发现时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime DiscoveredAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 故障开始时间
    /// </summary>
    [SugarColumn(ColumnName = "breakdown_start_time", ColumnDescription = "故障开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    [SugarColumn(ColumnName = "breakdown_end_time", ColumnDescription = "故障结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "reported_by_employee_id", ColumnDescription = "报告人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ReportedByEmployeeId { get; set; }
    /// <summary>
    /// 报告人名称（冗余：按 ReportedByEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "reported_by_employee_name", ColumnDescription = "报告人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ReportedByEmployeeName { get; set; }

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_id", ColumnDescription = "责任成本中心ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_code", ColumnDescription = "责任成本中心编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? CostCenterCode { get; set; }

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_work_order_id", ColumnDescription = "关联维护工单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_work_order_code", ColumnDescription = "关联维护工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? MaintenanceWorkOrderCode { get; set; }

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    [SugarColumn(ColumnName = "notification_images", ColumnDescription = "通知图片", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? NotificationImages { get; set; }

    /// <summary>
    /// 设备（主数据）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EquipmentId))]
    public TaktEquipment? Equipment { get; set; }

    /// <summary>
    /// 关联维护工单
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaintenanceWorkOrderId))]
    public TaktMaintenanceWorkOrder? MaintenanceWorkOrder { get; set; }
}
