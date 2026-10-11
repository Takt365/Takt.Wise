// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecSeizounika.api.ts
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
  EcSeizounika,
  EcSeizounikaCreate,
  EcSeizounikaObsolete,
  EcSeizounikaDiscontinuedStatus,
  EcSeizounikaUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecSeizounika.types';
import type {
  EcSeizounikaMaster
} from '@/types/logistics/manufacturing/engineering-change/ecSeizounikaMaster.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcSeizounikas
 */
const EC_SEIZOUNIKA_API_BASE = 'TaktEcSeizounikas';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变制二执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcSeizounika>>} 分页结果
 */
export function getEcSeizounikaList(queryDto: any): Promise<TaktPagedResult<EcSeizounika>> {
  return request<TaktPagedResult<EcSeizounika>>({
    url: `${EC_SEIZOUNIKA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcSeizounikaMaster>>} 分页结果
 */
export function getEcSeizounikaMasterList(queryDto: any): Promise<TaktPagedResult<EcSeizounikaMaster>> {
  return request<TaktPagedResult<EcSeizounikaMaster>>({
    url: `${EC_SEIZOUNIKA_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变制二执行
 * @param {string} id 设变制二执行ID
 * @returns {Promise<EcSeizounika>} 设变制二执行DTO
 */
export function getEcSeizounikaById(id: string): Promise<EcSeizounika> {
  return request<EcSeizounika>({
    url: `${EC_SEIZOUNIKA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变制二执行
 * @param {EcSeizounikaCreate} dto 创建DTO
 * @returns {Promise<EcSeizounika>} 设变制二执行DTO
 */
export function createEcSeizounika(dto: EcSeizounikaCreate): Promise<EcSeizounika> {
  return request<EcSeizounika>({
    url: `${EC_SEIZOUNIKA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变制二执行
 * @param {string} id 设变制二执行ID
 * @param {EcSeizounikaUpdate} dto 更新DTO
 * @returns {Promise<EcSeizounika>} 设变制二执行DTO
 */
export function updateEcSeizounika(id: string, dto: EcSeizounikaUpdate): Promise<EcSeizounika> {
  return request<EcSeizounika>({
    url: `${EC_SEIZOUNIKA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变制二执行
 * @param {string} id 设变制二执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcSeizounikaById(id: string): Promise<void> {
  return request({
    url: `${EC_SEIZOUNIKA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变制二执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcSeizounikaBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_SEIZOUNIKA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变seizounika执行停产状态
 * @param {EcSeizounikaDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcSeizounika>} 设变seizounika执行DTO
 */
export function updateEcSeizounikaDiscontinuedStatus(dto: EcSeizounikaDiscontinuedStatus): Promise<EcSeizounika> {
  return request<EcSeizounika>({
    url: `${EC_SEIZOUNIKA_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变制二执行作废状态
 * @param {EcSeizounikaObsolete} dto 作废 DTO
 * @returns {Promise<EcSeizounika>} 设变制二执行DTO
 */
export function updateEcSeizounikaObsolete(dto: EcSeizounikaObsolete): Promise<EcSeizounika> {
  return request<EcSeizounika>({
    url: `${EC_SEIZOUNIKA_API_BASE}/obsolete`,
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
export function getEcSeizounikaOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_SEIZOUNIKA_API_BASE}/options`,
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
export function getEcSeizounikaTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_SEIZOUNIKA_API_BASE}/template`,
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
export function importEcSeizounika(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_SEIZOUNIKA_API_BASE}/import`,
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
export function exportEcSeizounika(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_SEIZOUNIKA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
