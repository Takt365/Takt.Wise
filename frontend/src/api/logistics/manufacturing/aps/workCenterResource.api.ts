// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：workCenterResource.api.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  WorkCenterResource,
  WorkCenterResourceCreate,
  WorkCenterResourceStatus,
  WorkCenterResourceUpdate
} from '@/types/logistics/manufacturing/aps/workCenterResource.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktWorkCenterResources
 */
const WORK_CENTER_RESOURCE_API_BASE = 'TaktWorkCenterResources';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工作中心资源列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<WorkCenterResource>>} 分页结果
 */
export function getWorkCenterResourceList(queryDto: any): Promise<TaktPagedResult<WorkCenterResource>> {
  return request<TaktPagedResult<WorkCenterResource>>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工作中心资源
 * @param {string} id 工作中心资源ID
 * @returns {Promise<WorkCenterResource>} 工作中心资源DTO
 */
export function getWorkCenterResourceById(id: string): Promise<WorkCenterResource> {
  return request<WorkCenterResource>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工作中心资源
 * @param {WorkCenterResourceCreate} dto 创建DTO
 * @returns {Promise<WorkCenterResource>} 工作中心资源DTO
 */
export function createWorkCenterResource(dto: WorkCenterResourceCreate): Promise<WorkCenterResource> {
  return request<WorkCenterResource>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工作中心资源
 * @param {string} id 工作中心资源ID
 * @param {WorkCenterResourceUpdate} dto 更新DTO
 * @returns {Promise<WorkCenterResource>} 工作中心资源DTO
 */
export function updateWorkCenterResource(id: string, dto: WorkCenterResourceUpdate): Promise<WorkCenterResource> {
  return request<WorkCenterResource>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工作中心资源
 * @param {string} id 工作中心资源ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkCenterResourceById(id: string): Promise<void> {
  return request({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工作中心资源
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkCenterResourceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工作中心资源状态
 * @param {WorkCenterResourceStatus} dto 状态 DTO
 * @returns {Promise<WorkCenterResource>} 工作中心资源DTO
 */
export function updateWorkCenterResourceStatus(dto: WorkCenterResourceStatus): Promise<WorkCenterResource> {
  return request<WorkCenterResource>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工作中心资源选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getWorkCenterResourceOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/options`,
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
export function getWorkCenterResourceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工作中心资源
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importWorkCenterResource(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/import`,
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
 * 导出工作中心资源
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportWorkCenterResource(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_CENTER_RESOURCE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
