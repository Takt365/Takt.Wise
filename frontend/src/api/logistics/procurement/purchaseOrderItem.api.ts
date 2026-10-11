// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseOrderItem.api.ts
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
  PurchaseOrderItem,
  PurchaseOrderItemCreate,
  PurchaseOrderItemObsolete,
  PurchaseOrderItemStatus,
  PurchaseOrderItemUpdate
} from '@/types/logistics/procurement/purchaseOrderItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseOrderItems
 */
const PURCHASE_ORDER_ITEM_API_BASE = 'TaktPurchaseOrderItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购订单明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseOrderItem>>} 分页结果
 */
export function getPurchaseOrderItemList(queryDto: any): Promise<TaktPagedResult<PurchaseOrderItem>> {
  return request<TaktPagedResult<PurchaseOrderItem>>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购订单明细
 * @param {string} id 采购订单明细ID
 * @returns {Promise<PurchaseOrderItem>} 采购订单明细DTO
 */
export function getPurchaseOrderItemById(id: string): Promise<PurchaseOrderItem> {
  return request<PurchaseOrderItem>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购订单明细
 * @param {PurchaseOrderItemCreate} dto 创建DTO
 * @returns {Promise<PurchaseOrderItem>} 采购订单明细DTO
 */
export function createPurchaseOrderItem(dto: PurchaseOrderItemCreate): Promise<PurchaseOrderItem> {
  return request<PurchaseOrderItem>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购订单明细
 * @param {string} id 采购订单明细ID
 * @param {PurchaseOrderItemUpdate} dto 更新DTO
 * @returns {Promise<PurchaseOrderItem>} 采购订单明细DTO
 */
export function updatePurchaseOrderItem(id: string, dto: PurchaseOrderItemUpdate): Promise<PurchaseOrderItem> {
  return request<PurchaseOrderItem>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购订单明细
 * @param {string} id 采购订单明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购订单明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购订单明细状态
 * @param {PurchaseOrderItemStatus} dto 状态 DTO
 * @returns {Promise<PurchaseOrderItem>} 采购订单明细DTO
 */
export function updatePurchaseOrderItemStatus(dto: PurchaseOrderItemStatus): Promise<PurchaseOrderItem> {
  return request<PurchaseOrderItem>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新采购订单明细作废状态
 * @param {PurchaseOrderItemObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseOrderItem>} 采购订单明细DTO
 */
export function updatePurchaseOrderItemObsolete(dto: PurchaseOrderItemObsolete): Promise<PurchaseOrderItem> {
  return request<PurchaseOrderItem>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购订单明细选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseOrderItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/options`,
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
export function getPurchaseOrderItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购订单明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseOrderItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/import`,
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
 * 导出采购订单明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseOrderItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_ORDER_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
