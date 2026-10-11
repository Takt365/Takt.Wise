// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：materialDocument.api.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  MaterialDocument,
  MaterialDocumentCreate,
  MaterialDocumentUpdate
} from '@/types/logistics/materials/materialDocument.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialDocuments
 */
const MATERIAL_DOCUMENT_API_BASE = 'TaktMaterialDocuments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料凭证列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialDocument>>} 分页结果
 */
export function getMaterialDocumentList(queryDto: any): Promise<TaktPagedResult<MaterialDocument>> {
  return request<TaktPagedResult<MaterialDocument>>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料凭证
 * @param {string} id 物料凭证ID
 * @returns {Promise<MaterialDocument>} 物料凭证DTO
 */
export function getMaterialDocumentById(id: string): Promise<MaterialDocument> {
  return request<MaterialDocument>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建物料凭证
 * @param {MaterialDocumentCreate} dto 创建DTO
 * @returns {Promise<MaterialDocument>} 物料凭证DTO
 */
export function createMaterialDocument(dto: MaterialDocumentCreate): Promise<MaterialDocument> {
  return request<MaterialDocument>({
    url: `${MATERIAL_DOCUMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料凭证
 * @param {string} id 物料凭证ID
 * @param {MaterialDocumentUpdate} dto 更新DTO
 * @returns {Promise<MaterialDocument>} 物料凭证DTO
 */
export function updateMaterialDocument(id: string, dto: MaterialDocumentUpdate): Promise<MaterialDocument> {
  return request<MaterialDocument>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料凭证
 * @param {string} id 物料凭证ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDocumentById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_DOCUMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料凭证
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDocumentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_DOCUMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料凭证选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialDocumentOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/options`,
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
export function getMaterialDocumentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料凭证
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialDocument(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_DOCUMENT_API_BASE}/import`,
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
 * 导出物料凭证
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialDocument(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DOCUMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
