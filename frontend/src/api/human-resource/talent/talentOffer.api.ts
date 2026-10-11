// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/talent
// 文件名称：talentOffer.api.ts
// 创建时间：2026-06-09
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
  TalentOffer,
  TalentOfferCreate,
  TalentOfferUpdate
} from '@/types/human-resource/talent/talentOffer.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTalentOffers
 */
const TALENT_OFFER_API_BASE = 'TaktTalentOffers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取录用信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TalentOffer>>} 分页结果
 */
export function getTalentOfferList(queryDto: any): Promise<TaktPagedResult<TalentOffer>> {
  return request<TaktPagedResult<TalentOffer>>({
    url: `${TALENT_OFFER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取录用信息
 * @param {string} id 录用信息ID
 * @returns {Promise<TalentOffer>} 录用信息DTO
 */
export function getTalentOfferById(id: string): Promise<TalentOffer> {
  return request<TalentOffer>({
    url: `${TALENT_OFFER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建录用信息
 * @param {TalentOfferCreate} dto 创建DTO
 * @returns {Promise<TalentOffer>} 录用信息DTO
 */
export function createTalentOffer(dto: TalentOfferCreate): Promise<TalentOffer> {
  return request<TalentOffer>({
    url: `${TALENT_OFFER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新录用信息
 * @param {string} id 录用信息ID
 * @param {TalentOfferUpdate} dto 更新DTO
 * @returns {Promise<TalentOffer>} 录用信息DTO
 */
export function updateTalentOffer(id: string, dto: TalentOfferUpdate): Promise<TalentOffer> {
  return request<TalentOffer>({
    url: `${TALENT_OFFER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除录用信息
 * @param {string} id 录用信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentOfferById(id: string): Promise<void> {
  return request({
    url: `${TALENT_OFFER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除录用信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTalentOfferBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TALENT_OFFER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取录用信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTalentOfferOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TALENT_OFFER_API_BASE}/options`,
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
export function getTalentOfferTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_OFFER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入录用信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTalentOffer(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TALENT_OFFER_API_BASE}/import`,
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
 * 导出录用信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTalentOffer(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TALENT_OFFER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
