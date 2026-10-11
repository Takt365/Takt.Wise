// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：billOfMaterialItem.api.ts
// 创建时间：2026-08-28
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
  BillOfMaterialItem,
  BillOfMaterialItemCreate,
  BillOfMaterialItemObsolete,
  BillOfMaterialItemUpdate
} from '@/types/logistics/manufacturing/bom/billOfMaterialItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBillOfMaterialItems
 */
const BILL_OF_MATERIAL_ITEM_API_BASE = 'TaktBillOfMaterialItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料清单明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BillOfMaterialItem>>} 分页结果
 */
export function getBillOfMaterialItemList(queryDto: any): Promise<TaktPagedResult<BillOfMaterialItem>> {
  return request<TaktPagedResult<BillOfMaterialItem>>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料清单明细
 * @param {string} id 物料清单明细ID
 * @returns {Promise<BillOfMaterialItem>} 物料清单明细DTO
 */
export function getBillOfMaterialItemById(id: string): Promise<BillOfMaterialItem> {
  return request<BillOfMaterialItem>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料清单明细
 * @param {BillOfMaterialItemCreate} dto 创建DTO
 * @returns {Promise<BillOfMaterialItem>} 物料清单明细DTO
 */
export function createBillOfMaterialItem(dto: BillOfMaterialItemCreate): Promise<BillOfMaterialItem> {
  return request<BillOfMaterialItem>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料清单明细
 * @param {string} id 物料清单明细ID
 * @param {BillOfMaterialItemUpdate} dto 更新DTO
 * @returns {Promise<BillOfMaterialItem>} 物料清单明细DTO
 */
export function updateBillOfMaterialItem(id: string, dto: BillOfMaterialItemUpdate): Promise<BillOfMaterialItem> {
  return request<BillOfMaterialItem>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料清单明细
 * @param {string} id 物料清单明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialItemById(id: string): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料清单明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料清单明细作废状态
 * @param {BillOfMaterialItemObsolete} dto 作废 DTO
 * @returns {Promise<BillOfMaterialItem>} 物料清单明细DTO
 */
export function updateBillOfMaterialItemObsolete(dto: BillOfMaterialItemObsolete): Promise<BillOfMaterialItem> {
  return request<BillOfMaterialItem>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料清单明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBillOfMaterialItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/options`,
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
export function getBillOfMaterialItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料清单明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBillOfMaterialItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/import`,
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
 * 导出物料清单明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBillOfMaterialItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
