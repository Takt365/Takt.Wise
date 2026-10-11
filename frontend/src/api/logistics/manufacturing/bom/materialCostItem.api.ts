// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialCostItem.api.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  BomMaterialCostItem,
  BomMaterialCostItemCreate,
  BomMaterialCostItemUpdate,
} from '@/types/logistics/manufacturing/bom/materialCostItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBomMaterialCostItems
 */
const BOM_MATERIAL_COST_ITEM_API_BASE = 'TaktBomMaterialCostItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取BOM物料成本明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BomMaterialCostItem>>} 分页结果
 */
export function getBomMaterialCostItemList(queryDto: any): Promise<TaktPagedResult<BomMaterialCostItem>> {
  return request<TaktPagedResult<BomMaterialCostItem>>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function getBomMaterialCostItemById(id: string): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建BOM物料成本明细
 * @param {BomMaterialCostItemCreate} dto 创建DTO
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function createBomMaterialCostItem(dto: BomMaterialCostItemCreate): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @param {BomMaterialCostItemUpdate} dto 更新DTO
 * @returns {Promise<BomMaterialCostItem>} BOM物料成本明细DTO
 */
export function updateBomMaterialCostItem(id: string, dto: BomMaterialCostItemUpdate): Promise<BomMaterialCostItem> {
  return request<BomMaterialCostItem>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除BOM物料成本明细
 * @param {string} id BOM物料成本明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostItemById(id: string): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除BOM物料成本明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBomMaterialCostItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBomMaterialCostItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/options`,
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
export function getBomMaterialCostItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入BOM物料成本明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBomMaterialCostItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/import`,
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
 * 导出BOM物料成本明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBomMaterialCostItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
