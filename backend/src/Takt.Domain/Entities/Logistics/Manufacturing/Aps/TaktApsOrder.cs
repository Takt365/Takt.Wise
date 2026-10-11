// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrder.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS 排程订单，计划订单释放后的可排程订单头
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// APS 排程订单（Planned Order 释放后进入 APS 排程）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_order", "APS排程订单表")]
[SugarIndex("ix_aps_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_aps_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ApsOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_order_planned", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlannedOrderId), OrderByType.Asc, false)]
public class TaktApsOrder : TaktCompanyEntityBase
{

    /// <summary>
    /// APS 订单编码
    /// </summary>
    [SugarColumn(ColumnName = "aps_order_code", ColumnDescription = "APS订单编码", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_id", ColumnDescription = "来源计划订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_code", ColumnDescription = "来源计划订单编码", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? PlannedOrderCode { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    [SugarColumn(ColumnName = "order_quantity", ColumnDescription = "订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal OrderQuantity { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "PC")]
    public string UnitOfMeasure { get; set; } = "PC";

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? RoutingCode { get; set; }

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
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "APS订单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [SugarColumn(ColumnName = "aps_schedule_id", ColumnDescription = "APS排程批次ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS 工序排程列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktApsOperation.ApsOrderId))]
    public List<TaktApsOperation>? Operations { get; set; }
}
