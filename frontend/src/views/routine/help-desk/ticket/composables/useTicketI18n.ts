// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/help-desk/ticket/composables
// 文件名称：useTicketI18n.ts
// 功能描述：服务台工单实体字段清单 + useTicketI18n（字段名映射一次，文案由 entity.ticket.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TicketQuery } from '@/types/routine/help-desk/ticket.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTicketI18nSeedData 一致的实体 slug */
export const TICKET_ENTITY_SLUG = 'ticket'

/** entity.ticket._self 静态属性（导入组件 entity-i18n-key 等） */
export const TICKET_SELF_I18N_KEY = buildEntitySelfI18nKey(TICKET_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TICKET_LIST_FIELDS = [
  'ticketCode',
  'ticketTitle',
  'ticketContent',
  'attachments',
  'priority',
  'urgency',
  'impact',
  'categoryCode',
  'ticketSource',
  'submitterId',
  'submitterName',
  'assigneeId',
  'assigneeName',
  'knowledgeId',
  'parentTicketId',
  'firstResponseAt',
  'firstResponseDueBy',
  'resolvedAt',
  'resolutionDueBy',
  'closedAt',
  'itAssetId',
  'assetCode',
  'applicantDeptId',
  'applicantDeptName',
  'applicantBy',
  'applicantName',
  'ticketStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const TICKET_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  ticketCode: 'required',
  ticketTitle: 'required',
  ticketContent: 'optional',
  attachments: 'optional',
  priority: 'select',
  urgency: 'select',
  impact: 'select',
  categoryCode: 'optional',
  ticketSource: 'select',
  submitterId: 'select',
  submitterName: 'optional',
  assigneeId: 'optional',
  assigneeName: 'optional',
  knowledgeId: 'optional',
  parentTicketId: 'optional',
  firstResponseAt: 'optional',
  firstResponseDueBy: 'optional',
  resolvedAt: 'optional',
  resolutionDueBy: 'optional',
  closedAt: 'optional',
  itAssetId: 'optional',
  assetCode: 'optional',
  applicantDeptId: 'optional',
  applicantDeptName: 'optional',
  applicantBy: 'select',
  applicantName: 'optional',
  ticketStatus: 'select',
  childTickets: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type TicketField = keyof typeof TICKET_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TICKET_QUERY_STRING_FIELDS = [
  'cultureCode',
  'ticketCode',
  'ticketTitle',
  'ticketContent',
  'attachments',
  'categoryCode',
  'submitterId',
  'submitterName',
  'assigneeId',
  'assigneeName',
  'knowledgeId',
  'parentTicketId',
  'firstResponseAtStart',
  'firstResponseAtEnd',
  'firstResponseDueByStart',
  'firstResponseDueByEnd',
  'resolvedAtStart',
  'resolvedAtEnd',
  'resolutionDueByStart',
  'resolutionDueByEnd',
  'closedAtStart',
  'closedAtEnd',
  'itAssetId',
  'assetCode',
  'applicantDeptId',
  'applicantDeptName',
  'applicantBy',
  'applicantName',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof TicketQuery)[]

export type TicketQueryField =
  | (typeof TICKET_QUERY_STRING_FIELDS)[number]
  | 'priority' | 'urgency' | 'impact' | 'ticketSource' | 'ticketStatus'

/** 高级查询抽屉全部字段（含数值） */
export const TICKET_QUERY_FIELDS: readonly TicketQueryField[] = [
  ...TICKET_QUERY_STRING_FIELDS,
  'priority',
  'urgency',
  'impact',
  'ticketSource',
  'ticketStatus',
]

/**
 * 服务台工单实体字段 i18n：index / ticket-form 统一入口
 */
export function useTicketI18n() {
  const ef = useEntityFieldI18n(TICKET_ENTITY_SLUG)

  function ph(field: TicketField): string {
    return ef.placeholder(field, TICKET_PLACEHOLDER[field])
  }

  function queryPh(field: TicketQueryField, kind: EntityFieldPlaceholderKind): string {
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
