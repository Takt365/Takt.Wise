// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlanItem.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购计划明细实体，MRP 计划采购行（原材料/外购件）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mrp;

/// <summary>
/// Takt采购计划明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_mrp_purchase_plan_item", "采购计划明细表")]
[SugarIndex("ix_purchase_plan_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_plan_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_purchase_plan_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePlanId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mrp_purchase_plan_item_plan_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasePlanCode), OrderByType.Asc, false)]
public class TaktPurchasePlanItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_id", ColumnDescription = "采购计划ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePlanId { get; set; }

    /// <summary>
    /// 采购计划编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_plan_code", ColumnDescription = "采购计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchasePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源生产计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_id", ColumnDescription = "来源生产计划ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 来源生产计划编码
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_code", ColumnDescription = "来源生产计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ProductionPlanCode { get; set; }

    /// <summary>
    /// 来源生产计划行号
    /// </summary>
    [SugarColumn(ColumnName = "production_plan_line_number", ColumnDescription = "来源生产计划行号", ColumnDataType = "int", IsNullable = true)]
    public int? ProductionPlanLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [SugarColumn(ColumnName = "material_requirements_planning_item_id", ColumnDescription = "来源MRP明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

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
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "plan_unit", ColumnDescription = "计划单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PlanUnit { get; set; } = "PC";

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity", ColumnDescription = "计划数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantity { get; set; } = 0;

    /// <summary>
    /// 计划到货日期
    /// </summary>
    [SugarColumn(ColumnName = "planned_arrival_date", ColumnDescription = "计划到货日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedArrivalDate { get; set; }

    /// <summary>
    /// 已转申请/订单数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转申请订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 预计单价
    /// </summary>
    [SugarColumn(ColumnName = "estimated_unit_price", ColumnDescription = "预计单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedUnitPrice { get; set; } = 0;

    /// <summary>
    /// 预计金额
    /// </summary>
    [SugarColumn(ColumnName = "estimated_amount", ColumnDescription = "预计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EstimatedAmount { get; set; } = 0;
    /// <summary>
    /// 含税价格（打印用；如 100.00）
    /// </summary>
    [SugarColumn(ColumnName = "tax_included_price", ColumnDescription = "含税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxIncludedPrice { get; set; } = 0;
    /// <summary>
    /// 未税价格（打印用；如 88.50）
    /// </summary>
    [SugarColumn(ColumnName = "untaxed_price", ColumnDescription = "未税价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal UntaxedPrice { get; set; } = 0;
    /// <summary>
    /// 税费（打印用；行税额，如 11.50）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; } = 0;

    /// <summary>
    /// 参考供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "reference_supplier_code", ColumnDescription = "参考供货商编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ReferenceSupplierCode { get; set; }

    /// <summary>
    /// 参考供货商名称
    /// </summary>
    [SugarColumn(ColumnName = "reference_supplier_name1", ColumnDescription = "参考供货商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? ReferenceSupplierName1 { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
