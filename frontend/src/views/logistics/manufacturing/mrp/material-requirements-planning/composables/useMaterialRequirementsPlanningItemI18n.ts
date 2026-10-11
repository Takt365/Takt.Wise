// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/composables
// 文件名称：useMaterialRequirementsPlanningItemI18n.ts
// 功能描述：MaterialRequirementsPlanningItem字段清单 + useMaterialRequirementsPlanningItemI18n（字段名映射一次，文案由 entity.materialrequirementsplanningitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialRequirementsPlanningItemQuery } from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanningItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialRequirementsPlanningItemI18nSeedData 一致的实体 slug */
export const MATERIALREQUIREMENTSPLANNINGITEM_ENTITY_SLUG = 'materialrequirementsplanningitem'

/** entity.materialrequirementsplanningitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALREQUIREMENTSPLANNINGITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALREQUIREMENTSPLANNINGITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALREQUIREMENTSPLANNINGITEM_LIST_FIELDS = [
  'materialRequirementsPlanningCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'modelCode',
  'modelName',
  'parentMaterialCode',
  'bomLevel',
  'requirementDate',
  'planUnit',
  'grossRequirement',
  'scheduledReceipts',
  'onHandQuantity',
  'projectedOnHand',
  'netRequirement',
  'procurementType',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const MATERIALREQUIREMENTSPLANNINGITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'materialRequirementsPlanningCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'modelCode',
  'modelName',
  'parentMaterialCode',
  'bomLevel',
  'requirementDate',
  'planUnit',
  'grossRequirement',
  'scheduledReceipts',
  'onHandQuantity',
  'projectedOnHand',
  'netRequirement',
  'procurementType',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS = [
  'bomLevel',
  'grossRequirement',
  'scheduledReceipts',
  'onHandQuantity',
  'projectedOnHand',
  'netRequirement',
  'procurementType',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALREQUIREMENTSPLANNINGITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialRequirementsPlanningItemField = keyof typeof MATERIALREQUIREMENTSPLANNINGITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALREQUIREMENTSPLANNINGITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MaterialRequirementsPlanningItemQuery)[]

export type MaterialRequirementsPlanningItemQueryField = (typeof MATERIALREQUIREMENTSPLANNINGITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALREQUIREMENTSPLANNINGITEM_QUERY_FIELDS: readonly MaterialRequirementsPlanningItemQueryField[] = [...MATERIALREQUIREMENTSPLANNINGITEM_QUERY_STRING_FIELDS]

/**
 * MaterialRequirementsPlanningItem字段 i18n：index / material-requirements-planning-item-form 统一入口
 */
export function useMaterialRequirementsPlanningItemI18n() {
  const ef = useEntityFieldI18n(MATERIALREQUIREMENTSPLANNINGITEM_ENTITY_SLUG)

  function ph(field: MaterialRequirementsPlanningItemField): string {
    return ef.placeholder(field, MATERIALREQUIREMENTSPLANNINGITEM_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialRequirementsPlanningItemQueryField, kind: EntityFieldPlaceholderKind): string {
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
