// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：perfObjective.api.ts
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
  PerfObjective,
  PerfObjectiveCreate,
  PerfObjectiveStatus,
  PerfObjectiveUpdate
} from '@/types/human-resource/performance/perfObjective.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPerfObjectives
 */
const PERF_OBJECTIVE_API_BASE = 'TaktPerfObjectives';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效目标列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PerfObjective>>} 分页结果
 */
export function getPerfObjectiveList(queryDto: any): Promise<TaktPagedResult<PerfObjective>> {
  return request<TaktPagedResult<PerfObjective>>({
    url: `${PERF_OBJECTIVE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取绩效目标
 * @param {string} id 绩效目标ID
 * @returns {Promise<PerfObjective>} 绩效目标DTO
 */
export function getPerfObjectiveById(id: string): Promise<PerfObjective> {
  return request<PerfObjective>({
    url: `${PERF_OBJECTIVE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效目标
 * @param {PerfObjectiveCreate} dto 创建DTO
 * @returns {Promise<PerfObjective>} 绩效目标DTO
 */
export function createPerfObjective(dto: PerfObjectiveCreate): Promise<PerfObjective> {
  return request<PerfObjective>({
    url: `${PERF_OBJECTIVE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效目标
 * @param {string} id 绩效目标ID
 * @param {PerfObjectiveUpdate} dto 更新DTO
 * @returns {Promise<PerfObjective>} 绩效目标DTO
 */
export function updatePerfObjective(id: string, dto: PerfObjectiveUpdate): Promise<PerfObjective> {
  return request<PerfObjective>({
    url: `${PERF_OBJECTIVE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效目标
 * @param {string} id 绩效目标ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfObjectiveById(id: string): Promise<void> {
  return request({
    url: `${PERF_OBJECTIVE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效目标
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfObjectiveBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERF_OBJECTIVE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效目标状态
 * @param {PerfObjectiveStatus} dto 状态 DTO
 * @returns {Promise<PerfObjective>} 绩效目标DTO
 */
export function updatePerfObjectiveStatus(dto: PerfObjectiveStatus): Promise<PerfObjective> {
  return request<PerfObjective>({
    url: `${PERF_OBJECTIVE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效目标选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPerfObjectiveOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PERF_OBJECTIVE_API_BASE}/options`,
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
export function getPerfObjectiveTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_OBJECTIVE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效目标
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPerfObjective(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PERF_OBJECTIVE_API_BASE}/import`,
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
 * 导出绩效目标
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPerfObjective(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_OBJECTIVE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
