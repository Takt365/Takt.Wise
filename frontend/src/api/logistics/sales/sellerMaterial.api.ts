// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：sellerMaterial.api.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块 API（自动生成，请勿手改路由常量）
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
  SellerMaterial,
  SellerMaterialCreate,
  SellerMaterialUpdate
} from '@/types/logistics/sales/sellerMaterial.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSellerMaterials
 */
const SELLER_MATERIAL_API_BASE = 'TaktSellerMaterials';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售商物料列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SellerMaterial>>} 分页结果
 */
export function getSellerMaterialList(queryDto: any): Promise<TaktPagedResult<SellerMaterial>> {
  return request<TaktPagedResult<SellerMaterial>>({
    url: `${SELLER_MATERIAL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售商物料
 * @param {string} id 销售商物料ID
 * @returns {Promise<SellerMaterial>} 销售商物料DTO
 */
export function getSellerMaterialById(id: string): Promise<SellerMaterial> {
  return request<SellerMaterial>({
    url: `${SELLER_MATERIAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售商物料
 * @param {SellerMaterialCreate} dto 创建DTO
 * @returns {Promise<SellerMaterial>} 销售商物料DTO
 */
export function createSellerMaterial(dto: SellerMaterialCreate): Promise<SellerMaterial> {
  return request<SellerMaterial>({
    url: `${SELLER_MATERIAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售商物料
 * @param {string} id 销售商物料ID
 * @param {SellerMaterialUpdate} dto 更新DTO
 * @returns {Promise<SellerMaterial>} 销售商物料DTO
 */
export function updateSellerMaterial(id: string, dto: SellerMaterialUpdate): Promise<SellerMaterial> {
  return request<SellerMaterial>({
    url: `${SELLER_MATERIAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售商物料
 * @param {string} id 销售商物料ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSellerMaterialById(id: string): Promise<void> {
  return request({
    url: `${SELLER_MATERIAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售商物料
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSellerMaterialBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SELLER_MATERIAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售商物料选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSellerMaterialOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SELLER_MATERIAL_API_BASE}/options`,
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
export function getSellerMaterialTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SELLER_MATERIAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售商物料
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSellerMaterial(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SELLER_MATERIAL_API_BASE}/import`,
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
 * 导出销售商物料
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSellerMaterial(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SELLER_MATERIAL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
