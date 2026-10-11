// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/training
// 文件名称：trainingAttendee.api.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training 模块 API（自动生成，请勿手改路由常量）
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
  TrainingAttendee,
  TrainingAttendeeCreate,
  TrainingAttendeeStatus,
  TrainingAttendeeUpdate
} from '@/types/human-resource/training/trainingAttendee.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTrainingAttendees
 */
const TRAINING_ATTENDEE_API_BASE = 'TaktTrainingAttendees';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取培训参训记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TrainingAttendee>>} 分页结果
 */
export function getTrainingAttendeeList(queryDto: any): Promise<TaktPagedResult<TrainingAttendee>> {
  return request<TaktPagedResult<TrainingAttendee>>({
    url: `${TRAINING_ATTENDEE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取培训参训记录
 * @param {string} id 培训参训记录ID
 * @returns {Promise<TrainingAttendee>} 培训参训记录DTO
 */
export function getTrainingAttendeeById(id: string): Promise<TrainingAttendee> {
  return request<TrainingAttendee>({
    url: `${TRAINING_ATTENDEE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建培训参训记录
 * @param {TrainingAttendeeCreate} dto 创建DTO
 * @returns {Promise<TrainingAttendee>} 培训参训记录DTO
 */
export function createTrainingAttendee(dto: TrainingAttendeeCreate): Promise<TrainingAttendee> {
  return request<TrainingAttendee>({
    url: `${TRAINING_ATTENDEE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新培训参训记录
 * @param {string} id 培训参训记录ID
 * @param {TrainingAttendeeUpdate} dto 更新DTO
 * @returns {Promise<TrainingAttendee>} 培训参训记录DTO
 */
export function updateTrainingAttendee(id: string, dto: TrainingAttendeeUpdate): Promise<TrainingAttendee> {
  return request<TrainingAttendee>({
    url: `${TRAINING_ATTENDEE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除培训参训记录
 * @param {string} id 培训参训记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingAttendeeById(id: string): Promise<void> {
  return request({
    url: `${TRAINING_ATTENDEE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除培训参训记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingAttendeeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TRAINING_ATTENDEE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新培训参训记录状态
 * @param {TrainingAttendeeStatus} dto 状态 DTO
 * @returns {Promise<TrainingAttendee>} 培训参训记录DTO
 */
export function updateTrainingAttendeeStatus(dto: TrainingAttendeeStatus): Promise<TrainingAttendee> {
  return request<TrainingAttendee>({
    url: `${TRAINING_ATTENDEE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取培训结果选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTrainingAttendeeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TRAINING_ATTENDEE_API_BASE}/options`,
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
export function getTrainingAttendeeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_ATTENDEE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入培训参训记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTrainingAttendee(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TRAINING_ATTENDEE_API_BASE}/import`,
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
 * 导出培训参训记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTrainingAttendee(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_ATTENDEE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
