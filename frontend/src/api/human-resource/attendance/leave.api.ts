// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：leave.api.ts
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
  Leave,
  LeaveCreate,
  LeaveStatus,
  LeaveUpdate
} from '@/types/human-resource/attendance/leave.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktLeaves
 */
const LEAVE_API_BASE = 'TaktLeaves';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取请假信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Leave>>} 分页结果
 */
export function getLeaveList(queryDto: any): Promise<TaktPagedResult<Leave>> {
  return request<TaktPagedResult<Leave>>({
    url: `${LEAVE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取请假信息
 * @param {string} id 请假信息ID
 * @returns {Promise<Leave>} 请假信息DTO
 */
export function getLeaveById(id: string): Promise<Leave> {
  return request<Leave>({
    url: `${LEAVE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建请假信息
 * @param {LeaveCreate} dto 创建DTO
 * @returns {Promise<Leave>} 请假信息DTO
 */
export function createLeave(dto: LeaveCreate): Promise<Leave> {
  return request<Leave>({
    url: `${LEAVE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新请假信息
 * @param {string} id 请假信息ID
 * @param {LeaveUpdate} dto 更新DTO
 * @returns {Promise<Leave>} 请假信息DTO
 */
export function updateLeave(id: string, dto: LeaveUpdate): Promise<Leave> {
  return request<Leave>({
    url: `${LEAVE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除请假信息
 * @param {string} id 请假信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteLeaveById(id: string): Promise<void> {
  return request({
    url: `${LEAVE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除请假信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteLeaveBatch(ids: string[]): Promise<void> {
  return request({
    url: `${LEAVE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新请假信息状态
 * @param {LeaveStatus} dto 状态 DTO
 * @returns {Promise<Leave>} 请假信息DTO
 */
export function updateLeaveStatus(dto: LeaveStatus): Promise<Leave> {
  return request<Leave>({
    url: `${LEAVE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取请假信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getLeaveOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${LEAVE_API_BASE}/options`,
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
export function getLeaveTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${LEAVE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入请假信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importLeave(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${LEAVE_API_BASE}/import`,
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
 * 导出请假信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportLeave(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${LEAVE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
