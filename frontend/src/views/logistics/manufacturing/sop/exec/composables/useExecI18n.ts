// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/exec/composables
// 文件名称：useExecI18n.ts
// 功能描述：SOP 工位执行追溯实体字段清单 + useSopExecI18n（字段名映射一次，文案由 entity.sopexec.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopExecQuery } from '@/types/logistics/manufacturing/sop/exec.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopExecI18nSeedData 一致的实体 slug */
export const SOPEXEC_ENTITY_SLUG = 'sopexec'

/** entity.sopexec._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPEXEC_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPEXEC_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPEXEC_LIST_FIELDS = [
  'productionOrderId',
  'workOrderCode',
  'serialNumber',
  'materialCode',
  'routingItemId',
  'processSegmentType',
  'workstationId',
  'employeeId',
  'sopId',
  'revisionId',
  'revision',
  'startedAt',
  'endedAt',
  'selfCheckResult',
  'execStatus',
  'currentStepId',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPEXEC_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  productionOrderId: 'optional',
  workOrderCode: 'optional',
  serialNumber: 'optional',
  materialCode: 'select',
  routingItemId: 'select',
  processSegmentType: 'select',
  workstationId: 'select',
  employeeId: 'select',
  sopId: 'select',
  revisionId: 'select',
  revision: 'required',
  startedAt: 'select',
  endedAt: 'optional',
  selfCheckResult: 'optional',
  execStatus: 'select',
  currentStepId: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopExecField = keyof typeof SOPEXEC_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPEXEC_QUERY_STRING_FIELDS = [
  'cultureCode',
  'productionOrderId',
  'workOrderCode',
  'serialNumber',
  'materialCode',
  'routingItemId',
  'workstationId',
  'employeeId',
  'sopId',
  'revisionId',
  'revision',
  'startedAtStart',
  'startedAtEnd',
  'endedAtStart',
  'endedAtEnd',
  'currentStepId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopExecQuery)[]

export type SopExecQueryField =
  | (typeof SOPEXEC_QUERY_STRING_FIELDS)[number]
  | 'processSegmentType' | 'selfCheckResult' | 'execStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SOPEXEC_QUERY_FIELDS: readonly SopExecQueryField[] = [
  ...SOPEXEC_QUERY_STRING_FIELDS,
  'processSegmentType',
  'selfCheckResult',
  'execStatus',
]

/**
 * SOP 工位执行追溯实体字段 i18n：index / exec-form 统一入口
 */
export function useSopExecI18n() {
  const ef = useEntityFieldI18n(SOPEXEC_ENTITY_SLUG)

  function ph(field: SopExecField): string {
    return ef.placeholder(field, SOPEXEC_PLACEHOLDER[field])
  }

  function queryPh(field: SopExecQueryField, kind: EntityFieldPlaceholderKind): string {
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
