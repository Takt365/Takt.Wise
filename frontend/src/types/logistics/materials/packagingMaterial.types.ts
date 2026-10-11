// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：packagingMaterial.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt包装物料实体
 * 对应前端 TaktPackagingMaterialDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PackagingMaterial
 * @description 对应后端 TaktPackagingMaterialDto
 */
export interface PackagingMaterial extends CompanyDtoBase {
  /**
   * PackagingMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  packagingMaterialId: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PackagingMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PackagingMaterialQuery
 * @description 对应后端 TaktPackagingMaterialQueryDto
 */
export interface PackagingMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit?: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit?: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType?: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit?: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

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
 * 创建PackagingMaterial DTO
 * 对应前端 PackagingMaterialCreate
 * @description 对应后端 TaktPackagingMaterialCreateDto
 */
export interface PackagingMaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

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
 * 更新PackagingMaterial DTO
 * 继承 TaktPackagingMaterialCreateDto，添加 PackagingMaterialId 字段
 * 对应前端 PackagingMaterialUpdate
 * @description 对应后端 TaktPackagingMaterialUpdateDto
 */
export interface PackagingMaterialUpdate extends PackagingMaterialCreate {
  /**
   * PackagingMaterialID（标识要更新的实体）
   */
  packagingMaterialId: string;

}


/**
 * PackagingMaterial 排序更新 DTO
 * 对应前端 PackagingMaterialSort
 * @description 对应后端 TaktPackagingMaterialSortDto
 */
export interface PackagingMaterialSort {
  /**
   * PackagingMaterialID
   */
  packagingMaterialId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PackagingMaterial 导入模板行 DTO
 * 对应前端 PackagingMaterialTemplate
 * @description 对应后端 TaktPackagingMaterialTemplateDto
 */
export interface PackagingMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit?: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit?: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType?: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit?: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

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
 * PackagingMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PackagingMaterialImport
 * @description 对应后端 TaktPackagingMaterialImportDto
 */
export interface PackagingMaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit?: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit?: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType?: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit?: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

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
 * PackagingMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PackagingMaterialExport
 * @description 对应后端 TaktPackagingMaterialExportDto
 */
export interface PackagingMaterialExport {
  /**
   * PackagingMaterialID
   */
  packagingMaterialId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 包装物料编码
   */
  packagingMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 海关商品编码（HS Code）
   */
  hsCode?: string;

  /**
   * 商品名称（HS Name；海关申报完整品名，可超默认短串）
   */
  hsName?: string;

  /**
   * 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
   */
  additionalCode?: string;

  /**
   * 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  originCountryRegionCode?: string;

  /**
   * 原产国/地区名称
   */
  originCountryRegionName?: string;

  /**
   * 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
   */
  destinationCountryRegionCode?: string;

  /**
   * 目的国/地区名称
   */
  destinationCountryRegionName?: string;

  /**
   * 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
   */
  regulatoryConditionCode?: string;

  /**
   * 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
   */
  tariffRateType?: string;

  /**
   * 毛重（包含包装物的总重量，单位：千克）
   */
  grossWeight?: number;

  /**
   * 净重（不含包装物的净重量，单位：千克）
   */
  netWeight?: number;

  /**
   * 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
   */
  weightUnit: string;

  /**
   * 业务量/容积（一个包装单位的体积，单位：立方米）
   */
  businessVolume?: number;

  /**
   * 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
   */
  volumeUnit: string;

  /**
   * 大小/量纲（尺寸量纲或大小规格）
   */
  sizeDimension?: string;

  /**
   * 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
   */
  packagingType: string;

  /**
   * 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
   */
  packingUnit: string;

  /**
   * 每包装数量（一个包装包含的基本单位数量）
   */
  quantityPerPacking?: number;

  /**
   * 包装规格（含多段规格说明，可超默认短串）
   */
  packagingSpec?: string;

  /**
   * 包装描述（超长说明，可超默认短串）
   */
  packagingDescription?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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

