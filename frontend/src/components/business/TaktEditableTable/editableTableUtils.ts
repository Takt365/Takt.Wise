// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/components/business/TaktEditableTable
// 文件名称：editableTableUtils.ts
// 功能描述：可编辑子表行校验与列汇总纯函数
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TAKT_EDITABLE_ROW_KEY, type TaktEditableRow, type TaktEditableTableColumn, filterActiveEditableRows } from './types'

/** 可编辑子表校验错误项 */
export interface TaktEditableValidateError {
  /** 行索引（0-based） */
  rowIndex: number
  /** 行 row-key */
  rowKey: string
  /** 绑定字段名 */
  field: string
  /** 列 key */
  columnKey: string
  /** 错误文案 */
  message: string
}

/** validate 选项 */
export interface TaktEditableValidateOptions {
  /** 最少行数，默认 0 */
  minRows?: number
  /** 无行时是否跳过列级校验，默认 true */
  skipColumnValidateWhenEmpty?: boolean
  /** minRows 失败文案 */
  minRowsMessage?: string
  /** 行号 + 字段名文案工厂（必填等） */
  rowFieldLabel?: (rowIndex: number, column: TaktEditableTableColumn) => string
  /** 唯一性冲突文案工厂 */
  uniqueMessage?: (rowIndex: number, column: TaktEditableTableColumn) => string
  /** 作废字段名；设置后校验/最少行数仅统计未作废行 */
  obsoleteField?: string
  /** 作废字段取值，默认 1 */
  obsoleteValue?: number | string
}

/**
 * 判断单元格值是否为空
 * @param value 单元格值
 * @returns {boolean} 是否为空
 */
export function isEditableCellEmpty(value: unknown): boolean {
  if (value == null) {
    return true
  }
  if (typeof value === 'string') {
    return value.trim() === ''
  }
  return false
}

/**
 * 解析列绑定字段
 * @param column 列配置
 * @returns {string} dataIndex
 */
export function resolveEditableDataIndex(column: TaktEditableTableColumn): string {
  return column.dataIndex ?? column.key
}

/**
 * 解析行 row-key
 * @param row 行数据
 * @param idField 持久化主键字段
 * @returns {string} row-key
 */
export function resolveEditableRowKey(row: TaktEditableRow, idField?: string): string {
  const existingKey = row[TAKT_EDITABLE_ROW_KEY]
  if (typeof existingKey === 'string' && existingKey) {
    return existingKey
  }
  if (idField) {
    const idValue = row[idField]
    if (idValue != null && String(idValue)) {
      return String(idValue)
    }
  }
  return ''
}

/**
 * 将值转为可汇总数字
 * @param value 单元格值
 * @returns {number | null} 数字或 null
 */
function toSummaryNumber(value: unknown): number | null {
  if (value == null || value === '') {
    return null
  }
  const num = typeof value === 'number' ? value : Number(value)
  return Number.isFinite(num) ? num : null
}

/**
 * 格式化汇总展示值
 * @param value 汇总结果
 * @param precision 小数位
 * @returns {string} 展示文案
 */
export function formatSummaryValue(value: unknown, precision = 2): string {
  if (value == null || value === '') {
    return ''
  }
  if (typeof value === 'number' && Number.isFinite(value)) {
    if (Number.isInteger(value)) {
      return String(value)
    }
    return value.toFixed(precision)
  }
  return String(value)
}

/**
 * 计算单列汇总值
 * @param rows 行数组
 * @param column 列配置
 * @returns {unknown} 汇总结果
 */
export function computeEditableColumnSummary(
  rows: readonly TaktEditableRow[],
  column: TaktEditableTableColumn,
): unknown {
  if (!column.summary) {
    return ''
  }
  if (typeof column.summary === 'function') {
    return column.summary(rows)
  }
  const dataIndex = resolveEditableDataIndex(column)
  if (column.summary === 'count') {
    return rows.length
  }
  const numbers = rows
    .map((row) => toSummaryNumber(row[dataIndex]))
    .filter((item): item is number => item != null)
  if (column.summary === 'sum') {
    return numbers.reduce((acc, cur) => acc + cur, 0)
  }
  if (column.summary === 'avg') {
    if (!numbers.length) {
      return ''
    }
    return numbers.reduce((acc, cur) => acc + cur, 0) / numbers.length
  }
  if (column.summary === 'max') {
    return numbers.length ? Math.max(...numbers) : ''
  }
  if (column.summary === 'min') {
    return numbers.length ? Math.min(...numbers) : ''
  }
  return ''
}

