// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItem.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划 MRP 明细，BOM 展开后的物料需求行（自制/外购）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mrp;

/// <summary>
/// 物料需求计划 MRP 明细行（物料 + 需求日期 + 净需求数量）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mrp_material_requirements_planning_item", "物料需求计划MRP明细表")]
[SugarIndex("ix_mrp_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_mrp_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialRequirementsPlanningId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_material_requirements_planning_item_plan_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialRequirementsPlanningCode), OrderByType.Asc, false)]
public class TaktMaterialRequirementsPlanningItem : TaktCompanyEntityBase
{
    /// <summary>
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_id", ColumnDescription = "MRP头表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_code", ColumnDescription = "MRP编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

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
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ModelCode { get; set; }

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "model_name", ColumnDescription = "机种名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ModelName { get; set; }

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    [SugarColumn(ColumnName = "parent_material_code", ColumnDescription = "父项物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ParentMaterialCode { get; set; }

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    [SugarColumn(ColumnName = "bom_level", ColumnDescription = "BOM层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BomLevel { get; set; } = 1;

    /// <summary>
    /// 需求日期
    /// </summary>
    [SugarColumn(ColumnName = "requirement_date", ColumnDescription = "需求日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "plan_unit", ColumnDescription = "计划单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PlanUnit { get; set; } = "PC";

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "gross_requirement", ColumnDescription = "毛需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal GrossRequirement { get; set; } = 0;

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_receipts", ColumnDescription = "计划接收数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ScheduledReceipts { get; set; } = 0;

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    [SugarColumn(ColumnName = "on_hand_quantity", ColumnDescription = "现有库存数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal OnHandQuantity { get; set; } = 0;

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    [SugarColumn(ColumnName = "projected_on_hand", ColumnDescription = "预计可用库存", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ProjectedOnHand { get; set; } = 0;

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "net_requirement", ColumnDescription = "净需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal NetRequirement { get; set; } = 0;

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    [SugarColumn(ColumnName = "procurement_type", ColumnDescription = "供应类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcurementType { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
