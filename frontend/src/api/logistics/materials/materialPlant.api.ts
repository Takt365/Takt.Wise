// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：materialPlant.api.ts
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
  MaterialPlant,
  MaterialPlantCreate,
  MaterialPlantStatus,
  MaterialPlantUpdate
} from '@/types/logistics/materials/materialPlant.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialPlants
 */
const MATERIAL_PLANT_API_BASE = 'TaktMaterialPlants';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工厂物料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialPlant>>} 分页结果
 */
export function getMaterialPlantList(queryDto: any): Promise<TaktPagedResult<MaterialPlant>> {
  return request<TaktPagedResult<MaterialPlant>>({
    url: `${MATERIAL_PLANT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工厂物料
 * @param {string} id 工厂物料ID
 * @returns {Promise<MaterialPlant>} 工厂物料DTO
 */
export function getMaterialPlantById(id: string): Promise<MaterialPlant> {
  return request<MaterialPlant>({
    url: `${MATERIAL_PLANT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工厂物料
 * @param {MaterialPlantCreate} dto 创建DTO
 * @returns {Promise<MaterialPlant>} 工厂物料DTO
 */
export function createMaterialPlant(dto: MaterialPlantCreate): Promise<MaterialPlant> {
  return request<MaterialPlant>({
    url: `${MATERIAL_PLANT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工厂物料
 * @param {string} id 工厂物料ID
 * @param {MaterialPlantUpdate} dto 更新DTO
 * @returns {Promise<MaterialPlant>} 工厂物料DTO
 */
export function updateMaterialPlant(id: string, dto: MaterialPlantUpdate): Promise<MaterialPlant> {
  return request<MaterialPlant>({
    url: `${MATERIAL_PLANT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工厂物料
 * @param {string} id 工厂物料ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialPlantById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_PLANT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工厂物料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialPlantBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_PLANT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工厂物料状态
 * @param {MaterialPlantStatus} dto 状态 DTO
 * @returns {Promise<MaterialPlant>} 工厂物料DTO
 */
export function updateMaterialPlantStatus(dto: MaterialPlantStatus): Promise<MaterialPlant> {
  return request<MaterialPlant>({
    url: `${MATERIAL_PLANT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialPlantOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_PLANT_API_BASE}/options`,
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
export function getMaterialPlantTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_PLANT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工厂物料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialPlant(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_PLANT_API_BASE}/import`,
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
 * 导出工厂物料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialPlant(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_PLANT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
