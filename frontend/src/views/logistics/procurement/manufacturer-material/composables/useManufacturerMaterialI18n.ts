// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/manufacturer-material/composables
// 文件名称：useManufacturerMaterialI18n.ts
// 功能描述：Takt制造商物料实体字段清单 + useManufacturerMaterialI18n（字段名映射一次，文案由 entity.manufacturermaterial.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ManufacturerMaterialQuery } from '@/types/logistics/procurement/manufacturerMaterial.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktManufacturerMaterialI18nSeedData 一致的实体 slug */
export const MANUFACTURERMATERIAL_ENTITY_SLUG = 'manufacturermaterial'

/** entity.manufacturermaterial._self 静态属性（导入组件 entity-i18n-key 等） */
export const MANUFACTURERMATERIAL_SELF_I18N_KEY = buildEntitySelfI18nKey(MANUFACTURERMATERIAL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MANUFACTURERMATERIAL_LIST_FIELDS = [
  'vendorCode',
  'vendorShortName',
  'supplierCode',
  'supplierShortName',
  'materialType',
  'materialGroup',
  'internalMaterialCode',
  'materialCode',
  'materialDescription',
  'manufacturerMaterialCode',
  'manufacturerMaterialDescription',
  'manufacturerMaterialSpecification',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MANUFACTURERMATERIAL_PLACEHOLDER = {
  tenantCode: 'optional',
  vendorCode: 'optional',
  vendorShortName: 'optional',
  supplierCode: 'optional',
  supplierShortName: 'optional',
  materialType: 'select',
  materialGroup: 'select',
  internalMaterialCode: 'required',
  materialCode: 'select',
  materialDescription: 'optional',
  manufacturerMaterialCode: 'required',
  manufacturerMaterialDescription: 'optional',
  manufacturerMaterialSpecification: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ManufacturerMaterialField = keyof typeof MANUFACTURERMATERIAL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MANUFACTURERMATERIAL_QUERY_STRING_FIELDS = [
  'vendorCode',
  'vendorShortName',
  'supplierCode',
  'supplierShortName',
  'materialType',
  'materialGroup',
  'internalMaterialCode',
  'materialCode',
  'materialDescription',
  'manufacturerMaterialCode',
  'manufacturerMaterialDescription',
  'manufacturerMaterialSpecification',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ManufacturerMaterialQuery)[]

export type ManufacturerMaterialQueryField = (typeof MANUFACTURERMATERIAL_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MANUFACTURERMATERIAL_QUERY_FIELDS: readonly ManufacturerMaterialQueryField[] = [...MANUFACTURERMATERIAL_QUERY_STRING_FIELDS]

/**
 * Takt制造商物料实体字段 i18n：index / manufacturer-material-form 统一入口
 */
export function useManufacturerMaterialI18n() {
  const ef = useEntityFieldI18n(MANUFACTURERMATERIAL_ENTITY_SLUG)

  function ph(field: ManufacturerMaterialField): string {
    return ef.placeholder(field, MANUFACTURERMATERIAL_PLACEHOLDER[field])
  }

  function queryPh(field: ManufacturerMaterialQueryField, kind: EntityFieldPlaceholderKind): string {
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
