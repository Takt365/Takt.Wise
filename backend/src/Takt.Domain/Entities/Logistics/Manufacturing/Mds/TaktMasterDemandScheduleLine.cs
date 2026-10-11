// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLine.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划 MDS 行，按物料与时间桶记录独立需求
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mds;

/// <summary>
/// 主需求计划 MDS 行（物料 + 时间桶 + 需求来源）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mds_master_demand_schedule_line", "主需求计划MDS行表")]
[SugarIndex("ix_mds_line_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mds_line_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_bucket_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterDemandScheduleId), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(BucketStart), OrderByType.Asc, nameof(DemandSourceType), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterDemandScheduleId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_sales_order", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesOrderId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mds_master_demand_schedule_line_sales_plan", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesForecastId), OrderByType.Asc, false)]
public class TaktMasterDemandScheduleLine : TaktCompanyEntityBase
{
    /// <summary>
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "master_demand_schedule_id", ColumnDescription = "MDS头表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "mds_code", ColumnDescription = "MDS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    [SugarColumn(ColumnName = "demand_source_type", ColumnDescription = "需求来源", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DemandSourceType { get; set; } = 0;

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_id", ColumnDescription = "来源销售订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    [SugarColumn(ColumnName = "sales_order_line_number", ColumnDescription = "来源销售订单行号", ColumnDataType = "int", IsNullable = true)]
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_id", ColumnDescription = "来源销售预测ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_line_number", ColumnDescription = "来源销售预测行号", ColumnDataType = "int", IsNullable = true)]
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    [SugarColumn(ColumnName = "bucket_start", ColumnDescription = "时间桶开始", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    [SugarColumn(ColumnName = "bucket_end", ColumnDescription = "时间桶结束", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    [SugarColumn(ColumnName = "demand_quantity", ColumnDescription = "需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DemandQuantity { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "unit_of_measure", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "PC")]
    public string UnitOfMeasure { get; set; } = "PC";

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
