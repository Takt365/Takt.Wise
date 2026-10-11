// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/components/business/TaktEditableTable
// 文件名称：types.ts
// 功能描述：TaktEditableTable 列配置与行 key 工具（主子表表单内嵌可编辑子表）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 行内临时 row-key 字段名（提交前须剥离） */
export const TAKT_EDITABLE_ROW_KEY = '__rowKey'

/** 可编辑子表行（含可选 __rowKey） */
export type TaktEditableRow = Record<string, unknown> & {
  __rowKey?: string
}

/** 内置编辑器类型 */
export type TaktEditableEditorType =
  | 'input'
  | 'inputNumber'
  | 'textarea'
  | 'datePicker'
  | 'readonly'

/** 列汇总类型 */
export type TaktEditableSummaryType = 'sum' | 'count' | 'avg' | 'max' | 'min'

/** 可编辑子表列配置 */
export interface TaktEditableTableColumn {
  /** 列 key，与 dataIndex 默认一致 */
  key: string
  /** 列标题（已翻译文案） */
  title: string
  /** 列标题旁问号提示（可选） */
  titleHint?: string
  /** 绑定字段名，默认同 key */
  dataIndex?: string
  /** 列宽 */
  width?: number
  /** 固定列 */
  fixed?: 'left' | 'right'
  /** 内置编辑器；省略时优先 #cell-{key} 插槽，否则只读展示 */
  editor?: TaktEditableEditorType
  /** 占位符；省略时由组件按 editor 类型推断 required/select */
  placeholder?: string
  /** inputNumber 最小值 */
  min?: number
  /** textarea 行数 */
  rows?: number
  /** datePicker value-format */
  valueFormat?: string
  /** datePicker 是否含时间 */
  showTime?: boolean
  /** input 是否 allow-clear */
  allowClear?: boolean
  /** 只读展示（等同 editor: readonly） */
  readonly?: boolean
  /** 列必填（validate 时校验） */
  required?: boolean
  /** 列值唯一（validate 时校验） */
  unique?: boolean
  /** 自定义校验，返回错误文案 */
  validator?: (
    value: unknown,
    row: TaktEditableRow,
    index: number,
  ) => string | void | Promise<string | void>
  /** 汇总：内置类型或自定义函数 */
  summary?: TaktEditableSummaryType | ((rows: readonly TaktEditableRow[]) => unknown)
  /** sum/avg 小数位，默认 2 */
  summaryPrecision?: number
}

/**
 * 判断子表行是否已作废
 * @param row 行数据
 * @param obsoleteField 作废字段名（如 isObsolete）
 * @param obsoleteValue 作废取值，默认 1
 * @returns {boolean} 是否作废
 */
export function isEditableRowObsolete(
  row: Record<string, unknown>,
  obsoleteField: string,
  obsoleteValue: number | string = 1,
): boolean {
  if (!obsoleteField) {
    return false
  }
  return Number(row[obsoleteField]) === Number(obsoleteValue)
}

/**
 * 过滤未作废子表行（汇总/校验用）
 * @param rows 行数组
 * @param obsoleteField 作废字段名
 * @param obsoleteValue 作废取值
 * @returns {TaktEditableRow[]} 未作废行
 */
export function filterActiveEditableRows(
  rows: readonly TaktEditableRow[],
  obsoleteField?: string,
  obsoleteValue: number | string = 1,
): TaktEditableRow[] {
  if (!obsoleteField) {
    return [...rows]
  }
  return rows.filter((row) => !isEditableRowObsolete(row, obsoleteField, obsoleteValue))
}

/**
 * 生成客户端临时 row-key
 * @param prefix 前缀，默认 new
 * @returns {string} 唯一 row-key
 */
export function createEditableRowKey(prefix = 'new'): string {
  return `${prefix}-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`
}

/**
 * 为子表行附加 __rowKey（编辑态灌入 / 新增行）
 * @param rows 业务行数组
 * @param idField 持久化主键字段名（有值时用作 row-key）
 * @returns {TaktEditableRow[]} 带 __rowKey 的行
 */
export function attachEditableRowKeys(
  rows: readonly Record<string, unknown>[] | undefined | null,
  idField?: string,
): TaktEditableRow[] {
  if (!rows?.length) {
    return []
  }
  return rows.map((item, index) => {
    const existingKey = item[TAKT_EDITABLE_ROW_KEY]
    const idKey = idField ? item[idField] : undefined
    const rowKey =
      (typeof existingKey === 'string' && existingKey) ||
      (idKey != null && String(idKey)) ||
      createEditableRowKey(`new-${index}`)
    return {
      ...item,
      [TAKT_EDITABLE_ROW_KEY]: rowKey,
    }
  })
}

/**
 * 剥离 __rowKey，得到可提交 DTO 行
 * @param rows 带子表 row-key 的行
 * @returns {Record<string, unknown>[]} 纯业务字段行
 */
export function detachEditableRowKeys(rows: readonly Record<string, unknown>[]): Record<string, unknown>[] {
  return rows.map((row) => {
    const { [TAKT_EDITABLE_ROW_KEY]: _rowKey, ...rest } = row
    return rest
  })
}
