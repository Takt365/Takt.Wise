// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/composables
// 文件名称：useEcAttachmentI18n.ts
// 功能描述：设变附件字段清单 + useEcAttachmentI18n（视图只传小驼峰；文案由 entity.ecattachment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcAttachmentI18nSeedData 一致的实体 slug */
export const ECATTACHMENT_ENTITY_SLUG = 'ecattachment'

/** entity.ecattachment._self */
export const ECATTACHMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(ECATTACHMENT_ENTITY_SLUG)

/** 列表/表单业务列（不含主键） */
export const ECATTACHMENT_LIST_FIELDS = [
  'ecCode',
  'lineNumber',
  'attachmentType',
  'docCode',
  'fileName',
  'accessUrl',
  'isObsolete',
] as const

/** 表单控件默认占位类型 */
export const ECATTACHMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'required',
  ecCode: 'required',
  lineNumber: 'required',
  attachmentType: 'select',
  docCode: 'required',
  fileName: 'required',
  accessUrl: 'required',
  isObsolete: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

export type EcAttachmentField = keyof typeof ECATTACHMENT_PLACEHOLDER

/**
 * 设变附件字段 i18n：attachment-form / attachment-panel / ec-form 统一入口
 */
export function useEcAttachmentI18n() {
  const ef = useEntityFieldI18n(ECATTACHMENT_ENTITY_SLUG)

  function ph(field: EcAttachmentField): string {
    return ef.placeholder(field, ECATTACHMENT_PLACEHOLDER[field])
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    self: ef.self,
    ph,
  }
}
