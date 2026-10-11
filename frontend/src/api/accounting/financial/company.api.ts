// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：company.api.ts
// 创建时间：2026-08-15
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  Company,
  CompanyCreate,
  CompanySort,
  CompanyStatus,
  CompanyUpdate
} from '@/types/accounting/financial/company.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCompanies
 */
const COMPANY_API_BASE = 'TaktCompanies';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取公司列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Company>>} 分页结果
 */
export function getCompanyList(queryDto: any): Promise<TaktPagedResult<Company>> {
  return request<TaktPagedResult<Company>>({
    url: `${COMPANY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取公司
 * @param {string} id 公司ID
 * @returns {Promise<Company>} 公司DTO
 */
export function getCompanyById(id: string): Promise<Company> {
  return request<Company>({
    url: `${COMPANY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建公司
 * @param {CompanyCreate} dto 创建DTO
 * @returns {Promise<Company>} 公司DTO
 */
export function createCompany(dto: CompanyCreate): Promise<Company> {
  return request<Company>({
    url: `${COMPANY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新公司
 * @param {string} id 公司ID
 * @param {CompanyUpdate} dto 更新DTO
 * @returns {Promise<Company>} 公司DTO
 */
export function updateCompany(id: string, dto: CompanyUpdate): Promise<Company> {
  return request<Company>({
    url: `${COMPANY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除公司
 * @param {string} id 公司ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCompanyById(id: string): Promise<void> {
  return request({
    url: `${COMPANY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除公司
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCompanyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COMPANY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新公司状态
 * @param {CompanyStatus} dto 状态 DTO
 * @returns {Promise<Company>} 公司DTO
 */
export function updateCompanyStatus(dto: CompanyStatus): Promise<Company> {
  return request<Company>({
    url: `${COMPANY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新公司排序
 * @param {CompanySort} dto 排序DTO
 * @returns {Promise<Company>} 公司DTO
 */
export function updateCompanySort(dto: CompanySort): Promise<Company> {
  return request<Company>({
    url: `${COMPANY_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取公司选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCompanyOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${COMPANY_API_BASE}/options`,
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
export function getCompanyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COMPANY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入公司
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCompany(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COMPANY_API_BASE}/import`,
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
 * 导出公司
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCompany(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COMPANY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
