// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecGijutsu.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：设变技术部门 API（后端 TaktEcGijutsusController / 实体 TaktEcGijutsu）
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
  EcGijutsu,
  EcGijutsuCreate,
  EcGijutsuStat,
  EcGijutsuStatQuery,
  EcGijutsuStatus,
  EcGijutsuSubmitted,
  EcGijutsuUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types';
import type {
  EcGijutsuDraftFromSource,
  EcGijutsuImportFromSource,
  EcGijutsuImportFromSourceResult,
  EcGijutsuSourceEcInputItem,
  EcGijutsuSourceEcInputQuery,
  EcGijutsuSourcePlantCode,
} from '@/types/logistics/manufacturing/engineering-change/ecGijutsuSourceInput.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcGijutsus
 */
const EC_GIJUTSU_API_BASE = 'TaktEcGijutsus';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变技术课主表列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcGijutsu>>} 分页结果
 */
export function getEcGijutsuList(queryDto: any): Promise<TaktPagedResult<EcGijutsu>> {
  return request<TaktPagedResult<EcGijutsu>>({
    url: `${EC_GIJUTSU_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变技术课主表
 * @param {string} id 设变技术课主表ID
 * @returns {Promise<EcGijutsu>} 设变技术课主表DTO
 */
export function getEcGijutsuById(id: string): Promise<EcGijutsu> {
  return request<EcGijutsu>({
    url: `${EC_GIJUTSU_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变技术课主表（后台落库 + 派生各部门执行行；立即返回已提交回执）
 * @param {EcGijutsuCreate} dto 创建DTO
 * @returns {Promise<EcGijutsuSubmitted>} 已提交回执
 */
export function createEcGijutsu(dto: EcGijutsuCreate): Promise<EcGijutsuSubmitted> {
  return request<EcGijutsuSubmitted>({
    url: `${EC_GIJUTSU_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变技术课主表（后台落库 + 同步各部门执行行；立即返回已提交回执）
 * @param {string} id 设变技术课主表ID
 * @param {EcGijutsuUpdate} dto 更新DTO
 * @returns {Promise<EcGijutsuSubmitted>} 已提交回执
 */
export function updateEcGijutsu(id: string, dto: EcGijutsuUpdate): Promise<EcGijutsuSubmitted> {
  return request<EcGijutsuSubmitted>({
    url: `${EC_GIJUTSU_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变技术课主表
 * @param {string} id 设变技术课主表ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcGijutsuById(id: string): Promise<void> {
  return request({
    url: `${EC_GIJUTSU_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变技术课主表
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcGijutsuBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_GIJUTSU_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变技术课主表状态
 * @param {EcGijutsuStatus} dto 状态 DTO
 * @returns {Promise<EcGijutsu>} 设变技术课主表DTO
 */
export function updateEcGijutsuStatus(dto: EcGijutsuStatus): Promise<EcGijutsu> {
  return request<EcGijutsu>({
    url: `${EC_GIJUTSU_API_BASE}/status`,
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
export function getEcGijutsuOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EC_GIJUTSU_API_BASE}/options`,
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
export function getEcGijutsuTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_GIJUTSU_API_BASE}/template`,
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
export function importEcGijutsu(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  return request({
    url: `${EC_GIJUTSU_API_BASE}/import`,
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
export function exportEcGijutsu(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_GIJUTSU_API_BASE}/export`,
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
 * @param {EcGijutsuStatQuery} queryDto 查询参数
 * @returns {Promise<EcGijutsuStat>} 设变统计
 */
export function getEcGijutsuStat(queryDto?: EcGijutsuStatQuery): Promise<EcGijutsuStat> {
  return request<EcGijutsuStat>({
    url: `${EC_GIJUTSU_API_BASE}/stat`,
    method: 'get',
    params: queryDto,
  });
}

// ========================================
// 来源设变录入
// ========================================

/**
 * 获取当前公司对应的来源设变目标工厂代码
 * @returns {Promise<EcGijutsuSourcePlantCode>} 公司代码与映射工厂代码
 */
export function getEcGijutsuSourcePlantCode(): Promise<EcGijutsuSourcePlantCode> {
  return request<EcGijutsuSourcePlantCode>({
    url: `${EC_GIJUTSU_API_BASE}/source-ec/plant-code`,
    method: 'get',
  });
}

/**
 * 获取尚未导入的来源设变列表（分页）
 * @param {EcGijutsuSourceEcInputQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<EcGijutsuSourceEcInputItem>>} 分页结果
 */
export function getUnimportedSourceEcGijutsuList(queryDto: EcGijutsuSourceEcInputQuery): Promise<TaktPagedResult<EcGijutsuSourceEcInputItem>> {
  return request<TaktPagedResult<EcGijutsuSourceEcInputItem>>({
    url: `${EC_GIJUTSU_API_BASE}/source-ec/unimported-list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 从来源设变构建创建草稿（不落库，供 ec-form 补全后 create）
 * @param {EcGijutsuDraftFromSource} dto 草稿请求 DTO
 * @returns {Promise<EcGijutsuCreate>} 创建 DTO
 */
export function getEcGijutsuDraftFromSourceEc(dto: EcGijutsuDraftFromSource): Promise<EcGijutsuCreate> {
  return request<EcGijutsuCreate>({
    url: `${EC_GIJUTSU_API_BASE}/source-ec/draft`,
    method: 'post',
    data: dto,
  });
}

/**
 * 从来源设变导入设变技术课主表及明细（导入后同样派生各部门执行行，耗时可较长）
 * @param {EcGijutsuImportFromSource} dto 导入 DTO
 * @returns {Promise<EcGijutsuImportFromSourceResult>} 导入结果
 */
export function importEcGijutsuFromSource(dto: EcGijutsuImportFromSource): Promise<EcGijutsuImportFromSourceResult> {
  return request<EcGijutsuImportFromSourceResult>({
    url: `${EC_GIJUTSU_API_BASE}/import-from-source-ec`,
    method: 'post',
    data: dto,
    timeout: 300000,
  });
}
