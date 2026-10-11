// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/serial
// 文件名称：outbound.api.ts
// 创建时间：2026-07-02
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
  SerialOutbound,
  SerialOutboundCreate,
  SerialOutboundUpdate
} from '@/types/logistics/serial/outbound.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSerialOutbounds
 */
const SERIAL_OUTBOUND_API_BASE = 'TaktSerialOutbounds';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取序列号出库列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SerialOutbound>>} 分页结果
 */
export function getSerialOutboundList(queryDto: any): Promise<TaktPagedResult<SerialOutbound>> {
  return request<TaktPagedResult<SerialOutbound>>({
    url: `${SERIAL_OUTBOUND_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取序列号出库
 * @param {string} id 序列号出库ID
 * @returns {Promise<SerialOutbound>} 序列号出库DTO
 */
export function getSerialOutboundById(id: string): Promise<SerialOutbound> {
  return request<SerialOutbound>({
    url: `${SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建序列号出库
 * @param {SerialOutboundCreate} dto 创建DTO
 * @returns {Promise<SerialOutbound>} 序列号出库DTO
 */
export function createSerialOutbound(dto: SerialOutboundCreate): Promise<SerialOutbound> {
  return request<SerialOutbound>({
    url: `${SERIAL_OUTBOUND_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新序列号出库
 * @param {string} id 序列号出库ID
 * @param {SerialOutboundUpdate} dto 更新DTO
 * @returns {Promise<SerialOutbound>} 序列号出库DTO
 */
export function updateSerialOutbound(id: string, dto: SerialOutboundUpdate): Promise<SerialOutbound> {
  return request<SerialOutbound>({
    url: `${SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除序列号出库
 * @param {string} id 序列号出库ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSerialOutboundById(id: string): Promise<void> {
  return request({
    url: `${SERIAL_OUTBOUND_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除序列号出库
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSerialOutboundBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SERIAL_OUTBOUND_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取产品序列号出库选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSerialOutboundOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SERIAL_OUTBOUND_API_BASE}/options`,
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
export function getSerialOutboundTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SERIAL_OUTBOUND_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入序列号出库
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSerialOutbound(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SERIAL_OUTBOUND_API_BASE}/import`,
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
 * 导出序列号出库
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSerialOutbound(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SERIAL_OUTBOUND_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
