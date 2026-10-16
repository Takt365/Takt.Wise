// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktGeneralMaterial.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt全局物料实体（通用物料主数据；组合4仅租户；多语言描述见 TaktMaterialDescription）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt全局物料实体（租户内共享；字段；多语言描述见 TaktMaterialDescription）
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）；多语言走 TaktMaterialDescription
/// </summary>
[SugarTable("takt_logistics_materials_general_material", "全局物料表")]
[SugarIndex("ix_takt_logistics_materials_general_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_general_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_general_material_type", nameof(TenantCode), OrderByType.Asc, nameof(MaterialType), OrderByType.Asc, false)]
public class TaktGeneralMaterial : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 物料编码（租户内唯一；前端表单选择编码规则后自动通过 TaktNumbering 物料编码规则生成并展示，非手输；单据类型菜单：全局物料）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    [SugarColumn(ColumnName = "complete_maintenance_status", ColumnDescription = "完整状态", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? CompleteMaintenanceStatus { get; set; }

    /// <summary>
    /// 维护状态
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_status", ColumnDescription = "维护状态", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? MaintenanceStatus { get; set; }

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    [SugarColumn(ColumnName = "client_deletion_flag", ColumnDescription = "客户级删除标记", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ClientDeletionFlag { get; set; }

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "ROH")]
    public string MaterialType { get; set; } = "ROH";

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [SugarColumn(ColumnName = "industry_sector", ColumnDescription = "行业领域", ColumnDataType = "nvarchar", Length = 1, IsNullable = false)]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "nvarchar", Length = 9, IsNullable = false)]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    [SugarColumn(ColumnName = "old_material_number", ColumnDescription = "旧物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? OldMaterialNumber { get; set; }

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "基本计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    [SugarColumn(ColumnName = "order_unit", ColumnDescription = "采购订单单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? OrderUnit { get; set; }

    /// <summary>
    /// 单据号
    /// </summary>
    [SugarColumn(ColumnName = "document_number", ColumnDescription = "单据号", ColumnDataType = "nvarchar", Length = 22, IsNullable = true)]
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "单据类型", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentType { get; set; }

    /// <summary>
    /// 单据版本
    /// </summary>
    [SugarColumn(ColumnName = "document_version", ColumnDescription = "单据版本", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DocumentVersion { get; set; }

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    [SugarColumn(ColumnName = "document_page_format", ColumnDescription = "单据页格式", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? DocumentPageFormat { get; set; }

    /// <summary>
    /// 单据更改号
    /// </summary>
    [SugarColumn(ColumnName = "document_change_number", ColumnDescription = "单据更改号", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? DocumentChangeNumber { get; set; }

    /// <summary>
    /// 单据页号
    /// </summary>
    [SugarColumn(ColumnName = "document_page_number", ColumnDescription = "单据页号", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentPageNumber { get; set; }

    /// <summary>
    /// 单据页数
    /// </summary>
    [SugarColumn(ColumnName = "document_sheet_count", ColumnDescription = "单据页数", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DocumentSheetCount { get; set; }

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    [SugarColumn(ColumnName = "production_inspection_memo", ColumnDescription = "生产/检验备忘", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductionInspectionMemo { get; set; }

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    [SugarColumn(ColumnName = "production_memo_page_format", ColumnDescription = "生产备忘页格式", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ProductionMemoPageFormat { get; set; }

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    [SugarColumn(ColumnName = "size_dimensions", ColumnDescription = "尺寸/规格", ColumnDataType = "nvarchar", Length = 32, IsNullable = true)]
    public string? SizeDimensions { get; set; }

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    [SugarColumn(ColumnName = "basic_material", ColumnDescription = "基本物料（材质）", ColumnDataType = "nvarchar", Length = 48, IsNullable = true)]
    public string? BasicMaterial { get; set; }

    /// <summary>
    /// 行业标准描述
    /// </summary>
    [SugarColumn(ColumnName = "industry_standard_description", ColumnDescription = "行业标准描述", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? IndustryStandardDescription { get; set; }

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    [SugarColumn(ColumnName = "laboratory_design_office", ColumnDescription = "实验室/设计室", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LaboratoryDesignOffice { get; set; }

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    [SugarColumn(ColumnName = "purchasing_value_key", ColumnDescription = "采购价值码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PurchasingValueKey { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    [SugarColumn(ColumnName = "gross_weight", ColumnDescription = "毛重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    [SugarColumn(ColumnName = "net_weight", ColumnDescription = "净重", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    [SugarColumn(ColumnName = "weight_unit", ColumnDescription = "重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// 体积
    /// </summary>
    [SugarColumn(ColumnName = "volume", ColumnDescription = "体积", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    [SugarColumn(ColumnName = "volume_unit", ColumnDescription = "体积单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? VolumeUnit { get; set; }

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    [SugarColumn(ColumnName = "container_requirements", ColumnDescription = "容器要求", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? ContainerRequirements { get; set; }

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    [SugarColumn(ColumnName = "storage_conditions", ColumnDescription = "仓储条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? StorageConditions { get; set; }

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    [SugarColumn(ColumnName = "temperature_conditions", ColumnDescription = "温度条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TemperatureConditions { get; set; }

    /// <summary>
    /// 低层码
    /// </summary>
    [SugarColumn(ColumnName = "low_level_code", ColumnDescription = "低层码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LowLevelCode { get; set; }

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    [SugarColumn(ColumnName = "transportation_group", ColumnDescription = "运输组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TransportationGroup { get; set; }

    /// <summary>
    /// 危险品编码
    /// </summary>
    [SugarColumn(ColumnName = "hazardous_material_number", ColumnDescription = "危险品编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? HazardousMaterialNumber { get; set; }

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    [SugarColumn(ColumnName = "division", ColumnDescription = "产品组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? Division { get; set; }

    /// <summary>
    /// 竞争对手
    /// </summary>
    [SugarColumn(ColumnName = "competitor", ColumnDescription = "竞争对手", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? Competitor { get; set; }

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    [SugarColumn(ColumnName = "european_article_number_obsolete", ColumnDescription = "欧洲商品号（旧）", ColumnDataType = "nvarchar", Length = 13, IsNullable = true)]
    public string? EuropeanArticleNumberObsolete { get; set; }

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    [SugarColumn(ColumnName = "gr_gi_slip_quantity", ColumnDescription = "收发货凭证打印数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    [SugarColumn(ColumnName = "procurement_rule", ColumnDescription = "采购规则", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ProcurementRule { get; set; }

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    [SugarColumn(ColumnName = "source_of_supply", ColumnDescription = "货源", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SourceOfSupply { get; set; }

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    [SugarColumn(ColumnName = "season_category", ColumnDescription = "季节类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SeasonCategory { get; set; }

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    [SugarColumn(ColumnName = "label_type", ColumnDescription = "标签类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? LabelType { get; set; }

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    [SugarColumn(ColumnName = "label_form", ColumnDescription = "标签格式", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? LabelForm { get; set; }

    /// <summary>
    /// 已停用字段
    /// </summary>
    [SugarColumn(ColumnName = "deactivated_field", ColumnDescription = "已停用字段", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DeactivatedField { get; set; }

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    [SugarColumn(ColumnName = "international_article_number", ColumnDescription = "国际商品编码EAN/UPC", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? InternationalArticleNumber { get; set; }

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    [SugarColumn(ColumnName = "ean_category", ColumnDescription = "EAN类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? EanCategory { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [SugarColumn(ColumnName = "length", ColumnDescription = "长度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    [SugarColumn(ColumnName = "width", ColumnDescription = "宽度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    [SugarColumn(ColumnName = "height", ColumnDescription = "高度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    [SugarColumn(ColumnName = "dimension_unit", ColumnDescription = "长宽高单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DimensionUnit { get; set; }

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    [SugarColumn(ColumnName = "product_hierarchy", ColumnDescription = "产品层次", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductHierarchy { get; set; }

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    [SugarColumn(ColumnName = "stock_transfer_net_change_costing", ColumnDescription = "库存调拨净更改成本核算", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? StockTransferNetChangeCosting { get; set; }

    /// <summary>
    /// CAD标识
    /// </summary>
    [SugarColumn(ColumnName = "cad_indicator", ColumnDescription = "CAD标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CadIndicator { get; set; }

    /// <summary>
    /// 采购QM激活
    /// </summary>
    [SugarColumn(ColumnName = "qm_in_procurement", ColumnDescription = "采购QM激活", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? QmInProcurement { get; set; }

    /// <summary>
    /// 允许包装重量
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_weight", ColumnDescription = "允许包装重量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_weight_unit", ColumnDescription = "允许包装重量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AllowedPackagingWeightUnit { get; set; }

    /// <summary>
    /// 允许包装体积
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_volume", ColumnDescription = "允许包装体积", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    [SugarColumn(ColumnName = "allowed_packaging_volume_unit", ColumnDescription = "允许包装体积单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AllowedPackagingVolumeUnit { get; set; }

    /// <summary>
    /// 超重容差
    /// </summary>
    [SugarColumn(ColumnName = "excess_weight_tolerance", ColumnDescription = "超重容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    [SugarColumn(ColumnName = "excess_volume_tolerance", ColumnDescription = "超体积容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    [SugarColumn(ColumnName = "variable_purchase_order_unit", ColumnDescription = "可变采购订单单位", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariablePurchaseOrderUnit { get; set; }

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    [SugarColumn(ColumnName = "revision_level_assigned", ColumnDescription = "已分配修订级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? RevisionLevelAssigned { get; set; }

    /// <summary>
    /// 可配置物料
    /// </summary>
    [SugarColumn(ColumnName = "configurable_material", ColumnDescription = "可配置物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ConfigurableMaterial { get; set; }

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    [SugarColumn(ColumnName = "batch_management_required", ColumnDescription = "批次管理要求", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BatchManagementRequired { get; set; }

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    [SugarColumn(ColumnName = "packaging_material_type", ColumnDescription = "包装物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PackagingMaterialType { get; set; }

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_level_by_volume", ColumnDescription = "最大装载量（体积）", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    [SugarColumn(ColumnName = "stacking_factor", ColumnDescription = "堆叠因子", ColumnDataType = "int", IsNullable = true)]
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    [SugarColumn(ColumnName = "packaging_material_group", ColumnDescription = "包装物料组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PackagingMaterialGroup { get; set; }

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    [SugarColumn(ColumnName = "authorization_group", ColumnDescription = "权限组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? AuthorizationGroup { get; set; }

    /// <summary>
    /// 有效起始日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_from_date", ColumnDescription = "有效起始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_to_date", ColumnDescription = "有效至/删除日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    [SugarColumn(ColumnName = "season_year", ColumnDescription = "季节年份", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SeasonYear { get; set; }

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    [SugarColumn(ColumnName = "price_band_category", ColumnDescription = "价格带类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? PriceBandCategory { get; set; }

    /// <summary>
    /// 空容器BOM
    /// </summary>
    [SugarColumn(ColumnName = "empties_bill_of_material", ColumnDescription = "空容器BOM", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EmptiesBillOfMaterial { get; set; }

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    [SugarColumn(ColumnName = "external_material_group", ColumnDescription = "外部物料组", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ExternalMaterialGroup { get; set; }

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_configurable_material", ColumnDescription = "跨工厂可配置物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CrossPlantConfigurableMaterial { get; set; }

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    [SugarColumn(ColumnName = "material_category", ColumnDescription = "物料类别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? MaterialCategory { get; set; }

    /// <summary>
    /// 联产品标识
    /// </summary>
    [SugarColumn(ColumnName = "co_product_indicator", ColumnDescription = "联产品标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CoProductIndicator { get; set; }

    /// <summary>
    /// 后续物料标识
    /// </summary>
    [SugarColumn(ColumnName = "follow_up_material_indicator", ColumnDescription = "后续物料标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? FollowUpMaterialIndicator { get; set; }

    /// <summary>
    /// 定价参考物料
    /// </summary>
    [SugarColumn(ColumnName = "pricing_reference_material", ColumnDescription = "定价参考物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? PricingReferenceMaterial { get; set; }

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_material_status", ColumnDescription = "跨工厂物料状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CrossPlantMaterialStatus { get; set; }

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    [SugarColumn(ColumnName = "cross_distribution_chain_status", ColumnDescription = "跨分销链物料状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CrossDistributionChainStatus { get; set; }

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    [SugarColumn(ColumnName = "cross_plant_status_valid_from", ColumnDescription = "跨工厂状态生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    [SugarColumn(ColumnName = "cross_distribution_status_valid_from", ColumnDescription = "跨分销链状态生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    [SugarColumn(ColumnName = "tax_classification", ColumnDescription = "物料税分类", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? TaxClassification { get; set; }

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "catalog_profile", ColumnDescription = "目录参数文件", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? CatalogProfile { get; set; }

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    [SugarColumn(ColumnName = "minimum_remaining_shelf_life", ColumnDescription = "最短剩余货架寿命", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    [SugarColumn(ColumnName = "total_shelf_life", ColumnDescription = "总货架寿命", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    [SugarColumn(ColumnName = "storage_percentage", ColumnDescription = "仓储百分比", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    [SugarColumn(ColumnName = "content_unit", ColumnDescription = "含量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? ContentUnit { get; set; }

    /// <summary>
    /// 净含量
    /// </summary>
    [SugarColumn(ColumnName = "net_contents", ColumnDescription = "净含量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    [SugarColumn(ColumnName = "comparison_price_unit", ColumnDescription = "比较价格单位", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    [SugarColumn(ColumnName = "labeling_material_grouping", ColumnDescription = "标签物料分组", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? LabelingMaterialGrouping { get; set; }

    /// <summary>
    /// 毛含量
    /// </summary>
    [SugarColumn(ColumnName = "gross_contents", ColumnDescription = "毛含量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    [SugarColumn(ColumnName = "quantity_conversion_method", ColumnDescription = "数量换算方法", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? QuantityConversionMethod { get; set; }

    /// <summary>
    /// 内部对象号
    /// </summary>
    [SugarColumn(ColumnName = "internal_object_number", ColumnDescription = "内部对象号", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? InternalObjectNumber { get; set; }

    /// <summary>
    /// 环境相关
    /// </summary>
    [SugarColumn(ColumnName = "environmentally_relevant", ColumnDescription = "环境相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EnvironmentallyRelevant { get; set; }

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    [SugarColumn(ColumnName = "product_allocation_procedure", ColumnDescription = "产品分配确定过程", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? ProductAllocationProcedure { get; set; }

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "variant_pricing_profile", ColumnDescription = "变式定价参数文件", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariantPricingProfile { get; set; }

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    [SugarColumn(ColumnName = "discount_in_kind", ColumnDescription = "实物折扣资格", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DiscountInKind { get; set; }

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_part_number", ColumnDescription = "制造商零件号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ManufacturerPartNumber { get; set; }

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_number", ColumnDescription = "制造商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ManufacturerNumber { get; set; }

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    [SugarColumn(ColumnName = "inventory_managed_material_number", ColumnDescription = "自有库存管理物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? InventoryManagedMaterialNumber { get; set; }

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_part_profile", ColumnDescription = "制造商零件参数文件", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ManufacturerPartProfile { get; set; }

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    [SugarColumn(ColumnName = "units_of_measure_usage", ColumnDescription = "计量单位用途", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? UnitsOfMeasureUsage { get; set; }

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    [SugarColumn(ColumnName = "season_rollout", ColumnDescription = "季节推出", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? SeasonRollout { get; set; }

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "dangerous_goods_profile", ColumnDescription = "危险品参数文件", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? DangerousGoodsProfile { get; set; }

    /// <summary>
    /// 高粘度
    /// </summary>
    [SugarColumn(ColumnName = "highly_viscous", ColumnDescription = "高粘度", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? HighlyViscous { get; set; }

    /// <summary>
    /// 散装/液体
    /// </summary>
    [SugarColumn(ColumnName = "in_bulk_liquid", ColumnDescription = "散装/液体", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? InBulkLiquid { get; set; }

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    [SugarColumn(ColumnName = "serial_number_explicitness", ColumnDescription = "序列号明确级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SerialNumberExplicitness { get; set; }

    /// <summary>
    /// 封闭包装
    /// </summary>
    [SugarColumn(ColumnName = "closed_packaging", ColumnDescription = "封闭包装", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ClosedPackaging { get; set; }

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    [SugarColumn(ColumnName = "approved_batch_record_required", ColumnDescription = "需批准批次记录", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ApprovedBatchRecordRequired { get; set; }

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    [SugarColumn(ColumnName = "effectivity_parameter_override", ColumnDescription = "有效性参数覆盖", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? EffectivityParameterOverride { get; set; }

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    [SugarColumn(ColumnName = "material_completion_level", ColumnDescription = "物料完成级别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? MaterialCompletionLevel { get; set; }

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    [SugarColumn(ColumnName = "shelf_life_period_indicator", ColumnDescription = "货架寿命期间标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ShelfLifePeriodIndicator { get; set; }

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    [SugarColumn(ColumnName = "shelf_life_rounding_rule", ColumnDescription = "货架寿命舍入规则", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ShelfLifeRoundingRule { get; set; }

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    [SugarColumn(ColumnName = "product_composition_on_packaging", ColumnDescription = "包装打印产品成分", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ProductCompositionOnPackaging { get; set; }

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    [SugarColumn(ColumnName = "general_item_category_group", ColumnDescription = "通用项目类别组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? GeneralItemCategoryGroup { get; set; }

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    [SugarColumn(ColumnName = "logistical_variants", ColumnDescription = "后勤变式通用物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? LogisticalVariants { get; set; }

    /// <summary>
    /// 物料锁定
    /// </summary>
    [SugarColumn(ColumnName = "material_locked", ColumnDescription = "物料锁定", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? MaterialLocked { get; set; }

    /// <summary>
    /// 配置管理相关
    /// </summary>
    [SugarColumn(ColumnName = "configuration_management_relevant", ColumnDescription = "配置管理相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ConfigurationManagementRelevant { get; set; }

    /// <summary>
    /// 品种清单类型
    /// </summary>
    [SugarColumn(ColumnName = "assortment_list_type", ColumnDescription = "品种清单类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AssortmentListType { get; set; }

    /// <summary>
    /// 到期日期类型
    /// </summary>
    [SugarColumn(ColumnName = "expiration_date_type", ColumnDescription = "到期日期类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ExpirationDateType { get; set; }

    /// <summary>
    /// GTIN变式
    /// </summary>
    [SugarColumn(ColumnName = "gtin_variant", ColumnDescription = "GTIN变式", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? GtinVariant { get; set; }

    /// <summary>
    /// 通用物料号
    /// </summary>
    [SugarColumn(ColumnName = "generic_material_number", ColumnDescription = "通用物料号", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? GenericMaterialNumber { get; set; }

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    [SugarColumn(ColumnName = "same_packing_reference_material", ColumnDescription = "相同包装参考物料", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SamePackingReferenceMaterial { get; set; }

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    [SugarColumn(ColumnName = "global_data_sync_relevant", ColumnDescription = "全球数据同步相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? GlobalDataSyncRelevant { get; set; }

    /// <summary>
    /// 原产地验收
    /// </summary>
    [SugarColumn(ColumnName = "acceptance_at_origin", ColumnDescription = "原产地验收", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AcceptanceAtOrigin { get; set; }

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    [SugarColumn(ColumnName = "standard_hu_type", ColumnDescription = "标准HU类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? StandardHuType { get; set; }

    /// <summary>
    /// 易被盗
    /// </summary>
    [SugarColumn(ColumnName = "pilferable", ColumnDescription = "易被盗", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? Pilferable { get; set; }

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_storage_condition", ColumnDescription = "仓储存储条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? WarehouseStorageCondition { get; set; }

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_material_group", ColumnDescription = "仓储物料组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? WarehouseMaterialGroup { get; set; }

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    [SugarColumn(ColumnName = "handling_indicator", ColumnDescription = "处理标识", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? HandlingIndicator { get; set; }

    /// <summary>
    /// 危险物质相关
    /// </summary>
    [SugarColumn(ColumnName = "hazardous_substances_relevant", ColumnDescription = "危险物质相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? HazardousSubstancesRelevant { get; set; }

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    [SugarColumn(ColumnName = "handling_unit_type", ColumnDescription = "处理单元类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? HandlingUnitType { get; set; }

    /// <summary>
    /// 可变皮重
    /// </summary>
    [SugarColumn(ColumnName = "variable_tare_weight", ColumnDescription = "可变皮重", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariableTareWeight { get; set; }

    /// <summary>
    /// 最大允许容量
    /// </summary>
    [SugarColumn(ColumnName = "maximum_allowed_capacity", ColumnDescription = "最大允许容量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    [SugarColumn(ColumnName = "overcapacity_tolerance", ColumnDescription = "超容量容差", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = true)]
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_length", ColumnDescription = "最大包装长度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_width", ColumnDescription = "最大包装宽度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_height", ColumnDescription = "最大包装高度", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = true)]
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    [SugarColumn(ColumnName = "maximum_packing_dimension_unit", ColumnDescription = "最大包装尺寸单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? MaximumPackingDimensionUnit { get; set; }

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_of_origin", ColumnDescription = "原产国", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    [SugarColumn(ColumnName = "material_freight_group", ColumnDescription = "物料运费组", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? MaterialFreightGroup { get; set; }

    /// <summary>
    /// 隔离期
    /// </summary>
    [SugarColumn(ColumnName = "quarantine_period", ColumnDescription = "隔离期", ColumnDataType = "decimal", Length = 18, DecimalDigits = 0, IsNullable = true)]
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    [SugarColumn(ColumnName = "quarantine_period_unit", ColumnDescription = "隔离期单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? QuarantinePeriodUnit { get; set; }

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    [SugarColumn(ColumnName = "quality_inspection_group", ColumnDescription = "质检组", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? QualityInspectionGroup { get; set; }

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "serial_number_profile", ColumnDescription = "序列号参数文件", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SerialNumberProfile { get; set; }

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    [SugarColumn(ColumnName = "form_name", ColumnDescription = "表单名称", ColumnDataType = "nvarchar", Length = 30, IsNullable = true)]
    public string? FormName { get; set; }

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    [SugarColumn(ColumnName = "logistics_unit_of_measure", ColumnDescription = "后勤计量单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? LogisticsUnitOfMeasure { get; set; }

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_material", ColumnDescription = "捕捞重量物料", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CatchWeightMaterial { get; set; }

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_profile", ColumnDescription = "捕捞重量参数文件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CatchWeightProfile { get; set; }

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    [SugarColumn(ColumnName = "catch_weight_tolerance_group", ColumnDescription = "捕捞重量容差组", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? CatchWeightToleranceGroup { get; set; }

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    [SugarColumn(ColumnName = "adjustment_profile", ColumnDescription = "调整参数文件", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? AdjustmentProfile { get; set; }

    /// <summary>
    /// 知识产权ID
    /// </summary>
    [SugarColumn(ColumnName = "intellectual_property_id", ColumnDescription = "知识产权ID", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? IntellectualPropertyId { get; set; }

    /// <summary>
    /// 允许变式价格
    /// </summary>
    [SugarColumn(ColumnName = "variant_price_allowed", ColumnDescription = "允许变式价格", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? VariantPriceAllowed { get; set; }

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    [SugarColumn(ColumnName = "medium", ColumnDescription = "介质", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? Medium { get; set; }

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    [SugarColumn(ColumnName = "physical_commodity", ColumnDescription = "实物商品", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? PhysicalCommodity { get; set; }

    /// <summary>
    /// 动物源
    /// </summary>
    [SugarColumn(ColumnName = "animal_origin", ColumnDescription = "动物源", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? AnimalOrigin { get; set; }

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    [SugarColumn(ColumnName = "textile_composition_function", ColumnDescription = "纺织成分功能", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? TextileCompositionFunction { get; set; }

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_structure", ColumnDescription = "细分结构", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SegmentationStructure { get; set; }

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_strategy", ColumnDescription = "细分策略", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? SegmentationStrategy { get; set; }

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_status", ColumnDescription = "细分状态", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationStatus { get; set; }

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_scope", ColumnDescription = "细分范围", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationScope { get; set; }

    /// <summary>
    /// 细分相关
    /// </summary>
    [SugarColumn(ColumnName = "segmentation_relevant", ColumnDescription = "细分相关", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SegmentationRelevant { get; set; }

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    [SugarColumn(ColumnName = "anp_code", ColumnDescription = "ANP代码", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? AnpCode { get; set; }

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute1", ColumnDescription = "时装属性1", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? FashionAttribute1 { get; set; }

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute2", ColumnDescription = "时装属性2", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? FashionAttribute2 { get; set; }

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_attribute3", ColumnDescription = "时装属性3", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? FashionAttribute3 { get; set; }

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    [SugarColumn(ColumnName = "season_usage_indicator", ColumnDescription = "季节使用标识", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SeasonUsageIndicator { get; set; }

    /// <summary>
    /// 库存季节激活
    /// </summary>
    [SugarColumn(ColumnName = "season_active_in_inventory", ColumnDescription = "库存季节激活", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? SeasonActiveInInventory { get; set; }

    /// <summary>
    /// 特性转换ID
    /// </summary>
    [SugarColumn(ColumnName = "characteristic_conversion_id", ColumnDescription = "特性转换ID", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CharacteristicConversionId { get; set; }

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    [SugarColumn(ColumnName = "packaging_code", ColumnDescription = "包装代码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PackagingCode { get; set; }

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    [SugarColumn(ColumnName = "dangerous_goods_packaging_status", ColumnDescription = "危险品包装状态", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? DangerousGoodsPackagingStatus { get; set; }

    /// <summary>
    /// 物料条件管理
    /// </summary>
    [SugarColumn(ColumnName = "material_condition_management", ColumnDescription = "物料条件管理", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? MaterialConditionManagement { get; set; }

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    [SugarColumn(ColumnName = "return_code", ColumnDescription = "退货代码", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ReturnCode { get; set; }

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    [SugarColumn(ColumnName = "return_to_logistics_level", ColumnDescription = "退回后勤级别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? ReturnToLogisticsLevel { get; set; }

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    [SugarColumn(ColumnName = "nato_item_identification_number", ColumnDescription = "NATO物料识别号", ColumnDataType = "nvarchar", Length = 9, IsNullable = true)]
    public string? NatoItemIdentificationNumber { get; set; }

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    [SugarColumn(ColumnName = "fff_class", ColumnDescription = "FFF类别", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FffClass { get; set; }

    /// <summary>
    /// 替代链编码
    /// </summary>
    [SugarColumn(ColumnName = "supersession_chain_number", ColumnDescription = "替代链编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? SupersessionChainNumber { get; set; }

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    [SugarColumn(ColumnName = "seasonal_procurement_creation_status", ColumnDescription = "季节采购创建状态", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? SeasonalProcurementCreationStatus { get; set; }

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    [SugarColumn(ColumnName = "color_characteristic_internal_number", ColumnDescription = "颜色特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ColorCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    [SugarColumn(ColumnName = "main_size_characteristic_internal_number", ColumnDescription = "主尺码特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? MainSizeCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    [SugarColumn(ColumnName = "second_size_characteristic_internal_number", ColumnDescription = "次尺码特性内部号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SecondSizeCharacteristicInternalNumber { get; set; }

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    [SugarColumn(ColumnName = "color", ColumnDescription = "颜色", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? Color { get; set; }

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    [SugarColumn(ColumnName = "main_size", ColumnDescription = "主尺码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? MainSize { get; set; }

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    [SugarColumn(ColumnName = "second_size", ColumnDescription = "次尺码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? SecondSize { get; set; }

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_characteristic_value", ColumnDescription = "评估特性值", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? EvaluationCharacteristicValue { get; set; }

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    [SugarColumn(ColumnName = "care_code", ColumnDescription = "护理代码", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? CareCode { get; set; }

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    [SugarColumn(ColumnName = "brand_id", ColumnDescription = "品牌", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BrandId { get; set; }

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code1", ColumnDescription = "纤维代码1", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode1 { get; set; }

    /// <summary>
    /// 纤维占比1
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part1", ColumnDescription = "纤维占比1", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart1 { get; set; }

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code2", ColumnDescription = "纤维代码2", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode2 { get; set; }

    /// <summary>
    /// 纤维占比2
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part2", ColumnDescription = "纤维占比2", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart2 { get; set; }

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code3", ColumnDescription = "纤维代码3", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode3 { get; set; }

    /// <summary>
    /// 纤维占比3
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part3", ColumnDescription = "纤维占比3", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart3 { get; set; }

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code4", ColumnDescription = "纤维代码4", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode4 { get; set; }

    /// <summary>
    /// 纤维占比4
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part4", ColumnDescription = "纤维占比4", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart4 { get; set; }

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    [SugarColumn(ColumnName = "fiber_code5", ColumnDescription = "纤维代码5", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberCode5 { get; set; }

    /// <summary>
    /// 纤维占比5
    /// </summary>
    [SugarColumn(ColumnName = "fiber_part5", ColumnDescription = "纤维占比5", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? FiberPart5 { get; set; }

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    [SugarColumn(ColumnName = "fashion_grade", ColumnDescription = "时装等级", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? FashionGrade { get; set; }
}

