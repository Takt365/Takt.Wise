// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticket.api.ts
// 创建时间：2026-08-28
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
  Ticket,
  TicketCreate,
  TicketStatus,
  TicketUpdate
} from '@/types/routine/help-desk/ticket.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTickets
 */
const TICKET_API_BASE = 'TaktTickets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Ticket>>} 分页结果
 */
export function getTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工单
 * @param {string} id 工单ID
 * @returns {Promise<Ticket>} 工单DTO
 */
export function getTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单
 * @param {TicketCreate} dto 创建DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function createTicket(dto: TicketCreate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单
 * @param {string} id 工单ID
 * @param {TicketUpdate} dto 更新DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function updateTicket(id: string, dto: TicketUpdate): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单
 * @param {string} id 工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketById(id: string): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工单状态
 * @param {TicketStatus} dto 状态 DTO
 * @returns {Promise<Ticket>} 工单DTO
 */
export function updateTicketStatus(dto: TicketStatus): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TICKET_API_BASE}/options`,
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
export function getTicketTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTicket(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_API_BASE}/import`,
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
 * 导出工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicket(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_API_BASE}/export`,
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
// ITSM 工作流 / 门户（手工维护；codegen 勿覆盖）
// ========================================

/**
 * 当前用户提交的工单列表（分页）
 * @param queryDto 查询参数
 * @returns 分页结果
 */
export function getMyTicketList(queryDto: any): Promise<TaktPagedResult<Ticket>> {
  return request<TaktPagedResult<Ticket>>({
    url: `${TICKET_API_BASE}/my-tickets`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 当前用户提交的工单详情
 * @param id 工单 ID
 * @returns 工单 DTO
 */
export function getMyTicketById(id: string): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/my-tickets/${id}`,
    method: 'get',
  });
}

/**
 * 当前用户工单回复列表（分页，不含内部备注）
 * @param id 工单 ID
 * @param queryDto 分页查询
 * @returns 分页结果
 */
export function getMyTicketReplyList(id: string, queryDto: any): Promise<TaktPagedResult<any>> {
  return request<TaktPagedResult<any>>({
    url: `${TICKET_API_BASE}/my-tickets/${id}/replies`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 门户用户回复自己的工单
 * @param id 工单 ID
 * @param dto 回复内容
 * @returns 回复结果
 */
export function replyMyTicket(id: string, dto: { content: string }): Promise<any> {
  return request<any>({
    url: `${TICKET_API_BASE}/my-tickets/${id}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 门户提交新工单
 * @param dto 提交 DTO
 * @returns 工单 DTO
 */
export function submitTicket(dto: any): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/submit`,
    method: 'post',
    data: dto,
  });
}

/**
 * 客服领取/指派工单
 * @param dto 指派 DTO
 * @returns 工单 DTO
 */
export function assignTicket(dto: { ticketId: string; startImmediately?: boolean }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/assign`,
    method: 'post',
    data: dto,
  });
}

/**
 * 开始处理工单
 * @param dto 动作 DTO
 * @returns 工单 DTO
 */
export function startTicketProgress(dto: { ticketId: string }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/start`,
    method: 'post',
    data: dto,
  });
}

/**
 * 等待用户回复
 * @param dto 动作 DTO
 * @returns 工单 DTO
 */
export function waitForRequester(dto: { ticketId: string }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/wait`,
    method: 'post',
    data: dto,
  });
}

/**
 * 标记工单已解决
 * @param dto 动作 DTO
 * @returns 工单 DTO
 */
export function resolveTicket(dto: { ticketId: string }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/resolve`,
    method: 'post',
    data: dto,
  });
}

/**
 * 用户确认关闭工单
 * @param dto 动作 DTO
 * @returns 工单 DTO
 */
export function confirmCloseTicket(dto: { ticketId: string }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/confirm-close`,
    method: 'post',
    data: dto,
  });
}

/**
 * 重新打开工单
 * @param dto 动作 DTO
 * @returns 工单 DTO
 */
export function reopenTicket(dto: { ticketId: string }): Promise<Ticket> {
  return request<Ticket>({
    url: `${TICKET_API_BASE}/reopen`,
    method: 'post',
    data: dto,
  });
}

/**
 * 客服添加工单回复（会话）
 * @param dto 回复 DTO
 * @returns 回复结果
 */
export function replyTicket(dto: {
  ticketId: string
  content: string
  isInternal?: number
}): Promise<any> {
  return request<any>({
    url: `${TICKET_API_BASE}/reply`,
    method: 'post',
    data: dto,
  });
}

/**
 * 工单回复列表（分页，客服端）
 * @param queryDto 查询（含 ticketId）
 * @returns 分页结果
 */
export function getTicketReplyList(queryDto: any): Promise<TaktPagedResult<any>> {
  return request<TaktPagedResult<any>>({
    url: `${TICKET_API_BASE}/replies`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 当前用户工单关联资产汇总（分页）
 * @param queryDto 分页查询
 * @returns 分页结果
 */
export function getMyTicketAssetList(queryDto: any): Promise<TaktPagedResult<any>> {
  return request<TaktPagedResult<any>>({
    url: `${TICKET_API_BASE}/my-assets`,
    method: 'get',
    params: queryDto,
  });
}
