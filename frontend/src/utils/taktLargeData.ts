// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktLargeData
// 文件名称：taktLargeData.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：前端大数据自动处理阈值（下拉虚拟+远程、表格虚拟）；❌ 不是业务数据条数上限、禁止据此截断数据
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 大数据自动处理阈值（条）
 * - TaktSelect：达到后自动 virtual + remote-search（apiUrl）
 * - takt-*-table：达到后即使页面传 virtual=false 也强制 virtual
 * - 左树/右树：达到后自动走 parentId 一层懒加载（禁止一次拉全量树）
 * ❌ 禁止用本常量对 options / list 做 Take / slice 截断
 */
export const TAKT_LARGE_DATA_AUTO_THRESHOLD = 500

/**
 * 树表是否走大数据懒加载（左 tree?parentId= 一层 + 右 list/展开再拉）
 * @param totalCount 当前隔离范围内记录总数（来自 list.total，不是本页行数）
 * @returns {boolean} 达到阈值则必须懒加载
 */
export function shouldUseTreeLazyLoad(totalCount: number): boolean {
  const n = Number.isFinite(totalCount) ? Math.floor(totalCount) : 0
  return n >= TAKT_LARGE_DATA_AUTO_THRESHOLD
}
