// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/defect
// 文件名称：defectOrderStat.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  DefectOrderStatQuery,
  DefectOrderStatResult,
} from '@/types/logistics/manufacturing/defect/defectOrderStat.types';

/** API 路由前缀（对应 TaktDefectStatsController） */
const DEFECT_STAT_API_BASE = 'TaktDefectStats';

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectStatPlantOptionsUrl(): string {
  return `${DEFECT_STAT_API_BASE}/plant-options`;
}

/**
 * 工单选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectStatOrderOptionsUrl(): string {
  return `${DEFECT_STAT_API_BASE}/order-options`;
}

/**
 * 批次选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectStatBatchOptionsUrl(): string {
  return `${DEFECT_STAT_API_BASE}/batch-options`;
}

/**
 * 机种选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectStatModelOptionsUrl(): string {
  return `${DEFECT_STAT_API_BASE}/model-options`;
}

/**
 * 物料选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectStatMaterialOptionsUrl(): string {
  return `${DEFECT_STAT_API_BASE}/material-options`;
}

/**
 * 不良统计列表
 * @param {DefectOrderStatQuery} queryDto 查询条件
 * @returns {Promise<DefectOrderStatResult>} 分页结果
 */
export function getDefectStatList(queryDto: DefectOrderStatQuery): Promise<DefectOrderStatResult> {
  return request<DefectOrderStatResult>({
    url: `${DEFECT_STAT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出不良统计
 * @param {DefectOrderStatQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportDefectStat(
  query: DefectOrderStatQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${DEFECT_STAT_API_BASE}/export`,
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
