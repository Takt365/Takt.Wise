// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：assyOutputDetail.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  AssyOutputDetail,
  AssyOutputDetailCreate,
  AssyOutputDetailObsolete,
  AssyOutputDetailUpdate
} from '@/types/logistics/manufacturing/output/assyOutputDetail.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssyOutputDetails
 */
const ASSY_OUTPUT_DETAIL_API_BASE = 'TaktAssyOutputDetails';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取组立日报明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AssyOutputDetail>>} 分页结果
 */
export function getAssyOutputDetailList(queryDto: any): Promise<TaktPagedResult<AssyOutputDetail>> {
  return request<TaktPagedResult<AssyOutputDetail>>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取组立日报明细
 * @param {string} id 组立日报明细ID
 * @returns {Promise<AssyOutputDetail>} 组立日报明细DTO
 */
export function getAssyOutputDetailById(id: string): Promise<AssyOutputDetail> {
  return request<AssyOutputDetail>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建组立日报明细
 * @param {AssyOutputDetailCreate} dto 创建DTO
 * @returns {Promise<AssyOutputDetail>} 组立日报明细DTO
 */
export function createAssyOutputDetail(dto: AssyOutputDetailCreate): Promise<AssyOutputDetail> {
  return request<AssyOutputDetail>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新组立日报明细
 * @param {string} id 组立日报明细ID
 * @param {AssyOutputDetailUpdate} dto 更新DTO
 * @returns {Promise<AssyOutputDetail>} 组立日报明细DTO
 */
export function updateAssyOutputDetail(id: string, dto: AssyOutputDetailUpdate): Promise<AssyOutputDetail> {
  return request<AssyOutputDetail>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除组立日报明细
 * @param {string} id 组立日报明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyOutputDetailById(id: string): Promise<void> {
  return request({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除组立日报明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssyOutputDetailBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新组立日报明细作废状态
 * @param {AssyOutputDetailObsolete} dto 作废 DTO
 * @returns {Promise<AssyOutputDetail>} 组立日报明细DTO
 */
export function updateAssyOutputDetailObsolete(dto: AssyOutputDetailObsolete): Promise<AssyOutputDetail> {
  return request<AssyOutputDetail>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取组立日报明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssyOutputDetailOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/options`,
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
export function getAssyOutputDetailTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入组立日报明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAssyOutputDetail(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/import`,
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
 * 导出组立日报明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAssyOutputDetail(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSY_OUTPUT_DETAIL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
