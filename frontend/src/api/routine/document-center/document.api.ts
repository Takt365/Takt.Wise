// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/document-center
// 文件名称：document.api.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/document-center 模块 API（自动生成，请勿手改路由常量）
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
  Document,
  DocumentCreate,
  DocumentSort,
  DocumentStatus,
  DocumentUpdate
} from '@/types/routine/document-center/document.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDocuments
 */
const DOCUMENT_API_BASE = 'TaktDocuments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取文管中心列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Document>>} 分页结果
 */
export function getDocumentList(queryDto: any): Promise<TaktPagedResult<Document>> {
  return request<TaktPagedResult<Document>>({
    url: `${DOCUMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取文管中心
 * @param {string} id 文管中心ID
 * @returns {Promise<Document>} 文管中心DTO
 */
export function getDocumentById(id: string): Promise<Document> {
  return request<Document>({
    url: `${DOCUMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建文管中心
 * @param {DocumentCreate} dto 创建DTO
 * @returns {Promise<Document>} 文管中心DTO
 */
export function createDocument(dto: DocumentCreate): Promise<Document> {
  return request<Document>({
    url: `${DOCUMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新文管中心
 * @param {string} id 文管中心ID
 * @param {DocumentUpdate} dto 更新DTO
 * @returns {Promise<Document>} 文管中心DTO
 */
export function updateDocument(id: string, dto: DocumentUpdate): Promise<Document> {
  return request<Document>({
    url: `${DOCUMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除文管中心
 * @param {string} id 文管中心ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentById(id: string): Promise<void> {
  return request({
    url: `${DOCUMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除文管中心
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DOCUMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新文管中心状态
 * @param {DocumentStatus} dto 状态 DTO
 * @returns {Promise<Document>} 文管中心DTO
 */
export function updateDocumentStatus(dto: DocumentStatus): Promise<Document> {
  return request<Document>({
    url: `${DOCUMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新文管中心排序
 * @param {DocumentSort} dto 排序DTO
 * @returns {Promise<Document>} 文管中心DTO
 */
export function updateDocumentSort(dto: DocumentSort): Promise<Document> {
  return request<Document>({
    url: `${DOCUMENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取文管中心主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDocumentOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DOCUMENT_API_BASE}/options`,
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
export function getDocumentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DOCUMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入文管中心
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDocument(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DOCUMENT_API_BASE}/import`,
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
 * 导出文管中心
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDocument(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DOCUMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
