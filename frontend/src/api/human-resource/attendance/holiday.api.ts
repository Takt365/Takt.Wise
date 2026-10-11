// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/attendance
// 文件名称：holiday.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块 API（自动生成，请勿手改路由常量）
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
  Holiday,
  HolidayCreate,
  HolidayUpdate,
  HolidayTheme
} from '@/types/human-resource/attendance/holiday.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktHolidays
 */
const HOLIDAY_API_BASE = 'TaktHolidays';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取假日信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Holiday>>} 分页结果
 */
export function getHolidayList(queryDto: any): Promise<TaktPagedResult<Holiday>> {
  return request<TaktPagedResult<Holiday>>({
    url: `${HOLIDAY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取假日信息
 * @param {string} id 假日信息ID
 * @returns {Promise<Holiday>} 假日信息DTO
 */
export function getHolidayById(id: string): Promise<Holiday> {
  return request<Holiday>({
    url: `${HOLIDAY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建假日信息
 * @param {HolidayCreate} dto 创建DTO
 * @returns {Promise<Holiday>} 假日信息DTO
 */
export function createHoliday(dto: HolidayCreate): Promise<Holiday> {
  return request<Holiday>({
    url: `${HOLIDAY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新假日信息
 * @param {string} id 假日信息ID
 * @param {HolidayUpdate} dto 更新DTO
 * @returns {Promise<Holiday>} 假日信息DTO
 */
export function updateHoliday(id: string, dto: HolidayUpdate): Promise<Holiday> {
  return request<Holiday>({
    url: `${HOLIDAY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除假日信息
 * @param {string} id 假日信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteHolidayById(id: string): Promise<void> {
  return request({
    url: `${HOLIDAY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除假日信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteHolidayBatch(ids: string[]): Promise<void> {
  return request({
    url: `${HOLIDAY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取假日信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getHolidayOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${HOLIDAY_API_BASE}/options`,
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
export function getHolidayTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${HOLIDAY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入假日信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importHoliday(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${HOLIDAY_API_BASE}/import`,
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
 * 导出假日信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportHoliday(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${HOLIDAY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 假日主题（登录前预览）
// ========================================

/**
 * 获取服务器当日、指定租户与公司下的假日主题色与问候信息（登录前预览，须 X-Tenant-Code）
 * @param {string} tenantCode 租户编码（与登录页已校验租户一致）
 * @param {string} companyCode 公司编码（由 getLoginPreviewLocale 解析的默认公司）
 * @returns {Promise<HolidayTheme>} 假日主题
 */
export function getHolidayTheme(tenantCode: string, companyCode: string): Promise<HolidayTheme> {
  return request<HolidayTheme>({
    url: `${HOLIDAY_API_BASE}/theme`,
    method: 'get',
    params: {
      tenantCode,
      companyCode,
    },
    skipTokenRefresh: true,
    skipLoginAuthError: true,
  });
}