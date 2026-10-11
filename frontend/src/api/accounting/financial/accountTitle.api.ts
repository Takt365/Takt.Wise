// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：accountTitle.api.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktTreeSelectOption
} from '@/types/common.types';
import type {
  AccountTitle,
  AccountTitleCreate,
  AccountTitleSort,
  AccountTitleStatus,
  AccountTitleTree,
  AccountTitleUpdate
} from '@/types/accounting/financial/accountTitle.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAccountTitles
 */
const ACCOUNT_TITLE_API_BASE = 'TaktAccountTitles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取会计科目列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AccountTitle>>} 分页结果
 */
export function getAccountTitleList(queryDto: any): Promise<TaktPagedResult<AccountTitle>> {
  return request<TaktPagedResult<AccountTitle>>({
    url: `${ACCOUNT_TITLE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取会计科目
 * @param {string} id 会计科目ID
 * @returns {Promise<AccountTitle>} 会计科目DTO
 */
export function getAccountTitleById(id: string): Promise<AccountTitle> {
  return request<AccountTitle>({
    url: `${ACCOUNT_TITLE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取会计科目树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<AccountTitleTree[]>} 树形数据
 */
export function getAccountTitleTree(parentId: string, includeDisabled: boolean): Promise<AccountTitleTree[]> {
  return request<AccountTitleTree[]>({
    url: `${ACCOUNT_TITLE_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建会计科目
 * @param {AccountTitleCreate} dto 创建DTO
 * @returns {Promise<AccountTitle>} 会计科目DTO
 */
export function createAccountTitle(dto: AccountTitleCreate): Promise<AccountTitle> {
  return request<AccountTitle>({
    url: `${ACCOUNT_TITLE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新会计科目
 * @param {string} id 会计科目ID
 * @param {AccountTitleUpdate} dto 更新DTO
 * @returns {Promise<AccountTitle>} 会计科目DTO
 */
export function updateAccountTitle(id: string, dto: AccountTitleUpdate): Promise<AccountTitle> {
  return request<AccountTitle>({
    url: `${ACCOUNT_TITLE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除会计科目
 * @param {string} id 会计科目ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAccountTitleById(id: string): Promise<void> {
  return request({
    url: `${ACCOUNT_TITLE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除会计科目
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAccountTitleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ACCOUNT_TITLE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新会计科目状态
 * @param {AccountTitleStatus} dto 状态 DTO
 * @returns {Promise<AccountTitle>} 会计科目DTO
 */
export function updateAccountTitleStatus(dto: AccountTitleStatus): Promise<AccountTitle> {
  return request<AccountTitle>({
    url: `${ACCOUNT_TITLE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新会计科目排序
 * @param {AccountTitleSort} dto 排序DTO
 * @returns {Promise<AccountTitle>} 会计科目DTO
 */
export function updateAccountTitleSort(dto: AccountTitleSort): Promise<AccountTitle> {
  return request<AccountTitle>({
    url: `${ACCOUNT_TITLE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取会计科目树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 AccountTitleCode）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getAccountTitleTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${ACCOUNT_TITLE_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId
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
export function getAccountTitleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ACCOUNT_TITLE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入会计科目
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAccountTitle(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ACCOUNT_TITLE_API_BASE}/import`,
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
 * 导出会计科目
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAccountTitle(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ACCOUNT_TITLE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
