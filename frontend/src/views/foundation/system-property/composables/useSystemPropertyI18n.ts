// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/system-property/composables
// 文件名称：useSystemPropertyI18n.ts
// 功能描述：系统属性实体 存储系统的各种配置参数字段清单 + useSystemPropertyI18n（字段名映射一次，文案由 entity.systemproperty.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SystemPropertyQuery } from '@/types/foundation/systemProperty.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSystemPropertyI18nSeedData 一致的实体 slug */
export const SYSTEMPROPERTY_ENTITY_SLUG = 'systemproperty'

/** entity.systemproperty._self 静态属性（导入组件 entity-i18n-key 等） */
export const SYSTEMPROPERTY_SELF_I18N_KEY = buildEntitySelfI18nKey(SYSTEMPROPERTY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SYSTEMPROPERTY_LIST_FIELDS = [
  'systemPropertyKey',
  'systemPropertyValue',
  'systemPropertyName',
  'systemPropertyDescription',
  'systemPropertyGroup',
  'valueType',
  'isBuiltIn',
  'isReadonly',
  'isEncrypted',
  'systemPropertyStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SYSTEMPROPERTY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  systemPropertyKey: 'required',
  systemPropertyValue: 'optional',
  systemPropertyName: 'required',
  systemPropertyDescription: 'optional',
  systemPropertyGroup: 'select',
  valueType: 'select',
  isBuiltIn: 'select',
  isReadonly: 'select',
  isEncrypted: 'select',
  systemPropertyStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SystemPropertyField = keyof typeof SYSTEMPROPERTY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SYSTEMPROPERTY_QUERY_STRING_FIELDS = [
  'cultureCode',
  'systemPropertyKey',
  'systemPropertyValue',
  'systemPropertyName',
  'systemPropertyDescription',
  'systemPropertyGroup',
  'valueType',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SystemPropertyQuery)[]

export type SystemPropertyQueryField =
  | (typeof SYSTEMPROPERTY_QUERY_STRING_FIELDS)[number]
  | 'isBuiltIn' | 'isReadonly' | 'isEncrypted' | 'systemPropertyStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SYSTEMPROPERTY_QUERY_FIELDS: readonly SystemPropertyQueryField[] = [
  ...SYSTEMPROPERTY_QUERY_STRING_FIELDS,
  'isBuiltIn',
  'isReadonly',
  'isEncrypted',
  'systemPropertyStatus',
]

/**
 * 系统属性实体 存储系统的各种配置参数字段 i18n：index / SystemPropertyForm 统一入口
 */
export function useSystemPropertyI18n() {
  const ef = useEntityFieldI18n(SYSTEMPROPERTY_ENTITY_SLUG)

  function ph(field: SystemPropertyField): string {
    return ef.placeholder(field, SYSTEMPROPERTY_PLACEHOLDER[field])
  }

  function queryPh(field: SystemPropertyQueryField, kind: EntityFieldPlaceholderKind): string {
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
