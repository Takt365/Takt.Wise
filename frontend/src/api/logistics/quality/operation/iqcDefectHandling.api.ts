// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：iqcDefectHandling.api.ts
// 创建时间：2026-07-09
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
  IqcDefectHandling,
  IqcDefectHandlingCreate,
  IqcDefectHandlingObsolete,
  IqcDefectHandlingStatus,
  IqcDefectHandlingUpdate
} from '@/types/logistics/quality/operation/iqcDefectHandling.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktIqcDefectHandlings
 */
const IQC_DEFECT_HANDLING_API_BASE = 'TaktIqcDefectHandlings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取进货检验不良处理记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<IqcDefectHandling>>} 分页结果
 */
export function getIqcDefectHandlingList(queryDto: any): Promise<TaktPagedResult<IqcDefectHandling>> {
  return request<TaktPagedResult<IqcDefectHandling>>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取进货检验不良处理记录
 * @param {string} id 进货检验不良处理记录ID
 * @returns {Promise<IqcDefectHandling>} 进货检验不良处理记录DTO
 */
export function getIqcDefectHandlingById(id: string): Promise<IqcDefectHandling> {
  return request<IqcDefectHandling>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建进货检验不良处理记录
 * @param {IqcDefectHandlingCreate} dto 创建DTO
 * @returns {Promise<IqcDefectHandling>} 进货检验不良处理记录DTO
 */
export function createIqcDefectHandling(dto: IqcDefectHandlingCreate): Promise<IqcDefectHandling> {
  return request<IqcDefectHandling>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新进货检验不良处理记录
 * @param {string} id 进货检验不良处理记录ID
 * @param {IqcDefectHandlingUpdate} dto 更新DTO
 * @returns {Promise<IqcDefectHandling>} 进货检验不良处理记录DTO
 */
export function updateIqcDefectHandling(id: string, dto: IqcDefectHandlingUpdate): Promise<IqcDefectHandling> {
  return request<IqcDefectHandling>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除进货检验不良处理记录
 * @param {string} id 进货检验不良处理记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteIqcDefectHandlingById(id: string): Promise<void> {
  return request({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除进货检验不良处理记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteIqcDefectHandlingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新进货检验不良处理记录状态
 * @param {IqcDefectHandlingStatus} dto 状态 DTO
 * @returns {Promise<IqcDefectHandling>} 进货检验不良处理记录DTO
 */
export function updateIqcDefectHandlingStatus(dto: IqcDefectHandlingStatus): Promise<IqcDefectHandling> {
  return request<IqcDefectHandling>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新进货检验不良处理记录作废状态
 * @param {IqcDefectHandlingObsolete} dto 作废 DTO
 * @returns {Promise<IqcDefectHandling>} 进货检验不良处理记录DTO
 */
export function updateIqcDefectHandlingObsolete(dto: IqcDefectHandlingObsolete): Promise<IqcDefectHandling> {
  return request<IqcDefectHandling>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取进货检验不良处理记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getIqcDefectHandlingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/options`,
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
export function getIqcDefectHandlingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入进货检验不良处理记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importIqcDefectHandling(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/import`,
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
 * 导出进货检验不良处理记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportIqcDefectHandling(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IQC_DEFECT_HANDLING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