/**
 * 计算各列汇总值映射
 * @param rows 行数组
 * @param columns 列配置
 * @returns {Record<string, unknown>} key → 汇总值
 */
export function computeEditableSummaryMap(
  rows: readonly TaktEditableRow[],
  columns: readonly TaktEditableTableColumn[],
): Record<string, unknown> {
  const result: Record<string, unknown> = {}
  for (const column of columns) {
    if (column.summary) {
      result[column.key] = computeEditableColumnSummary(rows, column)
    }
  }
  return result
}

/**
 * 校验可编辑子表行
 * @param rows 行数组
 * @param columns 列配置
 * @param options 校验选项
 * @returns {Promise<TaktEditableValidateError[]>} 错误列表
 */
export async function validateEditableRows(
  rows: readonly TaktEditableRow[],
  columns: readonly TaktEditableTableColumn[],
  options: TaktEditableValidateOptions = {},
): Promise<TaktEditableValidateError[]> {
  const minRows = options.minRows ?? 0
  const skipColumnValidateWhenEmpty = options.skipColumnValidateWhenEmpty !== false
  const obsoleteValue = options.obsoleteValue ?? 1
  const activeRows = filterActiveEditableRows(rows, options.obsoleteField, obsoleteValue)
  const errors: TaktEditableValidateError[] = []
  if (activeRows.length < minRows) {
    errors.push({
      rowIndex: -1,
      rowKey: '',
      field: '',
      columnKey: '',
      message: options.minRowsMessage ?? '',
    })
    return errors
  }
  if (!activeRows.length && skipColumnValidateWhenEmpty) {
    return errors
  }
  for (let rowIndex = 0; rowIndex < rows.length; rowIndex++) {
    const row = rows[rowIndex]
    if (!row) {
      continue
    }
    if (options.obsoleteField && filterActiveEditableRows([row], options.obsoleteField, obsoleteValue).length === 0) {
      continue
    }
    const rowKey = resolveEditableRowKey(row)
    for (const column of columns) {
      const dataIndex = resolveEditableDataIndex(column)
      const value = row[dataIndex]
      if (column.required && isEditableCellEmpty(value)) {
        const fieldLabel = options.rowFieldLabel?.(rowIndex, column) ?? column.title
        errors.push({
          rowIndex,
          rowKey,
          field: dataIndex,
          columnKey: column.key,
          message: fieldLabel,
        })
        continue
      }
      if (column.validator) {
        const message = await column.validator(value, row, rowIndex)
        if (message) {
          errors.push({
            rowIndex,
            rowKey,
            field: dataIndex,
            columnKey: column.key,
            message,
          })
        }
      }
      if (column.unique) {
        const normalized = value == null ? '' : String(value).trim()
        if (normalized) {
          const duplicated = rows.some((other, otherIndex) => {
            if (otherIndex === rowIndex || !other) {
              return false
            }
            if (options.obsoleteField && filterActiveEditableRows([other], options.obsoleteField, obsoleteValue).length === 0) {
              return false
            }
            const otherValue = other[dataIndex]
            return otherValue != null && String(otherValue).trim() === normalized
          })
          if (duplicated) {
            errors.push({
              rowIndex,
              rowKey,
              field: dataIndex,
              columnKey: column.key,
              message:
                options.uniqueMessage?.(rowIndex, column)
                ?? options.rowFieldLabel?.(rowIndex, column)
                ?? column.title,
            })
          }
        }
      }
    }
  }
  return errors
}

/**
 * 将校验错误列表转为 cellErrors 映射
 * @param errors 错误列表
 * @returns {Record<string, string>} `${rowKey}:${field}` → message
 */
export function mapEditableErrorsToCells(errors: readonly TaktEditableValidateError[]): Record<string, string> {
  const result: Record<string, string> = {}
  for (const item of errors) {
    if (item.rowIndex < 0 || !item.rowKey || !item.field) {
      continue
    }
    result[`${item.rowKey}:${item.field}`] = item.message
  }
  return result
}
