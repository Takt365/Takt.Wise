// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/seller-material/composables
// 文件名称：useSellerMaterialI18n.ts
// 功能描述：Takt销售商物料实体字段清单 + useSellerMaterialI18n（字段名映射一次，文案由 entity.sellermaterial.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SellerMaterialQuery } from '@/types/logistics/sales/sellerMaterial.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSellerMaterialI18nSeedData 一致的实体 slug */
export const SELLERMATERIAL_ENTITY_SLUG = 'sellermaterial'

/** entity.sellermaterial._self 静态属性（导入组件 entity-i18n-key 等） */
export const SELLERMATERIAL_SELF_I18N_KEY = buildEntitySelfI18nKey(SELLERMATERIAL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SELLERMATERIAL_LIST_FIELDS = [
  'customerCode',
  'customerShortName',
  'clientCode',
  'clientShortName',
  'materialType',
  'materialGroup',
  'internalMaterialCode',
  'materialCode',
  'materialDescription',
  'sellerMaterialCode',
  'sellerMaterialDescription',
  'sellerMaterialSpecification',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SELLERMATERIAL_PLACEHOLDER = {
  tenantCode: 'optional',
  customerCode: 'optional',
  customerShortName: 'optional',
  clientCode: 'optional',
  clientShortName: 'optional',
  materialType: 'select',
  materialGroup: 'select',
  internalMaterialCode: 'required',
  materialCode: 'select',
  materialDescription: 'optional',
  sellerMaterialCode: 'required',
  sellerMaterialDescription: 'optional',
  sellerMaterialSpecification: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SellerMaterialField = keyof typeof SELLERMATERIAL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SELLERMATERIAL_QUERY_STRING_FIELDS = [
  'customerCode',
  'customerShortName',
  'clientCode',
  'clientShortName',
  'materialType',
  'materialGroup',
  'internalMaterialCode',
  'materialCode',
  'materialDescription',
  'sellerMaterialCode',
  'sellerMaterialDescription',
  'sellerMaterialSpecification',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SellerMaterialQuery)[]

export type SellerMaterialQueryField = (typeof SELLERMATERIAL_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SELLERMATERIAL_QUERY_FIELDS: readonly SellerMaterialQueryField[] = [...SELLERMATERIAL_QUERY_STRING_FIELDS]

/**
 * Takt销售商物料实体字段 i18n：index / seller-material-form 统一入口
 */
export function useSellerMaterialI18n() {
  const ef = useEntityFieldI18n(SELLERMATERIAL_ENTITY_SLUG)

  function ph(field: SellerMaterialField): string {
    return ef.placeholder(field, SELLERMATERIAL_PLACEHOLDER[field])
  }

  function queryPh(field: SellerMaterialQueryField, kind: EntityFieldPlaceholderKind): string {
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
