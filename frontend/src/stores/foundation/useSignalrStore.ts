// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/foundation
// 文件名称：useSignalrStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 状态管理（连接、消息、在线用户、强退）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import * as signalR from '@microsoft/signalr';
import { notify, showPrivateMessageNotify } from '@/utils/notification';
import {
  HEADER_ONLINE_AUTO_READ_MS,
  useHeaderNotificationStore,
} from '@/stores/navigation/useHeaderNotificationStore';
import type { SignalRMessageWithId } from '@/types/common.types';
import type {
  BroadcastMessage,
  ForceLogoutEvent,
  ForceLogoutScheduledEvent,
  OnlineUser,
  SignalRMessage,
} from '@/types/foundation/signalR.types';
import type { MessageStatistics } from '@/types/foundation/message.types';
import type { OnlineStatistics } from '@/types/foundation/online.types';
import { TaktReadStatus } from '@/utils/common';
import { getMessageStatistics } from '@/api/foundation/message.api';
import { getOnlineStatistics } from '@/api/foundation/online.api';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { taktSignalRManager } from '@/utils/taktSignalr';
import { executeForceLogoutAsync } from '@/bootstrap/taktLogoutFlow';
import { useForceLogoutScheduleStore } from '@/stores/foundation/useForceLogoutScheduleStore';
import { EventBus } from '@/utils/eventBus';
import { createLogger } from '@/utils/logger';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { useWorkflowTodoCountStore } from '@/stores/workflow/useTodoCountStore';
import { WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh';
import { QUARTZ_TABLE_NAME } from '@/composables/useQuartzSignalrRefresh';
import { BOM_MATERIAL_COST_ITEM_TABLE_NAME } from '@/composables/useBomMaterialCostItemRecalculateSignalr';
import {
  EC_GIJUTSU_TABLE_NAME,
  formatEcGijutsuPersistDuration,
} from '@/composables/useEcGijutsuPersistSignalr';
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
import type { EcGijutsuPersistCompletedEvent } from '@/types/logistics/manufacturing/engineering-change/ecGijutsuPersistSignalR.types';
import type {
  EcChangeClosedEvent,
  EcChangeNotificationEvent,
  EcExecutionTaskAlertEvent,
  EcExecutionTaskAssignedEvent,
  EcExecutionTaskProgressEvent,
  EcNotificationConfirmedEvent,
} from '@/types/logistics/manufacturing/engineering-change/ecChangeSignalR.types';
import { EC_NOTIFICATION_TABLE_NAME } from '@/composables/useEcChangeSignalrRefresh';

const signalrStoreLogger = createLogger('signalr-store');

/**
 * 双 Hub 均已连接时补拉工作流待办数量（首连 / 断线重连）
 * @returns {Promise<void>}
 */
async function refreshWorkflowTodoCountAfterHubConnectedAsync(): Promise<void> {
  await useWorkflowTodoCountStore().refreshTodoCountAsync().catch((error: unknown) => {
    signalrStoreLogger.warn('重连后补拉待办数量失败', { action: 'refreshWorkflowTodoCount' }, error);
  });
}

/**
 * 分发工作流 SignalR 事件到 EventBus
 * @param schemeEvent 方案变更
 * @param instanceEvent 实例推进
 * @param todoCountEvent 待办计数
 * @returns {void}
 */
function dispatchWorkflowSignalREvents(
  schemeEvent?: FlowSchemeChangedEvent,
  instanceEvent?: FlowInstanceProgressedEvent,
  todoCountEvent?: FlowTodoCountUpdatedEvent,
): void {
  if (schemeEvent) {
    EventBus.emit('workflow:scheme:changed', schemeEvent);
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.scheme });
  }
  if (instanceEvent) {
    EventBus.emit('workflow:instance:progressed', instanceEvent);
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.todo });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.my });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.processed });
    EventBus.emit('table:refresh', { tableName: WORKFLOW_TABLE_NAMES.instance });
  }
  if (todoCountEvent) {
    useWorkflowTodoCountStore().applyTodoCountFromSignalR(todoCountEvent);
    EventBus.emit('workflow:todo:count-updated', todoCountEvent);
  }
}

