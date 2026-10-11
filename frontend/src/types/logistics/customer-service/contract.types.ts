// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：contract.types.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 服务合同实体
 * 对应前端 TaktCustomerServiceContractDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerServiceContract
 * @description 对应后端 TaktCustomerServiceContractDto
 */
export interface CustomerServiceContract extends CompanyDtoBase {
  /**
   * CustomerServiceContractID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerServiceContractId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端名称（填充字段）
   */
  clientName?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceContract 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerServiceContractQuery
 * @description 对应后端 TaktCustomerServiceContractQueryDto
 */
export interface CustomerServiceContractQuery extends TaktPagedQuery {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期（范围查询-开始）
   */
  signDateStart?: string;

  /**
   * 签订日期（范围查询-结束）
   */
  signDateEnd?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 到期日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 到期日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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
 * 创建CustomerServiceContract DTO
 * 对应前端 CustomerServiceContractCreate
 * @description 对应后端 TaktCustomerServiceContractCreateDto
 */
export interface CustomerServiceContractCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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
 * 更新CustomerServiceContract DTO
 * 继承 TaktCustomerServiceContractCreateDto，添加 CustomerServiceContractId 字段
 * 对应前端 CustomerServiceContractUpdate
 * @description 对应后端 TaktCustomerServiceContractUpdateDto
 */
export interface CustomerServiceContractUpdate extends CustomerServiceContractCreate {
  /**
   * CustomerServiceContractID（标识要更新的实体）
   */
  customerServiceContractId: string;

}


/**
 * CustomerServiceContract 状态更新 DTO
 * 对应前端 CustomerServiceContractStatus
 * @description 对应后端 TaktCustomerServiceContractStatusDto
 */
export interface CustomerServiceContractStatus {
  /**
   * CustomerServiceContractID
   */
  customerServiceContractId: string;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

}


/**
 * CustomerServiceContract 排序更新 DTO
 * 对应前端 CustomerServiceContractSort
 * @description 对应后端 TaktCustomerServiceContractSortDto
 */
export interface CustomerServiceContractSort {
  /**
   * CustomerServiceContractID
   */
  customerServiceContractId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceContract 导入模板行 DTO
 * 对应前端 CustomerServiceContractTemplate
 * @description 对应后端 TaktCustomerServiceContractTemplateDto
 */
export interface CustomerServiceContractTemplate {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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
 * CustomerServiceContract 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerServiceContractImport
 * @description 对应后端 TaktCustomerServiceContractImportDto
 */
export interface CustomerServiceContractImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode?: string;

  /**
   * 合同名称
   */
  contractName?: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId?: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode?: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1?: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType?: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus?: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount?: number;

  /**
   * 结算币种代码
   */
  currencyCode?: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms?: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours?: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours?: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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
 * CustomerServiceContract 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerServiceContractExport
 * @description 对应后端 TaktCustomerServiceContractExportDto
 */
export interface CustomerServiceContractExport {
  /**
   * CustomerServiceContractID
   */
  customerServiceContractId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 服务合同编码（组合唯一索引）
   */
  serviceContractCode: string;

  /**
   * 合同名称
   */
  contractName: string;

  /**
   * 客户端ID（关联 TaktClient，序列化为string以避免Javascript精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（冗余字段，便于查询）
   */
  clientCode: string;

  /**
   * 客户端名称（冗余字段，便于查询）
   */
  clientName1: string;

  /**
   * 合同类型（0=维保，1=单次，2=框架，3=SLA，4=其他）
   */
  contractType: number;

  /**
   * 合同状态（0=草稿，1=生效，2=暂停，3=到期，4=终止）
   */
  contractStatus: number;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 合同金额
   */
  contractAmount: number;

  /**
   * 结算币种代码
   */
  currencyCode: string;

  /**
   * 付款条件（0=预付，1=后付，2=月结30天，3=月结60天，4=其他）
   */
  paymentTerms: number;

  /**
   * 服务范围描述
   */
  serviceScope?: string;

  /**
   * SLA 响应时限（小时）
   */
  slaResponseHours: number;

  /**
   * SLA 解决时限（小时）
   */
  slaResolveHours: number;

  /**
   * 客户经理（人员代码）
   */
  accountManager?: string;

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

