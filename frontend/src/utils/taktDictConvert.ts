// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktDictConvert
// 文件名称：taktDictConvert.ts
// 功能描述：字典 DictValue↔DictLabel 转换（单选/多选逗号分隔；与后端 TaktDictValueHelper、TaktDictMultiValueHelper 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktSelectOption } from '@/types/common.types'

/** 多选字典存储格式：value=DictValue 逗号分隔；label=DictLabel 逗号分隔 */
export type TaktDictMultiStorageFormat = 'value' | 'label'

/** 字典选项最小字段（转换/排序用） */
export type TaktDictConvertOption = Pick<TaktSelectOption, 'dictValue' | 'dictLabel' | 'sortOrder'>

/**
 * 构建 DictValue → DictLabel 查找表
 * @param options 字典项列表
 * @returns 查找表
 */
export function buildDictValueToLabelLookup(
  options: readonly TaktDictConvertOption[],
): Map<string, string> {
  const map = new Map<string, string>()
  for (const item of options) {
    const valueKey = String(item.dictValue ?? '').trim()
    if (!valueKey) {
      continue
    }
    const label = String(item.dictLabel ?? valueKey).trim()
    map.set(valueKey, label || valueKey)
  }
  return map
}

/**
 * 构建 DictLabel → DictValue 查找表
 * @param options 字典项列表
 * @returns 查找表
 */
export function buildDictLabelToValueLookup(
  options: readonly TaktDictConvertOption[],
): Map<string, string> {
  const map = new Map<string, string>()
  for (const item of options) {
    const labelKey = String(item.dictLabel ?? '').trim()
    const valueKey = String(item.dictValue ?? '').trim()
    if (!labelKey || !valueKey) {
      continue
    }
    map.set(labelKey, valueKey)
  }
  return map
}

/**
 * 构建 DictValue → sortOrder 查找表
 * @param options 字典项列表
 * @returns 查找表
 */
export function buildDictSortOrderLookup(
  options: readonly Pick<TaktSelectOption, 'dictValue' | 'sortOrder'>[],
): Map<string, number> {
  const map = new Map<string, number>()
  for (const item of options) {
    const key = String(item.dictValue ?? '').trim()
    if (!key) {
      continue
    }
    map.set(key, item.sortOrder ?? Number.MAX_SAFE_INTEGER)
  }
  return map
}

/**
 * 拆分逗号分隔字典片段
 * @param raw API/行内字符串或已解析数组
 * @returns 片段数组
 */
export function splitCommaSeparatedDictParts(
  raw: string | number | readonly (string | number)[] | null | undefined,
): string[] {
  if (raw == null || raw === '') {
    return []
  }
  if (Array.isArray(raw)) {
    return raw
      .map((item) => String(item).trim())
      .filter((item) => item !== '')
  }
  const text = String(raw).trim()
  if (!text) {
    return []
  }
  return text.split(',').map((part) => part.trim()).filter(Boolean)
}

/**
 * 拼接逗号分隔字典片段
 * @param parts 片段数组
 * @returns 逗号分隔串
 */
export function joinCommaSeparatedDictParts(parts: readonly string[]): string {
  const list = parts.map((part) => part.trim()).filter(Boolean)
  return list.length === 0 ? '' : list.join(',')
}

/**
 * 解析单个片段为 canonical DictValue（片段可为 DictValue 或 DictLabel）
 * @param part 单个片段
 * @param options 字典项列表
 * @returns DictValue；无法解析时返回 trim 后的原片段
 */
export function resolveDictPartToValue(
  part: string,
  options: readonly TaktDictConvertOption[],
): string {
  const trimmed = part.trim()
  if (!trimmed) {
    return ''
  }
  const valueToLabel = buildDictValueToLabelLookup(options)
  if (valueToLabel.has(trimmed)) {
    return trimmed
  }
  const labelToValue = buildDictLabelToValueLookup(options)
  return labelToValue.get(trimmed) ?? trimmed
}

/**
 * 解析单个片段为 canonical DictLabel（片段可为 DictValue 或 DictLabel）
 * @param part 单个片段
 * @param options 字典项列表
 * @returns DictLabel；无法解析时返回 trim 后的原片段
 */
export function resolveDictPartToLabel(
  part: string,
  options: readonly TaktDictConvertOption[],
): string {
  const trimmed = part.trim()
  if (!trimmed) {
    return ''
  }
  const labelToValue = buildDictLabelToValueLookup(options)
  if (labelToValue.has(trimmed)) {
    return trimmed
  }
  const valueToLabel = buildDictValueToLabelLookup(options)
  return valueToLabel.get(trimmed) ?? trimmed
}

/**
 * DictValue → DictLabel（单选）
 * @param dictValue DictValue
 * @param options 字典项列表
 * @param fallback 未命中回退
 * @returns DictLabel
 */
export function dictValueToLabel(
  dictValue: string | number | null | undefined,
  options: readonly TaktDictConvertOption[],
  fallback?: string,
): string {
  const key = String(dictValue ?? '').trim()
  if (!key) {
    return fallback ?? ''
  }
  const lookup = buildDictValueToLabelLookup(options)
  return lookup.get(key) ?? fallback ?? key
}

/**
 * DictLabel → DictValue（单选）
 * @param dictLabel DictLabel
 * @param options 字典项列表
 * @returns DictValue；未命中 null
 */
export function dictLabelToValue(
  dictLabel: string | null | undefined,
  options: readonly TaktDictConvertOption[],
): string | null {
  const key = String(dictLabel ?? '').trim()
  if (!key) {
    return null
  }
  const lookup = buildDictLabelToValueLookup(options)
  return lookup.get(key) ?? null
}

