// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecBatch.api.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变投入批次 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import type { EcBatch, EcBatchQuery, EcBatchUpdate } from '@/types/logistics/manufacturing/engineering-change/ecBatch.types';
import type {
  EcExecBatchTransposedQuery,
  EcExecBatchTransposedResult,
} from '@/types/logistics/manufacturing/engineering-change/ecExecTransposed.types';

const TAKTECBATCHES_API_BASE = 'TaktEcBatches';

/**
 * 获取投入批次列表（分页）
 */
export function getEcBatchList(queryDto: EcBatchQuery) {
  return request.get<TaktPagedResult<EcBatch>>(`/${TAKTECBATCHES_API_BASE}/list`, { params: queryDto });
}

/**
 * 获取投入批次详情
 */
export function getEcBatchByEcDetailId(ecDetailId: string) {
  return request.get<EcBatch>(`/${TAKTECBATCHES_API_BASE}/detail/${ecDetailId}`);
}

/**
 * 更新投入批次
 */
export function updateEcBatch(ecDetailId: string, dto: EcBatchUpdate) {
  return request.put<EcBatch>(`/${TAKTECBATCHES_API_BASE}/detail/${ecDetailId}`, dto);
}

/**
 * 导出投入批次
 */
export function exportEcBatchData(queryDto?: EcBatchQuery) {
  return request.get(`/${TAKTECBATCHES_API_BASE}/export`, { params: queryDto, responseType: 'blob' });
}

/**
 * 获取投入批次转置列表（分页；行=设变明细，列=各阶段日期+批次）
 * @param queryDto 查询参数
 * @returns {Promise<EcExecBatchTransposedResult>} 批次转置结果
 */
export function getEcBatchTransposedList(queryDto: EcExecBatchTransposedQuery): Promise<EcExecBatchTransposedResult> {
  return request.get<EcExecBatchTransposedResult>(`/${TAKTECBATCHES_API_BASE}/transposed`, { params: queryDto });
}
