// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：quotation.api.ts
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
  SalesQuotation,
  SalesQuotationCreate,
  SalesQuotationStatus,
  SalesQuotationUpdate
} from '@/types/logistics/sales/quotation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesQuotations
 */
const SALES_QUOTATION_API_BASE = 'TaktSalesQuotations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售报价列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesQuotation>>} 分页结果
 */
export function getSalesQuotationList(queryDto: any): Promise<TaktPagedResult<SalesQuotation>> {
  return request<TaktPagedResult<SalesQuotation>>({
    url: `${SALES_QUOTATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售报价
 * @param {string} id 销售报价ID
 * @returns {Promise<SalesQuotation>} 销售报价DTO
 */
export function getSalesQuotationById(id: string): Promise<SalesQuotation> {
  return request<SalesQuotation>({
    url: `${SALES_QUOTATION_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建销售报价
 * @param {SalesQuotationCreate} dto 创建DTO
 * @returns {Promise<SalesQuotation>} 销售报价DTO
 */
export function createSalesQuotation(dto: SalesQuotationCreate): Promise<SalesQuotation> {
  return request<SalesQuotation>({
    url: `${SALES_QUOTATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售报价
 * @param {string} id 销售报价ID
 * @param {SalesQuotationUpdate} dto 更新DTO
 * @returns {Promise<SalesQuotation>} 销售报价DTO
 */
export function updateSalesQuotation(id: string, dto: SalesQuotationUpdate): Promise<SalesQuotation> {
  return request<SalesQuotation>({
    url: `${SALES_QUOTATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售报价
 * @param {string} id 销售报价ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesQuotationById(id: string): Promise<void> {
  return request({
    url: `${SALES_QUOTATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售报价
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesQuotationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_QUOTATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售报价状态
 * @param {SalesQuotationStatus} dto 状态 DTO
 * @returns {Promise<SalesQuotation>} 销售报价DTO
 */
export function updateSalesQuotationStatus(dto: SalesQuotationStatus): Promise<SalesQuotation> {
  return request<SalesQuotation>({
    url: `${SALES_QUOTATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售报价选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesQuotationOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_QUOTATION_API_BASE}/options`,
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
export function getSalesQuotationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_QUOTATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售报价
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesQuotation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_QUOTATION_API_BASE}/import`,
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
 * 导出销售报价
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesQuotation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_QUOTATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
