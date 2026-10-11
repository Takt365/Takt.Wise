// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowScheme.api.ts
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
  FlowScheme,
  FlowSchemeCreate,
  FlowSchemeSort,
  FlowSchemeStatus,
  FlowSchemeUpdate
} from '@/types/workflow/flowScheme.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowSchemes
 */
const FLOW_SCHEME_API_BASE = 'TaktFlowSchemes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程定义列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowScheme>>} 分页结果
 */
export function getFlowSchemeList(queryDto: any): Promise<TaktPagedResult<FlowScheme>> {
  return request<TaktPagedResult<FlowScheme>>({
    url: `${FLOW_SCHEME_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取流程定义
 * @param {string} id 流程定义ID
 * @returns {Promise<FlowScheme>} 流程定义DTO
 */
export function getFlowSchemeById(id: string): Promise<FlowScheme> {
  return request<FlowScheme>({
    url: `${FLOW_SCHEME_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程定义
 * @param {FlowSchemeCreate} dto 创建DTO
 * @returns {Promise<FlowScheme>} 流程定义DTO
 */
export function createFlowScheme(dto: FlowSchemeCreate): Promise<FlowScheme> {
  return request<FlowScheme>({
    url: `${FLOW_SCHEME_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程定义
 * @param {string} id 流程定义ID
 * @param {FlowSchemeUpdate} dto 更新DTO
 * @returns {Promise<FlowScheme>} 流程定义DTO
 */
export function updateFlowScheme(id: string, dto: FlowSchemeUpdate): Promise<FlowScheme> {
  return request<FlowScheme>({
    url: `${FLOW_SCHEME_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程定义
 * @param {string} id 流程定义ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowSchemeById(id: string): Promise<void> {
  return request({
    url: `${FLOW_SCHEME_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程定义
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowSchemeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_SCHEME_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新流程定义状态
 * @param {FlowSchemeStatus} dto 状态 DTO（TaktFlowSchemeStatus 枚举）
 * @returns {Promise<FlowScheme>} 流程定义DTO
 */
export function updateFlowSchemeStatus(dto: FlowSchemeStatus): Promise<FlowScheme> {
  return request<FlowScheme>({
    url: `${FLOW_SCHEME_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新流程定义排序
 * @param {FlowSchemeSort} dto 排序DTO
 * @returns {Promise<FlowScheme>} 流程定义DTO
 */
export function updateFlowSchemeSort(dto: FlowSchemeSort): Promise<FlowScheme> {
  return request<FlowScheme>({
    url: `${FLOW_SCHEME_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程定义选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowSchemeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FLOW_SCHEME_API_BASE}/options`,
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
export function getFlowSchemeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_SCHEME_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程定义
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowScheme(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_SCHEME_API_BASE}/import`,
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
 * 导出流程定义
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowScheme(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_SCHEME_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
