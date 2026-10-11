// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useEcChangeSignalrRefresh.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅工程变更 SignalR EventBus 事件，刷新通知单列表页
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import { useEventBus } from '@/utils/eventBus';

/** 工程变更通知单列表 tableName 常量 */
export const EC_NOTIFICATION_TABLE_NAME = 'ec-notification';

/**
 * 订阅工程变更 SignalR 触发的列表刷新
 * @param reload 刷新回调
 * @param tableName 列表标识（与 EventBus table:refresh 对齐）
 */
export function useEcChangeSignalRRefresh(
  reload: () => void | Promise<void>,
  tableName: string = EC_NOTIFICATION_TABLE_NAME,
): void {
  const { on, off } = useEventBus();

  /**
   * 处理 table:refresh
   * @param payload 事件载荷
   */
  const handleTableRefresh = (payload?: { tableName?: string }): void => {
    if (payload?.tableName && payload.tableName !== tableName) {
      return;
    }
    void reload();
  };

  /**
   * 变更闭环：刷新通知单列表
   */
  const handleChangeClosed = (): void => {
    void reload();
  };

  /**
   * 通知确认：刷新通知单列表
   */
  const handleNotificationConfirmed = (): void => {
    void reload();
  };

  onMounted(() => {
    on('table:refresh', handleTableRefresh);
    on('logistics:ec-change:closed', handleChangeClosed);
    on('logistics:ec-change:notification-confirmed', handleNotificationConfirmed);
  });

  onUnmounted(() => {
    off('table:refresh', handleTableRefresh);
    off('logistics:ec-change:closed', handleChangeClosed);
    off('logistics:ec-change:notification-confirmed', handleNotificationConfirmed);
  });
}
