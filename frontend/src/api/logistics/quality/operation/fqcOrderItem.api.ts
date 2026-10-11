// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：fqcOrderItem.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common.types';
import type {
  FqcOrderItem,
  FqcOrderItemCreate,
  FqcOrderItemObsolete,
  FqcOrderItemStatus,
  FqcOrderItemUpdate
} from '@/types/logistics/quality/operation/fqcOrderItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFqcOrderItems
 */
const FQC_ORDER_ITEM_API_BASE = 'TaktFqcOrderItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取出货检验单明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FqcOrderItem>>} 分页结果
 */
export function getFqcOrderItemList(queryDto: any): Promise<TaktPagedResult<FqcOrderItem>> {
  return request<TaktPagedResult<FqcOrderItem>>({
    url: `${FQC_ORDER_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取出货检验单明细
 * @param {string} id 出货检验单明细ID
 * @returns {Promise<FqcOrderItem>} 出货检验单明细DTO
 */
export function getFqcOrderItemById(id: string): Promise<FqcOrderItem> {
  return request<FqcOrderItem>({
    url: `${FQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建出货检验单明细
 * @param {FqcOrderItemCreate} dto 创建DTO
 * @returns {Promise<FqcOrderItem>} 出货检验单明细DTO
 */
export function createFqcOrderItem(dto: FqcOrderItemCreate): Promise<FqcOrderItem> {
  return request<FqcOrderItem>({
    url: `${FQC_ORDER_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新出货检验单明细
 * @param {string} id 出货检验单明细ID
 * @param {FqcOrderItemUpdate} dto 更新DTO
 * @returns {Promise<FqcOrderItem>} 出货检验单明细DTO
 */
export function updateFqcOrderItem(id: string, dto: FqcOrderItemUpdate): Promise<FqcOrderItem> {
  return request<FqcOrderItem>({
    url: `${FQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除出货检验单明细
 * @param {string} id 出货检验单明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderItemById(id: string): Promise<void> {
  return request({
    url: `${FQC_ORDER_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除出货检验单明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FQC_ORDER_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新出货检验单明细状态
 * @param {FqcOrderItemStatus} dto 状态 DTO
 * @returns {Promise<FqcOrderItem>} 出货检验单明细DTO
 */
export function updateFqcOrderItemStatus(dto: FqcOrderItemStatus): Promise<FqcOrderItem> {
  return request<FqcOrderItem>({
    url: `${FQC_ORDER_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新出货检验单明细作废状态
 * @param {FqcOrderItemObsolete} dto 作废 DTO
 * @returns {Promise<FqcOrderItem>} 出货检验单明细DTO
 */
export function updateFqcOrderItemObsolete(dto: FqcOrderItemObsolete): Promise<FqcOrderItem> {
  return request<FqcOrderItem>({
    url: `${FQC_ORDER_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取出货检验单明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFqcOrderItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FQC_ORDER_ITEM_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getFqcOrderItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入出货检验单明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFqcOrderItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FQC_ORDER_ITEM_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出出货检验单明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFqcOrderItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
