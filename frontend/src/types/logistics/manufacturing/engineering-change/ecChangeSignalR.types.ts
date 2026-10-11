// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecChangeSignalR.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更 SignalR 事件类型（与 TaktEcChangeSignalRPushModels 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 变更通知推送 */
export interface EcChangeNotificationEvent {
  companyCode: string;
  deliveryId: string;
  ecNotificationId: string;
  ecNotificationCode: string;
  ecGijutsuId: string;
  ecCode: string;
  ecTitle?: string;
  deptCode: string;
  priority: number;
  pushedAt: string;
}

/** 执行任务分配推送 */
export interface EcExecutionTaskAssignedEvent {
  companyCode: string;
  taskId: string;
  ecCode: string;
  deptCode: string;
  taskTitle: string;
  dueDate?: string;
}

/** 任务进度推送 */
export interface EcExecutionTaskProgressEvent {
  companyCode: string;
  taskId: string;
  ecCode: string;
  deptCode: string;
  taskStatus: number;
  progressPercent: number;
  progressRemark?: string;
  reporterUserName?: string;
  reportedAt: string;
}

/** 变更闭环完成推送 */
export interface EcChangeClosedEvent {
  companyCode: string;
  ecGijutsuId: string;
  ecCode: string;
  ecNotificationId: string;
  closedAt: string;
}

/** 任务超时/阻塞预警 */
export interface EcExecutionTaskAlertEvent {
  companyCode: string;
  taskId: string;
  ecCode: string;
  deptCode: string;
  alertType: string;
  message: string;
}

/** 通知确认回执（推送给发起人） */
export interface EcNotificationConfirmedEvent {
  ecNotificationId: string;
  ecNotificationCode?: string;
  ecCode?: string;
  ecDeptGroupCode: string;
  confirmedByGroupName?: string;
  confirmedAt?: string;
}

/** 进度上报请求 DTO */
export interface EcExecutionTaskProgressReport {
  taskId: string;
  taskStatus: number;
  progressPercent: number;
  progressRemark?: string;
}

/** 通知确认请求 DTO */
export interface EcNotificationConfirmRequest {
  deliveryId?: string;
  ecNotificationId?: string;
  ecDeptGroupCode?: string;
}
