// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：numbering.api.ts
// 创建时间：2026-06-24
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
  Numbering,
  NumberingCreate,
  NumberingStatus,
  NumberingUpdate
} from '@/types/foundation/numbering.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktNumberings
 */
const NUMBERING_API_BASE = 'TaktNumberings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取编码规则列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Numbering>>} 分页结果
 */
export function getNumberingList(queryDto: any): Promise<TaktPagedResult<Numbering>> {
  return request<TaktPagedResult<Numbering>>({
    url: `${NUMBERING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取编码规则
 * @param {string} id 编码规则ID
 * @returns {Promise<Numbering>} 编码规则DTO
 */
export function getNumberingById(id: string): Promise<Numbering> {
  return request<Numbering>({
    url: `${NUMBERING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建编码规则
 * @param {NumberingCreate} dto 创建DTO
 * @returns {Promise<Numbering>} 编码规则DTO
 */
export function createNumbering(dto: NumberingCreate): Promise<Numbering> {
  return request<Numbering>({
    url: `${NUMBERING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新编码规则
 * @param {string} id 编码规则ID
 * @param {NumberingUpdate} dto 更新DTO
 * @returns {Promise<Numbering>} 编码规则DTO
 */
export function updateNumbering(id: string, dto: NumberingUpdate): Promise<Numbering> {
  return request<Numbering>({
    url: `${NUMBERING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除编码规则
 * @param {string} id 编码规则ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteNumberingById(id: string): Promise<void> {
  return request({
    url: `${NUMBERING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除编码规则
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteNumberingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${NUMBERING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新编码规则状态
 * @param {NumberingStatus} dto 状态 DTO
 * @returns {Promise<Numbering>} 编码规则DTO
 */
export function updateNumberingStatus(dto: NumberingStatus): Promise<Numbering> {
  return request<Numbering>({
    url: `${NUMBERING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 预览取号
// ========================================

/** 预览下一个业务编码（不占用流水号） */
export interface NumberingPreview {
  /** 规则编码 */
  ruleCode: string;
  /** 预览业务编码 */
  businessCode: string;
  /** 预览流水号 */
  currentSequence: number;
}

/**
 * 预览下一个业务编码
 * @param {string} ruleCode 规则编码（TaktNumbering.RuleCode）
 * @returns {Promise<NumberingPreview>} 预览结果
 */
export function previewNumberingNext(ruleCode: string): Promise<NumberingPreview> {
  return request<NumberingPreview>({
    url: `${NUMBERING_API_BASE}/preview-next`,
    method: 'get',
    params: { ruleCode },
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取编码规则选项列表
 * @param documentType 单据类型（TaktMenu.MenuName）；有值时仅返回该类型规则
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getNumberingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${NUMBERING_API_BASE}/options`,
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
export function getNumberingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${NUMBERING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入编码规则
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importNumbering(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${NUMBERING_API_BASE}/import`,
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
 * 导出编码规则
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportNumbering(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${NUMBERING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
