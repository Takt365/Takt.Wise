// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/ipqc-order/composables
// 文件名称：useIpqcOrderI18n.ts
// 功能描述：IPQC制程检验单实体字段清单 + useIpqcOrderI18n（字段名映射一次，文案由 entity.ipqcorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { IpqcOrderQuery } from '@/types/logistics/quality/operation/ipqcOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktIpqcOrderI18nSeedData 一致的实体 slug */
export const IPQCORDER_ENTITY_SLUG = 'ipqcorder'

/** entity.ipqcorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const IPQCORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(IPQCORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const IPQCORDER_LIST_FIELDS = [
  'sourceCode',
  'inspectionDate',
  'ipqcOrderCode',
  'processCode',
  'processName',
  'totalProductionQuantity',
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
export const IPQCORDER_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type IpqcOrderField = keyof typeof IPQCORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const IPQCORDER_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof IpqcOrderQuery)[]

export type IpqcOrderQueryField = (typeof IPQCORDER_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const IPQCORDER_QUERY_FIELDS: readonly IpqcOrderQueryField[] = [...IPQCORDER_QUERY_STRING_FIELDS]

/**
 * IPQC制程检验单实体字段 i18n：index / ipqc-order-form 统一入口
 */
export function useIpqcOrderI18n() {
  const ef = useEntityFieldI18n(IPQCORDER_ENTITY_SLUG)

  function ph(field: IpqcOrderField): string {
    return ef.placeholder(field, IPQCORDER_PLACEHOLDER[field])
  }

  function queryPh(field: IpqcOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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
