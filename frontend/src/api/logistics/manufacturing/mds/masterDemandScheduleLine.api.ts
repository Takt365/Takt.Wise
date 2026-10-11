// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mds
// 文件名称：masterDemandScheduleLine.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块 API（自动生成，请勿手改路由常量）
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
  MasterDemandScheduleLine,
  MasterDemandScheduleLineCreate,
  MasterDemandScheduleLineUpdate
} from '@/types/logistics/manufacturing/mds/masterDemandScheduleLine.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMasterDemandScheduleLines
 */
const MASTER_DEMAND_SCHEDULE_LINE_API_BASE = 'TaktMasterDemandScheduleLines';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取主需求计划MDS行列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MasterDemandScheduleLine>>} 分页结果
 */
export function getMasterDemandScheduleLineList(queryDto: any): Promise<TaktPagedResult<MasterDemandScheduleLine>> {
  return request<TaktPagedResult<MasterDemandScheduleLine>>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取主需求计划MDS行
 * @param {string} id 主需求计划MDS行ID
 * @returns {Promise<MasterDemandScheduleLine>} 主需求计划MDS行DTO
 */
export function getMasterDemandScheduleLineById(id: string): Promise<MasterDemandScheduleLine> {
  return request<MasterDemandScheduleLine>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建主需求计划MDS行
 * @param {MasterDemandScheduleLineCreate} dto 创建DTO
 * @returns {Promise<MasterDemandScheduleLine>} 主需求计划MDS行DTO
 */
export function createMasterDemandScheduleLine(dto: MasterDemandScheduleLineCreate): Promise<MasterDemandScheduleLine> {
  return request<MasterDemandScheduleLine>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新主需求计划MDS行
 * @param {string} id 主需求计划MDS行ID
 * @param {MasterDemandScheduleLineUpdate} dto 更新DTO
 * @returns {Promise<MasterDemandScheduleLine>} 主需求计划MDS行DTO
 */
export function updateMasterDemandScheduleLine(id: string, dto: MasterDemandScheduleLineUpdate): Promise<MasterDemandScheduleLine> {
  return request<MasterDemandScheduleLine>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除主需求计划MDS行
 * @param {string} id 主需求计划MDS行ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterDemandScheduleLineById(id: string): Promise<void> {
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除主需求计划MDS行
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterDemandScheduleLineBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取主需求计划MDS行选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMasterDemandScheduleLineOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/options`,
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
export function getMasterDemandScheduleLineTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入主需求计划MDS行
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMasterDemandScheduleLine(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/import`,
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
 * 导出主需求计划MDS行
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMasterDemandScheduleLine(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_DEMAND_SCHEDULE_LINE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
