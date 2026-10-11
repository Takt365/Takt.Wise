// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect/composables
// 文件名称：useAssyDefectDetailDictFormat.ts
// 功能描述：组立不良明细字典/修理员：库内 Label；TaktSelect 绑定 Value（与产出日报 TaktOutputDictMultiFieldsHelper 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import type { TaktSelectOption } from '@/types/common.types'
import {
  dictValueToLabel,
  resolveDictPartToLabel,
  resolveDictPartToValue,
} from '@/utils/taktDictConvert'
import {
  ASSY_DEFECT_DETAIL_DEFECT_CATEGORY_DICT,
  ASSY_DEFECT_DETAIL_DEFECT_LOCATION_DICT,
} from './assyDefectDetailDict'

/**
 * 单选值与 options.dictValue 对齐（兼容整型 DictValue）
 * @param raw 库内 Label/Value 或 Select 绑定值
 * @param options 字典/员工选项
 * @returns Select 绑定值；无有效项时为 undefined
 */
function alignSelectOptionValue(
  raw: unknown,
  options: readonly Pick<TaktSelectOption, 'dictValue' | 'dictLabel'>[],
): string | number | undefined {
  if (raw == null || raw === '') {
    return undefined
  }
  if (typeof raw === 'number' && raw === 0) {
    return undefined
  }
  const text = String(raw).trim()
  if (!text || text === '0') {
    return undefined
  }
  if (!options.length) {
    return text
  }
  const valueKey = resolveDictPartToValue(text, options)
  if (!valueKey) {
    return undefined
  }
  const matched = options.find((opt) => String(opt.dictValue ?? '').trim() === valueKey)
  if (!matched) {
    return undefined
  }
  const rawValue = matched.dictValue ?? valueKey
  const num = Number(rawValue)
  if (
    Number.isSafeInteger(num)
    && String(num) === String(rawValue).trim()
  ) {
    return num
  }
  return String(rawValue)
}

/**
 * 单选字典/员工：Select 绑定值 → 库内 Label（员工姓名）
 * @param selectValue Select 绑定值
 * @param options 字典/员工选项
 * @returns 存储 Label
 */
function formatSelectValueToStoredLabel(
  selectValue: unknown,
  options: readonly Pick<TaktSelectOption, 'dictValue' | 'dictLabel'>[],
): string {
  if (selectValue == null || selectValue === '') {
    return ''
  }
  const key = String(selectValue).trim()
  if (!key) {
    return ''
  }
  if (!options.length) {
    return key
  }
  const valueKey = resolveDictPartToValue(key, options)
  return resolveDictPartToLabel(valueKey, options)
}

/**
 * 组立不良明细字典/修理员格式化（依赖 Pinia 字典缓存与员工选项）
 * @returns 不良区分/个所/修理员：Select 绑定 ↔ 库内 Label
 */
export function useAssyDefectDetailDictFormat() {
  const dictStore = useDictDataStore()
  /** 员工下拉选项（修理员 Label↔Id 转换） */
  const employeeOptions = shallowRef<TaktSelectOption[]>([])

  /**
   * 预加载员工选项（修理员字段转换用）
   */
  async function loadEmployeeOptionsAsync() {
    if (employeeOptions.value.length > 0) {
      return
    }
    try {
      employeeOptions.value = await getEmployeeOptions()
    } catch {
      employeeOptions.value = []
    }
  }

  /**
   * 获取字典项列表
   * @param dictTypeCode 字典类型编码
   */
  function getDictOptions(dictTypeCode: string) {
    return dictStore.dictMap[dictTypeCode] ?? []
  }

  /**
   * 库内 Label（或历史 Value）→ TaktSelect DictValue
   * @param raw API/行内存储值
   * @param dictTypeCode 字典类型编码
   */
  function parseDictFieldForSelect(
    raw: unknown,
    dictTypeCode: string,
  ): string | number | undefined {
    return alignSelectOptionValue(raw, getDictOptions(dictTypeCode))
  }

  /**
   * TaktSelect DictValue → 库内 DictLabel
   * @param selectValue Select 绑定值
   * @param dictTypeCode 字典类型编码
   */
  function formatDictFieldForStorage(
    selectValue: unknown,
    dictTypeCode: string,
  ): string {
    return formatSelectValueToStoredLabel(selectValue, getDictOptions(dictTypeCode))
  }

  /**
   * 不良区分：库内值 → Select 绑定
   * @param raw 存储值
   */
  function parseDefectCategoryForSelect(raw: unknown): string | number | undefined {
    return parseDictFieldForSelect(raw, ASSY_DEFECT_DETAIL_DEFECT_CATEGORY_DICT)
  }

  /**
   * 不良个所：库内值 → Select 绑定
   * @param raw 存储值
   */
  function parseDefectLocationForSelect(raw: unknown): string | number | undefined {
    return parseDictFieldForSelect(raw, ASSY_DEFECT_DETAIL_DEFECT_LOCATION_DICT)
  }

  /**
   * 不良区分：Select 绑定 → 库内 DictLabel
   * @param selectValue Select 绑定值
   */
  function formatDefectCategoryForStorage(selectValue: unknown): string {
    return formatDictFieldForStorage(selectValue, ASSY_DEFECT_DETAIL_DEFECT_CATEGORY_DICT)
  }

  /**
   * 不良个所：Select 绑定 → 库内 DictLabel
   * @param selectValue Select 绑定值
   */
  function formatDefectLocationForStorage(selectValue: unknown): string {
    return formatDictFieldForStorage(selectValue, ASSY_DEFECT_DETAIL_DEFECT_LOCATION_DICT)
  }

  /**
   * 修理员：库内员工姓名（或历史 Id）→ Select 绑定 Id
   * @param raw 存储值
   */
  function parseRepairOperatorForSelect(raw: unknown): string | number | undefined {
    return alignSelectOptionValue(raw, employeeOptions.value)
  }

  /**
   * 修理员：Select 绑定 Id → 库内员工姓名
   * @param selectValue Select 绑定值
   */
  function formatRepairOperatorForStorage(selectValue: unknown): string {
    if (selectValue == null || selectValue === '') {
      return ''
    }
    const key = String(selectValue).trim()
    if (!key) {
      return ''
    }
    const options = employeeOptions.value
    if (!options.length) {
      return key
    }
    return dictValueToLabel(key, options, key)
  }

  /**
   * 高级查询：TaktSelect DictValue → 库内 Label（Contains 查询用）
   * @param dictValue 下拉选中值
   * @param dictTypeCode 字典类型编码
   */
  function resolveQueryDictLabel(
    dictValue: string | number | null | undefined,
    dictTypeCode: string,
  ): string {
    if (dictValue == null || dictValue === '') {
      return ''
    }
    return formatDictFieldForStorage(dictValue, dictTypeCode)
  }

  /**
   * 高级查询：修理员 Select Id → 员工姓名
   * @param employeeId 员工 Id
   */
  function resolveQueryRepairOperatorLabel(
    employeeId: string | number | null | undefined,
  ): string {
    return formatRepairOperatorForStorage(employeeId)
  }

  return {
    loadEmployeeOptionsAsync,
    parseDefectCategoryForSelect,
    parseDefectLocationForSelect,
    parseRepairOperatorForSelect,
    formatDefectCategoryForStorage,
    formatDefectLocationForStorage,
    formatRepairOperatorForStorage,
    resolveQueryDictLabel,
    resolveQueryRepairOperatorLabel,
  }
}
