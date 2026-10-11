// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：invoice.api.ts
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块 API（自动生成，请勿手改路由常量）
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
  SalesInvoice,
  SalesInvoiceCreate,
  SalesInvoiceStatus,
  SalesInvoiceUpdate
} from '@/types/logistics/sales/invoice.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesInvoices
 */
const SALES_INVOICE_API_BASE = 'TaktSalesInvoices';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售发票列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesInvoice>>} 分页结果
 */
export function getSalesInvoiceList(queryDto: any): Promise<TaktPagedResult<SalesInvoice>> {
  return request<TaktPagedResult<SalesInvoice>>({
    url: `${SALES_INVOICE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售发票
 * @param {string} id 销售发票ID
 * @returns {Promise<SalesInvoice>} 销售发票DTO
 */
export function getSalesInvoiceById(id: string): Promise<SalesInvoice> {
  return request<SalesInvoice>({
    url: `${SALES_INVOICE_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建销售发票
 * @param {SalesInvoiceCreate} dto 创建DTO
 * @returns {Promise<SalesInvoice>} 销售发票DTO
 */
export function createSalesInvoice(dto: SalesInvoiceCreate): Promise<SalesInvoice> {
  return request<SalesInvoice>({
    url: `${SALES_INVOICE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售发票
 * @param {string} id 销售发票ID
 * @param {SalesInvoiceUpdate} dto 更新DTO
 * @returns {Promise<SalesInvoice>} 销售发票DTO
 */
export function updateSalesInvoice(id: string, dto: SalesInvoiceUpdate): Promise<SalesInvoice> {
  return request<SalesInvoice>({
    url: `${SALES_INVOICE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售发票
 * @param {string} id 销售发票ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesInvoiceById(id: string): Promise<void> {
  return request({
    url: `${SALES_INVOICE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售发票
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesInvoiceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_INVOICE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售发票状态
 * @param {SalesInvoiceStatus} dto 状态 DTO
 * @returns {Promise<SalesInvoice>} 销售发票DTO
 */
export function updateSalesInvoiceStatus(dto: SalesInvoiceStatus): Promise<SalesInvoice> {
  return request<SalesInvoice>({
    url: `${SALES_INVOICE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售发票选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesInvoiceOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_INVOICE_API_BASE}/options`,
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
export function getSalesInvoiceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_INVOICE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售发票
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesInvoice(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_INVOICE_API_BASE}/import`,
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
 * 导出销售发票
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesInvoice(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_INVOICE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
