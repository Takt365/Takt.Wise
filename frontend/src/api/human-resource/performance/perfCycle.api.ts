// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/performance
// 文件名称：perfCycle.api.ts
// 创建时间：2026-06-24
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
  PerfCycle,
  PerfCycleCreate,
  PerfCycleStatus,
  PerfCycleUpdate
} from '@/types/human-resource/performance/perfCycle.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPerfCycles
 */
const PERF_CYCLE_API_BASE = 'TaktPerfCycles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取绩效周期日程列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PerfCycle>>} 分页结果
 */
export function getPerfCycleList(queryDto: any): Promise<TaktPagedResult<PerfCycle>> {
  return request<TaktPagedResult<PerfCycle>>({
    url: `${PERF_CYCLE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @returns {Promise<PerfCycle>} 绩效周期日程DTO
 */
export function getPerfCycleById(id: string): Promise<PerfCycle> {
  return request<PerfCycle>({
    url: `${PERF_CYCLE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建绩效周期日程
 * @param {PerfCycleCreate} dto 创建DTO
 * @returns {Promise<PerfCycle>} 绩效周期日程DTO
 */
export function createPerfCycle(dto: PerfCycleCreate): Promise<PerfCycle> {
  return request<PerfCycle>({
    url: `${PERF_CYCLE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @param {PerfCycleUpdate} dto 更新DTO
 * @returns {Promise<PerfCycle>} 绩效周期日程DTO
 */
export function updatePerfCycle(id: string, dto: PerfCycleUpdate): Promise<PerfCycle> {
  return request<PerfCycle>({
    url: `${PERF_CYCLE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除绩效周期日程
 * @param {string} id 绩效周期日程ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfCycleById(id: string): Promise<void> {
  return request({
    url: `${PERF_CYCLE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除绩效周期日程
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePerfCycleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PERF_CYCLE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新绩效周期日程状态
 * @param {PerfCycleStatus} dto 状态 DTO
 * @returns {Promise<PerfCycle>} 绩效周期日程DTO
 */
export function updatePerfCycleStatus(dto: PerfCycleStatus): Promise<PerfCycle> {
  return request<PerfCycle>({
    url: `${PERF_CYCLE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取绩效周期日程选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPerfCycleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PERF_CYCLE_API_BASE}/options`,
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
export function getPerfCycleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_CYCLE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入绩效周期日程
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPerfCycle(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PERF_CYCLE_API_BASE}/import`,
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
 * 导出绩效周期日程
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPerfCycle(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PERF_CYCLE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
