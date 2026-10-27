// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktSignalr.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 双 Hub 连接管理；每类事件单 Hub 推送/监听，与后端 TaktSignalRDispatchService 对齐
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import * as signalR from '@microsoft/signalr';
import type {
  BroadcastMessage,
  ForceLogoutEvent,
  ForceLogoutScheduledEvent,
  MessageReadEvent,
  MessageSentEvent,
  OnlineMessageEvent,
  OnlineUser,
  SignalRErrorEvent,
  SignalRMessage,
  UserConnectedEvent,
  UserDisconnectedEvent,
} from '@/types/foundation/signalR.types';
import type { MessageStatistics } from '@/types/foundation/message.types';
import type { OnlineStatistics } from '@/types/foundation/online.types';
import type {
  FlowInstanceProgressedEvent,
  FlowSchemeChangedEvent,
  FlowTodoCountUpdatedEvent,
} from '@/types/workflow/signalR.types';
import type {
  QuartzTaskChangedEvent,
  QuartzTaskExecutedEvent,
} from '@/types/foundation/quartzSignalR.types';
import type { BomMaterialCostItemRecalculateCompletedEvent } from '@/types/logistics/manufacturing/bom/materialCostItemSignalR.types';
import type { EcTePersistCompletedEvent } from '@/types/logistics/manufacturing/engineering-change/ecTePersistSignalR.types';
import type {
  EcChangeClosedEvent,
  EcChangeNotificationEvent,
  EcExecutionTaskAlertEvent,
  EcExecutionTaskAssignedEvent,
  EcExecutionTaskProgressEvent,
  EcExecutionTaskProgressReport,
  EcNotificationConfirmRequest,
  EcNotificationConfirmedEvent,
} from '@/types/logistics/manufacturing/engineering-change/ecChangeSignalR.types';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { getAppOrigin, joinOriginPath, requireViteEnv } from '@/config/viteEnv';
import { refreshOAuthTokens } from '@/utils/oauth';
import { createLogger } from '@/utils/logger';
import {
  buildTaktClientProfileHeaders,
  buildTaktClientProfileQueryParams,
} from '@/utils/taktClientProfile';

const signalrLogger = createLogger('signalr');

/**
 * 从 SignalR 载荷读取字符串（兼容 camelCase / PascalCase）
 * @param raw 原始对象
 * @param camelKey camelCase 键
 * @param pascalKey PascalCase 键
 * @returns 字符串；缺失时返回空串
 */
function readSignalRPayloadString(
  raw: Record<string, unknown>,
  camelKey: string,
  pascalKey: string,
): string {
  const value = raw[camelKey] ?? raw[pascalKey];
  return value != null ? String(value) : '';
}

/**
 * 从 SignalR 载荷读取数字（兼容 camelCase / PascalCase）
 * @param raw 原始对象
 * @param camelKey camelCase 键
 * @param pascalKey PascalCase 键
 * @param fallback 默认值
 * @returns 数字
 */
function readSignalRPayloadNumber(
  raw: Record<string, unknown>,
  camelKey: string,
  pascalKey: string,
  fallback = 0,
): number {
  const value = raw[camelKey] ?? raw[pascalKey];
  if (typeof value === 'number' && Number.isFinite(value)) {
    return value;
  }
  if (typeof value === 'string' && value.trim() !== '') {
    const parsed = Number(value);
    return Number.isFinite(parsed) ? parsed : fallback;
  }
  return fallback;
}

/**
 * 规范化 SignalR 私信载荷（Hub 序列化策略变更时仍可读）
 * @param raw Hub 原始载荷
 * @returns 规范化私信
 */
export function normalizeSignalRMessage(raw: unknown): SignalRMessage {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  const messageId = readSignalRPayloadString(payload, 'messageId', 'MessageId');
  return {
    messageId: messageId || undefined,
    fromUserName: readSignalRPayloadString(payload, 'fromUserName', 'FromUserName'),
    fromUserId: readSignalRPayloadString(payload, 'fromUserId', 'FromUserId') || undefined,
    fromUserNickName: readSignalRPayloadString(payload, 'fromUserNickName', 'FromUserNickName') || undefined,
    toUserName: readSignalRPayloadString(payload, 'toUserName', 'ToUserName'),
    toUserId: readSignalRPayloadString(payload, 'toUserId', 'ToUserId') || undefined,
    messageTitle: readSignalRPayloadString(payload, 'messageTitle', 'MessageTitle') || undefined,
    messageContent: readSignalRPayloadString(payload, 'messageContent', 'MessageContent'),
    fileName: readSignalRPayloadString(payload, 'fileName', 'FileName') || undefined,
    accessUrl: readSignalRPayloadString(payload, 'accessUrl', 'AccessUrl') || undefined,
    messageType: readSignalRPayloadString(payload, 'messageType', 'MessageType') || 'system',
    messageGroup: readSignalRPayloadString(payload, 'messageGroup', 'MessageGroup') || 'message',
    sendTime: readSignalRPayloadString(payload, 'sendTime', 'SendTime'),
    readTime: readSignalRPayloadString(payload, 'readTime', 'ReadTime') || undefined,
    readStatus: readSignalRPayloadNumber(payload, 'readStatus', 'ReadStatus', 0),
  };
}

