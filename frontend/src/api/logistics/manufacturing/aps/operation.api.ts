// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：operation.api.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  ApsOperation,
  ApsOperationCreate,
  ApsOperationObsolete,
  ApsOperationStatus,
  ApsOperationUpdate
} from '@/types/logistics/manufacturing/aps/operation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktApsOperations
 */
const APS_OPERATION_API_BASE = 'TaktApsOperations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取APS工序排程列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ApsOperation>>} 分页结果
 */
export function getApsOperationList(queryDto: any): Promise<TaktPagedResult<ApsOperation>> {
  return request<TaktPagedResult<ApsOperation>>({
    url: `${APS_OPERATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取APS工序排程
 * @param {string} id APS工序排程ID
 * @returns {Promise<ApsOperation>} APS工序排程DTO
 */
export function getApsOperationById(id: string): Promise<ApsOperation> {
  return request<ApsOperation>({
    url: `${APS_OPERATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建APS工序排程
 * @param {ApsOperationCreate} dto 创建DTO
 * @returns {Promise<ApsOperation>} APS工序排程DTO
 */
export function createApsOperation(dto: ApsOperationCreate): Promise<ApsOperation> {
  return request<ApsOperation>({
    url: `${APS_OPERATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新APS工序排程
 * @param {string} id APS工序排程ID
 * @param {ApsOperationUpdate} dto 更新DTO
 * @returns {Promise<ApsOperation>} APS工序排程DTO
 */
export function updateApsOperation(id: string, dto: ApsOperationUpdate): Promise<ApsOperation> {
  return request<ApsOperation>({
    url: `${APS_OPERATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除APS工序排程
 * @param {string} id APS工序排程ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsOperationById(id: string): Promise<void> {
  return request({
    url: `${APS_OPERATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除APS工序排程
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteApsOperationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${APS_OPERATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新APS工序排程状态
 * @param {ApsOperationStatus} dto 状态 DTO
 * @returns {Promise<ApsOperation>} APS工序排程DTO
 */
export function updateApsOperationStatus(dto: ApsOperationStatus): Promise<ApsOperation> {
  return request<ApsOperation>({
    url: `${APS_OPERATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新APS工序排程作废状态
 * @param {ApsOperationObsolete} dto 作废 DTO
 * @returns {Promise<ApsOperation>} APS工序排程DTO
 */
export function updateApsOperationObsolete(dto: ApsOperationObsolete): Promise<ApsOperation> {
  return request<ApsOperation>({
    url: `${APS_OPERATION_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取APS工序排程选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getApsOperationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${APS_OPERATION_API_BASE}/options`,
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
export function getApsOperationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${APS_OPERATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入APS工序排程
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importApsOperation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${APS_OPERATION_API_BASE}/import`,
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
 * 导出APS工序排程
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportApsOperation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${APS_OPERATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
