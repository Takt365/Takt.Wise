// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mrp
// 文件名称：purchasePlan.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块 API（自动生成，请勿手改路由常量）
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
  PurchasePlan,
  PurchasePlanCreate,
  PurchasePlanStatus,
  PurchasePlanUpdate
} from '@/types/logistics/manufacturing/mrp/purchasePlan.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchasePlans
 */
const PURCHASE_PLAN_API_BASE = 'TaktPurchasePlans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购计划列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchasePlan>>} 分页结果
 */
export function getPurchasePlanList(queryDto: any): Promise<TaktPagedResult<PurchasePlan>> {
  return request<TaktPagedResult<PurchasePlan>>({
    url: `${PURCHASE_PLAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购计划
 * @param {string} id 采购计划ID
 * @returns {Promise<PurchasePlan>} 采购计划DTO
 */
export function getPurchasePlanById(id: string): Promise<PurchasePlan> {
  return request<PurchasePlan>({
    url: `${PURCHASE_PLAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购计划
 * @param {PurchasePlanCreate} dto 创建DTO
 * @returns {Promise<PurchasePlan>} 采购计划DTO
 */
export function createPurchasePlan(dto: PurchasePlanCreate): Promise<PurchasePlan> {
  return request<PurchasePlan>({
    url: `${PURCHASE_PLAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购计划
 * @param {string} id 采购计划ID
 * @param {PurchasePlanUpdate} dto 更新DTO
 * @returns {Promise<PurchasePlan>} 采购计划DTO
 */
export function updatePurchasePlan(id: string, dto: PurchasePlanUpdate): Promise<PurchasePlan> {
  return request<PurchasePlan>({
    url: `${PURCHASE_PLAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购计划
 * @param {string} id 采购计划ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePlanById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_PLAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购计划
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchasePlanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_PLAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购计划状态
 * @param {PurchasePlanStatus} dto 状态 DTO
 * @returns {Promise<PurchasePlan>} 采购计划DTO
 */
export function updatePurchasePlanStatus(dto: PurchasePlanStatus): Promise<PurchasePlan> {
  return request<PurchasePlan>({
    url: `${PURCHASE_PLAN_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购计划选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchasePlanOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_PLAN_API_BASE}/options`,
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
export function getPurchasePlanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PLAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购计划
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchasePlan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_PLAN_API_BASE}/import`,
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
 * 导出采购计划
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchasePlan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PLAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
