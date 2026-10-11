// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：quartzTask.api.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  QuartzTask,
  QuartzTaskCreate,
  QuartzTaskStatus,
  QuartzTaskUpdate
} from '@/types/foundation/quartzTask.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQuartzTasks
 */
const QUARTZ_TASK_API_BASE = 'TaktQuartzTasks';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取定时任务列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QuartzTask>>} 分页结果
 */
export function getQuartzTaskList(queryDto: any): Promise<TaktPagedResult<QuartzTask>> {
  return request<TaktPagedResult<QuartzTask>>({
    url: `${QUARTZ_TASK_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取定时任务
 * @param {string} id 定时任务ID
 * @returns {Promise<QuartzTask>} 定时任务DTO
 */
export function getQuartzTaskById(id: string): Promise<QuartzTask> {
  return request<QuartzTask>({
    url: `${QUARTZ_TASK_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建定时任务
 * @param {QuartzTaskCreate} dto 创建DTO
 * @returns {Promise<QuartzTask>} 定时任务DTO
 */
export function createQuartzTask(dto: QuartzTaskCreate): Promise<QuartzTask> {
  return request<QuartzTask>({
    url: `${QUARTZ_TASK_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新定时任务
 * @param {string} id 定时任务ID
 * @param {QuartzTaskUpdate} dto 更新DTO
 * @returns {Promise<QuartzTask>} 定时任务DTO
 */
export function updateQuartzTask(id: string, dto: QuartzTaskUpdate): Promise<QuartzTask> {
  return request<QuartzTask>({
    url: `${QUARTZ_TASK_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除定时任务
 * @param {string} id 定时任务ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQuartzTaskById(id: string): Promise<void> {
  return request({
    url: `${QUARTZ_TASK_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除定时任务
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQuartzTaskBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUARTZ_TASK_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新定时任务状态
 * @param {QuartzTaskStatus} dto 状态 DTO
 * @returns {Promise<QuartzTask>} 定时任务DTO
 */
export function updateQuartzTaskStatus(dto: QuartzTaskStatus): Promise<QuartzTask> {
  return request<QuartzTask>({
    url: `${QUARTZ_TASK_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 立即执行一次定时任务
 * @param {string} id 定时任务ID
 * @param {string} [executeParams] 本次执行参数（非空则覆盖任务配置）
 * @returns {Promise<QuartzTask>} 定时任务DTO
 */
export function executeQuartzTaskNow(id: string, executeParams?: string): Promise<QuartzTask> {
  return request<QuartzTask>({
    url: `${QUARTZ_TASK_API_BASE}/${id}/execute`,
    method: 'post',
    data: executeParams != null && executeParams !== '' ? { executeParams } : undefined,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取定时任务选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQuartzTaskOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${QUARTZ_TASK_API_BASE}/options`,
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
export function getQuartzTaskTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUARTZ_TASK_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入定时任务
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQuartzTask(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUARTZ_TASK_API_BASE}/import`,
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
 * 导出定时任务
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQuartzTask(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUARTZ_TASK_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
