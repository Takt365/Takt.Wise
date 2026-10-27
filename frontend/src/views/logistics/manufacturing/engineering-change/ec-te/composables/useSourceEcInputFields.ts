// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/composables
// 文件名称：useSourceEcInputFields.ts
// 功能描述：来源设变录入列表字段（与 TaktSourceEc / TaktEcTeSourceEcInputItemDto 对齐）+ 复用 entity.sourceec.* i18n
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcTeSourceEcInputItem } from '@/types/logistics/manufacturing/engineering-change/ecTeSourceInput.types'
import {
  useSourceEcI18n,
  type SourceEcField,
} from '@/views/logistics/manufacturing/engineering-change/source-ec/composables/useSourceEcI18n'

/** 未导入列表列：TaktSourceEc 主表字段（与后端 SourceEcCode 等一致，禁止 No 后缀） */
export const SOURCE_EC_INPUT_SOURCE_FIELDS = [
  'sourceEcCode',
  'sourceModel',
  'sourceTitle',
  'sourceStatus',
  'sourceIssueDate',
] as const satisfies readonly SourceEcField[]

/** 录入列表扩展列（列表 DTO 聚合字段，非 TaktSourceEc 实体列） */
export const SOURCE_EC_INPUT_EXTRA_FIELDS = ['detailCount'] as const

export type SourceEcInputSourceField = (typeof SOURCE_EC_INPUT_SOURCE_FIELDS)[number]
export type SourceEcInputExtraField = (typeof SOURCE_EC_INPUT_EXTRA_FIELDS)[number]
export type SourceEcInputListField = SourceEcInputSourceField | SourceEcInputExtraField

/**
 * 来源设变录入：复用设变来源 entity.sourceec.* 字段 i18n
 */
export function useSourceEcInputI18n() {
  return useSourceEcI18n()
}

/**
 * 读取来源设变录入列表行字段
 * @param record 行数据
 * @param field 字段名
 * @returns 字段值
 */
export function getSourceEcInputField(
  record: EcTeSourceEcInputItem,
  field: SourceEcInputListField,
): unknown {
  return record?.[field as keyof EcTeSourceEcInputItem]
}
