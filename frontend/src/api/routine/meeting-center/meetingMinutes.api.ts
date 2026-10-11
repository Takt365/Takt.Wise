// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/meeting-center
// 文件名称：meetingMinutes.api.ts
// 创建时间：2026-07-09
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
  MeetingMinutes,
  MeetingMinutesCreate,
  MeetingMinutesObsolete,
  MeetingMinutesUpdate
} from '@/types/routine/meeting-center/meetingMinutes.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMeetingMinutes
 */
const MEETING_MINUTES_API_BASE = 'TaktMeetingMinutes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会后纪要列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MeetingMinutes>>} 分页结果
 */
export function getMeetingMinutesList(queryDto: any): Promise<TaktPagedResult<MeetingMinutes>> {
  return request<TaktPagedResult<MeetingMinutes>>({
    url: `${MEETING_MINUTES_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会后纪要
 * @param {string} id 会后纪要ID
 * @returns {Promise<MeetingMinutes>} 会后纪要DTO
 */
export function getMeetingMinutesById(id: string): Promise<MeetingMinutes> {
  return request<MeetingMinutes>({
    url: `${MEETING_MINUTES_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会后纪要
 * @param {MeetingMinutesCreate} dto 创建DTO
 * @returns {Promise<MeetingMinutes>} 会后纪要DTO
 */
export function createMeetingMinutes(dto: MeetingMinutesCreate): Promise<MeetingMinutes> {
  return request<MeetingMinutes>({
    url: `${MEETING_MINUTES_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会后纪要
 * @param {string} id 会后纪要ID
 * @param {MeetingMinutesUpdate} dto 更新DTO
 * @returns {Promise<MeetingMinutes>} 会后纪要DTO
 */
export function updateMeetingMinutes(id: string, dto: MeetingMinutesUpdate): Promise<MeetingMinutes> {
  return request<MeetingMinutes>({
    url: `${MEETING_MINUTES_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会后纪要
 * @param {string} id 会后纪要ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingMinutesById(id: string): Promise<void> {
  return request({
    url: `${MEETING_MINUTES_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会后纪要
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingMinutesBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MEETING_MINUTES_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会后纪要作废状态
 * @param {MeetingMinutesObsolete} dto 作废 DTO
 * @returns {Promise<MeetingMinutes>} 会后纪要DTO
 */
export function updateMeetingMinutesObsolete(dto: MeetingMinutesObsolete): Promise<MeetingMinutes> {
  return request<MeetingMinutes>({
    url: `${MEETING_MINUTES_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会后纪要选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMeetingMinutesOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MEETING_MINUTES_API_BASE}/options`,
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
export function getMeetingMinutesTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_MINUTES_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会后纪要
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMeetingMinutes(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MEETING_MINUTES_API_BASE}/import`,
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
 * 导出会后纪要
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMeetingMinutes(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_MINUTES_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
