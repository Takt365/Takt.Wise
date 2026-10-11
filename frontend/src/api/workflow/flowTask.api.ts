// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowTask.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块 API（自动生成，请勿手改路由常量）
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
  FlowTask,
  FlowTaskCreate,
  FlowTaskSort,
  FlowTaskStatus,
  FlowTaskUpdate
} from '@/types/workflow/flowTask.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowTasks
 */
const FLOW_TASK_API_BASE = 'TaktFlowTasks';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程用户任务列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowTask>>} 分页结果
 */
export function getFlowTaskList(queryDto: any): Promise<TaktPagedResult<FlowTask>> {
  return request<TaktPagedResult<FlowTask>>({
    url: `${FLOW_TASK_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取流程用户任务
 * @param {string} id 流程用户任务ID
 * @returns {Promise<FlowTask>} 流程用户任务DTO
 */
export function getFlowTaskById(id: string): Promise<FlowTask> {
  return request<FlowTask>({
    url: `${FLOW_TASK_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程用户任务
 * @param {FlowTaskCreate} dto 创建DTO
 * @returns {Promise<FlowTask>} 流程用户任务DTO
 */
export function createFlowTask(dto: FlowTaskCreate): Promise<FlowTask> {
  return request<FlowTask>({
    url: `${FLOW_TASK_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程用户任务
 * @param {string} id 流程用户任务ID
 * @param {FlowTaskUpdate} dto 更新DTO
 * @returns {Promise<FlowTask>} 流程用户任务DTO
 */
export function updateFlowTask(id: string, dto: FlowTaskUpdate): Promise<FlowTask> {
  return request<FlowTask>({
    url: `${FLOW_TASK_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程用户任务
 * @param {string} id 流程用户任务ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowTaskById(id: string): Promise<void> {
  return request({
    url: `${FLOW_TASK_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程用户任务
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowTaskBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_TASK_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新流程用户任务状态
 * @param {FlowTaskStatus} dto 状态 DTO（TaktFlowTaskStatus 枚举）
 * @returns {Promise<FlowTask>} 流程用户任务DTO
 */
export function updateFlowTaskStatus(dto: FlowTaskStatus): Promise<FlowTask> {
  return request<FlowTask>({
    url: `${FLOW_TASK_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新流程用户任务排序
 * @param {FlowTaskSort} dto 排序DTO
 * @returns {Promise<FlowTask>} 流程用户任务DTO
 */
export function updateFlowTaskSort(dto: FlowTaskSort): Promise<FlowTask> {
  return request<FlowTask>({
    url: `${FLOW_TASK_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程用户任务选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowTaskOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FLOW_TASK_API_BASE}/options`,
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
export function getFlowTaskTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_TASK_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程用户任务
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowTask(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_TASK_API_BASE}/import`,
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
 * 导出流程用户任务
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowTask(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_TASK_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
