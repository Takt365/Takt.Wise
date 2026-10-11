// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：legacyProduct.api.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变旧品管制 API（列表 / 详情 / 更新 / 导出）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type {
  EcLegacyProduct,
  EcLegacyProductQuery,
  EcLegacyProductUpdate,
} from '@/types/logistics/manufacturing/engineering-change/legacyProduct.types';

/**
 * API 路径前缀（对应后端 TaktEcLegacyProductsController）
 */
const EC_LEGACY_PRODUCT_API_BASE = 'TaktEcLegacyProducts';

/**
 * 获取旧品管制列表（分页）
 * @param queryDto 查询参数
 * @returns {Promise<TaktPagedResult<EcLegacyProduct>>} 分页结果
 */
export function getEcLegacyProductList(queryDto: EcLegacyProductQuery): Promise<TaktPagedResult<EcLegacyProduct>> {
  return request<TaktPagedResult<EcLegacyProduct>>({
    url: `${EC_LEGACY_PRODUCT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据设变明细 ID 获取旧品管制详情
 * @param ecDetailId 设变明细 ID
 * @returns {Promise<EcLegacyProduct>} 详情
 */
export function getEcLegacyProductByEcDetailId(ecDetailId: string): Promise<EcLegacyProduct> {
  return request<EcLegacyProduct>({
    url: `${EC_LEGACY_PRODUCT_API_BASE}/detail/${ecDetailId}`,
    method: 'get',
  });
}

/**
 * 更新旧品管制
 * @param ecDetailId 设变明细 ID
 * @param dto 更新 DTO
 * @returns {Promise<EcLegacyProduct>} 更新后的行
 */
export function updateEcLegacyProduct(ecDetailId: string, dto: EcLegacyProductUpdate): Promise<EcLegacyProduct> {
  return request<EcLegacyProduct>({
    url: `${EC_LEGACY_PRODUCT_API_BASE}/detail/${ecDetailId}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 导出旧品管制
 * @param query 查询条件
 * @param sheetName 工作表名
 * @param exportName 导出文件基名
 * @returns {Promise<Blob>} Excel blob
 */
export function exportEcLegacyProduct(
  query?: EcLegacyProductQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_LEGACY_PRODUCT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  });
}
