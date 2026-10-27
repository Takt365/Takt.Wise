// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecPur.api.ts
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
  EcPur,
  EcPurCreate,
  EcPurObsolete,
  EcPurDiscontinuedStatus,
  EcPurUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecPur.types';
import type {
  EcPurMaster
} from '@/types/logistics/manufacturing/engineering-change/ecPur.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcPurs
 */
const EC_MP_API_BASE = 'TaktEcPurs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变采购执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcPur>>} 分页结果
 */
export function getEcPurList(queryDto: any): Promise<TaktPagedResult<EcPur>> {
  return request<TaktPagedResult<EcPur>>({
    url: `${EC_MP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcPurMaster>>} 分页结果
 */
export function getEcPurMasterList(queryDto: any): Promise<TaktPagedResult<EcPurMaster>> {
  return request<TaktPagedResult<EcPurMaster>>({
    url: `${EC_MP_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变采购执行
 * @param {string} id 设变采购执行ID
 * @returns {Promise<EcPur>} 设变采购执行DTO
 */
export function getEcPurById(id: string): Promise<EcPur> {
  return request<EcPur>({
    url: `${EC_MP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变采购执行
 * @param {EcPurCreate} dto 创建DTO
 * @returns {Promise<EcPur>} 设变采购执行DTO
 */
export function createEcPur(dto: EcPurCreate): Promise<EcPur> {
  return request<EcPur>({
    url: `${EC_MP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变采购执行
 * @param {string} id 设变采购执行ID
 * @param {EcPurUpdate} dto 更新DTO
 * @returns {Promise<EcPur>} 设变采购执行DTO
 */
export function updateEcPur(id: string, dto: EcPurUpdate): Promise<EcPur> {
  return request<EcPur>({
    url: `${EC_MP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变采购执行
 * @param {string} id 设变采购执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcPurById(id: string): Promise<void> {
  return request({
    url: `${EC_MP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变采购执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcPurBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_MP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变mp执行停产状态
 * @param {EcPurDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcPur>} 设变mp执行DTO
 */
export function updateEcPurDiscontinuedStatus(dto: EcPurDiscontinuedStatus): Promise<EcPur> {
  return request<EcPur>({
    url: `${EC_MP_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变采购执行作废状态
 * @param {EcPurObsolete} dto 作废 DTO
 * @returns {Promise<EcPur>} 设变采购执行DTO
 */
export function updateEcPurObsolete(dto: EcPurObsolete): Promise<EcPur> {
  return request<EcPur>({
    url: `${EC_MP_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变采购执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcPurOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_MP_API_BASE}/options`,
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
export function getEcPurTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_MP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变采购执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcPur(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_MP_API_BASE}/import`,
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
 * 导出设变采购执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcPur(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_MP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
