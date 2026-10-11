// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInfoRecordScaleQuantity.api.ts
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
  PurchaseInfoRecordScaleQuantity,
  PurchaseInfoRecordScaleQuantityCreate,
  PurchaseInfoRecordScaleQuantityObsolete,
  PurchaseInfoRecordScaleQuantityUpdate
} from '@/types/logistics/procurement/purchaseInfoRecordScaleQuantity.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInfoRecordScaleQuantities
 */
const PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE = 'TaktPurchaseInfoRecordScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购阶梯数量列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInfoRecordScaleQuantity>>} 分页结果
 */
export function getPurchaseInfoRecordScaleQuantityList(queryDto: any): Promise<TaktPagedResult<PurchaseInfoRecordScaleQuantity>> {
  return request<TaktPagedResult<PurchaseInfoRecordScaleQuantity>>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @returns {Promise<PurchaseInfoRecordScaleQuantity>} 采购阶梯数量DTO
 */
export function getPurchaseInfoRecordScaleQuantityById(id: string): Promise<PurchaseInfoRecordScaleQuantity> {
  return request<PurchaseInfoRecordScaleQuantity>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购阶梯数量
 * @param {PurchaseInfoRecordScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<PurchaseInfoRecordScaleQuantity>} 采购阶梯数量DTO
 */
export function createPurchaseInfoRecordScaleQuantity(dto: PurchaseInfoRecordScaleQuantityCreate): Promise<PurchaseInfoRecordScaleQuantity> {
  return request<PurchaseInfoRecordScaleQuantity>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @param {PurchaseInfoRecordScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInfoRecordScaleQuantity>} 采购阶梯数量DTO
 */
export function updatePurchaseInfoRecordScaleQuantity(id: string, dto: PurchaseInfoRecordScaleQuantityUpdate): Promise<PurchaseInfoRecordScaleQuantity> {
  return request<PurchaseInfoRecordScaleQuantity>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoRecordScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购阶梯数量
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoRecordScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购阶梯数量作废状态
 * @param {PurchaseInfoRecordScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseInfoRecordScaleQuantity>} 采购阶梯数量DTO
 */
export function updatePurchaseInfoRecordScaleQuantityObsolete(dto: PurchaseInfoRecordScaleQuantityObsolete): Promise<PurchaseInfoRecordScaleQuantity> {
  return request<PurchaseInfoRecordScaleQuantity>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购阶梯数量选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseInfoRecordScaleQuantityOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/options`,
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
export function getPurchaseInfoRecordScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购阶梯数量
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseInfoRecordScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/import`,
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
 * 导出采购阶梯数量
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseInfoRecordScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_RECORD_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
