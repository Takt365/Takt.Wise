// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：manufacturerMaterial.api.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
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
  ManufacturerMaterial,
  ManufacturerMaterialCreate,
  ManufacturerMaterialUpdate
} from '@/types/logistics/procurement/manufacturerMaterial.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktManufacturerMaterials
 */
const MANUFACTURER_MATERIAL_API_BASE = 'TaktManufacturerMaterials';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取制造商物料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ManufacturerMaterial>>} 分页结果
 */
export function getManufacturerMaterialList(queryDto: any): Promise<TaktPagedResult<ManufacturerMaterial>> {
  return request<TaktPagedResult<ManufacturerMaterial>>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取制造商物料
 * @param {string} id 制造商物料ID
 * @returns {Promise<ManufacturerMaterial>} 制造商物料DTO
 */
export function getManufacturerMaterialById(id: string): Promise<ManufacturerMaterial> {
  return request<ManufacturerMaterial>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建制造商物料
 * @param {ManufacturerMaterialCreate} dto 创建DTO
 * @returns {Promise<ManufacturerMaterial>} 制造商物料DTO
 */
export function createManufacturerMaterial(dto: ManufacturerMaterialCreate): Promise<ManufacturerMaterial> {
  return request<ManufacturerMaterial>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新制造商物料
 * @param {string} id 制造商物料ID
 * @param {ManufacturerMaterialUpdate} dto 更新DTO
 * @returns {Promise<ManufacturerMaterial>} 制造商物料DTO
 */
export function updateManufacturerMaterial(id: string, dto: ManufacturerMaterialUpdate): Promise<ManufacturerMaterial> {
  return request<ManufacturerMaterial>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除制造商物料
 * @param {string} id 制造商物料ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteManufacturerMaterialById(id: string): Promise<void> {
  return request({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除制造商物料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteManufacturerMaterialBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取制造商物料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getManufacturerMaterialOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/options`,
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
export function getManufacturerMaterialTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入制造商物料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importManufacturerMaterial(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/import`,
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
 * 导出制造商物料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportManufacturerMaterial(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MANUFACTURER_MATERIAL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
