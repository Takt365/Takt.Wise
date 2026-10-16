// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：conditionScaleQuantity.api.ts
// 创建时间：2026-09-21
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
  SalesConditionScaleQuantity,
  SalesConditionScaleQuantityCreate,
  SalesConditionScaleQuantityObsolete,
  SalesConditionScaleQuantityUpdate
} from '@/types/logistics/sales/conditionScaleQuantity.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesConditionScaleQuantities
 */
const SALES_CONDITION_SCALE_QUANTITY_API_BASE = 'TaktSalesConditionScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售阶梯数量列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesConditionScaleQuantity>>} 分页结果
 */
export function getSalesConditionScaleQuantityList(queryDto: any): Promise<TaktPagedResult<SalesConditionScaleQuantity>> {
  return request<TaktPagedResult<SalesConditionScaleQuantity>>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @returns {Promise<SalesConditionScaleQuantity>} 销售阶梯数量DTO
 */
export function getSalesConditionScaleQuantityById(id: string): Promise<SalesConditionScaleQuantity> {
  return request<SalesConditionScaleQuantity>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建销售阶梯数量
 * @param {SalesConditionScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<SalesConditionScaleQuantity>} 销售阶梯数量DTO
 */
export function createSalesConditionScaleQuantity(dto: SalesConditionScaleQuantityCreate): Promise<SalesConditionScaleQuantity> {
  return request<SalesConditionScaleQuantity>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @param {SalesConditionScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<SalesConditionScaleQuantity>} 销售阶梯数量DTO
 */
export function updateSalesConditionScaleQuantity(id: string, dto: SalesConditionScaleQuantityUpdate): Promise<SalesConditionScaleQuantity> {
  return request<SalesConditionScaleQuantity>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesConditionScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售阶梯数量
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesConditionScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售阶梯数量作废状态
 * @param {SalesConditionScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<SalesConditionScaleQuantity>} 销售阶梯数量DTO
 */
export function updateSalesConditionScaleQuantityObsolete(dto: SalesConditionScaleQuantityObsolete): Promise<SalesConditionScaleQuantity> {
  return request<SalesConditionScaleQuantity>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售阶梯数量选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesConditionScaleQuantityOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/options`,
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
export function getSalesConditionScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售阶梯数量
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesConditionScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/import`,
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
 * 导出销售阶梯数量
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesConditionScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_CONDITION_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
