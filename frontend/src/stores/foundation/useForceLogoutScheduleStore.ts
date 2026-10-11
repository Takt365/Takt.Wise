// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/foundation
// 文件名称：useForceLogoutScheduleStore.ts
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：延迟强退倒计时状态（SignalR ForceLogoutScheduled，不阻塞业务操作）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { Modal } from 'ant-design-vue';
import { executeForceLogoutAsync } from '@/bootstrap/taktLogoutFlow';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import type { ForceLogoutScheduledEvent } from '@/types/foundation/signalR.types';

const TICK_MS = 1000;

/**
 * 延迟强退倒计时状态
 */
export const useForceLogoutScheduleStore = defineStore('forceLogoutSchedule', () => {
  /** 是否展示倒计时条 */
  const active = ref(false);
  /** 预告文案 */
  const message = ref('');
  /** 计划强退时间戳（毫秒） */
  const kickAtMs = ref(0);
  /** 剩余秒数 */
  const remainingSeconds = ref(0);
  let tickTimer: ReturnType<typeof setInterval> | null = null;
  let logoutTriggered = false;

  /**
   * 格式化为 mm:ss
   */
  const formattedCountdown = computed(() => {
    const sec = Math.max(0, remainingSeconds.value);
    const minutes = Math.floor(sec / 60);
    const seconds = sec % 60;
    return `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
  });

  /**
   * 清除倒计时与定时器
   */
  function clearSchedule(): void {
    if (tickTimer != null) {
      clearInterval(tickTimer);
      tickTimer = null;
    }
    active.value = false;
    message.value = '';
    kickAtMs.value = 0;
    remainingSeconds.value = 0;
    logoutTriggered = false;
  }

  /**
   * 每秒刷新剩余时间，到期后执行强退
   */
  function tick(): void {
    if (!active.value) {
      return;
    }
    const left = Math.ceil((kickAtMs.value - Date.now()) / 1000);
    remainingSeconds.value = Math.max(0, left);
    if (remainingSeconds.value <= 0 && !logoutTriggered) {
      logoutTriggered = true;
      const logoutMessage = message.value || translateLocaleMessage('common.tip.force.logout');
      clearSchedule();
      void executeForceLogoutAsync(logoutMessage);
    }
  }

  /**
   * 启动延迟强退倒计时（非阻塞，用户可继续 CRUD）
   * @param event SignalR 延迟强退预告
   */
  function startScheduledLogout(event: ForceLogoutScheduledEvent): void {
    clearSchedule();
    const parsedKickAt = event.kickAt ? Date.parse(event.kickAt) : Number.NaN;
    const delayFromEvent = event.delaySeconds ?? 0;
    const resolvedKickAt = Number.isFinite(parsedKickAt) && parsedKickAt > Date.now()
      ? parsedKickAt
      : Date.now() + delayFromEvent * 1000;
    if (resolvedKickAt <= Date.now()) {
      return;
    }
    message.value = event.message?.trim()
      || translateLocaleMessage('common.tip.force.logout.scheduled');
    kickAtMs.value = resolvedKickAt;
    active.value = true;
    tick();
    tickTimer = setInterval(tick, TICK_MS);
    Modal.warning({
      title: translateLocaleMessage('common.page.button.kick'),
      content: message.value,
      okText: translateLocaleMessage('common.page.button.ok'),
      mask: false,
      centered: false,
      width: 420,
    });
  }

  return {
    active,
    message,
    remainingSeconds,
    formattedCountdown,
    startScheduledLogout,
    clearSchedule,
  };
});
