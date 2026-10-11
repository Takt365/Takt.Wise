// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：exec.api.ts
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
  SopExec,
  SopExecCreate,
  SopExecStatus,
  SopExecUpdate
} from '@/types/logistics/manufacturing/sop/exec.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopExecs
 */
const SOP_EXEC_API_BASE = 'TaktSopExecs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP工位执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopExec>>} 分页结果
 */
export function getSopExecList(queryDto: any): Promise<TaktPagedResult<SopExec>> {
  return request<TaktPagedResult<SopExec>>({
    url: `${SOP_EXEC_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP工位执行
 * @param {string} id SOP工位执行ID
 * @returns {Promise<SopExec>} SOP工位执行DTO
 */
export function getSopExecById(id: string): Promise<SopExec> {
  return request<SopExec>({
    url: `${SOP_EXEC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP工位执行
 * @param {SopExecCreate} dto 创建DTO
 * @returns {Promise<SopExec>} SOP工位执行DTO
 */
export function createSopExec(dto: SopExecCreate): Promise<SopExec> {
  return request<SopExec>({
    url: `${SOP_EXEC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP工位执行
 * @param {string} id SOP工位执行ID
 * @param {SopExecUpdate} dto 更新DTO
 * @returns {Promise<SopExec>} SOP工位执行DTO
 */
export function updateSopExec(id: string, dto: SopExecUpdate): Promise<SopExec> {
  return request<SopExec>({
    url: `${SOP_EXEC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP工位执行
 * @param {string} id SOP工位执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopExecById(id: string): Promise<void> {
  return request({
    url: `${SOP_EXEC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP工位执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopExecBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_EXEC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新SOP工位执行状态
 * @param {SopExecStatus} dto 状态 DTO
 * @returns {Promise<SopExec>} SOP工位执行DTO
 */
export function updateSopExecStatus(dto: SopExecStatus): Promise<SopExec> {
  return request<SopExec>({
    url: `${SOP_EXEC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP工位执行选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopExecOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_EXEC_API_BASE}/options`,
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
export function getSopExecTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_EXEC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP工位执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopExec(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_EXEC_API_BASE}/import`,
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
 * 导出SOP工位执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopExec(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_EXEC_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
