// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputDetailFormColumns.ts
// 功能描述：组立日报主表弹窗内嵌子表 TaktEditableTable 列配置（与 ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import {
  ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS,
  useAssyOutputDetailI18n,
} from './useAssyOutputDetailI18n'

/**
 * 由 assy-output-form 模板 #cell-{key} 渲染的列：columns 只负责 title/width，不写 editor
 * （TaktEditableTable 优先使用插槽；此处禁止再配 input/readonly，避免与 slot 双轨）
 */
export const ASSYOUTPUTDETAIL_SLOT_CELL_FIELDS = [
  'downtimeReason',
  'unachievedReason',
] as const satisfies readonly (typeof ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS)[number][]

type EmbeddedField = (typeof ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS)[number]
type SlotCellField = (typeof ASSYOUTPUTDETAIL_SLOT_CELL_FIELDS)[number]

/** 子表数值列（0 合法；须为有效数字） */
function requiredDetailNumberValidator(message: string) {
  return (value: unknown): string | void => {
    if (value === undefined || value === null || value === '') {
      return message
    }
    const num = typeof value === 'number' ? value : Number(value)
    if (!Number.isFinite(num)) {
      return message
    }
  }
}

/** 内置编辑器列（无 slot 时由 TaktEditableTable 渲染 inputNumber / textarea / readonly） */
const BUILTIN_EDITOR_COLUMNS: Partial<Record<EmbeddedField, TaktEditableTableColumn>> = {
  timePeriod: { key: 'timePeriod', title: '', editor: 'readonly', width: 120, required: true },
  stdCapacity: { key: 'stdCapacity', title: '', editor: 'readonly', width: 120, summary: 'sum' },
  lineNumber: { key: 'lineNumber', title: '', editor: 'readonly', width: 80, required: true },
  prodActualQty: { key: 'prodActualQty', title: '', editor: 'inputNumber', width: 120, summary: 'sum', required: true },
  downtimeMinutes: { key: 'downtimeMinutes', title: '', editor: 'inputNumber', width: 120, summary: 'sum', required: true },
  downtimeDescription: {
    key: 'downtimeDescription',
    title: '',
    editor: 'textarea',
    rows: 1,
    width: 200,
  },
  unachievedDescription: {
    key: 'unachievedDescription',
    title: '',
    editor: 'textarea',
    rows: 1,
    width: 200,
  },
  confirmMinutes: { key: 'confirmMinutes', title: '', editor: 'inputNumber', width: 120, summary: 'sum', required: true },
}

/** #cell-{key} 列：仅布局元数据（停线/未达成原因可空，与 TaktAssyOutputDetail 实体对齐） */
const SLOT_CELL_COLUMNS: Record<SlotCellField, TaktEditableTableColumn> = {
  downtimeReason: { key: 'downtimeReason', title: '', width: 300 },
  unachievedReason: { key: 'unachievedReason', title: '', width: 300 },
}

/**
 * 组立日报内嵌子表列（assy-output-form TaktEditableTable :columns）
 */
export function useAssyOutputDetailFormColumns() {
  const pi = useAssyOutputDetailI18n()

  return computed<TaktEditableTableColumn[]>(() =>
    ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS.map((field) => {
      const title = pi.label(field)
      if ((ASSYOUTPUTDETAIL_SLOT_CELL_FIELDS as readonly string[]).includes(field)) {
        const base = SLOT_CELL_COLUMNS[field as SlotCellField]
        return {
          ...base,
          title,
        }
      }
      const base = BUILTIN_EDITOR_COLUMNS[field]
      if (!base) {
        return { key: field, title, editor: 'readonly', width: 120 }
      }
      const column: TaktEditableTableColumn = { ...base, key: field, title }
      if (field === 'stdCapacity') {
        column.titleHint = pi.stdCapacityHint()
      }
      if (field === 'confirmMinutes') {
        column.titleHint = pi.confirmMinutesHint()
      }
      if (field === 'downtimeDescription' || field === 'unachievedDescription') {
        column.placeholder = pi.ph(field)
      }
      if (
        field === 'prodActualQty'
        || field === 'downtimeMinutes'
        || field === 'confirmMinutes'
        || field === 'lineNumber'
      ) {
        column.validator = requiredDetailNumberValidator(pi.ph(field))
      }
      if (field === 'stdCapacity') {
        column.required = false
      }
      return column
    }),
  )
}
