// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：apsScheduleItem.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  ApsScheduleItem,
  ApsScheduleItemCreate,
  ApsScheduleItemObsolete,
  ApsScheduleItemStatus,
  ApsScheduleItemUpdate
} from '@/types/logistics/manufacturing/aps/apsScheduleItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktApsScheduleItems
 */
const APS_SCHEDULE_ITEM_API_BASE = 'TaktApsScheduleItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取APS排程明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ApsScheduleItem>>} 分页结果
 */
export function getApsScheduleItemList(queryDto: any): Promise<TaktPagedResult<ApsScheduleItem>> {
  return request<TaktPagedResult<ApsScheduleItem>>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取APS排程明细
 * @param {string} id APS排程明细ID
 * @returns {Promise<ApsScheduleItem>} APS排程明细DTO
 */
export function getApsScheduleItemById(id: string): Promise<ApsScheduleItem> {
  return request<ApsScheduleItem>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建APS排程明细
 * @param {ApsScheduleItemCreate} dto 创建DTO
 * @returns {Promise<ApsScheduleItem>} APS排程明细DTO
 */
export function createApsScheduleItem(dto: ApsScheduleItemCreate): Promise<ApsScheduleItem> {
  return request<ApsScheduleItem>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新APS排程明细
 * @param {string} id APS排程明细ID
 * @param {ApsScheduleItemUpdate} dto 更新DTO
 * @returns {Promise<ApsScheduleItem>} APS排程明细DTO
 */
export function updateApsScheduleItem(id: string, dto: ApsScheduleItemUpdate): Promise<ApsScheduleItem> {
  return request<ApsScheduleItem>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除APS排程明细
 * @param {string} id APS排程明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleItemById(id: string): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除APS排程明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsScheduleItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新APS排程明细状态
 * @param {ApsScheduleItemStatus} dto 状态 DTO
 * @returns {Promise<ApsScheduleItem>} APS排程明细DTO
 */
export function updateApsScheduleItemStatus(dto: ApsScheduleItemStatus): Promise<ApsScheduleItem> {
  return request<ApsScheduleItem>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新APS排程明细作废状态
 * @param {ApsScheduleItemObsolete} dto 作废 DTO
 * @returns {Promise<ApsScheduleItem>} APS排程明细DTO
 */
export function updateApsScheduleItemObsolete(dto: ApsScheduleItemObsolete): Promise<ApsScheduleItem> {
  return request<ApsScheduleItem>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取APS排程明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getApsScheduleItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/options`,
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
export function getApsScheduleItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入APS排程明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importApsScheduleItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/import`,
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
 * 导出APS排程明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportApsScheduleItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${APS_SCHEDULE_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
