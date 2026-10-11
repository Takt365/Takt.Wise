// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecExecTransposed.types.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门/批次转置列表类型（TaktEcBukans/transposed、TaktEcBatches/transposed）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/** 设变部门转置单元格 */
export interface EcExecTransposedCell {
  deptCode: string;
  isImplemented: number;
  completedDate?: string;
  displayText?: string | null;
}

/** 设变部门转置行 */
export interface EcExecTransposed {
  ecDetailId: string;
  ecGijutsuId: string;
  lineNumber: number;
  ecIssueDate: string;
  ecLeader: string;
  ecCode: string;
  ecModelCode: string;
  ecNewMaterialCode?: string | null;
  deptCells: Record<string, EcExecTransposedCell>;
}

/** 设变部门转置查询 */
export interface EcExecTransposedQuery extends TaktPagedQuery {
  ecCode?: string;
  ecModelCode?: string;
  ecNewMaterialCode?: string;
  ecLeader?: string;
  ecIssueDateStart?: string;
  ecIssueDateEnd?: string;
  deptCode?: string;
  isImplemented?: number;
}

/** 设变部门转置分页结果 */
export interface EcExecTransposedResult {
  paged: TaktPagedResult<EcExecTransposed>;
  deptCodeOrder: string[];
}

/** 设变批次转置阶段单元格 */
export interface EcExecBatchTransposedStage {
  stageCode: string;
  stageDate?: string | null;
  batchCode?: string | null;
  dateDisplayText?: string | null;
}

/** 设变批次转置行 */
export interface EcExecBatchTransposed {
  ecDetailId: string;
  ecGijutsuId: string;
  lineNumber: number;
  ecCode: string;
  technicalLiaisonNo?: string | null;
  pNo?: string | null;
  tcjLiaisonNo?: string | null;
  ecIssueDate: string;
  ecModelCode: string;
  ecNewMaterialCode?: string | null;
  ecEntryDate: string;
  stageCells: Record<string, EcExecBatchTransposedStage>;
}

/** 设变批次转置查询 */
export interface EcExecBatchTransposedQuery extends TaktPagedQuery {
  ecCode?: string;
  ecModelCode?: string;
  ecNewMaterialCode?: string;
  ecIssueDateStart?: string;
  ecIssueDateEnd?: string;
  batchCode?: string;
}

/** 设变批次转置分页结果 */
export interface EcExecBatchTransposedResult {
  paged: TaktPagedResult<EcExecBatchTransposed>;
  stageCodeOrder: string[];
}
