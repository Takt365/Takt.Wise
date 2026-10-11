// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistory.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护履历实体（仅由维护工单完工/关闭时归档写入，禁止业务手工新建）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Maintenance;

/// <summary>
/// 设备维护履历实体（TaktEquipment 子表；数据来源于 TaktMaintenanceWorkOrder 完工归档，只读展示）
/// </summary>
[SugarTable("takt_logistics_maintenance_history", "设备维护履历表")]
[SugarIndex("ix_maintenance_history_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_maintenance_history_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_history_work_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceWorkOrderId), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_maintenance_history_equipment_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EquipmentId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_history_maintenance_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceDate), OrderByType.Desc, false)]
public class TaktMaintenanceHistory : TaktCompanyEntityBase
{
    /// <summary>
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_work_order_id", ColumnDescription = "来源维护工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "来源维护工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_id", ColumnDescription = "设备ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "equipment_code", ColumnDescription = "设备编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = false)]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_type", ColumnDescription = "维护类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_category", ColumnDescription = "维护类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int MaintenanceCategory { get; set; } = 2;

    /// <summary>
    /// 维护单位
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_company", ColumnDescription = "维护单位", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaintenanceCompany { get; set; }

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_technician", ColumnDescription = "维护技师", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? MaintenanceTechnician { get; set; }

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_date", ColumnDescription = "维护日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_start_time", ColumnDescription = "维护开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_end_time", ColumnDescription = "维护结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_content", ColumnDescription = "维护内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceContent { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    [SugarColumn(ColumnName = "fault_description", ColumnDescription = "故障描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? FaultDescription { get; set; }

    /// <summary>
    /// 处理方案
    /// </summary>
    [SugarColumn(ColumnName = "solution", ColumnDescription = "处理方案", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? Solution { get; set; }

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    [SugarColumn(ColumnName = "used_parts", ColumnDescription = "使用配件", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? UsedParts { get; set; }

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_cost", ColumnDescription = "维护费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaintenanceCost { get; set; } = 0;

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_result", ColumnDescription = "维护结果", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_status", ColumnDescription = "履历状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "2")]
    public int MaintenanceStatus { get; set; } = 2;

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
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_documents", ColumnDescription = "维护文档", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceDocuments { get; set; }

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_images", ColumnDescription = "维护图片", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaintenanceImages { get; set; }

    /// <summary>
    /// 验收总结
    /// </summary>
    [SugarColumn(ColumnName = "accepted_summary", ColumnDescription = "验收总结", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? AcceptedSummary { get; set; }

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
    /// 归档时间
    /// </summary>
    [SugarColumn(ColumnName = "archived_at", ColumnDescription = "归档时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ArchivedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 设备（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EquipmentId))]
    public TaktEquipment? Equipment { get; set; }

    /// <summary>
    /// 来源维护工单
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaintenanceWorkOrderId))]
    public TaktMaintenanceWorkOrder? MaintenanceWorkOrder { get; set; }
}
