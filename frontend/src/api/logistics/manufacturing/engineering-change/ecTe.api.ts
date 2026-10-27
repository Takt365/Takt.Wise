// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecTe.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：设变技术部门 API（后端 TaktEcTesController / 实体 TaktEcTe）
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
  EcTe,
  EcTeCreate,
  EcTeStat,
  EcTeStatQuery,
  EcTeStatus,
  EcTeSubmitted,
  EcTeUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecTe.types';
import type {
  EcTeDraftFromSource,
  EcTeImportFromSource,
  EcTeImportFromSourceResult,
  EcTeSourceEcInputItem,
  EcTeSourceEcInputQuery,
  EcTeSourcePlantCode,
} from '@/types/logistics/manufacturing/engineering-change/ecTeSourceInput.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcTes
 */
const EC_ENG_API_BASE = 'TaktEcTes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变技术课主表列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcTe>>} 分页结果
 */
export function getEcTeList(queryDto: any): Promise<TaktPagedResult<EcTe>> {
  return request<TaktPagedResult<EcTe>>({
    url: `${EC_ENG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变技术课主表
 * @param {string} id 设变技术课主表ID
 * @returns {Promise<EcTe>} 设变技术课主表DTO
 */
export function getEcTeById(id: string): Promise<EcTe> {
  return request<EcTe>({
    url: `${EC_ENG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变技术课主表（后台落库 + 派生各部门执行行；立即返回已提交回执）
 * @param {EcTeCreate} dto 创建DTO
 * @returns {Promise<EcTeSubmitted>} 已提交回执
 */
export function createEcTe(dto: EcTeCreate): Promise<EcTeSubmitted> {
  return request<EcTeSubmitted>({
    url: `${EC_ENG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变技术课主表（后台落库 + 同步各部门执行行；立即返回已提交回执）
 * @param {string} id 设变技术课主表ID
 * @param {EcTeUpdate} dto 更新DTO
 * @returns {Promise<EcTeSubmitted>} 已提交回执
 */
export function updateEcTe(id: string, dto: EcTeUpdate): Promise<EcTeSubmitted> {
  return request<EcTeSubmitted>({
    url: `${EC_ENG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变技术课主表
 * @param {string} id 设变技术课主表ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcTeById(id: string): Promise<void> {
  return request({
    url: `${EC_ENG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变技术课主表
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcTeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_ENG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变技术课主表状态
 * @param {EcTeStatus} dto 状态 DTO
 * @returns {Promise<EcTe>} 设变技术课主表DTO
 */
export function updateEcTeStatus(dto: EcTeStatus): Promise<EcTe> {
  return request<EcTe>({
    url: `${EC_ENG_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变技术课主表选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcTeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EC_ENG_API_BASE}/options`,
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
export function getEcTeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_ENG_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变技术课主表
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcTe(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  return request({
    url: `${EC_ENG_API_BASE}/import`,
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
 * 导出设变技术课主表
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcTe(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_ENG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

/**
 * 获取设变技术课主表统计（主表设变单数 + 子表明细行数，如当月设变 1（18））
 * @param {EcTeStatQuery} queryDto 查询参数
 * @returns {Promise<EcTeStat>} 设变统计
 */
export function getEcTeStat(queryDto?: EcTeStatQuery): Promise<EcTeStat> {
  return request<EcTeStat>({
    url: `${EC_ENG_API_BASE}/stat`,
    method: 'get',
    params: queryDto,
  });
}

// ========================================
// 来源设变录入
// ========================================

/**
 * 获取当前公司对应的来源设变目标工厂代码
 * @returns {Promise<EcTeSourcePlantCode>} 公司代码与映射工厂代码
 */
export function getEcTeSourcePlantCode(): Promise<EcTeSourcePlantCode> {
  return request<EcTeSourcePlantCode>({
    url: `${EC_ENG_API_BASE}/source-ec/plant-code`,
    method: 'get',
  });
}

/**
 * 获取尚未导入的来源设变列表（分页）
 * @param {EcTeSourceEcInputQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<EcTeSourceEcInputItem>>} 分页结果
 */
export function getUnimportedSourceEcTeList(queryDto: EcTeSourceEcInputQuery): Promise<TaktPagedResult<EcTeSourceEcInputItem>> {
  return request<TaktPagedResult<EcTeSourceEcInputItem>>({
    url: `${EC_ENG_API_BASE}/source-ec/unimported-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 从来源设变构建创建草稿（不落库，供 ec-form 补全后 create）
 * @param {EcTeDraftFromSource} dto 草稿请求 DTO
 * @returns {Promise<EcTeCreate>} 创建 DTO
 */
export function getEcTeDraftFromSourceEc(dto: EcTeDraftFromSource): Promise<EcTeCreate> {
  return request<EcTeCreate>({
    url: `${EC_ENG_API_BASE}/source-ec/draft`,
    method: 'post',
    data: dto,
  });
}

/**
 * 从来源设变导入设变技术课主表及明细（导入后同样派生各部门执行行，耗时可较长）
 * @param {EcTeImportFromSource} dto 导入 DTO
 * @returns {Promise<EcTeImportFromSourceResult>} 导入结果
 */
export function importEcTeFromSource(dto: EcTeImportFromSource): Promise<EcTeImportFromSourceResult> {
  return request<EcTeImportFromSourceResult>({
    url: `${EC_ENG_API_BASE}/import-from-source-ec`,
    method: 'post',
    data: dto,
    timeout: 300000,
  });
}
