// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/announcement
// 文件名称：announcement.api.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/announcement 模块 API（自动生成，请勿手改路由常量）
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
  Announcement,
  AnnouncementCreate,
  AnnouncementStatus,
  AnnouncementUpdate
} from '@/types/routine/announcement.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAnnouncements
 */
const ANNOUNCEMENT_API_BASE = 'TaktAnnouncements';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取公告通知列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Announcement>>} 分页结果
 */
export function getAnnouncementList(queryDto: any): Promise<TaktPagedResult<Announcement>> {
  return request<TaktPagedResult<Announcement>>({
    url: `${ANNOUNCEMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取公告通知
 * @param {string} id 公告通知ID
 * @returns {Promise<Announcement>} 公告通知DTO
 */
export function getAnnouncementById(id: string): Promise<Announcement> {
  return request<Announcement>({
    url: `${ANNOUNCEMENT_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建公告通知
 * @param {AnnouncementCreate} dto 创建DTO
 * @returns {Promise<Announcement>} 公告通知DTO
 */
export function createAnnouncement(dto: AnnouncementCreate): Promise<Announcement> {
  return request<Announcement>({
    url: `${ANNOUNCEMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新公告通知
 * @param {string} id 公告通知ID
 * @param {AnnouncementUpdate} dto 更新DTO
 * @returns {Promise<Announcement>} 公告通知DTO
 */
export function updateAnnouncement(id: string, dto: AnnouncementUpdate): Promise<Announcement> {
  return request<Announcement>({
    url: `${ANNOUNCEMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除公告通知
 * @param {string} id 公告通知ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAnnouncementById(id: string): Promise<void> {
  return request({
    url: `${ANNOUNCEMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除公告通知
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAnnouncementBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ANNOUNCEMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新公告通知状态
 * @param {AnnouncementStatus} dto 状态 DTO
 * @returns {Promise<Announcement>} 公告通知DTO
 */
export function updateAnnouncementStatus(dto: AnnouncementStatus): Promise<Announcement> {
  return request<Announcement>({
    url: `${ANNOUNCEMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取公告通知选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAnnouncementOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ANNOUNCEMENT_API_BASE}/options`,
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
export function getAnnouncementTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ANNOUNCEMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入公告通知
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAnnouncement(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ANNOUNCEMENT_API_BASE}/import`,
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
 * 导出公告通知
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAnnouncement(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ANNOUNCEMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