/**
 * 分发 Quartz SignalR 事件到 EventBus
 * @param changedEvent 定义变更
 * @param executedEvent 执行完成
 * @returns {void}
 */
function dispatchQuartzSignalREvents(
  changedEvent?: QuartzTaskChangedEvent,
  executedEvent?: QuartzTaskExecutedEvent,
): void {
  if (changedEvent) {
    EventBus.emit('foundation:quartz-task:changed', changedEvent);
    EventBus.emit('table:refresh', { tableName: QUARTZ_TABLE_NAME });
  }
  if (executedEvent) {
    EventBus.emit('foundation:quartz-task:executed', executedEvent);
    EventBus.emit('table:refresh', { tableName: QUARTZ_TABLE_NAME });
  }
}

/**
 * 分发 BOM 物料成本重算 SignalR 事件到 EventBus
 * @param completedEvent 重算完成事件
 * @returns {void}
 */
function dispatchBomMaterialCostItemRecalculateSignalREvents(
  completedEvent?: BomMaterialCostItemRecalculateCompletedEvent,
): void {
  if (completedEvent) {
    EventBus.emit('logistics:bom-material-cost-item:recalculate-completed', completedEvent);
    EventBus.emit('table:refresh', { tableName: BOM_MATERIAL_COST_ITEM_TABLE_NAME });
  }
}

/**
 * 分发设变技术课后台保存 SignalR 事件到 EventBus
 * @param completedEvent 保存完成事件
 * @returns {void}
 */
function dispatchEcGijutsuPersistSignalREvents(
  completedEvent?: EcGijutsuPersistCompletedEvent,
): void {
  if (completedEvent) {
    EventBus.emit('logistics:ec-gijutsu:persist-completed', completedEvent);
    EventBus.emit('table:refresh', { tableName: EC_GIJUTSU_TABLE_NAME });
  }
}

/**
 * 分发工程变更 SignalR 事件到 EventBus
 * @param notificationEvent 变更通知
 * @param taskAssignedEvent 任务分配
 * @param taskProgressEvent 任务进度
 * @param changeClosedEvent 变更闭环
 * @param taskAlertEvent 任务预警
 * @param notificationConfirmedEvent 通知确认回执
 */
function dispatchEcChangeSignalREvents(
  notificationEvent?: EcChangeNotificationEvent,
  taskAssignedEvent?: EcExecutionTaskAssignedEvent,
  taskProgressEvent?: EcExecutionTaskProgressEvent,
  changeClosedEvent?: EcChangeClosedEvent,
  taskAlertEvent?: EcExecutionTaskAlertEvent,
  notificationConfirmedEvent?: EcNotificationConfirmedEvent,
): void {
  if (notificationEvent) {
    EventBus.emit('logistics:ec-change:notification', notificationEvent);
  }
  if (taskAssignedEvent) {
    EventBus.emit('logistics:ec-change:task-assigned', taskAssignedEvent);
  }
  if (taskProgressEvent) {
    EventBus.emit('logistics:ec-change:task-progress', taskProgressEvent);
  }
  if (changeClosedEvent) {
    EventBus.emit('logistics:ec-change:closed', changeClosedEvent);
    EventBus.emit('table:refresh', { tableName: EC_NOTIFICATION_TABLE_NAME });
  }
  if (taskAlertEvent) {
    EventBus.emit('logistics:ec-change:task-alert', taskAlertEvent);
  }
  if (notificationConfirmedEvent) {
    EventBus.emit('logistics:ec-change:notification-confirmed', notificationConfirmedEvent);
    EventBus.emit('table:refresh', { tableName: EC_NOTIFICATION_TABLE_NAME });
  }
}

/**
 * SignalR 状态管理
 */
