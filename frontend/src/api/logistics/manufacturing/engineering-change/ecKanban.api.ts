// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecKanban.api.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type { EcKanban, EcKanbanQuery } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types';

const TAKTECKANBANS_API_BASE = 'TaktEcKanbans';

/**
 * 获取设变看板列表（分页）
 */
export function getEcKanbanList(queryDto: EcKanbanQuery) {
  return request.get<TaktPagedResult<EcKanban>>(`/${TAKTECKANBANS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取设变看板详情
 */
export function getEcKanbanByEcId(ecGijutsuId: string) {
  return request.get<EcKanban>(`/${TAKTECKANBANS_API_BASE}/${ecGijutsuId}`);
}


/**
 * 导出设变看板
 */
export function exportEcKanbanData(queryDto?: EcKanbanQuery) {
  return request.get(`/${TAKTECKANBANS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
