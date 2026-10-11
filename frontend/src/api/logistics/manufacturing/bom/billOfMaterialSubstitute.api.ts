// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：billOfMaterialSubstitute.api.ts
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
  BillOfMaterialSubstitute,
  BillOfMaterialSubstituteCreate,
  BillOfMaterialSubstituteObsolete,
  BillOfMaterialSubstituteUpdate
} from '@/types/logistics/manufacturing/bom/billOfMaterialSubstitute.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBillOfMaterialSubstitutes
 */
const BILL_OF_MATERIAL_SUBSTITUTE_API_BASE = 'TaktBillOfMaterialSubstitutes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取BOM替代料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BillOfMaterialSubstitute>>} 分页结果
 */
export function getBillOfMaterialSubstituteList(queryDto: any): Promise<TaktPagedResult<BillOfMaterialSubstitute>> {
  return request<TaktPagedResult<BillOfMaterialSubstitute>>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取BOM替代料
 * @param {string} id BOM替代料ID
 * @returns {Promise<BillOfMaterialSubstitute>} BOM替代料DTO
 */
export function getBillOfMaterialSubstituteById(id: string): Promise<BillOfMaterialSubstitute> {
  return request<BillOfMaterialSubstitute>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建BOM替代料
 * @param {BillOfMaterialSubstituteCreate} dto 创建DTO
 * @returns {Promise<BillOfMaterialSubstitute>} BOM替代料DTO
 */
export function createBillOfMaterialSubstitute(dto: BillOfMaterialSubstituteCreate): Promise<BillOfMaterialSubstitute> {
  return request<BillOfMaterialSubstitute>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新BOM替代料
 * @param {string} id BOM替代料ID
 * @param {BillOfMaterialSubstituteUpdate} dto 更新DTO
 * @returns {Promise<BillOfMaterialSubstitute>} BOM替代料DTO
 */
export function updateBillOfMaterialSubstitute(id: string, dto: BillOfMaterialSubstituteUpdate): Promise<BillOfMaterialSubstitute> {
  return request<BillOfMaterialSubstitute>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除BOM替代料
 * @param {string} id BOM替代料ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialSubstituteById(id: string): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除BOM替代料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialSubstituteBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新BOM替代料作废状态
 * @param {BillOfMaterialSubstituteObsolete} dto 作废 DTO
 * @returns {Promise<BillOfMaterialSubstitute>} BOM替代料DTO
 */
export function updateBillOfMaterialSubstituteObsolete(dto: BillOfMaterialSubstituteObsolete): Promise<BillOfMaterialSubstitute> {
  return request<BillOfMaterialSubstitute>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取BOM替代料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBillOfMaterialSubstituteOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/options`,
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
export function getBillOfMaterialSubstituteTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入BOM替代料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBillOfMaterialSubstitute(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/import`,
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
 * 导出BOM替代料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBillOfMaterialSubstitute(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_SUBSTITUTE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
