// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/defect
// 文件名称：group.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块 API（自动生成，请勿手改路由常量）
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
  DefectGroup,
  DefectGroupCreate,
  DefectGroupSort,
  DefectGroupStatus,
  DefectGroupUpdate
} from '@/types/logistics/manufacturing/defect/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDefectGroups
 */
const DEFECT_GROUP_API_BASE = 'TaktDefectGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取不良组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DefectGroup>>} 分页结果
 */
export function getDefectGroupList(queryDto: any): Promise<TaktPagedResult<DefectGroup>> {
  return request<TaktPagedResult<DefectGroup>>({
    url: `${DEFECT_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取不良组主数据
 * @param {string} id 不良组主数据ID
 * @returns {Promise<DefectGroup>} 不良组主数据DTO
 */
export function getDefectGroupById(id: string): Promise<DefectGroup> {
  return request<DefectGroup>({
    url: `${DEFECT_GROUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建不良组主数据
 * @param {DefectGroupCreate} dto 创建DTO
 * @returns {Promise<DefectGroup>} 不良组主数据DTO
 */
export function createDefectGroup(dto: DefectGroupCreate): Promise<DefectGroup> {
  return request<DefectGroup>({
    url: `${DEFECT_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新不良组主数据
 * @param {string} id 不良组主数据ID
 * @param {DefectGroupUpdate} dto 更新DTO
 * @returns {Promise<DefectGroup>} 不良组主数据DTO
 */
export function updateDefectGroup(id: string, dto: DefectGroupUpdate): Promise<DefectGroup> {
  return request<DefectGroup>({
    url: `${DEFECT_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除不良组主数据
 * @param {string} id 不良组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDefectGroupById(id: string): Promise<void> {
  return request({
    url: `${DEFECT_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除不良组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDefectGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DEFECT_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新不良组主数据状态
 * @param {DefectGroupStatus} dto 状态 DTO
 * @returns {Promise<DefectGroup>} 不良组主数据DTO
 */
export function updateDefectGroupStatus(dto: DefectGroupStatus): Promise<DefectGroup> {
  return request<DefectGroup>({
    url: `${DEFECT_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新不良组主数据排序
 * @param {DefectGroupSort} dto 排序DTO
 * @returns {Promise<DefectGroup>} 不良组主数据DTO
 */
export function updateDefectGroupSort(dto: DefectGroupSort): Promise<DefectGroup> {
  return request<DefectGroup>({
    url: `${DEFECT_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取不良组主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDefectGroupOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DEFECT_GROUP_API_BASE}/options`,
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
export function getDefectGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DEFECT_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入不良组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDefectGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DEFECT_GROUP_API_BASE}/import`,
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
 * 导出不良组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDefectGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DEFECT_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
