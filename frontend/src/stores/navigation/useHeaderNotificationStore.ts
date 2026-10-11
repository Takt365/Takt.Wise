// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/navigation
// 文件名称：useHeaderNotificationStore.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：顶栏通知中心列表（TaktHeaderNotification 数据源；落库未读常驻、上线通知 5s 自动已读；清空全部含落库未读并同步标已读）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { computed, ref } from 'vue';
import { getMessageUnreadList, markMessageReadById } from '@/api/foundation/message.api';
import type { Message } from '@/types/foundation/message.types';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { formatPrivateMessageNotificationContent } from '@/utils/taktMessageDisplay';
import { createLogger } from '@/utils/logger';

const headerNotificationLogger = createLogger('header-notification');

/** 顶栏通知列表最大条数（07-overflow-vue；落库未读项不参与裁剪） */
const MAX_HEADER_NOTIFICATIONS = 100;

/** 上线通知在通知中心自动标为已读的毫秒数 */
export const HEADER_ONLINE_AUTO_READ_MS = 5000;

/** 拉取落库未读消息分页大小 */
const PERSISTED_UNREAD_PAGE_SIZE = 100;

/**
 * 顶栏通知类别
 */
export type TaktHeaderNotificationKind = 'online' | 'persisted' | 'system';

/**
 * 顶栏通知项（与 TaktHeaderNotification 列表结构对齐）
 */
export interface TaktHeaderNotificationItem {
  /** 唯一 ID */
  id: string;
  /** 通知类别 */
  kind: TaktHeaderNotificationKind;
  /** 标题（Notification 主文案） */
  title: string;
  /** 正文（Notification 副文案） */
  content: string;
  /** 展示时间 */
  time: string;
  /** 是否已读 */
  read: boolean;
  /** 落库消息 ID（persisted 时用于去重与已读同步） */
  messageId?: string;
}

/**
 * 入列通知参数
 */
export interface EnqueueHeaderNotificationPayload {
  title: string;
  content: string;
  kind?: TaktHeaderNotificationKind;
  messageId?: string;
  time?: string;
  /** 指定毫秒后自动标为已读（上线通知默认 5s） */
  autoMarkReadAfterMs?: number;
}

/** 自动已读定时器（登出时清理） */
const autoReadTimerById = new Map<string, ReturnType<typeof setTimeout>>();

/**
 * 顶栏通知中心状态
 */
