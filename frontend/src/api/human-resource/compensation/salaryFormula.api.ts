// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：salaryFormula.api.ts
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
  SalaryFormula,
  SalaryFormulaCreate,
  SalaryFormulaSort,
  SalaryFormulaStatus,
  SalaryFormulaUpdate
} from '@/types/human-resource/compensation/salaryFormula.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalaryFormulas
 */
const SALARY_FORMULA_API_BASE = 'TaktSalaryFormulas';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取薪资计算公式列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalaryFormula>>} 分页结果
 */
export function getSalaryFormulaList(queryDto: any): Promise<TaktPagedResult<SalaryFormula>> {
  return request<TaktPagedResult<SalaryFormula>>({
    url: `${SALARY_FORMULA_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取薪资计算公式
 * @param {string} id 薪资计算公式ID
 * @returns {Promise<SalaryFormula>} 薪资计算公式DTO
 */
export function getSalaryFormulaById(id: string): Promise<SalaryFormula> {
  return request<SalaryFormula>({
    url: `${SALARY_FORMULA_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建薪资计算公式
 * @param {SalaryFormulaCreate} dto 创建DTO
 * @returns {Promise<SalaryFormula>} 薪资计算公式DTO
 */
export function createSalaryFormula(dto: SalaryFormulaCreate): Promise<SalaryFormula> {
  return request<SalaryFormula>({
    url: `${SALARY_FORMULA_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新薪资计算公式
 * @param {string} id 薪资计算公式ID
 * @param {SalaryFormulaUpdate} dto 更新DTO
 * @returns {Promise<SalaryFormula>} 薪资计算公式DTO
 */
export function updateSalaryFormula(id: string, dto: SalaryFormulaUpdate): Promise<SalaryFormula> {
  return request<SalaryFormula>({
    url: `${SALARY_FORMULA_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除薪资计算公式
 * @param {string} id 薪资计算公式ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryFormulaById(id: string): Promise<void> {
  return request({
    url: `${SALARY_FORMULA_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除薪资计算公式
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryFormulaBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALARY_FORMULA_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新薪资计算公式状态
 * @param {SalaryFormulaStatus} dto 状态 DTO
 * @returns {Promise<SalaryFormula>} 薪资计算公式DTO
 */
export function updateSalaryFormulaStatus(dto: SalaryFormulaStatus): Promise<SalaryFormula> {
  return request<SalaryFormula>({
    url: `${SALARY_FORMULA_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新薪资计算公式排序
 * @param {SalaryFormulaSort} dto 排序DTO
 * @returns {Promise<SalaryFormula>} 薪资计算公式DTO
 */
export function updateSalaryFormulaSort(dto: SalaryFormulaSort): Promise<SalaryFormula> {
  return request<SalaryFormula>({
    url: `${SALARY_FORMULA_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取薪资计算公式选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalaryFormulaOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SALARY_FORMULA_API_BASE}/options`,
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
export function getSalaryFormulaTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_FORMULA_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入薪资计算公式
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalaryFormula(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALARY_FORMULA_API_BASE}/import`,
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
 * 导出薪资计算公式
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalaryFormula(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_FORMULA_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
