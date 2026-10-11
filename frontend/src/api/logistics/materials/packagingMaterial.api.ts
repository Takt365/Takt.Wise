// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：packagingMaterial.api.ts
// 创建时间：2026-08-11
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
  PackagingMaterial,
  PackagingMaterialCreate,
  PackagingMaterialSort,
  PackagingMaterialUpdate
} from '@/types/logistics/materials/packagingMaterial.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPackagingMaterials
 */
const PACKAGING_MATERIAL_API_BASE = 'TaktPackagingMaterials';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取包装物料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PackagingMaterial>>} 分页结果
 */
export function getPackagingMaterialList(queryDto: any): Promise<TaktPagedResult<PackagingMaterial>> {
  return request<TaktPagedResult<PackagingMaterial>>({
    url: `${PACKAGING_MATERIAL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取包装物料
 * @param {string} id 包装物料ID
 * @returns {Promise<PackagingMaterial>} 包装物料DTO
 */
export function getPackagingMaterialById(id: string): Promise<PackagingMaterial> {
  return request<PackagingMaterial>({
    url: `${PACKAGING_MATERIAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建包装物料
 * @param {PackagingMaterialCreate} dto 创建DTO
 * @returns {Promise<PackagingMaterial>} 包装物料DTO
 */
export function createPackagingMaterial(dto: PackagingMaterialCreate): Promise<PackagingMaterial> {
  return request<PackagingMaterial>({
    url: `${PACKAGING_MATERIAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新包装物料
 * @param {string} id 包装物料ID
 * @param {PackagingMaterialUpdate} dto 更新DTO
 * @returns {Promise<PackagingMaterial>} 包装物料DTO
 */
export function updatePackagingMaterial(id: string, dto: PackagingMaterialUpdate): Promise<PackagingMaterial> {
  return request<PackagingMaterial>({
    url: `${PACKAGING_MATERIAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除包装物料
 * @param {string} id 包装物料ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePackagingMaterialById(id: string): Promise<void> {
  return request({
    url: `${PACKAGING_MATERIAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除包装物料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePackagingMaterialBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PACKAGING_MATERIAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新包装物料排序
 * @param {PackagingMaterialSort} dto 排序DTO
 * @returns {Promise<PackagingMaterial>} 包装物料DTO
 */
export function updatePackagingMaterialSort(dto: PackagingMaterialSort): Promise<PackagingMaterial> {
  return request<PackagingMaterial>({
    url: `${PACKAGING_MATERIAL_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取包装物料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPackagingMaterialOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PACKAGING_MATERIAL_API_BASE}/options`,
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
export function getPackagingMaterialTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PACKAGING_MATERIAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入包装物料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPackagingMaterial(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PACKAGING_MATERIAL_API_BASE}/import`,
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
 * 导出包装物料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPackagingMaterial(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PACKAGING_MATERIAL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
