// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/sop
// 文件名称：sopExecScan.api.ts
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
  SopExecScan,
  SopExecScanCreate,
  SopExecScanUpdate
} from '@/types/logistics/manufacturing/sop/sopExecScan.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSopExecScans
 */
const SOP_EXEC_SCAN_API_BASE = 'TaktSopExecScans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取SOP物料扫码记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SopExecScan>>} 分页结果
 */
export function getSopExecScanList(queryDto: any): Promise<TaktPagedResult<SopExecScan>> {
  return request<TaktPagedResult<SopExecScan>>({
    url: `${SOP_EXEC_SCAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取SOP物料扫码记录
 * @param {string} id SOP物料扫码记录ID
 * @returns {Promise<SopExecScan>} SOP物料扫码记录DTO
 */
export function getSopExecScanById(id: string): Promise<SopExecScan> {
  return request<SopExecScan>({
    url: `${SOP_EXEC_SCAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建SOP物料扫码记录
 * @param {SopExecScanCreate} dto 创建DTO
 * @returns {Promise<SopExecScan>} SOP物料扫码记录DTO
 */
export function createSopExecScan(dto: SopExecScanCreate): Promise<SopExecScan> {
  return request<SopExecScan>({
    url: `${SOP_EXEC_SCAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新SOP物料扫码记录
 * @param {string} id SOP物料扫码记录ID
 * @param {SopExecScanUpdate} dto 更新DTO
 * @returns {Promise<SopExecScan>} SOP物料扫码记录DTO
 */
export function updateSopExecScan(id: string, dto: SopExecScanUpdate): Promise<SopExecScan> {
  return request<SopExecScan>({
    url: `${SOP_EXEC_SCAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除SOP物料扫码记录
 * @param {string} id SOP物料扫码记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopExecScanById(id: string): Promise<void> {
  return request({
    url: `${SOP_EXEC_SCAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除SOP物料扫码记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSopExecScanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOP_EXEC_SCAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取SOP物料扫码记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSopExecScanOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOP_EXEC_SCAN_API_BASE}/options`,
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
export function getSopExecScanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_EXEC_SCAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入SOP物料扫码记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSopExecScan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOP_EXEC_SCAN_API_BASE}/import`,
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
 * 导出SOP物料扫码记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSopExecScan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOP_EXEC_SCAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
