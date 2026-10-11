// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrder.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单实体，定义生产工单（制造订单）领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_production_order", "生产工单表")]
[SugarIndex("ix_production_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_production_order_plant_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdOrderType), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_production_order_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_production_order_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktProductionOrder : TaktCompanyEntityBase
{

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type；存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_type", ColumnDescription = "工单类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ZDTA")]
    public string ProdOrderType { get; set; } = "ZDTA";

    /// <summary>
    /// 工单号
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "工单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdOrderQty { get; set; } = 0;

    /// <summary>
    /// 已生产数量
    /// </summary>
    [SugarColumn(ColumnName = "produced_qty", ColumnDescription = "已生产数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProducedQty { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 实际开始日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_date", ColumnDescription = "实际开始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际完成日期
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_date", ColumnDescription = "实际完成日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Priority { get; set; } = 3;

    /// <summary>
 /// 工作中心（表单可选单码 TaktWorkCenters/options，故 Length=140，非单码 10）
    /// </summary>
    [SugarColumn(ColumnName = "work_center", ColumnDescription = "工作中心", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? WorkCenter { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    [SugarColumn(ColumnName = "prod_batch", ColumnDescription = "生产批次", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ProdBatch { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    [SugarColumn(ColumnName = "serial_code", ColumnDescription = "序列号", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? SerialCode { get; set; }

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? RoutingCode { get; set; }

    /// <summary>
    /// 来源计划订单 ID（选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_id", ColumnDescription = "来源计划订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源 APS 订单 ID（选项 TaktApsOrders/options，ExtValue=PlantCode 过滤，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "aps_order_id", ColumnDescription = "来源APS订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// 计划开工时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开工时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划完工时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_time", ColumnDescription = "计划完工时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
    /// </summary>
    [SugarColumn(ColumnName = "order_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int OrderStatus { get; set; } = 1;
}
