// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：generalMaterial.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * Takt全局物料实体（租户内共享；字段；多语言描述见 TaktMaterialDescription） 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）；多语言走 TaktMaterialDescription
 * 对应前端 TaktGeneralMaterialDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 GeneralMaterial
 * @description 对应后端 TaktGeneralMaterialDto
 */
export interface GeneralMaterial extends TenantCoreDtoBase {
  /**
   * GeneralMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  generalMaterialId: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期
   */
  validFromDate?: string;

  /**
   * 有效至/删除日期
   */
  validToDate?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 知识产权名称（填充字段）
   */
  intellectualPropertyName?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 特性转换名称（填充字段）
   */
  characteristicConversionName?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandName?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

}


/**
 * GeneralMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 GeneralMaterialQuery
 * @description 对应后端 TaktGeneralMaterialQueryDto
 */
export interface GeneralMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期（范围查询-开始）
   */
  validFromDateStart?: string;

  /**
   * 有效起始日期（范围查询-结束）
   */
  validFromDateEnd?: string;

  /**
   * 有效至/删除日期（范围查询-开始）
   */
  validToDateStart?: string;

  /**
   * 有效至/删除日期（范围查询-结束）
   */
  validToDateEnd?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期（范围查询-开始）
   */
  crossPlantStatusValidFromStart?: string;

  /**
   * 跨工厂状态生效日期（范围查询-结束）
   */
  crossPlantStatusValidFromEnd?: string;

  /**
   * 跨分销链状态生效日期（范围查询-开始）
   */
  crossDistributionStatusValidFromStart?: string;

  /**
   * 跨分销链状态生效日期（范围查询-结束）
   */
  crossDistributionStatusValidFromEnd?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建GeneralMaterial DTO
 * 对应前端 GeneralMaterialCreate
 * @description 对应后端 TaktGeneralMaterialCreateDto
 */
export interface GeneralMaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期
   */
  validFromDate?: string;

  /**
   * 有效至/删除日期
   */
  validToDate?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新GeneralMaterial DTO
 * 继承 TaktGeneralMaterialCreateDto，添加 GeneralMaterialId 字段
 * 对应前端 GeneralMaterialUpdate
 * @description 对应后端 TaktGeneralMaterialUpdateDto
 */
export interface GeneralMaterialUpdate extends GeneralMaterialCreate {
  /**
   * GeneralMaterialID（标识要更新的实体）
   */
  generalMaterialId: string;

}


/**
 * GeneralMaterial 状态更新 DTO
 * 对应前端 GeneralMaterialStatus
 * @description 对应后端 TaktGeneralMaterialStatusDto
 */
export interface GeneralMaterialStatus {
  /**
   * GeneralMaterialID
   */
  generalMaterialId: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus: string;

}


/**
 * GeneralMaterial 导入模板行 DTO
 * 对应前端 GeneralMaterialTemplate
 * @description 对应后端 TaktGeneralMaterialTemplateDto
 */
export interface GeneralMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期
   */
  validFromDate?: string;

  /**
   * 有效至/删除日期
   */
  validToDate?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * GeneralMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 GeneralMaterialImport
 * @description 对应后端 TaktGeneralMaterialImportDto
 */
export interface GeneralMaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode?: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期
   */
  validFromDate?: string;

  /**
   * 有效至/删除日期
   */
  validToDate?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * GeneralMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 GeneralMaterialExport
 * @description 对应后端 TaktGeneralMaterialExportDto
 */
export interface GeneralMaterialExport {
  /**
   * GeneralMaterialID
   */
  generalMaterialId: string;

  /**
   * 物料编码（租户内唯一）
   */
  materialCode: string;

  /**
   * 完整状态
   */
  completeMaintenanceStatus?: string;

  /**
   * 维护状态
   */
  maintenanceStatus?: string;

  /**
   * 客户级删除标记（字典 logistics_client_deletion_flag；空=未删除，X=已标记删除）
   */
  clientDeletionFlag?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 旧物料号
   */
  oldMaterialNumber?: string;

