// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mps
// 文件名称：masterProductionSchedule.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块 API（自动生成，请勿手改路由常量）
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
  MasterProductionSchedule,
  MasterProductionScheduleCreate,
  MasterProductionScheduleStatus,
  MasterProductionScheduleUpdate
} from '@/types/logistics/manufacturing/mps/masterProductionSchedule.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMasterProductionSchedules
 */
const MASTER_PRODUCTION_SCHEDULE_API_BASE = 'TaktMasterProductionSchedules';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取主生产计划MPS头列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MasterProductionSchedule>>} 分页结果
 */
export function getMasterProductionScheduleList(queryDto: any): Promise<TaktPagedResult<MasterProductionSchedule>> {
  return request<TaktPagedResult<MasterProductionSchedule>>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取主生产计划MPS头
 * @param {string} id 主生产计划MPS头ID
 * @returns {Promise<MasterProductionSchedule>} 主生产计划MPS头DTO
 */
export function getMasterProductionScheduleById(id: string): Promise<MasterProductionSchedule> {
  return request<MasterProductionSchedule>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建主生产计划MPS头
 * @param {MasterProductionScheduleCreate} dto 创建DTO
 * @returns {Promise<MasterProductionSchedule>} 主生产计划MPS头DTO
 */
export function createMasterProductionSchedule(dto: MasterProductionScheduleCreate): Promise<MasterProductionSchedule> {
  return request<MasterProductionSchedule>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新主生产计划MPS头
 * @param {string} id 主生产计划MPS头ID
 * @param {MasterProductionScheduleUpdate} dto 更新DTO
 * @returns {Promise<MasterProductionSchedule>} 主生产计划MPS头DTO
 */
export function updateMasterProductionSchedule(id: string, dto: MasterProductionScheduleUpdate): Promise<MasterProductionSchedule> {
  return request<MasterProductionSchedule>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除主生产计划MPS头
 * @param {string} id 主生产计划MPS头ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterProductionScheduleById(id: string): Promise<void> {
  return request({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除主生产计划MPS头
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMasterProductionScheduleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新主生产计划MPS头状态
 * @param {MasterProductionScheduleStatus} dto 状态 DTO
 * @returns {Promise<MasterProductionSchedule>} 主生产计划MPS头DTO
 */
export function updateMasterProductionScheduleStatus(dto: MasterProductionScheduleStatus): Promise<MasterProductionSchedule> {
  return request<MasterProductionSchedule>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取主生产计划MPS头选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMasterProductionScheduleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/options`,
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
export function getMasterProductionScheduleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入主生产计划MPS头
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMasterProductionSchedule(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/import`,
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
 * 导出主生产计划MPS头
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMasterProductionSchedule(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MASTER_PRODUCTION_SCHEDULE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