/**
 * 规范化消息统计载荷（兼容 camelCase / PascalCase）
 * @param raw Hub 原始载荷
 * @returns 消息统计
 */
export function normalizeMessageStatistics(raw: unknown): MessageStatistics {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    userName: readSignalRPayloadString(payload, 'userName', 'userName'),
    userId: readSignalRPayloadString(payload, 'userId', 'UserId') || undefined,
    totalCount: readSignalRPayloadNumber(payload, 'totalCount', 'TotalCount', 0),
    readCount: readSignalRPayloadNumber(payload, 'readCount', 'ReadCount', 0),
    unreadCount: readSignalRPayloadNumber(payload, 'unreadCount', 'UnreadCount', 0),
  };
}

/**
 * 规范化流程定义变更载荷
 * @param raw Hub 原始载荷
 * @returns 流程定义变更事件
 */
export function normalizeFlowSchemeChanged(raw: unknown): FlowSchemeChangedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    flowSchemeId: readSignalRPayloadString(payload, 'flowSchemeId', 'FlowSchemeId'),
    processKey: readSignalRPayloadString(payload, 'processKey', 'ProcessKey'),
    processName: readSignalRPayloadString(payload, 'processName', 'ProcessName'),
    changeType: readSignalRPayloadString(payload, 'changeType', 'ChangeType'),
    operatorUserName: readSignalRPayloadString(payload, 'operatorUserName', 'OperatorUserName') || undefined,
    changedAt: readSignalRPayloadString(payload, 'changedAt', 'ChangedAt'),
  };
}

/**
 * 规范化流程实例推进载荷
 * @param raw Hub 原始载荷
 * @returns 流程实例推进事件
 */
export function normalizeFlowInstanceProgressed(raw: unknown): FlowInstanceProgressedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    flowInstanceId: readSignalRPayloadString(payload, 'flowInstanceId', 'FlowInstanceId'),
    instanceCode: readSignalRPayloadString(payload, 'instanceCode', 'InstanceCode'),
    processName: readSignalRPayloadString(payload, 'processName', 'ProcessName'),
    instanceStatus: readSignalRPayloadNumber(payload, 'instanceStatus', 'InstanceStatus', 0),
    actionType: readSignalRPayloadString(payload, 'actionType', 'ActionType'),
    currentActivityName: readSignalRPayloadString(payload, 'currentActivityName', 'CurrentActivityName') || undefined,
    startUserName: readSignalRPayloadString(payload, 'startUserName', 'StartUserName') || undefined,
    progressedAt: readSignalRPayloadString(payload, 'progressedAt', 'ProgressedAt'),
  };
}

/**
 * 规范化待办数量更新载荷
 * @param raw Hub 原始载荷
 * @returns 待办数量更新事件
 */
export function normalizeFlowTodoCountUpdated(raw: unknown): FlowTodoCountUpdatedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    userName: readSignalRPayloadString(payload, 'userName', 'userName'),
    userId: readSignalRPayloadString(payload, 'userId', 'UserId') || undefined,
    todoCount: readSignalRPayloadNumber(payload, 'todoCount', 'TodoCount', 0),
    updatedAt: readSignalRPayloadString(payload, 'updatedAt', 'UpdatedAt'),
  };
}

/**
 * 规范化定时任务定义变更载荷
 * @param raw Hub 原始载荷
 * @returns 定时任务定义变更事件
 */
export function normalizeQuartzTaskChanged(raw: unknown): QuartzTaskChangedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    quartzTaskId: readSignalRPayloadString(payload, 'quartzTaskId', 'QuartzTaskId'),
    taskCode: readSignalRPayloadString(payload, 'taskCode', 'TaskCode'),
    taskName: readSignalRPayloadString(payload, 'taskName', 'TaskName'),
    changeType: readSignalRPayloadString(payload, 'changeType', 'ChangeType'),
    operatorUserName: readSignalRPayloadString(payload, 'operatorUserName', 'OperatorUserName') || undefined,
    changedAt: readSignalRPayloadString(payload, 'changedAt', 'ChangedAt'),
  };
}

/**
 * 规范化定时任务执行完成载荷
 * @param raw Hub 原始载荷
 * @returns 定时任务执行完成事件
 */
