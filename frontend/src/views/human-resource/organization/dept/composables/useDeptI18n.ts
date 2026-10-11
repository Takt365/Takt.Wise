// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/organization/dept/composables
// 文件名称：useDeptI18n.ts
// 功能描述：部门实体 代表组织架构中的部门字段清单 + useDeptI18n（字段名映射一次，文案由 entity.dept.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DeptQuery } from '@/types/human-resource/organization/dept.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDeptI18nSeedData 一致的实体 slug */
export const DEPT_ENTITY_SLUG = 'dept'

/** entity.dept._self 静态属性（导入组件 entity-i18n-key 等） */
export const DEPT_SELF_I18N_KEY = buildEntitySelfI18nKey(DEPT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DEPT_LIST_FIELDS = [
  'deptCode',
  'deptShortName',
  'deptName1',
  'deptName2',
  'parentId',
  'level',
  'deptPath',
  'isLeaf',
  'isoCode',
  'costCenterCode',
  'costCategory',
  'headUserId',
  'headUserName',
  'phone',
  'email',
  'location',
  'isBuiltIn',
  'deptDescription',
  'deptStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DEPT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  deptCode: 'required',
  deptShortName: 'required',
  deptName1: 'required',
  deptName2: 'required',
  parentId: 'select',
  isoCode: 'required',
  costCenterCode: 'select',
  costCategory: 'select',
  headUserId: 'select',
  headUserName: 'optional',
  phone: 'required',
  email: 'required',
  location: 'required',
  isBuiltIn: 'select',
  deptDescription: 'optional',
  deptStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DeptField = keyof typeof DEPT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DEPT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'deptCode',
  'deptShortName',
  'deptName1',
  'deptName2',
  'parentId',
  'deptPath',
  'isoCode',
  'costCenterCode',
  'headUserId',
  'headUserName',
  'phone',
  'email',
  'location',
  'deptDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof DeptQuery)[]

export type DeptQueryField =
  | (typeof DEPT_QUERY_STRING_FIELDS)[number]
  | 'level' | 'isLeaf' | 'costCategory' | 'isBuiltIn' | 'deptStatus'

/** 高级查询抽屉全部字段（含数值） */
export const DEPT_QUERY_FIELDS: readonly DeptQueryField[] = [
  ...DEPT_QUERY_STRING_FIELDS,
  'level',
  'isLeaf',
  'costCategory',
  'isBuiltIn',
  'deptStatus',
]

/**
 * 部门实体 代表组织架构中的部门字段 i18n：index / dept-form 统一入口
 */
export function useDeptI18n() {
  const ef = useEntityFieldI18n(DEPT_ENTITY_SLUG)

  function ph(field: DeptField): string {
    return ef.placeholder(field, DEPT_PLACEHOLDER[field])
  }

  function queryPh(field: DeptQueryField, kind: EntityFieldPlaceholderKind): string {
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
