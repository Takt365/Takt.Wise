// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecPc.api.ts
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
  EcPc,
  EcPcCreate,
  EcPcObsolete,
  EcPcDiscontinuedStatus,
  EcPcUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecPc.types';
import type {
  EcPcMaster
} from '@/types/logistics/manufacturing/engineering-change/ecPc.types-master';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcPcs
 */
const EC_PC_API_BASE = 'TaktEcPcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变生管执行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcPc>>} 分页结果
 */
export function getEcPcList(queryDto: any): Promise<TaktPagedResult<EcPc>> {
  return request<TaktPagedResult<EcPc>>({
    url: `${EC_PC_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcPcMaster>>} 分页结果
 */
export function getEcPcMasterList(queryDto: any): Promise<TaktPagedResult<EcPcMaster>> {
  return request<TaktPagedResult<EcPcMaster>>({
    url: `${EC_PC_API_BASE}/masters`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变生管执行
 * @param {string} id 设变生管执行ID
 * @returns {Promise<EcPc>} 设变生管执行DTO
 */
export function getEcPcById(id: string): Promise<EcPc> {
  return request<EcPc>({
    url: `${EC_PC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变生管执行
 * @param {EcPcCreate} dto 创建DTO
 * @returns {Promise<EcPc>} 设变生管执行DTO
 */
export function createEcPc(dto: EcPcCreate): Promise<EcPc> {
  return request<EcPc>({
    url: `${EC_PC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变生管执行
 * @param {string} id 设变生管执行ID
 * @param {EcPcUpdate} dto 更新DTO
 * @returns {Promise<EcPc>} 设变生管执行DTO
 */
export function updateEcPc(id: string, dto: EcPcUpdate): Promise<EcPc> {
  return request<EcPc>({
    url: `${EC_PC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变生管执行
 * @param {string} id 设变生管执行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcPcById(id: string): Promise<void> {
  return request({
    url: `${EC_PC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变生管执行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcPcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_PC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变pc执行停产状态
 * @param {EcPcDiscontinuedStatus} dto 停产状态 DTO
 * @returns {Promise<EcPc>} 设变pc执行DTO
 */
export function updateEcPcDiscontinuedStatus(dto: EcPcDiscontinuedStatus): Promise<EcPc> {
  return request<EcPc>({
    url: `${EC_PC_API_BASE}/discontinued-status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变生管执行作废状态
 * @param {EcPcObsolete} dto 作废 DTO
 * @returns {Promise<EcPc>} 设变生管执行DTO
 */
export function updateEcPcObsolete(dto: EcPcObsolete): Promise<EcPc> {
  return request<EcPc>({
    url: `${EC_PC_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变生管执行选项列表
 * @param {string} plantCode plantCode
 * @param {string} keyword keyword
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcPcOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${EC_PC_API_BASE}/options`,
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
export function getEcPcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_PC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变生管执行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcPc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_PC_API_BASE}/import`,
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
 * 导出设变生管执行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcPc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_PC_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
