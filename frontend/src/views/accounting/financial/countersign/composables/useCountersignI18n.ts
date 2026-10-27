// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/countersign/composables
// 文件名称：useCountersignI18n.ts
// 功能描述：会签单实体字段清单 + useCountersignI18n（字段名映射一次，文案由 entity.countersign.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CountersignQuery } from '@/types/accounting/financial/countersign.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCountersignI18nSeedData 一致的实体 slug */
export const COUNTERSIGN_ENTITY_SLUG = 'countersign'

/** entity.countersign._self 静态属性（导入组件 entity-i18n-key 等） */
export const COUNTERSIGN_SELF_I18N_KEY = buildEntitySelfI18nKey(COUNTERSIGN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const COUNTERSIGN_LIST_FIELDS = [
  'countersignCode',
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'businessType',
  'businessKey',
  'stepNo',
  'countersignDepts',
  'financeDept',
  'budgetReviewComment',
  'executiveOffice',
  'applicantBy',
  'applicantName',
  'applicationDeptId',
  'applicationDeptName',
  'costBearerDeptId',
  'costBearerDeptName',
  'isBudget',
  'budgetItemId',
  'budgetItem',
  'budgetAmount',
  'applicationAmount',
  'countersignTitle',
  'applicationReason',
  'budgetUsageDescription',
  'targetAndExpectedBenefit',
  'fileName',
  'accessUrl',
  'countersignStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const COUNTERSIGN_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchaseInquiryId: 'select',
  purchaseInquiryCode: 'optional',
  businessType: 'select',
  businessKey: 'optional',
  stepNo: 'select',
  countersignDepts: 'select',
  financeDept: 'optional',
  budgetReviewComment: 'optional',
  executiveOffice: 'optional',
  applicantBy: 'select',
  applicantName: 'optional',
  applicationDeptId: 'select',
  applicationDeptName: 'optional',
  costBearerDeptId: 'select',
  costBearerDeptName: 'optional',
  isBudget: 'select',
  budgetItemId: 'select',
  budgetItem: 'optional',
  budgetAmount: 'select',
  applicationAmount: 'select',
  countersignTitle: 'optional',
  applicationReason: 'optional',
  budgetUsageDescription: 'optional',
  targetAndExpectedBenefit: 'optional',
  fileName: 'optional',
  accessUrl: 'optional',
  countersignStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CountersignField = keyof typeof COUNTERSIGN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const COUNTERSIGN_QUERY_STRING_FIELDS = [
  'countersignCode',
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'businessType',
  'businessKey',
  'financeDept',
  'budgetReviewComment',
  'executiveOffice',
  'applicantBy',
  'applicantName',
  'applicationDeptId',
  'applicationDeptName',
  'costBearerDeptId',
  'costBearerDeptName',
  'budgetItemId',
  'budgetItem',
  'countersignTitle',
  'applicationReason',
  'budgetUsageDescription',
  'targetAndExpectedBenefit',
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
] as const satisfies readonly (keyof CountersignQuery)[]

export type CountersignQueryField =
  | (typeof COUNTERSIGN_QUERY_STRING_FIELDS)[number]
  | 'countersignDepts' | 'stepNo' | 'isBudget' | 'budgetAmount' | 'applicationAmount' | 'countersignStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const COUNTERSIGN_QUERY_FIELDS: readonly CountersignQueryField[] = [
  ...COUNTERSIGN_QUERY_STRING_FIELDS,
  'countersignDepts',
  'stepNo',
  'isBudget',
  'budgetAmount',
  'applicationAmount',
  'countersignStatus',
  'approvalStatus',
]

/**
 * 会签单实体字段 i18n：index / countersign-form 统一入口
 */
export function useCountersignI18n() {
  const ef = useEntityFieldI18n(COUNTERSIGN_ENTITY_SLUG)

  function ph(field: CountersignField): string {
    return ef.placeholder(field, COUNTERSIGN_PLACEHOLDER[field])
  }

  function queryPh(field: CountersignQueryField, kind: EntityFieldPlaceholderKind): string {
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