  /**
   * 基本计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 采购订单单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  orderUnit?: string;

  /**
   * 单据号
   */
  documentNumber?: string;

  /**
   * 单据类型（字典 logistics_document_type；DictValue=单据类型编码）
   */
  documentType?: string;

  /**
   * 单据版本
   */
  documentVersion?: string;

  /**
   * 单据页格式（字典 logistics_document_page_format；DictValue=页格式编码）
   */
  documentPageFormat?: string;

  /**
   * 单据更改号
   */
  documentChangeNumber?: string;

  /**
   * 单据页号
   */
  documentPageNumber?: string;

  /**
   * 单据页数
   */
  documentSheetCount?: string;

  /**
   * 生产/检验备忘
   */
  productionInspectionMemo?: string;

  /**
   * 生产备忘页格式（字典 logistics_production_memo_page_format；DictValue=页格式编码）
   */
  productionMemoPageFormat?: string;

  /**
   * 尺寸/规格
   */
  sizeDimensions?: string;

  /**
   * 基本物料（材质）
   */
  basicMaterial?: string;

  /**
   * 行业标准描述
   */
  industryStandardDescription?: string;

  /**
   * 实验室/设计室（字典 logistics_laboratory_design_office；DictValue=实验室编码）
   */
  laboratoryDesignOffice?: string;

  /**
   * 采购价值码（字典 logistics_purchasing_value_key；DictValue=采购价值码）
   */
  purchasingValueKey?: string;

  /**
   * 毛重
   */
  grossWeight?: number;

  /**
   * 净重
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  weightUnit?: string;

  /**
   * 体积
   */
  volume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  volumeUnit?: string;

  /**
   * 容器要求（字典 logistics_container_requirements；DictValue=容器要求编码）
   */
  containerRequirements?: string;

  /**
   * 仓储条件（字典 logistics_storage_conditions；DictValue=仓储条件编码）
   */
  storageConditions?: string;

  /**
   * 温度条件（字典 logistics_temperature_conditions；DictValue=温度条件编码）
   */
  temperatureConditions?: string;

  /**
   * 低层码
   */
  lowLevelCode?: string;

  /**
   * 运输组（字典 logistics_transportation_group；DictValue=运输组编码）
   */
  transportationGroup?: string;

  /**
   * 危险品编码
   */
  hazardousMaterialNumber?: string;

  /**
   * 产品组（字典 logistics_product_group；DictValue=产品组编码）
   */
  division?: string;

  /**
   * 竞争对手
   */
  competitor?: string;

  /**
   * 欧洲商品号（旧）
   */
  europeanArticleNumberObsolete?: string;

  /**
   * 收发货凭证打印数量
   */
  grGiSlipQuantity?: number;

  /**
   * 采购规则（字典 logistics_procurement_rule；DictValue=采购规则编码）
   */
  procurementRule?: string;

  /**
   * 货源（字典 logistics_source_of_supply_type；DictValue=货源标识）
   */
  sourceOfSupply?: string;

  /**
   * 季节类别（字典 logistics_season_category；DictValue=季节类别编码）
   */
  seasonCategory?: string;

  /**
   * 标签类型（字典 logistics_label_type；DictValue=标签类型编码）
   */
  labelType?: string;

  /**
   * 标签格式（字典 logistics_label_form；DictValue=标签格式编码）
   */
  labelForm?: string;

  /**
   * 已停用字段
   */
  deactivatedField?: string;

  /**
   * 国际商品编码EAN/UPC
   */
  internationalArticleNumber?: string;

  /**
   * EAN类别（字典 logistics_ean_category；DictValue=EAN类别编码）
   */
  eanCategory?: string;

  /**
   * 长度
   */
  length?: number;

  /**
   * 宽度
   */
  width?: number;

  /**
   * 高度
   */
  height?: number;

  /**
   * 长宽高单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  dimensionUnit?: string;

  /**
   * 产品层次（字典 logistics_product_hierarchy；DictValue=产品层次编码）
   */
  productHierarchy?: string;

