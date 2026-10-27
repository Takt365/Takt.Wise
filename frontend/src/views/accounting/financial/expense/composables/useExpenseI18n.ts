// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/expense/composables
// 文件名称：useExpenseI18n.ts
// 功能描述：费用单实体字段清单 + useExpenseI18n（字段名映射一次，文案由 entity.expense.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ExpenseQuery } from '@/types/accounting/financial/expense.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktExpenseI18nSeedData 一致的实体 slug */
export const EXPENSE_ENTITY_SLUG = 'expense'

/** entity.expense._self 静态属性（导入组件 entity-i18n-key 等） */
export const EXPENSE_SELF_I18N_KEY = buildEntitySelfI18nKey(EXPENSE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EXPENSE_LIST_FIELDS = [
  'expenseCode',
  'expenseTitle',
  'expenseType',
  'supplierCode',
  'supplierName1',
  'applicantBy',
  'applicantName',
  'applicationDeptId',
  'applicationDeptName',
  'costBearerDeptId',
  'costBearerDeptName',
  'costCenter',
  'countersignId',
  'purchaseOrderCode',
  'purchaseRequestCode',
  'expenseAmount',
  'taxRate',
  'taxAmount',
  'expenseDate',
  'applicationReason',
  'fileName',
  'accessUrl',
  'expenseStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EXPENSE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  expenseCode: 'optional',
  expenseTitle: 'optional',
  expenseType: 'select',
  supplierCode: 'select',
  supplierName1: 'optional',
  applicantBy: 'select',
  applicantName: 'optional',
  applicationDeptId: 'select',
  applicationDeptName: 'optional',
  costBearerDeptId: 'select',
  costBearerDeptName: 'optional',
  costCenter: 'select',
  countersignId: 'select',
  purchaseOrderCode: 'optional',
  purchaseRequestCode: 'optional',
  expenseAmount: 'select',
  taxRate: 'select',
  taxAmount: 'select',
  expenseDate: 'select',
  applicationReason: 'optional',
  fileName: 'optional',
  accessUrl: 'optional',
  expenseStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ExpenseField = keyof typeof EXPENSE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EXPENSE_QUERY_STRING_FIELDS = [
  'expenseCode',
  'expenseTitle',
  'supplierCode',
  'supplierName1',
  'applicantBy',
  'applicantName',
  'applicationDeptId',
  'applicationDeptName',
  'costBearerDeptId',
  'costBearerDeptName',
  'costCenter',
  'countersignId',
  'purchaseOrderCode',
  'purchaseRequestCode',
  'applicationReason',
  'fileName',
  'accessUrl',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ExpenseQuery)[]

export type ExpenseQueryField =
  | (typeof EXPENSE_QUERY_STRING_FIELDS)[number]
  | 'expenseType' | 'expenseAmount' | 'taxRate' | 'taxAmount' | 'expenseDateStart' | 'expenseDateEnd' | 'expenseStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EXPENSE_QUERY_FIELDS: readonly ExpenseQueryField[] = [
  ...EXPENSE_QUERY_STRING_FIELDS,
  'expenseType',
  'expenseAmount',
  'taxRate',
  'taxAmount',
  'expenseDateStart',
  'expenseDateEnd',
  'expenseStatus',
  'approvalStatus',
]

/**
 * 费用单实体字段 i18n：index / expense-form 统一入口
 */
export function useExpenseI18n() {
  const ef = useEntityFieldI18n(EXPENSE_ENTITY_SLUG)

  function ph(field: ExpenseField): string {
    return ef.placeholder(field, EXPENSE_PLACEHOLDER[field])
  }

  function queryPh(field: ExpenseQueryField, kind: EntityFieldPlaceholderKind): string {
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
