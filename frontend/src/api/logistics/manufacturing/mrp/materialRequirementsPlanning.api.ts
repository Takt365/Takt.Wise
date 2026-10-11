// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mrp
// 文件名称：materialRequirementsPlanning.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块 API（自动生成，请勿手改路由常量）
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
  MaterialRequirementsPlanning,
  MaterialRequirementsPlanningCreate,
  MaterialRequirementsPlanningStatus,
  MaterialRequirementsPlanningUpdate
} from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanning.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaterialRequirementsPlannings
 */
const MATERIAL_REQUIREMENTS_PLANNING_API_BASE = 'TaktMaterialRequirementsPlannings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取物料需求计划MRP头列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaterialRequirementsPlanning>>} 分页结果
 */
export function getMaterialRequirementsPlanningList(queryDto: any): Promise<TaktPagedResult<MaterialRequirementsPlanning>> {
  return request<TaktPagedResult<MaterialRequirementsPlanning>>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取物料需求计划MRP头
 * @param {string} id 物料需求计划MRP头ID
 * @returns {Promise<MaterialRequirementsPlanning>} 物料需求计划MRP头DTO
 */
export function getMaterialRequirementsPlanningById(id: string): Promise<MaterialRequirementsPlanning> {
  return request<MaterialRequirementsPlanning>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建物料需求计划MRP头
 * @param {MaterialRequirementsPlanningCreate} dto 创建DTO
 * @returns {Promise<MaterialRequirementsPlanning>} 物料需求计划MRP头DTO
 */
export function createMaterialRequirementsPlanning(dto: MaterialRequirementsPlanningCreate): Promise<MaterialRequirementsPlanning> {
  return request<MaterialRequirementsPlanning>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新物料需求计划MRP头
 * @param {string} id 物料需求计划MRP头ID
 * @param {MaterialRequirementsPlanningUpdate} dto 更新DTO
 * @returns {Promise<MaterialRequirementsPlanning>} 物料需求计划MRP头DTO
 */
export function updateMaterialRequirementsPlanning(id: string, dto: MaterialRequirementsPlanningUpdate): Promise<MaterialRequirementsPlanning> {
  return request<MaterialRequirementsPlanning>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除物料需求计划MRP头
 * @param {string} id 物料需求计划MRP头ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialRequirementsPlanningById(id: string): Promise<void> {
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除物料需求计划MRP头
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaterialRequirementsPlanningBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新物料需求计划MRP头状态
 * @param {MaterialRequirementsPlanningStatus} dto 状态 DTO
 * @returns {Promise<MaterialRequirementsPlanning>} 物料需求计划MRP头DTO
 */
export function updateMaterialRequirementsPlanningStatus(dto: MaterialRequirementsPlanningStatus): Promise<MaterialRequirementsPlanning> {
  return request<MaterialRequirementsPlanning>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取物料需求计划MRP头选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaterialRequirementsPlanningOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/options`,
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
export function getMaterialRequirementsPlanningTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入物料需求计划MRP头
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaterialRequirementsPlanning(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/import`,
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
 * 导出物料需求计划MRP头
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaterialRequirementsPlanning(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_REQUIREMENTS_PLANNING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
