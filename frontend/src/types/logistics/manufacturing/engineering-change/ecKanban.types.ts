// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecKanban.types.ts
// 创建时间：2026-06-22
// 功能描述：设变看板类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

export interface EcKanbanDeptStage {
  deptCode: string;
  implementedCount: number;
  totalCount: number;
}

export interface EcKanban extends CompanyDtoBase {
  ecGijutsuId: string;
  ecCode: string;
  ecTitle: string;
  changeStatus: number;
  ecStatus: number;
  ecLeader: string;
  detailCount: number;
  deptStages: EcKanbanDeptStage[];
  /** 当前待实施部门编码 */
  currentDeptCode?: string | null;
  /** 当前部门待实施明细数 */
  pendingAtCurrentDeptCount: number;
  /** 实施路径状态 0 未开始 1 实施中 2 正式完成 3 全部完成 */
  implementationStatus: number;
  /** 品管课是否已全部实施 0/1 */
  isOfficiallyCompleted: number;
}

export interface EcKanbanQuery extends TaktPagedQuery {
  ecCode?: string;
  changeStatus?: number;
  ecStatus?: number;
  /** 当前卡点部门 */
  currentDeptCode?: string;
  /** 实施路径状态 */
  implementationStatus?: number;
}
