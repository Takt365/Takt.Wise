// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/news-center
// 文件名称：newsComment.api.ts
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
  TaktTreeSelectOption
} from '@/types/common.types';
import type {
  NewsComment,
  NewsCommentCreate,
  NewsCommentObsolete,
  NewsCommentStatus,
  NewsCommentTree,
  NewsCommentUpdate
} from '@/types/routine/news-center/newsComment.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNewsComments
 */
const NEWS_COMMENT_API_BASE = 'TaktNewsComments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取新闻中心评论列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<NewsComment>>} 分页结果
 */
export function getNewsCommentList(queryDto: any): Promise<TaktPagedResult<NewsComment>> {
  return request<TaktPagedResult<NewsComment>>({
    url: `${NEWS_COMMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取新闻中心评论
 * @param {string} id 新闻中心评论ID
 * @returns {Promise<NewsComment>} 新闻中心评论DTO
 */
export function getNewsCommentById(id: string): Promise<NewsComment> {
  return request<NewsComment>({
    url: `${NEWS_COMMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取新闻中心评论树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<NewsCommentTree[]>} 树形数据
 */
export function getNewsCommentTree(parentId: string, includeDisabled: boolean): Promise<NewsCommentTree[]> {
  return request<NewsCommentTree[]>({
    url: `${NEWS_COMMENT_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建新闻中心评论
 * @param {NewsCommentCreate} dto 创建DTO
 * @returns {Promise<NewsComment>} 新闻中心评论DTO
 */
export function createNewsComment(dto: NewsCommentCreate): Promise<NewsComment> {
  return request<NewsComment>({
    url: `${NEWS_COMMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新新闻中心评论
 * @param {string} id 新闻中心评论ID
 * @param {NewsCommentUpdate} dto 更新DTO
 * @returns {Promise<NewsComment>} 新闻中心评论DTO
 */
export function updateNewsComment(id: string, dto: NewsCommentUpdate): Promise<NewsComment> {
  return request<NewsComment>({
    url: `${NEWS_COMMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除新闻中心评论
 * @param {string} id 新闻中心评论ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsCommentById(id: string): Promise<void> {
  return request({
    url: `${NEWS_COMMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除新闻中心评论
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteNewsCommentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${NEWS_COMMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新新闻中心评论状态
 * @param {NewsCommentStatus} dto 状态 DTO
 * @returns {Promise<NewsComment>} 新闻中心评论DTO
 */
export function updateNewsCommentStatus(dto: NewsCommentStatus): Promise<NewsComment> {
  return request<NewsComment>({
    url: `${NEWS_COMMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新新闻中心评论作废状态
 * @param {NewsCommentObsolete} dto 作废 DTO
 * @returns {Promise<NewsComment>} 新闻中心评论DTO
 */
export function updateNewsCommentObsolete(dto: NewsCommentObsolete): Promise<NewsComment> {
  return request<NewsComment>({
    url: `${NEWS_COMMENT_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取新闻中心评论树形选项列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getNewsCommentTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${NEWS_COMMENT_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId
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
export function getNewsCommentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_COMMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入新闻中心评论
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importNewsComment(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${NEWS_COMMENT_API_BASE}/import`,
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
 * 导出新闻中心评论
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportNewsComment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${NEWS_COMMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
