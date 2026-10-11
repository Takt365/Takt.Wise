// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/meeting-center
// 文件名称：meeting.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/meeting-center 模块 API（自动生成，请勿手改路由常量）
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
  Meeting,
  MeetingCreate,
  MeetingStatus,
  MeetingUpdate
} from '@/types/routine/meeting-center/meeting.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMeetings
 */
const MEETING_API_BASE = 'TaktMeetings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议中心列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Meeting>>} 分页结果
 */
export function getMeetingList(queryDto: any): Promise<TaktPagedResult<Meeting>> {
  return request<TaktPagedResult<Meeting>>({
    url: `${MEETING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议中心
 * @param {string} id 会议中心ID
 * @returns {Promise<Meeting>} 会议中心DTO
 */
export function getMeetingById(id: string): Promise<Meeting> {
  return request<Meeting>({
    url: `${MEETING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议中心
 * @param {MeetingCreate} dto 创建DTO
 * @returns {Promise<Meeting>} 会议中心DTO
 */
export function createMeeting(dto: MeetingCreate): Promise<Meeting> {
  return request<Meeting>({
    url: `${MEETING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议中心
 * @param {string} id 会议中心ID
 * @param {MeetingUpdate} dto 更新DTO
 * @returns {Promise<Meeting>} 会议中心DTO
 */
export function updateMeeting(id: string, dto: MeetingUpdate): Promise<Meeting> {
  return request<Meeting>({
    url: `${MEETING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议中心
 * @param {string} id 会议中心ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingById(id: string): Promise<void> {
  return request({
    url: `${MEETING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议中心
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MEETING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会议中心状态
 * @param {MeetingStatus} dto 状态 DTO
 * @returns {Promise<Meeting>} 会议中心DTO
 */
export function updateMeetingStatus(dto: MeetingStatus): Promise<Meeting> {
  return request<Meeting>({
    url: `${MEETING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议中心主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMeetingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MEETING_API_BASE}/options`,
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
export function getMeetingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议中心
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMeeting(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MEETING_API_BASE}/import`,
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
 * 导出会议中心
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMeeting(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
