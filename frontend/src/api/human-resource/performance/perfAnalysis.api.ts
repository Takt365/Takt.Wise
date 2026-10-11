// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：perfAnalysis.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块 API（自动生成，请勿手改路由常量）
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
  PerfAnalysis,
  PerfAnalysisCreate,
  PerfAnalysisStatus,
  PerfAnalysisUpdate
} from '@/types/human-resource/performance/perfAnalysis.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPerfAnalyses
 */
const PERF_ANALYSIS_API_BASE = 'TaktPerfAnalyses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取分析改进列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PerfAnalysis>>} 分页结果
 */
export function getPerfAnalysisList(queryDto: any): Promise<TaktPagedResult<PerfAnalysis>> {
  return request<TaktPagedResult<PerfAnalysis>>({
    url: `${PERF_ANALYSIS_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取分析改进
 * @param {string} id 分析改进ID
 * @returns {Promise<PerfAnalysis>} 分析改进DTO
 */
export function getPerfAnalysisById(id: string): Promise<PerfAnalysis> {
  return request<PerfAnalysis>({
    url: `${PERF_ANALYSIS_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建分析改进
 * @param {PerfAnalysisCreate} dto 创建DTO
 * @returns {Promise<PerfAnalysis>} 分析改进DTO
 */
export function createPerfAnalysis(dto: PerfAnalysisCreate): Promise<PerfAnalysis> {
  return request<PerfAnalysis>({
    url: `${PERF_ANALYSIS_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新分析改进
 * @param {string} id 分析改进ID
 * @param {PerfAnalysisUpdate} dto 更新DTO
 * @returns {Promise<PerfAnalysis>} 分析改进DTO
 */
export function updatePerfAnalysis(id: string, dto: PerfAnalysisUpdate): Promise<PerfAnalysis> {
  return request<PerfAnalysis>({
    url: `${PERF_ANALYSIS_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除分析改进
 * @param {string} id 分析改进ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfAnalysisById(id: string): Promise<void> {
  return request({
    url: `${PERF_ANALYSIS_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除分析改进
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfAnalysisBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERF_ANALYSIS_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新分析改进状态
 * @param {PerfAnalysisStatus} dto 状态 DTO
 * @returns {Promise<PerfAnalysis>} 分析改进DTO
 */
export function updatePerfAnalysisStatus(dto: PerfAnalysisStatus): Promise<PerfAnalysis> {
  return request<PerfAnalysis>({
    url: `${PERF_ANALYSIS_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取分析改进选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPerfAnalysisOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PERF_ANALYSIS_API_BASE}/options`,
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
export function getPerfAnalysisTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_ANALYSIS_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入分析改进
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPerfAnalysis(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PERF_ANALYSIS_API_BASE}/import`,
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
 * 导出分析改进
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPerfAnalysis(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_ANALYSIS_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
