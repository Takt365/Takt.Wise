// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：quartzSignalR.types.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务 SignalR Hub 事件类型（与 TaktSignalRQuartzPushModels 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 定时任务定义变更事件
 */
export interface QuartzTaskChangedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 定时任务 ID */
  quartzTaskId: string;
  /** 任务编码 */
  taskCode: string;
  /** 任务名称 */
  taskName: string;
  /** 变更类型（create / update / delete / status） */
  changeType: string;
  /** 操作人用户名 */
  operatorUserName?: string;
  /** 变更时间 */
  changedAt: string;
}

/**
 * 定时任务执行完成事件
 */
export interface QuartzTaskExecutedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 定时任务 ID */
  quartzTaskId: string;
  /** 执行日志 ID */
  quartzLogId: string;
  /** 任务编码 */
  taskCode: string;
  /** 任务名称 */
  taskName: string;
  /** 执行状态（int） */
  executeStatus: number;
  /** 执行摘要消息 */
  executeMessage?: string;
  /** 错误信息（失败时） */
  errorInfo?: string;
  /** 执行耗时（毫秒） */
  executeDuration: number;
  /** 累计执行次数 */
  executeCount: number;
  /** 上次执行 */
  lastRunAt?: string;
  /** 下次执行 */
  nextRunAt?: string;
  /** 触发用户名 */
  triggerUserName?: string;
  /** 执行时间 */
  executedAt: string;
}
