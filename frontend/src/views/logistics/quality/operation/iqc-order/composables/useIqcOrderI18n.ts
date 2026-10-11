// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/iqc-order/composables
// 文件名称：useIqcOrderI18n.ts
// 功能描述：IQC进货检验单实体字段清单 + useIqcOrderI18n（字段名映射一次，文案由 entity.iqcorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { IqcOrderQuery } from '@/types/logistics/quality/operation/iqcOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktIqcOrderI18nSeedData 一致的实体 slug */
export const IQCORDER_ENTITY_SLUG = 'iqcorder'

/** entity.iqcorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const IQCORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(IQCORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const IQCORDER_LIST_FIELDS = [
  'sourceCode',
  'inspectionDate',
  'iqcOrderCode',
  'supplierCode',
  'totalPurchaseQuantity',
  'totalSampleQuantity',
  'totalQualifiedQuantity',
  'totalUnqualifiedQuantity',
  'totalInspectionReturnQuantity',
  'judgeBy',
  'judgeDate',
  'judgeDescription',
  'judgeStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const IQCORDER_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type IqcOrderField = keyof typeof IQCORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const IQCORDER_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof IqcOrderQuery)[]

export type IqcOrderQueryField = (typeof IQCORDER_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const IQCORDER_QUERY_FIELDS: readonly IqcOrderQueryField[] = [...IQCORDER_QUERY_STRING_FIELDS]

/**
 * IQC进货检验单实体字段 i18n：index / iqc-order-form 统一入口
 */
export function useIqcOrderI18n() {
  const ef = useEntityFieldI18n(IQCORDER_ENTITY_SLUG)

  function ph(field: IqcOrderField): string {
    return ef.placeholder(field, IQCORDER_PLACEHOLDER[field])
  }

  function queryPh(field: IqcOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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
