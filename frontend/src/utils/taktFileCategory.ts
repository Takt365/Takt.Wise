// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFileCategory.ts
// 功能描述：文件分类展示与筛选（与后端 TaktFileHelper.GetFileCategoryFromMimeType 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 文件分类取值（0=文档…5=其他，由 FileType/MIME 自动推断） */
export const TAKT_FILE_CATEGORY_VALUES = [0, 1, 2, 3, 4, 5] as const

export type TaktFileCategoryValue = (typeof TAKT_FILE_CATEGORY_VALUES)[number]

/**
 * 文件分类 entity 翻译键
 * @param value 分类值 0~5
 * @returns i18n 键 entity.file.category.{value}
 */
export function taktFileCategoryI18nKey(value: number): string {
  if (!Number.isFinite(value)) {
    return 'entity.file.category.5'
  }
  const normalized = Math.trunc(value)
  if (normalized < 0 || normalized > 5) {
    return 'entity.file.category.5'
  }
  return `entity.file.category.${normalized}`
}

/**
 * 解析表格/查询中的分类值
 * @param raw 原始值
 * @returns 0~5 或 undefined
 */
export function parseTaktFileCategoryValue(raw: unknown): number | undefined {
  if (raw == null || raw === '') {
    return undefined
  }
  const num = typeof raw === 'number' ? raw : Number(raw)
  if (!Number.isFinite(num)) {
    return undefined
  }
  const normalized = Math.trunc(num)
  if (normalized < 0 || normalized > 5) {
    return undefined
  }
  return normalized
}
