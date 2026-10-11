// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/labor-hour
// 文件名称：assyLaborHour.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/labor-hour 模块 API（自动生成，请勿手改路由常量）
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
  AssyLaborHour,
  AssyLaborHourCreate,
  AssyLaborHourUpdate
} from '@/types/logistics/manufacturing/labor-hour/assyLaborHour.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssyLaborHours
 */
const ASSY_LABOR_HOUR_API_BASE = 'TaktAssyLaborHours';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取组立工数统计列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AssyLaborHour>>} 分页结果
 */
export function getAssyLaborHourList(queryDto: any): Promise<TaktPagedResult<AssyLaborHour>> {
  return request<TaktPagedResult<AssyLaborHour>>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取组立工数统计
 * @param {string} id 组立工数统计ID
 * @returns {Promise<AssyLaborHour>} 组立工数统计DTO
 */
export function getAssyLaborHourById(id: string): Promise<AssyLaborHour> {
  return request<AssyLaborHour>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建组立工数统计
 * @param {AssyLaborHourCreate} dto 创建DTO
 * @returns {Promise<AssyLaborHour>} 组立工数统计DTO
 */
export function createAssyLaborHour(dto: AssyLaborHourCreate): Promise<AssyLaborHour> {
  return request<AssyLaborHour>({
    url: `${ASSY_LABOR_HOUR_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新组立工数统计
 * @param {string} id 组立工数统计ID
 * @param {AssyLaborHourUpdate} dto 更新DTO
 * @returns {Promise<AssyLaborHour>} 组立工数统计DTO
 */
export function updateAssyLaborHour(id: string, dto: AssyLaborHourUpdate): Promise<AssyLaborHour> {
  return request<AssyLaborHour>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除组立工数统计
 * @param {string} id 组立工数统计ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyLaborHourById(id: string): Promise<void> {
  return request({
    url: `${ASSY_LABOR_HOUR_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除组立工数统计
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyLaborHourBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSY_LABOR_HOUR_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取组立工数统计选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssyLaborHourOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/options`,
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
export function getAssyLaborHourTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入组立工数统计
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAssyLaborHour(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSY_LABOR_HOUR_API_BASE}/import`,
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
 * 导出组立工数统计
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssyLaborHour(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_LABOR_HOUR_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
