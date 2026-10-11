// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/work-center/composables
// 文件名称：useWorkCenterResourceI18n.ts
// 功能描述：WorkCenterResource字段清单 + useWorkCenterResourceI18n（字段名映射一次，文案由 entity.workcenterresource.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { WorkCenterResourceQuery } from '@/types/logistics/manufacturing/aps/workCenterResource.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktWorkCenterResourceI18nSeedData 一致的实体 slug */
export const WORKCENTERRESOURCE_ENTITY_SLUG = 'workcenterresource'

/** entity.workcenterresource._self 静态属性（导入组件 entity-i18n-key 等） */
export const WORKCENTERRESOURCE_SELF_I18N_KEY = buildEntitySelfI18nKey(WORKCENTERRESOURCE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const WORKCENTERRESOURCE_LIST_FIELDS = [
  'workCenterCode',
  'resourceCode',
  'resourceName',
  'resourceType',
  'parallelCapacity',
  'efficiencyRate',
  'resourceStatus',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const WORKCENTERRESOURCE_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'workCenterCode',
  'resourceCode',
  'resourceName',
  'resourceType',
  'parallelCapacity',
  'efficiencyRate',
  'resourceStatus',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const WORKCENTERRESOURCE_SUMMARY_SUM_FIELDS = [
  'resourceType',
  'parallelCapacity',
  'efficiencyRate',
  'resourceStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const WORKCENTERRESOURCE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type WorkCenterResourceField = keyof typeof WORKCENTERRESOURCE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const WORKCENTERRESOURCE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof WorkCenterResourceQuery)[]

export type WorkCenterResourceQueryField = (typeof WORKCENTERRESOURCE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const WORKCENTERRESOURCE_QUERY_FIELDS: readonly WorkCenterResourceQueryField[] = [...WORKCENTERRESOURCE_QUERY_STRING_FIELDS]

/**
 * WorkCenterResource字段 i18n：index / work-center-resource-form 统一入口
 */
export function useWorkCenterResourceI18n() {
  const ef = useEntityFieldI18n(WORKCENTERRESOURCE_ENTITY_SLUG)

  function ph(field: WorkCenterResourceField): string {
    return ef.placeholder(field, WORKCENTERRESOURCE_PLACEHOLDER[field])
  }

  function queryPh(field: WorkCenterResourceQueryField, kind: EntityFieldPlaceholderKind): string {
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
