// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktSourceOfSupply.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt货源清单实体，定义工厂物料与供货商的采购货源清单关系（货源清单清单，与 MRP/采购订单寻源对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt货源清单实体（公司级；工厂+物料+供货商维度的有效货源清单记录）
/// </summary>
[SugarTable("takt_logistics_procurement_source_of_supply", "货源清单清单表")]
[SugarIndex("ix_source_of_supply_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_source_of_supply_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_source_of_supply_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, nameof(ValidFrom), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_source_of_supply_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceOfSupplyCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_source_of_supply_plant_material", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_source_of_supply_supplier", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
public class TaktSourceOfSupply : TaktCompanyEntityBase
{
    /// <summary>
    /// 货源清单编码（租户+公司内唯一；业务单据号）
    /// </summary>
    [SugarColumn(ColumnName = "source_of_supply_code", ColumnDescription = "货源清单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SourceOfSupplyCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供货商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? PurchaseGroup { get; set; }
    /// <summary>
    /// 固定（字典 sys_yes_no；1=是，0=否；固定货源清单，MRP/寻源优先选用）
    /// </summary>
    [SugarColumn(ColumnName = "is_fixed", ColumnDescription = "固定", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsFixed { get; set; } = 0;
    /// <summary>
    /// 冻结（字典 sys_yes_no；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    [SugarColumn(ColumnName = "is_blocked", ColumnDescription = "冻结", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBlocked { get; set; } = 0;
    /// <summary>
    /// 采购单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_unit", ColumnDescription = "采购单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string PurchaseUnit { get; set; } = "PC";
    /// <summary>
    /// 最小起订量（采购单位数量，整数）
    /// </summary>
    [SugarColumn(ColumnName = "min_order_quantity", ColumnDescription = "最小起订量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MinOrderQuantity { get; set; } = 0;
    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    [SugarColumn(ColumnName = "rounding_value", ColumnDescription = "舍入值", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RoundingValue { get; set; } = 0;
    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    [SugarColumn(ColumnName = "planned_delivery_time_days", ColumnDescription = "计划交货时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedDeliveryTimeDays { get; set; } = 0;
    /// <summary>
    /// 框架协议号（采购合同/协议编码，可选）
    /// </summary>
    [SugarColumn(ColumnName = "agreement_number", ColumnDescription = "框架协议号", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? AgreementNumber { get; set; }
    /// <summary>
    /// 协议行号
    /// </summary>
    [SugarColumn(ColumnName = "agreement_line_number", ColumnDescription = "协议行号", ColumnDataType = "int", IsNullable = true)]
    public int? AgreementLineNumber { get; set; }
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;
    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);
    /// <summary>
    /// 排序号（回填）（越小越靠前；同物料多货源清单时的优先级）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 货源清单状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "source_status", ColumnDescription = "货源清单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SourceStatus { get; set; } = 1;
}
