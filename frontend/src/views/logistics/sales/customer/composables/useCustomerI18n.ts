// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/customer/composables
// 文件名称：useCustomerI18n.ts
// 功能描述：Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+CustomerCode字段清单 + useCustomerI18n（字段名映射一次，文案由 entity.customer.* 种子动态解析）
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
  'customerName1',
  'customerName2',
  'customerShortName',
  'customerType',
  'enterpriseNature',
  'industryAttribute',
  'customerTaxNumber',
  'taxCode',
  'taxRate',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'customerPhone',
  'customerFax',
  'customerEmail',
  'customerWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'customerGroup',
  'tradingPartner',
  'accountAssignmentGroup',
  'supplierCode',
  'nielsenIndicator',
  'centralPostingBlock',
  'reconciliationAccount',
  'headquarters',
  'clearingWithVendor',
  'paymentTerms',
  'paymentMethod',
  'deliveringPlant',
  'incoterms1',
  'incoterms2',
  'shippingConditions',
  'customerPricingProcedure',
  'creditLevel',
  'creditAmount',
  'discountRate',
  'customerLevel',
  'evaluationScore',
  'customerStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  customerCode: 'required',
  customerName1: 'required',
  customerName2: 'optional',
  customerShortName: 'optional',
  customerType: 'select',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  customerTaxNumber: 'optional',
  taxCode: 'optional',
  taxRate: 'select',
  registrationCountry: 'optional',
  registrationProvince: 'optional',
  registrationCity: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  customerPhone: 'optional',
  customerFax: 'optional',
  customerEmail: 'optional',
  customerWebsite: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  currencyCode: 'select',
  salesOrganization: 'select',
  distributionChannel: 'required',
  productGroup: 'required',
  customerGroup: 'select',
  tradingPartner: 'select',
  accountAssignmentGroup: 'select',
  supplierCode: 'select',
  nielsenIndicator: 'required',
  centralPostingBlock: 'select',
  reconciliationAccount: 'select',
  headquarters: 'select',
  clearingWithVendor: 'select',
  paymentTerms: 'select',
  paymentMethod: 'select',
  deliveringPlant: 'select',
  incoterms1: 'select',
  incoterms2: 'required',
  shippingConditions: 'select',
  customerPricingProcedure: 'select',
  creditLevel: 'select',
  creditAmount: 'select',
  discountRate: 'select',
  customerLevel: 'select',
  evaluationScore: 'select',
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
  'customerName1',
  'customerName2',
  'customerShortName',
  'enterpriseNature',
  'industryAttribute',
  'customerTaxNumber',
  'taxCode',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'customerPhone',
  'customerFax',
  'customerEmail',
  'customerWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'customerGroup',
  'tradingPartner',
  'accountAssignmentGroup',
  'supplierCode',
  'nielsenIndicator',
  'reconciliationAccount',
  'headquarters',
  'paymentTerms',
  'deliveringPlant',
  'incoterms1',
  'incoterms2',
  'shippingConditions',
  'customerPricingProcedure',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerQuery)[]

export type CustomerQueryField =
  | (typeof CUSTOMER_QUERY_STRING_FIELDS)[number]
  | 'customerType' | 'taxRate' | 'centralPostingBlock' | 'clearingWithVendor' | 'paymentMethod' | 'creditLevel' | 'creditAmount' | 'discountRate' | 'customerLevel' | 'evaluationScore' | 'customerStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMER_QUERY_FIELDS: readonly CustomerQueryField[] = [
  ...CUSTOMER_QUERY_STRING_FIELDS,
  'customerType',
  'taxRate',
  'centralPostingBlock',
  'clearingWithVendor',
  'paymentMethod',
  'creditLevel',
  'creditAmount',
  'discountRate',
  'customerLevel',
  'evaluationScore',
  'customerStatus',
]

/**
 * Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+CustomerCode字段 i18n：index / customer-form 统一入口
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
