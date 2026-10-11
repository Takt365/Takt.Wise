// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/material-cost/composables
// 文件名称：useMaterialCostI18n.ts
// 功能描述：BOM 物料成本汇总表字段清单 + useBomMaterialCostI18n（字段名映射一次，文案由 entity.bommaterialcost.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BomMaterialCostQuery } from '@/types/logistics/manufacturing/bom/materialCost.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBomMaterialCostI18nSeedData 一致的实体 slug */
export const BOMMATERIALCOST_ENTITY_SLUG = 'bommaterialcost'

/** entity.bommaterialcost._self 静态属性（导入组件 entity-i18n-key 等） */
export const BOMMATERIALCOST_SELF_I18N_KEY = buildEntitySelfI18nKey(BOMMATERIALCOST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BOMMATERIALCOST_LIST_FIELDS = [
  'plantCode',
  'modelCode',
  'modelMonthlyAverageCost',
  'materialType',
  'productCode',
  'productDescription',
  'productMonthlyCost',
  'productMonthlyCalculation',
  'latestPurchaseCost',
  'currencyCode',
  'costingPeriod',
  'costingDate',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BOMMATERIALCOST_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  modelCode: 'required',
  modelMonthlyAverageCost: 'select',
  materialType: 'select',
  productCode: 'select',
  productDescription: 'optional',
  productMonthlyCost: 'select',
  productMonthlyCalculation: 'select',
  latestPurchaseCost: 'select',
  currencyCode: 'select',
  costingPeriod: 'select',
  costingDate: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BomMaterialCostField = keyof typeof BOMMATERIALCOST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BOMMATERIALCOST_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'modelCode',
  'materialType',
  'productCode',
  'productDescription',
  'currencyCode',
  'costingPeriod',
  'costingDateStart',
  'costingDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BomMaterialCostQuery)[]

export type BomMaterialCostQueryField =
  | (typeof BOMMATERIALCOST_QUERY_STRING_FIELDS)[number]
  | 'modelMonthlyAverageCost' | 'productMonthlyCost' | 'productMonthlyCalculation' | 'latestPurchaseCost'

/** 高级查询抽屉全部字段（含数值） */
export const BOMMATERIALCOST_QUERY_FIELDS: readonly BomMaterialCostQueryField[] = [
  ...BOMMATERIALCOST_QUERY_STRING_FIELDS,
  'modelMonthlyAverageCost',
  'productMonthlyCost',
  'productMonthlyCalculation',
  'latestPurchaseCost',
]

/**
 * BOM 物料成本汇总表字段 i18n：index / material-cost-form 统一入口
 */
export function useBomMaterialCostI18n() {
  const ef = useEntityFieldI18n(BOMMATERIALCOST_ENTITY_SLUG)

  function ph(field: BomMaterialCostField): string {
    return ef.placeholder(field, BOMMATERIALCOST_PLACEHOLDER[field])
  }

  function queryPh(field: BomMaterialCostQueryField, kind: EntityFieldPlaceholderKind): string {
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
