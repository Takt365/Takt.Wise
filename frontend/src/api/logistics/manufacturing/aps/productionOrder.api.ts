// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：productionOrder.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  ProductionOrder,
  ProductionOrderCreate,
  ProductionOrderStatus,
  ProductionOrderUpdate
} from '@/types/logistics/manufacturing/aps/productionOrder.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionOrders
 */
const PRODUCTION_ORDER_API_BASE = 'TaktProductionOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionOrder>>} 分页结果
 */
export function getProductionOrderList(queryDto: any): Promise<TaktPagedResult<ProductionOrder>> {
  return request<TaktPagedResult<ProductionOrder>>({
    url: `${PRODUCTION_ORDER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产工单
 * @param {string} id 生产工单ID
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function getProductionOrderById(id: string): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产工单
 * @param {ProductionOrderCreate} dto 创建DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function createProductionOrder(dto: ProductionOrderCreate): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产工单
 * @param {string} id 生产工单ID
 * @param {ProductionOrderUpdate} dto 更新DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function updateProductionOrder(id: string, dto: ProductionOrderUpdate): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产工单
 * @param {string} id 生产工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产工单状态
 * @param {ProductionOrderStatus} dto 状态 DTO
 * @returns {Promise<ProductionOrder>} 生产工单DTO
 */
export function updateProductionOrderStatus(dto: ProductionOrderStatus): Promise<ProductionOrder> {
  return request<ProductionOrder>({
    url: `${PRODUCTION_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产工单选项列表
 * @param plantCode 工厂代码（可选，精确匹配）
 * @param keyword 搜索关键字（工单号/物料号，可选）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionOrderOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_ORDER_API_BASE}/options`,
    method: 'get',
    params: {
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

/**
 * 按工单号查询生产工单（用于日报回填）
 * @param {string} prodOrderCode 工单号
 * @param {string} [plantCode] 工厂代码（可选，用于精确匹配）
 * @returns {Promise<ProductionOrder>} 生产工单 DTO
 */
export async function getProductionOrderByCode(
  prodOrderCode: string,
  plantCode?: string
): Promise<ProductionOrder> {
  const code = prodOrderCode?.trim()
  if (!code) {
    throw new Error('prodOrderCode is required')
  }
  const plant = plantCode?.trim()
  const res = await getProductionOrderList({
    pageIndex: 1,
    pageSize: 20,
    prodOrderCode: code,
    ...(plant ? { plantCode: plant } : {}),
  })
  const matched = (res?.data ?? []).find((row) => {
    if (row.prodOrderCode !== code) {
      return false
    }
    if (plant && row.plantCode !== plant) {
      return false
    }
    return true
  })
  if (!matched) {
    throw new Error(`Production order not found: ${code}`)
  }
  return matched
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
export function getProductionOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产工单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionOrder(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_ORDER_API_BASE}/import`,
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
 * 导出生产工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
