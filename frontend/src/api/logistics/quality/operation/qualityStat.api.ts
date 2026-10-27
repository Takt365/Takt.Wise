// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：qualityStat.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { QualityStatQuery, QualityStatResult } from '@/types/logistics/quality/operation/qualityStat.types';

/** API 路由前缀（对应 TaktQualityStatsController） */
const QUALITY_STAT_API_BASE = 'TaktQualityStats';

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getQualityStatPlantOptionsUrl(): string {
  return `${QUALITY_STAT_API_BASE}/plant-options`;
}

/**
 * 维度选项 URL
 * @returns {string} 相对 API 路径
 */
export function getQualityStatDimensionOptionsUrl(): string {
  return `${QUALITY_STAT_API_BASE}/dimension-options`;
}

/**
 * 质量统计列表
 * @param {QualityStatQuery} queryDto 查询条件
 * @returns {Promise<QualityStatResult>} 分页结果
 */
export function getQualityStatList(queryDto: QualityStatQuery): Promise<QualityStatResult> {
  return request<QualityStatResult>({
    url: `${QUALITY_STAT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出质量统计
 * @param {QualityStatQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportQualityStat(
  query: QualityStatQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_STAT_API_BASE}/export`,
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
