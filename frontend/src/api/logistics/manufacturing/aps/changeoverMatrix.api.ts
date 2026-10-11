// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：changeoverMatrix.api.ts
// 创建时间：2026-07-13
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
  ChangeoverMatrix,
  ChangeoverMatrixCreate,
  ChangeoverMatrixStatus,
  ChangeoverMatrixUpdate
} from '@/types/logistics/manufacturing/aps/changeoverMatrix.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktChangeoverMatrixes
 */
const CHANGEOVER_MATRIX_API_BASE = 'TaktChangeoverMatrixes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取换型矩阵列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ChangeoverMatrix>>} 分页结果
 */
export function getChangeoverMatrixList(queryDto: any): Promise<TaktPagedResult<ChangeoverMatrix>> {
  return request<TaktPagedResult<ChangeoverMatrix>>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取换型矩阵
 * @param {string} id 换型矩阵ID
 * @returns {Promise<ChangeoverMatrix>} 换型矩阵DTO
 */
export function getChangeoverMatrixById(id: string): Promise<ChangeoverMatrix> {
  return request<ChangeoverMatrix>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建换型矩阵
 * @param {ChangeoverMatrixCreate} dto 创建DTO
 * @returns {Promise<ChangeoverMatrix>} 换型矩阵DTO
 */
export function createChangeoverMatrix(dto: ChangeoverMatrixCreate): Promise<ChangeoverMatrix> {
  return request<ChangeoverMatrix>({
    url: `${CHANGEOVER_MATRIX_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新换型矩阵
 * @param {string} id 换型矩阵ID
 * @param {ChangeoverMatrixUpdate} dto 更新DTO
 * @returns {Promise<ChangeoverMatrix>} 换型矩阵DTO
 */
export function updateChangeoverMatrix(id: string, dto: ChangeoverMatrixUpdate): Promise<ChangeoverMatrix> {
  return request<ChangeoverMatrix>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除换型矩阵
 * @param {string} id 换型矩阵ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteChangeoverMatrixById(id: string): Promise<void> {
  return request({
    url: `${CHANGEOVER_MATRIX_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除换型矩阵
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteChangeoverMatrixBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CHANGEOVER_MATRIX_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新换型矩阵状态
 * @param {ChangeoverMatrixStatus} dto 状态 DTO
 * @returns {Promise<ChangeoverMatrix>} 换型矩阵DTO
 */
export function updateChangeoverMatrixStatus(dto: ChangeoverMatrixStatus): Promise<ChangeoverMatrix> {
  return request<ChangeoverMatrix>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取换型矩阵选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getChangeoverMatrixOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/options`,
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
export function getChangeoverMatrixTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入换型矩阵
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importChangeoverMatrix(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CHANGEOVER_MATRIX_API_BASE}/import`,
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
 * 导出换型矩阵
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportChangeoverMatrix(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CHANGEOVER_MATRIX_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
