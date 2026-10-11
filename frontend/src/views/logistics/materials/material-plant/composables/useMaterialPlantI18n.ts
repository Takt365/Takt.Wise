// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/material-plant/composables
// 文件名称：useMaterialPlantI18n.ts
// 功能描述：Takt工厂物料实体字段清单 + useMaterialPlantI18n（字段名映射一次，文案由 entity.materialplant.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialPlantQuery } from '@/types/logistics/materials/materialPlant.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialPlantI18nSeedData 一致的实体 slug */
export const MATERIALPLANT_ENTITY_SLUG = 'materialplant'

/** entity.materialplant._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALPLANT_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALPLANT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALPLANT_LIST_FIELDS = [
  'plantCode',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'industrySector',
  'materialHierarchy',
  'materialGroup',
  'materialType',
  'baseUnit',
  'purchaseGroup',
  'purchaseType',
  'specialProcurement',
  'isBulk',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'inHouseProductionDays',
  'manufacturer',
  'manufacturerMaterialCode',
  'currencyCode',
  'priceControl',
  'priceUnit',
  'valuation',
  'movingPrice',
  'differenceCode',
  'profitCenter',
  'currentStock',
  'productionLocation',
  'purchasingLocation',
  'storageLocation',
  'requiresInspection',
  'isBatch',
  'discontinuedStatus',
  'materialStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALPLANT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  industrySector: 'select',
  materialHierarchy: 'optional',
  materialGroup: 'select',
  materialType: 'select',
  baseUnit: 'select',
  purchaseGroup: 'select',
  purchaseType: 'select',
  specialProcurement: 'select',
  isBulk: 'select',
  minOrderQuantity: 'select',
  roundingValue: 'select',
  plannedDeliveryTimeDays: 'select',
  inHouseProductionDays: 'select',
  manufacturer: 'optional',
  manufacturerMaterialCode: 'optional',
  currencyCode: 'select',
  priceControl: 'select',
  priceUnit: 'select',
  valuation: 'select',
  movingPrice: 'select',
  differenceCode: 'optional',
  profitCenter: 'select',
  currentStock: 'select',
  productionLocation: 'select',
  purchasingLocation: 'select',
  storageLocation: 'select',
  requiresInspection: 'select',
  isBatch: 'select',
  discontinuedStatus: 'select',
  materialStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialPlantField = keyof typeof MATERIALPLANT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALPLANT_QUERY_STRING_FIELDS = [
  'plantCode',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'industrySector',
  'materialHierarchy',
  'materialGroup',
  'materialType',
  'baseUnit',
  'purchaseGroup',
  'purchaseType',
  'manufacturer',
  'manufacturerMaterialCode',
  'currencyCode',
  'priceControl',
  'valuation',
  'differenceCode',
  'profitCenter',
  'productionLocation',
  'purchasingLocation',
  'storageLocation',
  'discontinuedStatus',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaterialPlantQuery)[]

export type MaterialPlantQueryField =
  | (typeof MATERIALPLANT_QUERY_STRING_FIELDS)[number]
  | 'specialProcurement' | 'isBulk' | 'minOrderQuantity' | 'roundingValue' | 'plannedDeliveryTimeDays' | 'inHouseProductionDays' | 'priceUnit' | 'movingPrice' | 'currentStock' | 'requiresInspection' | 'isBatch' | 'materialStatus'

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALPLANT_QUERY_FIELDS: readonly MaterialPlantQueryField[] = [
  ...MATERIALPLANT_QUERY_STRING_FIELDS,
  'specialProcurement',
  'isBulk',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'inHouseProductionDays',
  'priceUnit',
  'movingPrice',
  'currentStock',
  'requiresInspection',
  'isBatch',
  'materialStatus',
]

/**
 * Takt工厂物料实体字段 i18n：index / material-plant-form 统一入口
 */
export function useMaterialPlantI18n() {
  const ef = useEntityFieldI18n(MATERIALPLANT_ENTITY_SLUG)

  function ph(field: MaterialPlantField): string {
    return ef.placeholder(field, MATERIALPLANT_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialPlantQueryField, kind: EntityFieldPlaceholderKind): string {
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
