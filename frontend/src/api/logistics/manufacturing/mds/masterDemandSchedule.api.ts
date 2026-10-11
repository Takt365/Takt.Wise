// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mds
// 文件名称：masterDemandSchedule.api.ts
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
  MasterDemandSchedule,
  MasterDemandScheduleCreate,
  MasterDemandScheduleStatus,
  MasterDemandScheduleUpdate
} from '@/types/logistics/manufacturing/mds/masterDemandSchedule.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMasterDemandSchedules
 */
const MASTER_DEMAND_SCHEDULE_API_BASE = 'TaktMasterDemandSchedules';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取主需求计划MDS头列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MasterDemandSchedule>>} 分页结果
 */
export function getMasterDemandScheduleList(queryDto: any): Promise<TaktPagedResult<MasterDemandSchedule>> {
  return request<TaktPagedResult<MasterDemandSchedule>>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取主需求计划MDS头
 * @param {string} id 主需求计划MDS头ID
 * @returns {Promise<MasterDemandSchedule>} 主需求计划MDS头DTO
 */
export function getMasterDemandScheduleById(id: string): Promise<MasterDemandSchedule> {
  return request<MasterDemandSchedule>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建主需求计划MDS头
 * @param {MasterDemandScheduleCreate} dto 创建DTO
 * @returns {Promise<MasterDemandSchedule>} 主需求计划MDS头DTO
 */
export function createMasterDemandSchedule(dto: MasterDemandScheduleCreate): Promise<MasterDemandSchedule> {
  return request<MasterDemandSchedule>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新主需求计划MDS头
 * @param {string} id 主需求计划MDS头ID
 * @param {MasterDemandScheduleUpdate} dto 更新DTO
 * @returns {Promise<MasterDemandSchedule>} 主需求计划MDS头DTO
 */
export function updateMasterDemandSchedule(id: string, dto: MasterDemandScheduleUpdate): Promise<MasterDemandSchedule> {
  return request<MasterDemandSchedule>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除主需求计划MDS头
 * @param {string} id 主需求计划MDS头ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterDemandScheduleById(id: string): Promise<void> {
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除主需求计划MDS头
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterDemandScheduleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新主需求计划MDS头状态
 * @param {MasterDemandScheduleStatus} dto 状态 DTO
 * @returns {Promise<MasterDemandSchedule>} 主需求计划MDS头DTO
 */
export function updateMasterDemandScheduleStatus(dto: MasterDemandScheduleStatus): Promise<MasterDemandSchedule> {
  return request<MasterDemandSchedule>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取主需求计划MDS头选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMasterDemandScheduleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/options`,
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
export function getMasterDemandScheduleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入主需求计划MDS头
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMasterDemandSchedule(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/import`,
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
 * 导出主需求计划MDS头
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMasterDemandSchedule(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_DEMAND_SCHEDULE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
