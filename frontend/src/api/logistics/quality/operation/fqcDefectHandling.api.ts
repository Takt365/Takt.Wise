// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：fqcDefectHandling.api.ts
// 创建时间：2026-07-23
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
  FqcDefectHandling,
  FqcDefectHandlingCreate,
  FqcDefectHandlingObsolete,
  FqcDefectHandlingStatus,
  FqcDefectHandlingUpdate
} from '@/types/logistics/quality/operation/fqcDefectHandling.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFqcDefectHandlings
 */
const FQC_DEFECT_HANDLING_API_BASE = 'TaktFqcDefectHandlings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取出货检验不良处理记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FqcDefectHandling>>} 分页结果
 */
export function getFqcDefectHandlingList(queryDto: any): Promise<TaktPagedResult<FqcDefectHandling>> {
  return request<TaktPagedResult<FqcDefectHandling>>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取出货检验不良处理记录
 * @param {string} id 出货检验不良处理记录ID
 * @returns {Promise<FqcDefectHandling>} 出货检验不良处理记录DTO
 */
export function getFqcDefectHandlingById(id: string): Promise<FqcDefectHandling> {
  return request<FqcDefectHandling>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建出货检验不良处理记录
 * @param {FqcDefectHandlingCreate} dto 创建DTO
 * @returns {Promise<FqcDefectHandling>} 出货检验不良处理记录DTO
 */
export function createFqcDefectHandling(dto: FqcDefectHandlingCreate): Promise<FqcDefectHandling> {
  return request<FqcDefectHandling>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新出货检验不良处理记录
 * @param {string} id 出货检验不良处理记录ID
 * @param {FqcDefectHandlingUpdate} dto 更新DTO
 * @returns {Promise<FqcDefectHandling>} 出货检验不良处理记录DTO
 */
export function updateFqcDefectHandling(id: string, dto: FqcDefectHandlingUpdate): Promise<FqcDefectHandling> {
  return request<FqcDefectHandling>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除出货检验不良处理记录
 * @param {string} id 出货检验不良处理记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcDefectHandlingById(id: string): Promise<void> {
  return request({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除出货检验不良处理记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcDefectHandlingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新出货检验不良处理记录状态
 * @param {FqcDefectHandlingStatus} dto 状态 DTO
 * @returns {Promise<FqcDefectHandling>} 出货检验不良处理记录DTO
 */
export function updateFqcDefectHandlingStatus(dto: FqcDefectHandlingStatus): Promise<FqcDefectHandling> {
  return request<FqcDefectHandling>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新出货检验不良处理记录作废状态
 * @param {FqcDefectHandlingObsolete} dto 作废 DTO
 * @returns {Promise<FqcDefectHandling>} 出货检验不良处理记录DTO
 */
export function updateFqcDefectHandlingObsolete(dto: FqcDefectHandlingObsolete): Promise<FqcDefectHandling> {
  return request<FqcDefectHandling>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取出货检验不良处理记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFqcDefectHandlingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/options`,
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
export function getFqcDefectHandlingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入出货检验不良处理记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFqcDefectHandling(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/import`,
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
 * 导出出货检验不良处理记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFqcDefectHandling(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_DEFECT_HANDLING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
