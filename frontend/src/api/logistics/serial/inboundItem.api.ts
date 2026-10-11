// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/serial
// 文件名称：inboundItem.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块 API（自动生成，请勿手改路由常量）
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
  SerialInboundItem,
  SerialInboundItemCreate,
  SerialInboundItemObsolete,
  SerialInboundItemUpdate
} from '@/types/logistics/serial/inboundItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSerialInboundItems
 */
const SERIAL_INBOUND_ITEM_API_BASE = 'TaktSerialInboundItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取序列号入库明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SerialInboundItem>>} 分页结果
 */
export function getSerialInboundItemList(queryDto: any): Promise<TaktPagedResult<SerialInboundItem>> {
  return request<TaktPagedResult<SerialInboundItem>>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取序列号入库明细
 * @param {string} id 序列号入库明细ID
 * @returns {Promise<SerialInboundItem>} 序列号入库明细DTO
 */
export function getSerialInboundItemById(id: string): Promise<SerialInboundItem> {
  return request<SerialInboundItem>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建序列号入库明细
 * @param {SerialInboundItemCreate} dto 创建DTO
 * @returns {Promise<SerialInboundItem>} 序列号入库明细DTO
 */
export function createSerialInboundItem(dto: SerialInboundItemCreate): Promise<SerialInboundItem> {
  return request<SerialInboundItem>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新序列号入库明细
 * @param {string} id 序列号入库明细ID
 * @param {SerialInboundItemUpdate} dto 更新DTO
 * @returns {Promise<SerialInboundItem>} 序列号入库明细DTO
 */
export function updateSerialInboundItem(id: string, dto: SerialInboundItemUpdate): Promise<SerialInboundItem> {
  return request<SerialInboundItem>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除序列号入库明细
 * @param {string} id 序列号入库明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSerialInboundItemById(id: string): Promise<void> {
  return request({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除序列号入库明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSerialInboundItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新序列号入库明细作废状态
 * @param {SerialInboundItemObsolete} dto 作废 DTO
 * @returns {Promise<SerialInboundItem>} 序列号入库明细DTO
 */
export function updateSerialInboundItemObsolete(dto: SerialInboundItemObsolete): Promise<SerialInboundItem> {
  return request<SerialInboundItem>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取产品序列号入库明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSerialInboundItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/options`,
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
export function getSerialInboundItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入序列号入库明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSerialInboundItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/import`,
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
 * 导出序列号入库明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSerialInboundItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SERIAL_INBOUND_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
