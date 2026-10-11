// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：apsSchedule.api.ts
// 创建时间：2026-07-09
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
  ApsSchedule,
  ApsScheduleCreate,
  ApsScheduleStatus,
  ApsScheduleUpdate
} from '@/types/logistics/manufacturing/aps/apsSchedule.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktApsSchedules
 */
const APS_SCHEDULE_API_BASE = 'TaktApsSchedules';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取APS排程主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ApsSchedule>>} 分页结果
 */
export function getApsScheduleList(queryDto: any): Promise<TaktPagedResult<ApsSchedule>> {
  return request<TaktPagedResult<ApsSchedule>>({
    url: `${APS_SCHEDULE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取APS排程主
 * @param {string} id APS排程主ID
 * @returns {Promise<ApsSchedule>} APS排程主DTO
 */
export function getApsScheduleById(id: string): Promise<ApsSchedule> {
  return request<ApsSchedule>({
    url: `${APS_SCHEDULE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建APS排程主
 * @param {ApsScheduleCreate} dto 创建DTO
 * @returns {Promise<ApsSchedule>} APS排程主DTO
 */
export function createApsSchedule(dto: ApsScheduleCreate): Promise<ApsSchedule> {
  return request<ApsSchedule>({
    url: `${APS_SCHEDULE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新APS排程主
 * @param {string} id APS排程主ID
 * @param {ApsScheduleUpdate} dto 更新DTO
 * @returns {Promise<ApsSchedule>} APS排程主DTO
 */
export function updateApsSchedule(id: string, dto: ApsScheduleUpdate): Promise<ApsSchedule> {
  return request<ApsSchedule>({
    url: `${APS_SCHEDULE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除APS排程主
 * @param {string} id APS排程主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleById(id: string): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除APS排程主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新APS排程主状态
 * @param {ApsScheduleStatus} dto 状态 DTO
 * @returns {Promise<ApsSchedule>} APS排程主DTO
 */
export function updateApsScheduleStatus(dto: ApsScheduleStatus): Promise<ApsSchedule> {
  return request<ApsSchedule>({
    url: `${APS_SCHEDULE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取APS排程主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getApsScheduleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${APS_SCHEDULE_API_BASE}/options`,
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
export function getApsScheduleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${APS_SCHEDULE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入APS排程主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importApsSchedule(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${APS_SCHEDULE_API_BASE}/import`,
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
 * 导出APS排程主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportApsSchedule(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${APS_SCHEDULE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
