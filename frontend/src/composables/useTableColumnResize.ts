// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/composables/useTableColumnResize
// 文件名称：useTableColumnResize.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：表格列宽拖拽；stable 列引用 + reactive 就地改 width，拖动不重建列数组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue'
import type { ColumnType } from 'ant-design-vue/es/table/interface'
import { reactive, shallowRef, type ShallowRef } from 'vue'
import { applyTableColumnPresentation } from '@/utils/tableScroll'

/** 拖拽列宽下限（px） */
const MIN_RESIZE_COLUMN_WIDTH = 40

/**
 * 解析列唯一键（优先 key，其次 dataIndex）
 * @param column 列配置
 * @returns 键；无法识别时返回空串
 */
export function resolveTableColumnResizeKey(column: ColumnType<unknown>): string {
  if (column.key != null && String(column.key) !== '') {
    return String(column.key)
  }
  const dataIndex = column.dataIndex
  if (dataIndex == null) {
    return ''
  }
  return Array.isArray(dataIndex) ? dataIndex.join('.') : String(dataIndex)
}

/**
 * 表格列宽拖拽：
 * - 仅在列源变化时重建展示列
 * - 拖动时只改 reactive 列的 width（丝滑，不换数组）
 * @returns displayColumns / rebuildDisplayColumns / handleResizeColumn
 */
export function useTableColumnResize() {
  /** 非响应式覆盖缓存（跨 rebuild 保留拖拽宽度） */
  const widthOverrides = new Map<string, number>()

  /** 传给 a-table 的稳定列数组（元素为 reactive） */
  const displayColumns: ShallowRef<TableColumnsType> = shallowRef([])

  /**
   * 由列源重建展示列（应用 presentation + 已缓存宽度）
   * @param source 过滤后的业务列
   * @param defaultEllipsis 默认 ellipsis
   */
  function rebuildDisplayColumns(source: TableColumnsType, defaultEllipsis: boolean): void {
    const presented = applyTableColumnPresentation(source, defaultEllipsis)
    displayColumns.value = presented.map((column) => {
      const key = resolveTableColumnResizeKey(column as ColumnType<unknown>)
      const overrideWidth = key ? widthOverrides.get(key) : undefined
      const next = overrideWidth == null ? { ...column } : { ...column, width: overrideWidth }
      return reactive(next)
    }) as TableColumnsType
  }

  /**
   * a-table @resize-column：只改当前列 width，不替换列数组
   * @param width 新宽度
   * @param column 当前列（displayColumns 内 reactive 对象）
   * @returns 是否已处理
   */
  function handleResizeColumn(width: number, column: ColumnType<unknown>): boolean {
    const resizable = (column as ColumnType<unknown> & { resizable?: boolean }).resizable
    if (resizable !== true) {
      return false
    }
    const key = resolveTableColumnResizeKey(column)
    if (!key) {
      return false
    }
    const nextWidth = Math.max(MIN_RESIZE_COLUMN_WIDTH, Math.floor(width))
    widthOverrides.set(key, nextWidth)
    column.width = nextWidth
    return true
  }

  return {
    displayColumns,
    rebuildDisplayColumns,
    handleResizeColumn,
  }
}
