// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/ticket/composables
// 文件名称：useTicketI18n.ts
// 功能描述：服务工单实体字段清单 + useCustomerServiceTicketI18n（字段名映射一次，文案由 entity.customerserviceticket.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerServiceTicketQuery } from '@/types/logistics/customer-service/ticket.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerServiceTicketI18nSeedData 一致的实体 slug */
export const CUSTOMERSERVICETICKET_ENTITY_SLUG = 'customerserviceticket'

/** entity.customerserviceticket._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSERVICETICKET_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSERVICETICKET_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSERVICETICKET_LIST_FIELDS = [
  'serviceTicketCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceRequestId',
  'serviceRequestCode',
  'serviceOrderId',
  'serviceOrderCode',
  'serviceContractId',
  'serviceContractCode',
  'ticketType',
  'priority',
  'ticketStatus',
  'ticketSubject',
  'faultDescription',
  'solutionDescription',
  'serviceLocation',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'scheduledStartTime',
  'scheduledEndTime',
  'actualStartTime',
  'actualEndTime',
  'acceptanceResult',
  'acceptedByEmployeeId',
  'acceptedByEmployeeName',
  'acceptedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSERVICETICKET_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  serviceTicketCode: 'required',
  clientId: 'required',
  clientCode: 'optional',
  clientName1: 'optional',
  serviceRequestId: 'optional',
  serviceRequestCode: 'optional',
  serviceOrderId: 'optional',
  serviceOrderCode: 'optional',
  serviceContractId: 'optional',
  serviceContractCode: 'optional',
  ticketType: 'select',
  priority: 'select',
  ticketStatus: 'select',
  ticketSubject: 'required',
  faultDescription: 'optional',
  solutionDescription: 'optional',
  serviceLocation: 'optional',
  assignedEmployeeId: 'optional',
  assignedEmployeeName: 'optional',
  scheduledStartTime: 'optional',
  scheduledEndTime: 'optional',
  actualStartTime: 'optional',
  actualEndTime: 'optional',
  acceptanceResult: 'optional',
  acceptedByEmployeeId: 'optional',
  acceptedByEmployeeName: 'optional',
  acceptedAt: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerServiceTicketField = keyof typeof CUSTOMERSERVICETICKET_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSERVICETICKET_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'serviceTicketCode',
  'clientId',
  'clientCode',
  'clientName1',
  'serviceRequestId',
  'serviceRequestCode',
  'serviceOrderId',
  'serviceOrderCode',
  'serviceContractId',
  'serviceContractCode',
  'ticketSubject',
  'faultDescription',
  'solutionDescription',
  'serviceLocation',
  'assignedEmployeeId',
  'assignedEmployeeName',
  'scheduledStartTimeStart',
  'scheduledStartTimeEnd',
  'scheduledEndTimeStart',
  'scheduledEndTimeEnd',
  'actualStartTimeStart',
  'actualStartTimeEnd',
  'actualEndTimeStart',
  'actualEndTimeEnd',
  'acceptedByEmployeeId',
  'acceptedByEmployeeName',
  'acceptedAtStart',
  'acceptedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerServiceTicketQuery)[]

export type CustomerServiceTicketQueryField =
  | (typeof CUSTOMERSERVICETICKET_QUERY_STRING_FIELDS)[number]
  | 'ticketType' | 'priority' | 'ticketStatus' | 'acceptanceResult'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSERVICETICKET_QUERY_FIELDS: readonly CustomerServiceTicketQueryField[] = [
  ...CUSTOMERSERVICETICKET_QUERY_STRING_FIELDS,
  'ticketType',
  'priority',
  'ticketStatus',
  'acceptanceResult',
]

/**
 * 服务工单实体字段 i18n：index / ticket-form 统一入口
 */
export function useCustomerServiceTicketI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSERVICETICKET_ENTITY_SLUG)

  function ph(field: CustomerServiceTicketField): string {
    return ef.placeholder(field, CUSTOMERSERVICETICKET_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerServiceTicketQueryField, kind: EntityFieldPlaceholderKind): string {
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
