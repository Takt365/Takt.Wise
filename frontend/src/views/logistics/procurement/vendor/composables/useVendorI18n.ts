// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/vendor/composables
// 文件名称：useVendorI18n.ts
// 功能描述：Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode字段清单 + useVendorI18n（字段名映射一次，文案由 entity.vendor.* 种子动态解析）
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
  'accountGroup',
  'vendorTitle',
  'vendorName1',
  'vendorName2',
  'vendorName3',
  'vendorName4',
  'vendorShortName',
  'countryCode',
  'regionCode',
  'cityCode',
  'addr1',
  'addr2',
  'postalCode',
  'phone1',
  'phone2',
  'faxNumber',
  'customerCode',
  'tradingPartner',
  'authorizationGroup',
  'groupKey',
  'vendorType',
  'enterpriseNature',
  'industryAttribute',
  'creditInfoNumber',
  'bankCountryCode',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'bankChargeBearerCode',
  'reconciliationAccount',
  'headOfficeAccountCode',
  'planningGroup',
  'paymentTerms',
  'doubleInvoiceCheck',
  'paymentMethod',
  'paymentBlock',
  'houseBank',
  'clearingWithCustomer',
  'accountingClerk',
  'ourAccountAtVendor',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'withholdingTaxCountry',
  'withholdingTaxType',
  'withholdingTaxCode',
  'purchasingOrganization',
  'orderCurrencyCode',
  'incoterms1',
  'incoterms2',
  'pricingDateControl',
  'grBasedInvoiceInspection',
  'evaluatedReceiptSettlement',
  'returnErsFlag',
  'automaticPurchaseOrder',
  'purchaseGroup',
  'taxNumber1',
  'taxNumber2',
  'taxNumber3',
  'creditLevel',
  'vendorLevel',
  'vendorStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const VENDOR_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  vendorCode: 'required',
  accountGroup: 'required',
  vendorTitle: 'select',
  vendorName1: 'required',
  vendorName2: 'optional',
  vendorName3: 'optional',
  vendorName4: 'optional',
  vendorShortName: 'optional',
  countryCode: 'optional',
  regionCode: 'optional',
  cityCode: 'optional',
  addr1: 'optional',
  addr2: 'optional',
  postalCode: 'optional',
  phone1: 'optional',
  phone2: 'optional',
  faxNumber: 'optional',
  customerCode: 'optional',
  tradingPartner: 'optional',
  authorizationGroup: 'optional',
  groupKey: 'optional',
  vendorType: 'select',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  creditInfoNumber: 'optional',
  bankCountryCode: 'optional',
  bankCode: 'optional',
  bankAccount: 'optional',
  accountHolder: 'optional',
  bankChargeBearerCode: 'optional',
  reconciliationAccount: 'select',
  headOfficeAccountCode: 'optional',
  planningGroup: 'optional',
  paymentTerms: 'select',
  doubleInvoiceCheck: 'select',
  paymentMethod: 'select',
  paymentBlock: 'select',
  houseBank: 'optional',
  clearingWithCustomer: 'select',
  accountingClerk: 'optional',
  ourAccountAtVendor: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  withholdingTaxCountry: 'optional',
  withholdingTaxType: 'optional',
  withholdingTaxCode: 'optional',
  purchasingOrganization: 'select',
  orderCurrencyCode: 'select',
  incoterms1: 'select',
  incoterms2: 'required',
  pricingDateControl: 'select',
  grBasedInvoiceInspection: 'select',
  evaluatedReceiptSettlement: 'select',
  returnErsFlag: 'select',
  automaticPurchaseOrder: 'select',
  purchaseGroup: 'select',
  taxNumber1: 'optional',
  taxNumber2: 'optional',
  taxNumber3: 'optional',
  creditLevel: 'select',
  vendorLevel: 'select',
  vendorStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type VendorField = keyof typeof VENDOR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const VENDOR_QUERY_STRING_FIELDS = [
  'cultureCode',
  'vendorCode',
  'accountGroup',
  'vendorName1',
  'vendorName2',
  'vendorName3',
  'vendorName4',
  'vendorShortName',
  'countryCode',
  'regionCode',
  'cityCode',
  'addr1',
  'addr2',
  'postalCode',
  'phone1',
  'phone2',
  'faxNumber',
  'customerCode',
  'tradingPartner',
  'authorizationGroup',
  'groupKey',
  'enterpriseNature',
  'industryAttribute',
  'creditInfoNumber',
  'bankCountryCode',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'bankChargeBearerCode',
  'reconciliationAccount',
  'headOfficeAccountCode',
  'planningGroup',
  'paymentTerms',
  'houseBank',
  'accountingClerk',
  'ourAccountAtVendor',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'withholdingTaxCountry',
  'withholdingTaxType',
  'withholdingTaxCode',
  'purchasingOrganization',
  'orderCurrencyCode',
  'incoterms1',
  'incoterms2',
  'purchaseGroup',
  'taxNumber1',
  'taxNumber2',
  'taxNumber3',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof VendorQuery)[]

export type VendorQueryField =
  | (typeof VENDOR_QUERY_STRING_FIELDS)[number]
  | 'vendorTitle' | 'vendorType' | 'doubleInvoiceCheck' | 'paymentMethod' | 'paymentBlock' | 'clearingWithCustomer' | 'pricingDateControl' | 'grBasedInvoiceInspection' | 'evaluatedReceiptSettlement' | 'returnErsFlag' | 'automaticPurchaseOrder' | 'creditLevel' | 'vendorLevel' | 'vendorStatus'

/** 高级查询抽屉全部字段（含数值） */
export const VENDOR_QUERY_FIELDS: readonly VendorQueryField[] = [
  ...VENDOR_QUERY_STRING_FIELDS,
  'vendorTitle',
  'vendorType',
  'doubleInvoiceCheck',
  'paymentMethod',
  'paymentBlock',
  'clearingWithCustomer',
  'pricingDateControl',
  'grBasedInvoiceInspection',
  'evaluatedReceiptSettlement',
  'returnErsFlag',
  'automaticPurchaseOrder',
  'creditLevel',
  'vendorLevel',
  'vendorStatus',
]

/**
 * Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode字段 i18n：index / VendorForm 统一入口
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
