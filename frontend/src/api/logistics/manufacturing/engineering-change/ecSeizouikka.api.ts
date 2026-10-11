// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecSeizouikka.api.ts
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
  EcSeizouikka,
  EcSeizouikkaCreate,
  EcSeizouikkaObsolete,
  EcSeizouikkaDiscontinuedStatus,
  EcSeizouikkaUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecSeizouikka.types';
import type {
  EcSeizouikkaMaster
} from '@/types/logistics/manufacturing/engineering-change/ecSeizouikka.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcSeizouikkas
 */
const EC_SEIZOUIKKA_API_BASE = 'TaktEcSeizouikkas';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变制一执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcSeizouikka>>} 分页结果
 */
export function getEcSeizouikkaList(queryDto: any): Promise<TaktPagedResult<EcSeizouikka>> {
  return request<TaktPagedResult<EcSeizouikka>>({
    url: `${EC_SEIZOUIKKA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcSeizouikkaMaster>>} 分页结果
 */
export function getEcSeizouikkaMasterList(queryDto: any): Promise<TaktPagedResult<EcSeizouikkaMaster>> {
  return request<TaktPagedResult<EcSeizouikkaMaster>>({
    url: `${EC_SEIZOUIKKA_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变制一执行
 * @param {string} id 设变制一执行ID
 * @returns {Promise<EcSeizouikka>} 设变制一执行DTO
 */
export function getEcSeizouikkaById(id: string): Promise<EcSeizouikka> {
  return request<EcSeizouikka>({
    url: `${EC_SEIZOUIKKA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变制一执行
 * @param {EcSeizouikkaCreate} dto 创建DTO
 * @returns {Promise<EcSeizouikka>} 设变制一执行DTO
 */
export function createEcSeizouikka(dto: EcSeizouikkaCreate): Promise<EcSeizouikka> {
  return request<EcSeizouikka>({
    url: `${EC_SEIZOUIKKA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变制一执行
 * @param {string} id 设变制一执行ID
 * @param {EcSeizouikkaUpdate} dto 更新DTO
 * @returns {Promise<EcSeizouikka>} 设变制一执行DTO
 */
export function updateEcSeizouikka(id: string, dto: EcSeizouikkaUpdate): Promise<EcSeizouikka> {
  return request<EcSeizouikka>({
    url: `${EC_SEIZOUIKKA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变制一执行
 * @param {string} id 设变制一执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcSeizouikkaById(id: string): Promise<void> {
  return request({
    url: `${EC_SEIZOUIKKA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变制一执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcSeizouikkaBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_SEIZOUIKKA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变seizouikka执行停产状态
 * @param {EcSeizouikkaDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcSeizouikka>} 设变seizouikka执行DTO
 */
export function updateEcSeizouikkaDiscontinuedStatus(dto: EcSeizouikkaDiscontinuedStatus): Promise<EcSeizouikka> {
  return request<EcSeizouikka>({
    url: `${EC_SEIZOUIKKA_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变制一执行作废状态
 * @param {EcSeizouikkaObsolete} dto 作废 DTO
 * @returns {Promise<EcSeizouikka>} 设变制一执行DTO
 */
export function updateEcSeizouikkaObsolete(dto: EcSeizouikkaObsolete): Promise<EcSeizouikka> {
  return request<EcSeizouikka>({
    url: `${EC_SEIZOUIKKA_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变制一执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcSeizouikkaOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_SEIZOUIKKA_API_BASE}/options`,
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
export function getEcSeizouikkaTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_SEIZOUIKKA_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变制一执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcSeizouikka(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_SEIZOUIKKA_API_BASE}/import`,
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
 * 导出设变制一执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcSeizouikka(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_SEIZOUIKKA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
