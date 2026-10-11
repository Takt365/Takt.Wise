// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：sopWorkstation.api.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块 API（自动生成，请勿手改路由常量）
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
  SopWorkstation,
  SopWorkstationCreate,
  SopWorkstationSort,
  SopWorkstationStatus,
  SopWorkstationUpdate
} from '@/types/logistics/manufacturing/sop/sopWorkstation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopWorkstations
 */
const SOP_WORKSTATION_API_BASE = 'TaktSopWorkstations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP工位主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopWorkstation>>} 分页结果
 */
export function getSopWorkstationList(queryDto: any): Promise<TaktPagedResult<SopWorkstation>> {
  return request<TaktPagedResult<SopWorkstation>>({
    url: `${SOP_WORKSTATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP工位主数据
 * @param {string} id SOP工位主数据ID
 * @returns {Promise<SopWorkstation>} SOP工位主数据DTO
 */
export function getSopWorkstationById(id: string): Promise<SopWorkstation> {
  return request<SopWorkstation>({
    url: `${SOP_WORKSTATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP工位主数据
 * @param {SopWorkstationCreate} dto 创建DTO
 * @returns {Promise<SopWorkstation>} SOP工位主数据DTO
 */
export function createSopWorkstation(dto: SopWorkstationCreate): Promise<SopWorkstation> {
  return request<SopWorkstation>({
    url: `${SOP_WORKSTATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP工位主数据
 * @param {string} id SOP工位主数据ID
 * @param {SopWorkstationUpdate} dto 更新DTO
 * @returns {Promise<SopWorkstation>} SOP工位主数据DTO
 */
export function updateSopWorkstation(id: string, dto: SopWorkstationUpdate): Promise<SopWorkstation> {
  return request<SopWorkstation>({
    url: `${SOP_WORKSTATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP工位主数据
 * @param {string} id SOP工位主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopWorkstationById(id: string): Promise<void> {
  return request({
    url: `${SOP_WORKSTATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP工位主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopWorkstationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_WORKSTATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新SOP工位主数据状态
 * @param {SopWorkstationStatus} dto 状态 DTO
 * @returns {Promise<SopWorkstation>} SOP工位主数据DTO
 */
export function updateSopWorkstationStatus(dto: SopWorkstationStatus): Promise<SopWorkstation> {
  return request<SopWorkstation>({
    url: `${SOP_WORKSTATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新SOP工位主数据排序
 * @param {SopWorkstationSort} dto 排序DTO
 * @returns {Promise<SopWorkstation>} SOP工位主数据DTO
 */
export function updateSopWorkstationSort(dto: SopWorkstationSort): Promise<SopWorkstation> {
  return request<SopWorkstation>({
    url: `${SOP_WORKSTATION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP工位主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopWorkstationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_WORKSTATION_API_BASE}/options`,
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
export function getSopWorkstationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_WORKSTATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP工位主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopWorkstation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_WORKSTATION_API_BASE}/import`,
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
 * 导出SOP工位主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopWorkstation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_WORKSTATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
