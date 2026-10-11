// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/talent
// 文件名称：jobPosting.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块 API（自动生成，请勿手改路由常量）
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
  TalentJobPosting,
  TalentJobPostingCreate,
  TalentJobPostingStatus,
  TalentJobPostingUpdate
} from '@/types/human-resource/talent/jobPosting.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTalentJobPostings
 */
const TALENT_JOB_POSTING_API_BASE = 'TaktTalentJobPostings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取职位发布列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TalentJobPosting>>} 分页结果
 */
export function getTalentJobPostingList(queryDto: any): Promise<TaktPagedResult<TalentJobPosting>> {
  return request<TaktPagedResult<TalentJobPosting>>({
    url: `${TALENT_JOB_POSTING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取职位发布
 * @param {string} id 职位发布ID
 * @returns {Promise<TalentJobPosting>} 职位发布DTO
 */
export function getTalentJobPostingById(id: string): Promise<TalentJobPosting> {
  return request<TalentJobPosting>({
    url: `${TALENT_JOB_POSTING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建职位发布
 * @param {TalentJobPostingCreate} dto 创建DTO
 * @returns {Promise<TalentJobPosting>} 职位发布DTO
 */
export function createTalentJobPosting(dto: TalentJobPostingCreate): Promise<TalentJobPosting> {
  return request<TalentJobPosting>({
    url: `${TALENT_JOB_POSTING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新职位发布
 * @param {string} id 职位发布ID
 * @param {TalentJobPostingUpdate} dto 更新DTO
 * @returns {Promise<TalentJobPosting>} 职位发布DTO
 */
export function updateTalentJobPosting(id: string, dto: TalentJobPostingUpdate): Promise<TalentJobPosting> {
  return request<TalentJobPosting>({
    url: `${TALENT_JOB_POSTING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除职位发布
 * @param {string} id 职位发布ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentJobPostingById(id: string): Promise<void> {
  return request({
    url: `${TALENT_JOB_POSTING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除职位发布
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentJobPostingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TALENT_JOB_POSTING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新职位发布状态
 * @param {TalentJobPostingStatus} dto 状态 DTO
 * @returns {Promise<TalentJobPosting>} 职位发布DTO
 */
export function updateTalentJobPostingStatus(dto: TalentJobPostingStatus): Promise<TalentJobPosting> {
  return request<TalentJobPosting>({
    url: `${TALENT_JOB_POSTING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取职位发布选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTalentJobPostingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TALENT_JOB_POSTING_API_BASE}/options`,
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
export function getTalentJobPostingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_JOB_POSTING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入职位发布
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTalentJobPosting(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TALENT_JOB_POSTING_API_BASE}/import`,
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
 * 导出职位发布
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTalentJobPosting(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_JOB_POSTING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
