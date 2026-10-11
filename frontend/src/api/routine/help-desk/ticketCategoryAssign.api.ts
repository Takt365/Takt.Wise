// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：ticketCategoryAssign.api.ts
// 创建时间：2026-06-23
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
  TicketCategoryAssign,
  TicketCategoryAssignCreate,
  TicketCategoryAssignSort,
  TicketCategoryAssignUpdate
} from '@/types/routine/help-desk/ticketCategoryAssign.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTicketCategoryAssigns
 */
const TICKET_CATEGORY_ASSIGN_API_BASE = 'TaktTicketCategoryAssigns';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工单分类默认处理人列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TicketCategoryAssign>>} 分页结果
 */
export function getTicketCategoryAssignList(queryDto: any): Promise<TaktPagedResult<TicketCategoryAssign>> {
  return request<TaktPagedResult<TicketCategoryAssign>>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工单分类默认处理人
 * @param {string} id 工单分类默认处理人ID
 * @returns {Promise<TicketCategoryAssign>} 工单分类默认处理人DTO
 */
export function getTicketCategoryAssignById(id: string): Promise<TicketCategoryAssign> {
  return request<TicketCategoryAssign>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工单分类默认处理人
 * @param {TicketCategoryAssignCreate} dto 创建DTO
 * @returns {Promise<TicketCategoryAssign>} 工单分类默认处理人DTO
 */
export function createTicketCategoryAssign(dto: TicketCategoryAssignCreate): Promise<TicketCategoryAssign> {
  return request<TicketCategoryAssign>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工单分类默认处理人
 * @param {string} id 工单分类默认处理人ID
 * @param {TicketCategoryAssignUpdate} dto 更新DTO
 * @returns {Promise<TicketCategoryAssign>} 工单分类默认处理人DTO
 */
export function updateTicketCategoryAssign(id: string, dto: TicketCategoryAssignUpdate): Promise<TicketCategoryAssign> {
  return request<TicketCategoryAssign>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工单分类默认处理人
 * @param {string} id 工单分类默认处理人ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketCategoryAssignById(id: string): Promise<void> {
  return request({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工单分类默认处理人
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTicketCategoryAssignBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工单分类默认处理人排序
 * @param {TicketCategoryAssignSort} dto 排序DTO
 * @returns {Promise<TicketCategoryAssign>} 工单分类默认处理人DTO
 */
export function updateTicketCategoryAssignSort(dto: TicketCategoryAssignSort): Promise<TicketCategoryAssign> {
  return request<TicketCategoryAssign>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工单分类默认处理人选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTicketCategoryAssignOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/options`,
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
export function getTicketCategoryAssignTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工单分类默认处理人
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTicketCategoryAssign(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/import`,
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
 * 导出工单分类默认处理人
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTicketCategoryAssign(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TICKET_CATEGORY_ASSIGN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
