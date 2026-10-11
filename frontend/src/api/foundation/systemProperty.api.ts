// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：systemProperty.api.ts
// 创建时间：2026-09-16
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
  SystemProperty,
  SystemPropertyCreate,
  SystemPropertySort,
  SystemPropertyStatus,
  SystemPropertyUpdate
} from '@/types/foundation/systemProperty.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSystemProperties
 */
const SYSTEM_PROPERTY_API_BASE = 'TaktSystemProperties';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取系统属性列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SystemProperty>>} 分页结果
 */
export function getSystemPropertyList(queryDto: any): Promise<TaktPagedResult<SystemProperty>> {
  return request<TaktPagedResult<SystemProperty>>({
    url: `${SYSTEM_PROPERTY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取系统属性
 * @param {string} id 系统属性ID
 * @returns {Promise<SystemProperty>} 系统属性DTO
 */
export function getSystemPropertyById(id: string): Promise<SystemProperty> {
  return request<SystemProperty>({
    url: `${SYSTEM_PROPERTY_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建系统属性
 * @param {SystemPropertyCreate} dto 创建DTO
 * @returns {Promise<SystemProperty>} 系统属性DTO
 */
export function createSystemProperty(dto: SystemPropertyCreate): Promise<SystemProperty> {
  return request<SystemProperty>({
    url: `${SYSTEM_PROPERTY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新系统属性
 * @param {string} id 系统属性ID
 * @param {SystemPropertyUpdate} dto 更新DTO
 * @returns {Promise<SystemProperty>} 系统属性DTO
 */
export function updateSystemProperty(id: string, dto: SystemPropertyUpdate): Promise<SystemProperty> {
  return request<SystemProperty>({
    url: `${SYSTEM_PROPERTY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除系统属性
 * @param {string} id 系统属性ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSystemPropertyById(id: string): Promise<void> {
  return request({
    url: `${SYSTEM_PROPERTY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除系统属性
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSystemPropertyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SYSTEM_PROPERTY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新系统属性状态
 * @param {SystemPropertyStatus} dto 状态 DTO
 * @returns {Promise<SystemProperty>} 系统属性DTO
 */
export function updateSystemPropertyStatus(dto: SystemPropertyStatus): Promise<SystemProperty> {
  return request<SystemProperty>({
    url: `${SYSTEM_PROPERTY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新系统属性排序
 * @param {SystemPropertySort} dto 排序DTO
 * @returns {Promise<SystemProperty>} 系统属性DTO
 */
export function updateSystemPropertySort(dto: SystemPropertySort): Promise<SystemProperty> {
  return request<SystemProperty>({
    url: `${SYSTEM_PROPERTY_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取系统属性选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSystemPropertyOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${SYSTEM_PROPERTY_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getSystemPropertyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SYSTEM_PROPERTY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入系统属性
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSystemProperty(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SYSTEM_PROPERTY_API_BASE}/import`,
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
 * 导出系统属性
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSystemProperty(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SYSTEM_PROPERTY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
