// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecKanban.types.ts
// 创建时间：2026-06-22
// 功能描述：设变看板。一行一个设变号，各部门列是该部门每一条执行行的实施标记
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

export interface EcKanban extends CompanyDtoBase {
  ecTeId: string;
  /** 发行日期 */
  ecIssueDate: string;
  /** 技术担当，设变组编码 */
  ecLeader: string;
  ecCode: string;
  /** 机种，同一设变号多个机种用顿号连接 */
  ecModelCode: string;
  /** 成品，同一设变号多个成品用顿号连接 */
  ecRootMaterialCode: string;
  /** 技术课，技术主表录入日期 */
  ecEntryDate: string;
  /** 采购课每一行的实施标记，0 否 1 是 */
  purImplemented: number[];
  /** 生管课每一行的实施标记 */
  pcImplemented: number[];
  /** 受检课每一行的实施标记 */
  iqcImplemented: number[];
  /** 部管课每一行的实施标记 */
  mcImplemented: number[];
  /** 物料每一行的实施标记 */
  mtlImplemented: number[];
  /** 制二课每一行的实施标记 */
  mfg2Implemented: number[];
  /** 制一课每一行的实施标记 */
  mfg1Implemented: number[];
  /** 品管课每一行的实施标记 */
  qcImplemented: number[];
  /** 制技每一行的更新 SOP 标记 */
  meImplemented: number[];
}

export interface EcKanbanQuery extends TaktPagedQuery {
  ecCode?: string;
  changeStatus?: number;
  ecStatus?: number;
  /** 录入日期起，yyyy-MM-dd */
  ecEntryDateStart?: string;
  /** 录入日期止，yyyy-MM-dd */
  ecEntryDateEnd?: string;
  cultureCode?: string;
  /** 实施路径状态 */
  implementationStatus?: number;
}

/** 看板统计·部门执行 */
export interface EcKanbanDeptStat {
  deptCode: string;
  implemented: number;
  notImplemented: number;
}

/** 看板统计·按月 */
export interface EcKanbanMonthStat {
  yearMonth: string;
  ecCount: number;
}

/** 看板统计页签 */
export interface EcKanbanBoardStat {
  ecTotal: number;
  notStarted: number;
  inProgress: number;
  officiallyCompleted: number;
  fullyCompleted: number;
  depts: EcKanbanDeptStat[];
  months: EcKanbanMonthStat[];
}
