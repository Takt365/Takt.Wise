// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mds/sales-forecast/composables
// 文件名称：useSalesForecastI18n.ts
// 功能描述：Takt销售预测实体字段清单 + useSalesForecastI18n（字段名映射一次，文案由 entity.salesforecast.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesForecastQuery } from '@/types/logistics/manufacturing/mds/salesForecast.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesForecastI18nSeedData 一致的实体 slug */
export const SALESFORECAST_ENTITY_SLUG = 'salesforecast'

/** entity.salesforecast._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESFORECAST_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESFORECAST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESFORECAST_LIST_FIELDS = [
  'salesForecastCode',
  'planDate',
  'receiveDate',
  'receiveVersionNo',
  'salesProduct',
  'productCategoryCode',
  'profitCenterCode',
  'modelCode',
  'materialCode',
  'materialDescription',
  'customerCode',
  'customerName1',
  'plannerGroupCode',
  'planBy',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'planDescription',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESFORECAST_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesForecastField = keyof typeof SALESFORECAST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESFORECAST_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof SalesForecastQuery)[]

export type SalesForecastQueryField = (typeof SALESFORECAST_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SALESFORECAST_QUERY_FIELDS: readonly SalesForecastQueryField[] = [...SALESFORECAST_QUERY_STRING_FIELDS]

/**
 * Takt销售预测实体字段 i18n：index / sales-forecast-form 统一入口
 */
export function useSalesForecastI18n() {
  const ef = useEntityFieldI18n(SALESFORECAST_ENTITY_SLUG)

  function ph(field: SalesForecastField): string {
    return ef.placeholder(field, SALESFORECAST_PLACEHOLDER[field])
  }

  function queryPh(field: SalesForecastQueryField, kind: EntityFieldPlaceholderKind): string {
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
