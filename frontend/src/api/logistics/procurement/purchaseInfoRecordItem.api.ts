// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInfoRecordItem.api.ts
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
  PurchaseInfoRecordItem,
  PurchaseInfoRecordItemCreate,
  PurchaseInfoRecordItemObsolete,
  PurchaseInfoRecordItemUpdate
} from '@/types/logistics/procurement/purchaseInfoRecordItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInfoRecordItems
 */
const PURCHASE_INFO_RECORD_ITEM_API_BASE = 'TaktPurchaseInfoRecordItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购信息记录行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInfoRecordItem>>} 分页结果
 */
export function getPurchaseInfoRecordItemList(queryDto: any): Promise<TaktPagedResult<PurchaseInfoRecordItem>> {
  return request<TaktPagedResult<PurchaseInfoRecordItem>>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购信息记录行
 * @param {string} id 采购信息记录行ID
 * @returns {Promise<PurchaseInfoRecordItem>} 采购信息记录行DTO
 */
export function getPurchaseInfoRecordItemById(id: string): Promise<PurchaseInfoRecordItem> {
  return request<PurchaseInfoRecordItem>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购信息记录行
 * @param {PurchaseInfoRecordItemCreate} dto 创建DTO
 * @returns {Promise<PurchaseInfoRecordItem>} 采购信息记录行DTO
 */
export function createPurchaseInfoRecordItem(dto: PurchaseInfoRecordItemCreate): Promise<PurchaseInfoRecordItem> {
  return request<PurchaseInfoRecordItem>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购信息记录行
 * @param {string} id 采购信息记录行ID
 * @param {PurchaseInfoRecordItemUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInfoRecordItem>} 采购信息记录行DTO
 */
export function updatePurchaseInfoRecordItem(id: string, dto: PurchaseInfoRecordItemUpdate): Promise<PurchaseInfoRecordItem> {
  return request<PurchaseInfoRecordItem>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购信息记录行
 * @param {string} id 采购信息记录行ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoRecordItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购信息记录行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInfoRecordItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购信息记录行作废状态
 * @param {PurchaseInfoRecordItemObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseInfoRecordItem>} 采购信息记录行DTO
 */
export function updatePurchaseInfoRecordItemObsolete(dto: PurchaseInfoRecordItemObsolete): Promise<PurchaseInfoRecordItem> {
  return request<PurchaseInfoRecordItem>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购信息记录行选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseInfoRecordItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/options`,
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
export function getPurchaseInfoRecordItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购信息记录行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseInfoRecordItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/import`,
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
 * 导出采购信息记录行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseInfoRecordItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INFO_RECORD_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
