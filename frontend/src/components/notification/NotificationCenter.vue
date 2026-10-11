<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/notification -->
<!-- 文件名称：NotificationCenter.vue -->
<!-- 功能描述：通知中心（订阅 EventBus notification:show，累积历史；Toast 由 taktEventHandlers 统一弹出） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="notification-center">
    <a-badge :count="unreadCount" :overflow-count="99">
      <a-button type="text" @click="toggleVisible">
        <template #icon>
          <RiNotificationLine class="takt-remix-icon" />
        </template>
      </a-button>
    </a-badge>

    <a-drawer
      v-model:open="visible"
      :title="t('components.navigation.page.systemsetting.notification')"
      placement="right"
      :width="400"
    >
      <template #extra>
        <a-space>
          <a-button type="text" @click="clearAll">
            {{ t('common.page.button.emptyall') }}
          </a-button>
          <a-button type="link" @click="markAllRead">
            {{ t('components.notification.page.markallread') }}
          </a-button>
        </a-space>
      </template>
      <a-list
        :data-source="notifications"
        item-layout="horizontal"
        :pagination="{ pageSize: getTaktDefaultPageSize(), size: 'small' }"
      >
        <template #renderItem="{ item }">
          <a-list-item>
            <a-list-item-meta
              :title="item.message"
              :description="item.description"
            >
              <template #avatar>
                <a-avatar :style="{ backgroundColor: getNotificationColor(item.type) }">
                  <template #icon>
                    <component :is="getNotificationIcon(item.type)" class="notification-type-icon takt-remix-icon" />
                  </template>
                </a-avatar>
              </template>
            </a-list-item-meta>
            <template #actions>
              <a-tag :color="getNotificationColor(item.type)">
                {{ item.type }}
              </a-tag>
            </template>
          </a-list-item>
        </template>
      </a-list>
    </a-drawer>
  </div>
</template>

<script setup lang="ts">
/**
 * 通知中心（EventBus notification:show + 统一日志）
 * @description 图标使用 @remixicon/vue；勿使用已废弃的 eventBus / NotificationEvents
 */
import {
  RiCheckboxCircleLine,
  RiCloseCircleLine,
  RiErrorWarningLine,
  RiInformationLine,
  RiNotificationLine,
} from '@remixicon/vue';
import type { Component } from 'vue';
import { useI18n } from 'vue-i18n';
import type { Events, NotificationType } from '@/types/event.types';
import { useEventBus } from '@/utils/eventBus';
import { createLogger } from '@/utils/logger';
import { getTaktDefaultPageSize } from '@/utils/taktPaged';

/** 通知中心模块日志 */
const notificationLogger = createLogger('notification-center');

/** i18n */
const { t } = useI18n();

/**
 * 通知中心列表项
 */
interface NotificationItem {
  /** 唯一 ID */
  id: string;
  /** 通知类型 */
  type: NotificationType;
  /** 主文案（对应 EventBus message） */
  message: string;
  /** 副文案（对应 EventBus description） */
  description?: string;
  /** 接收时间戳 */
  timestamp: number;
  /** 是否已读 */
  read: boolean;
}

/** 通知历史最大条数（07-overflow-vue） */
const MAX_NOTIFICATIONS = 100

/** 抽屉是否打开 */
const visible = ref(false);

/** 通知历史列表 */
const notifications = ref<NotificationItem[]>([]);

/** 未读数量 */
const unreadCount = ref(0);

const { on, off } = useEventBus();

/**
 * 处理全局 notification:show 事件（与 emitNotification / EventBus.emit 载荷一致）
 * @param {Events['notification:show']} payload 通知载荷
 */
function handleNotificationShow(payload: Events['notification:show']): void {
  const item: NotificationItem = {
    id: `notif_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`,
    type: payload.type,
    message: payload.message,
    description: payload.description,
    timestamp: Date.now(),
    read: false,
  };

  notifications.value.unshift(item)
  if (notifications.value.length > MAX_NOTIFICATIONS) {
    notifications.value.length = MAX_NOTIFICATIONS
  }
  unreadCount.value += 1

  notificationLogger.info('通知已入列', {
    action: 'enqueue',
    type: item.type,
    message: item.message,
  });
}

/**
 * 切换抽屉显示
 */
function toggleVisible(): void {
  visible.value = !visible.value;
}

/**
 * 清空全部通知
 */
function clearAll(): void {
  notifications.value = [];
  unreadCount.value = 0;
  notificationLogger.debug('通知列表已清空', { action: 'clear-all' });
}

/**
 * 全部标记已读
 */
function markAllRead(): void {
  notifications.value.forEach((item) => {
    item.read = true;
  });
  unreadCount.value = 0;
}

/**
 * 获取通知头像背景色
 * @param {NotificationType} type 通知类型
 * @returns {string} 颜色值
 */
function getNotificationColor(type: NotificationType): string {
  const colors: Record<NotificationType, string> = {
    info: '#1890ff',
    success: '#52c41a',
    warning: '#faad14',
    error: '#ff4d4f',
  };
  return colors[type];
}

/**
 * 获取通知图标组件
 * @param {NotificationType} type 通知类型
 * @returns {Component} 图标组件
 */
function getNotificationIcon(type: NotificationType): Component {
  const icons: Record<NotificationType, Component> = {
    info: RiInformationLine,
    success: RiCheckboxCircleLine,
    warning: RiErrorWarningLine,
    error: RiCloseCircleLine,
  };
  return icons[type];
}

onMounted(() => {
  on('notification:show', handleNotificationShow);
});

onUnmounted(() => {
  off('notification:show', handleNotificationShow);
});
</script>

<style scoped>
.notification-center {
  display: inline-block;
}

.notification-type-icon {
  color: #fff;
}
</style>
