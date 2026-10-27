// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFormTabs.ts
// 创建时间：2026-10-27
// 创建人：Takt365(Cursor AI)
// 功能描述：表单页签字段分页（与 scripts/gen/generate-vue-common.cjs FORM_TAB_* 对齐）；供 TaktFormTabs / 手工表单使用
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 每 Tab 最多字段数（满 Tab：2 列 × 5 行 = 10）
 * @description 必须与 scripts/gen/generate-vue-common.cjs → FORM_TAB_FIELDS_PER_TAB 保持一致
 */
export const TAKT_FORM_TAB_FIELDS_PER_TAB = 10

/** 首 Tab 固定前缀字段顺序：工厂 → 区域文化 */
export const TAKT_FORM_TAB_LEADING_FIELD_NAMES = [
  'plantCode',
  'relatedPlant',
  'cultureCode',
] as const

/** 末 Tab 固定后缀：租户/公司/扩展/备注 */
export const TAKT_FORM_TAB_TRAILING_FIELD_NAMES = [
  'tenantCode',
  'companyCode',
  'extField',
  'remark',
] as const

const LEADING_SET = new Set<string>(TAKT_FORM_TAB_LEADING_FIELD_NAMES)
const TRAILING_SET = new Set<string>(TAKT_FORM_TAB_TRAILING_FIELD_NAMES)

/** 可分页字段（至少含 name） */
export type TaktFormTabFieldLike = {
  name: string
}

/**
 * 按流水线规则拆分表单字段为多 Tab
 * @description 首 Tab：leading + 业务凑满；中间：业务分页；末 Tab：trailing
 * @param fields 字段列表
 * @param fieldsPerTab 每 Tab 容量，默认 TAKT_FORM_TAB_FIELDS_PER_TAB
 * @returns 各 Tab 字段数组
 */
export function taktPartitionFormFieldsForTabs<T extends TaktFormTabFieldLike>(
  fields: readonly T[],
  fieldsPerTab: number = TAKT_FORM_TAB_FIELDS_PER_TAB,
): T[][] {
  if (!fields?.length) {
    return [[]]
  }
  const perTab = Math.max(1, fieldsPerTab)
  const leadingByName = new Map<string, T>()
  const trailingByName = new Map<string, T>()
  const main: T[] = []
  for (const field of fields) {
    if (LEADING_SET.has(field.name)) {
      leadingByName.set(field.name, field)
    } else if (TRAILING_SET.has(field.name)) {
      trailingByName.set(field.name, field)
    } else {
      main.push(field)
    }
  }
  const leading = TAKT_FORM_TAB_LEADING_FIELD_NAMES
    .map((name) => leadingByName.get(name))
    .filter((item): item is T => item != null)
  const trailing = TAKT_FORM_TAB_TRAILING_FIELD_NAMES
    .map((name) => trailingByName.get(name))
    .filter((item): item is T => item != null)
  if (main.length === 0) {
    if (leading.length && trailing.length) {
      return [leading, trailing]
    }
    if (leading.length) {
      return [leading]
    }
    if (trailing.length) {
      return [trailing]
    }
    return [[]]
  }
  const firstMainCapacity = Math.max(0, perTab - leading.length)
  const firstMain = main.slice(0, firstMainCapacity)
  const restMain = main.slice(firstMainCapacity)
  const mainTabs: T[][] = [[...leading, ...firstMain]]
  for (let i = 0; i < restMain.length; i += perTab) {
    mainTabs.push(restMain.slice(i, i + perTab))
  }
  if (trailing.length) {
    mainTabs.push(trailing)
  }
  return mainTabs
}

/**
 * 表单 Tab 数量（与流水线 computeFormTabCount 一致）
 * @param fieldsOrCount 字段数组或仅总数（仅总数时用简单 ceil，无 leading/trailing）
 * @param fieldsPerTab 每 Tab 容量
 * @returns 至少 1
 */
export function taktComputeFormTabCount(
  fieldsOrCount: readonly TaktFormTabFieldLike[] | number,
  fieldsPerTab: number = TAKT_FORM_TAB_FIELDS_PER_TAB,
): number {
  if (Array.isArray(fieldsOrCount)) {
    return Math.max(1, taktPartitionFormFieldsForTabs(fieldsOrCount, fieldsPerTab).length)
  }
  const fieldCount = Number(fieldsOrCount) || 0
  const perTab = Math.max(1, fieldsPerTab)
  return Math.max(1, Math.ceil(fieldCount / perTab))
}

/**
 * 基础信息页签标题（多 Tab 时追加 当前/总数）
 * @param tabIndex 1-based
 * @param tabCount 总数
 * @param basicInfoLabel 已翻译的「基础信息」文案
 * @returns 页签标题
 */
export function taktBuildFormTabLabel(
  tabIndex: number,
  tabCount: number,
  basicInfoLabel: string,
): string {
  if (tabCount <= 1) {
    return basicInfoLabel
  }
  return `${basicInfoLabel} (${tabIndex}/${tabCount})`
}

/**
 * 按 Tab 数量生成默认 items（key=tab-0…）
 * @param tabCount 页签数
 * @param basicInfoLabel 已翻译基础信息文案
 * @returns items
 */
export function taktBuildBasicInfoFormTabItems(
  tabCount: number,
  basicInfoLabel: string,
): Array<{ key: string; label: string; forceRender: boolean }> {
  const count = Math.max(1, tabCount)
  return Array.from({ length: count }, (_, index) => ({
    key: `tab-${index}`,
    label: taktBuildFormTabLabel(index + 1, count, basicInfoLabel),
    forceRender: true,
  }))
}

/**
 * 当前 Tab 字段数是否应用单列布局（&lt; fieldsPerTab）
 * @param tabFieldCount 当前 Tab 字段数
 * @param fieldsPerTab 每 Tab 容量
 * @returns 是否单列
 */
export function taktShouldUseSingleColumnFormLayout(
  tabFieldCount: number,
  fieldsPerTab: number = TAKT_FORM_TAB_FIELDS_PER_TAB,
): boolean {
  return tabFieldCount < Math.max(1, fieldsPerTab)
}
