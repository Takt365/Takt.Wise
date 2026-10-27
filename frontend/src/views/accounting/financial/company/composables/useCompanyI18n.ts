// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/company/composables
// 文件名称：useCompanyI18n.ts
// 功能描述：公司实体 代表租户下的独立公司字段清单 + useCompanyI18n（字段名映射一次，文案由 entity.company.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyQuery } from '@/types/accounting/financial/company.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCompanyI18nSeedData 一致的实体 slug */
export const COMPANY_ENTITY_SLUG = 'company'

/** entity.company._self 静态属性（导入组件 entity-i18n-key 等） */
export const COMPANY_SELF_I18N_KEY = buildEntitySelfI18nKey(COMPANY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const COMPANY_LIST_FIELDS = [
  'companyName1',
  'companyName2',
  'companyShortName',
  'enterpriseNature',
  'industryAttribute',
  'enterpriseScale',
  'businessScope',
  'registrationAddress1',
  'registrationAddress2',
  'registrationRegion',
  'registrationProvince',
  'registrationCity',
  'businessRegion',
  'businessProvince',
  'businessCity',
  'businessAddress1',
  'businessAddress2',
  'companyPhone',
  'companyEmail',
  'companyFax',
  'companyWebsite',
  'unifiedSocialCreditCode',
  'taxRegistrationNumber',
  'legalRepresentative',
  'companyManager',
  'registeredCapital',
  'establishmentDate',
  'closingDate',
  'companyExistence',
  'codeAlias',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'currencyCode',
  'chartOfAccounts',
  'inputTaxCode',
  'outputTaxCode',
  'businessPlace',
  'postingPeriodVariant',
  'fiscalYearVariant',
  'creditControlArea',
  'financialManagementArea',
  'companyStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const COMPANY_PLACEHOLDER = {
  tenantCode: 'optional',
  cultureCode: 'optional',
  relatedPlant: 'select',
  companyName1: 'required',
  companyName2: 'optional',
  companyShortName: 'required',
  enterpriseNature: 'select',
  industryAttribute: 'select',
  enterpriseScale: 'select',
  businessScope: 'optional',
  registrationAddress1: 'optional',
  registrationAddress2: 'optional',
  registrationRegion: 'select',
  registrationProvince: 'select',
  registrationCity: 'select',
  businessRegion: 'select',
  businessProvince: 'select',
  businessCity: 'select',
  businessAddress1: 'optional',
  businessAddress2: 'optional',
  companyPhone: 'required',
  companyEmail: 'required',
  companyFax: 'required',
  companyWebsite: 'required',
  unifiedSocialCreditCode: 'required',
  taxRegistrationNumber: 'required',
  legalRepresentative: 'required',
  companyManager: 'required',
  registeredCapital: 'select',
  establishmentDate: 'select',
  closingDate: 'optional',
  companyExistence: 'select',
  codeAlias: 'required',
  bankCode: 'select',
  bankAccount: 'required',
  accountHolder: 'required',
  currencyCode: 'select',
  chartOfAccounts: 'select',
  inputTaxCode: 'select',
  outputTaxCode: 'select',
  businessPlace: 'required',
  postingPeriodVariant: 'select',
  fiscalYearVariant: 'select',
  creditControlArea: 'select',
  financialManagementArea: 'select',
  companyStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CompanyField = keyof typeof COMPANY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const COMPANY_QUERY_STRING_FIELDS = [
  'cultureCode',
  'companyName1',
  'companyName2',
  'companyShortName',
  'enterpriseNature',
  'industryAttribute',
  'enterpriseScale',
  'businessScope',
  'registrationAddress1',
  'registrationAddress2',
  'registrationRegion',
  'registrationProvince',
  'registrationCity',
  'businessRegion',
  'businessProvince',
  'businessCity',
  'businessAddress1',
  'businessAddress2',
  'companyPhone',
  'companyEmail',
  'companyFax',
  'companyWebsite',
  'unifiedSocialCreditCode',
  'taxRegistrationNumber',
  'legalRepresentative',
  'companyManager',
  'establishmentDateStart',
  'establishmentDateEnd',
  'closingDateStart',
  'closingDateEnd',
  'codeAlias',
  'bankCode',
  'bankAccount',
  'accountHolder',
  'currencyCode',
  'chartOfAccounts',
  'inputTaxCode',
  'outputTaxCode',
  'businessPlace',
  'postingPeriodVariant',
  'fiscalYearVariant',
  'creditControlArea',
  'financialManagementArea',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CompanyQuery)[]

export type CompanyQueryField =
  | (typeof COMPANY_QUERY_STRING_FIELDS)[number]
  | 'registeredCapital' | 'companyExistence' | 'companyStatus'

/** 高级查询抽屉全部字段（含数值） */
export const COMPANY_QUERY_FIELDS: readonly CompanyQueryField[] = [
  ...COMPANY_QUERY_STRING_FIELDS,
  'registeredCapital',
  'companyExistence',
  'companyStatus',
]

/**
 * 公司实体 代表租户下的独立公司字段 i18n：index / company-form 统一入口
 */
export function useCompanyI18n() {
  const ef = useEntityFieldI18n(COMPANY_ENTITY_SLUG)

  function ph(field: CompanyField): string {
    return ef.placeholder(field, COMPANY_PLACEHOLDER[field])
  }

  function queryPh(field: CompanyQueryField, kind: EntityFieldPlaceholderKind): string {
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
