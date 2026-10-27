// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/document-center/document/composables
// 文件名称：useDocumentI18n.ts
// 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制字段清单 + useDocumentI18n（字段名映射一次，文案由 entity.document.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DocumentQuery } from '@/types/routine/document-center/document.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDocumentI18nSeedData 一致的实体 slug */
export const DOCUMENT_ENTITY_SLUG = 'document'

/** entity.document._self 静态属性（导入组件 entity-i18n-key 等） */
export const DOCUMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(DOCUMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DOCUMENT_LIST_FIELDS = [
  'documentCode',
  'documentTitle',
  'documentCategory',
  'confidentialLevel',
  'version',
  'documentContent',
  'documentSummary',
  'documentTags',
  'fileName',
  'accessUrl',
  'documentEffectiveTime',
  'documentExpireTime',
  'documentPublishTime',
  'publisherId',
  'publisherName',
  'deptId',
  'deptName',
  'documentIsTop',
  'documentViewCount',
  'targetScope',
  'targetDepartments',
  'targetUsers',
  'documentStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DOCUMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  numberingRuleCode: 'select',
  documentCode: 'optional',
  numberingRuleCode: 'optional',
  documentTitle: 'required',
  documentCategory: 'select',
  confidentialLevel: 'select',
  version: 'select',
  documentContent: 'optional',
  documentSummary: 'optional',
  documentTags: 'optional',
  fileName: 'optional',
  accessUrl: 'optional',
  documentEffectiveTime: 'optional',
  documentExpireTime: 'optional',
  documentPublishTime: 'optional',
  publisherId: 'select',
  publisherName: 'optional',
  deptId: 'optional',
  deptName: 'optional',
  documentIsTop: 'select',
  documentViewCount: 'select',
  targetScope: 'select',
  targetDepartments: 'optional',
  targetUsers: 'optional',
  documentStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DocumentField = keyof typeof DOCUMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DOCUMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'documentCode',
  'documentTitle',
  'documentContent',
  'documentSummary',
  'documentTags',
  'fileName',
  'accessUrl',
  'documentEffectiveTimeStart',
  'documentEffectiveTimeEnd',
  'documentExpireTimeStart',
  'documentExpireTimeEnd',
  'documentPublishTimeStart',
  'documentPublishTimeEnd',
  'publisherId',
  'publisherName',
  'deptId',
  'deptName',
  'targetDepartments',
  'targetUsers',
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
] as const satisfies readonly (keyof DocumentQuery)[]

export type DocumentQueryField =
  | (typeof DOCUMENT_QUERY_STRING_FIELDS)[number]
  | 'documentCategory' | 'confidentialLevel' | 'version' | 'documentIsTop' | 'documentViewCount' | 'targetScope' | 'documentStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const DOCUMENT_QUERY_FIELDS: readonly DocumentQueryField[] = [
  ...DOCUMENT_QUERY_STRING_FIELDS,
  'documentCategory',
  'confidentialLevel',
  'version',
  'documentIsTop',
  'documentViewCount',
  'targetScope',
  'documentStatus',
  'approvalStatus',
]

/**
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制字段 i18n：index / document-form 统一入口
 */
export function useDocumentI18n() {
  const ef = useEntityFieldI18n(DOCUMENT_ENTITY_SLUG)

  function ph(field: DocumentField): string {
    return ef.placeholder(field, DOCUMENT_PLACEHOLDER[field])
  }

  function queryPh(field: DocumentQueryField, kind: EntityFieldPlaceholderKind): string {
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
