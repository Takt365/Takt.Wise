// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：message.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  Message,
  MessageBatchCreate,
  MessageCreate,
  MessageStatistics
} from '@/types/foundation/message.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMessages
 */
const MESSAGE_API_BASE = 'TaktMessages';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取在线消息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageList(queryDto: any): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前登录用户已读消息列表（分页）
 * @param {any} queryDto 已读列表查询 DTO
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageReadList(queryDto: any): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/read-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前登录用户未读消息列表（分页）
 * @param {any} queryDto 未读列表查询 DTO
 * @returns {Promise<TaktPagedResult<Message>>} 分页结果
 */
export function getMessageUnreadList(queryDto: any): Promise<TaktPagedResult<Message>> {
  return request<TaktPagedResult<Message>>({
    url: `${MESSAGE_API_BASE}/unread-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取在线消息
 * @param {string} id 在线消息ID
 * @returns {Promise<Message>} 在线消息DTO
 */
export function getMessageById(id: string): Promise<Message> {
  return request<Message>({
    url: `${MESSAGE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建在线消息
 * @param {MessageCreate} dto 创建DTO
 * @returns {Promise<Message>} 在线消息DTO
 */
export function createMessage(dto: MessageCreate): Promise<Message> {
  return request<Message>({
    url: `${MESSAGE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 批量创建在线消息并 SignalR 推送给各接收者
 * @param {MessageBatchCreate} dto 批量创建 DTO
 * @returns {Promise<Message[]>} 已落库消息列表
 */
export function createAndSendMessages(dto: MessageBatchCreate): Promise<Message[]> {
  return request<Message[]>({
    url: `${MESSAGE_API_BASE}/batch-send`,
    method: 'post',
    data: dto,
  });
}

/**
 * 按消息 ID 推送给接收者（SignalR）
 * @param {string} id 在线消息 ID
 * @returns {Promise<void>} 操作结果
 */
export function sendMessageById(id: string): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/${id}/send`,
    method: 'post',
  });
}

/**
 * 删除在线消息
 * @param {string} id 在线消息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMessageById(id: string): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除在线消息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMessageBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MESSAGE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 获取当前登录用户在线消息统计
 * @returns {Promise<MessageStatistics>} 统计结果
 */
export function getMessageStatistics(): Promise<MessageStatistics> {
  return request<MessageStatistics>({
    url: `${MESSAGE_API_BASE}/statistics`,
    method: 'get',
  });
}

/**
 * 标记在线消息为已读
 * @param {string} id 在线消息 ID
 * @returns {Promise<unknown>} 在线消息 DTO
 */
export function markMessageReadById(id: string): Promise<unknown> {
  return request({
    url: `${MESSAGE_API_BASE}/${id}/read`,
    method: 'put',
  });
}

/**
 * 标记在线消息为未读
 * @param {string} id 在线消息 ID
 * @returns {Promise<unknown>} 在线消息 DTO
 */
export function markMessageUnreadById(id: string): Promise<unknown> {
  return request({
    url: `${MESSAGE_API_BASE}/${id}/unread`,
    method: 'put',
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取在线消息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMessageOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MESSAGE_API_BASE}/options`,
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
 * 导出在线消息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMessage(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MESSAGE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
