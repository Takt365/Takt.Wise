// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/client/composables
// 文件名称：useClientI18n.ts
// 功能描述：Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode字段清单 + useClientI18n（字段名映射一次，文案由 entity.client.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ClientQuery } from '@/types/logistics/sales/client.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktClientI18nSeedData 一致的实体 slug */
export const CLIENT_ENTITY_SLUG = 'client'

/** entity.client._self 静态属性（导入组件 entity-i18n-key 等） */
export const CLIENT_SELF_I18N_KEY = buildEntitySelfI18nKey(CLIENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CLIENT_LIST_FIELDS = [
  'clientCode',
  'accountGroup',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'clientTitle',
  'clientName1',
  'clientName2',
  'clientName3',
  'clientName4',
  'clientShortName',
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
  'clientCategory',
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
  'clientContact',
  'clientPhone',
  'clientEmail',
  'creditLevel',
  'clientLevel',
  'clientStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CLIENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  numberingRuleCode: 'select',
  clientCode: 'optional',
  accountGroup: 'required',
  salesOrganization: 'select',
  distributionChannel: 'required',
  productGroup: 'required',
  clientTitle: 'select',
  clientName1: 'required',
  clientName2: 'optional',
  clientName3: 'optional',
  clientName4: 'optional',
  clientShortName: 'optional',
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
  bankCode: 'optional',
  bankAccount: 'optional',
  bankAccountHolder: 'optional',
  clientCategory: 'select',
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
  clientContact: 'optional',
  clientPhone: 'optional',
  clientEmail: 'optional',
  creditLevel: 'select',
  clientLevel: 'select',
  clientStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ClientField = keyof typeof CLIENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CLIENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'clientCode',
  'accountGroup',
  'salesOrganization',
  'distributionChannel',
  'productGroup',
  'clientName1',
  'clientName2',
  'clientName3',
  'clientName4',
  'clientShortName',
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
  'clientContact',
  'clientPhone',
  'clientEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ClientQuery)[]

export type ClientQueryField =
  | (typeof CLIENT_QUERY_STRING_FIELDS)[number]
  | 'clientTitle' | 'clientCategory' | 'paymentMethod' | 'creditLevel' | 'clientLevel' | 'clientStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CLIENT_QUERY_FIELDS: readonly ClientQueryField[] = [
  ...CLIENT_QUERY_STRING_FIELDS,
  'clientTitle',
  'clientCategory',
  'paymentMethod',
  'creditLevel',
  'clientLevel',
  'clientStatus',
]

/**
 * Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode字段 i18n：index / ClientForm 统一入口
 */
export function useClientI18n() {
  const ef = useEntityFieldI18n(CLIENT_ENTITY_SLUG)

  function ph(field: ClientField): string {
    return ef.placeholder(field, CLIENT_PLACEHOLDER[field])
  }

  function queryPh(field: ClientQueryField, kind: EntityFieldPlaceholderKind): string {
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