  /**
   * 库存调拨净更改成本核算
   */
  stockTransferNetChangeCosting?: string;

  /**
   * CAD标识
   */
  cadIndicator?: string;

  /**
   * 采购QM激活
   */
  qmInProcurement?: string;

  /**
   * 允许包装重量
   */
  allowedPackagingWeight?: number;

  /**
   * 允许包装重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等）
   */
  allowedPackagingWeightUnit?: string;

  /**
   * 允许包装体积
   */
  allowedPackagingVolume?: number;

  /**
   * 允许包装体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等）
   */
  allowedPackagingVolumeUnit?: string;

  /**
   * 超重容差
   */
  excessWeightTolerance?: number;

  /**
   * 超体积容差
   */
  excessVolumeTolerance?: number;

  /**
   * 可变采购订单单位
   */
  variablePurchaseOrderUnit?: string;

  /**
   * 已分配修订级别
   */
  revisionLevelAssigned?: string;

  /**
   * 可配置物料
   */
  configurableMaterial?: string;

  /**
   * 批次管理要求（字典 sys_yes_no；0=否，1=是；同步源可能为 X/空）
   */
  batchManagementRequired?: string;

  /**
   * 包装物料类型（字典 logistics_materials_material_type；DictValue=VERP 等）
   */
  packagingMaterialType?: string;

  /**
   * 最大装载量（体积）
   */
  maximumLevelByVolume?: number;

  /**
   * 堆叠因子
   */
  stackingFactor?: number;

  /**
   * 包装物料组（字典 logistics_packaging_material_group；DictValue=包装物料组编码）
   */
  packagingMaterialGroup?: string;

  /**
   * 权限组（字典 logistics_authorization_group；DictValue=权限组编码）
   */
  authorizationGroup?: string;

  /**
   * 有效起始日期
   */
  validFromDate?: string;

  /**
   * 有效至/删除日期
   */
  validToDate?: string;

  /**
   * 季节年份（字典 logistics_season_year；DictValue=季节年份）
   */
  seasonYear?: string;

  /**
   * 价格带类别（字典 logistics_price_band_category；DictValue=价格带类别编码）
   */
  priceBandCategory?: string;

  /**
   * 空容器BOM
   */
  emptiesBillOfMaterial?: string;

  /**
   * 外部物料组（字典 logistics_external_material_group；DictValue=外部物料组编码）
   */
  externalMaterialGroup?: string;

  /**
   * 跨工厂可配置物料
   */
  crossPlantConfigurableMaterial?: string;

  /**
   * 物料类别（字典 logistics_material_category；DictValue=物料类别编码）
   */
  materialCategory?: string;

  /**
   * 联产品标识
   */
  coProductIndicator?: string;

  /**
   * 后续物料标识
   */
  followUpMaterialIndicator?: string;

  /**
   * 定价参考物料
   */
  pricingReferenceMaterial?: string;

  /**
   * 跨工厂物料状态（字典 logistics_cross_plant_material_status；DictValue=物料状态编码）
   */
  crossPlantMaterialStatus?: string;

  /**
   * 跨分销链物料状态（字典 logistics_cross_distribution_chain_status；DictValue=物料状态编码）
   */
  crossDistributionChainStatus?: string;

  /**
   * 跨工厂状态生效日期
   */
  crossPlantStatusValidFrom?: string;

  /**
   * 跨分销链状态生效日期
   */
  crossDistributionStatusValidFrom?: string;

  /**
   * 物料税分类（字典 logistics_material_tax_classification；DictValue=税分类编码）
   */
  taxClassification?: string;

  /**
   * 目录参数文件（字典 logistics_catalog_profile；DictValue=参数文件编码）
   */
  catalogProfile?: string;

  /**
   * 最短剩余货架寿命
   */
  minimumRemainingShelfLife?: number;

  /**
   * 总货架寿命
   */
  totalShelfLife?: number;

  /**
   * 仓储百分比
   */
  storagePercentage?: number;

  /**
   * 含量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/L/KG 等）
   */
  contentUnit?: string;

