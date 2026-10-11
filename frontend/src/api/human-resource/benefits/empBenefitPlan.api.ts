// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/benefits
// 文件名称：empBenefitPlan.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块 API（自动生成，请勿手改路由常量）
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
  EmpBenefitPlan,
  EmpBenefitPlanCreate,
  EmpBenefitPlanStatus,
  EmpBenefitPlanUpdate
} from '@/types/human-resource/benefits/empBenefitPlan.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmpBenefitPlans
 */
const EMP_BENEFIT_PLAN_API_BASE = 'TaktEmpBenefitPlans';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工福利方案列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmpBenefitPlan>>} 分页结果
 */
export function getEmpBenefitPlanList(queryDto: any): Promise<TaktPagedResult<EmpBenefitPlan>> {
  return request<TaktPagedResult<EmpBenefitPlan>>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工福利方案
 * @param {string} id 员工福利方案ID
 * @returns {Promise<EmpBenefitPlan>} 员工福利方案DTO
 */
export function getEmpBenefitPlanById(id: string): Promise<EmpBenefitPlan> {
  return request<EmpBenefitPlan>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工福利方案
 * @param {EmpBenefitPlanCreate} dto 创建DTO
 * @returns {Promise<EmpBenefitPlan>} 员工福利方案DTO
 */
export function createEmpBenefitPlan(dto: EmpBenefitPlanCreate): Promise<EmpBenefitPlan> {
  return request<EmpBenefitPlan>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工福利方案
 * @param {string} id 员工福利方案ID
 * @param {EmpBenefitPlanUpdate} dto 更新DTO
 * @returns {Promise<EmpBenefitPlan>} 员工福利方案DTO
 */
export function updateEmpBenefitPlan(id: string, dto: EmpBenefitPlanUpdate): Promise<EmpBenefitPlan> {
  return request<EmpBenefitPlan>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工福利方案
 * @param {string} id 员工福利方案ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmpBenefitPlanById(id: string): Promise<void> {
  return request({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工福利方案
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmpBenefitPlanBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新员工福利方案状态
 * @param {EmpBenefitPlanStatus} dto 状态 DTO
 * @returns {Promise<EmpBenefitPlan>} 员工福利方案DTO
 */
export function updateEmpBenefitPlanStatus(dto: EmpBenefitPlanStatus): Promise<EmpBenefitPlan> {
  return request<EmpBenefitPlan>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工福利方案选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmpBenefitPlanOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/options`,
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
export function getEmpBenefitPlanTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工福利方案
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmpBenefitPlan(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/import`,
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
 * 导出员工福利方案
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmpBenefitPlan(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMP_BENEFIT_PLAN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
