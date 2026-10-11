// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/composables
// 文件名称：useSourceEcDetailFields.ts
// 功能描述：设变来源子表 sourceEcDetail 业务字段元数据（与 source-ec-detail.d.ts / TaktSourceEcDetail 对齐）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue'
import TaktDictTag from '@/components/common/TaktDictTag/index.vue'
import type { TaktEditableEditorType, TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import type { SourceEcDetail } from '@/types/logistics/manufacturing/engineering-change/sourceEcDetail.types'

/** 子表主键列 key */
export const SOURCE_EC_DETAIL_ID_COLUMN_KEY = 'sourceEcDetailId'

/** 子表操作列 key */
export const SOURCE_EC_DETAIL_ACTION_COLUMN_KEY = 'action'

/** 设变来源子表业务字段（与 SourceEcDetail 实体字段一致，不含 id / 外键 / 导航） */
export const SOURCE_EC_DETAIL_BUSINESS_FIELD_KEYS = [
  'sourceRootMaterialCode',
  'sourceParentMaterialCode',
  'sourceOldMaterialCode',
  'sourceOldMaterialDescription',
  'sourceOldUsageQuantity',
  'sourceOldItemPosition',
  'sourceNewMaterialCode',
  'sourceNewMaterialDescription',
  'sourceNewUsageQuantity',
  'sourceNewItemPosition',
  'sourceBomCode',
  'SourceCompatibility',
  'source2ndVendor',
  'SourceInstruction',
  'sourceOldPartDisposition',
  'sourceBomEffectiveDate',
] as const

export type SourceEcDetailBusinessFieldKey = (typeof SOURCE_EC_DETAIL_BUSINESS_FIELD_KEYS)[number]

/** 旧物料处理字典类型 */
export const SOURCE_EC_OLD_PART_DISPOSITION_DICT_TYPE = 'logistics_manufacturing_ec_old_part_disposition'

/** 安排指示字典类型 */
export const SOURCE_EC_SOURCE_INSTRUCTION_DICT_TYPE = 'logistics_manufacturing_ec_source_instruction'

/** 第二供应商字典类型 */
export const SOURCE_EC_2ND_VENDOR_DICT_TYPE = 'logistics_manufacturing_ec_2nd_vendor'

type TranslateFn = (key: string, ...args: unknown[]) => string

interface SourceEcDetailFieldMeta {
  key: SourceEcDetailBusinessFieldKey
  editor: TaktEditableEditorType
  width: number
  required?: boolean
  maxLength?: number
  valueFormat?: string
}

/** 子表业务字段 UI 元数据（列宽 / 编辑器 / 长度与后端实体一致） */
export const SOURCE_EC_DETAIL_FIELD_META: readonly SourceEcDetailFieldMeta[] = [
  { key: 'sourceRootMaterialCode', editor: 'input', width: 120, required: true, maxLength: 20 },
  { key: 'sourceParentMaterialCode', editor: 'input', width: 120, required: true, maxLength: 20 },
  { key: 'sourceOldMaterialCode', editor: 'input', width: 120, maxLength: 20 },
  { key: 'sourceOldMaterialDescription', editor: 'input', width: 120, maxLength: 40 },
  { key: 'sourceOldUsageQuantity', editor: 'inputNumber', width: 120 },
  { key: 'sourceOldItemPosition', editor: 'input', width: 120, maxLength: 40 },
  { key: 'sourceNewMaterialCode', editor: 'input', width: 120, maxLength: 20 },
  { key: 'sourceNewMaterialDescription', editor: 'input', width: 120, maxLength: 40 },
  { key: 'sourceNewUsageQuantity', editor: 'inputNumber', width: 120 },
  { key: 'sourceNewItemPosition', editor: 'input', width: 140, maxLength: 40 },
  { key: 'sourceBomCode', editor: 'input', width: 100, maxLength: 4 },
  { key: 'SourceCompatibility', editor: 'input', width: 100, maxLength: 4 },
  { key: 'source2ndVendor', editor: 'input', width: 100, maxLength: 4 },
  { key: 'SourceInstruction', editor: 'input', width: 120, maxLength: 4 },
  { key: 'sourceOldPartDisposition', editor: 'input', width: 120, maxLength: 4 },
  { key: 'sourceBomEffectiveDate', editor: 'datePicker', width: 120, valueFormat: 'YYYY-MM-DD' },
]

/**
 * 实体字段 i18n 键（entity.sourceecdetail.*）
 * @param field 业务字段名
 */
export function sourceEcDetailEntityI18nKey(field: SourceEcDetailBusinessFieldKey | string): string {
  return `entity.sourceecdetail.${String(field).toLowerCase()}`
}

/**
 * 子表列表默认可见列（含 id、全部业务列、操作列；避免 TaktSingleTable 默认仅展示 8 列）
 */
export function buildSourceEcDetailDefaultVisibleColumnKeys(): string[] {
  return [
    SOURCE_EC_DETAIL_ID_COLUMN_KEY,
    ...SOURCE_EC_DETAIL_BUSINESS_FIELD_KEYS,
    SOURCE_EC_DETAIL_ACTION_COLUMN_KEY,
  ]
}

/**
 * 日期列排序（当前页 dataSource，与 user/index.vue 列 sorter 一致）
 * @param a 行 A 字段值
 * @param b 行 B 字段值
 */
function compareSourceEcDetailDateField(a: unknown, b: unknown): number {
  return new Date(String(a ?? 0)).getTime() - new Date(String(b ?? 0)).getTime()
}

/**
 * 构建子表 TaktSingleTable 业务列（不含 id / action）
 * @param t i18n
 * @param getField 读取行字段
 */
export function buildSourceEcDetailListBusinessColumns(
  t: TranslateFn,
  getField: (record: SourceEcDetail, field: string) => unknown,
): TableColumnsType {
  return SOURCE_EC_DETAIL_FIELD_META.map((meta) => {
    const column: TableColumnsType[number] = {
      title: t(sourceEcDetailEntityI18nKey(meta.key)),
      dataIndex: meta.key,
      key: meta.key,
      width: meta.width,
      resizable: true,
      ellipsis: true,
    }
    if (meta.key === 'sourceBomEffectiveDate') {
      column.sorter = (a: SourceEcDetail, b: SourceEcDetail) =>
        compareSourceEcDetailDateField(getField(a, meta.key), getField(b, meta.key))
    }
    if (meta.key === 'sourceOldPartDisposition') {
      column.customRender = ({ record }: { record: SourceEcDetail }) =>
        h(TaktDictTag, {
          dictType: SOURCE_EC_OLD_PART_DISPOSITION_DICT_TYPE,
          value: getField(record, meta.key),
        })
    } else if (meta.key === 'SourceInstruction') {
      column.customRender = ({ record }: { record: SourceEcDetail }) =>
        h(TaktDictTag, {
          dictType: SOURCE_EC_SOURCE_INSTRUCTION_DICT_TYPE,
          value: getField(record, meta.key),
        })
    } else if (meta.key === 'source2ndVendor') {
      column.customRender = ({ record }: { record: SourceEcDetail }) =>
        h(TaktDictTag, {
          dictType: SOURCE_EC_2ND_VENDOR_DICT_TYPE,
          value: getField(record, meta.key),
        })
    } else {
      column.customRender = ({ record }: { record: SourceEcDetail }) =>
        String(getField(record, meta.key) ?? '')
    }
    return column
  })
}

/**
 * 构建主子表详情内嵌 TaktEditableTable 列
 * @param t i18n
 * @param readOnly 详情只读时禁用行内编辑
 */
export function buildSourceEcDetailEditableColumns(
  t: TranslateFn,
  readOnly = false,
): TaktEditableTableColumn[] {
  return SOURCE_EC_DETAIL_FIELD_META.map((meta) => {
    const label = t(sourceEcDetailEntityI18nKey(meta.key))
    const placeholderKey = meta.editor === 'datePicker'
      ? 'common.page.form.placeholder.select'
      : meta.required
        ? 'common.page.form.placeholder.required'
        : 'common.page.form.placeholder.optional'
    const col: TaktEditableTableColumn = {
      key: meta.key,
      title: label,
      editor: readOnly ? 'readonly' : meta.editor,
      width: meta.width,
      allowClear: !meta.required,
      placeholder: t(placeholderKey, { field: label }),
    }
    if (meta.valueFormat) {
      col.valueFormat = meta.valueFormat
    }
    return col
  })
}

/**
 * 子表空行默认值（与 CreateDto 字段对齐）
 */
export function createEmptySourceEcDetailRow(): Record<string, unknown> {
  return {
    sourceRootMaterialCode: '',
    sourceParentMaterialCode: '',
    sourceOldMaterialCode: '',
    sourceOldMaterialDescription: '',
    sourceOldUsageQuantity: undefined,
    sourceOldItemPosition: '',
    sourceNewMaterialCode: '',
    sourceNewMaterialDescription: '',
    sourceNewUsageQuantity: undefined,
    sourceNewItemPosition: '',
    sourceBomCode: '',
    SourceCompatibility: '',
    source2ndVendor: '',
    SourceInstruction: '',
    sourceOldPartDisposition: '',
    sourceBomEffectiveDate: '',
  }
}