  /**
   * 净含量
   */
  netContents?: number;

  /**
   * 比较价格单位
   */
  comparisonPriceUnit?: number;

  /**
   * 标签物料分组（字典 logistics_labeling_material_grouping；DictValue=分组编码）
   */
  labelingMaterialGrouping?: string;

  /**
   * 毛含量
   */
  grossContents?: number;

  /**
   * 数量换算方法（字典 logistics_quantity_conversion_method；DictValue=换算方法）
   */
  quantityConversionMethod?: string;

  /**
   * 内部对象号
   */
  internalObjectNumber?: string;

  /**
   * 环境相关
   */
  environmentallyRelevant?: string;

  /**
   * 产品分配确定过程（字典 logistics_product_allocation_procedure；DictValue=过程编码）
   */
  productAllocationProcedure?: string;

  /**
   * 变式定价参数文件（字典 logistics_variant_pricing_profile；DictValue=参数文件编码）
   */
  variantPricingProfile?: string;

  /**
   * 实物折扣资格
   */
  discountInKind?: string;

  /**
   * 制造商零件号（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerPartNumber?: string;

  /**
   * 制造商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturerNumber?: string;

  /**
   * 自有库存管理物料号
   */
  inventoryManagedMaterialNumber?: string;

  /**
   * 制造商零件参数文件（字典 logistics_manufacturer_part_profile；DictValue=参数文件编码）
   */
  manufacturerPartProfile?: string;

  /**
   * 计量单位用途（字典 logistics_units_of_measure_usage；DictValue=用途编码）
   */
  unitsOfMeasureUsage?: string;

  /**
   * 季节推出（字典 logistics_season_rollout；DictValue=推出编码）
   */
  seasonRollout?: string;

  /**
   * 危险品参数文件（字典 logistics_dangerous_goods_profile；DictValue=参数文件编码）
   */
  dangerousGoodsProfile?: string;

  /**
   * 高粘度
   */
  highlyViscous?: string;

  /**
   * 散装/液体
   */
  inBulkLiquid?: string;

  /**
   * 序列号明确级别（字典 logistics_serial_number_explicitness；DictValue=级别编码）
   */
  serialNumberExplicitness?: string;

  /**
   * 封闭包装
   */
  closedPackaging?: string;

  /**
   * 需批准批次记录
   */
  approvedBatchRecordRequired?: string;

  /**
   * 有效性参数覆盖
   */
  effectivityParameterOverride?: string;

  /**
   * 物料完成级别（字典 logistics_material_completion_level；DictValue=完成级别编码）
   */
  materialCompletionLevel?: string;

  /**
   * 货架寿命期间标识（字典 logistics_shelf_life_period_indicator；DictValue=期间标识）
   */
  shelfLifePeriodIndicator?: string;

  /**
   * 货架寿命舍入规则（字典 logistics_shelf_life_rounding_rule；DictValue=舍入规则）
   */
  shelfLifeRoundingRule?: string;

  /**
   * 包装打印产品成分
   */
  productCompositionOnPackaging?: string;

  /**
   * 通用项目类别组（字典 logistics_general_item_category_group；DictValue=项目类别组编码）
   */
  generalItemCategoryGroup?: string;

  /**
   * 后勤变式通用物料
   */
  logisticalVariants?: string;

  /**
   * 物料锁定
   */
  materialLocked?: string;

  /**
   * 配置管理相关
   */
  configurationManagementRelevant?: string;

  /**
   * 品种清单类型
   */
  assortmentListType?: string;

  /**
   * 到期日期类型
   */
  expirationDateType?: string;

  /**
   * GTIN变式
   */
  gtinVariant?: string;

  /**
   * 通用物料号
   */
  genericMaterialNumber?: string;

  /**
   * 相同包装参考物料
   */
  samePackingReferenceMaterial?: string;

  /**
   * 全球数据同步相关
   */
  globalDataSyncRelevant?: string;

  /**
   * 原产地验收
   */
  acceptanceAtOrigin?: string;

