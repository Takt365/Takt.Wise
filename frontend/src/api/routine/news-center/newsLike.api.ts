// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/news-center
// 文件名称：newsLike.api.ts
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
  NewsLike,
  NewsLikeCreate,
  NewsLikeObsolete,
  NewsLikeUpdate
} from '@/types/routine/news-center/newsLike.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNewsLikes
 */
const NEWS_LIKE_API_BASE = 'TaktNewsLikes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取新闻中心点赞记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<NewsLike>>} 分页结果
 */
export function getNewsLikeList(queryDto: any): Promise<TaktPagedResult<NewsLike>> {
  return request<TaktPagedResult<NewsLike>>({
    url: `${NEWS_LIKE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取新闻中心点赞记录
 * @param {string} id 新闻中心点赞记录ID
 * @returns {Promise<NewsLike>} 新闻中心点赞记录DTO
 */
export function getNewsLikeById(id: string): Promise<NewsLike> {
  return request<NewsLike>({
    url: `${NEWS_LIKE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建新闻中心点赞记录
 * @param {NewsLikeCreate} dto 创建DTO
 * @returns {Promise<NewsLike>} 新闻中心点赞记录DTO
 */
export function createNewsLike(dto: NewsLikeCreate): Promise<NewsLike> {
  return request<NewsLike>({
    url: `${NEWS_LIKE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新新闻中心点赞记录
 * @param {string} id 新闻中心点赞记录ID
 * @param {NewsLikeUpdate} dto 更新DTO
 * @returns {Promise<NewsLike>} 新闻中心点赞记录DTO
 */
export function updateNewsLike(id: string, dto: NewsLikeUpdate): Promise<NewsLike> {
  return request<NewsLike>({
    url: `${NEWS_LIKE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除新闻中心点赞记录
 * @param {string} id 新闻中心点赞记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsLikeById(id: string): Promise<void> {
  return request({
    url: `${NEWS_LIKE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除新闻中心点赞记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsLikeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${NEWS_LIKE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新新闻中心点赞记录作废状态
 * @param {NewsLikeObsolete} dto 作废 DTO
 * @returns {Promise<NewsLike>} 新闻中心点赞记录DTO
 */
export function updateNewsLikeObsolete(dto: NewsLikeObsolete): Promise<NewsLike> {
  return request<NewsLike>({
    url: `${NEWS_LIKE_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取新闻点赞记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getNewsLikeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${NEWS_LIKE_API_BASE}/options`,
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
export function getNewsLikeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_LIKE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入新闻中心点赞记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importNewsLike(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${NEWS_LIKE_API_BASE}/import`,
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
 * 导出新闻中心点赞记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportNewsLike(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_LIKE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
