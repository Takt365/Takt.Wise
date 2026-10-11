// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/meeting-center
// 文件名称：meetingNotification.api.ts
// 创建时间：2026-08-26
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
  MeetingNotification,
  MeetingNotificationCreate,
  MeetingNotificationStatus,
  MeetingNotificationUpdate
} from '@/types/routine/meeting-center/meetingNotification.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMeetingNotifications
 */
const MEETING_NOTIFICATION_API_BASE = 'TaktMeetingNotifications';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会议通知列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MeetingNotification>>} 分页结果
 */
export function getMeetingNotificationList(queryDto: any): Promise<TaktPagedResult<MeetingNotification>> {
  return request<TaktPagedResult<MeetingNotification>>({
    url: `${MEETING_NOTIFICATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会议通知
 * @param {string} id 会议通知ID
 * @returns {Promise<MeetingNotification>} 会议通知DTO
 */
export function getMeetingNotificationById(id: string): Promise<MeetingNotification> {
  return request<MeetingNotification>({
    url: `${MEETING_NOTIFICATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会议通知
 * @param {MeetingNotificationCreate} dto 创建DTO
 * @returns {Promise<MeetingNotification>} 会议通知DTO
 */
export function createMeetingNotification(dto: MeetingNotificationCreate): Promise<MeetingNotification> {
  return request<MeetingNotification>({
    url: `${MEETING_NOTIFICATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会议通知
 * @param {string} id 会议通知ID
 * @param {MeetingNotificationUpdate} dto 更新DTO
 * @returns {Promise<MeetingNotification>} 会议通知DTO
 */
export function updateMeetingNotification(id: string, dto: MeetingNotificationUpdate): Promise<MeetingNotification> {
  return request<MeetingNotification>({
    url: `${MEETING_NOTIFICATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会议通知
 * @param {string} id 会议通知ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingNotificationById(id: string): Promise<void> {
  return request({
    url: `${MEETING_NOTIFICATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会议通知
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMeetingNotificationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MEETING_NOTIFICATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会议通知状态
 * @param {MeetingNotificationStatus} dto 状态 DTO
 * @returns {Promise<MeetingNotification>} 会议通知DTO
 */
export function updateMeetingNotificationStatus(dto: MeetingNotificationStatus): Promise<MeetingNotification> {
  return request<MeetingNotification>({
    url: `${MEETING_NOTIFICATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会议通知选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMeetingNotificationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MEETING_NOTIFICATION_API_BASE}/options`,
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
export function getMeetingNotificationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_NOTIFICATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会议通知
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMeetingNotification(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MEETING_NOTIFICATION_API_BASE}/import`,
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
 * 导出会议通知
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMeetingNotification(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MEETING_NOTIFICATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 回执确认
// ========================================

/** 回执确认结果 */
export interface MeetingNotificationConfirmReceiptResult {
  meetingNotificationId: string;
  meetingTitle: string;
  alreadyConfirmed: boolean;
  confirmedAt?: string;
}

/**
 * 按邮件令牌确认收到会议通知（匿名）
 * @param {string} token 回执令牌
 * @returns {Promise<MeetingNotificationConfirmReceiptResult>} 确认结果
 */
export function confirmMeetingNotificationReceiptByToken(
  token: string
): Promise<MeetingNotificationConfirmReceiptResult> {
  return request<MeetingNotificationConfirmReceiptResult>({
    url: `${MEETING_NOTIFICATION_API_BASE}/confirm-receipt`,
    method: 'post',
    data: { confirmReceiptToken: token },
    skipTokenRefresh: true,
  });
}

/**
 * 当前登录用户确认收到会议通知
 * @param {string} id 会议通知 ID
 * @returns {Promise<MeetingNotificationConfirmReceiptResult>} 确认结果
 */
export function confirmMeetingNotificationReceipt(
  id: string
): Promise<MeetingNotificationConfirmReceiptResult> {
  return request<MeetingNotificationConfirmReceiptResult>({
    url: `${MEETING_NOTIFICATION_API_BASE}/${id}/confirm-receipt`,
    method: 'put',
  });
}
