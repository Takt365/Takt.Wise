// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/asset/composables
// 文件名称：useAssetI18n.ts
// 功能描述：资产实体字段清单 + useAssetI18n（字段名映射一次，文案由 entity.asset.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssetQuery } from '@/types/accounting/financial/asset.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssetI18nSeedData 一致的实体 slug */
export const ASSET_ENTITY_SLUG = 'asset'

/** entity.asset._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSET_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSET_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSET_LIST_FIELDS = [
  'assetCode',
  'assetName',
  'assetCategory',
  'assetType',
  'assetOriginalValue',
  'assetNetValue',
  'accumulatedDepreciation',
  'costCenterId',
  'costCenterName',
  'deptId',
  'deptName',
  'userId',
  'userName',
  'assetLocation',
  'purchaseDate',
  'startDate',
  'scrapDate',
  'disposalDate',
  'expectedLifeMonths',
  'depreciationMethod',
  'monthlyDepreciation',
  'assetStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSET_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  assetCode: 'required',
  assetName: 'required',
  assetCategory: 'select',
  assetType: 'select',
  assetOriginalValue: 'select',
  assetNetValue: 'select',
  accumulatedDepreciation: 'select',
  costCenterId: 'optional',
  costCenterName: 'optional',
  deptId: 'optional',
  deptName: 'optional',
  userId: 'optional',
  userName: 'optional',
  assetLocation: 'optional',
  purchaseDate: 'optional',
  startDate: 'optional',
  scrapDate: 'optional',
  disposalDate: 'optional',
  expectedLifeMonths: 'select',
  depreciationMethod: 'select',
  monthlyDepreciation: 'select',
  assetStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssetField = keyof typeof ASSET_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSET_QUERY_STRING_FIELDS = [
  'cultureCode',
  'assetCode',
  'assetName',
  'assetCategory',
  'assetType',
  'costCenterId',
  'costCenterName',
  'deptId',
  'deptName',
  'userId',
  'userName',
  'assetLocation',
  'purchaseDateStart',
  'purchaseDateEnd',
  'startDateStart',
  'startDateEnd',
  'scrapDateStart',
  'scrapDateEnd',
  'disposalDateStart',
  'disposalDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssetQuery)[]

export type AssetQueryField =
  | (typeof ASSET_QUERY_STRING_FIELDS)[number]
  | 'assetOriginalValue' | 'assetNetValue' | 'accumulatedDepreciation' | 'expectedLifeMonths' | 'depreciationMethod' | 'monthlyDepreciation' | 'assetStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ASSET_QUERY_FIELDS: readonly AssetQueryField[] = [
  ...ASSET_QUERY_STRING_FIELDS,
  'assetOriginalValue',
  'assetNetValue',
  'accumulatedDepreciation',
  'expectedLifeMonths',
  'depreciationMethod',
  'monthlyDepreciation',
  'assetStatus',
]

/**
 * 资产实体字段 i18n：index / asset-form 统一入口
 */
export function useAssetI18n() {
  const ef = useEntityFieldI18n(ASSET_ENTITY_SLUG)

  function ph(field: AssetField): string {
    return ef.placeholder(field, ASSET_PLACEHOLDER[field])
  }

  function queryPh(field: AssetQueryField, kind: EntityFieldPlaceholderKind): string {
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
