// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/workflow
// 文件名称：flowForm.api.ts
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
  FlowForm,
  FlowFormCreate,
  FlowFormSort,
  FlowFormStatus,
  FlowFormUpdate
} from '@/types/workflow/flowForm.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFlowForms
 */
const FLOW_FORM_API_BASE = 'TaktFlowForms';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取流程表单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FlowForm>>} 分页结果
 */
export function getFlowFormList(queryDto: any): Promise<TaktPagedResult<FlowForm>> {
  return request<TaktPagedResult<FlowForm>>({
    url: `${FLOW_FORM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取流程表单
 * @param {string} id 流程表单ID
 * @returns {Promise<FlowForm>} 流程表单DTO
 */
export function getFlowFormById(id: string): Promise<FlowForm> {
  return request<FlowForm>({
    url: `${FLOW_FORM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建流程表单
 * @param {FlowFormCreate} dto 创建DTO
 * @returns {Promise<FlowForm>} 流程表单DTO
 */
export function createFlowForm(dto: FlowFormCreate): Promise<FlowForm> {
  return request<FlowForm>({
    url: `${FLOW_FORM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新流程表单
 * @param {string} id 流程表单ID
 * @param {FlowFormUpdate} dto 更新DTO
 * @returns {Promise<FlowForm>} 流程表单DTO
 */
export function updateFlowForm(id: string, dto: FlowFormUpdate): Promise<FlowForm> {
  return request<FlowForm>({
    url: `${FLOW_FORM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除流程表单
 * @param {string} id 流程表单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowFormById(id: string): Promise<void> {
  return request({
    url: `${FLOW_FORM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除流程表单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFlowFormBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FLOW_FORM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新流程表单状态
 * @param {FlowFormStatus} dto 状态 DTO
 * @returns {Promise<FlowForm>} 流程表单DTO
 */
export function updateFlowFormStatus(dto: FlowFormStatus): Promise<FlowForm> {
  return request<FlowForm>({
    url: `${FLOW_FORM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新流程表单排序
 * @param {FlowFormSort} dto 排序DTO
 * @returns {Promise<FlowForm>} 流程表单DTO
 */
export function updateFlowFormSort(dto: FlowFormSort): Promise<FlowForm> {
  return request<FlowForm>({
    url: `${FLOW_FORM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取流程表单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFlowFormOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FLOW_FORM_API_BASE}/options`,
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
export function getFlowFormTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_FORM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入流程表单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFlowForm(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FLOW_FORM_API_BASE}/import`,
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
 * 导出流程表单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFlowForm(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FLOW_FORM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
