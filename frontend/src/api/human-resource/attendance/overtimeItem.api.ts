// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：overtimeItem.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块 API（自动生成，请勿手改路由常量）
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
  OvertimeItem,
  OvertimeItemCreate,
  OvertimeItemObsolete,
  OvertimeItemUpdate
} from '@/types/human-resource/attendance/overtimeItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktOvertimeItems
 */
const OVERTIME_ITEM_API_BASE = 'TaktOvertimeItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取加班明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<OvertimeItem>>} 分页结果
 */
export function getOvertimeItemList(queryDto: any): Promise<TaktPagedResult<OvertimeItem>> {
  return request<TaktPagedResult<OvertimeItem>>({
    url: `${OVERTIME_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取加班明细
 * @param {string} id 加班明细ID
 * @returns {Promise<OvertimeItem>} 加班明细DTO
 */
export function getOvertimeItemById(id: string): Promise<OvertimeItem> {
  return request<OvertimeItem>({
    url: `${OVERTIME_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建加班明细
 * @param {OvertimeItemCreate} dto 创建DTO
 * @returns {Promise<OvertimeItem>} 加班明细DTO
 */
export function createOvertimeItem(dto: OvertimeItemCreate): Promise<OvertimeItem> {
  return request<OvertimeItem>({
    url: `${OVERTIME_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新加班明细
 * @param {string} id 加班明细ID
 * @param {OvertimeItemUpdate} dto 更新DTO
 * @returns {Promise<OvertimeItem>} 加班明细DTO
 */
export function updateOvertimeItem(id: string, dto: OvertimeItemUpdate): Promise<OvertimeItem> {
  return request<OvertimeItem>({
    url: `${OVERTIME_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除加班明细
 * @param {string} id 加班明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteOvertimeItemById(id: string): Promise<void> {
  return request({
    url: `${OVERTIME_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除加班明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteOvertimeItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${OVERTIME_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新加班明细作废状态
 * @param {OvertimeItemObsolete} dto 作废 DTO
 * @returns {Promise<OvertimeItem>} 加班明细DTO
 */
export function updateOvertimeItemObsolete(dto: OvertimeItemObsolete): Promise<OvertimeItem> {
  return request<OvertimeItem>({
    url: `${OVERTIME_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取加班明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getOvertimeItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${OVERTIME_ITEM_API_BASE}/options`,
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
export function getOvertimeItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${OVERTIME_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入加班明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importOvertimeItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${OVERTIME_ITEM_API_BASE}/import`,
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
 * 导出加班明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportOvertimeItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${OVERTIME_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
