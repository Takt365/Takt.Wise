// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：client.types.ts
// 创建时间：2026-10-27
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
 * Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode。</para>
 * 对应前端 TaktClientDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Client
 * @description 对应后端 TaktClientDto
 */
export interface Client extends CompanyDtoBase {
  /**
   * ClientID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  clientId: string;

  /**
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode: string;

  /**
   * 科目组
   */
  accountGroup: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle: number;

  /**
   * 名称1
   */
  clientName1: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus: number;

}


/**
 * Client 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ClientQuery
 * @description 对应后端 TaktClientQueryDto
 */
export interface ClientQuery extends TaktPagedQuery {
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
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode?: string;

  /**
   * 科目组
   */
  accountGroup?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle?: number;

  /**
   * 名称1
   */
  clientName1?: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel?: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus?: number;

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
 * 创建Client DTO
 * 对应前端 ClientCreate
 * @description 对应后端 TaktClientCreateDto
 */
export interface ClientCreate {
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
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode: string;

  /**
   * 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 科目组
   */
  accountGroup: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle: number;

  /**
   * 名称1
   */
  clientName1: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus: number;

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
 * 更新Client DTO
 * 继承 TaktClientCreateDto，添加 ClientId 字段
 * 对应前端 ClientUpdate
 * @description 对应后端 TaktClientUpdateDto
 */
export interface ClientUpdate extends ClientCreate {
  /**
   * ClientID（标识要更新的实体）
   */
  clientId: string;

}


/**
 * Client 状态更新 DTO
 * 对应前端 ClientStatus
 * @description 对应后端 TaktClientStatusDto
 */
export interface ClientStatus {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus: number;

}


/**
 * Client 排序更新 DTO
 * 对应前端 ClientSort
 * @description 对应后端 TaktClientSortDto
 */
export interface ClientSort {
  /**
   * ClientID
   */
  clientId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Client 导入模板行 DTO
 * 对应前端 ClientTemplate
 * @description 对应后端 TaktClientTemplateDto
 */
export interface ClientTemplate {
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
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode?: string;

  /**
   * 科目组
   */
  accountGroup?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle?: number;

  /**
   * 名称1
   */
  clientName1?: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel?: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus?: number;

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
 * Client 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ClientImport
 * @description 对应后端 TaktClientImportDto
 */
export interface ClientImport {
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
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode?: string;

  /**
   * 科目组
   */
  accountGroup?: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 产品组
   */
  productGroup?: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle?: number;

  /**
   * 名称1
   */
  clientName1?: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions?: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup?: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel?: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus?: number;

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
 * Client 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ClientExport
 * @description 对应后端 TaktClientExportDto
 */
export interface ClientExport {
  /**
   * ClientID
   */
  clientId: string;

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
   * 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
   */
  clientCode: string;

  /**
   * 科目组
   */
  accountGroup: string;

  /**
   * 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  salesOrganization: string;

  /**
   * 分销渠道
   */
  distributionChannel: string;

  /**
   * 产品组
   */
  productGroup: string;

  /**
   * 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  clientTitle: number;

  /**
   * 名称1
   */
  clientName1: string;

  /**
   * 名称2
   */
  clientName2?: string;

  /**
   * 名称3
   */
  clientName3?: string;

  /**
   * 名称4
   */
  clientName4?: string;

  /**
   * 简称
   */
  clientShortName?: string;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationRegion?: string;

  /**
   * 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 地址1（含门牌，短字符串 40 不够）
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 地址3（含门牌，短字符串 40 不够）
   */
  addr3?: string;

  /**
   * 地址4（含门牌，短字符串 40 不够）
   */
  addr4?: string;

  /**
   * 地址5（含门牌，短字符串 40 不够）
   */
  addr5?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话1
   */
  phone1?: string;

  /**
   * 电话2
   */
  phone2?: string;

  /**
   * 传真
   */
  faxNumber?: string;

  /**
   * 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
   */
  tradingPartner: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 账户持有人
   */
  bankAccountHolder?: string;

  /**
   * 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  clientCategory: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 销售地区
   */
  salesDistrict?: string;

  /**
   * 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
   */
  salesDepartment?: string;

  /**
   * 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
   */
  salesGroup?: string;

  /**
   * 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
   */
  customerPricingProcedure: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
   */
  shippingConditions: string;

  /**
   * 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  deliveringPlant: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
   */
  accountAssignmentGroup: string;

  /**
   * 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxClassification?: string;

  /**
   * 客户端联系人
   */
  clientContact?: string;

  /**
   * 客户端联系电话
   */
  clientPhone?: string;

  /**
   * 客户端联系邮件（邮箱地址短字符串 40 不够）
   */
  clientEmail?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
   */
  clientLevel: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户端状态（字典 sys_normal_disable）
   */
  clientStatus: number;

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

