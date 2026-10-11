// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowInstance.api.ts
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
  FlowInstance,
  FlowInstanceCreate,
  FlowInstanceEditPayload,
  FlowInstanceStatus,
  FlowInstanceUpdate
} from '@/types/workflow/flowInstance.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowInstances
 */
const FLOW_INSTANCE_API_BASE = 'TaktFlowInstances';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程实例列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowInstance>>} 分页结果
 */
export function getFlowInstanceList(queryDto: any): Promise<TaktPagedResult<FlowInstance>> {
  return request<TaktPagedResult<FlowInstance>>({
    url: `${FLOW_INSTANCE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取流程实例
 * @param {string} id 流程实例ID
 * @returns {Promise<FlowInstance>} 流程实例DTO
 */
export function getFlowInstanceById(id: string): Promise<FlowInstance> {
  return request<FlowInstance>({
    url: `${FLOW_INSTANCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程实例
 * @param {FlowInstanceCreate} dto 创建DTO
 * @returns {Promise<FlowInstance>} 流程实例DTO
 */
export function createFlowInstance(dto: FlowInstanceCreate): Promise<FlowInstance> {
  return request<FlowInstance>({
    url: `${FLOW_INSTANCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程实例
 * @param {string} id 流程实例ID
 * @param {FlowInstanceUpdate | FlowInstanceEditPayload} dto 更新 DTO
 * @returns {Promise<FlowInstance>} 流程实例DTO
 */
export function updateFlowInstance(id: string, dto: FlowInstanceUpdate | FlowInstanceEditPayload): Promise<FlowInstance> {
  return request<FlowInstance>({
    url: `${FLOW_INSTANCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程实例
 * @param {string} id 流程实例ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowInstanceById(id: string): Promise<void> {
  return request({
    url: `${FLOW_INSTANCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程实例
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowInstanceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_INSTANCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新流程实例状态
 * @param {FlowInstanceStatus} dto 状态 DTO（TaktFlowInstanceStatus 枚举）
 * @returns {Promise<FlowInstance>} 流程实例DTO
 */
export function updateFlowInstanceStatus(dto: FlowInstanceStatus): Promise<FlowInstance> {
  return request<FlowInstance>({
    url: `${FLOW_INSTANCE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程实例选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowInstanceOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FLOW_INSTANCE_API_BASE}/options`,
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
export function getFlowInstanceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程实例
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowInstance(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_INSTANCE_API_BASE}/import`,
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
 * 导出流程实例
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowInstance(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_INSTANCE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
