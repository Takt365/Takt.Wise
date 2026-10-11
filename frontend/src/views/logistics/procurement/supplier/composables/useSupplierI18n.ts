// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/supplier/composables
// 文件名称：useSupplierI18n.ts
// 功能描述：Takt供货商实体字段清单 + useSupplierI18n（字段名映射一次，文案由 entity.supplier.* 种子动态解析）
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
  'supplierName1',
  'supplierName2',
  'supplierShortName',
  'supplierType',
  'enterpriseNature',
  'industryAttribute',
  'supplierTaxNumber',
  'taxCode',
  'taxRate',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'supplierPhone',
  'supplierFax',
  'supplierEmail',
  'supplierWebsite',
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
  'supplierLevel',
  'evaluationScore',
  'supplierStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SUPPLIER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  supplierCode: 'required',
  supplierName1: 'required',
  supplierName2: 'optional',
  supplierShortName: 'optional',
  supplierType: 'select',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  supplierTaxNumber: 'optional',
  taxCode: 'optional',
  taxRate: 'select',
  registrationCountry: 'optional',
  registrationProvince: 'optional',
  registrationCity: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  supplierPhone: 'optional',
  supplierFax: 'optional',
  supplierEmail: 'optional',
  supplierWebsite: 'optional',
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
  supplierLevel: 'select',
  evaluationScore: 'select',
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
  'supplierName1',
  'supplierName2',
  'supplierShortName',
  'enterpriseNature',
  'industryAttribute',
  'supplierTaxNumber',
  'taxCode',
  'registrationCountry',
  'registrationProvince',
  'registrationCity',
  'registrationAddress1',
  'registrationAddress2',
  'supplierPhone',
  'supplierFax',
  'supplierEmail',
  'supplierWebsite',
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
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SupplierQuery)[]

export type SupplierQueryField =
  | (typeof SUPPLIER_QUERY_STRING_FIELDS)[number]
  | 'supplierType' | 'taxRate' | 'clearingWithCustomer' | 'paymentMethod' | 'grBasedInvoiceInspection' | 'automaticPurchaseOrder' | 'pricingDateControl' | 'plannedDeliveryTimeDays' | 'evaluatedReceiptSettlement' | 'supplierLevel' | 'evaluationScore' | 'supplierStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SUPPLIER_QUERY_FIELDS: readonly SupplierQueryField[] = [
  ...SUPPLIER_QUERY_STRING_FIELDS,
  'supplierType',
  'taxRate',
  'clearingWithCustomer',
  'paymentMethod',
  'grBasedInvoiceInspection',
  'automaticPurchaseOrder',
  'pricingDateControl',
  'plannedDeliveryTimeDays',
  'evaluatedReceiptSettlement',
  'supplierLevel',
  'evaluationScore',
  'supplierStatus',
]

/**
 * Takt供货商实体字段 i18n：index / supplier-form 统一入口
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