export function normalizeQuartzTaskExecuted(raw: unknown): QuartzTaskExecutedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    quartzTaskId: readSignalRPayloadString(payload, 'quartzTaskId', 'QuartzTaskId'),
    quartzLogId: readSignalRPayloadString(payload, 'quartzLogId', 'QuartzLogId'),
    taskCode: readSignalRPayloadString(payload, 'taskCode', 'TaskCode'),
    taskName: readSignalRPayloadString(payload, 'taskName', 'TaskName'),
    executeStatus: readSignalRPayloadNumber(payload, 'executeStatus', 'ExecuteStatus', 0),
    executeMessage: readSignalRPayloadString(payload, 'executeMessage', 'ExecuteMessage') || undefined,
    errorInfo: readSignalRPayloadString(payload, 'errorInfo', 'ErrorInfo') || undefined,
    executeDuration: readSignalRPayloadNumber(payload, 'executeDuration', 'ExecuteDuration', 0),
    executeCount: readSignalRPayloadNumber(payload, 'executeCount', 'ExecuteCount', 0),
    lastRunAt: readSignalRPayloadString(payload, 'lastRunAt', 'LastRunAt') || undefined,
    nextRunAt: readSignalRPayloadString(payload, 'nextRunAt', 'NextRunAt') || undefined,
    triggerUserName: readSignalRPayloadString(payload, 'triggerUserName', 'TriggerUserName') || undefined,
    executedAt: readSignalRPayloadString(payload, 'executedAt', 'ExecutedAt'),
  };
}

/**
 * 规范化 BOM 物料成本机种月平均重算完成载荷
 * @param raw Hub 原始载荷
 * @returns 重算完成事件
 */
export function normalizeBomMaterialCostItemRecalculateCompleted(raw: unknown): BomMaterialCostItemRecalculateCompletedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    triggerUserName: readSignalRPayloadString(payload, 'triggerUserName', 'TriggerUserName'),
    processedMonth: readSignalRPayloadString(payload, 'processedMonth', 'ProcessedMonth'),
    forceRecalculate: Boolean(payload.forceRecalculate ?? payload.ForceRecalculate),
    executeStatus: readSignalRPayloadNumber(payload, 'executeStatus', 'ExecuteStatus', 0),
    executeDuration: readSignalRPayloadNumber(payload, 'executeDuration', 'ExecuteDuration', 0),
    errorMessage: readSignalRPayloadString(payload, 'errorMessage', 'ErrorMessage') || undefined,
    scannedRowCount: readSignalRPayloadNumber(payload, 'scannedRowCount', 'ScannedRowCount', 0),
    refreshedGroupCount: readSignalRPayloadNumber(payload, 'refreshedGroupCount', 'RefreshedGroupCount', 0),
    skippedGroupCount: readSignalRPayloadNumber(payload, 'skippedGroupCount', 'SkippedGroupCount', 0),
    resetGroupCount: readSignalRPayloadNumber(payload, 'resetGroupCount', 'ResetGroupCount', 0),
    processedMonthCount: readSignalRPayloadNumber(payload, 'processedMonthCount', 'ProcessedMonthCount', 0),
    completedAt: readSignalRPayloadString(payload, 'completedAt', 'CompletedAt'),
  };
}

/**
 * 规范化设变技术课主表后台保存完成载荷
 * @param raw Hub 原始载荷
 * @returns 保存完成事件
 */
export function normalizeEcTePersistCompleted(raw: unknown): EcTePersistCompletedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    tenantCode: readSignalRPayloadString(payload, 'tenantCode', 'TenantCode'),
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    triggerUserName: readSignalRPayloadString(payload, 'triggerUserName', 'TriggerUserName'),
    plantCode: readSignalRPayloadString(payload, 'plantCode', 'PlantCode'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    isUpdate: Boolean(payload.isUpdate ?? payload.IsUpdate),
    ecTeId: readSignalRPayloadString(payload, 'ecTeId', 'EcTeId'),
    detailCount: readSignalRPayloadNumber(payload, 'detailCount', 'DetailCount', 0),
    executeStatus: readSignalRPayloadNumber(payload, 'executeStatus', 'ExecuteStatus', 0),
    executeDuration: readSignalRPayloadNumber(payload, 'executeDuration', 'ExecuteDuration', 0),
    errorMessage: readSignalRPayloadString(payload, 'errorMessage', 'ErrorMessage') || undefined,
    completedAt: readSignalRPayloadString(payload, 'completedAt', 'CompletedAt'),
  };
}

/**
 * 规范化工程变更通知推送载荷
 * @param raw Hub 原始载荷
 * @returns 变更通知事件
 */
