// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowAddSign.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块 API（自动生成，请勿手改路由常量）
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
  FlowAddSign,
  FlowAddSignCreate,
  FlowAddSignUpdate
} from '@/types/workflow/flowAddSign.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowAddSigns
 */
const FLOW_ADD_SIGN_API_BASE = 'TaktFlowAddSigns';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程加签记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowAddSign>>} 分页结果
 */
export function getFlowAddSignList(queryDto: any): Promise<TaktPagedResult<FlowAddSign>> {
  return request<TaktPagedResult<FlowAddSign>>({
    url: `${FLOW_ADD_SIGN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取流程加签记录
 * @param {string} id 流程加签记录ID
 * @returns {Promise<FlowAddSign>} 流程加签记录DTO
 */
export function getFlowAddSignById(id: string): Promise<FlowAddSign> {
  return request<FlowAddSign>({
    url: `${FLOW_ADD_SIGN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程加签记录
 * @param {FlowAddSignCreate} dto 创建DTO
 * @returns {Promise<FlowAddSign>} 流程加签记录DTO
 */
export function createFlowAddSign(dto: FlowAddSignCreate): Promise<FlowAddSign> {
  return request<FlowAddSign>({
    url: `${FLOW_ADD_SIGN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程加签记录
 * @param {string} id 流程加签记录ID
 * @param {FlowAddSignUpdate} dto 更新DTO
 * @returns {Promise<FlowAddSign>} 流程加签记录DTO
 */
export function updateFlowAddSign(id: string, dto: FlowAddSignUpdate): Promise<FlowAddSign> {
  return request<FlowAddSign>({
    url: `${FLOW_ADD_SIGN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程加签记录
 * @param {string} id 流程加签记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowAddSignById(id: string): Promise<void> {
  return request({
    url: `${FLOW_ADD_SIGN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程加签记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowAddSignBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_ADD_SIGN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程加签记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowAddSignOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FLOW_ADD_SIGN_API_BASE}/options`,
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
export function getFlowAddSignTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_ADD_SIGN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程加签记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowAddSign(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_ADD_SIGN_API_BASE}/import`,
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
 * 导出流程加签记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowAddSign(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_ADD_SIGN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
