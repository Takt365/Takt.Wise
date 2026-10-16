// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/customer/composables
// 文件名称：useCustomerI18n.ts
// 功能描述：Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+CustomerCode字段清单 + useCustomerI18n（字段名映射一次，文案由 entity.customer.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerQuery } from '@/types/logistics/sales/customer.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerI18nSeedData 一致的实体 slug */
export const CUSTOMER_ENTITY_SLUG = 'customer'

/** entity.customer._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMER_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMER_LIST_FIELDS = [
  'customerCode',
  'accountGroup',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'customerTitle',
  'customerName1',
  'customerName2',
  'customerName3',
  'customerName4',
  'customerShortName',
  'registrationCountry',
  'registrationRegion',
  'registrationCity',
  'addr1',
  'addr2',
  'addr3',
  'addr4',
  'addr5',
  'postalCode',
  'phone1',
  'phone2',
  'faxNumber',
  'supplierCode',
  'tradingPartner',
  'taxNumber1',
  'taxNumber2',
  'taxNumber3',
  'bankCountryCode',
  'bankCode',
  'bankAccount',
  'bankAccountHolder',
  'customerCategory',
  'enterpriseNature',
  'industryAttribute',
  'reconciliationAccount',
  'paymentTerms',
  'paymentMethod',
  'salesDistrict',
  'salesDepartment',
  'salesGroup',
  'currencyCode',
  'customerPricingProcedure',
  'shippingConditions',
  'deliveringPlant',
  'incoterms1',
  'incoterms2',
  'accountAssignmentGroup',
  'taxClassification',
  'customerContact',
  'customerPhone',
  'customerEmail',
  'creditLevel',
  'customerLevel',
  'customerStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  numberingRuleCode: 'select',
  customerCode: 'optional',
  numberingRuleCode: 'optional',
  accountGroup: 'required',
  salesOrganization: 'select',
  distributionChannel: 'required',
  productGroup: 'required',
  customerTitle: 'select',
  customerName1: 'required',
  customerName2: 'optional',
  customerName3: 'optional',
  customerName4: 'optional',
  customerShortName: 'optional',
  registrationCountry: 'optional',
  registrationRegion: 'optional',
  registrationCity: 'optional',
  addr1: 'optional',
  addr2: 'optional',
  addr3: 'optional',
  addr4: 'optional',
  addr5: 'optional',
  postalCode: 'optional',
  phone1: 'optional',
  phone2: 'optional',
  faxNumber: 'optional',
  supplierCode: 'select',
  tradingPartner: 'select',
  taxNumber1: 'optional',
  taxNumber2: 'optional',
  taxNumber3: 'optional',
  bankCountryCode: 'optional',
  bankCode: 'select',
  bankAccount: 'optional',
  bankAccountHolder: 'optional',
  customerCategory: 'select',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  reconciliationAccount: 'select',
  paymentTerms: 'select',
  paymentMethod: 'select',
  salesDistrict: 'optional',
  salesDepartment: 'optional',
  salesGroup: 'optional',
  currencyCode: 'select',
  customerPricingProcedure: 'select',
  shippingConditions: 'select',
  deliveringPlant: 'select',
  incoterms1: 'select',
  incoterms2: 'required',
  accountAssignmentGroup: 'select',
  taxClassification: 'optional',
  customerContact: 'optional',
  customerPhone: 'optional',
  customerEmail: 'optional',
  creditLevel: 'select',
  customerLevel: 'select',
  customerStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerField = keyof typeof CUSTOMER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'customerCode',
  'accountGroup',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'customerName1',
  'customerName2',
  'customerName3',
  'customerName4',
  'customerShortName',
  'registrationCountry',
  'registrationRegion',
  'registrationCity',
  'addr1',
  'addr2',
  'addr3',
  'addr4',
  'addr5',
  'postalCode',
  'phone1',
  'phone2',
  'faxNumber',
  'supplierCode',
  'tradingPartner',
  'taxNumber1',
  'taxNumber2',
  'taxNumber3',
  'bankCountryCode',
  'bankCode',
  'bankAccount',
  'bankAccountHolder',
  'enterpriseNature',
  'industryAttribute',
  'reconciliationAccount',
  'paymentTerms',
  'salesDistrict',
  'salesDepartment',
  'salesGroup',
  'currencyCode',
  'customerPricingProcedure',
  'shippingConditions',
  'deliveringPlant',
  'incoterms1',
  'incoterms2',
  'accountAssignmentGroup',
  'taxClassification',
  'customerContact',
  'customerPhone',
  'customerEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerQuery)[]

export type CustomerQueryField =
  | (typeof CUSTOMER_QUERY_STRING_FIELDS)[number]
  | 'customerTitle' | 'customerCategory' | 'paymentMethod' | 'creditLevel' | 'customerLevel' | 'customerStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMER_QUERY_FIELDS: readonly CustomerQueryField[] = [
  ...CUSTOMER_QUERY_STRING_FIELDS,
  'customerTitle',
  'customerCategory',
  'paymentMethod',
  'creditLevel',
  'customerLevel',
  'customerStatus',
]

/**
 * Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+CustomerCode字段 i18n：index / CustomerForm 统一入口
 */
export function useCustomerI18n() {
  const ef = useEntityFieldI18n(CUSTOMER_ENTITY_SLUG)

  function ph(field: CustomerField): string {
    return ef.placeholder(field, CUSTOMER_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
