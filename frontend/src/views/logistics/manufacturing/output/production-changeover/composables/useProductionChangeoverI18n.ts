// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/production-changeover/composables
// 文件名称：useProductionChangeoverI18n.ts
// 功能描述：生产切换记录实体字段清单 + useProductionChangeoverI18n（字段名映射一次，文案由 entity.productionchangeover.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionChangeoverQuery } from '@/types/logistics/manufacturing/output/productionChangeover.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktProductionChangeoverI18nSeedData 一致的实体 slug */
export const PRODUCTIONCHANGEOVER_ENTITY_SLUG = 'productionchangeover'

/** entity.productionchangeover._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONCHANGEOVER_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONCHANGEOVER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONCHANGEOVER_LIST_FIELDS = [
  'plantCode',
  'prodCategory',
  'changeoverCategory',
  'prodDate',
  'TeamCode',
  'currentProdOrderCode',
  'currentModelCode',
  'changeoverProdOrderCode',
  'changeoverModelCode',
  'changeoverCount',
  'changeoverTime',
  'instrumentSetupTime',
  'totalChangeoverTime',
  'readSopTime',
  'learningTime',
  'personCount',
  'totalLearningTime',
  'totalSopTime',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONCHANGEOVER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'required',
  prodCategory: 'optional',
  changeoverCategory: 'select',
  prodDate: 'select',
  TeamCode: 'optional',
  currentProdOrderCode: 'select',
  currentModelCode: 'optional',
  changeoverProdOrderCode: 'select',
  changeoverModelCode: 'optional',
  changeoverCount: 'select',
  changeoverTime: 'select',
  instrumentSetupTime: 'select',
  totalChangeoverTime: 'select',
  readSopTime: 'select',
  learningTime: 'select',
  personCount: 'select',
  totalLearningTime: 'select',
  totalSopTime: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionChangeoverField = keyof typeof PRODUCTIONCHANGEOVER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONCHANGEOVER_QUERY_STRING_FIELDS = [
  'prodCategory',
  'changeoverCategory',
  'prodDateStart',
  'prodDateEnd',
  'TeamCode',
  'currentProdOrderCode',
  'currentModelCode',
  'changeoverProdOrderCode',
  'changeoverModelCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionChangeoverQuery)[]

export type ProductionChangeoverQueryField =
  | (typeof PRODUCTIONCHANGEOVER_QUERY_STRING_FIELDS)[number]
  | 'changeoverCount' | 'changeoverTime' | 'instrumentSetupTime' | 'totalChangeoverTime' | 'readSopTime' | 'learningTime' | 'personCount' | 'totalLearningTime' | 'totalSopTime'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONCHANGEOVER_QUERY_FIELDS: readonly ProductionChangeoverQueryField[] = [
  ...PRODUCTIONCHANGEOVER_QUERY_STRING_FIELDS,
  'changeoverCount',
  'changeoverTime',
  'instrumentSetupTime',
  'totalChangeoverTime',
  'readSopTime',
  'learningTime',
  'personCount',
  'totalLearningTime',
  'totalSopTime',
]

/**
 * 生产切换记录实体字段 i18n：index / production-changeover-form 统一入口
 */
export function useProductionChangeoverI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONCHANGEOVER_ENTITY_SLUG)

  function ph(field: ProductionChangeoverField): string {
    return ef.placeholder(field, PRODUCTIONCHANGEOVER_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionChangeoverQueryField, kind: EntityFieldPlaceholderKind): string {
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
