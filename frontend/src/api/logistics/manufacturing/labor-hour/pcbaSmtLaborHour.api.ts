// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/labor-hour
// 文件名称：pcbaSmtLaborHour.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/labor-hour 模块 API（自动生成，请勿手改路由常量）
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
  PcbaSmtLaborHour,
  PcbaSmtLaborHourCreate,
  PcbaSmtLaborHourUpdate
} from '@/types/logistics/manufacturing/labor-hour/pcbaSmtLaborHour.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPcbaSmtLaborHours
 */
const PCBA_SMT_LABOR_HOUR_API_BASE = 'TaktPcbaSmtLaborHours';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取PCBA SMT工数统计列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PcbaSmtLaborHour>>} 分页结果
 */
export function getPcbaSmtLaborHourList(queryDto: any): Promise<TaktPagedResult<PcbaSmtLaborHour>> {
  return request<TaktPagedResult<PcbaSmtLaborHour>>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取PCBA SMT工数统计
 * @param {string} id PCBA SMT工数统计ID
 * @returns {Promise<PcbaSmtLaborHour>} PCBA SMT工数统计DTO
 */
export function getPcbaSmtLaborHourById(id: string): Promise<PcbaSmtLaborHour> {
  return request<PcbaSmtLaborHour>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建PCBA SMT工数统计
 * @param {PcbaSmtLaborHourCreate} dto 创建DTO
 * @returns {Promise<PcbaSmtLaborHour>} PCBA SMT工数统计DTO
 */
export function createPcbaSmtLaborHour(dto: PcbaSmtLaborHourCreate): Promise<PcbaSmtLaborHour> {
  return request<PcbaSmtLaborHour>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新PCBA SMT工数统计
 * @param {string} id PCBA SMT工数统计ID
 * @param {PcbaSmtLaborHourUpdate} dto 更新DTO
 * @returns {Promise<PcbaSmtLaborHour>} PCBA SMT工数统计DTO
 */
export function updatePcbaSmtLaborHour(id: string, dto: PcbaSmtLaborHourUpdate): Promise<PcbaSmtLaborHour> {
  return request<PcbaSmtLaborHour>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除PCBA SMT工数统计
 * @param {string} id PCBA SMT工数统计ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaSmtLaborHourById(id: string): Promise<void> {
  return request({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除PCBA SMT工数统计
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaSmtLaborHourBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取PCBA SMT工数统计选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPcbaSmtLaborHourOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/options`,
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
export function getPcbaSmtLaborHourTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入PCBA SMT工数统计
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPcbaSmtLaborHour(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/import`,
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
 * 导出PCBA SMT工数统计
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPcbaSmtLaborHour(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_SMT_LABOR_HOUR_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
