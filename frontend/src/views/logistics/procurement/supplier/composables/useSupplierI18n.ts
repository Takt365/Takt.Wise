// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/supplier/composables
// 文件名称：useSupplierI18n.ts
// 功能描述：Takt供货商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+SupplierCode字段清单 + useSupplierI18n（字段名映射一次，文案由 entity.supplier.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SupplierQuery } from '@/types/logistics/procurement/supplier.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSupplierI18nSeedData 一致的实体 slug */
export const SUPPLIER_ENTITY_SLUG = 'supplier'

/** entity.supplier._self 静态属性（导入组件 entity-i18n-key 等） */
export const SUPPLIER_SELF_I18N_KEY = buildEntitySelfI18nKey(SUPPLIER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SUPPLIER_LIST_FIELDS = [
  'supplierCode',
  'accountGroup',
  'supplierTitle',
  'supplierName1',
  'supplierName2',
  'supplierName3',
  'supplierName4',
  'supplierShortName',
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
  'supplierType',
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
  'ourAccountAtSupplier',
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
  'supplierLevel',
  'supplierStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SUPPLIER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  supplierCode: 'required',
  accountGroup: 'required',
  supplierTitle: 'select',
  supplierName1: 'required',
  supplierName2: 'optional',
  supplierName3: 'optional',
  supplierName4: 'optional',
  supplierShortName: 'optional',
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
  supplierType: 'select',
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
  ourAccountAtSupplier: 'optional',
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
  supplierLevel: 'select',
  supplierStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SupplierField = keyof typeof SUPPLIER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SUPPLIER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'supplierCode',
  'accountGroup',
  'supplierName1',
  'supplierName2',
  'supplierName3',
  'supplierName4',
  'supplierShortName',
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
  'ourAccountAtSupplier',
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
] as const satisfies readonly (keyof SupplierQuery)[]

export type SupplierQueryField =
  | (typeof SUPPLIER_QUERY_STRING_FIELDS)[number]
  | 'supplierTitle' | 'supplierType' | 'doubleInvoiceCheck' | 'paymentMethod' | 'paymentBlock' | 'clearingWithCustomer' | 'pricingDateControl' | 'grBasedInvoiceInspection' | 'evaluatedReceiptSettlement' | 'returnErsFlag' | 'automaticPurchaseOrder' | 'creditLevel' | 'supplierLevel' | 'supplierStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SUPPLIER_QUERY_FIELDS: readonly SupplierQueryField[] = [
  ...SUPPLIER_QUERY_STRING_FIELDS,
  'supplierTitle',
  'supplierType',
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
  'supplierLevel',
  'supplierStatus',
]

/**
 * Takt供货商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+SupplierCode字段 i18n：index / SupplierForm 统一入口
 */
export function useSupplierI18n() {
  const ef = useEntityFieldI18n(SUPPLIER_ENTITY_SLUG)

  function ph(field: SupplierField): string {
    return ef.placeholder(field, SUPPLIER_PLACEHOLDER[field])
  }

  function queryPh(field: SupplierQueryField, kind: EntityFieldPlaceholderKind): string {
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
