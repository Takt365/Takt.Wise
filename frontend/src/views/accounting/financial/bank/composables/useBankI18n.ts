// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/bank/composables
// 文件名称：useBankI18n.ts
// 功能描述：银行信息实体字段清单 + useBankI18n（字段名映射一次，文案由 entity.bank.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BankQuery } from '@/types/accounting/financial/bank.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBankI18nSeedData 一致的实体 slug */
export const BANK_ENTITY_SLUG = 'bank'

/** entity.bank._self 静态属性（导入组件 entity-i18n-key 等） */
export const BANK_SELF_I18N_KEY = buildEntitySelfI18nKey(BANK_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BANK_LIST_FIELDS = [
  'countryRegion',
  'bankCode',
  'bankName1',
  'bankName2',
  'province',
  'prefecture',
  'district',
  'township',
  'village',
  'address1',
  'address2',
  'swiftBic',
  'bankGroup',
  'pobkCurAc',
  'bankNumber',
  'postalBank',
  'addressNumber',
  'branch',
  'bankMethod',
  'bankFormat',
  'ibanRule',
  'sddB2b',
  'sddCore',
  'sddRtrans',
  'bicPlusNumber',
  'pathCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BANK_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  countryRegion: 'select',
  bankCode: 'required',
  bankName1: 'required',
  bankName2: 'optional',
  province: 'optional',
  prefecture: 'optional',
  district: 'optional',
  township: 'optional',
  village: 'optional',
  address1: 'optional',
  address2: 'optional',
  swiftBic: 'optional',
  bankGroup: 'optional',
  pobkCurAc: 'select',
  bankNumber: 'optional',
  postalBank: 'optional',
  addressNumber: 'optional',
  branch: 'optional',
  bankMethod: 'optional',
  bankFormat: 'optional',
  ibanRule: 'optional',
  sddB2b: 'select',
  sddCore: 'select',
  sddRtrans: 'select',
  bicPlusNumber: 'optional',
  pathCode: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BankField = keyof typeof BANK_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BANK_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'countryRegion',
  'bankCode',
  'bankName1',
  'bankName2',
  'province',
  'prefecture',
  'district',
  'township',
  'village',
  'address1',
  'address2',
  'swiftBic',
  'bankGroup',
  'bankNumber',
  'postalBank',
  'addressNumber',
  'branch',
  'bankMethod',
  'bankFormat',
  'ibanRule',
  'bicPlusNumber',
  'pathCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BankQuery)[]

export type BankQueryField =
  | (typeof BANK_QUERY_STRING_FIELDS)[number]
  | 'pobkCurAc' | 'sddB2b' | 'sddCore' | 'sddRtrans'

/** 高级查询抽屉全部字段（含数值） */
export const BANK_QUERY_FIELDS: readonly BankQueryField[] = [
  ...BANK_QUERY_STRING_FIELDS,
  'pobkCurAc',
  'sddB2b',
  'sddCore',
  'sddRtrans',
]

/**
 * 银行信息实体字段 i18n：index / bank-form 统一入口
 */
export function useBankI18n() {
  const ef = useEntityFieldI18n(BANK_ENTITY_SLUG)

  function ph(field: BankField): string {
    return ef.placeholder(field, BANK_PLACEHOLDER[field])
  }

  function queryPh(field: BankQueryField, kind: EntityFieldPlaceholderKind): string {
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
