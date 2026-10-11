// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/meeting-center
// 文件名称：meetingAttendee.api.ts
// 创建时间：2026-06-23
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
  MeetingAttendee,
  MeetingAttendeeCreate,
  MeetingAttendeeStatus,
  MeetingAttendeeUpdate
} from '@/types/routine/meeting-center/meetingAttendee.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMeetingAttendees
 */
const MEETING_ATTENDEE_API_BASE = 'TaktMeetingAttendees';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取参会人员列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MeetingAttendee>>} 分页结果
 */
export function getMeetingAttendeeList(queryDto: any): Promise<TaktPagedResult<MeetingAttendee>> {
  return request<TaktPagedResult<MeetingAttendee>>({
    url: `${MEETING_ATTENDEE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取参会人员
 * @param {string} id 参会人员ID
 * @returns {Promise<MeetingAttendee>} 参会人员DTO
 */
export function getMeetingAttendeeById(id: string): Promise<MeetingAttendee> {
  return request<MeetingAttendee>({
    url: `${MEETING_ATTENDEE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建参会人员
 * @param {MeetingAttendeeCreate} dto 创建DTO
 * @returns {Promise<MeetingAttendee>} 参会人员DTO
 */
export function createMeetingAttendee(dto: MeetingAttendeeCreate): Promise<MeetingAttendee> {
  return request<MeetingAttendee>({
    url: `${MEETING_ATTENDEE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新参会人员
 * @param {string} id 参会人员ID
 * @param {MeetingAttendeeUpdate} dto 更新DTO
 * @returns {Promise<MeetingAttendee>} 参会人员DTO
 */
export function updateMeetingAttendee(id: string, dto: MeetingAttendeeUpdate): Promise<MeetingAttendee> {
  return request<MeetingAttendee>({
    url: `${MEETING_ATTENDEE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除参会人员
 * @param {string} id 参会人员ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingAttendeeById(id: string): Promise<void> {
  return request({
    url: `${MEETING_ATTENDEE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除参会人员
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingAttendeeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MEETING_ATTENDEE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新参会人员状态
 * @param {MeetingAttendeeStatus} dto 状态 DTO
 * @returns {Promise<MeetingAttendee>} 参会人员DTO
 */
export function updateMeetingAttendeeStatus(dto: MeetingAttendeeStatus): Promise<MeetingAttendee> {
  return request<MeetingAttendee>({
    url: `${MEETING_ATTENDEE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取参会人员选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMeetingAttendeeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MEETING_ATTENDEE_API_BASE}/options`,
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
export function getMeetingAttendeeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_ATTENDEE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入参会人员
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMeetingAttendee(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MEETING_ATTENDEE_API_BASE}/import`,
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
 * 导出参会人员
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMeetingAttendee(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_ATTENDEE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
