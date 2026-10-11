// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：notification.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Ant Design Vue Notification 封装（同步 EventBus 通知中心，避免与 Message 重复弹出）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { notification } from 'ant-design-vue';
import type { NotificationPlacement } from 'ant-design-vue';
import type { VNode } from 'vue';
import { translateLocaleMessage as translate } from '@/utils/taktI18nMessage';
import { formatPrivateMessageNotificationContent } from '@/utils/taktMessageDisplay';
import type { NotificationType } from '@/types/event.types';
import {
  HEADER_ONLINE_AUTO_READ_MS,
  useHeaderNotificationStore,
  type TaktHeaderNotificationKind,
} from '@/stores/navigation/useHeaderNotificationStore';

/** 与 EventBus / 通知中心一致的类型 */
export type NotifyType = NotificationType;

export interface NotifyOptions {
  /**
   * 主文案
   */
  message: string;

  /**
   * 副文案
   */
  description?: string | VNode;

  /**
   * 通知类型
   */
  type?: NotifyType;

  /**
   * 自动关闭秒数；null 表示不自动关闭
   */
  duration?: number | null;

  /**
   * 弹出位置
   */
  placement?: NotificationPlacement;

  /**
   * 唯一 key（用于关闭或更新同一条通知）
   */
  key?: string;

  /**
   * 关闭回调
   */
  onClose?: () => void;

  /**
   * 是否同步写入通知中心（默认 true）
   */
  syncToCenter?: boolean;

  /**
   * 通知中心入列选项（类别、落库 ID、自动已读等）
   */
  center?: {
    kind?: TaktHeaderNotificationKind;
    messageId?: string;
    time?: string;
    autoMarkReadAfterMs?: number;
  };
}

/** API 连接失败通知固定 key */
export const API_CONNECT_FAIL_KEY = 'api-connect-fail';

const DEFAULT_PLACEMENT: NotificationPlacement = 'topRight';
const DEFAULT_DURATION = 4.5;

notification.config({ placement: DEFAULT_PLACEMENT });

/**
 * 弹出 Ant Design Notification，并可选同步通知中心
 * @param {NotifyOptions} options 通知选项
 */
export function notify(options: NotifyOptions): void {
  const {
    type = 'info',
    message,
    description,
    duration = DEFAULT_DURATION,
    placement = DEFAULT_PLACEMENT,
    key,
    onClose,
    syncToCenter = true,
    center,
  } = options;

  if (syncToCenter) {
    useHeaderNotificationStore().enqueueNotification({
      title: message,
      content: typeof description === 'string' ? description : '',
      kind: center?.kind,
      messageId: center?.messageId,
      time: center?.time,
      autoMarkReadAfterMs: center?.autoMarkReadAfterMs,
    });
  }

  notification[type]({
    message,
    ...(description !== undefined ? { description } : {}),
    ...(duration !== undefined ? { duration } : {}),
    placement,
    ...(key !== undefined ? { key } : {}),
    ...(onClose !== undefined ? { onClose } : {}),
  });
}

/**
 * 关闭 API 连接失败通知
 */
export function closeApiConnectFailNotification(): void {
  notification.close(API_CONNECT_FAIL_KEY);
}

/**
 * 显示 API 连接失败通知（固定 key，可重复调用前先 close）
 * @param {Partial<NotifyOptions>} [options] 覆盖选项
 */
export function showApiConnectFail(options?: Partial<NotifyOptions>): void {
  notify({
    type: 'error',
    message: translate('common.page.api.connect.fail'),
    description: translate('common.page.api.connect.description'),
    duration: DEFAULT_DURATION,
    placement: DEFAULT_PLACEMENT,
    key: API_CONNECT_FAIL_KEY,
    ...options,
  });
}

/**
 * 显示 API 错误通知；message 含 \\n 时首行作主文案，其余作 description
 * @param {string} message 错误信息
 * @param {string} [description] 副文案
 */
export function showApiError(message: string, description?: string): void {
  const lines = message.split('\n');
  const mainMessage = lines[0] ?? message;
  const detail = description ?? (lines.length > 1 ? lines.slice(1).join('\n') : undefined);

  notify({
    type: 'error',
    message: mainMessage,
    description: detail,
    duration: DEFAULT_DURATION,
    placement: DEFAULT_PLACEMENT,
  });
}

/**
 * 显示 SignalR 连接失败通知
 * @param {Partial<NotifyOptions>} [options] 覆盖选项
 */
export function showSignalrConnectFail(options?: Partial<NotifyOptions>): void {
  notify({
    type: 'error',
    message: translate('common.page.signalr.connect.fail'),
    placement: DEFAULT_PLACEMENT,
    ...options,
  });
}

/**
 * 显示 SignalR 恢复在线通知
 * @param {Partial<NotifyOptions> & { description: string }} options 须含 description
 */
export function showOnlineNotify(options: Partial<NotifyOptions> & { description: string }): void {
  const { center: centerOverride, ...rest } = options;
  notify({
    type: 'success',
    message: translate('common.page.signalr.online.notify'),
    placement: DEFAULT_PLACEMENT,
    duration: 5,
    ...rest,
    center: {
      kind: 'online',
      autoMarkReadAfterMs: HEADER_ONLINE_AUTO_READ_MS,
      ...centerOverride,
    },
  });
}

/**
 * 显示新消息通知（站内信 / 推送等）
 * @param {Omit<NotifyOptions, 'type'> & { message: string; description: string }} options 消息内容
 */
export function showNewMessage(
  options: Omit<NotifyOptions, 'type'> & { message: string; description: string },
): void {
  notify({
    type: 'info',
    placement: DEFAULT_PLACEMENT,
    duration: 5,
    ...options,
  });
}

/**
 * 显示站内私信通知（右上角 Notification + 顶栏通知中心入列）
 * @param options 发送者与正文
 */
export function showPrivateMessageNotify(options: {
  sender: string;
  senderNickName?: string;
  content: string;
  messageId?: string;
  sendTime?: string;
}): void {
  notify({
    type: 'info',
    message: translate('common.page.signalr.new.message'),
    description: formatPrivateMessageNotificationContent(
      options.senderNickName,
      options.sender,
      options.content,
    ),
    placement: DEFAULT_PLACEMENT,
    duration: 5,
    center: {
      kind: 'persisted',
      messageId: options.messageId,
      time: options.sendTime,
    },
  });
}
