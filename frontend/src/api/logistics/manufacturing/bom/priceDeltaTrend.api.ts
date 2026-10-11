// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：priceDeltaTrend.api.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移 API（TaktBomPriceDeltaTrends）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  BomPriceDeltaTrendQuery,
  BomPriceDeltaTrendResult,
} from '@/types/logistics/manufacturing/bom/priceDeltaTrend.types'

const API_BASE = 'TaktBomPriceDeltaTrends'

/**
 * 成本差异推移列表
 * @param queryDto 查询
 * @returns 分页结果
 */
export function getBomPriceDeltaTrendList(
  queryDto: BomPriceDeltaTrendQuery,
): Promise<BomPriceDeltaTrendResult> {
  return request<BomPriceDeltaTrendResult>({
    url: `${API_BASE}/list`,
    method: 'get',
    params: queryDto,
  })
}

/**
 * 导出成本差异推移
 * @param query 查询
 * @param sheetName 工作表名
 * @param exportName 导出文件名
 * @returns Excel
 */
export function exportBomPriceDeltaTrendData(
  query: BomPriceDeltaTrendQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${API_BASE}/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
  })
}
