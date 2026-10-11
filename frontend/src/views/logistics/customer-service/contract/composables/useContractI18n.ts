// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/contract/composables
// 文件名称：useContractI18n.ts
// 功能描述：服务合同实体字段清单 + useCustomerServiceContractI18n（字段名映射一次，文案由 entity.customerservicecontract.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerServiceContractQuery } from '@/types/logistics/customer-service/contract.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerServiceContractI18nSeedData 一致的实体 slug */
export const CUSTOMERSERVICECONTRACT_ENTITY_SLUG = 'customerservicecontract'

/** entity.customerservicecontract._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSERVICECONTRACT_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSERVICECONTRACT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSERVICECONTRACT_LIST_FIELDS = [
  'plantCode',
  'serviceContractCode',
  'contractName',
  'clientId',
  'clientCode',
  'clientName1',
  'contractType',
  'contractStatus',
  'signDate',
  'effectiveDate',
  'expiryDate',
  'contractAmount',
  'currencyCode',
  'paymentTerms',
  'serviceScope',
  'slaResponseHours',
  'slaResolveHours',
  'accountManager',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSERVICECONTRACT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  serviceContractCode: 'required',
  contractName: 'required',
  clientId: 'required',
  clientCode: 'optional',
  clientName1: 'optional',
  contractType: 'select',
  contractStatus: 'select',
  signDate: 'optional',
  effectiveDate: 'select',
  expiryDate: 'optional',
  contractAmount: 'select',
  currencyCode: 'required',
  paymentTerms: 'select',
  serviceScope: 'optional',
  slaResponseHours: 'select',
  slaResolveHours: 'select',
  accountManager: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerServiceContractField = keyof typeof CUSTOMERSERVICECONTRACT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSERVICECONTRACT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'serviceContractCode',
  'contractName',
  'clientId',
  'clientCode',
  'clientName1',
  'signDateStart',
  'signDateEnd',
  'effectiveDateStart',
  'effectiveDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'currencyCode',
  'serviceScope',
  'accountManager',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerServiceContractQuery)[]

export type CustomerServiceContractQueryField =
  | (typeof CUSTOMERSERVICECONTRACT_QUERY_STRING_FIELDS)[number]
  | 'contractType' | 'contractStatus' | 'contractAmount' | 'paymentTerms' | 'slaResponseHours' | 'slaResolveHours'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSERVICECONTRACT_QUERY_FIELDS: readonly CustomerServiceContractQueryField[] = [
  ...CUSTOMERSERVICECONTRACT_QUERY_STRING_FIELDS,
  'contractType',
  'contractStatus',
  'contractAmount',
  'paymentTerms',
  'slaResponseHours',
  'slaResolveHours',
]

/**
 * 服务合同实体字段 i18n：index / contract-form 统一入口
 */
export function useCustomerServiceContractI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSERVICECONTRACT_ENTITY_SLUG)

  function ph(field: CustomerServiceContractField): string {
    return ef.placeholder(field, CUSTOMERSERVICECONTRACT_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerServiceContractQueryField, kind: EntityFieldPlaceholderKind): string {
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
