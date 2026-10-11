// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/composables
// 文件名称：useBillOfMaterialI18n.ts
// 功能描述：Takt物料清单实体字段清单 + useBillOfMaterialI18n（字段名映射一次，文案由 entity.billofmaterial.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BillOfMaterialQuery } from '@/types/logistics/manufacturing/bom/billOfMaterial.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBillOfMaterialI18nSeedData 一致的实体 slug */
export const BILLOFMATERIAL_ENTITY_SLUG = 'billofmaterial'

/** entity.billofmaterial._self 静态属性（导入组件 entity-i18n-key 等） */
export const BILLOFMATERIAL_SELF_I18N_KEY = buildEntitySelfI18nKey(BILLOFMATERIAL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BILLOFMATERIAL_LIST_FIELDS = [
  'bomCode',
  'bomName',
  'parentMaterialCode',
  'parentMaterialDescription',
  'bomVersion',
  'bomType',
  'alternativeBomNumber',
  'effectiveDate',
  'expiryDate',
  'parentMaterialUnit',
  'parentMaterialQuantity',
  'bomDescription',
  'bomGroupCode',
  'bomGroupName',
  'bomStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BILLOFMATERIAL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  bomCode: 'required',
  bomName: 'required',
  parentMaterialCode: 'select',
  parentMaterialDescription: 'optional',
  bomVersion: 'required',
  bomType: 'select',
  alternativeBomNumber: 'required',
  effectiveDate: 'select',
  expiryDate: 'optional',
  parentMaterialUnit: 'select',
  parentMaterialQuantity: 'select',
  bomDescription: 'optional',
  bomGroupCode: 'select',
  bomGroupName: 'optional',
  bomStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BillOfMaterialField = keyof typeof BILLOFMATERIAL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BILLOFMATERIAL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'bomCode',
  'bomName',
  'parentMaterialCode',
  'parentMaterialDescription',
  'bomVersion',
  'alternativeBomNumber',
  'effectiveDateStart',
  'effectiveDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'parentMaterialUnit',
  'bomDescription',
  'bomGroupCode',
  'bomGroupName',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BillOfMaterialQuery)[]

export type BillOfMaterialQueryField =
  | (typeof BILLOFMATERIAL_QUERY_STRING_FIELDS)[number]
  | 'bomType' | 'parentMaterialQuantity' | 'bomStatus'

/** 高级查询抽屉全部字段（含数值） */
export const BILLOFMATERIAL_QUERY_FIELDS: readonly BillOfMaterialQueryField[] = [
  ...BILLOFMATERIAL_QUERY_STRING_FIELDS,
  'bomType',
  'parentMaterialQuantity',
  'bomStatus',
]

/**
 * Takt物料清单实体字段 i18n：index / bill-of-material-form 统一入口
 */
export function useBillOfMaterialI18n() {
  const ef = useEntityFieldI18n(BILLOFMATERIAL_ENTITY_SLUG)

  function ph(field: BillOfMaterialField): string {
    return ef.placeholder(field, BILLOFMATERIAL_PLACEHOLDER[field])
  }

  function queryPh(field: BillOfMaterialQueryField, kind: EntityFieldPlaceholderKind): string {
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
