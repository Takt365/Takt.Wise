// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/request/composables
// 文件名称：useRequestI18n.ts
// 功能描述：服务请求实体字段清单 + useCustomerServiceRequestI18n（字段名映射一次，文案由 entity.customerservicerequest.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerServiceRequestQuery } from '@/types/logistics/customer-service/request.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerServiceRequestI18nSeedData 一致的实体 slug */
export const CUSTOMERSERVICEREQUEST_ENTITY_SLUG = 'customerservicerequest'

/** entity.customerservicerequest._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSERVICEREQUEST_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSERVICEREQUEST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSERVICEREQUEST_LIST_FIELDS = [
  'serviceRequestCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'requestDate',
  'expectedServiceDate',
  'requestType',
  'sourceChannel',
  'priority',
  'requestStatus',
  'requestSubject',
  'requestDescription',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'serviceAddress',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'assignedAt',
  'closedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSERVICEREQUEST_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  serviceRequestCode: 'required',
  clientId: 'required',
  clientCode: 'optional',
  clientName1: 'optional',
  serviceContractId: 'optional',
  serviceContractCode: 'optional',
  requestDate: 'select',
  expectedServiceDate: 'optional',
  requestType: 'select',
  sourceChannel: 'select',
  priority: 'select',
  requestStatus: 'select',
  requestSubject: 'required',
  requestDescription: 'optional',
  contactPerson: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  serviceAddress: 'optional',
  assignedEmployeeId: 'optional',
  assignedEmployeeName: 'optional',
  assignedAt: 'optional',
  closedAt: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerServiceRequestField = keyof typeof CUSTOMERSERVICEREQUEST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSERVICEREQUEST_QUERY_STRING_FIELDS = [
  'cultureCode',
  'serviceRequestCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceContractId',
  'serviceContractCode',
  'requestDateStart',
  'requestDateEnd',
  'expectedServiceDateStart',
  'expectedServiceDateEnd',
  'requestSubject',
  'requestDescription',
  'contactPerson',
  'contactPhone',
  'contactEmail',
  'serviceAddress',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'assignedAtStart',
  'assignedAtEnd',
  'closedAtStart',
  'closedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerServiceRequestQuery)[]

export type CustomerServiceRequestQueryField =
  | (typeof CUSTOMERSERVICEREQUEST_QUERY_STRING_FIELDS)[number]
  | 'requestType' | 'sourceChannel' | 'priority' | 'requestStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSERVICEREQUEST_QUERY_FIELDS: readonly CustomerServiceRequestQueryField[] = [
  ...CUSTOMERSERVICEREQUEST_QUERY_STRING_FIELDS,
  'requestType',
  'sourceChannel',
  'priority',
  'requestStatus',
]

/**
 * 服务请求实体字段 i18n：index / request-form 统一入口
 */
export function useCustomerServiceRequestI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSERVICEREQUEST_ENTITY_SLUG)

  function ph(field: CustomerServiceRequestField): string {
    return ef.placeholder(field, CUSTOMERSERVICEREQUEST_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerServiceRequestQueryField, kind: EntityFieldPlaceholderKind): string {
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
