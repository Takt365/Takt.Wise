// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：group.api.ts
// 创建时间：2026-10-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块 API（自动生成，请勿手改路由常量）
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
  ComplaintGroup,
  ComplaintGroupCreate,
  ComplaintGroupSort,
  ComplaintGroupStatus,
  ComplaintGroupUpdate
} from '@/types/logistics/quality/complaint/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktComplaintGroups
 */
const COMPLAINT_GROUP_API_BASE = 'TaktComplaintGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客诉组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ComplaintGroup>>} 分页结果
 */
export function getComplaintGroupList(queryDto: any): Promise<TaktPagedResult<ComplaintGroup>> {
  return request<TaktPagedResult<ComplaintGroup>>({
    url: `${COMPLAINT_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客诉组主数据
 * @param {string} id 客诉组主数据ID
 * @returns {Promise<ComplaintGroup>} 客诉组主数据DTO
 */
export function getComplaintGroupById(id: string): Promise<ComplaintGroup> {
  return request<ComplaintGroup>({
    url: `${COMPLAINT_GROUP_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客诉组主数据
 * @param {ComplaintGroupCreate} dto 创建DTO
 * @returns {Promise<ComplaintGroup>} 客诉组主数据DTO
 */
export function createComplaintGroup(dto: ComplaintGroupCreate): Promise<ComplaintGroup> {
  return request<ComplaintGroup>({
    url: `${COMPLAINT_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客诉组主数据
 * @param {string} id 客诉组主数据ID
 * @param {ComplaintGroupUpdate} dto 更新DTO
 * @returns {Promise<ComplaintGroup>} 客诉组主数据DTO
 */
export function updateComplaintGroup(id: string, dto: ComplaintGroupUpdate): Promise<ComplaintGroup> {
  return request<ComplaintGroup>({
    url: `${COMPLAINT_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客诉组主数据
 * @param {string} id 客诉组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteComplaintGroupById(id: string): Promise<void> {
  return request({
    url: `${COMPLAINT_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客诉组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteComplaintGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COMPLAINT_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客诉组主数据状态
 * @param {ComplaintGroupStatus} dto 状态 DTO
 * @returns {Promise<ComplaintGroup>} 客诉组主数据DTO
 */
export function updateComplaintGroupStatus(dto: ComplaintGroupStatus): Promise<ComplaintGroup> {
  return request<ComplaintGroup>({
    url: `${COMPLAINT_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客诉组主数据排序
 * @param {ComplaintGroupSort} dto 排序DTO
 * @returns {Promise<ComplaintGroup>} 客诉组主数据DTO
 */
export function updateComplaintGroupSort(dto: ComplaintGroupSort): Promise<ComplaintGroup> {
  return request<ComplaintGroup>({
    url: `${COMPLAINT_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客诉组主数据选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @param {number} complaintGroupCategory complaintGroupCategory
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getComplaintGroupOptions(
  plantCode?: string,
  keyword?: string,
  complaintGroupCategory?: number
): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${COMPLAINT_GROUP_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword,
      complaintGroupCategory
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
export function getComplaintGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COMPLAINT_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客诉组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importComplaintGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COMPLAINT_GROUP_API_BASE}/import`,
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
 * 导出客诉组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportComplaintGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COMPLAINT_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
