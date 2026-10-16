// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecKakunin.api.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变物料确认 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types';
import type { EcKakuninQuery, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/ecKakunin.types';

const TAKTECKAKUNINS_API_BASE = 'TaktEcKakunins';

/**
 * 获取物料确认列表（分页）
 */
export function getEcKakuninList(queryDto: EcKakuninQuery) {
  return request.get<TaktPagedResult<EcDetail>>(`/${TAKTECKAKUNINS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取物料确认详情
 */
export function getEcKakuninByEcDetailId(ecDetailId: string) {
  return request.get<EcDetail>(`/${TAKTECKAKUNINS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新物料确认
 */
export function updateEcKakunin(ecDetailId: string, dto: EcKakuninUpdate) {
  return request.put<EcDetail>(`/${TAKTECKAKUNINS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出物料确认
 */
export function exportEcKakuninData(queryDto?: EcKakuninQuery) {
  return request.get(`/${TAKTECKAKUNINS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
