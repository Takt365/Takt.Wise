// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/training
// 文件名称：course.api.ts
// 创建时间：2026-06-23
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
  TrainingCourse,
  TrainingCourseCreate,
  TrainingCourseSort,
  TrainingCourseStatus,
  TrainingCourseUpdate
} from '@/types/human-resource/training/course.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTrainingCourses
 */
const TRAINING_COURSE_API_BASE = 'TaktTrainingCourses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取培训课程列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TrainingCourse>>} 分页结果
 */
export function getTrainingCourseList(queryDto: any): Promise<TaktPagedResult<TrainingCourse>> {
  return request<TaktPagedResult<TrainingCourse>>({
    url: `${TRAINING_COURSE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取培训课程
 * @param {string} id 培训课程ID
 * @returns {Promise<TrainingCourse>} 培训课程DTO
 */
export function getTrainingCourseById(id: string): Promise<TrainingCourse> {
  return request<TrainingCourse>({
    url: `${TRAINING_COURSE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建培训课程
 * @param {TrainingCourseCreate} dto 创建DTO
 * @returns {Promise<TrainingCourse>} 培训课程DTO
 */
export function createTrainingCourse(dto: TrainingCourseCreate): Promise<TrainingCourse> {
  return request<TrainingCourse>({
    url: `${TRAINING_COURSE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新培训课程
 * @param {string} id 培训课程ID
 * @param {TrainingCourseUpdate} dto 更新DTO
 * @returns {Promise<TrainingCourse>} 培训课程DTO
 */
export function updateTrainingCourse(id: string, dto: TrainingCourseUpdate): Promise<TrainingCourse> {
  return request<TrainingCourse>({
    url: `${TRAINING_COURSE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除培训课程
 * @param {string} id 培训课程ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingCourseById(id: string): Promise<void> {
  return request({
    url: `${TRAINING_COURSE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除培训课程
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrainingCourseBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TRAINING_COURSE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新培训课程状态
 * @param {TrainingCourseStatus} dto 状态 DTO
 * @returns {Promise<TrainingCourse>} 培训课程DTO
 */
export function updateTrainingCourseStatus(dto: TrainingCourseStatus): Promise<TrainingCourse> {
  return request<TrainingCourse>({
    url: `${TRAINING_COURSE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新培训课程排序
 * @param {TrainingCourseSort} dto 排序DTO
 * @returns {Promise<TrainingCourse>} 培训课程DTO
 */
export function updateTrainingCourseSort(dto: TrainingCourseSort): Promise<TrainingCourse> {
  return request<TrainingCourse>({
    url: `${TRAINING_COURSE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取培训课程选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTrainingCourseOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TRAINING_COURSE_API_BASE}/options`,
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
export function getTrainingCourseTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_COURSE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入培训课程
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTrainingCourse(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TRAINING_COURSE_API_BASE}/import`,
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
 * 导出培训课程
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTrainingCourse(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TRAINING_COURSE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
