// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerComplaintTrend.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  CustomerComplaintMonthlyTrendQuery,
  CustomerComplaintMonthlyTrendResult
} from '@/types/logistics/quality/complaint/customerComplaintTrend.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerComplaintTrends
 */
const CUSTOMER_COMPLAINT_TREND_API_BASE = 'TaktCustomerComplaintTrends';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 顾客投诉月度推移转置分析
 * @param {CustomerComplaintMonthlyTrendQuery} queryDto 查询 DTO
 * @returns {Promise<CustomerComplaintMonthlyTrendResult>} 分析结果
 */
export function getCustomerComplaintMonthlyTrendAnalysis(queryDto: CustomerComplaintMonthlyTrendQuery): Promise<CustomerComplaintMonthlyTrendResult> {
  return request<CustomerComplaintMonthlyTrendResult>({
    url: `${CUSTOMER_COMPLAINT_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出顾客投诉月度推移转置分析
 * @param {any} query 查询 DTO
 * @param {string} sheetName 工作表名
 * @param {string} exportName 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportCustomerComplaintMonthlyTrendAnalysis(
  query: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_TREND_API_BASE}/monthly-trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
