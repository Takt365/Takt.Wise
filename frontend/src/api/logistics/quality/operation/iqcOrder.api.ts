// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：iqcOrder.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  IqcOrder,
  IqcOrderCreate,
  IqcOrderStatus,
  IqcOrderUpdate
} from '@/types/logistics/quality/operation/iqcOrder.types';
import type {
  IqcOrderStat
} from '@/types/logistics/quality/operation/iqcOrderStat.types';
import type {
  QualityStatQuery
} from '@/types/logistics/quality/operation/qualityStatQuery.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktIqcOrders
 */
const IQC_ORDER_API_BASE = 'TaktIqcOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取进货检验单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<IqcOrder>>} 分页结果
 */
export function getIqcOrderList(queryDto: any): Promise<TaktPagedResult<IqcOrder>> {
  return request<TaktPagedResult<IqcOrder>>({
    url: `${IQC_ORDER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取 IQC 检验统计（数据看板）
 * @param {QualityStatQuery} queryDto 查询 DTO
 * @returns {Promise<IqcOrderStat>} IQC 检验统计
 */
export function getIqcOrderStat(queryDto: QualityStatQuery): Promise<IqcOrderStat> {
  return request<IqcOrderStat>({
    url: `${IQC_ORDER_API_BASE}/inspection-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取进货检验单
 * @param {string} id 进货检验单ID
 * @returns {Promise<IqcOrder>} 进货检验单DTO
 */
export function getIqcOrderById(id: string): Promise<IqcOrder> {
  return request<IqcOrder>({
    url: `${IQC_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建进货检验单
 * @param {IqcOrderCreate} dto 创建DTO
 * @returns {Promise<IqcOrder>} 进货检验单DTO
 */
export function createIqcOrder(dto: IqcOrderCreate): Promise<IqcOrder> {
  return request<IqcOrder>({
    url: `${IQC_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新进货检验单
 * @param {string} id 进货检验单ID
 * @param {IqcOrderUpdate} dto 更新DTO
 * @returns {Promise<IqcOrder>} 进货检验单DTO
 */
export function updateIqcOrder(id: string, dto: IqcOrderUpdate): Promise<IqcOrder> {
  return request<IqcOrder>({
    url: `${IQC_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除进货检验单
 * @param {string} id 进货检验单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteIqcOrderById(id: string): Promise<void> {
  return request({
    url: `${IQC_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除进货检验单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteIqcOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IQC_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新进货检验单状态
 * @param {IqcOrderStatus} dto 状态 DTO
 * @returns {Promise<IqcOrder>} 进货检验单DTO
 */
export function updateIqcOrderStatus(dto: IqcOrderStatus): Promise<IqcOrder> {
  return request<IqcOrder>({
    url: `${IQC_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取进货检验单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getIqcOrderOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${IQC_ORDER_API_BASE}/options`,
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
export function getIqcOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${IQC_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入进货检验单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importIqcOrder(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${IQC_ORDER_API_BASE}/import`,
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
 * 导出进货检验单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportIqcOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IQC_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
