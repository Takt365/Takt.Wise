// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：translation.api.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
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
  Translation,
  TranslationCreate,
  TranslationMessages,
  TranslationTransposedResult,
  TranslationUpdate
} from '@/types/foundation/translation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTranslations
 */
const TRANSLATION_API_BASE = 'TaktTranslations';

// ========================================
// 前端动态 messages（vue-i18n）
// ========================================

/**
 * 获取指定文化下的前端动态翻译键值（GET /messages，允许匿名）
 * @param {string} cultureCode 区域文化编码（BCP47）
 * @returns {Promise<TranslationMessages>} 扁平 I18nKey → 文本
 */
export function getTranslationMessages(cultureCode: string): Promise<TranslationMessages> {
  return request<TranslationMessages>({
    url: `${TRANSLATION_API_BASE}/messages`,
    method: 'get',
    params: { cultureCode },
  });
}

// ========================================
// 转置（多语言表格）
// ========================================

/**
 * 获取翻译转置列表（分页）
 * @param {any} queryDto queryDto
 */
export function getTranslationTransposedList(queryDto: any): Promise<TranslationTransposedResult> {
  return request<TranslationTransposedResult>({
    url: `${TRANSLATION_API_BASE}/transposed`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 批量保存翻译转置数据
 * @param {any} dto dto
 */
export function saveTranslationTransposedBatch(dto: any): Promise<number> {
  return request<number>({
    url: `${TRANSLATION_API_BASE}/transposed/batch`,
    method: 'post',
    data: dto,
  });
}

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取翻译列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Translation>>} 分页结果
 */
export function getTranslationList(queryDto: any): Promise<TaktPagedResult<Translation>> {
  return request<TaktPagedResult<Translation>>({
    url: `${TRANSLATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取翻译
 * @param {string} id 翻译ID
 * @returns {Promise<Translation>} 翻译DTO
 */
export function getTranslationById(id: string): Promise<Translation> {
  return request<Translation>({
    url: `${TRANSLATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建翻译
 * @param {TranslationCreate} dto 创建DTO
 * @returns {Promise<Translation>} 翻译DTO
 */
export function createTranslation(dto: TranslationCreate): Promise<Translation> {
  return request<Translation>({
    url: `${TRANSLATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新翻译
 * @param {string} id 翻译ID
 * @param {TranslationUpdate} dto 更新DTO
 * @returns {Promise<Translation>} 翻译DTO
 */
export function updateTranslation(id: string, dto: TranslationUpdate): Promise<Translation> {
  return request<Translation>({
    url: `${TRANSLATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除翻译
 * @param {string} id 翻译ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTranslationById(id: string): Promise<void> {
  return request({
    url: `${TRANSLATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除翻译
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTranslationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TRANSLATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取翻译选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTranslationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TRANSLATION_API_BASE}/options`,
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
export function getTranslationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TRANSLATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入翻译
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTranslation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TRANSLATION_API_BASE}/import`,
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
 * 导出翻译
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTranslation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TRANSLATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
