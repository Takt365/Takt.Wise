// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktImportResult.ts
// 创建时间：2026-06-26
// 创建人：Takt365(Cursor AI)
// 功能描述：统一解析后端导入 API 返回（SuccessCount/success 等字段归一化）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 导入结果（与 TaktImportFile 及后端 Import 接口对齐） */
export interface TaktImportResult {
  /** 成功导入条数 */
  success: number
  /** 失败总条数（含重复与其他错误） */
  fail: number
  /** 重复记录条数 */
  duplicate: number
  /** 非重复类错误条数 */
  error: number
  /** 逐行错误明细 */
  errors: string[]
}

/**
 * 从对象中读取非负整数（兼容 camelCase / PascalCase 字段名）
 * @param obj 响应对象
 * @param keys 候选字段名（按优先级）
 * @returns 解析到的整数，缺省为 0
 */
function readNonNegativeInt(obj: Record<string, unknown>, ...keys: string[]): number {
  for (const key of keys) {
    const value = obj[key]
    if (typeof value === 'number' && Number.isFinite(value)) {
      return Math.max(0, Math.trunc(value))
    }
  }
  return 0
}

/**
 * 判断单行导入错误是否属于重复类（Excel 内重复或库内已存在）
 * @param line 错误明细行
 * @returns 是否为重复类错误
 */
export function isDuplicateImportErrorLine(line: string): boolean {
  if (!line?.trim()) {
    return false
  }
  return (
    line.includes('重复')
    || line.includes('已存在')
    || /duplicate/i.test(line)
    || /already exists/i.test(line)
  )
}

/**
 * 将后端 Import 响应归一化为 TaktImportResult
 * @param raw 解包后的 API data（可为 success/fail 或 successCount/failCount）
 * @returns 归一化导入结果
 */
export function normalizeImportResult(raw: unknown): TaktImportResult {
  if (!raw || typeof raw !== 'object') {
    return { success: 0, fail: 0, duplicate: 0, error: 0, errors: [] }
  }
  const obj = raw as Record<string, unknown>
  const success = readNonNegativeInt(obj, 'success', 'successCount', 'SuccessCount')
  let fail = readNonNegativeInt(obj, 'fail', 'failCount', 'FailCount')
  let duplicate = readNonNegativeInt(obj, 'duplicate', 'duplicateCount', 'DuplicateCount')
  let error = readNonNegativeInt(obj, 'error', 'errorCount', 'ErrorCount')
  const errorsRaw = obj.errors ?? obj.Errors
  const errors = Array.isArray(errorsRaw)
    ? errorsRaw.filter((item): item is string => typeof item === 'string')
    : []
  if (duplicate === 0 && errors.length > 0) {
    duplicate = errors.filter(isDuplicateImportErrorLine).length
  }
  if (error === 0 && fail > 0) {
    error = Math.max(0, fail - duplicate)
  } else if (error === 0 && errors.length > duplicate) {
    error = errors.length - duplicate
  }
  if (fail === 0 && errors.length > 0 && success === 0) {
    fail = errors.length
    if (duplicate === 0) {
      duplicate = errors.filter(isDuplicateImportErrorLine).length
    }
    if (error === 0) {
      error = Math.max(0, fail - duplicate)
    }
  } else if (fail === 0 && duplicate + error > 0) {
    fail = duplicate + error
  }
  return { success, fail, duplicate, error, errors }
}
