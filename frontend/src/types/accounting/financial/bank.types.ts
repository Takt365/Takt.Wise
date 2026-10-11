// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：bank.types.ts
// 创建时间：2026-08-13
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
 * 银行信息实体（公司级；租户+公司隔离；按国家地区 + 银行代码唯一）
 * 对应前端 TaktBankDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Bank
 * @description 对应后端 TaktBankDto
 */
export interface Bank extends CompanyDtoBase {
  /**
   * BankID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bankId: string;

  /**
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode: string;

  /**
   * 银行名称1
   */
  bankName1: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

}


/**
 * Bank 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BankQuery
 * @description 对应后端 TaktBankQueryDto
 */
export interface BankQuery extends TaktPagedQuery {
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
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion?: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode?: string;

  /**
   * 银行名称1
   */
  bankName1?: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc?: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b?: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore?: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans?: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

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
 * 创建Bank DTO
 * 对应前端 BankCreate
 * @description 对应后端 TaktBankCreateDto
 */
export interface BankCreate {
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
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode: string;

  /**
   * 银行名称1
   */
  bankName1: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

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
 * 更新Bank DTO
 * 继承 TaktBankCreateDto，添加 BankId 字段
 * 对应前端 BankUpdate
 * @description 对应后端 TaktBankUpdateDto
 */
export interface BankUpdate extends BankCreate {
  /**
   * BankID（标识要更新的实体）
   */
  bankId: string;

}


/**
 * Bank 导入模板行 DTO
 * 对应前端 BankTemplate
 * @description 对应后端 TaktBankTemplateDto
 */
export interface BankTemplate {
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
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion?: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode?: string;

  /**
   * 银行名称1
   */
  bankName1?: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc?: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b?: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore?: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans?: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

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
 * Bank 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BankImport
 * @description 对应后端 TaktBankImportDto
 */
export interface BankImport {
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
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion?: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode?: string;

  /**
   * 银行名称1
   */
  bankName1?: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc?: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b?: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore?: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans?: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

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
 * Bank 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BankExport
 * @description 对应后端 TaktBankExportDto
 */
export interface BankExport {
  /**
   * BankID
   */
  bankId: string;

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
   * 国家地区（选项字典 sys_country_code，DictValue=ISO alpha-2）
   */
  countryRegion: string;

  /**
   * 银行代码（；CHAR 15；与国家地区组成业务唯一键）
   */
  bankCode: string;

  /**
   * 银行名称1
   */
  bankName1: string;

  /**
   * 银行名称2
   */
  bankName2?: string;

  /**
   * 州省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 地市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  prefecture?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 乡镇街道（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=5）
   */
  township?: string;

  /**
   * 行政村（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=6）
   */
  village?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 银行识别码（SWIFT/BIC；CHAR 11）
   */
  swiftBic?: string;

  /**
   * 银行组（；CHAR 2）
   */
  bankGroup?: string;

  /**
   * 邮政银行往来账户（字典 sys_yes_no）
   */
  pobkCurAc: number;

  /**
   * 银行编码（；CHAR 15）
   */
  bankNumber?: string;

  /**
   * 邮政银行（；CHAR 16）
   */
  postalBank?: string;

  /**
   * 地址号（；CHAR 10）
   */
  addressNumber?: string;

  /**
   * 分行（；CHAR 40）
   */
  branch?: string;

  /**
   * 方法（CHAR 4）
   */
  bankMethod?: string;

  /**
   * 格式（含银行数据文件的格式；CHAR 3）
   */
  bankFormat?: string;

  /**
   * IBAN 规则（CHAR 6）
   */
  ibanRule?: string;

  /**
   * 企业间（字典 sys_yes_no）
   */
  sddB2b: number;

  /**
   * 核心个人（字典 sys_yes_no）
   */
  sddCore: number;

  /**
   * SEPA拒付交易支持标识（字典 accounting_financial_sepa_rtrans_type）
   */
  sddRtrans: number;

  /**
   * BIC+ 编码（CHAR 12）
   */
  bicPlusNumber?: string;

  /**
   * 路径代码（CHAR 15）
   */
  pathCode?: string;

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

