// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：assyOutputDetailDictMulti.ts
// 功能描述：产出日报明细停线/未达成原因：库内 DictLabel 逗号分隔；Select 绑定 DictValue（见 @/utils/taktDictConvert）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktSelectOption } from '@/types/common.types'
import {
  buildDictSortOrderLookup,
  normalizeCommaSeparatedDictStorage,
  parseCommaSeparatedDictMultiValue,
  sortDictValuesBySortOrder,
} from '@/utils/taktDictConvert'

/** 停线原因字典 */
export const ASSY_DETAIL_DOWNTIME_REASON_DICT = 'logistics_manufacturing_stop_reason'
/** 未达成原因字典 */
export const ASSY_DETAIL_UNACHIEVED_REASON_DICT = 'logistics_manufacturing_nonachievement_reason'

export { buildDictSortOrderLookup }

/**
 * 按字典 sortOrder 对多选 DictValue 升序排序
 * @param values 多选绑定值
 * @param sortOrderByValue DictValue → sortOrder
 * @returns 排序后的多选值
 */
export function sortAssyDetailDictMultiValues(
  values: readonly (string | number)[],
  sortOrderByValue: ReadonlyMap<string, number>,
): (string | number)[] {
  const asStrings = values.map((item) => String(item).trim()).filter(Boolean)
  const sorted = sortDictValuesBySortOrder(asStrings, sortOrderByValue)
  return sorted.map((part) => {
    const num = Number(part)
    if (Number.isSafeInteger(num) && String(num) === part) {
      return num
    }
    return part
  })
}

/**
 * 规范化多选字典为库内 DictLabel 存储
 * @param raw 原始串或 Select 多选值
 * @param dictOptions 字典项
 * @returns DictLabel 逗号分隔串
 */
export function formatAssyDetailDictMultiStorage(
  raw: string | number | readonly (string | number)[] | null | undefined,
  dictOptions: readonly Pick<TaktSelectOption, 'dictValue' | 'dictLabel' | 'sortOrder'>[],
): string {
  return normalizeCommaSeparatedDictStorage(raw, dictOptions, 'label')
}

/**
 * @deprecated 库内存 Label；请用 useAssyOutputDetailDictMultiFormat().parseDowntimeReasonForSelect
 */
export function parseAssyDetailDictMultiValue(
  raw: string | number | readonly (string | number)[] | null | undefined,
): (string | number)[] {
  return parseCommaSeparatedDictMultiValue(raw)
}
