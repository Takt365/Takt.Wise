// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/benefits
// 文件名称：socialInsurance.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块 API（自动生成，请勿手改路由常量）
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
  SocialInsurance,
  SocialInsuranceCreate,
  SocialInsuranceStatus,
  SocialInsuranceUpdate
} from '@/types/human-resource/benefits/socialInsurance.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSocialInsurances
 */
const SOCIAL_INSURANCE_API_BASE = 'TaktSocialInsurances';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取社保公积金列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SocialInsurance>>} 分页结果
 */
export function getSocialInsuranceList(queryDto: any): Promise<TaktPagedResult<SocialInsurance>> {
  return request<TaktPagedResult<SocialInsurance>>({
    url: `${SOCIAL_INSURANCE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取社保公积金
 * @param {string} id 社保公积金ID
 * @returns {Promise<SocialInsurance>} 社保公积金DTO
 */
export function getSocialInsuranceById(id: string): Promise<SocialInsurance> {
  return request<SocialInsurance>({
    url: `${SOCIAL_INSURANCE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建社保公积金
 * @param {SocialInsuranceCreate} dto 创建DTO
 * @returns {Promise<SocialInsurance>} 社保公积金DTO
 */
export function createSocialInsurance(dto: SocialInsuranceCreate): Promise<SocialInsurance> {
  return request<SocialInsurance>({
    url: `${SOCIAL_INSURANCE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新社保公积金
 * @param {string} id 社保公积金ID
 * @param {SocialInsuranceUpdate} dto 更新DTO
 * @returns {Promise<SocialInsurance>} 社保公积金DTO
 */
export function updateSocialInsurance(id: string, dto: SocialInsuranceUpdate): Promise<SocialInsurance> {
  return request<SocialInsurance>({
    url: `${SOCIAL_INSURANCE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除社保公积金
 * @param {string} id 社保公积金ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSocialInsuranceById(id: string): Promise<void> {
  return request({
    url: `${SOCIAL_INSURANCE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除社保公积金
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSocialInsuranceBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOCIAL_INSURANCE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新社保公积金状态
 * @param {SocialInsuranceStatus} dto 状态 DTO
 * @returns {Promise<SocialInsurance>} 社保公积金DTO
 */
export function updateSocialInsuranceStatus(dto: SocialInsuranceStatus): Promise<SocialInsurance> {
  return request<SocialInsurance>({
    url: `${SOCIAL_INSURANCE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取社保公积金选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSocialInsuranceOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOCIAL_INSURANCE_API_BASE}/options`,
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
export function getSocialInsuranceTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOCIAL_INSURANCE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入社保公积金
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSocialInsurance(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOCIAL_INSURANCE_API_BASE}/import`,
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
 * 导出社保公积金
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSocialInsurance(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOCIAL_INSURANCE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
