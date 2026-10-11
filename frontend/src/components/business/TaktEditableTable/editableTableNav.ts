// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/components/business/TaktEditableTable
// 文件名称：editableTableNav.ts
// 功能描述：可编辑子表键盘单元格导航（方向键、回车；纯函数，与 TaktEditableTable 配合）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktEditableTableColumn } from './types'

/** 可编辑单元格 data 属性名（用于 DOM 定位） */
export const TAKT_EDITABLE_CELL_ATTR = 'data-takt-editable-cell'

/** 方向键导航方向 */
export type TaktEditableNavDirection = 'up' | 'down' | 'left' | 'right'

/**
 * 可参与方向键导航的列 key 列表（排除只读列，保持列配置顺序）
 * @param columns 列配置
 * @returns 可导航列 key
 */
export function buildEditableNavColumnKeys(columns: readonly TaktEditableTableColumn[]): string[] {
  return columns
    .filter((col) => col.readonly !== true && col.editor !== 'readonly')
    .map((col) => col.key)
}

/**
 * 编码单元格坐标
 * @param rowIndex 行索引（0-based）
 * @param columnKey 列 key
 * @returns 坐标字符串
 */
export function formatEditableCellCoord(rowIndex: number, columnKey: string): string {
  return `${rowIndex}:${columnKey}`
}

/**
 * 解析单元格坐标
 * @param coord 坐标字符串
 * @returns 行索引与列 key；非法时 null
 */
export function parseEditableCellCoord(coord: string): { rowIndex: number; columnKey: string } | null {
  const sep = coord.indexOf(':')
  if (sep < 0) {
    return null
  }
  const rowIndex = Number(coord.slice(0, sep))
  const columnKey = coord.slice(sep + 1)
  if (!Number.isFinite(rowIndex) || !columnKey) {
    return null
  }
  return { rowIndex, columnKey }
}

/**
 * 计算方向键目标单元格
 * @param rowIndex 当前行索引
 * @param columnKey 当前列 key
 * @param direction 方向
 * @param navigableKeys 可导航列（有序）
 * @param rowCount 总行数
 * @returns 目标坐标；无法移动时 null
 */
export function resolveNextEditableCell(
  rowIndex: number,
  columnKey: string,
  direction: TaktEditableNavDirection,
  navigableKeys: readonly string[],
  rowCount: number,
): { rowIndex: number; columnKey: string } | null {
  const colIdx = navigableKeys.indexOf(columnKey)
  if (colIdx < 0 || rowCount <= 0) {
    return null
  }
  if (direction === 'up') {
    return rowIndex > 0 ? { rowIndex: rowIndex - 1, columnKey } : null
  }
  if (direction === 'down') {
    return rowIndex < rowCount - 1 ? { rowIndex: rowIndex + 1, columnKey } : null
  }
  if (direction === 'left') {
    return colIdx > 0 ? { rowIndex, columnKey: navigableKeys[colIdx - 1]! } : null
  }
  if (colIdx < navigableKeys.length - 1) {
    return { rowIndex, columnKey: navigableKeys[colIdx + 1]! }
  }
  return null
}

/**
 * 左右方向键是否应切换列（文本光标不在中间时留在当前格编辑）
 * @param event 键盘事件
 * @param direction 左或右
 * @returns 是否切换列
 */
export function shouldNavigateHorizontalOnArrowKey(
  event: KeyboardEvent,
  direction: 'left' | 'right',
): boolean {
  const target = event.target
  if (!(target instanceof HTMLInputElement || target instanceof HTMLTextAreaElement)) {
    return true
  }
  const { selectionStart, selectionEnd, value } = target
  if (selectionStart == null || selectionEnd == null) {
    return true
  }
  if (selectionStart !== selectionEnd) {
    return true
  }
  if (direction === 'left' && selectionStart > 0) {
    return false
  }
  if (direction === 'right' && selectionEnd < value.length) {
    return false
  }
  return true
}

/**
 * 回车是否应切换单元格（Excel：Enter 下一行；多行 textarea 保留换行）
 * @param event 键盘事件
 * @param column 列配置
 * @returns 是否导航
 */
export function shouldNavigateOnEnterKey(
  event: KeyboardEvent,
  column?: TaktEditableTableColumn,
): boolean {
  if (event.key !== 'Enter' || event.isComposing) {
    return false
  }
  if (column?.editor === 'textarea' && (column.rows ?? 1) > 1) {
    return false
  }
  return true
}

/**
 * 将焦点移到指定可编辑单元格内的输入控件
 * @param container 表格根元素
 * @param rowIndex 行索引
 * @param columnKey 列 key
 * @returns 是否成功聚焦
 */
export function focusEditableTableCell(
  container: HTMLElement | null | undefined,
  rowIndex: number,
  columnKey: string,
): boolean {
  if (!container) {
    return false
  }
  const coord = formatEditableCellCoord(rowIndex, columnKey)
  const cell = container.querySelector(
    `[${TAKT_EDITABLE_CELL_ATTR}="${coord}"]`,
  ) as HTMLElement | null
  if (!cell) {
    return false
  }
  const focusable =
    cell.querySelector<HTMLElement>('.ant-select-selection-search-input')
    ?? cell.querySelector<HTMLElement>('input.ant-input-number-input')
    ?? cell.querySelector<HTMLElement>('input:not([disabled]):not([type="hidden"])')
    ?? cell.querySelector<HTMLElement>('textarea:not([disabled])')
  if (!focusable) {
    return false
  }
  focusable.focus()
  if (focusable instanceof HTMLInputElement || focusable instanceof HTMLTextAreaElement) {
    focusable.select()
  }
  return true
}

/**
 * 上下方向键是否应切换行（InputNumber 内保留增减）
 * @param event 键盘事件
 * @returns 是否切换行
 */
export function shouldNavigateVerticalOnArrowKey(event: KeyboardEvent): boolean {
  const target = event.target
  if (target instanceof HTMLInputElement && target.classList.contains('ant-input-number-input')) {
    return false
  }
  return true
}

/**
 * 构建单元格导航 DOM 绑定（data 坐标 + keydown）
 * @param rowIndex 行索引
 * @param columnKey 列 key
 * @param onKeydown 键盘处理
 * @returns Vue v-bind 对象
 */
export function buildEditableCellNavAttrs(
  rowIndex: number,
  columnKey: string,
  onKeydown: (event: KeyboardEvent) => void,
): Record<string, unknown> {
  return {
    [TAKT_EDITABLE_CELL_ATTR]: formatEditableCellCoord(rowIndex, columnKey),
    onKeydown,
  }
}
