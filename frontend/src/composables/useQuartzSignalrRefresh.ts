// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useQuartzSignalrRefresh.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅 Quartz SignalR EventBus 事件，刷新定时任务列表页
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import type { QuartzTaskExecutedEvent } from '@/types/foundation/quartzSignalR.types';
import { useEventBus } from '@/utils/eventBus';

/** Quartz 列表 tableName 常量 */
export const QUARTZ_TABLE_NAME = 'foundation-quartz-task';

/**
 * 订阅 Quartz SignalR 触发的列表刷新
 * @param {() => void | Promise<void>} reload 刷新主表回调
 * @param {(event: QuartzTaskExecutedEvent) => void | Promise<void>} [onTaskExecuted] 任务执行完成回调（如刷新展开行日志）
 * @returns {void}
 */
export function useQuartzSignalRRefresh(
  reload: () => void | Promise<void>,
  onTaskExecuted?: (event: QuartzTaskExecutedEvent) => void | Promise<void>,
): void {
  const { on, off } = useEventBus();

  /**
   * 处理 table:refresh
   * @param payload 事件载荷
   */
  const handleTableRefresh = (payload?: { tableName?: string }): void => {
    if (payload?.tableName && payload.tableName !== QUARTZ_TABLE_NAME) {
      return;
    }
    void reload();
  };

  /**
   * 定时任务定义变更：刷新列表
   */
  const handleTaskChanged = (): void => {
    void reload();
  };

  /**
   * 定时任务执行完成：刷新列表并回调展开行
   * @param event 执行事件
   */
  const handleTaskExecuted = (event: QuartzTaskExecutedEvent): void => {
    void (async () => {
      await reload();
      if (onTaskExecuted) {
        await onTaskExecuted(event);
      }
    })();
  };

  onMounted(() => {
    on('table:refresh', handleTableRefresh);
    on('foundation:quartz-task:changed', handleTaskChanged);
    on('foundation:quartz-task:executed', handleTaskExecuted);
  });

  onUnmounted(() => {
    off('table:refresh', handleTableRefresh);
    off('foundation:quartz-task:changed', handleTaskChanged);
    off('foundation:quartz-task:executed', handleTaskExecuted);
  });
}
