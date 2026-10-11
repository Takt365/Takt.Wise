// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/pcba-output/composables
// 文件名称：usePcbaOutputDetailDictFormat.ts
// 功能描述：PCBA 日报明细字典：TaktSelect 绑定 DictValue，提交/落库 DictLabel（依赖 Pinia 实时字典缓存）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import {
  normalizeCommaSeparatedDictStorage,
  normalizeSingleDictStorage,
  parseCommaSeparatedDictMultiValue,
} from '@/utils/taktDictConvert'
import {
  PCBA_DETAIL_DOWNTIME_REASON_DICT,
  PCBA_DETAIL_PCB_BOARD_TYPE_DICT,
  PCBA_DETAIL_UNACHIEVED_REASON_DICT,
} from './pcbaOutputDetailDictFormat'
import {
  buildDictSortOrderLookup,
  sortAssyDetailDictMultiValues,
} from '../../assy-output/composables/assyOutputDetailDictMulti'

/**
 * PCBA 日报明细字典格式化（依赖 Pinia 字典缓存，与后端 UI 提交口径一致）
 */
export function usePcbaOutputDetailDictFormat() {
  const dictStore = useDictDataStore()

  /** 获取字典项列表 */
  function getDictOptions(dictTypeCode: string) {
    return dictStore.dictMap[dictTypeCode] ?? []
  }

  /** 库内串 → TaktSelect 多选 DictValue */
  function parseMultiSelectForForm(
    raw: string | number | readonly (string | number)[] | null | undefined,
    dictTypeCode: string,
  ): (string | number)[] {
    const options = getDictOptions(dictTypeCode)
    if (!options.length) {
      return parseCommaSeparatedDictMultiValue(raw)
    }
    const valueString = normalizeCommaSeparatedDictStorage(raw, options, 'value')
    const parsed = parseCommaSeparatedDictMultiValue(valueString)
    return sortAssyDetailDictMultiValues(parsed, buildDictSortOrderLookup(options))
  }

  /** 库内串 → TaktSelect 单选 DictValue */
  function parseSingleSelectForForm(
    raw: string | number | null | undefined,
    dictTypeCode: string,
  ): string | number {
    const options = getDictOptions(dictTypeCode)
    if (!options.length) {
      return raw == null ? '' : String(raw)
    }
    return normalizeSingleDictStorage(raw, options, 'value')
  }

  /** 停线原因：Select → 库内 DictLabel */
  function formatDowntimeReason(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): string {
    return normalizeCommaSeparatedDictStorage(
      values,
      getDictOptions(PCBA_DETAIL_DOWNTIME_REASON_DICT),
      'label',
    )
  }

  /** 未达成原因：Select → 库内 DictLabel */
  function formatUnachievedReason(
    values: string | number | readonly (string | number)[] | null | undefined,
  ): string {
    return normalizeCommaSeparatedDictStorage(
      values,
      getDictOptions(PCBA_DETAIL_UNACHIEVED_REASON_DICT),
      'label',
    )
  }

  /** PCB 板别：Select → 库内 DictLabel */
  function formatPcbBoardType(
    value: string | number | null | undefined,
  ): string {
    return normalizeSingleDictStorage(
      value,
      getDictOptions(PCBA_DETAIL_PCB_BOARD_TYPE_DICT),
      'label',
    )
  }

  /** 灌入表单：库内 Label/Value 串 → Select 绑定值 */
  function hydrateDetailDictFields(target: Record<string, unknown>) {
    if ('downtimeReason' in target) {
      target.downtimeReason = parseMultiSelectForForm(
        target.downtimeReason as string,
        PCBA_DETAIL_DOWNTIME_REASON_DICT,
      )
    }
    if ('unachievedReason' in target) {
      target.unachievedReason = parseMultiSelectForForm(
        target.unachievedReason as string,
        PCBA_DETAIL_UNACHIEVED_REASON_DICT,
      )
    }
    if ('pcbBoardType' in target) {
      target.pcbBoardType = parseSingleSelectForForm(
        target.pcbBoardType as string,
        PCBA_DETAIL_PCB_BOARD_TYPE_DICT,
      )
    }
  }

  /** 提交 DTO：Select 绑定值 → 库内 DictLabel */
  function formatDetailDictFieldsForSubmit(target: Record<string, unknown>) {
    if ('downtimeReason' in target) {
      target.downtimeReason = formatDowntimeReason(
        target.downtimeReason as string | number | readonly (string | number)[] | null | undefined,
      )
    }
    if ('unachievedReason' in target) {
      target.unachievedReason = formatUnachievedReason(
        target.unachievedReason as string | number | readonly (string | number)[] | null | undefined,
      )
    }
    if ('pcbBoardType' in target) {
      target.pcbBoardType = formatPcbBoardType(target.pcbBoardType as string | number | null | undefined)
    }
  }

  return {
    formatDowntimeReason,
    formatUnachievedReason,
    formatPcbBoardType,
    hydrateDetailDictFields,
    formatDetailDictFieldsForSubmit,
  }
}
