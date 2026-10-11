// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerSatisfactionSurveyItem.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块 API（自动生成，请勿手改路由常量）
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
  CustomerSatisfactionSurveyItem,
  CustomerSatisfactionSurveyItemCreate,
  CustomerSatisfactionSurveyItemObsolete,
  CustomerSatisfactionSurveyItemStatus,
  CustomerSatisfactionSurveyItemUpdate
} from '@/types/logistics/quality/complaint/customerSatisfactionSurveyItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerSatisfactionSurveyItems
 */
const CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE = 'TaktCustomerSatisfactionSurveyItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客户满意度调查项目明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerSatisfactionSurveyItem>>} 分页结果
 */
export function getCustomerSatisfactionSurveyItemList(queryDto: any): Promise<TaktPagedResult<CustomerSatisfactionSurveyItem>> {
  return request<TaktPagedResult<CustomerSatisfactionSurveyItem>>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客户满意度调查项目明细
 * @param {string} id 客户满意度调查项目明细ID
 * @returns {Promise<CustomerSatisfactionSurveyItem>} 客户满意度调查项目明细DTO
 */
export function getCustomerSatisfactionSurveyItemById(id: string): Promise<CustomerSatisfactionSurveyItem> {
  return request<CustomerSatisfactionSurveyItem>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建客户满意度调查项目明细
 * @param {CustomerSatisfactionSurveyItemCreate} dto 创建DTO
 * @returns {Promise<CustomerSatisfactionSurveyItem>} 客户满意度调查项目明细DTO
 */
export function createCustomerSatisfactionSurveyItem(dto: CustomerSatisfactionSurveyItemCreate): Promise<CustomerSatisfactionSurveyItem> {
  return request<CustomerSatisfactionSurveyItem>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客户满意度调查项目明细
 * @param {string} id 客户满意度调查项目明细ID
 * @param {CustomerSatisfactionSurveyItemUpdate} dto 更新DTO
 * @returns {Promise<CustomerSatisfactionSurveyItem>} 客户满意度调查项目明细DTO
 */
export function updateCustomerSatisfactionSurveyItem(id: string, dto: CustomerSatisfactionSurveyItemUpdate): Promise<CustomerSatisfactionSurveyItem> {
  return request<CustomerSatisfactionSurveyItem>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客户满意度调查项目明细
 * @param {string} id 客户满意度调查项目明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerSatisfactionSurveyItemById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客户满意度调查项目明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerSatisfactionSurveyItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客户满意度调查项目明细状态
 * @param {CustomerSatisfactionSurveyItemStatus} dto 状态 DTO
 * @returns {Promise<CustomerSatisfactionSurveyItem>} 客户满意度调查项目明细DTO
 */
export function updateCustomerSatisfactionSurveyItemStatus(dto: CustomerSatisfactionSurveyItemStatus): Promise<CustomerSatisfactionSurveyItem> {
  return request<CustomerSatisfactionSurveyItem>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客户满意度调查项目明细作废状态
 * @param {CustomerSatisfactionSurveyItemObsolete} dto 作废 DTO
 * @returns {Promise<CustomerSatisfactionSurveyItem>} 客户满意度调查项目明细DTO
 */
export function updateCustomerSatisfactionSurveyItemObsolete(dto: CustomerSatisfactionSurveyItemObsolete): Promise<CustomerSatisfactionSurveyItem> {
  return request<CustomerSatisfactionSurveyItem>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客户满意度调查项目明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerSatisfactionSurveyItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
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
export function getCustomerSatisfactionSurveyItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客户满意度调查项目明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerSatisfactionSurveyItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/import`,
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
 * 导出客户满意度调查项目明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerSatisfactionSurveyItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
