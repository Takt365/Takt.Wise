// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：taxCondition.types.ts
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 税务条件实体（公司级；租户内公司 + 应用程序 + 国家 + 税码 + 条件记录号唯一）。应用程序用字典 accounting_financial_application，条件类型用字典 accounting_financial_condition_type（ExtValue=应用程序）。
 * 对应前端 TaktTaxConditionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TaxCondition
 * @description 对应后端 TaktTaxConditionDto
 */
export interface TaxCondition extends CompanyDtoBase {
  /**
   * TaxConditionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  taxConditionId: string;

  /**
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus: number;

}


/**
 * TaxCondition 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TaxConditionQuery
 * @description 对应后端 TaktTaxConditionQueryDto
 */
export interface TaxConditionQuery extends TaktPagedQuery {
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
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode?: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus?: number;

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
 * 创建TaxCondition DTO
 * 对应前端 TaxConditionCreate
 * @description 对应后端 TaktTaxConditionCreateDto
 */
export interface TaxConditionCreate {
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
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode: string;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus: number;

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
 * 更新TaxCondition DTO
 * 继承 TaktTaxConditionCreateDto，添加 TaxConditionId 字段
 * 对应前端 TaxConditionUpdate
 * @description 对应后端 TaktTaxConditionUpdateDto
 */
export interface TaxConditionUpdate extends TaxConditionCreate {
  /**
   * TaxConditionID（标识要更新的实体）
   */
  taxConditionId: string;

}


/**
 * TaxCondition 状态更新 DTO
 * 对应前端 TaxConditionStatus
 * @description 对应后端 TaktTaxConditionStatusDto
 */
export interface TaxConditionStatus {
  /**
   * TaxConditionID
   */
  taxConditionId: string;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus: number;

}


/**
 * TaxCondition 排序更新 DTO
 * 对应前端 TaxConditionSort
 * @description 对应后端 TaktTaxConditionSortDto
 */
export interface TaxConditionSort {
  /**
   * TaxConditionID
   */
  taxConditionId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * TaxCondition 导入模板行 DTO
 * 对应前端 TaxConditionTemplate
 * @description 对应后端 TaktTaxConditionTemplateDto
 */
export interface TaxConditionTemplate {
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
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode?: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode?: string;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus?: number;

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
 * TaxCondition 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TaxConditionImport
 * @description 对应后端 TaktTaxConditionImportDto
 */
export interface TaxConditionImport {
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
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode?: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode?: string;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus?: number;

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
 * TaxCondition 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TaxConditionExport
 * @description 对应后端 TaktTaxConditionExportDto
 */
export interface TaxConditionExport {
  /**
   * TaxConditionID
   */
  taxConditionId: string;

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
   * 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
   */
  applicationCode: string;

  /**
   * 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
   */
  conditionTypeCode: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode: string;

  /**
   * 条件记录号（长度 10）
   */
  conditionRecordCode: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxConditionStatus: number;

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

