// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/document-center/document/composables
// 文件名称：useDocumentVersionI18n.ts
// 功能描述：DocumentVersion字段清单 + useDocumentVersionI18n（字段名映射一次，文案由 entity.documentversion.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DocumentVersionQuery } from '@/types/routine/document-center/documentVersion.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDocumentVersionI18nSeedData 一致的实体 slug */
export const DOCUMENTVERSION_ENTITY_SLUG = 'documentversion'

/** entity.documentversion._self 静态属性（导入组件 entity-i18n-key 等） */
export const DOCUMENTVERSION_SELF_I18N_KEY = buildEntitySelfI18nKey(DOCUMENTVERSION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DOCUMENTVERSION_LIST_FIELDS = [
  'documentId',
  'lineNumber',
  'versionNo',
  'versionNote',
  'fileId',
  'revisedBy',
  'revisedByName',
  'revisedAt',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const DOCUMENTVERSION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'documentId',
  'lineNumber',
  'versionNo',
  'versionNote',
  'fileId',
  'revisedBy',
  'revisedByName',
  'revisedAt',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const DOCUMENTVERSION_SUMMARY_SUM_FIELDS = [
  'versionNo',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DOCUMENTVERSION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  versionNo: 'select',
  versionNote: 'optional',
  fileId: 'select',
  revisedBy: 'select',
  revisedByName: 'optional',
  revisedAt: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DocumentVersionField = keyof typeof DOCUMENTVERSION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DOCUMENTVERSION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'versionNote',
  'fileId',
  'revisedBy',
  'revisedByName',
  'revisedAtStart',
  'revisedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof DocumentVersionQuery)[]

export type DocumentVersionQueryField =
  | (typeof DOCUMENTVERSION_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'versionNo' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const DOCUMENTVERSION_QUERY_FIELDS: readonly DocumentVersionQueryField[] = [
  ...DOCUMENTVERSION_QUERY_STRING_FIELDS,
  'lineNumber',
  'versionNo',
  'isObsolete',
]

/**
 * DocumentVersion字段 i18n：index / document-version-form 统一入口
 */
export function useDocumentVersionI18n() {
  const ef = useEntityFieldI18n(DOCUMENTVERSION_ENTITY_SLUG)

  function ph(field: DocumentVersionField): string {
    return ef.placeholder(field, DOCUMENTVERSION_PLACEHOLDER[field])
  }

  function queryPh(field: DocumentVersionQueryField, kind: EntityFieldPlaceholderKind): string {
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
