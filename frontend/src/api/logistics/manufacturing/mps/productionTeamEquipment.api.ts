// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mps
// 文件名称：productionTeamEquipment.api.ts
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
  ProductionTeamEquipment,
  ProductionTeamEquipmentCreate,
  ProductionTeamEquipmentObsolete,
  ProductionTeamEquipmentStatus,
  ProductionTeamEquipmentUpdate
} from '@/types/logistics/manufacturing/mps/productionTeamEquipment.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionTeamEquipments
 */
const PRODUCTION_TEAM_EQUIPMENT_API_BASE = 'TaktProductionTeamEquipments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产班组设备组列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionTeamEquipment>>} 分页结果
 */
export function getProductionTeamEquipmentList(queryDto: any): Promise<TaktPagedResult<ProductionTeamEquipment>> {
  return request<TaktPagedResult<ProductionTeamEquipment>>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产班组设备组
 * @param {string} id 生产班组设备组ID
 * @returns {Promise<ProductionTeamEquipment>} 生产班组设备组DTO
 */
export function getProductionTeamEquipmentById(id: string): Promise<ProductionTeamEquipment> {
  return request<ProductionTeamEquipment>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产班组设备组
 * @param {ProductionTeamEquipmentCreate} dto 创建DTO
 * @returns {Promise<ProductionTeamEquipment>} 生产班组设备组DTO
 */
export function createProductionTeamEquipment(dto: ProductionTeamEquipmentCreate): Promise<ProductionTeamEquipment> {
  return request<ProductionTeamEquipment>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产班组设备组
 * @param {string} id 生产班组设备组ID
 * @param {ProductionTeamEquipmentUpdate} dto 更新DTO
 * @returns {Promise<ProductionTeamEquipment>} 生产班组设备组DTO
 */
export function updateProductionTeamEquipment(id: string, dto: ProductionTeamEquipmentUpdate): Promise<ProductionTeamEquipment> {
  return request<ProductionTeamEquipment>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产班组设备组
 * @param {string} id 生产班组设备组ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionTeamEquipmentById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产班组设备组
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionTeamEquipmentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产班组设备组状态
 * @param {ProductionTeamEquipmentStatus} dto 状态 DTO
 * @returns {Promise<ProductionTeamEquipment>} 生产班组设备组DTO
 */
export function updateProductionTeamEquipmentStatus(dto: ProductionTeamEquipmentStatus): Promise<ProductionTeamEquipment> {
  return request<ProductionTeamEquipment>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新生产班组设备组作废状态
 * @param {ProductionTeamEquipmentObsolete} dto 作废 DTO
 * @returns {Promise<ProductionTeamEquipment>} 生产班组设备组DTO
 */
export function updateProductionTeamEquipmentObsolete(dto: ProductionTeamEquipmentObsolete): Promise<ProductionTeamEquipment> {
  return request<ProductionTeamEquipment>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产班组设备组选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionTeamEquipmentOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/options`,
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
export function getProductionTeamEquipmentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产班组设备组
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionTeamEquipment(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/import`,
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
 * 导出生产班组设备组
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionTeamEquipment(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_TEAM_EQUIPMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
