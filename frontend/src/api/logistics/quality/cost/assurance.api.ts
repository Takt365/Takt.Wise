// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：assurance.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块 API（自动生成，请勿手改路由常量）
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
  QualityAssurance,
  QualityAssuranceCreate,
  QualityAssuranceUpdate
} from '@/types/logistics/quality/cost/assurance.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityAssurances
 */
const QUALITY_ASSURANCE_API_BASE = 'TaktQualityAssurances';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取品质业务主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityAssurance>>} 分页结果
 */
export function getQualityAssuranceList(queryDto: any): Promise<TaktPagedResult<QualityAssurance>> {
  return request<TaktPagedResult<QualityAssurance>>({
    url: `${QUALITY_ASSURANCE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取品质业务主
 * @param {string} id 品质业务主ID
 * @returns {Promise<QualityAssurance>} 品质业务主DTO
 */
export function getQualityAssuranceById(id: string): Promise<QualityAssurance> {
  return request<QualityAssurance>({
    url: `${QUALITY_ASSURANCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建品质业务主
 * @param {QualityAssuranceCreate} dto 创建DTO
 * @returns {Promise<QualityAssurance>} 品质业务主DTO
 */
export function createQualityAssurance(dto: QualityAssuranceCreate): Promise<QualityAssurance> {
  return request<QualityAssurance>({
    url: `${QUALITY_ASSURANCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品质业务主
 * @param {string} id 品质业务主ID
 * @param {QualityAssuranceUpdate} dto 更新DTO
 * @returns {Promise<QualityAssurance>} 品质业务主DTO
 */
export function updateQualityAssurance(id: string, dto: QualityAssuranceUpdate): Promise<QualityAssurance> {
  return request<QualityAssurance>({
    url: `${QUALITY_ASSURANCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除品质业务主
 * @param {string} id 品质业务主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityAssuranceById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_ASSURANCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除品质业务主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityAssuranceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_ASSURANCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取品质业务主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityAssuranceOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${QUALITY_ASSURANCE_API_BASE}/options`,
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
export function getQualityAssuranceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ASSURANCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入品质业务主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityAssurance(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_ASSURANCE_API_BASE}/import`,
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
 * 导出品质业务主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityAssurance(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ASSURANCE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
