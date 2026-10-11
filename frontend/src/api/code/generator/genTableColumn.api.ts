// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/generator
// 文件名称：genTableColumn.api.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：code/generator 模块 API（自动生成，请勿手改路由常量）
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
  GenTableColumn,
  GenTableColumnCreate,
  GenTableColumnUpdate
} from '@/types/code/generator/genTableColumn.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktGenTableColumns
 */
const GEN_TABLE_COLUMN_API_BASE = 'TaktGenTableColumns';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取代码生成数据表列配置列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<GenTableColumn>>} 分页结果
 */
export function getGenTableColumnList(queryDto: any): Promise<TaktPagedResult<GenTableColumn>> {
  return request<TaktPagedResult<GenTableColumn>>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取代码生成数据表列配置
 * @param {string} id 代码生成数据表列配置ID
 * @returns {Promise<GenTableColumn>} 代码生成数据表列配置DTO
 */
export function getGenTableColumnById(id: string): Promise<GenTableColumn> {
  return request<GenTableColumn>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建代码生成数据表列配置
 * @param {GenTableColumnCreate} dto 创建DTO
 * @returns {Promise<GenTableColumn>} 代码生成数据表列配置DTO
 */
export function createGenTableColumn(dto: GenTableColumnCreate): Promise<GenTableColumn> {
  return request<GenTableColumn>({
    url: `${GEN_TABLE_COLUMN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新代码生成数据表列配置
 * @param {string} id 代码生成数据表列配置ID
 * @param {GenTableColumnUpdate} dto 更新DTO
 * @returns {Promise<GenTableColumn>} 代码生成数据表列配置DTO
 */
export function updateGenTableColumn(id: string, dto: GenTableColumnUpdate): Promise<GenTableColumn> {
  return request<GenTableColumn>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除代码生成数据表列配置
 * @param {string} id 代码生成数据表列配置ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteGenTableColumnById(id: string): Promise<void> {
  return request({
    url: `${GEN_TABLE_COLUMN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除代码生成数据表列配置
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteGenTableColumnBatch(ids: string[]): Promise<void> {
  return request({
    url: `${GEN_TABLE_COLUMN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取代码生成字段配置选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getGenTableColumnOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/options`,
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
export function getGenTableColumnTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入代码生成数据表列配置
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importGenTableColumn(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${GEN_TABLE_COLUMN_API_BASE}/import`,
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
 * 导出代码生成数据表列配置
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportGenTableColumn(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${GEN_TABLE_COLUMN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
