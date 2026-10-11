// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：issueAssyRework.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块 API（自动生成，请勿手改路由常量）
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
  QualityIssueAssyRework,
  QualityIssueAssyReworkCreate,
  QualityIssueAssyReworkObsolete,
  QualityIssueAssyReworkUpdate
} from '@/types/logistics/quality/cost/issueAssyRework.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQualityIssueAssyReworks
 */
const QUALITY_ISSUE_ASSY_REWORK_API_BASE = 'TaktQualityIssueAssyReworks';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取质量问题组装不良改修费用明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QualityIssueAssyRework>>} 分页结果
 */
export function getQualityIssueAssyReworkList(queryDto: any): Promise<TaktPagedResult<QualityIssueAssyRework>> {
  return request<TaktPagedResult<QualityIssueAssyRework>>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取质量问题组装不良改修费用明细
 * @param {string} id 质量问题组装不良改修费用明细ID
 * @returns {Promise<QualityIssueAssyRework>} 质量问题组装不良改修费用明细DTO
 */
export function getQualityIssueAssyReworkById(id: string): Promise<QualityIssueAssyRework> {
  return request<QualityIssueAssyRework>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建质量问题组装不良改修费用明细
 * @param {QualityIssueAssyReworkCreate} dto 创建DTO
 * @returns {Promise<QualityIssueAssyRework>} 质量问题组装不良改修费用明细DTO
 */
export function createQualityIssueAssyRework(dto: QualityIssueAssyReworkCreate): Promise<QualityIssueAssyRework> {
  return request<QualityIssueAssyRework>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新质量问题组装不良改修费用明细
 * @param {string} id 质量问题组装不良改修费用明细ID
 * @param {QualityIssueAssyReworkUpdate} dto 更新DTO
 * @returns {Promise<QualityIssueAssyRework>} 质量问题组装不良改修费用明细DTO
 */
export function updateQualityIssueAssyRework(id: string, dto: QualityIssueAssyReworkUpdate): Promise<QualityIssueAssyRework> {
  return request<QualityIssueAssyRework>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除质量问题组装不良改修费用明细
 * @param {string} id 质量问题组装不良改修费用明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityIssueAssyReworkById(id: string): Promise<void> {
  return request({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除质量问题组装不良改修费用明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQualityIssueAssyReworkBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新质量问题组装不良改修费用明细作废状态
 * @param {QualityIssueAssyReworkObsolete} dto 作废 DTO
 * @returns {Promise<QualityIssueAssyRework>} 质量问题组装不良改修费用明细DTO
 */
export function updateQualityIssueAssyReworkObsolete(dto: QualityIssueAssyReworkObsolete): Promise<QualityIssueAssyRework> {
  return request<QualityIssueAssyRework>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取质量问题组装不良改修费用明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQualityIssueAssyReworkOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/options`,
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
export function getQualityIssueAssyReworkTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入质量问题组装不良改修费用明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importQualityIssueAssyRework(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/import`,
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
 * 导出质量问题组装不良改修费用明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQualityIssueAssyRework(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_ISSUE_ASSY_REWORK_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
