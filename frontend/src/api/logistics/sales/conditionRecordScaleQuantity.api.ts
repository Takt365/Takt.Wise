// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：conditionRecordScaleQuantity.api.ts
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
  SalesConditionRecordScaleQuantity,
  SalesConditionRecordScaleQuantityCreate,
  SalesConditionRecordScaleQuantityObsolete,
  SalesConditionRecordScaleQuantityUpdate
} from '@/types/logistics/sales/conditionRecordScaleQuantity.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesConditionRecordScaleQuantities
 */
const SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE = 'TaktSalesConditionRecordScaleQuantities';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售阶梯数量列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesConditionRecordScaleQuantity>>} 分页结果
 */
export function getSalesConditionRecordScaleQuantityList(queryDto: any): Promise<TaktPagedResult<SalesConditionRecordScaleQuantity>> {
  return request<TaktPagedResult<SalesConditionRecordScaleQuantity>>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @returns {Promise<SalesConditionRecordScaleQuantity>} 销售阶梯数量DTO
 */
export function getSalesConditionRecordScaleQuantityById(id: string): Promise<SalesConditionRecordScaleQuantity> {
  return request<SalesConditionRecordScaleQuantity>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建销售阶梯数量
 * @param {SalesConditionRecordScaleQuantityCreate} dto 创建DTO
 * @returns {Promise<SalesConditionRecordScaleQuantity>} 销售阶梯数量DTO
 */
export function createSalesConditionRecordScaleQuantity(dto: SalesConditionRecordScaleQuantityCreate): Promise<SalesConditionRecordScaleQuantity> {
  return request<SalesConditionRecordScaleQuantity>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @param {SalesConditionRecordScaleQuantityUpdate} dto 更新DTO
 * @returns {Promise<SalesConditionRecordScaleQuantity>} 销售阶梯数量DTO
 */
export function updateSalesConditionRecordScaleQuantity(id: string, dto: SalesConditionRecordScaleQuantityUpdate): Promise<SalesConditionRecordScaleQuantity> {
  return request<SalesConditionRecordScaleQuantity>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售阶梯数量
 * @param {string} id 销售阶梯数量ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesConditionRecordScaleQuantityById(id: string): Promise<void> {
  return request({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售阶梯数量
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesConditionRecordScaleQuantityBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售阶梯数量作废状态
 * @param {SalesConditionRecordScaleQuantityObsolete} dto 作废 DTO
 * @returns {Promise<SalesConditionRecordScaleQuantity>} 销售阶梯数量DTO
 */
export function updateSalesConditionRecordScaleQuantityObsolete(dto: SalesConditionRecordScaleQuantityObsolete): Promise<SalesConditionRecordScaleQuantity> {
  return request<SalesConditionRecordScaleQuantity>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/obsolete`,
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
export function getSalesConditionRecordScaleQuantityOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/options`,
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
export function getSalesConditionRecordScaleQuantityTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/template`,
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
export function importSalesConditionRecordScaleQuantity(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/import`,
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
export function exportSalesConditionRecordScaleQuantity(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_CONDITION_RECORD_SCALE_QUANTITY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
