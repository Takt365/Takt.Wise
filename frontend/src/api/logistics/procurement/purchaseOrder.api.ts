// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseOrder.api.ts
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
  PurchaseOrder,
  PurchaseOrderCreate,
  PurchaseOrderStatus,
  PurchaseOrderUpdate
} from '@/types/logistics/procurement/purchaseOrder.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseOrders
 */
const PURCHASE_ORDER_API_BASE = 'TaktPurchaseOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购订单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseOrder>>} 分页结果
 */
export function getPurchaseOrderList(queryDto: any): Promise<TaktPagedResult<PurchaseOrder>> {
  return request<TaktPagedResult<PurchaseOrder>>({
    url: `${PURCHASE_ORDER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购订单
 * @param {string} id 采购订单ID
 * @returns {Promise<PurchaseOrder>} 采购订单DTO
 */
export function getPurchaseOrderById(id: string): Promise<PurchaseOrder> {
  return request<PurchaseOrder>({
    url: `${PURCHASE_ORDER_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建采购订单
 * @param {PurchaseOrderCreate} dto 创建DTO
 * @returns {Promise<PurchaseOrder>} 采购订单DTO
 */
export function createPurchaseOrder(dto: PurchaseOrderCreate): Promise<PurchaseOrder> {
  return request<PurchaseOrder>({
    url: `${PURCHASE_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购订单
 * @param {string} id 采购订单ID
 * @param {PurchaseOrderUpdate} dto 更新DTO
 * @returns {Promise<PurchaseOrder>} 采购订单DTO
 */
export function updatePurchaseOrder(id: string, dto: PurchaseOrderUpdate): Promise<PurchaseOrder> {
  return request<PurchaseOrder>({
    url: `${PURCHASE_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购订单
 * @param {string} id 采购订单ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购订单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购订单状态
 * @param {PurchaseOrderStatus} dto 状态 DTO
 * @returns {Promise<PurchaseOrder>} 采购订单DTO
 */
export function updatePurchaseOrderStatus(dto: PurchaseOrderStatus): Promise<PurchaseOrder> {
  return request<PurchaseOrder>({
    url: `${PURCHASE_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购订单选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseOrderOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_ORDER_API_BASE}/options`,
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
export function getPurchaseOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购订单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseOrder(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_ORDER_API_BASE}/import`,
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
 * 导出采购订单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
