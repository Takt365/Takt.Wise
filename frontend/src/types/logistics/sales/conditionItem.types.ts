// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：conditionItem.types.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售条件行实体（定价记录条件行；主子表：TaktSalesCondition → Items → ScaleQuantities / ScaleValues）
 * 对应前端 TaktSalesConditionItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesConditionItem
 * @description 对应后端 TaktSalesConditionItemDto
 */
export interface SalesConditionItem extends CompanyDtoBase {
  /**
   * SalesConditionItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesConditionItemId: string;

  /**
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId: string;

  /**
   * 销售价格 名称（填充字段）
   */
  salesConditionName?: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（主子表关系） （子表：TaktSalesConditionScaleQuantity）
   */
  scaleQuantities?: SalesConditionScaleQuantity[];

  /**
   * 价值等级行列表（主子表关系） （子表：TaktSalesConditionScaleValue）
   */
  scaleValues?: SalesConditionScaleValue[];

}


/**
 * SalesConditionItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesConditionItemQuery
 * @description 对应后端 TaktSalesConditionItemQueryDto
 */
export interface SalesConditionItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId?: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode?: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建SalesConditionItem DTO
 * 对应前端 SalesConditionItemCreate
 * @description 对应后端 TaktSalesConditionItemCreateDto
 */
export interface SalesConditionItemCreate {
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
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 数量等级行列表（主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesConditionScaleQuantityCreate[];

  /**
   * 价值等级行列表（主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesConditionScaleValueCreate[];

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
 * 更新SalesConditionItem DTO
 * 继承 TaktSalesConditionItemCreateDto，添加 SalesConditionItemId 字段
 * 对应前端 SalesConditionItemUpdate
 * @description 对应后端 TaktSalesConditionItemUpdateDto
 */
export interface SalesConditionItemUpdate extends SalesConditionItemCreate {
  /**
   * SalesConditionItemID（标识要更新的实体）
   */
  salesConditionItemId: string;

  /**
   * 数量等级行列表（主子表关系）（子表，级联保存）
   */
  scaleQuantities?: any;

  /**
   * 价值等级行列表（主子表关系）（子表，级联保存）
   */
  scaleValues?: any;

}


/**
 * SalesConditionItem 作废/撤销作废 DTO
 * 对应前端 SalesConditionItemObsolete
 * @description 对应后端 TaktSalesConditionItemObsoleteDto
 */
export interface SalesConditionItemObsolete {
  /**
   * SalesConditionItemID
   */
  salesConditionItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesConditionItem 导入模板行 DTO
 * 对应前端 SalesConditionItemTemplate
 * @description 对应后端 TaktSalesConditionItemTemplateDto
 */
export interface SalesConditionItemTemplate {
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
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId?: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode?: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesConditionScaleQuantityCreate[];

  /**
   * 价值等级行列表（主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesConditionScaleValueCreate[];

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
 * SalesConditionItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesConditionItemImport
 * @description 对应后端 TaktSalesConditionItemImportDto
 */
export interface SalesConditionItemImport {
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
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId?: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode?: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq?: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType?: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity?: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue?: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType?: string;

  /**
   * 价格
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode?: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 数量等级行列表（主子表关系）（子表，级联保存）
   */
  scaleQuantities?: SalesConditionScaleQuantityCreate[];

  /**
   * 价值等级行列表（主子表关系）（子表，级联保存）
   */
  scaleValues?: SalesConditionScaleValueCreate[];

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
 * SalesConditionItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesConditionItemExport
 * @description 对应后端 TaktSalesConditionItemExportDto
 */
export interface SalesConditionItemExport {
  /**
   * SalesConditionItemID
   */
  salesConditionItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 销售价格 ID（主子表关系；选项 TaktSalesConditions/options，DictValue=Id）
   */
  salesConditionId: string;

  /**
   * 定价记录号（冗余：按对应 Id 取主数据名称联动）
   */
  salesConditionCode: string;

  /**
   * 定价序号（项号/序号，固定步长=10）
   */
  salesConditionSeq: number;

  /**
   * 条件类型（冗余；字典 logistics_procurement_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
   */
  priceType: string;

  /**
   * 等级类型（字典 logistics_procurement_scale；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
   */
  scaleType?: string;

  /**
   * 等级基础（字典 logistics_procurement_scale_basis；B=价值等级，C=数量规模，…）
   */
  scaleBasis?: string;

  /**
   * 等级数量
   */
  scaleQuantity: number;

  /**
   * 等级单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等）
   */
  scaleUnit?: string;

  /**
   * 等级值
   */
  scaleValue: number;

  /**
   * 等级货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  scaleCurrencyCode?: string;

  /**
   * 计算类型（字典 logistics_procurement_calculation_type；默认 A=百分数）
   */
  calculationType: string;

  /**
   * 价格
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 条件货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等；默认 CNY）
   */
  conditionCurrencyCode: string;

  /**
   * 定价单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 最小起订量（计量单位数量，整数）
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
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

