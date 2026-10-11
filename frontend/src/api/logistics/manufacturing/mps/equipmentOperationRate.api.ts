// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mps
// 文件名称：equipmentOperationRate.api.ts
// 创建时间：2026-07-13
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
  EquipmentOperationRate,
  EquipmentOperationRateCreate,
  EquipmentOperationRateStatus,
  EquipmentOperationRateUpdate
} from '@/types/logistics/manufacturing/mps/equipmentOperationRate.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEquipmentOperationRates
 */
const EQUIPMENT_OPERATION_RATE_API_BASE = 'TaktEquipmentOperationRates';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取机器稼动率列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EquipmentOperationRate>>} 分页结果
 */
export function getEquipmentOperationRateList(queryDto: any): Promise<TaktPagedResult<EquipmentOperationRate>> {
  return request<TaktPagedResult<EquipmentOperationRate>>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取机器稼动率
 * @param {string} id 机器稼动率ID
 * @returns {Promise<EquipmentOperationRate>} 机器稼动率DTO
 */
export function getEquipmentOperationRateById(id: string): Promise<EquipmentOperationRate> {
  return request<EquipmentOperationRate>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建机器稼动率
 * @param {EquipmentOperationRateCreate} dto 创建DTO
 * @returns {Promise<EquipmentOperationRate>} 机器稼动率DTO
 */
export function createEquipmentOperationRate(dto: EquipmentOperationRateCreate): Promise<EquipmentOperationRate> {
  return request<EquipmentOperationRate>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新机器稼动率
 * @param {string} id 机器稼动率ID
 * @param {EquipmentOperationRateUpdate} dto 更新DTO
 * @returns {Promise<EquipmentOperationRate>} 机器稼动率DTO
 */
export function updateEquipmentOperationRate(id: string, dto: EquipmentOperationRateUpdate): Promise<EquipmentOperationRate> {
  return request<EquipmentOperationRate>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除机器稼动率
 * @param {string} id 机器稼动率ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEquipmentOperationRateById(id: string): Promise<void> {
  return request({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除机器稼动率
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEquipmentOperationRateBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新机器稼动率状态
 * @param {EquipmentOperationRateStatus} dto 状态 DTO
 * @returns {Promise<EquipmentOperationRate>} 机器稼动率DTO
 */
export function updateEquipmentOperationRateStatus(dto: EquipmentOperationRateStatus): Promise<EquipmentOperationRate> {
  return request<EquipmentOperationRate>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取机器稼动率选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEquipmentOperationRateOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/options`,
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
export function getEquipmentOperationRateTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入机器稼动率
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEquipmentOperationRate(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/import`,
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
 * 导出机器稼动率
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEquipmentOperationRate(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EQUIPMENT_OPERATION_RATE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
