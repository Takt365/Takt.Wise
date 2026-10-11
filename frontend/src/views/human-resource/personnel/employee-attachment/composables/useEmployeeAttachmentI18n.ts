// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-attachment/composables
// 文件名称：useEmployeeAttachmentI18n.ts
// 功能描述：EmployeeAttachment字段清单 + useEmployeeAttachmentI18n（字段名映射一次，文案由 entity.employeeattachment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeAttachmentQuery } from '@/types/human-resource/personnel/employeeAttachment.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeAttachmentI18nSeedData 一致的实体 slug */
export const EMPLOYEEATTACHMENT_ENTITY_SLUG = 'employeeattachment'

/** entity.employeeattachment._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEATTACHMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEATTACHMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEATTACHMENT_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'fileName',
  'accessUrl',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEATTACHMENT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'fileName',
  'accessUrl',
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const EMPLOYEEATTACHMENT_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEATTACHMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  fileName: 'required',
  accessUrl: 'required',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeAttachmentField = keyof typeof EMPLOYEEATTACHMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEATTACHMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'employeeCode',
  'employeeName',
  'fileName',
  'accessUrl',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeAttachmentQuery)[]

export type EmployeeAttachmentQueryField = (typeof EMPLOYEEATTACHMENT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEATTACHMENT_QUERY_FIELDS: readonly EmployeeAttachmentQueryField[] = [...EMPLOYEEATTACHMENT_QUERY_STRING_FIELDS]

/**
 * EmployeeAttachment字段 i18n：index / employee-attachment-form 统一入口
 */
export function useEmployeeAttachmentI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEATTACHMENT_ENTITY_SLUG)

  function ph(field: EmployeeAttachmentField): string {
    return ef.placeholder(field, EMPLOYEEATTACHMENT_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeAttachmentQueryField, kind: EntityFieldPlaceholderKind): string {
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
