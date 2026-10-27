// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomExportFileName.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析/推移导出文件名：{工厂}_{标准名}_{选中条件}
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 清理导出文件名片段中的非法字符
 * @param {string} segment 原始片段
 * @returns {string} 安全片段
 */
function sanitizeExportSegment(segment: string): string {
  return segment.trim().replace(/[/\\?%*:|"<>]/g, '_');
}

/**
 * 标准导出名 + 选中条件（工厂置前）组成下载基名，与推移分析统一为 {Plant}_{报表名}_{其它}
 * @param {string} standardName 标准名，如「DTA BOM成本推移表」
 * @param {ReadonlyArray<string | undefined | null>} selecmedParts 选中信息；首段为工厂码，其后为机种/产品等
 * @returns {string} 基名（不含扩展名），如「C100_DTA BOM成本推移表_202MK7」
 */
export function buildBomExportBaseName(
  standardName: string,
  selecmedParts: ReadonlyArray<string | undefined | null>,
): string {
  const base = sanitizeExportSegment(standardName || '')
  if (!base) {
    return 'export'
  }
  const parts = selecmedParts
    .map((part) => (part == null ? '' : sanitizeExportSegment(String(part))))
    .filter((part) => part.length > 0)
  if (parts.length === 0) {
    return base
  }
  const [plant, ...rest] = parts
  return [plant, base, ...rest].join('_')
}

/**
 * 带 .xlsx 的完整导出文件名
 * @param {string} standardName 标准名
 * @param {ReadonlyArray<string | undefined | null>} selecmedParts 选中信息（首段工厂）
 * @returns {string} 如「C100_DTA BOM通用组件成本推移表_202MK7.xlsx」
 */
export function buildBomExportFileName(
  standardName: string,
  selecmedParts: ReadonlyArray<string | undefined | null>,
): string {
  return `${buildBomExportBaseName(standardName, selecmedParts)}.xlsx`
}
