// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/visitor-center
// 文件名称：visitor.api.ts
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
  Visitor,
  VisitorCreate,
  VisitorUpdate
} from '@/types/routine/visitor-center/visitor.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktVisitors
 */
const VISITOR_API_BASE = 'TaktVisitors';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取来访接待列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Visitor>>} 分页结果
 */
export function getVisitorList(queryDto: any): Promise<TaktPagedResult<Visitor>> {
  return request<TaktPagedResult<Visitor>>({
    url: `${VISITOR_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取来访接待
 * @param {string} id 来访接待ID
 * @returns {Promise<Visitor>} 来访接待DTO
 */
export function getVisitorById(id: string): Promise<Visitor> {
  return request<Visitor>({
    url: `${VISITOR_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建来访接待
 * @param {VisitorCreate} dto 创建DTO
 * @returns {Promise<Visitor>} 来访接待DTO
 */
export function createVisitor(dto: VisitorCreate): Promise<Visitor> {
  return request<Visitor>({
    url: `${VISITOR_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新来访接待
 * @param {string} id 来访接待ID
 * @param {VisitorUpdate} dto 更新DTO
 * @returns {Promise<Visitor>} 来访接待DTO
 */
export function updateVisitor(id: string, dto: VisitorUpdate): Promise<Visitor> {
  return request<Visitor>({
    url: `${VISITOR_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除来访接待
 * @param {string} id 来访接待ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitorById(id: string): Promise<void> {
  return request({
    url: `${VISITOR_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除来访接待
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitorBatch(ids: string[]): Promise<void> {
  return request({
    url: `${VISITOR_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取访客中心来访记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getVisitorOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${VISITOR_API_BASE}/options`,
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
export function getVisitorTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${VISITOR_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入来访接待
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importVisitor(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${VISITOR_API_BASE}/import`,
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
 * 导出来访接待
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportVisitor(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${VISITOR_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
