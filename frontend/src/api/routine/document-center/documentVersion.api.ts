// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/document-center
// 文件名称：documentVersion.api.ts
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
  DocumentVersion,
  DocumentVersionCreate,
  DocumentVersionObsolete,
  DocumentVersionUpdate
} from '@/types/routine/document-center/documentVersion.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDocumentVersions
 */
const DOCUMENT_VERSION_API_BASE = 'TaktDocumentVersions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取文管文档版本列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DocumentVersion>>} 分页结果
 */
export function getDocumentVersionList(queryDto: any): Promise<TaktPagedResult<DocumentVersion>> {
  return request<TaktPagedResult<DocumentVersion>>({
    url: `${DOCUMENT_VERSION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取文管文档版本
 * @param {string} id 文管文档版本ID
 * @returns {Promise<DocumentVersion>} 文管文档版本DTO
 */
export function getDocumentVersionById(id: string): Promise<DocumentVersion> {
  return request<DocumentVersion>({
    url: `${DOCUMENT_VERSION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建文管文档版本
 * @param {DocumentVersionCreate} dto 创建DTO
 * @returns {Promise<DocumentVersion>} 文管文档版本DTO
 */
export function createDocumentVersion(dto: DocumentVersionCreate): Promise<DocumentVersion> {
  return request<DocumentVersion>({
    url: `${DOCUMENT_VERSION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新文管文档版本
 * @param {string} id 文管文档版本ID
 * @param {DocumentVersionUpdate} dto 更新DTO
 * @returns {Promise<DocumentVersion>} 文管文档版本DTO
 */
export function updateDocumentVersion(id: string, dto: DocumentVersionUpdate): Promise<DocumentVersion> {
  return request<DocumentVersion>({
    url: `${DOCUMENT_VERSION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除文管文档版本
 * @param {string} id 文管文档版本ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentVersionById(id: string): Promise<void> {
  return request({
    url: `${DOCUMENT_VERSION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除文管文档版本
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDocumentVersionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DOCUMENT_VERSION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新文管文档版本作废状态
 * @param {DocumentVersionObsolete} dto 作废 DTO
 * @returns {Promise<DocumentVersion>} 文管文档版本DTO
 */
export function updateDocumentVersionObsolete(dto: DocumentVersionObsolete): Promise<DocumentVersion> {
  return request<DocumentVersion>({
    url: `${DOCUMENT_VERSION_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取文管文档版本选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDocumentVersionOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DOCUMENT_VERSION_API_BASE}/options`,
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
export function getDocumentVersionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DOCUMENT_VERSION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入文管文档版本
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDocumentVersion(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DOCUMENT_VERSION_API_BASE}/import`,
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
 * 导出文管文档版本
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDocumentVersion(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DOCUMENT_VERSION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
