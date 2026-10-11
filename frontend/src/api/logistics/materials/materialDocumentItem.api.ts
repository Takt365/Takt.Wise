// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：materialDocumentItem.api.ts
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
  MaterialDocumentItem,
  MaterialDocumentItemCreate,
  MaterialDocumentItemObsolete,
  MaterialDocumentItemUpdate
} from '@/types/logistics/materials/materialDocumentItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialDocumentItems
 */
const MATERIAL_DOCUMENT_ITEM_API_BASE = 'TaktMaterialDocumentItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料凭证行项目列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialDocumentItem>>} 分页结果
 */
export function getMaterialDocumentItemList(queryDto: any): Promise<TaktPagedResult<MaterialDocumentItem>> {
  return request<TaktPagedResult<MaterialDocumentItem>>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料凭证行项目
 * @param {string} id 物料凭证行项目ID
 * @returns {Promise<MaterialDocumentItem>} 物料凭证行项目DTO
 */
export function getMaterialDocumentItemById(id: string): Promise<MaterialDocumentItem> {
  return request<MaterialDocumentItem>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建物料凭证行项目
 * @param {MaterialDocumentItemCreate} dto 创建DTO
 * @returns {Promise<MaterialDocumentItem>} 物料凭证行项目DTO
 */
export function createMaterialDocumentItem(dto: MaterialDocumentItemCreate): Promise<MaterialDocumentItem> {
  return request<MaterialDocumentItem>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料凭证行项目
 * @param {string} id 物料凭证行项目ID
 * @param {MaterialDocumentItemUpdate} dto 更新DTO
 * @returns {Promise<MaterialDocumentItem>} 物料凭证行项目DTO
 */
export function updateMaterialDocumentItem(id: string, dto: MaterialDocumentItemUpdate): Promise<MaterialDocumentItem> {
  return request<MaterialDocumentItem>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料凭证行项目
 * @param {string} id 物料凭证行项目ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDocumentItemById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料凭证行项目
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDocumentItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料凭证行项目作废状态
 * @param {MaterialDocumentItemObsolete} dto 作废 DTO
 * @returns {Promise<MaterialDocumentItem>} 物料凭证行项目DTO
 */
export function updateMaterialDocumentItemObsolete(dto: MaterialDocumentItemObsolete): Promise<MaterialDocumentItem> {
  return request<MaterialDocumentItem>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料凭证明细选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialDocumentItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/options`,
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
export function getMaterialDocumentItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料凭证行项目
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialDocumentItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/import`,
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
 * 导出物料凭证行项目
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialDocumentItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DOCUMENT_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