export function normalizeEcChangeNotification(raw: unknown): EcChangeNotificationEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    deliveryId: readSignalRPayloadString(payload, 'deliveryId', 'DeliveryId'),
    ecNotificationId: readSignalRPayloadString(payload, 'ecNotificationId', 'EcNotificationId'),
    ecNotificationCode: readSignalRPayloadString(payload, 'ecNotificationCode', 'EcNotificationCode'),
    ecTeId: readSignalRPayloadString(payload, 'ecTeId', 'EcTeId'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    ecTitle: readSignalRPayloadString(payload, 'ecTitle', 'EcTitle') || undefined,
    deptCode: readSignalRPayloadString(payload, 'deptCode', 'DeptCode'),
    priority: readSignalRPayloadNumber(payload, 'priority', 'Priority', 1),
    pushedAt: readSignalRPayloadString(payload, 'pushedAt', 'PushedAt'),
  };
}

/**
 * 规范化设变执行任务分配载荷
 * @param raw Hub 原始载荷
 * @returns 任务分配事件
 */
export function normalizeEcExecutionTaskAssigned(raw: unknown): EcExecutionTaskAssignedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    taskId: readSignalRPayloadString(payload, 'taskId', 'TaskId'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    deptCode: readSignalRPayloadString(payload, 'deptCode', 'DeptCode'),
    taskTitle: readSignalRPayloadString(payload, 'taskTitle', 'TaskTitle'),
    dueDate: readSignalRPayloadString(payload, 'dueDate', 'DueDate') || undefined,
  };
}

/**
 * 规范化设变任务进度载荷
 * @param raw Hub 原始载荷
 * @returns 任务进度事件
 */
export function normalizeEcExecutionTaskProgress(raw: unknown): EcExecutionTaskProgressEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    taskId: readSignalRPayloadString(payload, 'taskId', 'TaskId'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    deptCode: readSignalRPayloadString(payload, 'deptCode', 'DeptCode'),
    taskStatus: readSignalRPayloadNumber(payload, 'taskStatus', 'TaskStatus', 0),
    progressPercent: readSignalRPayloadNumber(payload, 'progressPercent', 'ProgressPercent', 0),
    progressRemark: readSignalRPayloadString(payload, 'progressRemark', 'ProgressRemark') || undefined,
    reporterUserName: readSignalRPayloadString(payload, 'reporterUserName', 'ReporterUserName') || undefined,
    reportedAt: readSignalRPayloadString(payload, 'reportedAt', 'ReportedAt'),
  };
}

/**
 * 规范化设变闭环载荷
 * @param raw Hub 原始载荷
 * @returns 闭环事件
 */
export function normalizeEcChangeClosed(raw: unknown): EcChangeClosedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    ecTeId: readSignalRPayloadString(payload, 'ecTeId', 'EcTeId'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    ecNotificationId: readSignalRPayloadString(payload, 'ecNotificationId', 'EcNotificationId'),
    closedAt: readSignalRPayloadString(payload, 'closedAt', 'ClosedAt'),
  };
}

/**
 * 规范化设变任务预警载荷
 * @param raw Hub 原始载荷
 * @returns 预警事件
 */
export function normalizeEcExecutionTaskAlert(raw: unknown): EcExecutionTaskAlertEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    companyCode: readSignalRPayloadString(payload, 'companyCode', 'CompanyCode'),
    taskId: readSignalRPayloadString(payload, 'taskId', 'TaskId'),
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode'),
    deptCode: readSignalRPayloadString(payload, 'deptCode', 'DeptCode'),
    alertType: readSignalRPayloadString(payload, 'alertType', 'AlertType'),
    message: readSignalRPayloadString(payload, 'message', 'Message'),
  };
}

/**
 * 规范化设变通知确认回执载荷
 * @param raw Hub 原始载荷
 * @returns 确认回执事件
 */
export function normalizeEcNotificationConfirmed(raw: unknown): EcNotificationConfirmedEvent {
  const payload = raw != null && typeof raw === 'object'
    ? raw as Record<string, unknown>
    : {};
  return {
    ecNotificationId: readSignalRPayloadString(payload, 'ecNotificationId', 'EcNotificationId'),
    ecNotificationCode: readSignalRPayloadString(payload, 'ecNotificationCode', 'EcNotificationCode') || undefined,
    ecCode: readSignalRPayloadString(payload, 'ecCode', 'EcCode') || undefined,
    ecDeptGroupCode: readSignalRPayloadString(payload, 'ecDeptGroupCode', 'EcDeptGroupCode'),
    confirmedByGroupName: readSignalRPayloadString(payload, 'confirmedByGroupName', 'ConfirmedByGroupName') || undefined,
    confirmedAt: readSignalRPayloadString(payload, 'confirmedAt', 'ConfirmedAt') || undefined,
  };
}

/**
 * SignalR 事件回调
 */
export interface TaktSignalRCallbacks {
  /**
   * 用户连接
   */
  onUserConnected?: (event: UserConnectedEvent) => void;

  /**
   * 用户断开
   */
  onUserDisconnected?: (event: UserDisconnectedEvent) => void;

