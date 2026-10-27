// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecQc.api.ts
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
  EcQc,
  EcQcCreate,
  EcQcObsolete,
  EcQcDiscontinuedStatus,
  EcQcUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecQc.types';
import type {
  EcQcMaster
} from '@/types/logistics/manufacturing/engineering-change/ecQc.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcQcs
 */
const EC_QA_API_BASE = 'TaktEcQcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变品管执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcQc>>} 分页结果
 */
export function getEcQcList(queryDto: any): Promise<TaktPagedResult<EcQc>> {
  return request<TaktPagedResult<EcQc>>({
    url: `${EC_QA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcQcMaster>>} 分页结果
 */
export function getEcQcMasterList(queryDto: any): Promise<TaktPagedResult<EcQcMaster>> {
  return request<TaktPagedResult<EcQcMaster>>({
    url: `${EC_QA_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变品管执行
 * @param {string} id 设变品管执行ID
 * @returns {Promise<EcQc>} 设变品管执行DTO
 */
export function getEcQcById(id: string): Promise<EcQc> {
  return request<EcQc>({
    url: `${EC_QA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变品管执行
 * @param {EcQcCreate} dto 创建DTO
 * @returns {Promise<EcQc>} 设变品管执行DTO
 */
export function createEcQc(dto: EcQcCreate): Promise<EcQc> {
  return request<EcQc>({
    url: `${EC_QA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变品管执行
 * @param {string} id 设变品管执行ID
 * @param {EcQcUpdate} dto 更新DTO
 * @returns {Promise<EcQc>} 设变品管执行DTO
 */
export function updateEcQc(id: string, dto: EcQcUpdate): Promise<EcQc> {
  return request<EcQc>({
    url: `${EC_QA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变品管执行
 * @param {string} id 设变品管执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcQcById(id: string): Promise<void> {
  return request({
    url: `${EC_QA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变品管执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcQcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_QA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变qa执行停产状态
 * @param {EcQcDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcQc>} 设变qa执行DTO
 */
export function updateEcQcDiscontinuedStatus(dto: EcQcDiscontinuedStatus): Promise<EcQc> {
  return request<EcQc>({
    url: `${EC_QA_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变品管执行作废状态
 * @param {EcQcObsolete} dto 作废 DTO
 * @returns {Promise<EcQc>} 设变品管执行DTO
 */
export function updateEcQcObsolete(dto: EcQcObsolete): Promise<EcQc> {
  return request<EcQc>({
    url: `${EC_QA_API_BASE}/obsolete`,
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
export function getEcQcOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_QA_API_BASE}/options`,
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
export function getEcQcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_QA_API_BASE}/template`,
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
export function importEcQc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_QA_API_BASE}/import`,
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
export function exportEcQc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_QA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
