// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：plant.api.ts
// 创建时间：2026-08-21
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
  Plant,
  PlantCreate,
  PlantSort,
  PlantStatus,
  PlantUpdate
} from '@/types/logistics/materials/plant.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPlants
 */
const PLANT_API_BASE = 'TaktPlants';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工厂列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Plant>>} 分页结果
 */
export function getPlantList(queryDto: any): Promise<TaktPagedResult<Plant>> {
  return request<TaktPagedResult<Plant>>({
    url: `${PLANT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工厂
 * @param {string} id 工厂ID
 * @returns {Promise<Plant>} 工厂DTO
 */
export function getPlantById(id: string): Promise<Plant> {
  return request<Plant>({
    url: `${PLANT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工厂
 * @param {PlantCreate} dto 创建DTO
 * @returns {Promise<Plant>} 工厂DTO
 */
export function createPlant(dto: PlantCreate): Promise<Plant> {
  return request<Plant>({
    url: `${PLANT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工厂
 * @param {string} id 工厂ID
 * @param {PlantUpdate} dto 更新DTO
 * @returns {Promise<Plant>} 工厂DTO
 */
export function updatePlant(id: string, dto: PlantUpdate): Promise<Plant> {
  return request<Plant>({
    url: `${PLANT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工厂
 * @param {string} id 工厂ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePlantById(id: string): Promise<void> {
  return request({
    url: `${PLANT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工厂
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePlantBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PLANT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工厂状态
 * @param {PlantStatus} dto 状态 DTO
 * @returns {Promise<Plant>} 工厂DTO
 */
export function updatePlantStatus(dto: PlantStatus): Promise<Plant> {
  return request<Plant>({
    url: `${PLANT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新工厂排序
 * @param {PlantSort} dto 排序DTO
 * @returns {Promise<Plant>} 工厂DTO
 */
export function updatePlantSort(dto: PlantSort): Promise<Plant> {
  return request<Plant>({
    url: `${PLANT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工厂选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPlantOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PLANT_API_BASE}/options`,
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
export function getPlantTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PLANT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工厂
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPlant(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PLANT_API_BASE}/import`,
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
 * 导出工厂
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPlant(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PLANT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
