// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecMfg2.api.ts
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
  EcMfg2,
  EcMfg2Create,
  EcMfg2Obsolete,
  EcMfg2DiscontinuedStatus,
  EcMfg2Update
} from '@/types/logistics/manufacturing/engineering-change/ecMfg2.types';
import type {
  EcMfg2Master
} from '@/types/logistics/manufacturing/engineering-change/ecMfg2Master.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcMfg2s
 */
const EC_PCBA_API_BASE = 'TaktEcMfg2s';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变制二执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcMfg2>>} 分页结果
 */
export function getEcMfg2List(queryDto: any): Promise<TaktPagedResult<EcMfg2>> {
  return request<TaktPagedResult<EcMfg2>>({
    url: `${EC_PCBA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcMfg2Master>>} 分页结果
 */
export function getEcMfg2MasterList(queryDto: any): Promise<TaktPagedResult<EcMfg2Master>> {
  return request<TaktPagedResult<EcMfg2Master>>({
    url: `${EC_PCBA_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变制二执行
 * @param {string} id 设变制二执行ID
 * @returns {Promise<EcMfg2>} 设变制二执行DTO
 */
export function getEcMfg2ById(id: string): Promise<EcMfg2> {
  return request<EcMfg2>({
    url: `${EC_PCBA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变制二执行
 * @param {EcMfg2Create} dto 创建DTO
 * @returns {Promise<EcMfg2>} 设变制二执行DTO
 */
export function createEcMfg2(dto: EcMfg2Create): Promise<EcMfg2> {
  return request<EcMfg2>({
    url: `${EC_PCBA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变制二执行
 * @param {string} id 设变制二执行ID
 * @param {EcMfg2Update} dto 更新DTO
 * @returns {Promise<EcMfg2>} 设变制二执行DTO
 */
export function updateEcMfg2(id: string, dto: EcMfg2Update): Promise<EcMfg2> {
  return request<EcMfg2>({
    url: `${EC_PCBA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变制二执行
 * @param {string} id 设变制二执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcMfg2ById(id: string): Promise<void> {
  return request({
    url: `${EC_PCBA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变制二执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcMfg2Batch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_PCBA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变pcba执行停产状态
 * @param {EcMfg2DiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcMfg2>} 设变pcba执行DTO
 */
export function updateEcMfg2DiscontinuedStatus(dto: EcMfg2DiscontinuedStatus): Promise<EcMfg2> {
  return request<EcMfg2>({
    url: `${EC_PCBA_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变制二执行作废状态
 * @param {EcMfg2Obsolete} dto 作废 DTO
 * @returns {Promise<EcMfg2>} 设变制二执行DTO
 */
export function updateEcMfg2Obsolete(dto: EcMfg2Obsolete): Promise<EcMfg2> {
  return request<EcMfg2>({
    url: `${EC_PCBA_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变制二执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcMfg2Options(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_PCBA_API_BASE}/options`,
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
export function getEcMfg2Template(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_PCBA_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变制二执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcMfg2(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_PCBA_API_BASE}/import`,
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
 * 导出设变制二执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcMfg2(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_PCBA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