  /**
   * 收到私信
   */
  onReceiveMessage?: (message: SignalRMessage) => void;

  /**
   * 收到广播
   */
  onReceiveBroadcast?: (message: BroadcastMessage) => void;

  /**
   * 消息已发送
   */
  onMessageSent?: (event: MessageSentEvent) => void;

  /**
   * 消息已读
   */
  onMessageRead?: (event: MessageReadEvent) => void;

  /**
   * Hub 错误
   */
  onError?: (error: SignalRErrorEvent) => void;

  /**
   * 上线通知
   */
  onOnlineMessage?: (event: OnlineMessageEvent) => void;

  /**
   * 强退下线
   */
  onForceLogout?: (event: ForceLogoutEvent) => void;

  /**
   * 延迟强退预告（倒计时）
   */
  onForceLogoutScheduled?: (event: ForceLogoutScheduledEvent) => void;

  /**
   * 在线统计实时更新
   */
  onOnlineStatisticsUpdated?: (statistics: OnlineStatistics) => void;

  /**
   * 消息统计实时更新
   */
  onMessageStatisticsUpdated?: (statistics: MessageStatistics) => void;

  /**
   * 流程定义变更
   */
  onFlowSchemeChanged?: (event: FlowSchemeChangedEvent) => void;

  /**
   * 流程实例推进
   */
  onFlowInstanceProgressed?: (event: FlowInstanceProgressedEvent) => void;

  /**
   * 待办数量更新
   */
  onFlowTodoCountUpdated?: (event: FlowTodoCountUpdatedEvent) => void;

  /**
   * 定时任务定义变更
   */
  onQuartzTaskChanged?: (event: QuartzTaskChangedEvent) => void;

  /**
   * 定时任务执行完成
   */
  onQuartzTaskExecuted?: (event: QuartzTaskExecutedEvent) => void;

  /**
   * BOM 物料成本机种月平均重算完成
   */
  onBomMaterialCostItemRecalculateCompleted?: (event: BomMaterialCostItemRecalculateCompletedEvent) => void;

  /**
   * 设变技术课主表后台保存完成
   */
  onEcTePersistCompleted?: (event: EcTePersistCompletedEvent) => void;

  /**
   * 工程变更通知推送
   */
  onEcChangeNotification?: (event: EcChangeNotificationEvent) => void;

  /**
   * 设变执行任务分配
   */
  onEcExecutionTaskAssigned?: (event: EcExecutionTaskAssignedEvent) => void;

  /**
   * 设变任务进度
   */
  onEcExecutionTaskProgress?: (event: EcExecutionTaskProgressEvent) => void;

  /**
   * 设变闭环完成
   */
  onEcChangeClosed?: (event: EcChangeClosedEvent) => void;

  /**
   * 设变任务预警
   */
  onEcExecutionTaskAlert?: (event: EcExecutionTaskAlertEvent) => void;

  /**
   * 设变通知确认回执
   */
  onEcNotificationConfirmed?: (event: EcNotificationConfirmedEvent) => void;

  /**
   * Hub 连接状态变化（断开 / 重连中 / 已重连）
   */
  onConnectionStateChange?: (state: {
    connectHub: signalR.HubConnectionState;
    notificationHub: signalR.HubConnectionState;
    ecChangeHub: signalR.HubConnectionState;
  }) => void;
}

/**
 * 拼接 SignalR Hub 完整 URL（VITE_APP_ORIGIN + Hub 路径环境变量）
 * @param hubPathEnvKey Hub 路径环境变量名
 */
function buildSignalRHubUrl(
  hubPathEnvKey:
    | 'VITE_SIGNALR_HUB_CONNECT_PATH'
    | 'VITE_SIGNALR_HUB_NOTIFICATION_PATH'
    | 'VITE_SIGNALR_HUB_EC_CHANGE_PATH',
): string {
  return joinOriginPath(getAppOrigin(), requireViteEnv(hubPathEnvKey));
}

/**
 * 构建 SignalR 请求头（negotiate / long polling）
 * @returns 租户与公司请求头
 */
function buildSignalRContextHeaders(): Record<string, string> {
  const tenantStore = useTenantStore();
  const headers: Record<string, string> = {
    ...buildTaktClientProfileHeaders(),
  };
  const tenantCode = tenantStore.tenantCode.trim();
  const companyCode = tenantStore.companyCode.trim();
  if (tenantCode) {
    headers['X-Tenant-Code'] = tenantCode;
  }
  if (companyCode) {
    headers['X-Company-Code'] = companyCode;
  }
  return headers;
}

/**
 * 为 Hub URL 附加 tenant_code / company_code 查询参数（WebSocket 无法自定义 Header）
 * @param hubUrl Hub 完整 URL
 * @returns 带上下文查询参数的 URL
 */
