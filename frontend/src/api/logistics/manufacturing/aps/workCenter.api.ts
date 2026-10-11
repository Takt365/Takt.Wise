// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：workCenter.api.ts
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
  WorkCenter,
  WorkCenterCreate,
  WorkCenterStatus,
  WorkCenterUpdate
} from '@/types/logistics/manufacturing/aps/workCenter.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktWorkCenters
 */
const WORK_CENTER_API_BASE = 'TaktWorkCenters';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工作中心列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<WorkCenter>>} 分页结果
 */
export function getWorkCenterList(queryDto: any): Promise<TaktPagedResult<WorkCenter>> {
  return request<TaktPagedResult<WorkCenter>>({
    url: `${WORK_CENTER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工作中心
 * @param {string} id 工作中心ID
 * @returns {Promise<WorkCenter>} 工作中心DTO
 */
export function getWorkCenterById(id: string): Promise<WorkCenter> {
  return request<WorkCenter>({
    url: `${WORK_CENTER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工作中心
 * @param {WorkCenterCreate} dto 创建DTO
 * @returns {Promise<WorkCenter>} 工作中心DTO
 */
export function createWorkCenter(dto: WorkCenterCreate): Promise<WorkCenter> {
  return request<WorkCenter>({
    url: `${WORK_CENTER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工作中心
 * @param {string} id 工作中心ID
 * @param {WorkCenterUpdate} dto 更新DTO
 * @returns {Promise<WorkCenter>} 工作中心DTO
 */
export function updateWorkCenter(id: string, dto: WorkCenterUpdate): Promise<WorkCenter> {
  return request<WorkCenter>({
    url: `${WORK_CENTER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工作中心
 * @param {string} id 工作中心ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkCenterById(id: string): Promise<void> {
  return request({
    url: `${WORK_CENTER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工作中心
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkCenterBatch(ids: string[]): Promise<void> {
  return request({
    url: `${WORK_CENTER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工作中心状态
 * @param {WorkCenterStatus} dto 状态 DTO
 * @returns {Promise<WorkCenter>} 工作中心DTO
 */
export function updateWorkCenterStatus(dto: WorkCenterStatus): Promise<WorkCenter> {
  return request<WorkCenter>({
    url: `${WORK_CENTER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工作中心选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getWorkCenterOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${WORK_CENTER_API_BASE}/options`,
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
export function getWorkCenterTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_CENTER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工作中心
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importWorkCenter(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${WORK_CENTER_API_BASE}/import`,
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
 * 导出工作中心
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportWorkCenter(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_CENTER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