  /**
   * 标准HU类型（字典 logistics_standard_hu_type；DictValue=HU类型编码）
   */
  standardHuType?: string;

  /**
   * 易被盗
   */
  pilferable?: string;

  /**
   * 仓储存储条件（字典 logistics_warehouse_storage_condition；DictValue=存储条件编码）
   */
  warehouseStorageCondition?: string;

  /**
   * 仓储物料组（字典 logistics_warehouse_material_group；DictValue=仓储物料组编码）
   */
  warehouseMaterialGroup?: string;

  /**
   * 处理标识（字典 logistics_handling_indicator；DictValue=处理标识编码）
   */
  handlingIndicator?: string;

  /**
   * 危险物质相关
   */
  hazardousSubstancesRelevant?: string;

  /**
   * 处理单元类型（字典 logistics_handling_unit_type；DictValue=HU类型编码）
   */
  handlingUnitType?: string;

  /**
   * 可变皮重
   */
  variableTareWeight?: string;

  /**
   * 最大允许容量
   */
  maximumAllowedCapacity?: number;

  /**
   * 超容量容差
   */
  overcapacityTolerance?: number;

  /**
   * 最大包装长度
   */
  maximumPackingLength?: number;

  /**
   * 最大包装宽度
   */
  maximumPackingWidth?: number;

  /**
   * 最大包装高度
   */
  maximumPackingHeight?: number;

  /**
   * 最大包装尺寸单位（字典 logistics_materials_unit_of_measure_code；DictValue=M/CM/MM 等）
   */
  maximumPackingDimensionUnit?: string;

  /**
   * 原产国（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryOfOrigin?: string;

  /**
   * 物料运费组（字典 logistics_material_freight_group；DictValue=运费组编码）
   */
  materialFreightGroup?: string;

  /**
   * 隔离期
   */
  quarantinePeriod?: number;

  /**
   * 隔离期单位（字典 logistics_materials_unit_of_measure_code；DictValue=计量单位代码）
   */
  quarantinePeriodUnit?: string;

  /**
   * 质检组（字典 logistics_quality_inspection_group；DictValue=质检组编码）
   */
  qualityInspectionGroup?: string;

  /**
   * 序列号参数文件（字典 logistics_serial_number_profile；DictValue=参数文件编码）
   */
  serialNumberProfile?: string;

  /**
   * 表单名称（字典 logistics_form_name；DictValue=表单名称编码）
   */
  formName?: string;

  /**
   * 后勤计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  logisticsUnitOfMeasure?: string;

  /**
   * 捕捞重量物料
   */
  catchWeightMaterial?: string;

  /**
   * 捕捞重量参数文件（字典 logistics_catch_weight_profile；DictValue=参数文件编码）
   */
  catchWeightProfile?: string;

  /**
   * 捕捞重量容差组（字典 logistics_catch_weight_tolerance_group；DictValue=容差组编码）
   */
  catchWeightToleranceGroup?: string;

  /**
   * 调整参数文件（字典 logistics_adjustment_profile；DictValue=参数文件编码）
   */
  adjustmentProfile?: string;

  /**
   * 知识产权ID
   */
  intellectualPropertyId?: string;

  /**
   * 允许变式价格
   */
  variantPriceAllowed?: string;

  /**
   * 介质（字典 logistics_medium；DictValue=介质编码）
   */
  medium?: string;

  /**
   * 实物商品（字典 logistics_physical_commodity；DictValue=实物商品编码）
   */
  physicalCommodity?: string;

  /**
   * 动物源
   */
  animalOrigin?: string;

  /**
   * 纺织成分功能
   */
  textileCompositionFunction?: string;

  /**
   * 细分结构（字典 logistics_segmentation_structure；DictValue=细分结构编码）
   */
  segmentationStructure?: string;

  /**
   * 细分策略（字典 logistics_segmentation_strategy；DictValue=细分策略编码）
   */
  segmentationStrategy?: string;

  /**
   * 细分状态（字典 logistics_segmentation_status；DictValue=细分状态编码）
   */
  segmentationStatus?: string;

