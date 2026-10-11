// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：userPreference.api.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
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
  UserPreference,
  UserPreferenceCreate,
  UserPreferenceStatus,
  UserPreferenceUpdate
} from '@/types/identity/userPreference.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktUserPreferences
 */
const USER_PREFERENCE_API_BASE = 'TaktUserPreferences';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取用户个人偏好列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<UserPreference>>} 分页结果
 */
export function getUserPreferenceList(queryDto: any): Promise<TaktPagedResult<UserPreference>> {
  return request<TaktPagedResult<UserPreference>>({
    url: `${USER_PREFERENCE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取用户个人偏好
 * @param {string} id 用户个人偏好ID
 * @returns {Promise<UserPreference>} 用户个人偏好DTO
 */
export function getUserPreferenceById(id: string): Promise<UserPreference> {
  return request<UserPreference>({
    url: `${USER_PREFERENCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取当前登录用户个人偏好（无记录时为 null）
 * @returns {Promise<UserPreference | null>} 偏好 DTO
 */
export function getUserPreferenceMe(): Promise<UserPreference | null> {
  return request<UserPreference | null>({
    url: `${USER_PREFERENCE_API_BASE}/me`,
    method: 'get',
  });
}

/**
 * Upsert 当前登录用户个人偏好
 * @param preferenceJson AppSetting JSON 字符串
 * @returns {Promise<UserPreference>} 偏好 DTO
 */
export function updateUserPreferenceMe(preferenceJson: string): Promise<UserPreference> {
  return request<UserPreference>({
    url: `${USER_PREFERENCE_API_BASE}/me`,
    method: 'put',
    data: { preferenceJson },
  });
}

/**
 * 创建用户个人偏好
 * @param {UserPreferenceCreate} dto 创建DTO
 * @returns {Promise<UserPreference>} 用户个人偏好DTO
 */
export function createUserPreference(dto: UserPreferenceCreate): Promise<UserPreference> {
  return request<UserPreference>({
    url: `${USER_PREFERENCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新用户个人偏好
 * @param {string} id 用户个人偏好ID
 * @param {UserPreferenceUpdate} dto 更新DTO
 * @returns {Promise<UserPreference>} 用户个人偏好DTO
 */
export function updateUserPreference(id: string, dto: UserPreferenceUpdate): Promise<UserPreference> {
  return request<UserPreference>({
    url: `${USER_PREFERENCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除用户个人偏好
 * @param {string} id 用户个人偏好ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteUserPreferenceById(id: string): Promise<void> {
  return request({
    url: `${USER_PREFERENCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除用户个人偏好
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteUserPreferenceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${USER_PREFERENCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新用户个人偏好状态
 * @param {UserPreferenceStatus} dto 状态 DTO
 * @returns {Promise<UserPreference>} 用户个人偏好DTO
 */
export function updateUserPreferenceStatus(dto: UserPreferenceStatus): Promise<UserPreference> {
  return request<UserPreference>({
    url: `${USER_PREFERENCE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取用户个人偏好选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getUserPreferenceOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${USER_PREFERENCE_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getUserPreferenceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${USER_PREFERENCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入用户个人偏好
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importUserPreference(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${USER_PREFERENCE_API_BASE}/import`,
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
 * 导出用户个人偏好
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportUserPreference(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${USER_PREFERENCE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
