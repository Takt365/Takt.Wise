// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect/composables
// 文件名称：useAssyDefectDetailEditableDict.ts
// 功能描述：内嵌子表不良区分/个所/修理员：行内清洗、TaktSelect 绑定、提交写回 Label
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useAssyDefectDetailDictFormat } from './useAssyDefectDetailDictFormat'

/** 内嵌子表字典/选项字段 */
export type AssyDefectDetailDictField = 'defectCategory' | 'defectLocation' | 'repairOperator'

/**
 * 内嵌子表字典/修理员行内编辑（TaktEditableTable #cell-defectCategory 等）
 */
export function useAssyDefectDetailEditableDict() {
  const {
    loadEmployeeOptionsAsync,
    parseDefectCategoryForSelect,
    parseDefectLocationForSelect,
    parseRepairOperatorForSelect,
    formatDefectCategoryForStorage,
    formatDefectLocationForStorage,
    formatRepairOperatorForStorage,
  } = useAssyDefectDetailDictFormat()

  /**
   * 解析并对齐 Select 绑定值；未选为 undefined
   * @param raw 行内原始值
   * @param field 字段名
   */
  function sanitizeDetailDictSelectValue(
    raw: unknown,
    field: AssyDefectDetailDictField,
  ): string | number | undefined {
    if (raw == null || raw === '') {
      return undefined
    }
    if (typeof raw === 'number' && raw === 0) {
      return undefined
    }
    const parseFn = field === 'defectCategory'
      ? parseDefectCategoryForSelect
      : field === 'defectLocation'
        ? parseDefectLocationForSelect
        : parseRepairOperatorForSelect
    return parseFn(raw)
  }

  /** 子表行内字典/修理员字段规范（未选时 delete 字段） */
  function ensureDetailDictFields(row: Record<string, unknown>) {
    const fields: AssyDefectDetailDictField[] = ['defectCategory', 'defectLocation', 'repairOperator']
    for (const field of fields) {
      const sanitized = sanitizeDetailDictSelectValue(row[field], field)
      if (sanitized === undefined) {
        delete row[field]
      } else {
        row[field] = sanitized
      }
    }
  }

  /** TaktSelect :model-value（只读计算） */
  function getDetailDictSelectModelValue(
    record: Record<string, unknown>,
    field: AssyDefectDetailDictField,
  ): string | number | undefined {
    return sanitizeDetailDictSelectValue(record[field], field)
  }

  /** TaktSelect @update:model-value 写回行内 */
  function applyDetailDictChange(
    record: Record<string, unknown>,
    field: AssyDefectDetailDictField,
    value: string | number | readonly (string | number)[] | null | undefined,
  ) {
    const raw = Array.isArray(value) ? value[0] : value
    const sanitized = sanitizeDetailDictSelectValue(raw, field)
    if (sanitized === undefined) {
      delete record[field]
    } else {
      record[field] = sanitized
    }
  }

  /** 提交前：Select 绑定值 → 库内 DictLabel / 员工姓名 */
  function normalizeAssyDefectDetailRowForSubmit(row: Record<string, unknown>) {
    return {
      ...row,
      defectCategory: formatDefectCategoryForStorage(row.defectCategory),
      defectLocation: formatDefectLocationForStorage(row.defectLocation),
      repairOperator: formatRepairOperatorForStorage(row.repairOperator),
    }
  }

  return {
    loadEmployeeOptionsAsync,
    ensureDetailDictFields,
    getDetailDictSelectModelValue,
    applyDetailDictChange,
    normalizeAssyDefectDetailRowForSubmit,
  }
}
