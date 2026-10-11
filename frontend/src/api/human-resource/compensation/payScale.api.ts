// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：payScale.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块 API（自动生成，请勿手改路由常量）
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
  PayScale,
  PayScaleCreate,
  PayScaleSort,
  PayScaleStatus,
  PayScaleUpdate
} from '@/types/human-resource/compensation/payScale.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPayScales
 */
const PAY_SCALE_API_BASE = 'TaktPayScales';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取薪级列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PayScale>>} 分页结果
 */
export function getPayScaleList(queryDto: any): Promise<TaktPagedResult<PayScale>> {
  return request<TaktPagedResult<PayScale>>({
    url: `${PAY_SCALE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取薪级
 * @param {string} id 薪级ID
 * @returns {Promise<PayScale>} 薪级DTO
 */
export function getPayScaleById(id: string): Promise<PayScale> {
  return request<PayScale>({
    url: `${PAY_SCALE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建薪级
 * @param {PayScaleCreate} dto 创建DTO
 * @returns {Promise<PayScale>} 薪级DTO
 */
export function createPayScale(dto: PayScaleCreate): Promise<PayScale> {
  return request<PayScale>({
    url: `${PAY_SCALE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新薪级
 * @param {string} id 薪级ID
 * @param {PayScaleUpdate} dto 更新DTO
 * @returns {Promise<PayScale>} 薪级DTO
 */
export function updatePayScale(id: string, dto: PayScaleUpdate): Promise<PayScale> {
  return request<PayScale>({
    url: `${PAY_SCALE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除薪级
 * @param {string} id 薪级ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePayScaleById(id: string): Promise<void> {
  return request({
    url: `${PAY_SCALE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除薪级
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePayScaleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PAY_SCALE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新薪级状态
 * @param {PayScaleStatus} dto 状态 DTO
 * @returns {Promise<PayScale>} 薪级DTO
 */
export function updatePayScaleStatus(dto: PayScaleStatus): Promise<PayScale> {
  return request<PayScale>({
    url: `${PAY_SCALE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新薪级排序
 * @param {PayScaleSort} dto 排序DTO
 * @returns {Promise<PayScale>} 薪级DTO
 */
export function updatePayScaleSort(dto: PayScaleSort): Promise<PayScale> {
  return request<PayScale>({
    url: `${PAY_SCALE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取薪级选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPayScaleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PAY_SCALE_API_BASE}/options`,
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
export function getPayScaleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PAY_SCALE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入薪级
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPayScale(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PAY_SCALE_API_BASE}/import`,
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
 * 导出薪级
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPayScale(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PAY_SCALE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
