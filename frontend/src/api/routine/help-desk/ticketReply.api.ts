// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticketReply.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块 API（自动生成，请勿手改路由常量）
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
  TicketReply,
  TicketReplyCreate,
  TicketReplyUpdate
} from '@/types/routine/help-desk/ticketReply.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTicketReplies
 */
const TICKET_REPLY_API_BASE = 'TaktTicketReplies';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单回复列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TicketReply>>} 分页结果
 */
export function getTicketReplyList(queryDto: any): Promise<TaktPagedResult<TicketReply>> {
  return request<TaktPagedResult<TicketReply>>({
    url: `${TICKET_REPLY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工单回复
 * @param {string} id 工单回复ID
 * @returns {Promise<TicketReply>} 工单回复DTO
 */
export function getTicketReplyById(id: string): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_REPLY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单回复
 * @param {TicketReplyCreate} dto 创建DTO
 * @returns {Promise<TicketReply>} 工单回复DTO
 */
export function createTicketReply(dto: TicketReplyCreate): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_REPLY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单回复
 * @param {string} id 工单回复ID
 * @param {TicketReplyUpdate} dto 更新DTO
 * @returns {Promise<TicketReply>} 工单回复DTO
 */
export function updateTicketReply(id: string, dto: TicketReplyUpdate): Promise<TicketReply> {
  return request<TicketReply>({
    url: `${TICKET_REPLY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单回复
 * @param {string} id 工单回复ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketReplyById(id: string): Promise<void> {
  return request({
    url: `${TICKET_REPLY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单回复
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketReplyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_REPLY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单回复选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketReplyOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TICKET_REPLY_API_BASE}/options`,
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
export function getTicketReplyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_REPLY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工单回复
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTicketReply(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_REPLY_API_BASE}/import`,
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
 * 导出工单回复
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicketReply(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_REPLY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
