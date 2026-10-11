// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：materialDescription.api.ts
// 创建时间：2026-08-05
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
  MaterialDescription,
  MaterialDescriptionCreate,
  MaterialDescriptionUpdate
} from '@/types/logistics/materials/materialDescription.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialDescriptions
 */
const MATERIAL_DESCRIPTION_API_BASE = 'TaktMaterialDescriptions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料描述列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialDescription>>} 分页结果
 */
export function getMaterialDescriptionList(queryDto: any): Promise<TaktPagedResult<MaterialDescription>> {
  return request<TaktPagedResult<MaterialDescription>>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料描述
 * @param {string} id 物料描述ID
 * @returns {Promise<MaterialDescription>} 物料描述DTO
 */
export function getMaterialDescriptionById(id: string): Promise<MaterialDescription> {
  return request<MaterialDescription>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料描述
 * @param {MaterialDescriptionCreate} dto 创建DTO
 * @returns {Promise<MaterialDescription>} 物料描述DTO
 */
export function createMaterialDescription(dto: MaterialDescriptionCreate): Promise<MaterialDescription> {
  return request<MaterialDescription>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料描述
 * @param {string} id 物料描述ID
 * @param {MaterialDescriptionUpdate} dto 更新DTO
 * @returns {Promise<MaterialDescription>} 物料描述DTO
 */
export function updateMaterialDescription(id: string, dto: MaterialDescriptionUpdate): Promise<MaterialDescription> {
  return request<MaterialDescription>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料描述
 * @param {string} id 物料描述ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDescriptionById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料描述
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialDescriptionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料描述选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialDescriptionOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/options`,
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
export function getMaterialDescriptionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料描述
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialDescription(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/import`,
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
 * 导出物料描述
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialDescription(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_DESCRIPTION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
