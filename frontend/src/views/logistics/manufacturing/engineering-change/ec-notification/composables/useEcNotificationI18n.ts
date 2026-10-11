// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-notification/composables
// 文件名称：useEcNotificationI18n.ts
// 功能描述：工程变更通知单字段清单 + useEcNotificationI18n（字段名小驼峰，文案由 entity.ecnotification.* 种子动态解析）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcNotificationI18nSeedData 一致的实体 slug */
export const ECNOTIFICATION_ENTITY_SLUG = 'ecnotification'

/** entity.ecnotification._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECNOTIFICATION_SELF_I18N_KEY = buildEntitySelfI18nKey(ECNOTIFICATION_ENTITY_SLUG)

/** 列表业务列（不含主键；对齐 TaktEcNotification 属性小驼峰） */
export const ECNOTIFICATION_LIST_FIELDS = [
  'ecNotificationCode',
  'ecGijutsuId',
  'ecCode',
  'ecTitle',
  'ecNotificationDate',
  'ecNotificationDeptCodes',
  'ecNotificationDeptNames',
  'ecNotificationNotifierGroupCode',
  'ecNotificationNotifierGroupName',
  'ecNotificationMethod',
  'ecNotificationStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECNOTIFICATION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  ecNotificationCode: 'required',
  ecGijutsuId: 'required',
  ecCode: 'required',
  ecTitle: 'optional',
  ecNotificationDate: 'select',
  ecNotificationDeptCodes: 'optional',
  ecNotificationDeptNames: 'optional',
  ecNotificationNotifierGroupCode: 'select',
  ecNotificationNotifierGroupName: 'optional',
  ecNotificationMethod: 'select',
  ecNotificationStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段 */
export type EcNotificationField = keyof typeof ECNOTIFICATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段（对齐 QueryDto 小驼峰） */
export const ECNOTIFICATION_QUERY_STRING_FIELDS = [
  'plantCode',
  'ecNotificationCode',
  'ecGijutsuId',
  'ecCode',
  'ecTitle',
  'ecNotificationDateStart',
  'ecNotificationDateEnd',
  'ecNotificationDeptCodes',
  'ecNotificationDeptNames',
  'ecNotificationNotifierGroupCode',
  'ecNotificationNotifierGroupName',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const

export type EcNotificationQueryField =
  | (typeof ECNOTIFICATION_QUERY_STRING_FIELDS)[number]
  | 'ecNotificationMethod'
  | 'ecNotificationStatus'
  | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ECNOTIFICATION_QUERY_FIELDS: readonly EcNotificationQueryField[] = [
  ...ECNOTIFICATION_QUERY_STRING_FIELDS,
  'ecNotificationMethod',
  'ecNotificationStatus',
  'approvalStatus',
]

/**
 * 工程变更通知单字段 i18n：index / ec-notification-form 统一入口
 */
export function useEcNotificationI18n() {
  const ef = useEntityFieldI18n(ECNOTIFICATION_ENTITY_SLUG)

  function ph(field: EcNotificationField): string {
    return ef.placeholder(field, ECNOTIFICATION_PLACEHOLDER[field])
  }

  function queryPh(field: EcNotificationQueryField, kind: EntityFieldPlaceholderKind): string {
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
