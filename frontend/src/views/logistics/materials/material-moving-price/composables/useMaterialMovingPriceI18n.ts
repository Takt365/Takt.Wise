// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/material-moving-price/composables
// 文件名称：useMaterialMovingPriceI18n.ts
// 功能描述：移动价格实体 <para>业务唯一键字段清单 + useMaterialMovingPriceI18n（字段名映射一次，文案由 entity.materialmovingprice.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialMovingPriceQuery } from '@/types/logistics/materials/materialMovingPrice.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialMovingPriceI18nSeedData 一致的实体 slug */
export const MATERIALMOVINGPRICE_ENTITY_SLUG = 'materialmovingprice'

/** entity.materialmovingprice._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALMOVINGPRICE_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALMOVINGPRICE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALMOVINGPRICE_LIST_FIELDS = [
  'valuationPeriod',
  'materialCode',
  'valuation',
  'stockQuantity',
  'stockAmount',
  'priceControl',
  'movingPrice',
  'priceUnit',
  'currencyCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALMOVINGPRICE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  valuationPeriod: 'required',
  materialCode: 'select',
  valuation: 'select',
  stockQuantity: 'select',
  stockAmount: 'select',
  priceControl: 'select',
  movingPrice: 'select',
  priceUnit: 'select',
  currencyCode: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialMovingPriceField = keyof typeof MATERIALMOVINGPRICE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALMOVINGPRICE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'valuationPeriod',
  'materialCode',
  'valuation',
  'priceControl',
  'currencyCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaterialMovingPriceQuery)[]

export type MaterialMovingPriceQueryField =
  | (typeof MATERIALMOVINGPRICE_QUERY_STRING_FIELDS)[number]
  | 'stockQuantity' | 'stockAmount' | 'movingPrice' | 'priceUnit'

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALMOVINGPRICE_QUERY_FIELDS: readonly MaterialMovingPriceQueryField[] = [
  ...MATERIALMOVINGPRICE_QUERY_STRING_FIELDS,
  'stockQuantity',
  'stockAmount',
  'movingPrice',
  'priceUnit',
]

/**
 * 移动价格实体 <para>业务唯一键字段 i18n：index / material-moving-price-form 统一入口
 */
export function useMaterialMovingPriceI18n() {
  const ef = useEntityFieldI18n(MATERIALMOVINGPRICE_ENTITY_SLUG)

  function ph(field: MaterialMovingPriceField): string {
    return ef.placeholder(field, MATERIALMOVINGPRICE_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialMovingPriceQueryField, kind: EntityFieldPlaceholderKind): string {
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
