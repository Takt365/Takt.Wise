// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/material-cost/composables
// 文件名称：useMaterialCostItemI18n.ts
// 功能描述：BomMaterialCostItem字段清单 + useBomMaterialCostItemI18n（字段名映射一次，文案由 entity.bommaterialcostitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BomMaterialCostItemQuery } from '@/types/logistics/manufacturing/bom/materialCostItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBomMaterialCostItemI18nSeedData 一致的实体 slug */
export const BOMMATERIALCOSTITEM_ENTITY_SLUG = 'bommaterialcostitem'

/** entity.bommaterialcostitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const BOMMATERIALCOSTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(BOMMATERIALCOSTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BOMMATERIALCOSTITEM_LIST_FIELDS = [
  'plantCode',
  'bomLevel',
  'bomItemCode',
  'productCode',
  'lineNumber',
  'productDescription',
  'componentCode',
  'componentDescription',
  'componentQuantity',
  'batchIndicator',
  'productionRelated',
  'purchaseType',
  'specialProcurementType',
  'profitCenterCode',
  'movingAveragePrice',
  'movingPriceUnit',
  'movingPriceCurrencyCode',
  'purchaseOrganization',
  'purchaseGroup',
  'supplierCode',
  'netPurchasePrice',
  'purchasePriceUnit',
  'purchaseCurrencyCode',
  'costingDate',
] as const

/** 明细右栏 panel 默认展示列（浏览核心三列） */
export const BOMMATERIALCOSTITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'componentCode',
  'componentQuantity',
  'movingAveragePrice',
] as const

/**
 * 明细右栏需展示公式合计的列（movingAveragePrice 列展示 Σ qty×(price/unit)，口径 X/F）
 */
export const BOMMATERIALCOSTITEM_SUMMARY_SUM_FIELDS = ['movingAveragePrice'] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BOMMATERIALCOSTITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  bomLevel: 'required',
  bomItemCode: 'required',
  productCode: 'required',
  lineNumber: 'required',
  productDescription: 'optional',
  componentCode: 'required',
  componentDescription: 'optional',
  componentQuantity: 'select',
  batchIndicator: 'optional',
  productionRelated: 'optional',
  purchaseType: 'required',
  specialProcurementType: 'optional',
  profitCenterCode: 'select',
  movingAveragePrice: 'select',
  movingPriceUnit: 'select',
  movingPriceCurrencyCode: 'select',
  purchaseOrganization: 'required',
  purchaseGroup: 'select',
  supplierCode: 'select',
  netPurchasePrice: 'select',
  purchasePriceUnit: 'select',
  purchaseCurrencyCode: 'select',
  costingDate: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BomMaterialCostItemField = keyof typeof BOMMATERIALCOSTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS = [
  'plantCode',
  'bomLevel',
  'bomItemCode',
  'productCode',
  'productDescription',
  'componentCode',
  'componentDescription',
  'batchIndicator',
  'productionRelated',
  'purchaseType',
  'specialProcurementType',
  'profitCenterCode',
  'movingPriceCurrencyCode',
  'purchaseOrganization',
  'purchaseGroup',
  'supplierCode',
  'purchaseCurrencyCode',
  'costingDateStart',
  'costingDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BomMaterialCostItemQuery)[]

export type BomMaterialCostItemQueryField =
  | (typeof BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'componentQuantity' | 'movingAveragePrice' | 'movingPriceUnit' | 'netPurchasePrice' | 'purchasePriceUnit'

/** 高级查询抽屉全部字段（含数值） */
export const BOMMATERIALCOSTITEM_QUERY_FIELDS: readonly BomMaterialCostItemQueryField[] = [
  ...BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'componentQuantity',
  'movingAveragePrice',
  'movingPriceUnit',
  'netPurchasePrice',
  'purchasePriceUnit',
]

/**
 * BomMaterialCostItem字段 i18n：index / material-cost-item-form 统一入口
 */
export function useBomMaterialCostItemI18n() {
  const ef = useEntityFieldI18n(BOMMATERIALCOSTITEM_ENTITY_SLUG)

  function ph(field: BomMaterialCostItemField): string {
    return ef.placeholder(field, BOMMATERIALCOSTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: BomMaterialCostItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
