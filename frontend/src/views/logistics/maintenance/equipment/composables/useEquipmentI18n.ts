// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/maintenance/equipment/composables
// 文件名称：useEquipmentI18n.ts
// 功能描述：Takt工厂设备实体字段清单 + useEquipmentI18n（字段名映射一次，文案由 entity.equipment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EquipmentQuery } from '@/types/logistics/maintenance/equipment.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEquipmentI18nSeedData 一致的实体 slug */
export const EQUIPMENT_ENTITY_SLUG = 'equipment'

/** entity.equipment._self 静态属性（导入组件 entity-i18n-key 等） */
export const EQUIPMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(EQUIPMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EQUIPMENT_LIST_FIELDS = [
  'EquipCode',
  'equipmentName',
  'equipmentType',
  'equipmentModel',
  'EquipSpecification',
  'EquipBrand',
  'manufacturer',
  'dealerBy',
  'serialNumber',
  'workshopBy',
  'productionLineBy',
  'workstationBy',
  'deptBy',
  'equipmentLocation',
  'responsibleUserBy',
  'operatorBy',
  'purchaseDate',
  'installationDate',
  'startDate',
  'warrantyStartDate',
  'warrantyEndDate',
  'equipmentOriginalValue',
  'technicalParameters',
  'equipmentImages',
  'equipmentDocuments',
  'isCritical',
  'warrantyStatus',
  'equipmentStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EQUIPMENT_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EquipmentField = keyof typeof EQUIPMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EQUIPMENT_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof EquipmentQuery)[]

export type EquipmentQueryField = (typeof EQUIPMENT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const EQUIPMENT_QUERY_FIELDS: readonly EquipmentQueryField[] = [...EQUIPMENT_QUERY_STRING_FIELDS]

/**
 * Takt工厂设备实体字段 i18n：index / equipment-form 统一入口
 */
export function useEquipmentI18n() {
  const ef = useEntityFieldI18n(EQUIPMENT_ENTITY_SLUG)

  function ph(field: EquipmentField): string {
    return ef.placeholder(field, EQUIPMENT_PLACEHOLDER[field])
  }

  function queryPh(field: EquipmentQueryField, kind: EntityFieldPlaceholderKind): string {
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
