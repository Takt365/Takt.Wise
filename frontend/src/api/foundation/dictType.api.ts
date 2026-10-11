// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：dictType.api.ts
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  DictType,
  DictTypeBuiltIn,
  DictTypeCreate,
  DictTypeSort,
  DictTypeStatus,
  DictTypeUpdate
} from '@/types/foundation/dictType.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDictTypes
 */
const DICT_TYPE_API_BASE = 'TaktDictTypes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取字典类型列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DictType>>} 分页结果
 */
export function getDictTypeList(queryDto: any): Promise<TaktPagedResult<DictType>> {
  return request<TaktPagedResult<DictType>>({
    url: `${DICT_TYPE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取字典类型
 * @param {string} id 字典类型ID
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function getDictTypeById(id: string): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建字典类型
 * @param {DictTypeCreate} dto 创建DTO
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function createDictType(dto: DictTypeCreate): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新字典类型
 * @param {string} id 字典类型ID
 * @param {DictTypeUpdate} dto 更新DTO
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function updateDictType(id: string, dto: DictTypeUpdate): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除字典类型
 * @param {string} id 字典类型ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDictTypeById(id: string): Promise<void> {
  return request({
    url: `${DICT_TYPE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除字典类型
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDictTypeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DICT_TYPE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新字典类型状态
 * @param {DictTypeStatus} dto 状态DTO
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function updateDictTypeStatus(dto: DictTypeStatus): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新字典类型内置
 * @param {DictTypeBuiltIn} dto 内置 DTO
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function updateDictTypeBuiltIn(dto: DictTypeBuiltIn): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}/built-in`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新字典类型排序
 * @param {DictTypeSort} dto 排序DTO
 * @returns {Promise<DictType>} 字典类型DTO
 */
export function updateDictTypeSort(dto: DictTypeSort): Promise<DictType> {
  return request<DictType>({
    url: `${DICT_TYPE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取字典类型选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDictTypeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DICT_TYPE_API_BASE}/options`,
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
export function getDictTypeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DICT_TYPE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入字典类型
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDictType(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DICT_TYPE_API_BASE}/import`,
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
 * 导出字典类型
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDictType(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DICT_TYPE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
