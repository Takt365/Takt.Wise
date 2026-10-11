// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：group.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  QualityGroup,
  QualityGroupCreate,
  QualityGroupSort,
  QualityGroupStatus,
  QualityGroupUpdate
} from '@/types/logistics/quality/operation/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityGroups
 */
const QUALITY_GROUP_API_BASE = 'TaktQualityGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取质量组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityGroup>>} 分页结果
 */
export function getQualityGroupList(queryDto: any): Promise<TaktPagedResult<QualityGroup>> {
  return request<TaktPagedResult<QualityGroup>>({
    url: `${QUALITY_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取质量组主数据
 * @param {string} id 质量组主数据ID
 * @returns {Promise<QualityGroup>} 质量组主数据DTO
 */
export function getQualityGroupById(id: string): Promise<QualityGroup> {
  return request<QualityGroup>({
    url: `${QUALITY_GROUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建质量组主数据
 * @param {QualityGroupCreate} dto 创建DTO
 * @returns {Promise<QualityGroup>} 质量组主数据DTO
 */
export function createQualityGroup(dto: QualityGroupCreate): Promise<QualityGroup> {
  return request<QualityGroup>({
    url: `${QUALITY_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新质量组主数据
 * @param {string} id 质量组主数据ID
 * @param {QualityGroupUpdate} dto 更新DTO
 * @returns {Promise<QualityGroup>} 质量组主数据DTO
 */
export function updateQualityGroup(id: string, dto: QualityGroupUpdate): Promise<QualityGroup> {
  return request<QualityGroup>({
    url: `${QUALITY_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除质量组主数据
 * @param {string} id 质量组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityGroupById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除质量组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新质量组主数据状态
 * @param {QualityGroupStatus} dto 状态 DTO
 * @returns {Promise<QualityGroup>} 质量组主数据DTO
 */
export function updateQualityGroupStatus(dto: QualityGroupStatus): Promise<QualityGroup> {
  return request<QualityGroup>({
    url: `${QUALITY_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新质量组主数据排序
 * @param {QualityGroupSort} dto 排序DTO
 * @returns {Promise<QualityGroup>} 质量组主数据DTO
 */
export function updateQualityGroupSort(dto: QualityGroupSort): Promise<QualityGroup> {
  return request<QualityGroup>({
    url: `${QUALITY_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取质量组主数据选项列表
 * @param plantCode 工厂代码（可选）
 * @param keyword 搜索关键字（可选）
 * @param qualityGroupCategory 质量组类别（可选）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityGroupOptions(
  plantCode?: string,
  keyword?: string,
  qualityGroupCategory?: number
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${QUALITY_GROUP_API_BASE}/options`,
    method: 'get',
    params: {
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
      ...(qualityGroupCategory != null ? { qualityGroupCategory } : {}),
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
export function getQualityGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入质量组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_GROUP_API_BASE}/import`,
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
 * 导出质量组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
