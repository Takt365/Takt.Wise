// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/exec-scan/composables
// 文件名称：useExecScanI18n.ts
// 功能描述：SOP 物料扫码记录实体字段清单 + useSopExecScanI18n（字段名映射一次，文案由 entity.sopexecscan.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopExecScanQuery } from '@/types/logistics/manufacturing/sop/execScan.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopExecScanI18nSeedData 一致的实体 slug */
export const SOPEXECSCAN_ENTITY_SLUG = 'sopexecscan'

/** entity.sopexecscan._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPEXECSCAN_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPEXECSCAN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPEXECSCAN_LIST_FIELDS = [
  'execId',
  'execStepId',
  'stepId',
  'scannedBarcode',
  'expectedMaterialCode',
  'scanResult',
  'matchMessage',
  'scannedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPEXECSCAN_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  execId: 'select',
  execStepId: 'optional',
  stepId: 'select',
  scannedBarcode: 'required',
  expectedMaterialCode: 'optional',
  scanResult: 'select',
  matchMessage: 'optional',
  scannedAt: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopExecScanField = keyof typeof SOPEXECSCAN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPEXECSCAN_QUERY_STRING_FIELDS = [
  'cultureCode',
  'execId',
  'execStepId',
  'stepId',
  'scannedBarcode',
  'expectedMaterialCode',
  'matchMessage',
  'scannedAtStart',
  'scannedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopExecScanQuery)[]

export type SopExecScanQueryField =
  | (typeof SOPEXECSCAN_QUERY_STRING_FIELDS)[number]
  | 'scanResult'

/** 高级查询抽屉全部字段（含数值） */
export const SOPEXECSCAN_QUERY_FIELDS: readonly SopExecScanQueryField[] = [
  ...SOPEXECSCAN_QUERY_STRING_FIELDS,
  'scanResult',
]

/**
 * SOP 物料扫码记录实体字段 i18n：index / exec-scan-form 统一入口
 */
export function useSopExecScanI18n() {
  const ef = useEntityFieldI18n(SOPEXECSCAN_ENTITY_SLUG)

  function ph(field: SopExecScanField): string {
    return ef.placeholder(field, SOPEXECSCAN_PLACEHOLDER[field])
  }

  function queryPh(field: SopExecScanQueryField, kind: EntityFieldPlaceholderKind): string {
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
