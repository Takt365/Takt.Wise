// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/warehouse/composables
// 文件名称：useWarehouseI18n.ts
// 功能描述：Takt仓库主数据实体字段清单 + useWarehouseI18n（字段名映射一次，文案由 entity.warehouse.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { WarehouseQuery } from '@/types/logistics/materials/warehouse.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktWarehouseI18nSeedData 一致的实体 slug */
export const WAREHOUSE_ENTITY_SLUG = 'warehouse'

/** entity.warehouse._self 静态属性（导入组件 entity-i18n-key 等） */
export const WAREHOUSE_SELF_I18N_KEY = buildEntitySelfI18nKey(WAREHOUSE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const WAREHOUSE_LIST_FIELDS = [
  'warehouseCode',
  'warehouseName',
  'warehouseShortName',
  'address',
  'contactPerson',
  'contactPhone',
  'managerUserCode',
  'isVirtual',
  'warehouseType',
  'warehouseStatus',
  'isBuiltIn',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const WAREHOUSE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type WarehouseField = keyof typeof WAREHOUSE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const WAREHOUSE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof WarehouseQuery)[]

export type WarehouseQueryField = (typeof WAREHOUSE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const WAREHOUSE_QUERY_FIELDS: readonly WarehouseQueryField[] = [...WAREHOUSE_QUERY_STRING_FIELDS]

/**
 * Takt仓库主数据实体字段 i18n：index / warehouse-form 统一入口
 */
export function useWarehouseI18n() {
  const ef = useEntityFieldI18n(WAREHOUSE_ENTITY_SLUG)

  function ph(field: WarehouseField): string {
    return ef.placeholder(field, WAREHOUSE_PLACEHOLDER[field])
  }

  function queryPh(field: WarehouseQueryField, kind: EntityFieldPlaceholderKind): string {
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
