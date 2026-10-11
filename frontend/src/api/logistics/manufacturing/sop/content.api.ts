// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：content.api.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块 API（自动生成，请勿手改路由常量）
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
  SopContent,
  SopContentCreate,
  SopContentUpdate
} from '@/types/logistics/manufacturing/sop/content.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopContents
 */
const SOP_CONTENT_API_BASE = 'TaktSopContents';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP多语言正文列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopContent>>} 分页结果
 */
export function getSopContentList(queryDto: any): Promise<TaktPagedResult<SopContent>> {
  return request<TaktPagedResult<SopContent>>({
    url: `${SOP_CONTENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP多语言正文
 * @param {string} id SOP多语言正文ID
 * @returns {Promise<SopContent>} SOP多语言正文DTO
 */
export function getSopContentById(id: string): Promise<SopContent> {
  return request<SopContent>({
    url: `${SOP_CONTENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP多语言正文
 * @param {SopContentCreate} dto 创建DTO
 * @returns {Promise<SopContent>} SOP多语言正文DTO
 */
export function createSopContent(dto: SopContentCreate): Promise<SopContent> {
  return request<SopContent>({
    url: `${SOP_CONTENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP多语言正文
 * @param {string} id SOP多语言正文ID
 * @param {SopContentUpdate} dto 更新DTO
 * @returns {Promise<SopContent>} SOP多语言正文DTO
 */
export function updateSopContent(id: string, dto: SopContentUpdate): Promise<SopContent> {
  return request<SopContent>({
    url: `${SOP_CONTENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP多语言正文
 * @param {string} id SOP多语言正文ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopContentById(id: string): Promise<void> {
  return request({
    url: `${SOP_CONTENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP多语言正文
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopContentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_CONTENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP多语言正文选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopContentOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_CONTENT_API_BASE}/options`,
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
export function getSopContentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_CONTENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP多语言正文
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopContent(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_CONTENT_API_BASE}/import`,
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
 * 导出SOP多语言正文
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopContent(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_CONTENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
