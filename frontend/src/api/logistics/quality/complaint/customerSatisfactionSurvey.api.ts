// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerSatisfactionSurvey.api.ts
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
  CustomerSatisfactionSurvey,
  CustomerSatisfactionSurveyCreate,
  CustomerSatisfactionSurveySort,
  CustomerSatisfactionSurveyStatus,
  CustomerSatisfactionSurveyUpdate
} from '@/types/logistics/quality/complaint/customerSatisfactionSurvey.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerSatisfactionSurveys
 */
const CUSTOMER_SATISFACTION_SURVEY_API_BASE = 'TaktCustomerSatisfactionSurveys';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客户满意度调查列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerSatisfactionSurvey>>} 分页结果
 */
export function getCustomerSatisfactionSurveyList(queryDto: any): Promise<TaktPagedResult<CustomerSatisfactionSurvey>> {
  return request<TaktPagedResult<CustomerSatisfactionSurvey>>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客户满意度调查
 * @param {string} id 客户满意度调查ID
 * @returns {Promise<CustomerSatisfactionSurvey>} 客户满意度调查DTO
 */
export function getCustomerSatisfactionSurveyById(id: string): Promise<CustomerSatisfactionSurvey> {
  return request<CustomerSatisfactionSurvey>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建客户满意度调查
 * @param {CustomerSatisfactionSurveyCreate} dto 创建DTO
 * @returns {Promise<CustomerSatisfactionSurvey>} 客户满意度调查DTO
 */
export function createCustomerSatisfactionSurvey(dto: CustomerSatisfactionSurveyCreate): Promise<CustomerSatisfactionSurvey> {
  return request<CustomerSatisfactionSurvey>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客户满意度调查
 * @param {string} id 客户满意度调查ID
 * @param {CustomerSatisfactionSurveyUpdate} dto 更新DTO
 * @returns {Promise<CustomerSatisfactionSurvey>} 客户满意度调查DTO
 */
export function updateCustomerSatisfactionSurvey(id: string, dto: CustomerSatisfactionSurveyUpdate): Promise<CustomerSatisfactionSurvey> {
  return request<CustomerSatisfactionSurvey>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客户满意度调查
 * @param {string} id 客户满意度调查ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerSatisfactionSurveyById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客户满意度调查
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerSatisfactionSurveyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客户满意度调查状态
 * @param {CustomerSatisfactionSurveyStatus} dto 状态 DTO
 * @returns {Promise<CustomerSatisfactionSurvey>} 客户满意度调查DTO
 */
export function updateCustomerSatisfactionSurveyStatus(dto: CustomerSatisfactionSurveyStatus): Promise<CustomerSatisfactionSurvey> {
  return request<CustomerSatisfactionSurvey>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客户满意度调查排序
 * @param {CustomerSatisfactionSurveySort} dto 排序DTO
 * @returns {Promise<CustomerSatisfactionSurvey>} 客户满意度调查DTO
 */
export function updateCustomerSatisfactionSurveySort(dto: CustomerSatisfactionSurveySort): Promise<CustomerSatisfactionSurvey> {
  return request<CustomerSatisfactionSurvey>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客户满意度调查选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerSatisfactionSurveyOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/options`,
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
export function getCustomerSatisfactionSurveyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客户满意度调查
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerSatisfactionSurvey(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/import`,
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
 * 导出客户满意度调查
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerSatisfactionSurvey(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SATISFACTION_SURVEY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
