// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：standardOperationTime.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  StandardOperationTime,
  StandardOperationTimeCreate,
  StandardOperationTimeUpdate
} from '@/types/logistics/manufacturing/bom/standardOperationTime.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStandardOperationTimes
 */
const STANDARD_OPERATION_TIME_API_BASE = 'TaktStandardOperationTimes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取标准工时列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StandardOperationTime>>} 分页结果
 */
export function getStandardOperationTimeList(queryDto: any): Promise<TaktPagedResult<StandardOperationTime>> {
  return request<TaktPagedResult<StandardOperationTime>>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取标准工时
 * @param {string} id 标准工时ID
 * @returns {Promise<StandardOperationTime>} 标准工时DTO
 */
export function getStandardOperationTimeById(id: string): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 根据物料编码获取当前有效的标准工时列表
 * @param {string} materialCode 物料编码
 * @param {string} plantCode 工厂代码（可选）
 * @param {string} prodDate 生产日期（可选；未传时按当天解析有效期）
 * @returns {Promise<StandardOperationTime[]>} 标准工时 DTO 列表
 */
export function getStandardOperationTimeByMaterial(
  materialCode: string,
  plantCode?: string,
  prodDate?: string
): Promise<StandardOperationTime[]> {
  return request<StandardOperationTime[]>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/by-material`,
    method: 'get',
    params: {
      materialCode,
      plantCode,
      prodDate
    },
  });
}

/**
 * 创建标准工时
 * @param {StandardOperationTimeCreate} dto 创建DTO
 * @returns {Promise<StandardOperationTime>} 标准工时DTO
 */
export function createStandardOperationTime(dto: StandardOperationTimeCreate): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新标准工时
 * @param {string} id 标准工时ID
 * @param {StandardOperationTimeUpdate} dto 更新DTO
 * @returns {Promise<StandardOperationTime>} 标准工时DTO
 */
export function updateStandardOperationTime(id: string, dto: StandardOperationTimeUpdate): Promise<StandardOperationTime> {
  return request<StandardOperationTime>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除标准工时
 * @param {string} id 标准工时ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationTimeById(id: string): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除标准工时
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationTimeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取标准工时选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardOperationTimeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

/**
 * 根据物料编码获取当前有效的标准工时选项列表
 * @param {string} materialCode 物料编码
 * @param {string} plantCode 工厂代码（可选）
 * @param {string} prodDate 生产日期（可选；未传时按当天解析有效期）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardOperationTimeOptionsByMaterial(
  materialCode: string,
  plantCode?: string,
  prodDate?: string
): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/options-by-material`,
    method: 'get',
    params: {
      materialCode,
      plantCode,
      prodDate
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
export function getStandardOperationTimeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入标准工时
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importStandardOperationTime(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/import`,
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
 * 导出标准工时
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStandardOperationTime(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_TIME_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
