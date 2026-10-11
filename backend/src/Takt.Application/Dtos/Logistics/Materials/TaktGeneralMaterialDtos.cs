// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktGeneralMaterialDtos.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：GeneralMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktGeneralMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// GeneralMaterial 响应 DTO
// ========================================

/// <summary>
/// Takt全局物料实体（租户内共享；字段；多语言描述见 TaktMaterialDescription） 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）；多语言走 TaktMaterialDescription
/// 对应前端 TaktGeneralMaterialDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktGeneralMaterialDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// GeneralMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GeneralMaterialId { get; set; }

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权名称（填充字段）
    /// </summary>
    public string? IntellectualPropertyName { get; set; }

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换名称（填充字段）
    /// </summary>
    public string? CharacteristicConversionName { get; set; }

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandName { get; set; }

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

}

// ========================================
// GeneralMaterial 查询 DTO
// ========================================

/// <summary>
/// GeneralMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktGeneralMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromDateStart { get; set; }

    /// <summary>
    /// 有效起始日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromDateEnd { get; set; }

    /// <summary>
    /// 有效至/删除日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToDateStart { get; set; }

    /// <summary>
    /// 有效至/删除日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToDateEnd { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期（范围查询-开始）
    /// </summary>
    public DateTime? CrossPlantStatusValidFromStart { get; set; }

    /// <summary>
    /// 跨工厂状态生效日期（范围查询-结束）
    /// </summary>
    public DateTime? CrossPlantStatusValidFromEnd { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（范围查询-开始）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFromStart { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期（范围查询-结束）
    /// </summary>
    public DateTime? CrossDistributionStatusValidFromEnd { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建GeneralMaterial DTO
// ========================================

/// <summary>
/// 创建GeneralMaterial DTO
/// </summary>
public class TaktGeneralMaterialCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    [Required(ErrorMessage = "物料编码（租户内唯一）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [Required(ErrorMessage = "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [Required(ErrorMessage = "行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）不能为空")]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [Required(ErrorMessage = "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）不能为空")]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新GeneralMaterial DTO
// ========================================

/// <summary>
/// 更新GeneralMaterial DTO
/// 继承 TaktGeneralMaterialCreateDto，添加 GeneralMaterialId 字段
/// </summary>
public class TaktGeneralMaterialUpdateDto : TaktGeneralMaterialCreateDto
{
    /// <summary>
    /// GeneralMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GeneralMaterialId { get; set; }

}

// ========================================
// GeneralMaterial 状态 DTO
// ========================================

/// <summary>
/// GeneralMaterial 状态更新 DTO
/// </summary>
public class TaktGeneralMaterialStatusDto
{
    /// <summary>
    /// GeneralMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GeneralMaterialId { get; set; }

    /// <summary>
    /// 完整状态
    /// </summary>
    [Required(ErrorMessage = "完整状态不能为空")]
    public string CompleteMaintenanceStatus { get; set; } = string.Empty;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// GeneralMaterial 导入模板行 DTO
/// </summary>
public class TaktGeneralMaterialTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// GeneralMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktGeneralMaterialImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// GeneralMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktGeneralMaterialExportDto
{
    /// <summary>
    /// GeneralMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GeneralMaterialId { get; set; }

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 完整状态
    /// </summary>
    public string? CompleteMaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 维护状态
    /// </summary>
    public string? MaintenanceStatus { get; set; } = string.Empty;

    /// <summary>
    /// 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
    /// </summary>
    public string? ClientDeletionFlag { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? OldMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? OrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 单据号
    /// </summary>
    public string? DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 单据版本
    /// </summary>
    public string? DocumentVersion { get; set; } = string.Empty;

    /// <summary>
    /// 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
    /// </summary>
    public string? DocumentPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 单据更改号
    /// </summary>
    public string? DocumentChangeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页号
    /// </summary>
    public string? DocumentPageNumber { get; set; } = string.Empty;

    /// <summary>
    /// 单据页数
    /// </summary>
    public string? DocumentSheetCount { get; set; } = string.Empty;

    /// <summary>
    /// 生产/检验备忘
    /// </summary>
    public string? ProductionInspectionMemo { get; set; } = string.Empty;

    /// <summary>
    /// 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
    /// </summary>
    public string? ProductionMemoPageFormat { get; set; } = string.Empty;

    /// <summary>
    /// 尺寸/规格
    /// </summary>
    public string? SizeDimensions { get; set; } = string.Empty;

    /// <summary>
    /// 基本物料（材质）
    /// </summary>
    public string? BasicMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 行业标准描述
    /// </summary>
    public string? IndustryStandardDescription { get; set; } = string.Empty;

    /// <summary>
    /// 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
    /// </summary>
    public string? LaboratoryDesignOffice { get; set; } = string.Empty;

    /// <summary>
    /// 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
    /// </summary>
    public string? PurchasingValueKey { get; set; } = string.Empty;

    /// <summary>
    /// 毛重
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 体积
    /// </summary>
    public decimal? Volume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
    /// </summary>
    public string? ContainerRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
    /// </summary>
    public string? StorageConditions { get; set; } = string.Empty;

    /// <summary>
    /// 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
    /// </summary>
    public string? TemperatureConditions { get; set; } = string.Empty;

    /// <summary>
    /// 低层码
    /// </summary>
    public string? LowLevelCode { get; set; } = string.Empty;

    /// <summary>
    /// 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
    /// </summary>
    public string? TransportationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 危险品编码
    /// </summary>
    public string? HazardousMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（字典 logistics_product_group；DictValue=产品组编码）
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 竞争对手
    /// </summary>
    public string? Competitor { get; set; } = string.Empty;

    /// <summary>
    /// 欧洲商品号（旧）
    /// </summary>
    public string? EuropeanArticleNumberObsolete { get; set; } = string.Empty;

    /// <summary>
    /// 收发货凭证打印数量
    /// </summary>
    public decimal? GrGiSlipQuantity { get; set; }

    /// <summary>
    /// 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
    /// </summary>
    public string? ProcurementRule { get; set; } = string.Empty;

    /// <summary>
    /// 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
    /// </summary>
    public string? SourceOfSupply { get; set; } = string.Empty;

    /// <summary>
    /// 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
    /// </summary>
    public string? SeasonCategory { get; set; } = string.Empty;

    /// <summary>
    /// 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
    /// </summary>
    public string? LabelType { get; set; } = string.Empty;

    /// <summary>
    /// 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
    /// </summary>
    public string? LabelForm { get; set; } = string.Empty;

    /// <summary>
    /// 已停用字段
    /// </summary>
    public string? DeactivatedField { get; set; } = string.Empty;

    /// <summary>
    /// 国际商品编码EAN/UPC
    /// </summary>
    public string? InternationalArticleNumber { get; set; } = string.Empty;

    /// <summary>
    /// EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
    /// </summary>
    public string? EanCategory { get; set; } = string.Empty;

    /// <summary>
    /// 长度
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? DimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
    /// </summary>
    public string? ProductHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 库存调拨净更改成本核算
    /// </summary>
    public string? StockTransferNetChangeCosting { get; set; } = string.Empty;

    /// <summary>
    /// CAD标识
    /// </summary>
    public string? CadIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 采购QM激活
    /// </summary>
    public string? QmInProcurement { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装重量
    /// </summary>
    public decimal? AllowedPackagingWeight { get; set; }

    /// <summary>
    /// 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
    /// </summary>
    public string? AllowedPackagingWeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 允许包装体积
    /// </summary>
    public decimal? AllowedPackagingVolume { get; set; }

    /// <summary>
    /// 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
    /// </summary>
    public string? AllowedPackagingVolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 超重容差
    /// </summary>
    public decimal? ExcessWeightTolerance { get; set; }

    /// <summary>
    /// 超体积容差
    /// </summary>
    public decimal? ExcessVolumeTolerance { get; set; }

    /// <summary>
    /// 可变采购订单单位
    /// </summary>
    public string? VariablePurchaseOrderUnit { get; set; } = string.Empty;

    /// <summary>
    /// 已分配修订级别
    /// </summary>
    public string? RevisionLevelAssigned { get; set; } = string.Empty;

    /// <summary>
    /// 可配置物料
    /// </summary>
    public string? ConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
    /// </summary>
    public string? BatchManagementRequired { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
    /// </summary>
    public string? PackagingMaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 最大装载量（体积）
    /// </summary>
    public decimal? MaximumLevelByVolume { get; set; }

    /// <summary>
    /// 堆叠因子
    /// </summary>
    public int? StackingFactor { get; set; }

    /// <summary>
    /// 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
    /// </summary>
    public string? PackagingMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 有效起始日期
    /// </summary>
    public DateTime? ValidFromDate { get; set; }

    /// <summary>
    /// 有效至/删除日期
    /// </summary>
    public DateTime? ValidToDate { get; set; }

    /// <summary>
    /// 季节年份（字典 logistics_season_year；DictValue=季节年份）
    /// </summary>
    public string? SeasonYear { get; set; } = string.Empty;

    /// <summary>
    /// 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
    /// </summary>
    public string? PriceBandCategory { get; set; } = string.Empty;

    /// <summary>
    /// 空容器BOM
    /// </summary>
    public string? EmptiesBillOfMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
    /// </summary>
    public string? ExternalMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂可配置物料
    /// </summary>
    public string? CrossPlantConfigurableMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
    /// </summary>
    public string? MaterialCategory { get; set; } = string.Empty;

    /// <summary>
    /// 联产品标识
    /// </summary>
    public string? CoProductIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 后续物料标识
    /// </summary>
    public string? FollowUpMaterialIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 定价参考物料
    /// </summary>
    public string? PricingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossPlantMaterialStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
    /// </summary>
    public string? CrossDistributionChainStatus { get; set; } = string.Empty;

    /// <summary>
    /// 跨工厂状态生效日期
    /// </summary>
    public DateTime? CrossPlantStatusValidFrom { get; set; }

    /// <summary>
    /// 跨分销链状态生效日期
    /// </summary>
    public DateTime? CrossDistributionStatusValidFrom { get; set; }

    /// <summary>
    /// 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatalogProfile { get; set; } = string.Empty;

    /// <summary>
    /// 最短剩余货架寿命
    /// </summary>
    public decimal? MinimumRemainingShelfLife { get; set; }

    /// <summary>
    /// 总货架寿命
    /// </summary>
    public decimal? TotalShelfLife { get; set; }

    /// <summary>
    /// 仓储百分比
    /// </summary>
    public decimal? StoragePercentage { get; set; }

    /// <summary>
    /// 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
    /// </summary>
    public string? ContentUnit { get; set; } = string.Empty;

    /// <summary>
    /// 净含量
    /// </summary>
    public decimal? NetContents { get; set; }

    /// <summary>
    /// 比较价格单位
    /// </summary>
    public decimal? ComparisonPriceUnit { get; set; }

    /// <summary>
    /// 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
    /// </summary>
    public string? LabelingMaterialGrouping { get; set; } = string.Empty;

    /// <summary>
    /// 毛含量
    /// </summary>
    public decimal? GrossContents { get; set; }

    /// <summary>
    /// 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
    /// </summary>
    public string? QuantityConversionMethod { get; set; } = string.Empty;

    /// <summary>
    /// 内部对象号
    /// </summary>
    public string? InternalObjectNumber { get; set; } = string.Empty;

    /// <summary>
    /// 环境相关
    /// </summary>
    public string? EnvironmentallyRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
    /// </summary>
    public string? ProductAllocationProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
    /// </summary>
    public string? VariantPricingProfile { get; set; } = string.Empty;

    /// <summary>
    /// 实物折扣资格
    /// </summary>
    public string? DiscountInKind { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? ManufacturerNumber { get; set; } = string.Empty;

    /// <summary>
    /// 自有库存管理物料号
    /// </summary>
    public string? InventoryManagedMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
    /// </summary>
    public string? ManufacturerPartProfile { get; set; } = string.Empty;

    /// <summary>
    /// 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
    /// </summary>
    public string? UnitsOfMeasureUsage { get; set; } = string.Empty;

    /// <summary>
    /// 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
    /// </summary>
    public string? SeasonRollout { get; set; } = string.Empty;

    /// <summary>
    /// 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
    /// </summary>
    public string? DangerousGoodsProfile { get; set; } = string.Empty;

    /// <summary>
    /// 高粘度
    /// </summary>
    public string? HighlyViscous { get; set; } = string.Empty;

    /// <summary>
    /// 散装/液体
    /// </summary>
    public string? InBulkLiquid { get; set; } = string.Empty;

    /// <summary>
    /// 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
    /// </summary>
    public string? SerialNumberExplicitness { get; set; } = string.Empty;

    /// <summary>
    /// 封闭包装
    /// </summary>
    public string? ClosedPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 需批准批次记录
    /// </summary>
    public string? ApprovedBatchRecordRequired { get; set; } = string.Empty;

    /// <summary>
    /// 有效性参数覆盖
    /// </summary>
    public string? EffectivityParameterOverride { get; set; } = string.Empty;

    /// <summary>
    /// 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
    /// </summary>
    public string? MaterialCompletionLevel { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
    /// </summary>
    public string? ShelfLifePeriodIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
    /// </summary>
    public string? ShelfLifeRoundingRule { get; set; } = string.Empty;

    /// <summary>
    /// 包装打印产品成分
    /// </summary>
    public string? ProductCompositionOnPackaging { get; set; } = string.Empty;

    /// <summary>
    /// 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
    /// </summary>
    public string? GeneralItemCategoryGroup { get; set; } = string.Empty;

    /// <summary>
    /// 后勤变式通用物料
    /// </summary>
    public string? LogisticalVariants { get; set; } = string.Empty;

    /// <summary>
    /// 物料锁定
    /// </summary>
    public string? MaterialLocked { get; set; } = string.Empty;

    /// <summary>
    /// 配置管理相关
    /// </summary>
    public string? ConfigurationManagementRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 品种清单类型
    /// </summary>
    public string? AssortmentListType { get; set; } = string.Empty;

    /// <summary>
    /// 到期日期类型
    /// </summary>
    public string? ExpirationDateType { get; set; } = string.Empty;

    /// <summary>
    /// GTIN变式
    /// </summary>
    public string? GtinVariant { get; set; } = string.Empty;

    /// <summary>
    /// 通用物料号
    /// </summary>
    public string? GenericMaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 相同包装参考物料
    /// </summary>
    public string? SamePackingReferenceMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 全球数据同步相关
    /// </summary>
    public string? GlobalDataSyncRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 原产地验收
    /// </summary>
    public string? AcceptanceAtOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
    /// </summary>
    public string? StandardHuType { get; set; } = string.Empty;

    /// <summary>
    /// 易被盗
    /// </summary>
    public string? Pilferable { get; set; } = string.Empty;

    /// <summary>
    /// 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
    /// </summary>
    public string? WarehouseStorageCondition { get; set; } = string.Empty;

    /// <summary>
    /// 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
    /// </summary>
    public string? WarehouseMaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
    /// </summary>
    public string? HandlingIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 危险物质相关
    /// </summary>
    public string? HazardousSubstancesRelevant { get; set; } = string.Empty;

    /// <summary>
    /// 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
    /// </summary>
    public string? HandlingUnitType { get; set; } = string.Empty;

    /// <summary>
    /// 可变皮重
    /// </summary>
    public string? VariableTareWeight { get; set; } = string.Empty;

    /// <summary>
    /// 最大允许容量
    /// </summary>
    public decimal? MaximumAllowedCapacity { get; set; }

    /// <summary>
    /// 超容量容差
    /// </summary>
    public decimal? OvercapacityTolerance { get; set; }

    /// <summary>
    /// 最大包装长度
    /// </summary>
    public decimal? MaximumPackingLength { get; set; }

    /// <summary>
    /// 最大包装宽度
    /// </summary>
    public decimal? MaximumPackingWidth { get; set; }

    /// <summary>
    /// 最大包装高度
    /// </summary>
    public decimal? MaximumPackingHeight { get; set; }

    /// <summary>
    /// 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
    /// </summary>
    public string? MaximumPackingDimensionUnit { get; set; } = string.Empty;

    /// <summary>
    /// 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryOfOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
    /// </summary>
    public string? MaterialFreightGroup { get; set; } = string.Empty;

    /// <summary>
    /// 隔离期
    /// </summary>
    public decimal? QuarantinePeriod { get; set; }

    /// <summary>
    /// 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
    /// </summary>
    public string? QuarantinePeriodUnit { get; set; } = string.Empty;

    /// <summary>
    /// 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
    /// </summary>
    public string? QualityInspectionGroup { get; set; } = string.Empty;

    /// <summary>
    /// 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
    /// </summary>
    public string? SerialNumberProfile { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? LogisticsUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量物料
    /// </summary>
    public string? CatchWeightMaterial { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
    /// </summary>
    public string? CatchWeightProfile { get; set; } = string.Empty;

    /// <summary>
    /// 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
    /// </summary>
    public string? CatchWeightToleranceGroup { get; set; } = string.Empty;

    /// <summary>
    /// 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
    /// </summary>
    public string? AdjustmentProfile { get; set; } = string.Empty;

    /// <summary>
    /// 知识产权ID
    /// </summary>
    public string? IntellectualPropertyId { get; set; } = string.Empty;

    /// <summary>
    /// 允许变式价格
    /// </summary>
    public string? VariantPriceAllowed { get; set; } = string.Empty;

    /// <summary>
    /// 介质（字典 logistics_medium；DictValue=介质编码）
    /// </summary>
    public string? Medium { get; set; } = string.Empty;

    /// <summary>
    /// 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
    /// </summary>
    public string? PhysicalCommodity { get; set; } = string.Empty;

    /// <summary>
    /// 动物源
    /// </summary>
    public string? AnimalOrigin { get; set; } = string.Empty;

    /// <summary>
    /// 纺织成分功能
    /// </summary>
    public string? TextileCompositionFunction { get; set; } = string.Empty;

    /// <summary>
    /// 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
    /// </summary>
    public string? SegmentationStructure { get; set; } = string.Empty;

    /// <summary>
    /// 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
    /// </summary>
    public string? SegmentationStrategy { get; set; } = string.Empty;

    /// <summary>
    /// 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
    /// </summary>
    public string? SegmentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
    /// </summary>
    public string? SegmentationScope { get; set; } = string.Empty;

    /// <summary>
    /// 细分相关
    /// </summary>
    public string? SegmentationRelevant { get; set; } = string.Empty;

    /// <summary>
    /// ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
    /// </summary>
    public string? AnpCode { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute1 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute2 { get; set; } = string.Empty;

    /// <summary>
    /// 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
    /// </summary>
    public string? FashionAttribute3 { get; set; } = string.Empty;

    /// <summary>
    /// 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
    /// </summary>
    public string? SeasonUsageIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 库存季节激活
    /// </summary>
    public string? SeasonActiveInInventory { get; set; } = string.Empty;

    /// <summary>
    /// 特性转换ID
    /// </summary>
    public string? CharacteristicConversionId { get; set; } = string.Empty;

    /// <summary>
    /// 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
    /// </summary>
    public string? PackagingCode { get; set; } = string.Empty;

    /// <summary>
    /// 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
    /// </summary>
    public string? DangerousGoodsPackagingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 物料条件管理
    /// </summary>
    public string? MaterialConditionManagement { get; set; } = string.Empty;

    /// <summary>
    /// 退货代码（字典 logistics_return_code；DictValue=退货代码）
    /// </summary>
    public string? ReturnCode { get; set; } = string.Empty;

    /// <summary>
    /// 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
    /// </summary>
    public string? ReturnToLogisticsLevel { get; set; } = string.Empty;

    /// <summary>
    /// NATO物料识别号
    /// </summary>
    public string? NatoItemIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
    /// </summary>
    public string? FffClass { get; set; } = string.Empty;

    /// <summary>
    /// 替代链编码
    /// </summary>
    public string? SupersessionChainNumber { get; set; } = string.Empty;

    /// <summary>
    /// 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
    /// </summary>
    public string? SeasonalProcurementCreationStatus { get; set; } = string.Empty;

    /// <summary>
    /// 颜色特性内部号
    /// </summary>
    public string? ColorCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码特性内部号
    /// </summary>
    public string? MainSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码特性内部号
    /// </summary>
    public string? SecondSizeCharacteristicInternalNumber { get; set; } = string.Empty;

    /// <summary>
    /// 颜色（字典 logistics_color；DictValue=颜色编码）
    /// </summary>
    public string? Color { get; set; } = string.Empty;

    /// <summary>
    /// 主尺码（字典 logistics_main_size；DictValue=尺码编码）
    /// </summary>
    public string? MainSize { get; set; } = string.Empty;

    /// <summary>
    /// 次尺码（字典 logistics_second_size；DictValue=尺码编码）
    /// </summary>
    public string? SecondSize { get; set; } = string.Empty;

    /// <summary>
    /// 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
    /// </summary>
    public string? EvaluationCharacteristicValue { get; set; } = string.Empty;

    /// <summary>
    /// 护理代码（字典 logistics_care_code；DictValue=护理代码）
    /// </summary>
    public string? CareCode { get; set; } = string.Empty;

    /// <summary>
    /// 品牌（字典 logistics_brand_id；DictValue=品牌编码）
    /// </summary>
    public string? BrandId { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比1
    /// </summary>
    public string? FiberPart1 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比2
    /// </summary>
    public string? FiberPart2 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比3
    /// </summary>
    public string? FiberPart3 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比4
    /// </summary>
    public string? FiberPart4 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
    /// </summary>
    public string? FiberCode5 { get; set; } = string.Empty;

    /// <summary>
    /// 纤维占比5
    /// </summary>
    public string? FiberPart5 { get; set; } = string.Empty;

    /// <summary>
    /// 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
    /// </summary>
    public string? FashionGrade { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
