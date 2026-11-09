// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/utils
// 文件名称：fetchAllEcKanbanMasters.ts
// 创建时间：2026-11-06
// 创建人：Takt365(Cursor AI)
// 功能描述：按筛选条件翻页拉全量设变看板主表行（供前端按明细行展开后再分页）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getEcKanbanList } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api'
import type {
  EcKanban,
  EcKanbanQuery,
} from '@/types/logistics/manufacturing/engineering-change/ecKanban.types'
import { ensureTaktPaginationConfigAsync, getTaktMaxPageSize } from '@/utils/taktPaged'

/** 拉取时单页上限保护（防止异常 total 死循环） */
const FETCH_ALL_MAX_PAGES = 500

/**
 * 按筛选条件拉取全部设变看板主表行（忽略调用方 pageIndex/pageSize）
 * @param filter 查询条件（关键字、期间、状态等）
 * @returns {Promise<EcKanban[]>} 主表行全集
 */
export async function fetchAllEcKanbanMasters(
  filter: Omit<EcKanbanQuery, 'pageIndex' | 'pageSize'> | EcKanbanQuery,
): Promise<EcKanban[]> {
  await ensureTaktPaginationConfigAsync()
  const pageSize = Math.max(1, getTaktMaxPageSize())
  const base: EcKanbanQuery = {
    ...filter,
    pageIndex: 1,
    pageSize,
  }
  const all: EcKanban[] = []
  let pageIndex = 1
  let total = Number.POSITIVE_INFINITY
  while (all.length < total && pageIndex <= FETCH_ALL_MAX_PAGES) {
    const res = await getEcKanbanList({
      ...base,
      pageIndex,
      pageSize,
    })
    total = Number.isFinite(res.total) ? Math.max(0, res.total) : 0
    const chunk = res.data ?? []
    if (chunk.length > 0) {
      all.push(...chunk)
    }
    if (chunk.length === 0 || chunk.length < pageSize || all.length >= total) {
      break
    }
    pageIndex += 1
  }
  return all
}

/**
 * 按明细行总数切当前页；页码越界时回退到末页
 * @param rows 全量明细行
 * @param pageIndex 当前页（1 起）
 * @param pageSize 每页条数
 * @returns 当前页切片与校正后的页码、总数
 */
export function sliceEcKanbanDetailPage<T>(
  rows: readonly T[],
  pageIndex: number,
  pageSize: number,
): { pageRows: T[]; pageIndex: number; total: number } {
  const size = Math.max(1, pageSize)
  const total = rows.length
  const maxPage = Math.max(1, Math.ceil(total / size) || 1)
  const safePage = Math.min(Math.max(1, pageIndex), maxPage)
  const start = (safePage - 1) * size
  return {
    pageRows: rows.slice(start, start + size) as T[],
    pageIndex: safePage,
    total,
  }
}
