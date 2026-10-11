// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/visitor-center
// 文件名称：visitorCompanion.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/visitor-center 模块 API（自动生成，请勿手改路由常量）
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
  VisitorCompanion,
  VisitorCompanionCreate,
  VisitorCompanionUpdate
} from '@/types/routine/visitor-center/visitorCompanion.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktVisitorCompanions
 */
const VISITOR_COMPANION_API_BASE = 'TaktVisitorCompanions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取来访人员列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<VisitorCompanion>>} 分页结果
 */
export function getVisitorCompanionList(queryDto: any): Promise<TaktPagedResult<VisitorCompanion>> {
  return request<TaktPagedResult<VisitorCompanion>>({
    url: `${VISITOR_COMPANION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取来访人员
 * @param {string} id 来访人员ID
 * @returns {Promise<VisitorCompanion>} 来访人员DTO
 */
export function getVisitorCompanionById(id: string): Promise<VisitorCompanion> {
  return request<VisitorCompanion>({
    url: `${VISITOR_COMPANION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建来访人员
 * @param {VisitorCompanionCreate} dto 创建DTO
 * @returns {Promise<VisitorCompanion>} 来访人员DTO
 */
export function createVisitorCompanion(dto: VisitorCompanionCreate): Promise<VisitorCompanion> {
  return request<VisitorCompanion>({
    url: `${VISITOR_COMPANION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新来访人员
 * @param {string} id 来访人员ID
 * @param {VisitorCompanionUpdate} dto 更新DTO
 * @returns {Promise<VisitorCompanion>} 来访人员DTO
 */
export function updateVisitorCompanion(id: string, dto: VisitorCompanionUpdate): Promise<VisitorCompanion> {
  return request<VisitorCompanion>({
    url: `${VISITOR_COMPANION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除来访人员
 * @param {string} id 来访人员ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitorCompanionById(id: string): Promise<void> {
  return request({
    url: `${VISITOR_COMPANION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除来访人员
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitorCompanionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${VISITOR_COMPANION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取来访人员选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getVisitorCompanionOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${VISITOR_COMPANION_API_BASE}/options`,
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
export function getVisitorCompanionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${VISITOR_COMPANION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入来访人员
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importVisitorCompanion(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${VISITOR_COMPANION_API_BASE}/import`,
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
 * 导出来访人员
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportVisitorCompanion(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${VISITOR_COMPANION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
