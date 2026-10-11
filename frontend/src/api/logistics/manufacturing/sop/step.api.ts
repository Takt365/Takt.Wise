// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：step.api.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块 API（自动生成，请勿手改路由常量）
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
  SopStep,
  SopStepCreate,
  SopStepUpdate
} from '@/types/logistics/manufacturing/sop/step.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopSteps
 */
const SOP_STEP_API_BASE = 'TaktSopSteps';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP工步列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopStep>>} 分页结果
 */
export function getSopStepList(queryDto: any): Promise<TaktPagedResult<SopStep>> {
  return request<TaktPagedResult<SopStep>>({
    url: `${SOP_STEP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP工步
 * @param {string} id SOP工步ID
 * @returns {Promise<SopStep>} SOP工步DTO
 */
export function getSopStepById(id: string): Promise<SopStep> {
  return request<SopStep>({
    url: `${SOP_STEP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP工步
 * @param {SopStepCreate} dto 创建DTO
 * @returns {Promise<SopStep>} SOP工步DTO
 */
export function createSopStep(dto: SopStepCreate): Promise<SopStep> {
  return request<SopStep>({
    url: `${SOP_STEP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP工步
 * @param {string} id SOP工步ID
 * @param {SopStepUpdate} dto 更新DTO
 * @returns {Promise<SopStep>} SOP工步DTO
 */
export function updateSopStep(id: string, dto: SopStepUpdate): Promise<SopStep> {
  return request<SopStep>({
    url: `${SOP_STEP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP工步
 * @param {string} id SOP工步ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopStepById(id: string): Promise<void> {
  return request({
    url: `${SOP_STEP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP工步
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopStepBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_STEP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP工步选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopStepOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_STEP_API_BASE}/options`,
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
export function getSopStepTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_STEP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP工步
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopStep(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_STEP_API_BASE}/import`,
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
 * 导出SOP工步
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopStep(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_STEP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
