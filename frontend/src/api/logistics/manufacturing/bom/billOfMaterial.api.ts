// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：billOfMaterial.api.ts
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
  BillOfMaterial,
  BillOfMaterialCreate,
  BillOfMaterialSort,
  BillOfMaterialStatus,
  BillOfMaterialUpdate
} from '@/types/logistics/manufacturing/bom/billOfMaterial.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBillOfMaterials
 */
const BILL_OF_MATERIAL_API_BASE = 'TaktBillOfMaterials';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料清单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BillOfMaterial>>} 分页结果
 */
export function getBillOfMaterialList(queryDto: any): Promise<TaktPagedResult<BillOfMaterial>> {
  return request<TaktPagedResult<BillOfMaterial>>({
    url: `${BILL_OF_MATERIAL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料清单
 * @param {string} id 物料清单ID
 * @returns {Promise<BillOfMaterial>} 物料清单DTO
 */
export function getBillOfMaterialById(id: string): Promise<BillOfMaterial> {
  return request<BillOfMaterial>({
    url: `${BILL_OF_MATERIAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料清单
 * @param {BillOfMaterialCreate} dto 创建DTO
 * @returns {Promise<BillOfMaterial>} 物料清单DTO
 */
export function createBillOfMaterial(dto: BillOfMaterialCreate): Promise<BillOfMaterial> {
  return request<BillOfMaterial>({
    url: `${BILL_OF_MATERIAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料清单
 * @param {string} id 物料清单ID
 * @param {BillOfMaterialUpdate} dto 更新DTO
 * @returns {Promise<BillOfMaterial>} 物料清单DTO
 */
export function updateBillOfMaterial(id: string, dto: BillOfMaterialUpdate): Promise<BillOfMaterial> {
  return request<BillOfMaterial>({
    url: `${BILL_OF_MATERIAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料清单
 * @param {string} id 物料清单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialById(id: string): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料清单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBillOfMaterialBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BILL_OF_MATERIAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料清单状态
 * @param {BillOfMaterialStatus} dto 状态 DTO
 * @returns {Promise<BillOfMaterial>} 物料清单DTO
 */
export function updateBillOfMaterialStatus(dto: BillOfMaterialStatus): Promise<BillOfMaterial> {
  return request<BillOfMaterial>({
    url: `${BILL_OF_MATERIAL_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新物料清单排序
 * @param {BillOfMaterialSort} dto 排序DTO
 * @returns {Promise<BillOfMaterial>} 物料清单DTO
 */
export function updateBillOfMaterialSort(dto: BillOfMaterialSort): Promise<BillOfMaterial> {
  return request<BillOfMaterial>({
    url: `${BILL_OF_MATERIAL_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料清单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBillOfMaterialOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BILL_OF_MATERIAL_API_BASE}/options`,
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
export function getBillOfMaterialTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料清单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBillOfMaterial(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BILL_OF_MATERIAL_API_BASE}/import`,
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
 * 导出物料清单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBillOfMaterial(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BILL_OF_MATERIAL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
