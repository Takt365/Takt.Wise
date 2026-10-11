// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mps
// 文件名称：productionEquipment.api.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块 API（自动生成，请勿手改路由常量）
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
  ProductionEquipment,
  ProductionEquipmentCreate,
  ProductionEquipmentSort,
  ProductionEquipmentStatus,
  ProductionEquipmentUpdate
} from '@/types/logistics/manufacturing/mps/productionEquipment.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionEquipments
 */
const PRODUCTION_EQUIPMENT_API_BASE = 'TaktProductionEquipments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产设备列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionEquipment>>} 分页结果
 */
export function getProductionEquipmentList(queryDto: any): Promise<TaktPagedResult<ProductionEquipment>> {
  return request<TaktPagedResult<ProductionEquipment>>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产设备
 * @param {string} id 生产设备ID
 * @returns {Promise<ProductionEquipment>} 生产设备DTO
 */
export function getProductionEquipmentById(id: string): Promise<ProductionEquipment> {
  return request<ProductionEquipment>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产设备
 * @param {ProductionEquipmentCreate} dto 创建DTO
 * @returns {Promise<ProductionEquipment>} 生产设备DTO
 */
export function createProductionEquipment(dto: ProductionEquipmentCreate): Promise<ProductionEquipment> {
  return request<ProductionEquipment>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产设备
 * @param {string} id 生产设备ID
 * @param {ProductionEquipmentUpdate} dto 更新DTO
 * @returns {Promise<ProductionEquipment>} 生产设备DTO
 */
export function updateProductionEquipment(id: string, dto: ProductionEquipmentUpdate): Promise<ProductionEquipment> {
  return request<ProductionEquipment>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产设备
 * @param {string} id 生产设备ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionEquipmentById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产设备
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionEquipmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产设备状态
 * @param {ProductionEquipmentStatus} dto 状态 DTO
 * @returns {Promise<ProductionEquipment>} 生产设备DTO
 */
export function updateProductionEquipmentStatus(dto: ProductionEquipmentStatus): Promise<ProductionEquipment> {
  return request<ProductionEquipment>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新生产设备排序
 * @param {ProductionEquipmentSort} dto 排序DTO
 * @returns {Promise<ProductionEquipment>} 生产设备DTO
 */
export function updateProductionEquipmentSort(dto: ProductionEquipmentSort): Promise<ProductionEquipment> {
  return request<ProductionEquipment>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产设备选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionEquipmentOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/options`,
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
export function getProductionEquipmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产设备
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionEquipment(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/import`,
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
 * 导出生产设备
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionEquipment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_EQUIPMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
