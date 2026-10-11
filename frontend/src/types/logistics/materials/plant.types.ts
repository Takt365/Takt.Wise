// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：plant.types.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCultureDtoBase
} from '@/types/common.types';

/**
 * Takt工厂实体 代表租户下的独立工厂主档 与公司种子对称 组合 2：无关联工厂、有语言（TaktTenantCultureEntityBase；业务键即 PlantCode，无需 RelatedPlant）
 * 对应前端 TaktPlantDto
 * 继承 TaktTenantCultureDtoBase
 * 对应前端 Plant
 * @description 对应后端 TaktPlantDto
 */
export interface Plant extends TenantCultureDtoBase {
  /**
   * PlantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  plantId: string;

  /**
   * 工厂名称1
   */
  plantName1: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale: string;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup: string;

  /**
   * 税收标识
   */
  taxIndicator: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber: string;

  /**
   * 工厂日历
   */
  factoryCalendar: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus: number;

}


/**
 * Plant 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PlantQuery
 * @description 对应后端 TaktPlantQueryDto
 */
export interface PlantQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂名称1
   */
  plantName1?: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute?: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale?: string;

  /**
   * 经营范围
   */
  businessScope?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion?: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince?: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity?: string;

  /**
   * 经营地址1
   */
  businessAddress1?: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone?: string;

  /**
   * 工厂邮箱
   */
  plantEmail?: string;

  /**
   * 工厂传真
   */
  plantFax?: string;

  /**
   * 工厂网站
   */
  plantWebsite?: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode?: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber?: string;

  /**
   * 法定代表人
   */
  legalRepresentative?: string;

  /**
   * 工厂负责人
   */
  plantManager?: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital?: number;

  /**
   * 成立日期（范围查询-开始）
   */
  establishmentDateStart?: string;

  /**
   * 成立日期（范围查询-结束）
   */
  establishmentDateEnd?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）（范围查询-开始）
   */
  closingDateStart?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）（范围查询-结束）
   */
  closingDateEnd?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence?: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup?: string;

  /**
   * 税收标识
   */
  taxIndicator?: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea?: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber?: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber?: string;

  /**
   * 工厂日历
   */
  factoryCalendar?: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus?: number;

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
 * 创建Plant DTO
 * 对应前端 PlantCreate
 * @description 对应后端 TaktPlantCreateDto
 */
export interface PlantCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂名称1
   */
  plantName1: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale: string;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup: string;

  /**
   * 税收标识
   */
  taxIndicator: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber: string;

  /**
   * 工厂日历
   */
  factoryCalendar: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany: string;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus: number;

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
 * 更新Plant DTO
 * 继承 TaktPlantCreateDto，添加 PlantId 字段
 * 对应前端 PlantUpdate
 * @description 对应后端 TaktPlantUpdateDto
 */
export interface PlantUpdate extends PlantCreate {
  /**
   * PlantID（标识要更新的实体）
   */
  plantId: string;

}


/**
 * Plant 状态更新 DTO
 * 对应前端 PlantStatus
 * @description 对应后端 TaktPlantStatusDto
 */
export interface PlantStatus {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus: number;

}


/**
 * Plant 排序更新 DTO
 * 对应前端 PlantSort
 * @description 对应后端 TaktPlantSortDto
 */
export interface PlantSort {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Plant 导入模板行 DTO
 * 对应前端 PlantTemplate
 * @description 对应后端 TaktPlantTemplateDto
 */
export interface PlantTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂名称1
   */
  plantName1?: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute?: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale?: string;

  /**
   * 经营范围
   */
  businessScope?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion?: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince?: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity?: string;

  /**
   * 经营地址1
   */
  businessAddress1?: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone?: string;

  /**
   * 工厂邮箱
   */
  plantEmail?: string;

  /**
   * 工厂传真
   */
  plantFax?: string;

  /**
   * 工厂网站
   */
  plantWebsite?: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode?: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber?: string;

  /**
   * 法定代表人
   */
  legalRepresentative?: string;

  /**
   * 工厂负责人
   */
  plantManager?: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital?: number;

  /**
   * 成立日期
   */
  establishmentDate?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence?: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup?: string;

  /**
   * 税收标识
   */
  taxIndicator?: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea?: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber?: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber?: string;

