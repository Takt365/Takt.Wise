// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInfo.api.ts
// 创建时间：2026-09-21
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
  PurchaseInfo,
  PurchaseInfoCreate,
  PurchaseInfoUpdate
} from '@/types/logistics/procurement/purchaseInfo.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInfos
 */
const PURCHASE_INFO_API_BASE = 'TaktPurchaseInfos';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInfo>>} 分页结果
 */
export function getPurchaseInfoList(queryDto: any): Promise<TaktPagedResult<PurchaseInfo>> {
  return request<TaktPagedResult<PurchaseInfo>>({
    url: `${PURCHASE_INFO_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购信息
 * @param {string} id 采购信息ID
 * @returns {Promise<PurchaseInfo>} 采购信息DTO
 */
export function getPurchaseInfoById(id: string): Promise<PurchaseInfo> {
  return request<PurchaseInfo>({
    url: `${PURCHASE_INFO_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购信息
 * @param {PurchaseInfoCreate} dto 创建DTO
 * @returns {Promise<PurchaseInfo>} 采购信息DTO
 */
export function createPurchaseInfo(dto: PurchaseInfoCreate): Promise<PurchaseInfo> {
  return request<PurchaseInfo>({
    url: `${PURCHASE_INFO_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购信息
 * @param {string} id 采购信息ID
 * @param {PurchaseInfoUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInfo>} 采购信息DTO
 */
export function updatePurchaseInfo(id: string, dto: PurchaseInfoUpdate): Promise<PurchaseInfo> {
  return request<PurchaseInfo>({
    url: `${PURCHASE_INFO_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购信息
 * @param {string} id 采购信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购价格选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseInfoOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INFO_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getPurchaseInfoTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseInfo(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INFO_API_BASE}/import`,
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
 * 导出采购信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseInfo(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
