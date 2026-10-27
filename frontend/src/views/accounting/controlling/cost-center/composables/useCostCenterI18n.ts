// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/controlling/cost-center/composables
// 文件名称：useCostCenterI18n.ts
// 功能描述：成本中心实体字段清单 + useCostCenterI18n（字段名映射一次，文案由 entity.costcenter.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CostCenterQuery } from '@/types/accounting/controlling/costCenter.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCostCenterI18nSeedData 一致的实体 slug */
export const COSTCENTER_ENTITY_SLUG = 'costcenter'

/** entity.costcenter._self 静态属性（导入组件 entity-i18n-key 等） */
export const COSTCENTER_SELF_I18N_KEY = buildEntitySelfI18nKey(COSTCENTER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const COSTCENTER_LIST_FIELDS = [
  'costCenterCode',
  'costCenterName',
  'parentId',
  'costCenterType',
  'managerId',
  'managerName',
  'deptId',
  'deptName',
  'costCenterLevel',
  'validFrom',
  'validTo',
  'costCenterStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const COSTCENTER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  costCenterCode: 'required',
  costCenterName: 'required',
  parentId: 'required',
  costCenterType: 'select',
  managerId: 'optional',
  managerName: 'optional',
  deptId: 'optional',
  deptName: 'optional',
  costCenterLevel: 'select',
  validFrom: 'select',
  validTo: 'select',
  costCenterStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CostCenterField = keyof typeof COSTCENTER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const COSTCENTER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'costCenterCode',
  'costCenterName',
  'parentId',
  'costCenterType',
  'managerId',
  'managerName',
  'deptId',
  'deptName',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CostCenterQuery)[]

export type CostCenterQueryField =
  | (typeof COSTCENTER_QUERY_STRING_FIELDS)[number]
  | 'costCenterLevel' | 'costCenterStatus'

/** 高级查询抽屉全部字段（含数值） */
export const COSTCENTER_QUERY_FIELDS: readonly CostCenterQueryField[] = [
  ...COSTCENTER_QUERY_STRING_FIELDS,
  'costCenterLevel',
  'costCenterStatus',
]

/**
 * 成本中心实体字段 i18n：index / cost-center-form 统一入口
 */
export function useCostCenterI18n() {
  const ef = useEntityFieldI18n(COSTCENTER_ENTITY_SLUG)

  function ph(field: CostCenterField): string {
    return ef.placeholder(field, COSTCENTER_PLACEHOLDER[field])
  }

  function queryPh(field: CostCenterQueryField, kind: EntityFieldPlaceholderKind): string {
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
