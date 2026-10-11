// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mrp
// 文件名称：materialRequirementsPlanningItem.api.ts
// 创建时间：2026-07-23
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
  MaterialRequirementsPlanningItem,
  MaterialRequirementsPlanningItemCreate,
  MaterialRequirementsPlanningItemObsolete,
  MaterialRequirementsPlanningItemUpdate
} from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanningItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialRequirementsPlanningItems
 */
const MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE = 'TaktMaterialRequirementsPlanningItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料需求计划MRP明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialRequirementsPlanningItem>>} 分页结果
 */
export function getMaterialRequirementsPlanningItemList(queryDto: any): Promise<TaktPagedResult<MaterialRequirementsPlanningItem>> {
  return request<TaktPagedResult<MaterialRequirementsPlanningItem>>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料需求计划MRP明细
 * @param {string} id 物料需求计划MRP明细ID
 * @returns {Promise<MaterialRequirementsPlanningItem>} 物料需求计划MRP明细DTO
 */
export function getMaterialRequirementsPlanningItemById(id: string): Promise<MaterialRequirementsPlanningItem> {
  return request<MaterialRequirementsPlanningItem>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料需求计划MRP明细
 * @param {MaterialRequirementsPlanningItemCreate} dto 创建DTO
 * @returns {Promise<MaterialRequirementsPlanningItem>} 物料需求计划MRP明细DTO
 */
export function createMaterialRequirementsPlanningItem(dto: MaterialRequirementsPlanningItemCreate): Promise<MaterialRequirementsPlanningItem> {
  return request<MaterialRequirementsPlanningItem>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料需求计划MRP明细
 * @param {string} id 物料需求计划MRP明细ID
 * @param {MaterialRequirementsPlanningItemUpdate} dto 更新DTO
 * @returns {Promise<MaterialRequirementsPlanningItem>} 物料需求计划MRP明细DTO
 */
export function updateMaterialRequirementsPlanningItem(id: string, dto: MaterialRequirementsPlanningItemUpdate): Promise<MaterialRequirementsPlanningItem> {
  return request<MaterialRequirementsPlanningItem>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料需求计划MRP明细
 * @param {string} id 物料需求计划MRP明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialRequirementsPlanningItemById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料需求计划MRP明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialRequirementsPlanningItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料需求计划MRP明细作废状态
 * @param {MaterialRequirementsPlanningItemObsolete} dto 作废 DTO
 * @returns {Promise<MaterialRequirementsPlanningItem>} 物料需求计划MRP明细DTO
 */
export function updateMaterialRequirementsPlanningItemObsolete(dto: MaterialRequirementsPlanningItemObsolete): Promise<MaterialRequirementsPlanningItem> {
  return request<MaterialRequirementsPlanningItem>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料需求计划MRP明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialRequirementsPlanningItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/options`,
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
export function getMaterialRequirementsPlanningItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料需求计划MRP明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialRequirementsPlanningItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/import`,
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
 * 导出物料需求计划MRP明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialRequirementsPlanningItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
