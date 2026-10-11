// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/organization
// 文件名称：post.api.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/organization 模块 API（自动生成，请勿手改路由常量）
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
  Post,
  PostCreate,
  PostSort,
  PostStatus,
  PostUpdate
} from '@/types/human-resource/organization/post.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPosts
 */
const POST_API_BASE = 'TaktPosts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取岗位列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Post>>} 分页结果
 */
export function getPostList(queryDto: any): Promise<TaktPagedResult<Post>> {
  return request<TaktPagedResult<Post>>({
    url: `${POST_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取岗位
 * @param {string} id 岗位ID
 * @returns {Promise<Post>} 岗位DTO
 */
export function getPostById(id: string): Promise<Post> {
  return request<Post>({
    url: `${POST_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建岗位
 * @param {PostCreate} dto 创建DTO
 * @returns {Promise<Post>} 岗位DTO
 */
export function createPost(dto: PostCreate): Promise<Post> {
  return request<Post>({
    url: `${POST_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新岗位
 * @param {string} id 岗位ID
 * @param {PostUpdate} dto 更新DTO
 * @returns {Promise<Post>} 岗位DTO
 */
export function updatePost(id: string, dto: PostUpdate): Promise<Post> {
  return request<Post>({
    url: `${POST_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除岗位
 * @param {string} id 岗位ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePostById(id: string): Promise<void> {
  return request({
    url: `${POST_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除岗位
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePostBatch(ids: string[]): Promise<void> {
  return request({
    url: `${POST_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新岗位状态
 * @param {PostStatus} dto 状态 DTO
 * @returns {Promise<Post>} 岗位DTO
 */
export function updatePostStatus(dto: PostStatus): Promise<Post> {
  return request<Post>({
    url: `${POST_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新岗位排序
 * @param {PostSort} dto 排序DTO
 * @returns {Promise<Post>} 岗位DTO
 */
export function updatePostSort(dto: PostSort): Promise<Post> {
  return request<Post>({
    url: `${POST_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取岗位选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPostOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${POST_API_BASE}/options`,
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
export function getPostTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${POST_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入岗位
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPost(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${POST_API_BASE}/import`,
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
 * 导出岗位
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPost(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${POST_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
