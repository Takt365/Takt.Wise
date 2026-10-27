// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecStat.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { EcStatQuery, EcStatResult } from '@/types/logistics/manufacturing/engineering-change/ecStat.types';

/** API 路由前缀（对应 TaktEcStatsController） */
const EC_STAT_API_BASE = 'TaktEcStats';

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getEcStatPlantOptionsUrl(): string {
  return `${EC_STAT_API_BASE}/plant-options`;
}

/**
 * 设变单号选项 URL
 * @returns {string} 相对 API 路径
 */
export function getEcStatEcOptionsUrl(): string {
  return `${EC_STAT_API_BASE}/ec-options`;
}

/**
 * 设变统计列表
 * @param {EcStatQuery} queryDto 查询条件
 * @returns {Promise<EcStatResult>} 分页结果
 */
export function getEcStatList(queryDto: EcStatQuery): Promise<EcStatResult> {
  return request<EcStatResult>({
    url: `${EC_STAT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出设变统计
 * @param {EcStatQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportEcStat(query: EcStatQuery, sheetName?: string, exportName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_STAT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}
