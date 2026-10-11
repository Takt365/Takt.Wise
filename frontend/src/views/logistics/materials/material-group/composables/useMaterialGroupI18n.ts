// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/material-group/composables
// 文件名称：useMaterialGroupI18n.ts
// 功能描述：Takt物料组主数据实体字段清单 + useMaterialGroupI18n（字段名映射一次，文案由 entity.materialgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialGroupQuery } from '@/types/logistics/materials/materialGroup.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialGroupI18nSeedData 一致的实体 slug */
export const MATERIALGROUP_ENTITY_SLUG = 'materialgroup'

/** entity.materialgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALGROUP_LIST_FIELDS = [
  'materialGroupCode',
  'materialGroupName',
  'materialGroupDescription',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  materialGroupCode: 'required',
  materialGroupName: 'required',
  materialGroupDescription: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialGroupField = keyof typeof MATERIALGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALGROUP_QUERY_STRING_FIELDS = [
  'materialGroupCode',
  'materialGroupName',
  'materialGroupDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaterialGroupQuery)[]

export type MaterialGroupQueryField = (typeof MATERIALGROUP_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALGROUP_QUERY_FIELDS: readonly MaterialGroupQueryField[] = [...MATERIALGROUP_QUERY_STRING_FIELDS]

/**
 * Takt物料组主数据实体字段 i18n：index / material-group-form 统一入口
 */
export function useMaterialGroupI18n() {
  const ef = useEntityFieldI18n(MATERIALGROUP_ENTITY_SLUG)

  function ph(field: MaterialGroupField): string {
    return ef.placeholder(field, MATERIALGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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