  /**
   * 细分范围（字典 logistics_segmentation_scope；DictValue=细分范围编码）
   */
  segmentationScope?: string;

  /**
   * 细分相关
   */
  segmentationRelevant?: string;

  /**
   * ANP代码（字典 logistics_anp_code；DictValue=ANP代码）
   */
  anpCode?: string;

  /**
   * 时装属性1（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute1?: string;

  /**
   * 时装属性2（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute2?: string;

  /**
   * 时装属性3（字典 logistics_fashion_attribute；DictValue=时装属性编码）
   */
  fashionAttribute3?: string;

  /**
   * 季节使用标识（字典 logistics_season_usage_indicator；DictValue=使用标识）
   */
  seasonUsageIndicator?: string;

  /**
   * 库存季节激活
   */
  seasonActiveInInventory?: string;

  /**
   * 特性转换ID
   */
  characteristicConversionId?: string;

  /**
   * 包装代码（字典 logistics_packaging_code；DictValue=包装代码）
   */
  packagingCode?: string;

  /**
   * 危险品包装状态（字典 logistics_dangerous_goods_packaging_status；DictValue=包装状态编码）
   */
  dangerousGoodsPackagingStatus?: string;

  /**
   * 物料条件管理
   */
  materialConditionManagement?: string;

  /**
   * 退货代码（字典 logistics_return_code；DictValue=退货代码）
   */
  returnCode?: string;

  /**
   * 退回后勤级别（字典 logistics_return_to_logistics_level；DictValue=后勤级别）
   */
  returnToLogisticsLevel?: string;

  /**
   * NATO物料识别号
   */
  natoItemIdentificationNumber?: string;

  /**
   * FFF类别（字典 logistics_fff_class；DictValue=FFF类别编码）
   */
  fffClass?: string;

  /**
   * 替代链编码
   */
  supersessionChainNumber?: string;

  /**
   * 季节采购创建状态（字典 logistics_seasonal_procurement_creation_status；DictValue=创建状态编码）
   */
  seasonalProcurementCreationStatus?: string;

  /**
   * 颜色特性内部号
   */
  colorCharacteristicInternalNumber?: string;

  /**
   * 主尺码特性内部号
   */
  mainSizeCharacteristicInternalNumber?: string;

  /**
   * 次尺码特性内部号
   */
  secondSizeCharacteristicInternalNumber?: string;

  /**
   * 颜色（字典 logistics_color；DictValue=颜色编码）
   */
  color?: string;

  /**
   * 主尺码（字典 logistics_main_size；DictValue=尺码编码）
   */
  mainSize?: string;

  /**
   * 次尺码（字典 logistics_second_size；DictValue=尺码编码）
   */
  secondSize?: string;

  /**
   * 评估特性值（字典 logistics_evaluation_characteristic_value；DictValue=特性值）
   */
  evaluationCharacteristicValue?: string;

  /**
   * 护理代码（字典 logistics_care_code；DictValue=护理代码）
   */
  careCode?: string;

  /**
   * 品牌（字典 logistics_brand_id；DictValue=品牌编码）
   */
  brandId?: string;

  /**
   * 纤维代码1（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode1?: string;

  /**
   * 纤维占比1
   */
  fiberPart1?: string;

  /**
   * 纤维代码2（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode2?: string;

  /**
   * 纤维占比2
   */
  fiberPart2?: string;

  /**
   * 纤维代码3（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode3?: string;

  /**
   * 纤维占比3
   */
  fiberPart3?: string;

  /**
   * 纤维代码4（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode4?: string;

  /**
   * 纤维占比4
   */
  fiberPart4?: string;

  /**
   * 纤维代码5（字典 logistics_fiber_code；DictValue=纤维代码）
   */
  fiberCode5?: string;

  /**
   * 纤维占比5
   */
  fiberPart5?: string;

  /**
   * 时装等级（字典 logistics_fashion_grade；DictValue=时装等级编码）
   */
  fashionGrade?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

