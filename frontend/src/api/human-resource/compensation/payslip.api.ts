// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：payslip.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块 API（自动生成，请勿手改路由常量）
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
  Payslip,
  PayslipCreate,
  PayslipStatus,
  PayslipUpdate
} from '@/types/human-resource/compensation/payslip.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPayslips
 */
const PAYSLIP_API_BASE = 'TaktPayslips';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工资条列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Payslip>>} 分页结果
 */
export function getPayslipList(queryDto: any): Promise<TaktPagedResult<Payslip>> {
  return request<TaktPagedResult<Payslip>>({
    url: `${PAYSLIP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工资条
 * @param {string} id 工资条ID
 * @returns {Promise<Payslip>} 工资条DTO
 */
export function getPayslipById(id: string): Promise<Payslip> {
  return request<Payslip>({
    url: `${PAYSLIP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工资条
 * @param {PayslipCreate} dto 创建DTO
 * @returns {Promise<Payslip>} 工资条DTO
 */
export function createPayslip(dto: PayslipCreate): Promise<Payslip> {
  return request<Payslip>({
    url: `${PAYSLIP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工资条
 * @param {string} id 工资条ID
 * @param {PayslipUpdate} dto 更新DTO
 * @returns {Promise<Payslip>} 工资条DTO
 */
export function updatePayslip(id: string, dto: PayslipUpdate): Promise<Payslip> {
  return request<Payslip>({
    url: `${PAYSLIP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工资条
 * @param {string} id 工资条ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePayslipById(id: string): Promise<void> {
  return request({
    url: `${PAYSLIP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工资条
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePayslipBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PAYSLIP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工资条状态
 * @param {PayslipStatus} dto 状态 DTO
 * @returns {Promise<Payslip>} 工资条DTO
 */
export function updatePayslipStatus(dto: PayslipStatus): Promise<Payslip> {
  return request<Payslip>({
    url: `${PAYSLIP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工资条选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPayslipOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PAYSLIP_API_BASE}/options`,
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
export function getPayslipTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PAYSLIP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工资条
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPayslip(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PAYSLIP_API_BASE}/import`,
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
 * 导出工资条
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPayslip(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PAYSLIP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
