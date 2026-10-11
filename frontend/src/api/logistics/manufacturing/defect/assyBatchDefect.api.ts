// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/defect
// 文件名称：assyBatchDefect.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块 API（自动生成，请勿手改路由常量）
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
  AssyBatchDefect,
  AssyBatchDefectCreate,
  AssyBatchDefectStatus,
  AssyBatchDefectUpdate
} from '@/types/logistics/manufacturing/defect/assyBatchDefect.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssyBatchDefects
 */
const ASSY_BATCH_DEFECT_API_BASE = 'TaktAssyBatchDefects';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取组立批量不良统计列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AssyBatchDefect>>} 分页结果
 */
export function getAssyBatchDefectList(queryDto: any): Promise<TaktPagedResult<AssyBatchDefect>> {
  return request<TaktPagedResult<AssyBatchDefect>>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取组立批量不良统计
 * @param {string} id 组立批量不良统计ID
 * @returns {Promise<AssyBatchDefect>} 组立批量不良统计DTO
 */
export function getAssyBatchDefectById(id: string): Promise<AssyBatchDefect> {
  return request<AssyBatchDefect>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建组立批量不良统计
 * @param {AssyBatchDefectCreate} dto 创建DTO
 * @returns {Promise<AssyBatchDefect>} 组立批量不良统计DTO
 */
export function createAssyBatchDefect(dto: AssyBatchDefectCreate): Promise<AssyBatchDefect> {
  return request<AssyBatchDefect>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新组立批量不良统计
 * @param {string} id 组立批量不良统计ID
 * @param {AssyBatchDefectUpdate} dto 更新DTO
 * @returns {Promise<AssyBatchDefect>} 组立批量不良统计DTO
 */
export function updateAssyBatchDefect(id: string, dto: AssyBatchDefectUpdate): Promise<AssyBatchDefect> {
  return request<AssyBatchDefect>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除组立批量不良统计
 * @param {string} id 组立批量不良统计ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyBatchDefectById(id: string): Promise<void> {
  return request({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除组立批量不良统计
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyBatchDefectBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新组立批量不良统计状态
 * @param {AssyBatchDefectStatus} dto 状态 DTO
 * @returns {Promise<AssyBatchDefect>} 组立批量不良统计DTO
 */
export function updateAssyBatchDefectStatus(dto: AssyBatchDefectStatus): Promise<AssyBatchDefect> {
  return request<AssyBatchDefect>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取组立批量不良统计选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssyBatchDefectOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/options`,
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
export function getAssyBatchDefectTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入组立批量不良统计
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAssyBatchDefect(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/import`,
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
 * 导出组立批量不良统计
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssyBatchDefect(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_BATCH_DEFECT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
