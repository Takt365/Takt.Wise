// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：signalR.types.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流 SignalR Hub 事件类型（与 TaktSignalRWorkflowPushModels 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 流程定义变更事件
 */
export interface FlowSchemeChangedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 流程定义 ID */
  flowSchemeId: string;
  /** 流程标识 */
  processKey: string;
  /** 流程名称 */
  processName: string;
  /** 变更类型（create / update / delete / status） */
  changeType: string;
  /** 操作人用户名 */
  operatorUserName?: string;
  /** 变更时间 */
  changedAt: string;
}

/**
 * 流程实例推进事件
 */
export interface FlowInstanceProgressedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 流程实例 ID */
  flowInstanceId: string;
  /** 实例编码 */
  instanceCode: string;
  /** 流程名称 */
  processName: string;
  /** 实例状态（int） */
  instanceStatus: number;
  /** 动作类型 */
  actionType: string;
  /** 当前节点名称 */
  currentActivityName?: string;
  /** 发起人用户名 */
  startUserName?: string;
  /** 推进时间 */
  progressedAt: string;
}

/**
 * 待办数量更新事件
 */
export interface FlowTodoCountUpdatedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 目标用户名 */
  userName: string;
  /** 目标用户 ID */
  userId?: string;
  /** 待办数量 */
  todoCount: number;
  /** 统计时间 */
  updatedAt: string;
}
