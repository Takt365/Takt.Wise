// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：menu.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
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
  Menu,
  MenuCreate,
  MenuSort,
  MenuStatus,
  MenuTree,
  MenuUpdate
} from '@/types/identity/menu.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMenus
 */
const MENU_API_BASE = 'TaktMenus';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取菜单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Menu>>} 分页结果
 */
export function getMenuList(queryDto: any): Promise<TaktPagedResult<Menu>> {
  return request<TaktPagedResult<Menu>>({
    url: `${MENU_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取菜单
 * @param {string} id 菜单ID
 * @returns {Promise<Menu>} 菜单DTO
 */
export function getMenuById(id: string): Promise<Menu> {
  return request<Menu>({
    url: `${MENU_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取菜单树形列表
 * @param {string} parentId parentId
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 枚举字段，如 1）
 * @returns {Promise<MenuTree[]>} 树形数据
 */
export function getMenuTree(parentId: string, includeDisabled: boolean): Promise<MenuTree[]> {
  return request<MenuTree[]>({
    url: `${MENU_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建菜单
 * @param {MenuCreate} dto 创建DTO
 * @returns {Promise<Menu>} 菜单DTO
 */
export function createMenu(dto: MenuCreate): Promise<Menu> {
  return request<Menu>({
    url: `${MENU_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新菜单
 * @param {string} id 菜单ID
 * @param {MenuUpdate} dto 更新DTO
 * @returns {Promise<Menu>} 菜单DTO
 */
export function updateMenu(id: string, dto: MenuUpdate): Promise<Menu> {
  return request<Menu>({
    url: `${MENU_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除菜单
 * @param {string} id 菜单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMenuById(id: string): Promise<void> {
  return request({
    url: `${MENU_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除菜单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMenuBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MENU_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新菜单状态
 * @param {MenuStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Menu>} 菜单DTO
 */
export function updateMenuStatus(dto: MenuStatus): Promise<Menu> {
  return request<Menu>({
    url: `${MENU_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新菜单排序
 * @param {MenuSort} dto 排序DTO
 * @returns {Promise<Menu>} 菜单DTO
 */
export function updateMenuSort(dto: MenuSort): Promise<Menu> {
  return request<Menu>({
    url: `${MENU_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取菜单树形选项列表
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
/**
 * 获取菜单树形选项（懒加载：传 parentId 仅一层；不传默认根）
 * @param parentId 父级菜单 Id（string；默认 '0'）
 * @param keyword 可选关键字（当前层 MenuName）
 * @param valueBy 可选：name=按菜单名称作 DictValue（嵌套整树，单据类型用）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getMenuTreeOptions(
  parentId: string = '0',
  keyword?: string,
  valueBy?: string,
): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${MENU_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId,
      ...(keyword ? { keyword } : {}),
      ...(valueBy ? { valueBy } : {}),
    },
  });
}

/**
 * 角色菜单分配：互斥懒加载树（一层）
 * @param parentId 父级 Id（'0'=根；根含父在对侧的提升节点）
 * @param assignedSide true=已分配侧；false=未分配侧
 * @param assignedMenuIds 当前已分配菜单 Id
 * @returns {Promise<MenuTree[]>} 一层节点
 */
export function getMenuAssignTree(
  parentId: string,
  assignedSide: boolean,
  assignedMenuIds: string[],
): Promise<MenuTree[]> {
  return request<MenuTree[]>({
    url: `${MENU_API_BASE}/assign-tree`,
    method: 'post',
    data: {
      parentId,
      assignedSide,
      assignedMenuIds,
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
export function getMenuTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MENU_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入菜单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMenu(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MENU_API_BASE}/import`,
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
 * 导出菜单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMenu(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MENU_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
