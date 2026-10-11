// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/talent
// 文件名称：staffingRequirement.api.ts
// 创建时间：2026-06-23
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
  TalentStaffingRequirement,
  TalentStaffingRequirementCreate,
  TalentStaffingRequirementUpdate
} from '@/types/human-resource/talent/staffingRequirement.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTalentStaffingRequirements
 */
const TALENT_STAFFING_REQUIREMENT_API_BASE = 'TaktTalentStaffingRequirements';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取用人需求列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TalentStaffingRequirement>>} 分页结果
 */
export function getTalentStaffingRequirementList(queryDto: any): Promise<TaktPagedResult<TalentStaffingRequirement>> {
  return request<TaktPagedResult<TalentStaffingRequirement>>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取用人需求
 * @param {string} id 用人需求ID
 * @returns {Promise<TalentStaffingRequirement>} 用人需求DTO
 */
export function getTalentStaffingRequirementById(id: string): Promise<TalentStaffingRequirement> {
  return request<TalentStaffingRequirement>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建用人需求
 * @param {TalentStaffingRequirementCreate} dto 创建DTO
 * @returns {Promise<TalentStaffingRequirement>} 用人需求DTO
 */
export function createTalentStaffingRequirement(dto: TalentStaffingRequirementCreate): Promise<TalentStaffingRequirement> {
  return request<TalentStaffingRequirement>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新用人需求
 * @param {string} id 用人需求ID
 * @param {TalentStaffingRequirementUpdate} dto 更新DTO
 * @returns {Promise<TalentStaffingRequirement>} 用人需求DTO
 */
export function updateTalentStaffingRequirement(id: string, dto: TalentStaffingRequirementUpdate): Promise<TalentStaffingRequirement> {
  return request<TalentStaffingRequirement>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除用人需求
 * @param {string} id 用人需求ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentStaffingRequirementById(id: string): Promise<void> {
  return request({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除用人需求
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentStaffingRequirementBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取用人需求选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTalentStaffingRequirementOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/options`,
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
export function getTalentStaffingRequirementTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入用人需求
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTalentStaffingRequirement(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/import`,
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
 * 导出用人需求
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTalentStaffingRequirement(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_STAFFING_REQUIREMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
