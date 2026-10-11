// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/materials/material-document/composables
// 文件名称：useMaterialDocumentI18n.ts
// 功能描述：Takt物料凭证主表实体字段清单 + useMaterialDocumentI18n（字段名映射一次，文案由 entity.materialdocument.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaterialDocumentQuery } from '@/types/logistics/materials/materialDocument.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaterialDocumentI18nSeedData 一致的实体 slug */
export const MATERIALDOCUMENT_ENTITY_SLUG = 'materialdocument'

/** entity.materialdocument._self 静态属性（导入组件 entity-i18n-key 等） */
export const MATERIALDOCUMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(MATERIALDOCUMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MATERIALDOCUMENT_LIST_FIELDS = [
  'materialDocumentCode',
  'materialDocumentYear',
  'transactionEventType',
  'documentType',
  'revaluationType',
  'documentDate',
  'postingDate',
  'referenceCode',
  'headerText',
  'billOfLadingCode',
  'deliveryCode',
  'transactionCode',
  'postedBy',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MATERIALDOCUMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  materialDocumentCode: 'required',
  materialDocumentYear: 'required',
  transactionEventType: 'optional',
  documentType: 'optional',
  revaluationType: 'optional',
  documentDate: 'select',
  postingDate: 'select',
  referenceCode: 'optional',
  headerText: 'optional',
  billOfLadingCode: 'optional',
  deliveryCode: 'optional',
  transactionCode: 'optional',
  postedBy: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaterialDocumentField = keyof typeof MATERIALDOCUMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MATERIALDOCUMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'materialDocumentCode',
  'materialDocumentYear',
  'transactionEventType',
  'documentType',
  'revaluationType',
  'documentDateStart',
  'documentDateEnd',
  'postingDateStart',
  'postingDateEnd',
  'referenceCode',
  'headerText',
  'billOfLadingCode',
  'deliveryCode',
  'transactionCode',
  'postedBy',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaterialDocumentQuery)[]

export type MaterialDocumentQueryField = (typeof MATERIALDOCUMENT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MATERIALDOCUMENT_QUERY_FIELDS: readonly MaterialDocumentQueryField[] = [...MATERIALDOCUMENT_QUERY_STRING_FIELDS]

/**
 * Takt物料凭证主表实体字段 i18n：index / MaterialDocumentForm 统一入口
 */
export function useMaterialDocumentI18n() {
  const ef = useEntityFieldI18n(MATERIALDOCUMENT_ENTITY_SLUG)

  function ph(field: MaterialDocumentField): string {
    return ef.placeholder(field, MATERIALDOCUMENT_PLACEHOLDER[field])
  }

  function queryPh(field: MaterialDocumentQueryField, kind: EntityFieldPlaceholderKind): string {
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