function appendSignalRContextQuery(hubUrl: string): string {
  const tenantStore = useTenantStore();
  const url = new URL(hubUrl);
  const tenantCode = tenantStore.tenantCode.trim();
  const companyCode = tenantStore.companyCode.trim();
  if (tenantCode) {
    url.searchParams.set('tenant_code', tenantCode);
  }
  if (companyCode) {
    url.searchParams.set('company_code', companyCode);
  }
  for (const [key, value] of Object.entries(buildTaktClientProfileQueryParams())) {
    url.searchParams.set(key, value);
  }
  return url.toString();
}

/**
 * SignalR 连接管理器
 */
export class TaktSignalRManager {
  private connectHub: signalR.HubConnection | null = null;

  private notificationHub: signalR.HubConnection | null = null;

  private ecChangeHub: signalR.HubConnection | null = null;

  private heartbeatTimer: number | null = null;

  private readonly heartbeatIntervalMs = 30_000;

  /**
   * 创建 Hub 连接
   * @param hubPath Hub 路径
   */
  private createHubConnection(hubUrl: string): signalR.HubConnection {
    const contextualUrl = appendSignalRContextQuery(hubUrl);
    const headers = buildSignalRContextHeaders();

    return new signalR.HubConnectionBuilder()
      .withUrl(contextualUrl, {
        withCredentials: true,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
        accessTokenFactory: () => {
          const token = useUserStore().token;
          return Promise.resolve(token || '');
        },
        headers,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  /**
   * 挂载连接状态变更回调（断开、自动重连）
   * @param hub Hub 连接
   * @param callbacks 事件回调
   */
  private attachConnectionStateHooks(
    hub: signalR.HubConnection | null,
    callbacks?: TaktSignalRCallbacks,
  ): void {
    if (!hub) {
      return;
    }
    const notify = () => {
      callbacks?.onConnectionStateChange?.(this.getConnectionState());
    };
    hub.onclose(() => {
      notify();
    });
    hub.onreconnecting(() => {
      notify();
    });
    hub.onreconnected(() => {
      notify();
    });
  }

  /**
   * 注册 Connect Hub 事件（在线/强退/在线统计；与 NotificationHub 互斥，每类仅一处监听）
   * @param callbacks 回调
   */
  private registerConnectHubEvents(callbacks?: TaktSignalRCallbacks): void {
    if (!this.connectHub) {
      return;
    }

    this.connectHub.on('OnlineMessage', (event: OnlineMessageEvent) => {
      callbacks?.onOnlineMessage?.(event);
    });

    this.connectHub.on('UserConnected', (event: UserConnectedEvent) => {
      callbacks?.onUserConnected?.(event);
    });

    this.connectHub.on('UserDisconnected', (event: UserDisconnectedEvent) => {
      callbacks?.onUserDisconnected?.(event);
    });

    this.connectHub.on('ForceLogout', (event: ForceLogoutEvent) => {
      callbacks?.onForceLogout?.(event);
    });

    this.connectHub.on('ForceLogoutScheduled', (event: ForceLogoutScheduledEvent) => {
      callbacks?.onForceLogoutScheduled?.(event);
    });

    this.connectHub.on('OnlineStatisticsUpdated', (statistics: OnlineStatistics) => {
      callbacks?.onOnlineStatisticsUpdated?.(statistics);
    });

    this.connectHub.onclose(async (error) => {
      this.stopHeartbeat();

      if (error?.message?.includes('401') || error?.message?.includes('Unauthorized')) {
        await refreshOAuthTokens();
      }
    });
  }

  /**
   * 注册 Notification Hub 事件（私信/广播/消息统计；与 ConnectHub 互斥，每类仅一处监听）
   * @param callbacks 回调
   */
  private registerNotificationHubEvents(callbacks?: TaktSignalRCallbacks): void {
    if (!this.notificationHub) {
      return;
    }

    this.notificationHub.on('ReceiveMessage', (message: unknown) => {
      callbacks?.onReceiveMessage?.(normalizeSignalRMessage(message));
    });

    this.notificationHub.on('ReceiveBroadcast', (message: BroadcastMessage) => {
      callbacks?.onReceiveBroadcast?.(message);
    });

    this.notificationHub.on('MessageSent', (event: MessageSentEvent) => {
      callbacks?.onMessageSent?.(event);
    });

    this.notificationHub.on('MessageRead', (event: MessageReadEvent) => {
      callbacks?.onMessageRead?.(event);
    });

    this.notificationHub.on('Error', (error: SignalRErrorEvent) => {
      callbacks?.onError?.(error);
    });

    this.notificationHub.on('MessageStatisticsUpdated', (statistics: unknown) => {
      callbacks?.onMessageStatisticsUpdated?.(normalizeMessageStatistics(statistics));
    });

    this.notificationHub.on('FlowSchemeChanged', (payload: unknown) => {
      callbacks?.onFlowSchemeChanged?.(normalizeFlowSchemeChanged(payload));
    });

    this.notificationHub.on('FlowInstanceProgressed', (payload: unknown) => {
      callbacks?.onFlowInstanceProgressed?.(normalizeFlowInstanceProgressed(payload));
    });

    this.notificationHub.on('FlowTodoCountUpdated', (payload: unknown) => {
      callbacks?.onFlowTodoCountUpdated?.(normalizeFlowTodoCountUpdated(payload));
    });

    this.notificationHub.on('QuartzTaskChanged', (payload: unknown) => {
      callbacks?.onQuartzTaskChanged?.(normalizeQuartzTaskChanged(payload));
    });

    this.notificationHub.on('QuartzTaskExecuted', (payload: unknown) => {
      callbacks?.onQuartzTaskExecuted?.(normalizeQuartzTaskExecuted(payload));
    });

    this.notificationHub.on('BomMaterialCostItemRecalculateCompleted', (payload: unknown) => {
      callbacks?.onBomMaterialCostItemRecalculateCompleted?.(normalizeBomMaterialCostItemRecalculateCompleted(payload));
    });

    this.notificationHub.on('EcTePersistCompleted', (payload: unknown) => {
      callbacks?.onEcTePersistCompleted?.(normalizeEcTePersistCompleted(payload));
    });
  }

  /**
   * 注册 EcChange Hub 事件（设变通知/任务/闭环）
   * @param callbacks 回调
   */
  private registerEcChangeHubEvents(callbacks?: TaktSignalRCallbacks): void {
    if (!this.ecChangeHub) {
      return;
    }

    this.ecChangeHub.on('ChangeNotification', (payload: unknown) => {
      callbacks?.onEcChangeNotification?.(normalizeEcChangeNotification(payload));
    });

    this.ecChangeHub.on('TaskAssigned', (payload: unknown) => {
      callbacks?.onEcExecutionTaskAssigned?.(normalizeEcExecutionTaskAssigned(payload));
    });

    this.ecChangeHub.on('TaskProgress', (payload: unknown) => {
      callbacks?.onEcExecutionTaskProgress?.(normalizeEcExecutionTaskProgress(payload));
    });

    this.ecChangeHub.on('ChangeClosed', (payload: unknown) => {
      callbacks?.onEcChangeClosed?.(normalizeEcChangeClosed(payload));
    });

    this.ecChangeHub.on('TaskAlert', (payload: unknown) => {
      callbacks?.onEcExecutionTaskAlert?.(normalizeEcExecutionTaskAlert(payload));
    });

    this.ecChangeHub.on('NotificationConfirmed', (payload: unknown) => {
      callbacks?.onEcNotificationConfirmed?.(normalizeEcNotificationConfirmed(payload));
    });
  }

  /**
   * 启动心跳
   */
  private startHeartbeat(): void {
    if (this.heartbeatTimer) {
      return;
    }

    this.heartbeatTimer = window.setInterval(() => {
      if (this.connectHub?.state === signalR.HubConnectionState.Connected) {
        void this.connectHub.invoke('Heartbeat').catch((error: unknown) => {
          signalrLogger.warn('SignalR 心跳失败', { action: 'heartbeat' }, error);
        });
      }
    }, this.heartbeatIntervalMs);
  }

  /**
   * 停止心跳
   */
  private stopHeartbeat(): void {
    if (this.heartbeatTimer) {
      window.clearInterval(this.heartbeatTimer);
      this.heartbeatTimer = null;
    }
  }

  /**
   * 连接所有 Hub
   * @param callbacks 事件回调
   */
  async connectSignalRHubsAsync(callbacks?: TaktSignalRCallbacks): Promise<void> {
    await this.disconnectSignalRHubsAsync();
    this.connectHub = this.createHubConnection(buildSignalRHubUrl('VITE_SIGNALR_HUB_CONNECT_PATH'));
    this.notificationHub = this.createHubConnection(buildSignalRHubUrl('VITE_SIGNALR_HUB_NOTIFICATION_PATH'));
    this.ecChangeHub = this.createHubConnection(buildSignalRHubUrl('VITE_SIGNALR_HUB_EC_CHANGE_PATH'));

    this.registerConnectHubEvents(callbacks);
    this.registerNotificationHubEvents(callbacks);
    this.registerEcChangeHubEvents(callbacks);
    this.attachConnectionStateHooks(this.connectHub, callbacks);
    this.attachConnectionStateHooks(this.notificationHub, callbacks);
    this.attachConnectionStateHooks(this.ecChangeHub, callbacks);

    await this.connectHub.start();
    await this.notificationHub.start();
    await this.ecChangeHub.start();

    this.startHeartbeat();
    callbacks?.onConnectionStateChange?.(this.getConnectionState());
    signalrLogger.info('SignalR Hub 已全部连接');
  }

  /**
   * 断开所有 Hub
   */
  async disconnectSignalRHubsAsync(): Promise<void> {
    this.stopHeartbeat();

    const tasks: Promise<void>[] = [];

    if (this.connectHub) {
      tasks.push(this.connectHub.stop());
      this.connectHub = null;
    }

    if (this.notificationHub) {
      tasks.push(this.notificationHub.stop());
      this.notificationHub = null;
    }

    if (this.ecChangeHub) {
      tasks.push(this.ecChangeHub.stop());
      this.ecChangeHub = null;
    }

    await Promise.allSettled(tasks);
    signalrLogger.info('SignalR Hub 已全部断开');
  }

  /**
   * 获取在线用户列表
   */
  async getOnlineUsersAsync(): Promise<OnlineUser[]> {
    if (!this.connectHub || this.connectHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Connect Hub 未连接');
    }

    const users = await this.connectHub.invoke<OnlineUser[]>('GetOnlineUsers');
    return users ?? [];
  }

  /**
   * 获取未读消息数量
   */
  async getUnreadCountAsync(): Promise<number> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    const count = await this.notificationHub.invoke<number>('GetUnreadCount');
    return count ?? 0;
  }

  /**
   * 发送私信
   */
  async sendMessageAsync(
    toUserName: string,
    messageContent: string,
    messageTitle?: string,
    messageType: string = 'text',
    messageGroup: string = 'message'
  ): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke(
      'SendMessage',
      toUserName,
      messageContent,
      messageTitle,
      messageType,
      messageGroup
    );
  }

  /**
   * 发送广播
   */
  async broadcastMessageAsync(
    messageContent: string,
    messageTitle?: string,
    messageType: string = 'system',
    messageGroup: string = 'message'
  ): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke(
      'BroadcastMessage',
      messageContent,
      messageTitle,
      messageType,
      messageGroup
    );
  }

