// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：pcbaOutputDetail.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  PcbaOutputDetail,
  PcbaOutputDetailCreate,
  PcbaOutputDetailObsolete,
  PcbaOutputDetailStatus,
  PcbaOutputDetailUpdate
} from '@/types/logistics/manufacturing/output/pcbaOutputDetail.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPcbaOutputDetails
 */
const PCBA_OUTPUT_DETAIL_API_BASE = 'TaktPcbaOutputDetails';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取PCBA日报明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PcbaOutputDetail>>} 分页结果
 */
export function getPcbaOutputDetailList(queryDto: any): Promise<TaktPagedResult<PcbaOutputDetail>> {
  return request<TaktPagedResult<PcbaOutputDetail>>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取PCBA日报明细
 * @param {string} id PCBA日报明细ID
 * @returns {Promise<PcbaOutputDetail>} PCBA日报明细DTO
 */
export function getPcbaOutputDetailById(id: string): Promise<PcbaOutputDetail> {
  return request<PcbaOutputDetail>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建PCBA日报明细
 * @param {PcbaOutputDetailCreate} dto 创建DTO
 * @returns {Promise<PcbaOutputDetail>} PCBA日报明细DTO
 */
export function createPcbaOutputDetail(dto: PcbaOutputDetailCreate): Promise<PcbaOutputDetail> {
  return request<PcbaOutputDetail>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新PCBA日报明细
 * @param {string} id PCBA日报明细ID
 * @param {PcbaOutputDetailUpdate} dto 更新DTO
 * @returns {Promise<PcbaOutputDetail>} PCBA日报明细DTO
 */
export function updatePcbaOutputDetail(id: string, dto: PcbaOutputDetailUpdate): Promise<PcbaOutputDetail> {
  return request<PcbaOutputDetail>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除PCBA日报明细
 * @param {string} id PCBA日报明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaOutputDetailById(id: string): Promise<void> {
  return request({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除PCBA日报明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePcbaOutputDetailBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新PCBA日报明细状态
 * @param {PcbaOutputDetailStatus} dto 状态 DTO
 * @returns {Promise<PcbaOutputDetail>} PCBA日报明细DTO
 */
export function updatePcbaOutputDetailStatus(dto: PcbaOutputDetailStatus): Promise<PcbaOutputDetail> {
  return request<PcbaOutputDetail>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新PCBA日报明细作废状态
 * @param {PcbaOutputDetailObsolete} dto 作废 DTO
 * @returns {Promise<PcbaOutputDetail>} PCBA日报明细DTO
 */
export function updatePcbaOutputDetailObsolete(dto: PcbaOutputDetailObsolete): Promise<PcbaOutputDetail> {
  return request<PcbaOutputDetail>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取PCBA日报明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPcbaOutputDetailOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/options`,
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
export function getPcbaOutputDetailTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入PCBA日报明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPcbaOutputDetail(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/import`,
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
 * 导出PCBA日报明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPcbaOutputDetail(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PCBA_OUTPUT_DETAIL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
