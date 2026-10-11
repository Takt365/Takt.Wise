// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：perfScheme.api.ts
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
  PerfScheme,
  PerfSchemeCreate,
  PerfSchemeSort,
  PerfSchemeStatus,
  PerfSchemeUpdate
} from '@/types/human-resource/performance/perfScheme.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPerfSchemes
 */
const PERF_SCHEME_API_BASE = 'TaktPerfSchemes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效方案指标列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PerfScheme>>} 分页结果
 */
export function getPerfSchemeList(queryDto: any): Promise<TaktPagedResult<PerfScheme>> {
  return request<TaktPagedResult<PerfScheme>>({
    url: `${PERF_SCHEME_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @returns {Promise<PerfScheme>} 绩效方案指标DTO
 */
export function getPerfSchemeById(id: string): Promise<PerfScheme> {
  return request<PerfScheme>({
    url: `${PERF_SCHEME_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效方案指标
 * @param {PerfSchemeCreate} dto 创建DTO
 * @returns {Promise<PerfScheme>} 绩效方案指标DTO
 */
export function createPerfScheme(dto: PerfSchemeCreate): Promise<PerfScheme> {
  return request<PerfScheme>({
    url: `${PERF_SCHEME_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @param {PerfSchemeUpdate} dto 更新DTO
 * @returns {Promise<PerfScheme>} 绩效方案指标DTO
 */
export function updatePerfScheme(id: string, dto: PerfSchemeUpdate): Promise<PerfScheme> {
  return request<PerfScheme>({
    url: `${PERF_SCHEME_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效方案指标
 * @param {string} id 绩效方案指标ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfSchemeById(id: string): Promise<void> {
  return request({
    url: `${PERF_SCHEME_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效方案指标
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfSchemeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERF_SCHEME_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效方案指标状态
 * @param {PerfSchemeStatus} dto 状态 DTO
 * @returns {Promise<PerfScheme>} 绩效方案指标DTO
 */
export function updatePerfSchemeStatus(dto: PerfSchemeStatus): Promise<PerfScheme> {
  return request<PerfScheme>({
    url: `${PERF_SCHEME_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新绩效方案指标排序
 * @param {PerfSchemeSort} dto 排序DTO
 * @returns {Promise<PerfScheme>} 绩效方案指标DTO
 */
export function updatePerfSchemeSort(dto: PerfSchemeSort): Promise<PerfScheme> {
  return request<PerfScheme>({
    url: `${PERF_SCHEME_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效方案指标选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPerfSchemeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PERF_SCHEME_API_BASE}/options`,
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
export function getPerfSchemeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_SCHEME_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效方案指标
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPerfScheme(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PERF_SCHEME_API_BASE}/import`,
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
 * 导出绩效方案指标
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPerfScheme(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_SCHEME_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
