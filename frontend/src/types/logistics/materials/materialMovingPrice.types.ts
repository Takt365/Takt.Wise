// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialMovingPrice.types.ts
// 创建时间：2026-08-21
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
 * 移动价格实体 <para>业务唯一键（新增/更新匹配）：TenantCode+CompanyCode+PlantCode+MaterialCode+ValuationPeriod；Valuation 为业务字段，不参与唯一匹配。</para>
 * 对应前端 TaktMaterialMovingPriceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialMovingPrice
 * @description 对应后端 TaktMaterialMovingPriceDto
 */
export interface MaterialMovingPrice extends CompanyDtoBase {
  /**
   * MaterialMovingPriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialMovingPriceId: string;

  /**
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

}


/**
 * MaterialMovingPrice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialMovingPriceQuery
 * @description 对应后端 TaktMaterialMovingPriceQueryDto
 */
export interface MaterialMovingPriceQuery extends TaktPagedQuery {
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
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity?: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount?: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl?: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

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
 * 创建MaterialMovingPrice DTO
 * 对应前端 MaterialMovingPriceCreate
 * @description 对应后端 TaktMaterialMovingPriceCreateDto
 */
export interface MaterialMovingPriceCreate {
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
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

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
 * 更新MaterialMovingPrice DTO
 * 继承 TaktMaterialMovingPriceCreateDto，添加 MaterialMovingPriceId 字段
 * 对应前端 MaterialMovingPriceUpdate
 * @description 对应后端 TaktMaterialMovingPriceUpdateDto
 */
export interface MaterialMovingPriceUpdate extends MaterialMovingPriceCreate {
  /**
   * MaterialMovingPriceID（标识要更新的实体）
   */
  materialMovingPriceId: string;

}


/**
 * MaterialMovingPrice 导入模板行 DTO
 * 对应前端 MaterialMovingPriceTemplate
 * @description 对应后端 TaktMaterialMovingPriceTemplateDto
 */
export interface MaterialMovingPriceTemplate {
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
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity?: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount?: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl?: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

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
 * MaterialMovingPrice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialMovingPriceImport
 * @description 对应后端 TaktMaterialMovingPriceImportDto
 */
export interface MaterialMovingPriceImport {
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
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation?: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity?: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount?: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl?: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice?: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

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
 * MaterialMovingPrice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialMovingPriceExport
 * @description 对应后端 TaktMaterialMovingPriceExportDto
 */
export interface MaterialMovingPriceExport {
  /**
   * MaterialMovingPriceID
   */
  materialMovingPriceId: string;

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
   * 评估期间（yyyy-MM；与工厂+物料编码构成业务唯一键）
   */
  valuationPeriod: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 评估类别（字典 logistics_materials_valuation_class；Z792=成品，Z790=半成品，Z300=原材料）
   */
  valuation: string;

  /**
   * 库存数量（基本单位，4 位小数）
   */
  stockQuantity: number;

  /**
   * 库存金额（与币种一致，2 位小数）
   */
  stockAmount: number;

  /**
   * 价格控制（字典 logistics_materials_price_control；S=标准价格，V=移动平均价格/周期单价；默认 V）
   */
  priceControl: string;

  /**
   * 移动价格（decimal，5 位小数；相对价格单位）
   */
  movingPrice: number;

  /**
   * 价格单位（字典 logistics_materials_price_unit_param；1/10/100/1000；默认 1000）
   */
  priceUnit: number;

  /**
   * 币种（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

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