/**
 * 按 DictValue 的 sortOrder 对片段升序排序
 * @param parts 片段（应为 DictValue）
 * @param sortOrderByValue DictValue → sortOrder
 * @returns 排序后的片段
 */
export function sortDictValuesBySortOrder(
  parts: readonly string[],
  sortOrderByValue: ReadonlyMap<string, number>,
): string[] {
  const resolveOrder = (value: string): number => {
    const key = value.trim()
    if (!key) {
      return Number.MAX_SAFE_INTEGER
    }
    return sortOrderByValue.get(key) ?? Number.MAX_SAFE_INTEGER
  }
  return [...parts]
    .map((item) => item.trim())
    .filter(Boolean)
    .sort((a, b) => {
      const diff = resolveOrder(a) - resolveOrder(b)
      if (diff !== 0) {
        return diff
      }
      return a.localeCompare(b)
    })
}

/**
 * 规范化多选字典逗号分隔存储（输入可为 DictValue 或 DictLabel 混用）
 * @param raw 原始逗号分隔串或数组
 * @param options 字典项列表
 * @param storageFormat 目标存储格式：value 或 label
 * @returns 规范化后的逗号分隔串
 */
export function normalizeCommaSeparatedDictStorage(
  raw: string | number | readonly (string | number)[] | null | undefined,
  options: readonly TaktDictConvertOption[],
  storageFormat: TaktDictMultiStorageFormat,
): string {
  const parts = splitCommaSeparatedDictParts(raw)
  if (parts.length === 0 || options.length === 0) {
    return joinCommaSeparatedDictParts(parts)
  }
  const resolvedValues = parts
    .map((part) => resolveDictPartToValue(part, options))
    .filter(Boolean)
  if (resolvedValues.length === 0) {
    return ''
  }
  const sortOrderByValue = buildDictSortOrderLookup(options)
  const sortedValues = resolvedValues.length > 1
    ? sortDictValuesBySortOrder(resolvedValues, sortOrderByValue)
    : resolvedValues
  if (storageFormat === 'label') {
    const labels = sortedValues.map((value) => resolveDictPartToLabel(value, options)).filter(Boolean)
    return joinCommaSeparatedDictParts(labels)
  }
  return joinCommaSeparatedDictParts(sortedValues)
}

/**
 * 规范化单选字典落库（输入可为 DictValue 或 DictLabel；含逗号时按多选处理）
 * @param raw 原始值
 * @param options 字典项列表
 * @param storageFormat 目标存储格式：value 或 label
 * @returns 规范化后的值
 */
export function normalizeSingleDictStorage(
  raw: string | number | null | undefined,
  options: readonly TaktDictConvertOption[],
  storageFormat: TaktDictMultiStorageFormat,
): string {
  const text = raw == null ? '' : String(raw).trim()
  if (!text) {
    return ''
  }
  if (text.includes(',')) {
    return normalizeCommaSeparatedDictStorage(text, options, storageFormat)
  }
  if (storageFormat === 'label') {
    return resolveDictPartToLabel(text, options)
  }
  return resolveDictPartToValue(text, options)
}

/**
 * 逗号分隔 DictValue → DictLabel
 * @param raw DictValue 逗号分隔串
 * @param options 字典项列表
 * @returns DictLabel 逗号分隔串
 */
export function convertCommaSeparatedDictValuesToLabels(
  raw: string | number | readonly (string | number)[] | null | undefined,
  options: readonly TaktDictConvertOption[],
): string {
  return normalizeCommaSeparatedDictStorage(raw, options, 'label')
}

/**
 * 逗号分隔 DictLabel（或混用）→ DictValue
 * @param raw 原始串
 * @param options 字典项列表
 * @returns DictValue 逗号分隔串
 */
export function convertCommaSeparatedDictLabelsToValues(
  raw: string | number | readonly (string | number)[] | null | undefined,
  options: readonly TaktDictConvertOption[],
): string {
  return normalizeCommaSeparatedDictStorage(raw, options, 'value')
}

/**
 * 解析逗号分隔 DictValue 为 TaktSelect 多选绑定值（兼容整型 DictValue）
 * @param raw API/行内字符串或已解析数组
 * @returns 多选值数组
 */
export function parseCommaSeparatedDictMultiValue(
  raw: string | number | readonly (string | number)[] | null | undefined,
): (string | number)[] {
  return splitCommaSeparatedDictParts(raw).map((part) => {
    const num = Number(part)
    if (Number.isSafeInteger(num) && String(num) === part) {
      return num
    }
    return part
  })
}

/**
 * 多选 DictValue 序列化为逗号分隔字符串（可选按 sortOrder 排序）
 * @param values TaktSelect 多选绑定值
 * @param options 字典项（用于排序）
 * @returns 逗号分隔 DictValue
 */
export function formatCommaSeparatedDictMultiValue(
  values: string | number | readonly (string | number)[] | null | undefined,
  options?: readonly Pick<TaktSelectOption, 'dictValue' | 'sortOrder'>[],
): string {
  const parsed = parseCommaSeparatedDictMultiValue(values)
  if (parsed.length === 0) {
    return ''
  }
  const asStrings = parsed.map((item) => String(item).trim()).filter(Boolean)
  if (!options?.length) {
    return joinCommaSeparatedDictParts(asStrings)
  }
  const sorted = sortDictValuesBySortOrder(asStrings, buildDictSortOrderLookup(options))
  return joinCommaSeparatedDictParts(sorted)
}
