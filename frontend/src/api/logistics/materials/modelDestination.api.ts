// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：modelDestination.api.ts
// 创建时间：2026-06-23
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
  ModelDestination,
  ModelDestinationCreate,
  ModelDestinationSort,
  ModelDestinationUpdate
} from '@/types/logistics/materials/modelDestination.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktModelDestinations
 */
const MODEL_DESTINATION_API_BASE = 'TaktModelDestinations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取型号目的地列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ModelDestination>>} 分页结果
 */
export function getModelDestinationList(queryDto: any): Promise<TaktPagedResult<ModelDestination>> {
  return request<TaktPagedResult<ModelDestination>>({
    url: `${MODEL_DESTINATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取型号目的地
 * @param {string} id 型号目的地ID
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function getModelDestinationById(id: string): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建型号目的地
 * @param {ModelDestinationCreate} dto 创建DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function createModelDestination(dto: ModelDestinationCreate): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新型号目的地
 * @param {string} id 型号目的地ID
 * @param {ModelDestinationUpdate} dto 更新DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function updateModelDestination(id: string, dto: ModelDestinationUpdate): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除型号目的地
 * @param {string} id 型号目的地ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteModelDestinationById(id: string): Promise<void> {
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除型号目的地
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteModelDestinationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新型号目的地排序
 * @param {ModelDestinationSort} dto 排序DTO
 * @returns {Promise<ModelDestination>} 型号目的地DTO
 */
export function updateModelDestinationSort(dto: ModelDestinationSort): Promise<ModelDestination> {
  return request<ModelDestination>({
    url: `${MODEL_DESTINATION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取型号目的地选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getModelDestinationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MODEL_DESTINATION_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

/**
 * 获取机种下拉选项（ModelCode 去重）
 * @returns {Promise<TaktSelectOption[]>} 机种下拉选项
 */
export function getModelOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MODEL_DESTINATION_API_BASE}/model-options`,
    method: 'get',
  });
}

/**
 * 根据机种编码获取级联物料选项（DictValue=物料编码）
 * @param {string} modelCode 机种编码
 * @returns {Promise<TaktSelectOption[]>} 物料下拉选项
 */
export function getMaterialOptionsByModel(modelCode: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MODEL_DESTINATION_API_BASE}/material-options-by-model`,
    method: 'get',
    params: {
      modelCode,
    },
  });
}

/**
 * 根据物料编码获取机种名称与仕向地信息
 * @param {string} materialCode 物料编码
 * @returns {Promise<ModelDestination | null>} 型号目的地 DTO；未匹配时为 null
 */
export function getModelDestinationByMaterial(materialCode: string): Promise<ModelDestination | null> {
  return request<ModelDestination | null>({
    url: `${MODEL_DESTINATION_API_BASE}/by-material`,
    method: 'get',
    params: {
      materialCode,
    },
  });
}

/**
 * 根据物料编码获取机种仕向列表
 * @param {string} materialCode 物料编码
 * @returns {Promise<ModelDestination[]>} 型号目的地 DTO 列表
 */
export function getModelDestinationListByMaterial(materialCode: string): Promise<ModelDestination[]> {
  return request<ModelDestination[]>({
    url: `${MODEL_DESTINATION_API_BASE}/list-by-material`,
    method: 'get',
    params: {
      materialCode,
    },
  });
}

/**
 * 根据物料编码获取机种仕向选项列表
 * @param {string} materialCode 物料编码
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getModelDestinationOptionsByMaterial(materialCode: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MODEL_DESTINATION_API_BASE}/options-by-material`,
    method: 'get',
    params: {
      materialCode,
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
export function getModelDestinationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MODEL_DESTINATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入型号目的地
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importModelDestination(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MODEL_DESTINATION_API_BASE}/import`,
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
 * 导出型号目的地
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportModelDestination(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MODEL_DESTINATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
