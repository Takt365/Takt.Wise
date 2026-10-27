// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/purchase-sales-inventory/composables
// 文件名称：usePurchaseSalesInventoryI18n.ts
// 功能描述：进销存表实体字段清单 + usePurchaseSalesInventoryI18n（字段名映射一次，文案由 entity.purchasesalesinventory.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseSalesInventoryQuery } from '@/types/accounting/financial/purchaseSalesInventory.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseSalesInventoryI18nSeedData 一致的实体 slug */
export const PURCHASESALESINVENTORY_ENTITY_SLUG = 'purchasesalesinventory'

/** entity.purchasesalesinventory._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASESALESINVENTORY_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASESALESINVENTORY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASESALESINVENTORY_LIST_FIELDS = [
  'plantCode',
  'periodCode',
  'materialCode',
  'materialDescription',
  'valuation',
  'unitCode',
  'openingQty',
  'openingAmount',
  'purchaseQty',
  'purchaseAmount',
  'productionQty',
  'productionAmount',
  'issueQty',
  'issueCostAmount',
  'salesRevenueAmount',
  'adjustQty',
  'adjustAmount',
  'closingQty',
  'closingAmount',
  'closingUnitCost',
  'currencyCode',
  'psiStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASESALESINVENTORY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  periodCode: 'required',
  materialCode: 'select',
  materialDescription: 'required',
  valuation: 'select',
  unitCode: 'optional',
  openingQty: 'select',
  openingAmount: 'select',
  purchaseQty: 'select',
  purchaseAmount: 'select',
  productionQty: 'select',
  productionAmount: 'select',
  issueQty: 'select',
  issueCostAmount: 'select',
  salesRevenueAmount: 'select',
  adjustQty: 'select',
  adjustAmount: 'select',
  closingQty: 'select',
  closingAmount: 'select',
  closingUnitCost: 'select',
  currencyCode: 'select',
  psiStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseSalesInventoryField = keyof typeof PURCHASESALESINVENTORY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASESALESINVENTORY_QUERY_STRING_FIELDS = [
  'periodCode',
  'materialCode',
  'materialDescription',
  'valuation',
  'unitCode',
  'currencyCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseSalesInventoryQuery)[]

export type PurchaseSalesInventoryQueryField =
  | (typeof PURCHASESALESINVENTORY_QUERY_STRING_FIELDS)[number]
  | 'openingQty' | 'openingAmount' | 'purchaseQty' | 'purchaseAmount' | 'productionQty' | 'productionAmount' | 'issueQty' | 'issueCostAmount' | 'salesRevenueAmount' | 'adjustQty' | 'adjustAmount' | 'closingQty' | 'closingAmount' | 'closingUnitCost' | 'psiStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASESALESINVENTORY_QUERY_FIELDS: readonly PurchaseSalesInventoryQueryField[] = [
  ...PURCHASESALESINVENTORY_QUERY_STRING_FIELDS,
  'openingQty',
  'openingAmount',
  'purchaseQty',
  'purchaseAmount',
  'productionQty',
  'productionAmount',
  'issueQty',
  'issueCostAmount',
  'salesRevenueAmount',
  'adjustQty',
  'adjustAmount',
  'closingQty',
  'closingAmount',
  'closingUnitCost',
  'psiStatus',
]

/**
 * 进销存表实体字段 i18n：index / purchase-sales-inventory-form 统一入口
 */
export function usePurchaseSalesInventoryI18n() {
  const ef = useEntityFieldI18n(PURCHASESALESINVENTORY_ENTITY_SLUG)

  function ph(field: PurchaseSalesInventoryField): string {
    return ef.placeholder(field, PURCHASESALESINVENTORY_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseSalesInventoryQueryField, kind: EntityFieldPlaceholderKind): string {
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
