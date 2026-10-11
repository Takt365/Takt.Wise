// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleLine.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划 MPS 行，按 MDS 明细与时间桶记录净需求与 ATP
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 主生产计划 MPS 行（MDS 明细 + 时间桶 + ATP；下推 MRP）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_master_production_schedule_line", "主生产计划MPS行表")]
[SugarIndex("ix_mps_line_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mps_line_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_master_production_schedule_line_bucket_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterProductionScheduleId), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(BucketStart), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_master_production_schedule_line_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MasterProductionScheduleId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
public class TaktMasterProductionScheduleLine : TaktCompanyEntityBase
{
    /// <summary>
    /// MPS 头表 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "master_production_schedule_id", ColumnDescription = "MPS头表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterProductionScheduleId { get; set; }

    /// <summary>
    /// MPS 编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "mps_code", ColumnDescription = "MPS编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MpsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）
    /// </summary>
    [SugarColumn(ColumnName = "master_demand_schedule_line_id", ColumnDescription = "来源MDS行ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleLineId { get; set; }

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
    /// 毛需求数量
    /// </summary>
    [SugarColumn(ColumnName = "gross_requirement", ColumnDescription = "毛需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal GrossRequirement { get; set; } = 0;

    /// <summary>
    /// 预计入库（计划接收）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_receipts", ColumnDescription = "预计入库", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ScheduledReceipts { get; set; } = 0;

    /// <summary>
    /// 预计可用库存（期初预计库存）
    /// </summary>
    [SugarColumn(ColumnName = "projected_on_hand", ColumnDescription = "预计可用库存", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ProjectedOnHand { get; set; } = 0;

    /// <summary>
    /// 净需求数量
    /// </summary>
    [SugarColumn(ColumnName = "net_requirement", ColumnDescription = "净需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal NetRequirement { get; set; } = 0;

    /// <summary>
    /// 计划订单数量（MPS 产出，供 MRP 展开）
    /// </summary>
    [SugarColumn(ColumnName = "planned_order_quantity", ColumnDescription = "计划订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 可承诺量 ATP
    /// </summary>
    [SugarColumn(ColumnName = "atp_quantity", ColumnDescription = "可承诺量ATP", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal AtpQuantity { get; set; } = 0;

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
