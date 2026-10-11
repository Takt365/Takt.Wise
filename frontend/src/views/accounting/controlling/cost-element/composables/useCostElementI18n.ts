// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/controlling/cost-element/composables
// 文件名称：useCostElementI18n.ts
// 功能描述：成本要素实体字段清单 + useCostElementI18n（字段名映射一次，文案由 entity.costelement.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CostElementQuery } from '@/types/accounting/controlling/costElement.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCostElementI18nSeedData 一致的实体 slug */
export const COSTELEMENT_ENTITY_SLUG = 'costelement'

/** entity.costelement._self 静态属性（导入组件 entity-i18n-key 等） */
export const COSTELEMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(COSTELEMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const COSTELEMENT_LIST_FIELDS = [
  'costElementCode',
  'costElementName',
  'costElementType',
  'costElementCategory',
  'parentId',
  'costElementLevel',
  'validFrom',
  'validTo',
  'costElementStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const COSTELEMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  costElementCode: 'required',
  costElementName: 'required',
  costElementType: 'select',
  costElementCategory: 'select',
  parentId: 'required',
  costElementLevel: 'select',
  validFrom: 'select',
  validTo: 'select',
  costElementStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CostElementField = keyof typeof COSTELEMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const COSTELEMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'costElementCode',
  'costElementName',
  'parentId',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CostElementQuery)[]

export type CostElementQueryField =
  | (typeof COSTELEMENT_QUERY_STRING_FIELDS)[number]
  | 'costElementType' | 'costElementCategory' | 'costElementLevel' | 'costElementStatus'

/** 高级查询抽屉全部字段（含数值） */
export const COSTELEMENT_QUERY_FIELDS: readonly CostElementQueryField[] = [
  ...COSTELEMENT_QUERY_STRING_FIELDS,
  'costElementType',
  'costElementCategory',
  'costElementLevel',
  'costElementStatus',
]

/**
 * 成本要素实体字段 i18n：index / cost-element-form 统一入口
 */
export function useCostElementI18n() {
  const ef = useEntityFieldI18n(COSTELEMENT_ENTITY_SLUG)

  function ph(field: CostElementField): string {
    return ef.placeholder(field, COSTELEMENT_PLACEHOLDER[field])
  }

  function queryPh(field: CostElementQueryField, kind: EntityFieldPlaceholderKind): string {
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
