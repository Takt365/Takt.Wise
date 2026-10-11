// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：countersignDetail.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  CountersignDetail,
  CountersignDetailCreate,
  CountersignDetailObsolete,
  CountersignDetailUpdate
} from '@/types/accounting/financial/countersignDetail.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCountersignDetails
 */
const COUNTERSIGN_DETAIL_API_BASE = 'TaktCountersignDetails';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会签单明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CountersignDetail>>} 分页结果
 */
export function getCountersignDetailList(queryDto: any): Promise<TaktPagedResult<CountersignDetail>> {
  return request<TaktPagedResult<CountersignDetail>>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会签单明细
 * @param {string} id 会签单明细ID
 * @returns {Promise<CountersignDetail>} 会签单明细DTO
 */
export function getCountersignDetailById(id: string): Promise<CountersignDetail> {
  return request<CountersignDetail>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建会签单明细
 * @param {CountersignDetailCreate} dto 创建DTO
 * @returns {Promise<CountersignDetail>} 会签单明细DTO
 */
export function createCountersignDetail(dto: CountersignDetailCreate): Promise<CountersignDetail> {
  return request<CountersignDetail>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会签单明细
 * @param {string} id 会签单明细ID
 * @param {CountersignDetailUpdate} dto 更新DTO
 * @returns {Promise<CountersignDetail>} 会签单明细DTO
 */
export function updateCountersignDetail(id: string, dto: CountersignDetailUpdate): Promise<CountersignDetail> {
  return request<CountersignDetail>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会签单明细
 * @param {string} id 会签单明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCountersignDetailById(id: string): Promise<void> {
  return request({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会签单明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCountersignDetailBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会签单明细作废状态
 * @param {CountersignDetailObsolete} dto 作废 DTO
 * @returns {Promise<CountersignDetail>} 会签单明细DTO
 */
export function updateCountersignDetailObsolete(dto: CountersignDetailObsolete): Promise<CountersignDetail> {
  return request<CountersignDetail>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会签单明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCountersignDetailOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/options`,
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
export function getCountersignDetailTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会签单明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCountersignDetail(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/import`,
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
 * 导出会签单明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCountersignDetail(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COUNTERSIGN_DETAIL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
