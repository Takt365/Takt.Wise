// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：bonusPlan.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块 API（自动生成，请勿手改路由常量）
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
  BonusPlan,
  BonusPlanCreate,
  BonusPlanStatus,
  BonusPlanUpdate
} from '@/types/human-resource/compensation/bonusPlan.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBonusPlans
 */
const BONUS_PLAN_API_BASE = 'TaktBonusPlans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取奖金方案列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BonusPlan>>} 分页结果
 */
export function getBonusPlanList(queryDto: any): Promise<TaktPagedResult<BonusPlan>> {
  return request<TaktPagedResult<BonusPlan>>({
    url: `${BONUS_PLAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取奖金方案
 * @param {string} id 奖金方案ID
 * @returns {Promise<BonusPlan>} 奖金方案DTO
 */
export function getBonusPlanById(id: string): Promise<BonusPlan> {
  return request<BonusPlan>({
    url: `${BONUS_PLAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建奖金方案
 * @param {BonusPlanCreate} dto 创建DTO
 * @returns {Promise<BonusPlan>} 奖金方案DTO
 */
export function createBonusPlan(dto: BonusPlanCreate): Promise<BonusPlan> {
  return request<BonusPlan>({
    url: `${BONUS_PLAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新奖金方案
 * @param {string} id 奖金方案ID
 * @param {BonusPlanUpdate} dto 更新DTO
 * @returns {Promise<BonusPlan>} 奖金方案DTO
 */
export function updateBonusPlan(id: string, dto: BonusPlanUpdate): Promise<BonusPlan> {
  return request<BonusPlan>({
    url: `${BONUS_PLAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除奖金方案
 * @param {string} id 奖金方案ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBonusPlanById(id: string): Promise<void> {
  return request({
    url: `${BONUS_PLAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除奖金方案
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBonusPlanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BONUS_PLAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新奖金方案状态
 * @param {BonusPlanStatus} dto 状态 DTO
 * @returns {Promise<BonusPlan>} 奖金方案DTO
 */
export function updateBonusPlanStatus(dto: BonusPlanStatus): Promise<BonusPlan> {
  return request<BonusPlan>({
    url: `${BONUS_PLAN_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取奖金方案选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBonusPlanOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BONUS_PLAN_API_BASE}/options`,
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
export function getBonusPlanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BONUS_PLAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入奖金方案
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBonusPlan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BONUS_PLAN_API_BASE}/import`,
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
 * 导出奖金方案
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBonusPlan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BONUS_PLAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
