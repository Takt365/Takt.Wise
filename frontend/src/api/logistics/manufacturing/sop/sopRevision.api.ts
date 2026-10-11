// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：sopRevision.api.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块 API（自动生成，请勿手改路由常量）
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
  SopRevision,
  SopRevisionCreate,
  SopRevisionStatus,
  SopRevisionUpdate
} from '@/types/logistics/manufacturing/sop/sopRevision.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopRevisions
 */
const SOP_REVISION_API_BASE = 'TaktSopRevisions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP版本列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopRevision>>} 分页结果
 */
export function getSopRevisionList(queryDto: any): Promise<TaktPagedResult<SopRevision>> {
  return request<TaktPagedResult<SopRevision>>({
    url: `${SOP_REVISION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP版本
 * @param {string} id SOP版本ID
 * @returns {Promise<SopRevision>} SOP版本DTO
 */
export function getSopRevisionById(id: string): Promise<SopRevision> {
  return request<SopRevision>({
    url: `${SOP_REVISION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP版本
 * @param {SopRevisionCreate} dto 创建DTO
 * @returns {Promise<SopRevision>} SOP版本DTO
 */
export function createSopRevision(dto: SopRevisionCreate): Promise<SopRevision> {
  return request<SopRevision>({
    url: `${SOP_REVISION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP版本
 * @param {string} id SOP版本ID
 * @param {SopRevisionUpdate} dto 更新DTO
 * @returns {Promise<SopRevision>} SOP版本DTO
 */
export function updateSopRevision(id: string, dto: SopRevisionUpdate): Promise<SopRevision> {
  return request<SopRevision>({
    url: `${SOP_REVISION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP版本
 * @param {string} id SOP版本ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopRevisionById(id: string): Promise<void> {
  return request({
    url: `${SOP_REVISION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP版本
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopRevisionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_REVISION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新SOP版本状态
 * @param {SopRevisionStatus} dto 状态 DTO
 * @returns {Promise<SopRevision>} SOP版本DTO
 */
export function updateSopRevisionStatus(dto: SopRevisionStatus): Promise<SopRevision> {
  return request<SopRevision>({
    url: `${SOP_REVISION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP版本选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopRevisionOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_REVISION_API_BASE}/options`,
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
export function getSopRevisionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_REVISION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP版本
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopRevision(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_REVISION_API_BASE}/import`,
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
 * 导出SOP版本
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopRevision(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_REVISION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
