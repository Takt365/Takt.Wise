// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseInvoiceItem.api.ts
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
  PurchaseInvoiceItem,
  PurchaseInvoiceItemCreate,
  PurchaseInvoiceItemObsolete,
  PurchaseInvoiceItemUpdate
} from '@/types/logistics/procurement/purchaseInvoiceItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseInvoiceItems
 */
const PURCHASE_INVOICE_ITEM_API_BASE = 'TaktPurchaseInvoiceItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购发票明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseInvoiceItem>>} 分页结果
 */
export function getPurchaseInvoiceItemList(queryDto: any): Promise<TaktPagedResult<PurchaseInvoiceItem>> {
  return request<TaktPagedResult<PurchaseInvoiceItem>>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购发票明细
 * @param {string} id 采购发票明细ID
 * @returns {Promise<PurchaseInvoiceItem>} 采购发票明细DTO
 */
export function getPurchaseInvoiceItemById(id: string): Promise<PurchaseInvoiceItem> {
  return request<PurchaseInvoiceItem>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购发票明细
 * @param {PurchaseInvoiceItemCreate} dto 创建DTO
 * @returns {Promise<PurchaseInvoiceItem>} 采购发票明细DTO
 */
export function createPurchaseInvoiceItem(dto: PurchaseInvoiceItemCreate): Promise<PurchaseInvoiceItem> {
  return request<PurchaseInvoiceItem>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购发票明细
 * @param {string} id 采购发票明细ID
 * @param {PurchaseInvoiceItemUpdate} dto 更新DTO
 * @returns {Promise<PurchaseInvoiceItem>} 采购发票明细DTO
 */
export function updatePurchaseInvoiceItem(id: string, dto: PurchaseInvoiceItemUpdate): Promise<PurchaseInvoiceItem> {
  return request<PurchaseInvoiceItem>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购发票明细
 * @param {string} id 采购发票明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInvoiceItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购发票明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseInvoiceItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购发票明细作废状态
 * @param {PurchaseInvoiceItemObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseInvoiceItem>} 采购发票明细DTO
 */
export function updatePurchaseInvoiceItemObsolete(dto: PurchaseInvoiceItemObsolete): Promise<PurchaseInvoiceItem> {
  return request<PurchaseInvoiceItem>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购发票明细选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseInvoiceItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/options`,
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
export function getPurchaseInvoiceItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购发票明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseInvoiceItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/import`,
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
 * 导出采购发票明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseInvoiceItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_INVOICE_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
