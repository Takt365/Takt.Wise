// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useEcTePersistSignalr.ts
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅设变技术课主表后台保存 SignalR 完成事件
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import type { EcTePersistCompletedEvent } from '@/types/logistics/manufacturing/engineering-change/ecTePersistSignalR.types';
import { useEventBus } from '@/utils/eventBus';

/** 设变技术课列表 tableName（table:refresh） */
export const EC_TE_TABLE_NAME = 'logistics-manufacturing-engineering-change-ec-te';

/**
 * 格式化执行耗时（毫秒 → 可读文案）
 * @param {number} ms 毫秒
 * @returns {string} 可读耗时
 */
export function formatEcTePersistDuration(ms: number): string {
  if (!Number.isFinite(ms) || ms < 0) {
    return '0ms';
  }
  if (ms < 1000) {
    return `${Math.round(ms)}ms`;
  }
  const totalSeconds = Math.round(ms / 1000);
  if (totalSeconds < 60) {
    return `${totalSeconds}s`;
  }
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;
  return seconds > 0 ? `${minutes}m ${seconds}s` : `${minutes}m`;
}

/**
 * 订阅设变技术课主表后台保存完成事件
 * @param {(event: EcTePersistCompletedEvent) => void | Promise<void>} onCompleted 完成回调
 * @returns {void}
 */
export function useEcTePersistSignalR(
  onCompleted: (event: EcTePersistCompletedEvent) => void | Promise<void>,
): void {
  const { on, off } = useEventBus();

  const handlePersistCompleted = (event: EcTePersistCompletedEvent): void => {
    void onCompleted(event);
  };

  onMounted(() => {
    on('logistics:ec-te:persist-completed', handlePersistCompleted);
  });

  onUnmounted(() => {
    off('logistics:ec-te:persist-completed', handlePersistCompleted);
  });
}
