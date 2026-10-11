// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：workShift.api.ts
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
  WorkShift,
  WorkShiftCreate,
  WorkShiftSort,
  WorkShiftUpdate
} from '@/types/human-resource/attendance/workShift.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktWorkShifts
 */
const WORK_SHIFT_API_BASE = 'TaktWorkShifts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取班次信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<WorkShift>>} 分页结果
 */
export function getWorkShiftList(queryDto: any): Promise<TaktPagedResult<WorkShift>> {
  return request<TaktPagedResult<WorkShift>>({
    url: `${WORK_SHIFT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取班次信息
 * @param {string} id 班次信息ID
 * @returns {Promise<WorkShift>} 班次信息DTO
 */
export function getWorkShiftById(id: string): Promise<WorkShift> {
  return request<WorkShift>({
    url: `${WORK_SHIFT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建班次信息
 * @param {WorkShiftCreate} dto 创建DTO
 * @returns {Promise<WorkShift>} 班次信息DTO
 */
export function createWorkShift(dto: WorkShiftCreate): Promise<WorkShift> {
  return request<WorkShift>({
    url: `${WORK_SHIFT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新班次信息
 * @param {string} id 班次信息ID
 * @param {WorkShiftUpdate} dto 更新DTO
 * @returns {Promise<WorkShift>} 班次信息DTO
 */
export function updateWorkShift(id: string, dto: WorkShiftUpdate): Promise<WorkShift> {
  return request<WorkShift>({
    url: `${WORK_SHIFT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除班次信息
 * @param {string} id 班次信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkShiftById(id: string): Promise<void> {
  return request({
    url: `${WORK_SHIFT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除班次信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteWorkShiftBatch(ids: string[]): Promise<void> {
  return request({
    url: `${WORK_SHIFT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新班次信息排序
 * @param {WorkShiftSort} dto 排序DTO
 * @returns {Promise<WorkShift>} 班次信息DTO
 */
export function updateWorkShiftSort(dto: WorkShiftSort): Promise<WorkShift> {
  return request<WorkShift>({
    url: `${WORK_SHIFT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取班次信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getWorkShiftOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${WORK_SHIFT_API_BASE}/options`,
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
export function getWorkShiftTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_SHIFT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入班次信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importWorkShift(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${WORK_SHIFT_API_BASE}/import`,
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
 * 导出班次信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportWorkShift(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${WORK_SHIFT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
