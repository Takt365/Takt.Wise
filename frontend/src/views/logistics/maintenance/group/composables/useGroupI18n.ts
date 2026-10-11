// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/maintenance/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：设备维护组主数据实体字段清单 + useMaintenanceGroupI18n（字段名映射一次，文案由 entity.maintenancegroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaintenanceGroupQuery } from '@/types/logistics/maintenance/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaintenanceGroupI18nSeedData 一致的实体 slug */
export const MAINTENANCEGROUP_ENTITY_SLUG = 'maintenancegroup'

/** entity.maintenancegroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const MAINTENANCEGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(MAINTENANCEGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MAINTENANCEGROUP_LIST_FIELDS = [
  'maintenanceGroupCode',
  'maintenanceGroupName',
  'maintenanceGroupDescription',
  'maintenanceGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MAINTENANCEGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  maintenanceGroupCode: 'required',
  maintenanceGroupName: 'required',
  maintenanceGroupDescription: 'optional',
  maintenanceGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaintenanceGroupField = keyof typeof MAINTENANCEGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MAINTENANCEGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'maintenanceGroupCode',
  'maintenanceGroupName',
  'maintenanceGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaintenanceGroupQuery)[]

export type MaintenanceGroupQueryField =
  | (typeof MAINTENANCEGROUP_QUERY_STRING_FIELDS)[number]
  | 'maintenanceGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const MAINTENANCEGROUP_QUERY_FIELDS: readonly MaintenanceGroupQueryField[] = [
  ...MAINTENANCEGROUP_QUERY_STRING_FIELDS,
  'maintenanceGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 设备维护组主数据实体字段 i18n：index / MaintenanceGroupForm 统一入口
 */
export function useMaintenanceGroupI18n() {
  const ef = useEntityFieldI18n(MAINTENANCEGROUP_ENTITY_SLUG)

  function ph(field: MaintenanceGroupField): string {
    return ef.placeholder(field, MAINTENANCEGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: MaintenanceGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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
