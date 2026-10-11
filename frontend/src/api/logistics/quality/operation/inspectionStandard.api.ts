// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：inspectionStandard.api.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  InspectionStandard,
  InspectionStandardCreate,
  InspectionStandardStatus,
  InspectionStandardUpdate
} from '@/types/logistics/quality/operation/inspectionStandard.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktInspectionStandards
 */
const INSPECTION_STANDARD_API_BASE = 'TaktInspectionStandards';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取检验标准列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<InspectionStandard>>} 分页结果
 */
export function getInspectionStandardList(queryDto: any): Promise<TaktPagedResult<InspectionStandard>> {
  return request<TaktPagedResult<InspectionStandard>>({
    url: `${INSPECTION_STANDARD_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取检验标准
 * @param {string} id 检验标准ID
 * @returns {Promise<InspectionStandard>} 检验标准DTO
 */
export function getInspectionStandardById(id: string): Promise<InspectionStandard> {
  return request<InspectionStandard>({
    url: `${INSPECTION_STANDARD_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建检验标准
 * @param {InspectionStandardCreate} dto 创建DTO
 * @returns {Promise<InspectionStandard>} 检验标准DTO
 */
export function createInspectionStandard(dto: InspectionStandardCreate): Promise<InspectionStandard> {
  return request<InspectionStandard>({
    url: `${INSPECTION_STANDARD_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新检验标准
 * @param {string} id 检验标准ID
 * @param {InspectionStandardUpdate} dto 更新DTO
 * @returns {Promise<InspectionStandard>} 检验标准DTO
 */
export function updateInspectionStandard(id: string, dto: InspectionStandardUpdate): Promise<InspectionStandard> {
  return request<InspectionStandard>({
    url: `${INSPECTION_STANDARD_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除检验标准
 * @param {string} id 检验标准ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteInspectionStandardById(id: string): Promise<void> {
  return request({
    url: `${INSPECTION_STANDARD_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除检验标准
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteInspectionStandardBatch(ids: string[]): Promise<void> {
  return request({
    url: `${INSPECTION_STANDARD_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新检验标准状态
 * @param {InspectionStandardStatus} dto 状态 DTO
 * @returns {Promise<InspectionStandard>} 检验标准DTO
 */
export function updateInspectionStandardStatus(dto: InspectionStandardStatus): Promise<InspectionStandard> {
  return request<InspectionStandard>({
    url: `${INSPECTION_STANDARD_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取检验标准选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getInspectionStandardOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${INSPECTION_STANDARD_API_BASE}/options`,
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
export function getInspectionStandardTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${INSPECTION_STANDARD_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入检验标准
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importInspectionStandard(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${INSPECTION_STANDARD_API_BASE}/import`,
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
 * 导出检验标准
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportInspectionStandard(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${INSPECTION_STANDARD_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
