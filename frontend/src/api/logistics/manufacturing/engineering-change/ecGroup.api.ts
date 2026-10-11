// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecGroup.api.ts
// 创建时间：2026-07-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  EcGroup,
  EcGroupCreate,
  EcGroupSort,
  EcGroupStatus,
  EcGroupUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecGroup.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcGroups
 */
const EC_GROUP_API_BASE = 'TaktEcGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcGroup>>} 分页结果
 */
export function getEcGroupList(queryDto: any): Promise<TaktPagedResult<EcGroup>> {
  return request<TaktPagedResult<EcGroup>>({
    url: `${EC_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变组主数据
 * @param {string} id 设变组主数据ID
 * @returns {Promise<EcGroup>} 设变组主数据DTO
 */
export function getEcGroupById(id: string): Promise<EcGroup> {
  return request<EcGroup>({
    url: `${EC_GROUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变组主数据
 * @param {EcGroupCreate} dto 创建DTO
 * @returns {Promise<EcGroup>} 设变组主数据DTO
 */
export function createEcGroup(dto: EcGroupCreate): Promise<EcGroup> {
  return request<EcGroup>({
    url: `${EC_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变组主数据
 * @param {string} id 设变组主数据ID
 * @param {EcGroupUpdate} dto 更新DTO
 * @returns {Promise<EcGroup>} 设变组主数据DTO
 */
export function updateEcGroup(id: string, dto: EcGroupUpdate): Promise<EcGroup> {
  return request<EcGroup>({
    url: `${EC_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变组主数据
 * @param {string} id 设变组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcGroupById(id: string): Promise<void> {
  return request({
    url: `${EC_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变组主数据状态
 * @param {EcGroupStatus} dto 状态 DTO
 * @returns {Promise<EcGroup>} 设变组主数据DTO
 */
export function updateEcGroupStatus(dto: EcGroupStatus): Promise<EcGroup> {
  return request<EcGroup>({
    url: `${EC_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新设变组主数据排序
 * @param {EcGroupSort} dto 排序DTO
 * @returns {Promise<EcGroup>} 设变组主数据DTO
 */
export function updateEcGroupSort(dto: EcGroupSort): Promise<EcGroup> {
  return request<EcGroup>({
    url: `${EC_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变组主数据选项列表
 * @param plantCode 工厂代码（可选）
 * @param keyword 搜索关键字（可选）
 * @param ecGroupCategory 设变组类别（可选；部门编码，多个逗号分隔）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcGroupOptions(
  plantCode?: string,
  keyword?: string,
  ecGroupCategory?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  const category = ecGroupCategory?.trim()
  return request<TaktSelectOption[]>({
    url: `${EC_GROUP_API_BASE}/options`,
    method: 'get',
    params: {
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
      ...(category ? { ecGroupCategory: category } : {}),
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
export function getEcGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_GROUP_API_BASE}/import`,
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
 * 导出设变组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
