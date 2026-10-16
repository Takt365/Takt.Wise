// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInfoScaleQuantity.api.ts
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
  PurchaseInfoScaleQuantity,
  PurchaseInfoScaleQuantityCreate,
  PurchaseInfoScaleQuantityObsolete,
  PurchaseInfoScaleQuantityUpdate
} from '@/types/logistics/procurement/purchaseInfoScaleQuantity.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInfoScaleQuantities
 */
const PURCHASE_INFO_SCALE_QUANTITY_API_BASE = 'TaktPurchaseInfoScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购阶梯数量列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInfoScaleQuantity>>} 分页结果
 */
export function getPurchaseInfoScaleQuantityList(queryDto: any): Promise<TaktPagedResult<PurchaseInfoScaleQuantity>> {
  return request<TaktPagedResult<PurchaseInfoScaleQuantity>>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @returns {Promise<PurchaseInfoScaleQuantity>} 采购阶梯数量DTO
 */
export function getPurchaseInfoScaleQuantityById(id: string): Promise<PurchaseInfoScaleQuantity> {
  return request<PurchaseInfoScaleQuantity>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购阶梯数量
 * @param {PurchaseInfoScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<PurchaseInfoScaleQuantity>} 采购阶梯数量DTO
 */
export function createPurchaseInfoScaleQuantity(dto: PurchaseInfoScaleQuantityCreate): Promise<PurchaseInfoScaleQuantity> {
  return request<PurchaseInfoScaleQuantity>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @param {PurchaseInfoScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInfoScaleQuantity>} 采购阶梯数量DTO
 */
export function updatePurchaseInfoScaleQuantity(id: string, dto: PurchaseInfoScaleQuantityUpdate): Promise<PurchaseInfoScaleQuantity> {
  return request<PurchaseInfoScaleQuantity>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购阶梯数量
 * @param {string} id 采购阶梯数量ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购阶梯数量
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购阶梯数量作废状态
 * @param {PurchaseInfoScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseInfoScaleQuantity>} 采购阶梯数量DTO
 */
export function updatePurchaseInfoScaleQuantityObsolete(dto: PurchaseInfoScaleQuantityObsolete): Promise<PurchaseInfoScaleQuantity> {
  return request<PurchaseInfoScaleQuantity>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/obsolete`,
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
export function getPurchaseInfoScaleQuantityOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/options`,
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
export function getPurchaseInfoScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/template`,
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
export function importPurchaseInfoScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/import`,
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
export function exportPurchaseInfoScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
