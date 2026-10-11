// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialPlant.types.ts
// 创建时间：2026-08-05
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
 * Takt工厂物料实体
 * 对应前端 TaktMaterialPlantDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialPlant
 * @description 对应后端 TaktMaterialPlantDto
 */
export interface MaterialPlant extends CompanyDtoBase {

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector?: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType?: string;

  /**
   * 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
   */
  purchaseType?: string;

  /**
   * 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
   */
  specialProcurement?: number;

  /**
   * 是否散装（字典 sys_yes_no；0=否，1=是）
   */
  isBulk?: number;

  /**
   * 最小起订量（基本单位数量，整数）
   */
  minOrderQuantity?: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue?: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
   */
  inHouseProductionDays?: number;

  /**
   * 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerMaterialCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl?: string;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 移动价格（decimal，4 位小数）
   */
  movingPrice?: number;

  /**
   * 差异码（6）
   */
  differenceCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenter?: string;

  /**
   * 当前库存（基本单位数量，decimal，4 位小数）
   */
  currentStock?: number;

  /**
   * 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  productionLocation?: string;

  /**
   * 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  purchasingLocation?: string;

  /**
   * 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
   */
  storageLocation?: string;

  /**
   * 是否需检验（字典 sys_yes_no；0=否，1=是）
   */
  requiresInspection?: number;

  /**
   * 批次标识（字典 sys_yes_no；0=否，1=是）
   */
  isBatch?: number;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus?: string;

  /**
   * 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  materialStatus?: number;

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
 * MaterialPlant 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialPlantExport
 * @description 对应后端 TaktMaterialPlantExportDto
 */
export interface MaterialPlantExport {
  /**
   * MaterialPlantID
   */
  materialPlantId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述（冗余：按 MaterialCode、PlantCode→CultureCode 取 TaktMaterialDescription.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 行业领域（字典 logistics_materials_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
   */
  industrySector: string;

  /**
   * 物料层级
   */
  materialHierarchy?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB 等；默认 ROH）
   */
  materialType: string;

  /**
   * 基本单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  baseUnit: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 采购类型（字典 logistics_procurement_type；E=自制生产，F=外部采购，X=两种采购类型；默认 F）
   */
  purchaseType: string;

  /**
   * 特殊采购（字典 logistics_procurement_special_procurement_type；0=无，10=寄售，30=外协加工，50=虚设品号；默认 0）
   */
  specialProcurement: number;

  /**
   * 是否散装（字典 sys_yes_no；0=否，1=是）
   */
  isBulk: number;

  /**
   * 最小起订量（基本单位数量，整数）
   */
  minOrderQuantity: number;

  /**
   * 舍入值（基本单位数量，用于数量舍入，整数）
   */
  roundingValue: number;

  /**
   * 计划交货时间（天数，整数）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 自制生产天数（内部生产所需天数，支持 1 位小数，如 0.5、2.5）
   */
  inHouseProductionDays: number;

  /**
   * 制造商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  manufacturer?: string;

  /**
   * 制造商物料编码（选项 TaktManufacturerMaterials/options；DictValue=ManufacturerMaterialCode）
   */
  manufacturerMaterialCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl: string;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 移动价格（decimal，4 位小数）
   */
  movingPrice: number;

  /**
   * 差异码（6）
   */
  differenceCode?: string;

  /**
   * 利润中心（选项 TaktProfitCenters/options；DictValue=ProfitCenterCode）
   */
  profitCenter: string;

  /**
   * 当前库存（基本单位数量，decimal，4 位小数）
   */
  currentStock: number;

  /**
   * 生产仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  productionLocation: string;

  /**
   * 采购仓储（选项 TaktWarehouses/options；DictValue=WarehouseCode）
   */
  purchasingLocation: string;

  /**
   * 库位（选项 TaktStorageLocations/options；DictValue=LocationCode）
   */
  storageLocation: string;

  /**
   * 是否需检验（字典 sys_yes_no；0=否，1=是）
   */
  requiresInspection: number;

  /**
   * 批次标识（字典 sys_yes_no；0=否，1=是）
   */
  isBatch: number;

  /**
   * 停产状态（字典 logistics_materials_material_discontinued_status；DictValue=01/Z0 等；默认 Z0=计划物料）
   */
  discontinuedStatus: string;

  /**
   * 物料状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  materialStatus: number;

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