  /**
   * 工厂日历
   */
  factoryCalendar?: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany?: string;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus?: number;

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
 * Plant 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PlantImport
 * @description 对应后端 TaktPlantImportDto
 */
export interface PlantImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂名称1
   */
  plantName1?: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName?: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias?: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute?: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale?: string;

  /**
   * 经营范围
   */
  businessScope?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion?: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince?: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity?: string;

  /**
   * 经营地址1
   */
  businessAddress1?: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone?: string;

  /**
   * 工厂邮箱
   */
  plantEmail?: string;

  /**
   * 工厂传真
   */
  plantFax?: string;

  /**
   * 工厂网站
   */
  plantWebsite?: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode?: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber?: string;

  /**
   * 法定代表人
   */
  legalRepresentative?: string;

  /**
   * 工厂负责人
   */
  plantManager?: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital?: number;

  /**
   * 成立日期
   */
  establishmentDate?: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence?: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup?: string;

  /**
   * 税收标识
   */
  taxIndicator?: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea?: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber?: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber?: string;

  /**
   * 工厂日历
   */
  factoryCalendar?: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany?: string;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus?: number;

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
 * Plant 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PlantExport
 * @description 对应后端 TaktPlantExportDto
 */
export interface PlantExport {
  /**
   * PlantID
   */
  plantId: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂名称1
   */
  plantName1: string;

  /**
   * 工厂名称2
   */
  plantName2?: string;

  /**
   * 工厂简称
   */
  plantShortName: string;

  /**
   * 编码代号（如 TKC、TCJ、DTA；前端字典录入）
   */
  codeAlias: string;

  /**
   * 企业性质（字典 sys_enterprise_nature；DictValue=150 等）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute；DictValue=C 等）
   */
  industryAttribute: string;

  /**
   * 企业规模（字典 sys_enterprise_scale；DictValue=M 等）
   */
  enterpriseScale: string;

  /**
   * 经营范围
   */
  businessScope: string;

  /**
   * 注册地址1
   */
  registrationAddress1: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationRegion: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity: string;

  /**
   * 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  businessRegion: string;

  /**
   * 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  businessProvince: string;

  /**
   * 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  businessCity: string;

  /**
   * 经营地址1
   */
  businessAddress1: string;

  /**
   * 经营地址2
   */
  businessAddress2?: string;

  /**
   * 工厂地址1
   */
  plantAddress1?: string;

  /**
   * 工厂地址2
   */
  plantAddress2?: string;

  /**
   * 工厂电话
   */
  plantPhone: string;

  /**
   * 工厂邮箱
   */
  plantEmail: string;

  /**
   * 工厂传真
   */
  plantFax: string;

  /**
   * 工厂网站
   */
  plantWebsite: string;

  /**
   * 统一社会信用代码
   */
  unifiedSocialCreditCode: string;

  /**
   * 税务登记号
   */
  taxRegistrationNumber: string;

  /**
   * 法定代表人
   */
  legalRepresentative: string;

  /**
   * 工厂负责人
   */
  plantManager: string;

  /**
   * 注册资本（万元）
   */
  registeredCapital: number;

  /**
   * 成立日期
   */
  establishmentDate: string;

  /**
   * 关闭日期（注销/停业；未关闭则为 null）
   */
  closingDate?: string;

  /**
   * 存续状态（字典 sys_entity_existence_status；1=存续（在营），2=吊销，3=注销，4=迁出，5=停业）
   */
  plantExistence: number;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 物料需求计划（MRP 范围/控制；对齐）
   */
  materialRequirementsPlanning: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 公司间出具发票产品组（产品组/Division）
   */
  intercompanyBillingProductGroup: string;

  /**
   * 税收标识
   */
  taxIndicator: string;

  /**
   * 评估范围（选项 TaktPlants/options；DictValue=PlantCode；常与工厂代码相同）
   */
  valuationArea: string;

  /**
   * 工厂供应商号码（工厂作为供应商）
   */
  plantVendorNumber: string;

  /**
   * 客户编码-工厂（工厂作为客户）
   */
  plantCustomerNumber: string;

  /**
   * 工厂日历
   */
  factoryCalendar: string;

  /**
   * 关联公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  relatedCompany: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 工厂状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
   */
  plantStatus: number;

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

