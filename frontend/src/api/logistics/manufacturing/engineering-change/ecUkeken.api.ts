// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecUkeken.api.ts
// 创建时间：2026-09-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  EcUkeken,
  EcUkekenCreate,
  EcUkekenObsolete,
  EcUkekenDiscontinuedStatus,
  EcUkekenUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecUkeken.types';
import type {
  EcUkekenMaster
} from '@/types/logistics/manufacturing/engineering-change/ecUkeken.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcUkekens
 */
const EC_UKEKEN_API_BASE = 'TaktEcUkekens';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变受检执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcUkeken>>} 分页结果
 */
export function getEcUkekenList(queryDto: any): Promise<TaktPagedResult<EcUkeken>> {
  return request<TaktPagedResult<EcUkeken>>({
    url: `${EC_UKEKEN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcUkekenMaster>>} 分页结果
 */
export function getEcUkekenMasterList(queryDto: any): Promise<TaktPagedResult<EcUkekenMaster>> {
  return request<TaktPagedResult<EcUkekenMaster>>({
    url: `${EC_UKEKEN_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变受检执行
 * @param {string} id 设变受检执行ID
 * @returns {Promise<EcUkeken>} 设变受检执行DTO
 */
export function getEcUkekenById(id: string): Promise<EcUkeken> {
  return request<EcUkeken>({
    url: `${EC_UKEKEN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变受检执行
 * @param {EcUkekenCreate} dto 创建DTO
 * @returns {Promise<EcUkeken>} 设变受检执行DTO
 */
export function createEcUkeken(dto: EcUkekenCreate): Promise<EcUkeken> {
  return request<EcUkeken>({
    url: `${EC_UKEKEN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变受检执行
 * @param {string} id 设变受检执行ID
 * @param {EcUkekenUpdate} dto 更新DTO
 * @returns {Promise<EcUkeken>} 设变受检执行DTO
 */
export function updateEcUkeken(id: string, dto: EcUkekenUpdate): Promise<EcUkeken> {
  return request<EcUkeken>({
    url: `${EC_UKEKEN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变受检执行
 * @param {string} id 设变受检执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcUkekenById(id: string): Promise<void> {
  return request({
    url: `${EC_UKEKEN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变受检执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcUkekenBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_UKEKEN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变ukeken执行停产状态
 * @param {EcUkekenDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcUkeken>} 设变ukeken执行DTO
 */
export function updateEcUkekenDiscontinuedStatus(dto: EcUkekenDiscontinuedStatus): Promise<EcUkeken> {
  return request<EcUkeken>({
    url: `${EC_UKEKEN_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变受检执行作废状态
 * @param {EcUkekenObsolete} dto 作废 DTO
 * @returns {Promise<EcUkeken>} 设变受检执行DTO
 */
export function updateEcUkekenObsolete(dto: EcUkekenObsolete): Promise<EcUkeken> {
  return request<EcUkeken>({
    url: `${EC_UKEKEN_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变受检执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcUkekenOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_UKEKEN_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getEcUkekenTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_UKEKEN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变受检执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcUkeken(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_UKEKEN_API_BASE}/import`,
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
 * 导出设变受检执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcUkeken(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_UKEKEN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
