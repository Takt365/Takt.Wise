// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：changeoverStat.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type { ChangeoverStatQuery, ChangeoverStatResult } from '@/types/logistics/manufacturing/output/changeoverStat.types'

/** API 路由前缀（对应 TaktChangeoverStatsController） */
const CHANGEOVER_STAT_API_BASE = 'TaktChangeoverStats'

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getChangeoverStatPlantOptionsUrl(): string {
  return `${CHANGEOVER_STAT_API_BASE}/plant-options`
}

/**
 * 班组选项 URL
 * @returns {string} 相对 API 路径
 */
export function getChangeoverStatTeamOptionsUrl(): string {
  return `${CHANGEOVER_STAT_API_BASE}/team-options`
}

/**
 * 切换统计清单
 * @param {ChangeoverStatQuery} queryDto 查询条件
 * @returns {Promise<ChangeoverStatResult>} 分页结果
 */
export function getChangeoverStatList(queryDto: ChangeoverStatQuery): Promise<ChangeoverStatResult> {
  return request<ChangeoverStatResult>({
    url: `${CHANGEOVER_STAT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  })
}

/**
 * 导出切换统计
 * @param {ChangeoverStatQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportChangeoverStat(
  query: ChangeoverStatQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${CHANGEOVER_STAT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  })
}
