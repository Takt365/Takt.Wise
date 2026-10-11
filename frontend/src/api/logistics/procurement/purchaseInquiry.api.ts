// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInquiry.api.ts
// 创建时间：2026-09-04
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
  PurchaseInquiry,
  PurchaseInquiryCreate,
  PurchaseInquiryStatus,
  PurchaseInquiryUpdate
} from '@/types/logistics/procurement/purchaseInquiry.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInquiries
 */
const PURCHASE_INQUIRY_API_BASE = 'TaktPurchaseInquiries';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购询价列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInquiry>>} 分页结果
 */
export function getPurchaseInquiryList(queryDto: any): Promise<TaktPagedResult<PurchaseInquiry>> {
  return request<TaktPagedResult<PurchaseInquiry>>({
    url: `${PURCHASE_INQUIRY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购询价
 * @param {string} id 采购询价ID
 * @returns {Promise<PurchaseInquiry>} 采购询价DTO
 */
export function getPurchaseInquiryById(id: string): Promise<PurchaseInquiry> {
  return request<PurchaseInquiry>({
    url: `${PURCHASE_INQUIRY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购询价
 * @param {PurchaseInquiryCreate} dto 创建DTO
 * @returns {Promise<PurchaseInquiry>} 采购询价DTO
 */
export function createPurchaseInquiry(dto: PurchaseInquiryCreate): Promise<PurchaseInquiry> {
  return request<PurchaseInquiry>({
    url: `${PURCHASE_INQUIRY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购询价
 * @param {string} id 采购询价ID
 * @param {PurchaseInquiryUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInquiry>} 采购询价DTO
 */
export function updatePurchaseInquiry(id: string, dto: PurchaseInquiryUpdate): Promise<PurchaseInquiry> {
  return request<PurchaseInquiry>({
    url: `${PURCHASE_INQUIRY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购询价
 * @param {string} id 采购询价ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInquiryById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INQUIRY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购询价
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInquiryBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INQUIRY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购询价状态
 * @param {PurchaseInquiryStatus} dto 状态 DTO
 * @returns {Promise<PurchaseInquiry>} 采购询价DTO
 */
export function updatePurchaseInquiryStatus(dto: PurchaseInquiryStatus): Promise<PurchaseInquiry> {
  return request<PurchaseInquiry>({
    url: `${PURCHASE_INQUIRY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购询价选项列表（DictValue=Id，ExtValue=PurchaseInquiryCode）
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseInquiryOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INQUIRY_API_BASE}/options`,
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
export function getPurchaseInquiryTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INQUIRY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购询价
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseInquiry(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INQUIRY_API_BASE}/import`,
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
 * 导出采购询价
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseInquiry(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INQUIRY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
