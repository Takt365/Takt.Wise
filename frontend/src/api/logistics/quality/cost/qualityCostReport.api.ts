// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：qualityCostReport.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  QualityCostReportQuery,
  QualityCostReportResult,
} from '@/types/logistics/quality/cost/qualityCostReport.types';

/** API 路由前缀（对应 TaktQualityCostReportsController） */
const QUALITY_COST_REPORT_API_BASE = 'TaktQualityCostReports';

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getQualityCostReportPlantOptionsUrl(): string {
  return `${QUALITY_COST_REPORT_API_BASE}/plant-options`;
}

/**
 * 维度选项 URL
 * @returns {string} 相对 API 路径
 */
export function getQualityCostReportDimensionOptionsUrl(): string {
  return `${QUALITY_COST_REPORT_API_BASE}/dimension-options`;
}

/**
 * 币种选项 URL
 * @returns {string} 相对 API 路径
 */
export function getQualityCostReportCurrencyOptionsUrl(): string {
  return `${QUALITY_COST_REPORT_API_BASE}/currency-options`;
}

/**
 * 品质成本报告列表
 * @param {QualityCostReportQuery} queryDto 查询条件
 * @returns {Promise<QualityCostReportResult>} 分页结果
 */
export function getQualityCostReportList(queryDto: QualityCostReportQuery): Promise<QualityCostReportResult> {
  return request<QualityCostReportResult>({
    url: `${QUALITY_COST_REPORT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出品质成本报告
 * @param {QualityCostReportQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportQualityCostReport(
  query: QualityCostReportQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_COST_REPORT_API_BASE}/export`,
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
