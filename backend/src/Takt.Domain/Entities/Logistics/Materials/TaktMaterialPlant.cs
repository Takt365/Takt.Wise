// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialPlant.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt工厂物料实体，定义物料领域模型（PlantCode 标识工厂维度）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt工厂物料实体
/// <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+MaterialCode。</para>
/// </summary>
[SugarTable("takt_logistics_materials_material_plant", "工厂物料表")]
[SugarIndex("ix_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_material_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_material_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktMaterialPlant : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? MaterialDescription { get; set; }
    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialSpecification { get; set; }
    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string IndustrySector { get; set; } = string.Empty;
    /// <summary>
    /// 物料层级
    /// </summary>
    [SugarColumn(ColumnName = "material_hierarchy", ColumnDescription = "物料层级", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? MaterialHierarchy { get; set; }
    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string MaterialGroup { get; set; } = string.Empty;
    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ROH")]
    public string MaterialType { get; set; } = "ROH";
    /// <summary>
    /// 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";
    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "nvarchar", Length = 3, IsNullable = false)]
    public string PurchaseGroup { get; set; } = string.Empty;
    /// <summary>
    /// 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_type", ColumnDescription = "采购类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "f")]
    public string PurchaseType { get; set; } = "f";
    /// <summary>
    /// 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
    /// </summary>
    [SugarColumn(ColumnName = "special_procurement", ColumnDescription = "特殊采购", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SpecialProcurement { get; set; } = 0;
    /// <summary>
    /// 是否散装（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_bulk", ColumnDescription = "是否散装", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBulk { get; set; } = 0;
    /// <summary>
    /// 最小起订量（基本单位数量，整数）
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
    /// 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
    /// </summary>
    [SugarColumn(ColumnName = "in_house_production_days", ColumnDescription = "自制生产天数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal InHouseProductionDays { get; set; } = 0;
    /// <summary>
    /// 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer", ColumnDescription = "制造商", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Manufacturer { get; set; }
    /// <summary>
    /// 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_code", ColumnDescription = "制造商物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ManufacturerMaterialCode { get; set; }
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string CurrencyCode { get; set; } = string.Empty;
    /// <summary>
    /// 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [SugarColumn(ColumnName = "price_control", ColumnDescription = "价格控制", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "V")]
    public string PriceControl { get; set; } = "V";
    /// <summary>
    /// 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "price_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PriceUnit { get; set; } = 1000;
    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [SugarColumn(ColumnName = "valuation", ColumnDescription = "评估类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string Valuation { get; set; } = string.Empty;
    /// <summary>
    /// 移动价格（decimal，4 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "moving_price", ColumnDescription = "移动价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal MovingPrice { get; set; } = 0;
    /// <summary>
    /// 差异码（6）
    /// </summary>
    [SugarColumn(ColumnName = "difference_code", ColumnDescription = "差异码", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? DifferenceCode { get; set; }
    /// <summary>
    /// 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "profit_center", ColumnDescription = "利润中心", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProfitCenter { get; set; } = string.Empty;
    /// <summary>
    /// 当前库存（基本单位数量，decimal，4 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "current_stock", ColumnDescription = "当前库存", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal CurrentStock { get; set; } = 0;
    /// <summary>
    /// 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "production_location", ColumnDescription = "生产仓储", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProductionLocation { get; set; } = string.Empty;
    /// <summary>
    /// 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchasing_location", ColumnDescription = "采购仓储", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PurchasingLocation { get; set; } = string.Empty;
    /// <summary>
    /// 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
    /// </summary>
    [SugarColumn(ColumnName = "storage_location", ColumnDescription = "库位", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string StorageLocation { get; set; } = string.Empty;
    /// <summary>
    /// 是否需检验（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "requires_inspection", ColumnDescription = "是否需检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RequiresInspection { get; set; } = 0;
    /// <summary>
    /// 批次标识（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_batch", ColumnDescription = "批次标识", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBatch { get; set; } = 0;
    /// <summary>
    /// 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "Z0")]
    public string DiscontinuedStatus { get; set; } = "Z0";
    /// <summary>
    /// 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "material_status", ColumnDescription = "物料状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MaterialStatus { get; set; } = 1;
}
