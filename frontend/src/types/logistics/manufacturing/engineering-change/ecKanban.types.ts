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

/** 部门执行行：已按生管行序对齐；实施标记 + 日期/批次（-1=该生管行下无对应执行） */
export interface EcKanbanDeptLine {
  /** 0=否 1=是；-1=无对应行 */
  isImplemented: number;
  /** 行数（原样行常为 1） */
  lineCount?: number;
  /** 已实施行数 */
  implementedCount?: number;
  /** 未实施行数 */
  notImplementedCount?: number;
  /** 机种（生管/采购等本行组展示） */
  ecModelCode?: string;
  /** 成品（生管/采购等本行组展示） */
  ecRootMaterialCode?: string;
  stageDate?: string | null;
  /** 多行聚合时可能为顿号连接的多个 yyyyMMdd */
  dateDisplayText?: string;
  /** 多行聚合时可能为顿号连接的多个批次/单号 */
  prodBatch?: string;
}

/** 看板采购页签行（TaktEcPur 业务列原样） */
export interface EcKanbanPurLine {
  ecPurId: string;
  lineNumber: number;
  discontinuedStatus: string;
  ecCode: string;
  ecModelCodeGroup: string;
  ecRootMaterialCodeGroup: string;
  ecNewMaterialCodeGroup: string;
  ecNewPurchaseType: string;
  isImplemented: number;
  execContent?: string | null;
  ecOldPartBalance?: string | null;
  inboundDate?: string | null;
  deptCode: string;
  deptName: string;
  ecImplScope: number;
  ecControlScope: number;
  ecGrainKey: string;
  isObsolete: number;
}

/** 看板受检页签行（TaktEcIqc 原样） */
export interface EcKanbanIqcLine {
  ecIqcId: string;
  lineNumber: number;
  discontinuedStatus: string;
  ecCode: string;
  ecModelCodeGroup: string;
  ecRootMaterialCodeGroup: string;
  ecNewMaterialCodeGroup: string;
  ecNewRequiresInspection: number;
  isImplemented: number;
  execContent?: string | null;
  inspectionDate?: string | null;
  deptCode: string;
  deptName: string;
  ecImplScope: number;
  ecControlScope: number;
  ecGrainKey: string;
  isObsolete: number;
}

/** 看板部管页签行（TaktEcMc 原样） */
export interface EcKanbanMcLine {
  ecMcId: string;
  lineNumber: number;
  ecCode: string;
  discontinuedStatus: string;
  ecModelCodeGroup: string;
  ecRootMaterialCodeGroup: string;
  ecNewMaterialCodeGroup: string;
    ecNewPurchaseType: string;
  ecNewWarehouse: string;
  isImplemented: number;
  scheduledDate?: string | null;
  scheduledBatch?: string | null;
  execContent?: string | null;
  outboundBatch?: string | null;
  outboundDate?: string | null;
  deptCode: string;
  deptName: string;
  ecImplScope: number;
  ecControlScope: number;
  ecGrainKey: string;
  isObsolete: number;
}

/** 看板物料页签行（TaktEcMtl 原样） */
export interface EcKanbanMtlLine {
  ecMtlId: string;
  lineNumber: number;
  ecCode: string;
  discontinuedStatus: string;
  ecModelCodeGroup: string;
  ecRootMaterialCodeGroup: string;
  ecNewMaterialCodeGroup: string;
  ecParentMaterialCode: string;
  ecParentMaterialDescription: string;
    ecNewPurchaseType: string;
  ecNewWarehouse: string;
  isImplemented: number;
  scheduledDate?: string | null;
  scheduledBatch?: string | null;
  execContent?: string | null;
  outboundBatch?: string | null;
  outboundDate?: string | null;
  deptCode: string;
  deptName: string;
  ecImplScope: number;
  ecControlScope: number;
  ecGrainKey: string;
  isObsolete: number;
}

export interface EcKanban extends CompanyDtoBase {
  ecTeId: string;
  /** 发行日期 */
  ecIssueDate: string;
  /** 技术担当，设变组编码 */
  ecLeader: string;
  ecCode: string;
  /** 机种（兼容；展开行以 pcLines[i].ecModelCode 为准） */
  ecModelCode: string;
  /** 成品（兼容；展开行以 pcLines[i].ecRootMaterialCode 为准） */
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
  /** 采购课执行行（精简日期/批次） */
  purLines?: EcKanbanDeptLine[];
  /** 采购课完整业务行（采购页签） */
  purDetailLines?: EcKanbanPurLine[];
  /** 受检课完整业务行 */
  iqcDetailLines?: EcKanbanIqcLine[];
  /** 部管课完整业务行 */
  mcDetailLines?: EcKanbanMcLine[];
  /** 物料完整业务行 */
  mtlDetailLines?: EcKanbanMtlLine[];
  /** 生管课执行行 */
  pcLines?: EcKanbanDeptLine[];
  /** 受检课执行行 */
  iqcLines?: EcKanbanDeptLine[];
  /** 部管课执行行 */
  mcLines?: EcKanbanDeptLine[];
  /** 物料执行行 */
  mtlLines?: EcKanbanDeptLine[];
  /** 制二课执行行 */
  mfg2Lines?: EcKanbanDeptLine[];
  /** 制一课执行行 */
  mfg1Lines?: EcKanbanDeptLine[];
  /** 品管课执行行 */
  qcLines?: EcKanbanDeptLine[];
  /** 制技执行行 */
  meLines?: EcKanbanDeptLine[];
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
  /** 已实施行数（旭日外圈） */
  implemented: number;
  /** 未实施行数（旭日外圈） */
  notImplemented: number;
  /** 该部门尚无执行行的设变单数 */
  ecNotStarted: number;
  /** 该部门有未实施明细的设变单数 */
  ecInProgress: number;
  /** 该部门明细全部已实施的设变单数 */
  ecCompleted: number;
}

/** 看板统计·按月 */
export interface EcKanbanMonthStat {
  yearMonth: string;
  ecCount: number;
}

/** 看板统计页签 */
export interface EcKanbanBoardStat {
  /** 主表行数（旭日内围） */
  ecTotal: number;
  /** 明细行数（旭日中围） */
  detailTotal: number;
  notStarted: number;
  inProgress: number;
  officiallyCompleted: number;
  fullyCompleted: number;
  /** 各部门执行行（旭日外围 = implemented + notImplemented） */
  depts: EcKanbanDeptStat[];
  months: EcKanbanMonthStat[];
}
