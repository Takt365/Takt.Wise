// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperation.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS 工序排程，关联工艺路线工序与工作中心资源
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// APS 工序排程（APS_Order → Operation，关联 RoutingItem 与 WC/Resource）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_operation", "APS工序排程表")]
[SugarIndex("ix_aps_operation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_aps_operation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_operation_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApsOrderId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_operation_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktApsOperation : TaktCompanyEntityBase
{
    /// <summary>
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "aps_order_id", ColumnDescription = "APS订单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }


    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "aps_order_code", ColumnDescription = "APS订单编码", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_id", ColumnDescription = "工艺路线工序ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    [SugarColumn(ColumnName = "process_code", ColumnDescription = "工序编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    [SugarColumn(ColumnName = "process_name", ColumnDescription = "工序名称", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? ProcessName { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenterCode { get; set; }

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_resource_id", ColumnDescription = "工作中心资源ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

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
    /// 计划工时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "planned_duration_minutes", ColumnDescription = "计划工时分钟", ColumnDataType = "decimal", Length = 12, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedDurationMinutes { get; set; } = 0;

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "changeover_minutes", ColumnDescription = "换型时间分钟", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ChangeoverMinutes { get; set; } = 0;

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    [SugarColumn(ColumnName = "operation_status", ColumnDescription = "工序状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OperationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