  /**
   * 标记消息已读
   * @param messageId 消息 ID
   */
  async markMessageAsReadAsync(messageId: number): Promise<void> {
    if (!this.notificationHub || this.notificationHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Notification Hub 未连接');
    }

    await this.notificationHub.invoke('MarkAsRead', messageId);
  }

  /**
   * 获取 Hub 连接状态
   */
  getConnectionState(): {
    connectHub: signalR.HubConnectionState;
    notificationHub: signalR.HubConnectionState;
    ecChangeHub: signalR.HubConnectionState;
  } {
    return {
      connectHub: this.connectHub?.state ?? signalR.HubConnectionState.Disconnected,
      notificationHub: this.notificationHub?.state ?? signalR.HubConnectionState.Disconnected,
      ecChangeHub: this.ecChangeHub?.state ?? signalR.HubConnectionState.Disconnected,
    };
  }

  /**
   * 是否已全部连接
   */
  isConnected(): boolean {
    const state = this.getConnectionState();

    return (
      state.connectHub === signalR.HubConnectionState.Connected &&
      state.notificationHub === signalR.HubConnectionState.Connected &&
      state.ecChangeHub === signalR.HubConnectionState.Connected
    );
  }

  /**
   * 加入设变部门通知组
   * @param deptCode 部门编码
   */
  async joinEcExecGroupAsync(deptCode: string): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('JoinDeptGroup', deptCode);
  }

  /**
   * 离开设变部门通知组
   * @param deptCode 部门编码
   */
  async leaveEcExecGroupAsync(deptCode: string): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('LeaveDeptGroup', deptCode);
  }

  /**
   * 加入设变执行任务进度组
   * @param taskId 任务 ID（string）
   */
  async joinEcTaskGroupAsync(taskId: string): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('JoinTaskGroup', taskId);
  }

  /**
   * 确认变更通知（按投递记录 ID）
   * @param deliveryId 投递记录 ID
   */
  async confirmEcNotificationAsync(deliveryId: string): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('ConfirmNotification', deliveryId);
  }

  /**
   * 确认变更通知（按通知单 + 部门）
   * @param ecNotificationId 通知单 ID
   * @param deptCode 部门编码
   */
  async confirmEcNotificationByDeptAsync(ecNotificationId: string, deptCode: string): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('ConfirmNotificationByDept', ecNotificationId, deptCode);
  }

  /**
   * 上报设变执行任务进度
   * @param dto 进度 DTO
   */
  async reportEcExecutionTaskProgressAsync(dto: EcExecutionTaskProgressReport): Promise<void> {
    if (!this.ecChangeHub || this.ecChangeHub.state !== signalR.HubConnectionState.Connected) {
      throw new Error('EcChange Hub 未连接');
    }
    await this.ecChangeHub.invoke('ReportProgress', dto);
  }
}

/** SignalR 管理器单例 */
export const taktSignalRManager = new TaktSignalRManager();
