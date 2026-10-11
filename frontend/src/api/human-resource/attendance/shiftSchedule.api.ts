// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：shiftSchedule.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块 API（自动生成，请勿手改路由常量）
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
  ShiftSchedule,
  ShiftScheduleCreate,
  ShiftScheduleUpdate
} from '@/types/human-resource/attendance/shiftSchedule.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktShiftSchedules
 */
const SHIFT_SCHEDULE_API_BASE = 'TaktShiftSchedules';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取排班信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ShiftSchedule>>} 分页结果
 */
export function getShiftScheduleList(queryDto: any): Promise<TaktPagedResult<ShiftSchedule>> {
  return request<TaktPagedResult<ShiftSchedule>>({
    url: `${SHIFT_SCHEDULE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取排班信息
 * @param {string} id 排班信息ID
 * @returns {Promise<ShiftSchedule>} 排班信息DTO
 */
export function getShiftScheduleById(id: string): Promise<ShiftSchedule> {
  return request<ShiftSchedule>({
    url: `${SHIFT_SCHEDULE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建排班信息
 * @param {ShiftScheduleCreate} dto 创建DTO
 * @returns {Promise<ShiftSchedule>} 排班信息DTO
 */
export function createShiftSchedule(dto: ShiftScheduleCreate): Promise<ShiftSchedule> {
  return request<ShiftSchedule>({
    url: `${SHIFT_SCHEDULE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新排班信息
 * @param {string} id 排班信息ID
 * @param {ShiftScheduleUpdate} dto 更新DTO
 * @returns {Promise<ShiftSchedule>} 排班信息DTO
 */
export function updateShiftSchedule(id: string, dto: ShiftScheduleUpdate): Promise<ShiftSchedule> {
  return request<ShiftSchedule>({
    url: `${SHIFT_SCHEDULE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除排班信息
 * @param {string} id 排班信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteShiftScheduleById(id: string): Promise<void> {
  return request({
    url: `${SHIFT_SCHEDULE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除排班信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteShiftScheduleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SHIFT_SCHEDULE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取排班信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getShiftScheduleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SHIFT_SCHEDULE_API_BASE}/options`,
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
export function getShiftScheduleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SHIFT_SCHEDULE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入排班信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importShiftSchedule(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SHIFT_SCHEDULE_API_BASE}/import`,
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
 * 导出排班信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportShiftSchedule(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SHIFT_SCHEDULE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
