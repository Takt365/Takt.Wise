// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputDetailDictMultiFormat.ts
// 功能描述：产出日报明细停线/未达成原因：库内存 DictLabel；TaktSelect 绑定 DictValue；提交按 sortOrder 排序
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import {
  dictValueToLabel,
  normalizeCommaSeparatedDictStorage,
  parseCommaSeparatedDictMultiValue,
} from '@/utils/taktDictConvert'
import {
  ASSY_DETAIL_DOWNTIME_REASON_DICT,
  ASSY_DETAIL_UNACHIEVED_REASON_DICT,
  sortAssyDetailDictMultiValues,
  buildDictSortOrderLookup,
} from './assyOutputDetailDictMulti'

/**
 * 产出日报明细多选字典格式化（依赖 Pinia 字典缓存）
 * @returns 停线/未达成原因：Select 绑定 ↔ 库内 Label 串
 */
export function useAssyOutputDetailDictMultiFormat() {
  const dictStore = useDictDataStore()

  /**
   * 获取字典项列表
   * @param dictTypeCode 字典类型编码
   */
  function getDictOptions(dictTypeCode: string) {
    return dictStore.dictMap[dictTypeCode] ?? []
  }

  /**
   * 库内 Label 串（或历史 Value 串）→ TaktSelect 多选 DictValue 数组（按 sortOrder 排序）
   * @param raw API/行内存储串
   * @param dictTypeCode 字典类型编码
   */
  function parseStoredToSelectValues(
    raw: string | number | readonly (string | number)[] | null | undefined,
    dictTypeCode: string,
  ): (string | number)[] {
    const options = getDictOptions(dictTypeCode)
    if (!options.length) {
      return parseCommaSeparatedDictMultiValue(raw)
    }
    const valueString = normalizeCommaSeparatedDictStorage(raw, options, 'value')
    const parsed = parseCommaSeparatedDictMultiValue(valueString)
    return sortAssyDetailDictMultiValues(
      parsed,
      buildDictSortOrderLookup(options),
    )
  }

  /**
   * 停线原因：库内串 → Select 绑定值
   * @param raw 存储串
   */
  function parseDowntimeReasonForSelect(
    raw: string | number | readonly (string | number)[] | null | undefined,
  ): (string | number)[] {
    return parseStoredToSelectValues(raw, ASSY_DETAIL_DOWNTIME_REASON_DICT)
  }

  /**
   * 未达成原因：库内串 → Select 绑定值
   * @param raw 存储串
   */
  function parseUnachievedReasonForSelect(
    raw: string | number | readonly (string | number)[] | null | undefined,
  ): (string | number)[] {
    return parseStoredToSelectValues(raw, ASSY_DETAIL_UNACHIEVED_REASON_DICT)
  }

  /**
   * 多选绑定值与字典选项对齐（剔除无法命中 DictValue 的项，避免 TaktSelect 空白 ×）
   * @param values 候选多选值
   * @param dictTypeCode 字典类型编码
   * @returns 与 options.value 类型一致的有效多选值；无有效项时为 undefined
   */
  function alignDictMultiValuesToSelectOptions(
    values: readonly (string | number)[],
    dictTypeCode: string,
  ): (string | number)[] | undefined {
    if (!values.length) {
      return undefined
    }
    const options = getDictOptions(dictTypeCode)
    if (!options.length) {
      return undefined
    }
    const aligned = values
      .map((item) => {
        const key = String(item).trim()
        if (!key || key === '0') {
          return null
        }
        const matched = options.find((opt) => String(opt.dictValue ?? '').trim() === key)
        if (!matched) {
          return null
        }
        const rawValue = matched.dictValue ?? key
        const num = Number(rawValue)
        if (
          Number.isSafeInteger(num)
          && String(num) === String(rawValue).trim()
        ) {
          return num
        }
        return String(rawValue)
      })
      .filter((item): item is string | number => item != null)
    return aligned.length > 0 ? aligned : undefined
  }

  /**
   * 停线原因：Select 多选值按字典 sortOrder 排序（行内编辑态）
   * @param values 多选绑定值
   */
  function sortDowntimeReasonValues(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): (string | number)[] {
    const options = getDictOptions(ASSY_DETAIL_DOWNTIME_REASON_DICT)
    const parsed = Array.isArray(values)
      ? values
      : parseCommaSeparatedDictMultiValue(values)
    return sortAssyDetailDictMultiValues(parsed, buildDictSortOrderLookup(options))
  }

  /**
   * 未达成原因：Select 多选值按字典 sortOrder 排序
   * @param values 多选绑定值
   */
  function sortUnachievedReasonValues(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): (string | number)[] {
    const options = getDictOptions(ASSY_DETAIL_UNACHIEVED_REASON_DICT)
    const parsed = Array.isArray(values)
      ? values
      : parseCommaSeparatedDictMultiValue(values)
    return sortAssyDetailDictMultiValues(parsed, buildDictSortOrderLookup(options))
  }

  /**
   * 停线原因：Select 多选值 → 库内 DictLabel 逗号分隔串（按 sortOrder）
   * @param values 多选绑定值
   */
  function formatDowntimeReason(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): string {
    const options = getDictOptions(ASSY_DETAIL_DOWNTIME_REASON_DICT)
    return normalizeCommaSeparatedDictStorage(values, options, 'label')
  }

  /**
   * 未达成原因：Select 多选值 → 库内 DictLabel 逗号分隔串（按 sortOrder）
   * @param values 多选绑定值
   */
  function formatUnachievedReason(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): string {
    const options = getDictOptions(ASSY_DETAIL_UNACHIEVED_REASON_DICT)
    return normalizeCommaSeparatedDictStorage(values, options, 'label')
  }

  /**
   * 高级查询：TaktSelect 单选 DictValue → 库内 DictLabel（Contains 查询用）
   * @param dictValue 下拉选中值
   * @param dictTypeCode 字典类型编码
   */
  function resolveQueryLabel(
    dictValue: string | number | null | undefined,
    dictTypeCode: string,
  ): string {
    if (dictValue == null || dictValue === '') {
      return ''
    }
    return dictValueToLabel(dictValue, getDictOptions(dictTypeCode), String(dictValue))
  }

  return {
    parseDowntimeReasonForSelect,
    parseUnachievedReasonForSelect,
    sortDowntimeReasonValues,
    sortUnachievedReasonValues,
    formatDowntimeReason,
    formatUnachievedReason,
    resolveQueryLabel,
    alignDictMultiValuesToSelectOptions,
  }
}
