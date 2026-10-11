// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/composables
// 文件名称：useMaterialRequirementsPlanningI18n.ts
// 功能描述：物料需求计划 MRP 头表字段清单 + useMaterialRequirementsPlanningI18n（字段名映射一次，文案由 entity.materialrequirementsplanning.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialRequirementsPlanningQuery } from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanning.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialRequirementsPlanningI18nSeedData 一致的实体 slug */
export const MATERIALREQUIREMENTSPLANNING_ENTITY_SLUG = 'materialrequirementsplanning'

/** entity.materialrequirementsplanning._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALREQUIREMENTSPLANNING_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALREQUIREMENTSPLANNING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALREQUIREMENTSPLANNING_LIST_FIELDS = [
  'materialRequirementsPlanningCode',
  'masterProductionScheduleId',
  'mpsCode',
  'masterDemandScheduleId',
  'mdsCode',
  'planDate',
  'planPeriodStart',
  'planPeriodEnd',
  'plannerGroupCode',
  'planBy',
  'runStatus',
  'productionPlanId',
  'productionPlanCode',
  'purchasePlanId',
  'purchasePlanCode',
  'planDescription',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALREQUIREMENTSPLANNING_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialRequirementsPlanningField = keyof typeof MATERIALREQUIREMENTSPLANNING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALREQUIREMENTSPLANNING_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MaterialRequirementsPlanningQuery)[]

export type MaterialRequirementsPlanningQueryField = (typeof MATERIALREQUIREMENTSPLANNING_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALREQUIREMENTSPLANNING_QUERY_FIELDS: readonly MaterialRequirementsPlanningQueryField[] = [...MATERIALREQUIREMENTSPLANNING_QUERY_STRING_FIELDS]

/**
 * 物料需求计划 MRP 头表字段 i18n：index / material-requirements-planning-form 统一入口
 */
export function useMaterialRequirementsPlanningI18n() {
  const ef = useEntityFieldI18n(MATERIALREQUIREMENTSPLANNING_ENTITY_SLUG)

  function ph(field: MaterialRequirementsPlanningField): string {
    return ef.placeholder(field, MATERIALREQUIREMENTSPLANNING_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialRequirementsPlanningQueryField, kind: EntityFieldPlaceholderKind): string {
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
