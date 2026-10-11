// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mrp
// 文件名称：productionPlanItem.api.ts
// 创建时间：2026-07-13
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
  ProductionPlanItem,
  ProductionPlanItemCreate,
  ProductionPlanItemObsolete,
  ProductionPlanItemUpdate
} from '@/types/logistics/manufacturing/mrp/productionPlanItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionPlanItems
 */
const PRODUCTION_PLAN_ITEM_API_BASE = 'TaktProductionPlanItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产计划明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionPlanItem>>} 分页结果
 */
export function getProductionPlanItemList(queryDto: any): Promise<TaktPagedResult<ProductionPlanItem>> {
  return request<TaktPagedResult<ProductionPlanItem>>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产计划明细
 * @param {string} id 生产计划明细ID
 * @returns {Promise<ProductionPlanItem>} 生产计划明细DTO
 */
export function getProductionPlanItemById(id: string): Promise<ProductionPlanItem> {
  return request<ProductionPlanItem>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产计划明细
 * @param {ProductionPlanItemCreate} dto 创建DTO
 * @returns {Promise<ProductionPlanItem>} 生产计划明细DTO
 */
export function createProductionPlanItem(dto: ProductionPlanItemCreate): Promise<ProductionPlanItem> {
  return request<ProductionPlanItem>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产计划明细
 * @param {string} id 生产计划明细ID
 * @param {ProductionPlanItemUpdate} dto 更新DTO
 * @returns {Promise<ProductionPlanItem>} 生产计划明细DTO
 */
export function updateProductionPlanItem(id: string, dto: ProductionPlanItemUpdate): Promise<ProductionPlanItem> {
  return request<ProductionPlanItem>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产计划明细
 * @param {string} id 生产计划明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionPlanItemById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产计划明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionPlanItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产计划明细作废状态
 * @param {ProductionPlanItemObsolete} dto 作废 DTO
 * @returns {Promise<ProductionPlanItem>} 生产计划明细DTO
 */
export function updateProductionPlanItemObsolete(dto: ProductionPlanItemObsolete): Promise<ProductionPlanItem> {
  return request<ProductionPlanItem>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产计划明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionPlanItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/options`,
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
export function getProductionPlanItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产计划明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionPlanItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/import`,
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
 * 导出生产计划明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionPlanItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_PLAN_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
