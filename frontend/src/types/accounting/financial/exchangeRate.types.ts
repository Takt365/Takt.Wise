// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：exchangeRate.types.ts
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 汇率实体（租户级主数据；租户内各公司共用同一套汇率；维护自币种至目标币种的折算汇率及生效区间）
 * 对应前端 TaktExchangeRateDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 ExchangeRate
 * @description 对应后端 TaktExchangeRateDto
 */
export interface ExchangeRate extends TenantCoreDtoBase {
  /**
   * ExchangeRateID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  exchangeRateId: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus: number;

}

/**
 * ExchangeRate 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ExchangeRateQuery
 * @description 对应后端 TaktExchangeRateQueryDto
 */
export interface ExchangeRateQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode?: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode?: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType?: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate?: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom?: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus?: number;

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
 * 创建ExchangeRate DTO
 * 对应前端 ExchangeRateCreate
 * @description 对应后端 TaktExchangeRateCreateDto
 */
export interface ExchangeRateCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus: number;

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
 * 更新ExchangeRate DTO
 * 继承 TaktExchangeRateCreateDto，添加 ExchangeRateId 字段
 * 对应前端 ExchangeRateUpdate
 * @description 对应后端 TaktExchangeRateUpdateDto
 */
export interface ExchangeRateUpdate extends ExchangeRateCreate {
  /**
   * ExchangeRateID（标识要更新的实体）
   */
  exchangeRateId: string;

}

/**
 * ExchangeRate 状态更新 DTO
 * 对应前端 ExchangeRateStatus
 * @description 对应后端 TaktExchangeRateStatusDto
 */
export interface ExchangeRateStatus {
  /**
   * ExchangeRateID
   */
  exchangeRateId: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus: number;

}

/**
 * ExchangeRate 导入模板行 DTO
 * 对应前端 ExchangeRateTemplate
 * @description 对应后端 TaktExchangeRateTemplateDto
 */
export interface ExchangeRateTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode?: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode?: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType?: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate?: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom?: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus?: number;

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
 * ExchangeRate 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ExchangeRateImport
 * @description 对应后端 TaktExchangeRateImportDto
 */
export interface ExchangeRateImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode?: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode?: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType?: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate?: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom?: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus?: number;

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
 * ExchangeRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ExchangeRateExport
 * @description 对应后端 TaktExchangeRateExportDto
 */
export interface ExchangeRateExport {
  /**
   * ExchangeRateID
   */
  exchangeRateId: string;

  /**
   * 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
   */
  fromCurrencyCode: string;

  /**
   * 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
   */
  toCurrencyCode: string;

  /**
   * 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
   */
  exchangeRateType: string;

  /**
   * 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
   */
  exchangeRate: number;

  /**
   * 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
   */
  ratioFrom: number;

  /**
   * 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
   */
  ratioTo: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  exchangeRateStatus: number;

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

