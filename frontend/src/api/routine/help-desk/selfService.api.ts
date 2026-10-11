// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：selfService.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块 API（自动生成，请勿手改路由常量）
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
  SelfService,
  SelfServiceCreate,
  SelfServiceSort,
  SelfServiceStatus,
  SelfServiceUpdate
} from '@/types/routine/help-desk/selfService.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSelfServices
 */
const SELF_SERVICE_API_BASE = 'TaktSelfServices';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取自助服务列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SelfService>>} 分页结果
 */
export function getSelfServiceList(queryDto: any): Promise<TaktPagedResult<SelfService>> {
  return request<TaktPagedResult<SelfService>>({
    url: `${SELF_SERVICE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取自助服务
 * @param {string} id 自助服务ID
 * @returns {Promise<SelfService>} 自助服务DTO
 */
export function getSelfServiceById(id: string): Promise<SelfService> {
  return request<SelfService>({
    url: `${SELF_SERVICE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建自助服务
 * @param {SelfServiceCreate} dto 创建DTO
 * @returns {Promise<SelfService>} 自助服务DTO
 */
export function createSelfService(dto: SelfServiceCreate): Promise<SelfService> {
  return request<SelfService>({
    url: `${SELF_SERVICE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新自助服务
 * @param {string} id 自助服务ID
 * @param {SelfServiceUpdate} dto 更新DTO
 * @returns {Promise<SelfService>} 自助服务DTO
 */
export function updateSelfService(id: string, dto: SelfServiceUpdate): Promise<SelfService> {
  return request<SelfService>({
    url: `${SELF_SERVICE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除自助服务
 * @param {string} id 自助服务ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSelfServiceById(id: string): Promise<void> {
  return request({
    url: `${SELF_SERVICE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除自助服务
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSelfServiceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SELF_SERVICE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新自助服务状态
 * @param {SelfServiceStatus} dto 状态 DTO
 * @returns {Promise<SelfService>} 自助服务DTO
 */
export function updateSelfServiceStatus(dto: SelfServiceStatus): Promise<SelfService> {
  return request<SelfService>({
    url: `${SELF_SERVICE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新自助服务排序
 * @param {SelfServiceSort} dto 排序DTO
 * @returns {Promise<SelfService>} 自助服务DTO
 */
export function updateSelfServiceSort(dto: SelfServiceSort): Promise<SelfService> {
  return request<SelfService>({
    url: `${SELF_SERVICE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取自助服务选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSelfServiceOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SELF_SERVICE_API_BASE}/options`,
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
export function getSelfServiceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SELF_SERVICE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入自助服务
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSelfService(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SELF_SERVICE_API_BASE}/import`,
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
 * 导出自助服务
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSelfService(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SELF_SERVICE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
