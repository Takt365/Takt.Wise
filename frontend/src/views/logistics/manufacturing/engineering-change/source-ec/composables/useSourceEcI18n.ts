// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/composables
// 文件名称：useSourceEcI18n.ts
// 功能描述：设变来源明细列表字段清单 + useSourceEcI18n（字段名映射一次，文案由 entity.sourceec.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SourceEcQuery } from '@/types/logistics/manufacturing/engineering-change/sourceEc.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSourceEcI18nSeedData 一致的实体 slug */
export const SOURCEEC_ENTITY_SLUG = 'sourceec'

/** entity.sourceec._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOURCEEC_SELF_I18N_KEY = buildEntitySelfI18nKey(SOURCEEC_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOURCEEC_LIST_FIELDS = [
  'sourceEcCode',
  'sourceModel',
  'sourceTitle',
  'sourceStatus',
  'sourceIssueDate',
  'sourceTcjOwner',
  'sourceTcjDependency',
  'sourceEcMeeting',
  'sourcePpCode',
  'sourceTechnicalNoticeCode',
  'sourceImplementation',
  'sourceMainChangeReason',
  'sourceSecondaryChangeReason',
  'sourceSafetyRegulation',
  'sourceProgressStatus',
  'sourceSerialNumberControl',
  'sourceCustomerApproval',
  'sourceServiceManualRevision',
  'sourceUserManualRevision',
  'sourcePromotionManualRevision',
  'sourceStandardDocumentRevision',
  'sourceInformationRelease',
  'sourceCostChange',
  'sourceUnitCost',
  'sourceMoldModificationCost',
  'sourceRelatedDrawing',
  'sourceEcContent',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOURCEEC_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  sourceEcCode: 'required',
  sourceModel: 'required',
  sourceTitle: 'required',
  sourceStatus: 'required',
  sourceIssueDate: 'select',
  sourceTcjOwner: 'optional',
  sourceTcjDependency: 'optional',
  sourceEcMeeting: 'optional',
  sourcePpCode: 'optional',
  sourceTechnicalNoticeCode: 'optional',
  sourceImplementation: 'optional',
  sourceMainChangeReason: 'optional',
  sourceSecondaryChangeReason: 'optional',
  sourceSafetyRegulation: 'optional',
  sourceProgressStatus: 'optional',
  sourceSerialNumberControl: 'optional',
  sourceCustomerApproval: 'optional',
  sourceServiceManualRevision: 'optional',
  sourceUserManualRevision: 'optional',
  sourcePromotionManualRevision: 'optional',
  sourceStandardDocumentRevision: 'optional',
  sourceInformationRelease: 'optional',
  sourceCostChange: 'optional',
  sourceUnitCost: 'select',
  sourceMoldModificationCost: 'select',
  sourceRelatedDrawing: 'optional',
  sourceEcContent: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SourceEcField = keyof typeof SOURCEEC_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOURCEEC_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'sourceEcCode',
  'sourceModel',
  'sourceTitle',
  'sourceStatus',
  'sourceIssueDateStart',
  'sourceIssueDateEnd',
  'sourceTcjOwner',
  'sourceTcjDependency',
  'sourceEcMeeting',
  'sourcePpCode',
  'sourceTechnicalNoticeCode',
  'sourceImplementation',
  'sourceMainChangeReason',
  'sourceSecondaryChangeReason',
  'sourceSafetyRegulation',
  'sourceProgressStatus',
  'sourceSerialNumberControl',
  'sourceCustomerApproval',
  'sourceServiceManualRevision',
  'sourceUserManualRevision',
  'sourcePromotionManualRevision',
  'sourceStandardDocumentRevision',
  'sourceInformationRelease',
  'sourceCostChange',
  'sourceRelatedDrawing',
  'sourceEcContent',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SourceEcQuery)[]

export type SourceEcQueryField =
  | (typeof SOURCEEC_QUERY_STRING_FIELDS)[number]
  | 'sourceUnitCost' | 'sourceMoldModificationCost'

/** 高级查询抽屉全部字段（含数值） */
export const SOURCEEC_QUERY_FIELDS: readonly SourceEcQueryField[] = [
  ...SOURCEEC_QUERY_STRING_FIELDS,
  'sourceUnitCost',
  'sourceMoldModificationCost',
]

/**
 * 设变来源明细列表字段 i18n：index / source-ec-form 统一入口
 */
export function useSourceEcI18n() {
  const ef = useEntityFieldI18n(SOURCEEC_ENTITY_SLUG)

  function ph(field: SourceEcField): string {
    return ef.placeholder(field, SOURCEEC_PLACEHOLDER[field])
  }

  function queryPh(field: SourceEcQueryField, kind: EntityFieldPlaceholderKind): string {
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
