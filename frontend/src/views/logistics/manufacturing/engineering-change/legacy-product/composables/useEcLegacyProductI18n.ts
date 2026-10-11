// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/legacy-product/composables
// 文件名称：useEcLegacyProductI18n.ts
// 功能描述：旧品管制字段清单 + useEcLegacyProductI18n（明细字段走 entity.ecdetail.*；生管旧品处理走 entity.eclegacyproduct.oldproducthandling）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcLegacyProductQuery } from '@/types/logistics/manufacturing/engineering-change/legacyProduct.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 旧品管制视图自称 slug（TaktEcLegacyProductI18nSeedData） */
export const ECLEGACYPRODUCT_ENTITY_SLUG = 'eclegacyproduct'

/** 明细字段 slug（TaktEcDetailI18nSeedData） */
export const ECDETAIL_ENTITY_SLUG = 'ecdetail'

/** entity.eclegacyproduct._self */
export const ECLEGACYPRODUCT_SELF_I18N_KEY = buildEntitySelfI18nKey(ECLEGACYPRODUCT_ENTITY_SLUG)

/** 列表业务列（不含主键；对齐 TaktEcLegacyProductDto 小驼峰） */
export const ECLEGACYPRODUCT_LIST_FIELDS = [
  'plantCode',
  'ecCode',
  'lineNumber',
  'ecModelCode',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldUsageQuantity',
  'ecIsCompatible',
  'ec2ndVendor',
  'ecInstruction',
  'ecOldPartDisposition',
  'ecNewMaterialCode',
  'oldProductHandling',
  'discontinuedStatus',
] as const

/** 列表默认可见列（须显式传入，否则 TaktSingleTable 默认仅 8 列，会裁掉第二供应商等） */
export const ECLEGACYPRODUCT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  ...ECLEGACYPRODUCT_LIST_FIELDS,
  'action',
] as const

/** 表单控件默认占位类型 */
export const ECLEGACYPRODUCT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  ecCode: 'required',
  lineNumber: 'required',
  ecModelCode: 'required',
  ecOldMaterialCode: 'optional',
  ecOldMaterialDescription: 'optional',
  ecOldUsageQuantity: 'optional',
  ecIsCompatible: 'optional',
  ec2ndVendor: 'select',
  ecInstruction: 'select',
  ecOldPartDisposition: 'select',
  ecNewMaterialCode: 'optional',
  oldProductHandling: 'select',
  discontinuedStatus: 'select',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段 */
export type EcLegacyProductField = keyof typeof ECLEGACYPRODUCT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段（对齐 QueryDto） */
export const ECLEGACYPRODUCT_QUERY_STRING_FIELDS = [
  'plantCode',
  'cultureCode',
  'ecCode',
  'ecModelCode',
  'ecOldMaterialCode',
] as const satisfies readonly (keyof EcLegacyProductQuery)[]

export type EcLegacyProductQueryField = (typeof ECLEGACYPRODUCT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段 */
export const ECLEGACYPRODUCT_QUERY_FIELDS: readonly EcLegacyProductQueryField[] = [
  ...ECLEGACYPRODUCT_QUERY_STRING_FIELDS,
]

/**
 * 旧品管制字段 i18n：index / legacy-product-form 统一入口
 */
export function useEcLegacyProductI18n() {
  const selfI18n = useEntityFieldI18n(ECLEGACYPRODUCT_ENTITY_SLUG)
  const detailI18n = useEntityFieldI18n(ECDETAIL_ENTITY_SLUG)

  /**
   * 按字段来源选择 i18n 解析器
   * @param field 小驼峰字段名
   */
  function resolverFor(field: string) {
    return field === 'oldProductHandling' ? selfI18n : detailI18n
  }

  /**
   * 业务字段标签
   * @param field DTO 属性 camelCase
   */
  function label(field: string): string {
    return resolverFor(field).label(field)
  }

  /**
   * 高级查询字段标签
   * @param field 查询 DTO 属性 camelCase
   */
  function queryLabel(field: string): string {
    return resolverFor(field).queryLabel(field)
  }

  /**
   * 表单占位符
   * @param field 字段名
   */
  function ph(field: EcLegacyProductField): string {
    return resolverFor(field).placeholder(field, ECLEGACYPRODUCT_PLACEHOLDER[field])
  }

  /**
   * 高级查询占位符
   * @param field 查询字段
   * @param kind 占位类型
   */
  function queryPh(field: EcLegacyProductQueryField, kind: EntityFieldPlaceholderKind): string {
    return resolverFor(field).queryPlaceholder(field, kind)
  }

  return {
    t: selfI18n.t,
    label,
    queryLabel,
    queryPh,
    self: selfI18n.self,
    ph,
  }
}
