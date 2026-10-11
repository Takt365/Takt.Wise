// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseGroup.api.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
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
  PurchaseGroup,
  PurchaseGroupCreate,
  PurchaseGroupSort,
  PurchaseGroupStatus,
  PurchaseGroupUpdate
} from '@/types/logistics/procurement/purchaseGroup.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseGroups
 */
const PURCHASE_GROUP_API_BASE = 'TaktPurchaseGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseGroup>>} 分页结果
 */
export function getPurchaseGroupList(queryDto: any): Promise<TaktPagedResult<PurchaseGroup>> {
  return request<TaktPagedResult<PurchaseGroup>>({
    url: `${PURCHASE_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购组主数据
 * @param {string} id 采购组主数据ID
 * @returns {Promise<PurchaseGroup>} 采购组主数据DTO
 */
export function getPurchaseGroupById(id: string): Promise<PurchaseGroup> {
  return request<PurchaseGroup>({
    url: `${PURCHASE_GROUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购组主数据
 * @param {PurchaseGroupCreate} dto 创建DTO
 * @returns {Promise<PurchaseGroup>} 采购组主数据DTO
 */
export function createPurchaseGroup(dto: PurchaseGroupCreate): Promise<PurchaseGroup> {
  return request<PurchaseGroup>({
    url: `${PURCHASE_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购组主数据
 * @param {string} id 采购组主数据ID
 * @param {PurchaseGroupUpdate} dto 更新DTO
 * @returns {Promise<PurchaseGroup>} 采购组主数据DTO
 */
export function updatePurchaseGroup(id: string, dto: PurchaseGroupUpdate): Promise<PurchaseGroup> {
  return request<PurchaseGroup>({
    url: `${PURCHASE_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购组主数据
 * @param {string} id 采购组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseGroupById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购组主数据状态
 * @param {PurchaseGroupStatus} dto 状态 DTO
 * @returns {Promise<PurchaseGroup>} 采购组主数据DTO
 */
export function updatePurchaseGroupStatus(dto: PurchaseGroupStatus): Promise<PurchaseGroup> {
  return request<PurchaseGroup>({
    url: `${PURCHASE_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新采购组主数据排序
 * @param {PurchaseGroupSort} dto 排序DTO
 * @returns {Promise<PurchaseGroup>} 采购组主数据DTO
 */
export function updatePurchaseGroupSort(dto: PurchaseGroupSort): Promise<PurchaseGroup> {
  return request<PurchaseGroup>({
    url: `${PURCHASE_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购组主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseGroupOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_GROUP_API_BASE}/options`,
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
export function getPurchaseGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_GROUP_API_BASE}/import`,
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
 * 导出采购组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
