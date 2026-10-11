// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：quotationItem.api.ts
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
  SalesQuotationItem,
  SalesQuotationItemCreate,
  SalesQuotationItemObsolete,
  SalesQuotationItemUpdate
} from '@/types/logistics/sales/quotationItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesQuotationItems
 */
const SALES_QUOTATION_ITEM_API_BASE = 'TaktSalesQuotationItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售报价明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesQuotationItem>>} 分页结果
 */
export function getSalesQuotationItemList(queryDto: any): Promise<TaktPagedResult<SalesQuotationItem>> {
  return request<TaktPagedResult<SalesQuotationItem>>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售报价明细
 * @param {string} id 销售报价明细ID
 * @returns {Promise<SalesQuotationItem>} 销售报价明细DTO
 */
export function getSalesQuotationItemById(id: string): Promise<SalesQuotationItem> {
  return request<SalesQuotationItem>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建销售报价明细
 * @param {SalesQuotationItemCreate} dto 创建DTO
 * @returns {Promise<SalesQuotationItem>} 销售报价明细DTO
 */
export function createSalesQuotationItem(dto: SalesQuotationItemCreate): Promise<SalesQuotationItem> {
  return request<SalesQuotationItem>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售报价明细
 * @param {string} id 销售报价明细ID
 * @param {SalesQuotationItemUpdate} dto 更新DTO
 * @returns {Promise<SalesQuotationItem>} 销售报价明细DTO
 */
export function updateSalesQuotationItem(id: string, dto: SalesQuotationItemUpdate): Promise<SalesQuotationItem> {
  return request<SalesQuotationItem>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售报价明细
 * @param {string} id 销售报价明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesQuotationItemById(id: string): Promise<void> {
  return request({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售报价明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesQuotationItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售报价明细作废状态
 * @param {SalesQuotationItemObsolete} dto 作废 DTO
 * @returns {Promise<SalesQuotationItem>} 销售报价明细DTO
 */
export function updateSalesQuotationItemObsolete(dto: SalesQuotationItemObsolete): Promise<SalesQuotationItem> {
  return request<SalesQuotationItem>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售报价明细选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesQuotationItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/options`,
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
export function getSalesQuotationItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售报价明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesQuotationItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/import`,
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
 * 导出销售报价明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesQuotationItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_QUOTATION_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
