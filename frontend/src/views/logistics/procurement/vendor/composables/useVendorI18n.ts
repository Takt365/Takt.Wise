// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/vendor/composables
// 文件名称：useVendorI18n.ts
// 功能描述：Takt经销商实体字段清单 + useVendorI18n（字段名映射一次，文案由 entity.vendor.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { VendorQuery } from '@/types/logistics/procurement/vendor.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktVendorI18nSeedData 一致的实体 slug */
export const VENDOR_ENTITY_SLUG = 'vendor'

/** entity.vendor._self 静态属性（导入组件 entity-i18n-key 等） */
export const VENDOR_SELF_I18N_KEY = buildEntitySelfI18nKey(VENDOR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const VENDOR_LIST_FIELDS = [
  'vendorCode',
  'vendorName1',
  'vendorName2',
  'vendorShortName',
  'vendorType',
  'enterpriseNature',
  'industryAttribute',
  'vendorTaxNumber',
  'taxCode',
  'taxRate',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'vendorPhone',
  'vendorFax',
  'vendorEmail',
  'vendorWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'reconciliationAccount',
  'customerCode',
  'clearingWithCustomer',
  'paymentMethod',
  'paymentTerms',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'grBasedInvoiceInspection',
  'incoterms1',
  'incoterms2',
  'automaticPurchaseOrder',
  'pricingDateControl',
  'purchaseGroup',
  'plannedDeliveryTimeDays',
  'evaluatedReceiptSettlement',
  'purchasingOrganization',
  'creditLevel',
  'creditAmount',
  'authorizedBrand',
  'agentRegion',
  'vendorLevel',
  'evaluationScore',
  'vendorStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const VENDOR_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  vendorCode: 'required',
  vendorName1: 'required',
  vendorName2: 'optional',
  vendorShortName: 'optional',
  vendorType: 'select',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  vendorTaxNumber: 'optional',
  taxCode: 'optional',
  taxRate: 'select',
  registrationCountry: 'optional',
  registrationProvince: 'optional',
  registrationCity: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  vendorPhone: 'optional',
  vendorFax: 'optional',
  vendorEmail: 'optional',
  vendorWebsite: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  currencyCode: 'select',
  reconciliationAccount: 'select',
  customerCode: 'select',
  clearingWithCustomer: 'select',
  paymentMethod: 'select',
  paymentTerms: 'select',
  bankCode: 'select',
  bankAccount: 'required',
  accountHolder: 'required',
  grBasedInvoiceInspection: 'select',
  incoterms1: 'select',
  incoterms2: 'required',
  automaticPurchaseOrder: 'select',
  pricingDateControl: 'select',
  purchaseGroup: 'select',
  plannedDeliveryTimeDays: 'select',
  evaluatedReceiptSettlement: 'select',
  purchasingOrganization: 'select',
  creditLevel: 'select',
  creditAmount: 'select',
  authorizedBrand: 'optional',
  agentRegion: 'optional',
  vendorLevel: 'select',
  evaluationScore: 'select',
  vendorStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type VendorField = keyof typeof VENDOR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const VENDOR_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'vendorCode',
  'vendorName1',
  'vendorName2',
  'vendorShortName',
  'enterpriseNature',
  'industryAttribute',
  'vendorTaxNumber',
  'taxCode',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'vendorPhone',
  'vendorFax',
  'vendorEmail',
  'vendorWebsite',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'currencyCode',
  'reconciliationAccount',
  'customerCode',
  'paymentTerms',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'incoterms1',
  'incoterms2',
  'purchaseGroup',
  'purchasingOrganization',
  'authorizedBrand',
  'agentRegion',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof VendorQuery)[]

export type VendorQueryField =
  | (typeof VENDOR_QUERY_STRING_FIELDS)[number]
  | 'vendorType' | 'taxRate' | 'clearingWithCustomer' | 'paymentMethod' | 'grBasedInvoiceInspection' | 'automaticPurchaseOrder' | 'pricingDateControl' | 'plannedDeliveryTimeDays' | 'evaluatedReceiptSettlement' | 'creditLevel' | 'creditAmount' | 'vendorLevel' | 'evaluationScore' | 'vendorStatus'

/** 高级查询抽屉全部字段（含数值） */
export const VENDOR_QUERY_FIELDS: readonly VendorQueryField[] = [
  ...VENDOR_QUERY_STRING_FIELDS,
  'vendorType',
  'taxRate',
  'clearingWithCustomer',
  'paymentMethod',
  'grBasedInvoiceInspection',
  'automaticPurchaseOrder',
  'pricingDateControl',
  'plannedDeliveryTimeDays',
  'evaluatedReceiptSettlement',
  'creditLevel',
  'creditAmount',
  'vendorLevel',
  'evaluationScore',
  'vendorStatus',
]

/**
 * Takt经销商实体字段 i18n：index / vendor-form 统一入口
 */
export function useVendorI18n() {
  const ef = useEntityFieldI18n(VENDOR_ENTITY_SLUG)

  function ph(field: VendorField): string {
    return ef.placeholder(field, VENDOR_PLACEHOLDER[field])
  }

  function queryPh(field: VendorQueryField, kind: EntityFieldPlaceholderKind): string {
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
