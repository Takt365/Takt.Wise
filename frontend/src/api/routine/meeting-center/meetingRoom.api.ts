// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/meeting-center
// 文件名称：meetingRoom.api.ts
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
  MeetingRoom,
  MeetingRoomCreate,
  MeetingRoomSort,
  MeetingRoomStatus,
  MeetingRoomUpdate
} from '@/types/routine/meeting-center/meetingRoom.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMeetingRooms
 */
const MEETING_ROOM_API_BASE = 'TaktMeetingRooms';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议室列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MeetingRoom>>} 分页结果
 */
export function getMeetingRoomList(queryDto: any): Promise<TaktPagedResult<MeetingRoom>> {
  return request<TaktPagedResult<MeetingRoom>>({
    url: `${MEETING_ROOM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议室
 * @param {string} id 会议室ID
 * @returns {Promise<MeetingRoom>} 会议室DTO
 */
export function getMeetingRoomById(id: string): Promise<MeetingRoom> {
  return request<MeetingRoom>({
    url: `${MEETING_ROOM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议室
 * @param {MeetingRoomCreate} dto 创建DTO
 * @returns {Promise<MeetingRoom>} 会议室DTO
 */
export function createMeetingRoom(dto: MeetingRoomCreate): Promise<MeetingRoom> {
  return request<MeetingRoom>({
    url: `${MEETING_ROOM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议室
 * @param {string} id 会议室ID
 * @param {MeetingRoomUpdate} dto 更新DTO
 * @returns {Promise<MeetingRoom>} 会议室DTO
 */
export function updateMeetingRoom(id: string, dto: MeetingRoomUpdate): Promise<MeetingRoom> {
  return request<MeetingRoom>({
    url: `${MEETING_ROOM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议室
 * @param {string} id 会议室ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingRoomById(id: string): Promise<void> {
  return request({
    url: `${MEETING_ROOM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议室
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingRoomBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MEETING_ROOM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会议室状态
 * @param {MeetingRoomStatus} dto 状态 DTO
 * @returns {Promise<MeetingRoom>} 会议室DTO
 */
export function updateMeetingRoomStatus(dto: MeetingRoomStatus): Promise<MeetingRoom> {
  return request<MeetingRoom>({
    url: `${MEETING_ROOM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新会议室排序
 * @param {MeetingRoomSort} dto 排序DTO
 * @returns {Promise<MeetingRoom>} 会议室DTO
 */
export function updateMeetingRoomSort(dto: MeetingRoomSort): Promise<MeetingRoom> {
  return request<MeetingRoom>({
    url: `${MEETING_ROOM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议室选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMeetingRoomOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MEETING_ROOM_API_BASE}/options`,
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
export function getMeetingRoomTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_ROOM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议室
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMeetingRoom(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MEETING_ROOM_API_BASE}/import`,
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
 * 导出会议室
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMeetingRoom(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_ROOM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
