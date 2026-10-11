// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecHinkan.api.ts
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
  EcHinkan,
  EcHinkanCreate,
  EcHinkanObsolete,
  EcHinkanDiscontinuedStatus,
  EcHinkanUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecHinkan.types';
import type {
  EcHinkanMaster
} from '@/types/logistics/manufacturing/engineering-change/ecHinkan.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcHinkans
 */
const EC_HINKAN_API_BASE = 'TaktEcHinkans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变品管执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcHinkan>>} 分页结果
 */
export function getEcHinkanList(queryDto: any): Promise<TaktPagedResult<EcHinkan>> {
  return request<TaktPagedResult<EcHinkan>>({
    url: `${EC_HINKAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcHinkanMaster>>} 分页结果
 */
export function getEcHinkanMasterList(queryDto: any): Promise<TaktPagedResult<EcHinkanMaster>> {
  return request<TaktPagedResult<EcHinkanMaster>>({
    url: `${EC_HINKAN_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变品管执行
 * @param {string} id 设变品管执行ID
 * @returns {Promise<EcHinkan>} 设变品管执行DTO
 */
export function getEcHinkanById(id: string): Promise<EcHinkan> {
  return request<EcHinkan>({
    url: `${EC_HINKAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变品管执行
 * @param {EcHinkanCreate} dto 创建DTO
 * @returns {Promise<EcHinkan>} 设变品管执行DTO
 */
export function createEcHinkan(dto: EcHinkanCreate): Promise<EcHinkan> {
  return request<EcHinkan>({
    url: `${EC_HINKAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变品管执行
 * @param {string} id 设变品管执行ID
 * @param {EcHinkanUpdate} dto 更新DTO
 * @returns {Promise<EcHinkan>} 设变品管执行DTO
 */
export function updateEcHinkan(id: string, dto: EcHinkanUpdate): Promise<EcHinkan> {
  return request<EcHinkan>({
    url: `${EC_HINKAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变品管执行
 * @param {string} id 设变品管执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcHinkanById(id: string): Promise<void> {
  return request({
    url: `${EC_HINKAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变品管执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcHinkanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_HINKAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变hinkan执行停产状态
 * @param {EcHinkanDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcHinkan>} 设变hinkan执行DTO
 */
export function updateEcHinkanDiscontinuedStatus(dto: EcHinkanDiscontinuedStatus): Promise<EcHinkan> {
  return request<EcHinkan>({
    url: `${EC_HINKAN_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变品管执行作废状态
 * @param {EcHinkanObsolete} dto 作废 DTO
 * @returns {Promise<EcHinkan>} 设变品管执行DTO
 */
export function updateEcHinkanObsolete(dto: EcHinkanObsolete): Promise<EcHinkan> {
  return request<EcHinkan>({
    url: `${EC_HINKAN_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变品管执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcHinkanOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_HINKAN_API_BASE}/options`,
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
export function getEcHinkanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_HINKAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变品管执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcHinkan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_HINKAN_API_BASE}/import`,
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
 * 导出设变品管执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcHinkan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_HINKAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
