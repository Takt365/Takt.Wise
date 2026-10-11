// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputDetailEditableDict.ts
// 功能描述：内嵌子表停线/未达成原因多选字典：行内清洗、TaktSelect 绑定、变更写回
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  ASSY_DETAIL_DOWNTIME_REASON_DICT,
  ASSY_DETAIL_UNACHIEVED_REASON_DICT,
} from './assyOutputDetailDictMulti'
import { useAssyOutputDetailDictMultiFormat } from './useAssyOutputDetailDictMultiFormat'

/** 内嵌子表多选字典字段 */
export type AssyOutputDetailDictMultiField = 'downtimeReason' | 'unachievedReason'

const DICT_FIELD_CONFIG: Record<
  AssyOutputDetailDictMultiField,
  { dictTypeCode: string }
> = {
  downtimeReason: { dictTypeCode: ASSY_DETAIL_DOWNTIME_REASON_DICT },
  unachievedReason: { dictTypeCode: ASSY_DETAIL_UNACHIEVED_REASON_DICT },
}

/**
 * 内嵌子表多选字典行内编辑（TaktEditableTable #cell-downtimeReason / #cell-unachievedReason）
 */
export function useAssyOutputDetailEditableDict() {
  const {
    parseDowntimeReasonForSelect,
    parseUnachievedReasonForSelect,
    sortDowntimeReasonValues,
    sortUnachievedReasonValues,
    formatDowntimeReason,
    formatUnachievedReason,
    alignDictMultiValuesToSelectOptions,
  } = useAssyOutputDetailDictMultiFormat()

  /**
   * 解析/排序/对齐字典 options 后的 Select 绑定值；未选为 undefined
   * @param raw 行内原始值
   * @param field 字段名
   */
  function sanitizeDetailDictMultiSelectValue(
    raw: unknown,
    field: AssyOutputDetailDictMultiField,
  ): (string | number)[] | undefined {
    if (raw == null || raw === '') {
      return undefined
    }
    if (typeof raw === 'number' && raw === 0) {
      return undefined
    }
    if (Array.isArray(raw) && raw.length === 0) {
      return undefined
    }
    const parseFn = field === 'downtimeReason' ? parseDowntimeReasonForSelect : parseUnachievedReasonForSelect
    const sortFn = field === 'downtimeReason' ? sortDowntimeReasonValues : sortUnachievedReasonValues
    const dictTypeCode = DICT_FIELD_CONFIG[field].dictTypeCode
    const parsed = Array.isArray(raw) ? raw : parseFn(raw as string | number)
    const filtered = parsed.filter((item) => {
      if (item == null) {
        return false
      }
      const text = String(item).trim()
      return text !== '' && text !== '0' && text !== 'undefined'
    })
    if (filtered.length === 0) {
      return undefined
    }
    const sorted = sortFn(filtered)
    if (sorted.length === 0) {
      return undefined
    }
    return alignDictMultiValuesToSelectOptions(sorted, dictTypeCode)
  }

  /** 子表行内多选字典字段规范（未选时 delete 字段，不用 []） */
  function ensureDetailDictMultiFields(row: Record<string, unknown>) {
    for (const field of Object.keys(DICT_FIELD_CONFIG) as AssyOutputDetailDictMultiField[]) {
      const sanitized = sanitizeDetailDictMultiSelectValue(row[field], field)
      if (sanitized === undefined) {
        delete row[field]
      } else {
        row[field] = sanitized
      }
    }
  }

  /** TaktSelect :model-value（只读计算） */
  function getDetailDictMultiSelectModelValue(
    record: Record<string, unknown>,
    field: AssyOutputDetailDictMultiField,
  ): (string | number)[] | undefined {
    return sanitizeDetailDictMultiSelectValue(record[field], field)
  }

  /** TaktSelect @update:model-value 写回行内 */
  function applyDetailDictMultiChange(
    record: Record<string, unknown>,
    field: AssyOutputDetailDictMultiField,
    values: string | number | readonly (string | number)[] | null | undefined,
  ) {
    const sanitized = sanitizeDetailDictMultiSelectValue(values, field)
    if (sanitized === undefined) {
      delete record[field]
    } else {
      record[field] = sanitized
    }
  }

  /** 提交前：Select 多选值 → 库内 DictLabel 串 */
  function normalizeAssyOutputDetailRowForSubmit(row: Record<string, unknown>) {
    return {
      ...row,
      downtimeReason: formatDowntimeReason(row.downtimeReason),
      unachievedReason: formatUnachievedReason(row.unachievedReason),
    }
  }

  return {
    parseDowntimeReasonForSelect,
    sortDowntimeReasonValues,
    ensureDetailDictMultiFields,
    getDetailDictMultiSelectModelValue,
    applyDetailDictMultiChange,
    normalizeAssyOutputDetailRowForSubmit,
  }
}