export const useSignalRStore = defineStore('signalr', () => {
  const connectHubState = ref<signalR.HubConnectionState>(signalR.HubConnectionState.Disconnected);
  const notificationHubState = ref<signalR.HubConnectionState>(signalR.HubConnectionState.Disconnected);
  const ecChangeHubState = ref<signalR.HubConnectionState>(signalR.HubConnectionState.Disconnected);
  const onlineUsers = ref<OnlineUser[]>([]);
  const unreadCount = ref(0);
  const onlineStatistics = ref<OnlineStatistics | null>(null);
  const messageStatistics = ref<MessageStatistics | null>(null);
  const messages = ref<SignalRMessage[]>([]);
  const broadcastMessages = ref<BroadcastMessage[]>([]);
  const connecting = ref(false);

  /**
   * 双 Hub 是否均已连接
   */
  const isConnected = computed(
    () =>
      connectHubState.value === signalR.HubConnectionState.Connected &&
      notificationHubState.value === signalR.HubConnectionState.Connected &&
      ecChangeHubState.value === signalR.HubConnectionState.Connected
  );

  /**
   * 同步 Hub 连接状态
   */
  function syncConnectionState(): void {
    const state = taktSignalRManager.getConnectionState();
    connectHubState.value = state.connectHub;
    notificationHubState.value = state.notificationHub;
    ecChangeHubState.value = state.ecChangeHub;
  }

  /**
   * 刷新在线用户
   */
  async function refreshOnlineUsersAsync(): Promise<void> {
    onlineUsers.value = await taktSignalRManager.getOnlineUsersAsync();
  }

  /**
   * 刷新未读消息数
   */
  async function refreshUnreadCountAsync(): Promise<void> {
    unreadCount.value = await taktSignalRManager.getUnreadCountAsync();
  }

  /**
   * 应用在线统计（SignalR 推送或 HTTP 拉取）
   * @param statistics 统计 DTO
   */
  function applyOnlineStatistics(statistics: OnlineStatistics): void {
    onlineStatistics.value = statistics;
  }

  /**
   * 应用消息统计（SignalR 推送或 HTTP 拉取）
   * @param statistics 统计 DTO
   */
  function applyMessageStatistics(statistics: MessageStatistics): void {
    messageStatistics.value = statistics;
    unreadCount.value = statistics.unreadCount;
  }

  /**
   * 通过 HTTP 拉取在线统计（Hub 未连接时的兜底）
   */
  async function refreshOnlineStatisticsAsync(): Promise<void> {
    applyOnlineStatistics(await getOnlineStatistics());
  }

  /**
   * 通过 HTTP 拉取消息统计（Hub 未连接时的兜底）
   */
  async function refreshMessageStatisticsAsync(): Promise<void> {
    applyMessageStatistics(await getMessageStatistics());
  }

  /**
   * 处理强退事件
   */
  function handleForceLogout(event: ForceLogoutEvent): void {
    useForceLogoutScheduleStore().clearSchedule();
    const message = event.message || translateLocaleMessage('common.tip.force.logout');
    void executeForceLogoutAsync(message);
  }

  /**
   * 处理延迟强退预告（倒计时，不立即退出）
   * @param event 延迟强退事件
   */
  function handleForceLogoutScheduled(event: ForceLogoutScheduledEvent): void {
    useForceLogoutScheduleStore().startScheduledLogout(event);
  }

  /**
   * 连接 SignalR
   */
  async function connectSignalRAsync(): Promise<void> {
    if (connecting.value) {
      return;
    }

    const actualState = taktSignalRManager.getConnectionState();
    const actuallyConnected =
      actualState.connectHub === signalR.HubConnectionState.Connected
      && actualState.notificationHub === signalR.HubConnectionState.Connected
      && actualState.ecChangeHub === signalR.HubConnectionState.Connected;

    if (actuallyConnected) {
      syncConnectionState();
      void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('同步落库未读至通知中心失败', { action: 'hydratePersistedUnread' }, error);
      });
      return;
    }

    if (isConnected.value && !actuallyConnected) {
      signalrStoreLogger.warn('SignalR 缓存状态与 Hub 实际状态不一致，强制重连', {
        action: 'connect',
        cached: { connectHub: connectHubState.value, notificationHub: notificationHubState.value },
        actual: actualState,
      });
      await disconnectSignalRAsync();
    }

    const tenantStore = useTenantStore();
    if (!tenantStore.tenantCode.trim() || !tenantStore.companyCode.trim()) {
      signalrStoreLogger.warn('SignalR 跳过连接：租户或公司上下文未就绪', {
        action: 'connect',
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
      });
      return;
    }

    connecting.value = true;

    try {
      await taktSignalRManager.connectSignalRHubsAsync({
        onConnectionStateChange: (state) => {
          connectHubState.value = state.connectHub;
          notificationHubState.value = state.notificationHub;
          ecChangeHubState.value = state.ecChangeHub;
          if (
            state.connectHub === signalR.HubConnectionState.Disconnected
            || state.notificationHub === signalR.HubConnectionState.Disconnected
            || state.ecChangeHub === signalR.HubConnectionState.Disconnected
          ) {
            signalrStoreLogger.warn('SignalR Hub 已断开，等待自动重连或手动刷新', {
              action: 'connectionStateChange',
              state,
            });
            return;
          }
          if (
            state.connectHub === signalR.HubConnectionState.Connected
            && state.notificationHub === signalR.HubConnectionState.Connected
            && state.ecChangeHub === signalR.HubConnectionState.Connected
          ) {
            void refreshWorkflowTodoCountAfterHubConnectedAsync();
          }
        },
        onReceiveMessage: (msg) => {
          messages.value.push(msg);
          const sender = msg.fromUserName?.trim() || '?';
          const body = msg.messageContent?.trim() || '';
          if (!body && !msg.messageTitle?.trim()) {
            signalrStoreLogger.warn('收到空内容私信', { action: 'receiveMessage', msg });
          }
          showPrivateMessageNotify({
            sender,
            senderNickName: msg.fromUserNickName,
            content: body,
            messageId: msg.messageId,
            sendTime: msg.sendTime,
          });
          EventBus.emit('foundation:message:received', msg);
          signalrStoreLogger.info('私信已送达客户端', {
            action: 'receiveMessage',
            fromUserName: msg.fromUserName,
            messageId: msg.messageId,
          });
        },
        onReceiveBroadcast: (msg) => {
          broadcastMessages.value.push(msg);
          notify({
            type: 'info',
            message: msg.messageTitle?.trim() || translateLocaleMessage('common.page.signalr.new.message'),
            description: msg.messageContent,
            duration: 5,
          });
        },
        onMessageRead: (event) => {
          const target = messages.value.find(
            (item) => String((item as SignalRMessageWithId).messageId) === String(event.messageId)
          );

          if (target) {
            target.readStatus = TaktReadStatus.Read;
          }
          useHeaderNotificationStore().markPersistedReadByMessageId(String(event.messageId));
        },
        onOnlineStatisticsUpdated: applyOnlineStatistics,
        onMessageStatisticsUpdated: applyMessageStatistics,
        onFlowSchemeChanged: (event) => {
          dispatchWorkflowSignalREvents(event);
        },
        onFlowInstanceProgressed: (event) => {
          dispatchWorkflowSignalREvents(undefined, event);
        },
        onFlowTodoCountUpdated: (event) => {
          dispatchWorkflowSignalREvents(undefined, undefined, event);
        },
        onQuartzTaskChanged: (event) => {
          dispatchQuartzSignalREvents(event);
        },
        onQuartzTaskExecuted: (event) => {
          dispatchQuartzSignalREvents(undefined, event);
          const isSuccess = event.executeStatus === 1;
          notify({
            type: isSuccess ? 'success' : 'error',
            message: translateLocaleMessage(
              isSuccess
                ? 'foundation.quartz-task.page.signalr.executeSucceeded'
                : 'foundation.quartz-task.page.signalr.executeFailed',
              {
                code: event.taskCode || event.taskName,
                duration: String(event.executeDuration ?? 0),
              },
            ),
            description: isSuccess
              ? (event.executeMessage || undefined)
              : (event.errorInfo || event.executeMessage || undefined),
            duration: 10,
          });
        },
        onBomMaterialCostItemRecalculateCompleted: (event) => {
          dispatchBomMaterialCostItemRecalculateSignalREvents(event);
        },
        onEcGijutsuPersistCompleted: (event) => {
          dispatchEcGijutsuPersistSignalREvents(event);
          const isSuccess = event.executeStatus === 1;
          notify({
            type: isSuccess ? 'success' : 'error',
            message: translateLocaleMessage(
              isSuccess
                ? 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.completed'
                : 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.failed',
              {
                ecCode: event.ecCode,
                action: translateLocaleMessage(
                  event.isUpdate
                    ? 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.actionUpdate'
                    : 'logistics.manufacturing.engineering-change.ec-gijutsu.page.persist.actionCreate',
                ),
                detailCount: String(event.detailCount ?? 0),
                duration: formatEcGijutsuPersistDuration(event.executeDuration ?? 0),
              },
            ),
            description: isSuccess ? undefined : (event.errorMessage || undefined),
            duration: 10,
          });
        },
        onEcChangeNotification: (event) => {
          dispatchEcChangeSignalREvents(event);
          notify({
            type: 'info',
            message: translateLocaleMessage('logistics.manufacturing.engineering-change.ec-notification.page.signalr.changeNotification'),
            description: `${event.ecCode} · ${event.deptCode}`,
            duration: 8,
          });
        },
        onEcExecutionTaskAssigned: (event) => {
          dispatchEcChangeSignalREvents(undefined, event);
          notify({
            type: 'info',
            message: translateLocaleMessage('logistics.manufacturing.engineering-change.ec-notification.page.signalr.taskAssigned'),
            description: `${event.ecCode} · ${event.taskTitle}`,
            duration: 8,
          });
        },
        onEcExecutionTaskProgress: (event) => {
          dispatchEcChangeSignalREvents(undefined, undefined, event);
        },
        onEcChangeClosed: (event) => {
          dispatchEcChangeSignalREvents(undefined, undefined, undefined, event);
          notify({
            type: 'success',
            message: translateLocaleMessage('logistics.manufacturing.engineering-change.ec-notification.page.signalr.changeClosed'),
            description: event.ecCode,
            duration: 8,
          });
        },
        onEcExecutionTaskAlert: (event) => {
          dispatchEcChangeSignalREvents(undefined, undefined, undefined, undefined, event);
          notify({
            type: 'warning',
            message: translateLocaleMessage('logistics.manufacturing.engineering-change.ec-notification.page.signalr.taskAlert'),
            description: event.message,
            duration: 10,
          });
        },
        onEcNotificationConfirmed: (event) => {
          dispatchEcChangeSignalREvents(undefined, undefined, undefined, undefined, undefined, event);
          notify({
            type: 'success',
            message: translateLocaleMessage('logistics.manufacturing.engineering-change.ec-notification.page.signalr.notificationConfirmed'),
            description: `${event.ecCode ?? ''} · ${event.ecDeptGroupCode}`.trim(),
            duration: 6,
          });
        },
        onOnlineMessage: (event) => {
          notify({
            type: 'success',
            message: translateLocaleMessage('common.feedback.connect.success'),
            description: String(event.message ?? ''),
            duration: 5,
            center: {
              kind: 'online',
              autoMarkReadAfterMs: HEADER_ONLINE_AUTO_READ_MS,
            },
          });
        },
        onUserConnected: () => {
          void refreshOnlineUsersAsync().catch(() => undefined);
        },
        onUserDisconnected: () => {
          void refreshOnlineUsersAsync().catch(() => undefined);
        },
        onForceLogout: handleForceLogout,
        onForceLogoutScheduled: handleForceLogoutScheduled,
        onError: (error) => {
          EventBus.emit('notification:show', {
            type: 'error',
            message: error.message || translateLocaleMessage('common.feedback.signalr.error'),
          });
        },
      });

      syncConnectionState();

      void refreshOnlineUsersAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取在线用户失败', { action: 'refreshOnlineUsers' }, error);
      });

      void refreshUnreadCountAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取未读消息数失败', { action: 'refreshUnreadCount' }, error);
      });

      void refreshOnlineStatisticsAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取在线统计失败', { action: 'refreshOnlineStatistics' }, error);
      });

      void refreshMessageStatisticsAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('获取消息统计失败', { action: 'refreshMessageStatistics' }, error);
      });

      void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch((error: unknown) => {
        signalrStoreLogger.warn('同步落库未读至通知中心失败', { action: 'hydratePersistedUnread' }, error);
      });
    } catch (error) {
      EventBus.emit('notification:show', {
        type: 'error',
        message: error instanceof Error ? error.message : 'SignalR 连接失败',
      });

      throw error;
    } finally {
      connecting.value = false;
    }
  }

  /**
   * 公司切换后重连 SignalR（携带新的 X-Company-Code / company_code）
   */
  async function reconnectSignalRAsync(): Promise<void> {
    await disconnectSignalRAsync();
    await connectSignalRAsync();
  }

  /**
   * 断开 SignalR
   */
  async function disconnectSignalRAsync(): Promise<void> {
    await taktSignalRManager.disconnectSignalRHubsAsync();
    syncConnectionState();
    onlineUsers.value = [];
    messages.value = [];
    broadcastMessages.value = [];
    unreadCount.value = 0;
    onlineStatistics.value = null;
    messageStatistics.value = null;
    useWorkflowTodoCountStore().resetTodoCount();
  }

  /**
   * 发送私信
   */
  async function sendSignalRMessageAsync(
    toUserName: string,
    messageContent: string,
    messageTitle?: string
  ): Promise<void> {
    await taktSignalRManager.sendMessageAsync(toUserName, messageContent, messageTitle);
  }

  /**
   * 发送广播
   */
  async function broadcastSignalRMessageAsync(messageContent: string, messageTitle?: string): Promise<void> {
    await taktSignalRManager.broadcastMessageAsync(messageContent, messageTitle);
  }

  /**
   * 标记消息已读
   * @param messageId 消息 ID
   */
  async function markSignalRMessageAsReadAsync(messageId: number): Promise<void> {
    await taktSignalRManager.markMessageAsReadAsync(messageId);
  }

  /**
   * 重置 SignalR 状态
   */
  function resetSignalRState(): void {
    connectHubState.value = signalR.HubConnectionState.Disconnected;
    notificationHubState.value = signalR.HubConnectionState.Disconnected;
    ecChangeHubState.value = signalR.HubConnectionState.Disconnected;
    onlineUsers.value = [];
    unreadCount.value = 0;
    onlineStatistics.value = null;
    messageStatistics.value = null;
    messages.value = [];
    broadcastMessages.value = [];
    connecting.value = false;
    useWorkflowTodoCountStore().resetTodoCount();
  }

  return {
    connectHubState,
    notificationHubState,
    ecChangeHubState,
    onlineUsers,
    unreadCount,
    onlineStatistics,
    messageStatistics,
    messages,
    broadcastMessages,
    connecting,
    isConnected,
    connectSignalRAsync,
    reconnectSignalRAsync,
    disconnectSignalRAsync,
    refreshOnlineUsersAsync,
    refreshUnreadCountAsync,
    refreshOnlineStatisticsAsync,
    refreshMessageStatisticsAsync,
    sendSignalRMessageAsync,
    broadcastSignalRMessageAsync,
    markSignalRMessageAsReadAsync,
    resetSignalRState,
    syncConnectionState,
  };
});