export const useHeaderNotificationStore = defineStore('headerNotification', () => {
  const items = ref<TaktHeaderNotificationItem[]>([]);

  /** 未读数量 */
  const unreadCount = computed(() => items.value.filter((item) => !item.read).length);

  /**
   * 清除单条自动已读定时器
   * @param id 通知 ID
   */
  function clearAutoReadTimer(id: string): void {
    const timer = autoReadTimerById.get(id);
    if (timer != null) {
      clearTimeout(timer);
      autoReadTimerById.delete(id);
    }
  }

  /**
   * 裁剪列表：保留全部未读落库消息，其余按上限淘汰已读/非落库项
   */
  function trimNotificationItems(): void {
    const unreadPersisted = items.value.filter((item) => item.kind === 'persisted' && !item.read);
    const others = items.value.filter((item) => item.kind !== 'persisted' || item.read);
    const maxOthers = Math.max(0, MAX_HEADER_NOTIFICATIONS - unreadPersisted.length);
    if (others.length > maxOthers) {
      others.length = maxOthers;
    }
    items.value = [...unreadPersisted, ...others];
  }

  /**
   * 调度自动已读
   * @param id 通知 ID
   * @param delayMs 延迟毫秒
   */
  function scheduleAutoMarkRead(id: string, delayMs: number): void {
    clearAutoReadTimer(id);
    autoReadTimerById.set(id, setTimeout(() => {
      autoReadTimerById.delete(id);
      markNotificationRead(id);
    }, delayMs));
  }

  /**
   * 格式化落库消息展示文案
   * @param message 在线消息 DTO
   */
  function formatPersistedMessageDisplay(message: Message): { title: string; content: string } {
    return {
      title: translateLocaleMessage('common.page.signalr.new.message'),
      content: formatPrivateMessageNotificationContent(
        message.fromUserNickName,
        message.fromUserName,
        message.messageContent,
      ),
    };
  }

  /**
   * 入列一条通知
   * @param payload 标题、正文与类别
   * @returns 新通知 ID；落库重复 messageId 时返回已有 ID
   */
  function enqueueNotification(payload: EnqueueHeaderNotificationPayload): string {
    const kind = payload.kind ?? 'system';
    const messageId = payload.messageId?.trim();
    if (kind === 'persisted' && messageId) {
      const existing = items.value.find((item) => item.messageId === messageId);
      if (existing) {
        return existing.id;
      }
    }

    const id = messageId ? `hdr_msg_${messageId}` : `hdr_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`;
    const autoMarkReadAfterMs = payload.autoMarkReadAfterMs
      ?? (kind === 'online' ? HEADER_ONLINE_AUTO_READ_MS : undefined);

    items.value.unshift({
      id,
      kind,
      title: payload.title,
      content: payload.content,
      time: payload.time ?? new Date().toLocaleString(),
      read: false,
      messageId: messageId || undefined,
    });

    trimNotificationItems();

    if (autoMarkReadAfterMs != null && autoMarkReadAfterMs > 0) {
      scheduleAutoMarkRead(id, autoMarkReadAfterMs);
    }

    return id;
  }

  /**
   * 从 API 拉取当前用户落库未读并合并入通知中心（登录/刷新后调用，不依赖 SignalR）
   */
  async function hydratePersistedUnreadAsync(): Promise<void> {
    try {
      const allRows: Message[] = [];
      let pageIndex = 1;
      let total = 0;

      do {
        const result = await getMessageUnreadList({
          pageIndex,
          pageSize: PERSISTED_UNREAD_PAGE_SIZE,
        });
        const rows = result.data ?? [];
        total = result.total ?? rows.length;
        allRows.push(...rows);
        pageIndex += 1;
      } while (allRows.length < total && pageIndex <= Math.ceil(total / PERSISTED_UNREAD_PAGE_SIZE));

      for (let i = allRows.length - 1; i >= 0; i -= 1) {
        const message = allRows[i];
        const messageId = String(message.messageId ?? '').trim();
        if (!messageId) {
          continue;
        }
        const display = formatPersistedMessageDisplay(message);
        enqueueNotification({
          kind: 'persisted',
          messageId,
          title: display.title,
          content: display.content,
          time: message.sendTime || new Date().toLocaleString(),
        });
      }

      headerNotificationLogger.info('落库未读已同步至通知中心', {
        action: 'hydratePersistedUnread',
        count: allRows.length,
        total,
      });
    } catch (error: unknown) {
      headerNotificationLogger.warn('拉取落库未读失败', { action: 'hydratePersistedUnread' }, error);
    }
  }

  /**
   * 标记单条已读（落库消息同步调用后端 read API）
   * @param id 通知 ID
   */
  async function markNotificationReadAsync(id: string): Promise<void> {
    const target = items.value.find((item) => item.id === id);
    if (!target || target.read) {
      return;
    }

    clearAutoReadTimer(id);
    target.read = true;

    const messageId = target.messageId?.trim();
    if (target.kind === 'persisted' && messageId) {
      try {
        await markMessageReadById(messageId);
      } catch (error: unknown) {
        target.read = false;
        headerNotificationLogger.warn('落库消息标已读失败', { action: 'markRead', messageId }, error);
        throw error;
      }
    }
  }

  /**
   * 标记单条已读（同步入口，落库走 fire-and-forget API）
   * @param id 通知 ID
   */
  function markNotificationRead(id: string): void {
    const target = items.value.find((item) => item.id === id);
    if (!target || target.read) {
      return;
    }

    clearAutoReadTimer(id);
    target.read = true;

    const messageId = target.messageId?.trim();
    if (target.kind === 'persisted' && messageId) {
      void markMessageReadById(messageId).catch((error: unknown) => {
        target.read = false;
        headerNotificationLogger.warn('落库消息标已读失败', { action: 'markRead', messageId }, error);
      });
    }
  }

  /**
   * 按落库 messageId 标为已读（SignalR MessageRead 等场景）
   * @param messageId 消息 ID
   */
  function markPersistedReadByMessageId(messageId: string): void {
    const normalized = messageId.trim();
    if (!normalized) {
      return;
    }
    const target = items.value.find((item) => item.messageId === normalized);
    if (target) {
      clearAutoReadTimer(target.id);
      target.read = true;
    }
  }

  /**
   * 删除单条通知（未读落库项仅移出本地列表，刷新后仍可从 API 拉回）
   * @param id 通知 ID
   */
  function removeNotification(id: string): void {
    clearAutoReadTimer(id);
    items.value = items.value.filter((item) => item.id !== id);
  }

  /**
   * 清空通知中心全部条目；未读落库消息同步标为已读，避免刷新后再次 hydrate 回来
   */
  function clearAllNotifications(): void {
    const unreadPersisted = items.value.filter((item) => item.kind === 'persisted' && !item.read);
    for (const item of items.value) {
      clearAutoReadTimer(item.id);
    }
    items.value = [];
    for (const item of unreadPersisted) {
      const messageId = item.messageId?.trim();
      if (messageId) {
        void markMessageReadById(messageId).catch((error: unknown) => {
          headerNotificationLogger.warn('清空时标已读失败', { action: 'clearAll', messageId }, error);
        });
      }
    }
  }

  /** 全部标记已读 */
  function markAllNotificationsRead(): void {
    const unreadPersisted = items.value.filter((item) => item.kind === 'persisted' && !item.read);
    for (const item of items.value) {
      clearAutoReadTimer(item.id);
      item.read = true;
    }
    for (const item of unreadPersisted) {
      const messageId = item.messageId?.trim();
      if (messageId) {
        void markMessageReadById(messageId).catch((error: unknown) => {
          item.read = false;
          headerNotificationLogger.warn('批量标已读失败', { action: 'markAllRead', messageId }, error);
        });
      }
    }
  }

  /** 登出时重置 */
  function resetHeaderNotifications(): void {
    for (const id of autoReadTimerById.keys()) {
      clearAutoReadTimer(id);
    }
    items.value = [];
  }

  return {
    items,
    unreadCount,
    enqueueNotification,
    hydratePersistedUnreadAsync,
    markNotificationRead,
    markNotificationReadAsync,
    markPersistedReadByMessageId,
    removeNotification,
    clearAllNotifications,
    markAllNotificationsRead,
    resetHeaderNotifications,
  };
});
