// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecImplCtrl.api.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施管控 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type { EcImplCtrl, EcImplCtrlQuery, EcImplCtrlUpdate } from '@/types/logistics/manufacturing/engineering-change/ecImplCtrl.types';

const TAKTECIMPLCTRLS_API_BASE = 'TaktEcImplCtrls';

/**
 * 获取实施管控列表（分页）
 */
export function getEcImplCtrlList(queryDto: EcImplCtrlQuery) {
  return request.get<TaktPagedResult<EcImplCtrl>>(`/${TAKTECIMPLCTRLS_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取实施管控详情
 */
export function getEcImplCtrlByEcDetailId(ecDetailId: string) {
  return request.get<EcDetail>(`/${TAKTECIMPLCTRLS_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新实施管控
 */
export function updateEcImplCtrl(ecDetailId: string, dto: EcImplCtrlUpdate) {
  return request.put<EcDetail>(`/${TAKTECIMPLCTRLS_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出实施管控
 */
export function exportEcImplCtrlData(queryDto?: EcImplCtrlQuery) {
  return request.get(`/${TAKTECIMPLCTRLS_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}
