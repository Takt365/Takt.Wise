// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/news-center
// 文件名称：newsShare.api.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/news-center 模块 API（自动生成，请勿手改路由常量）
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
  NewsShare,
  NewsShareCreate,
  NewsShareObsolete,
  NewsShareUpdate
} from '@/types/routine/news-center/newsShare.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNewsShares
 */
const NEWS_SHARE_API_BASE = 'TaktNewsShares';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取新闻中心分享记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<NewsShare>>} 分页结果
 */
export function getNewsShareList(queryDto: any): Promise<TaktPagedResult<NewsShare>> {
  return request<TaktPagedResult<NewsShare>>({
    url: `${NEWS_SHARE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取新闻中心分享记录
 * @param {string} id 新闻中心分享记录ID
 * @returns {Promise<NewsShare>} 新闻中心分享记录DTO
 */
export function getNewsShareById(id: string): Promise<NewsShare> {
  return request<NewsShare>({
    url: `${NEWS_SHARE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建新闻中心分享记录
 * @param {NewsShareCreate} dto 创建DTO
 * @returns {Promise<NewsShare>} 新闻中心分享记录DTO
 */
export function createNewsShare(dto: NewsShareCreate): Promise<NewsShare> {
  return request<NewsShare>({
    url: `${NEWS_SHARE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新新闻中心分享记录
 * @param {string} id 新闻中心分享记录ID
 * @param {NewsShareUpdate} dto 更新DTO
 * @returns {Promise<NewsShare>} 新闻中心分享记录DTO
 */
export function updateNewsShare(id: string, dto: NewsShareUpdate): Promise<NewsShare> {
  return request<NewsShare>({
    url: `${NEWS_SHARE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除新闻中心分享记录
 * @param {string} id 新闻中心分享记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsShareById(id: string): Promise<void> {
  return request({
    url: `${NEWS_SHARE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除新闻中心分享记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsShareBatch(ids: string[]): Promise<void> {
  return request({
    url: `${NEWS_SHARE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新新闻中心分享记录作废状态
 * @param {NewsShareObsolete} dto 作废 DTO
 * @returns {Promise<NewsShare>} 新闻中心分享记录DTO
 */
export function updateNewsShareObsolete(dto: NewsShareObsolete): Promise<NewsShare> {
  return request<NewsShare>({
    url: `${NEWS_SHARE_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取新闻分享记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getNewsShareOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${NEWS_SHARE_API_BASE}/options`,
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
export function getNewsShareTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_SHARE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入新闻中心分享记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importNewsShare(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${NEWS_SHARE_API_BASE}/import`,
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
 * 导出新闻中心分享记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportNewsShare(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_SHARE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
