// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useWorkflowSignalrRefresh.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅工作流 SignalR EventBus 事件，刷新对应列表页
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import { useEventBus } from '@/utils/eventBus';

/** 工作流列表 tableName 常量 */
export const WORKFLOW_TABLE_NAMES = {
  todo: 'workflow-todo',
  my: 'workflow-my',
  processed: 'workflow-processed',
  scheme: 'workflow-scheme',
  instance: 'workflow-instance',
} as const;

/**
 * 订阅工作流 SignalR 触发的列表刷新
 * @param {() => void | Promise<void>} reload 刷新回调
 * @param {string} tableName 列表标识（与 EventBus table:refresh 对齐）
 * @returns {void}
 */
export function useWorkflowSignalRRefresh(
  reload: () => void | Promise<void>,
  tableName: string,
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
   * 流程定义变更：刷新方案页
   */
  const handleSchemeChanged = (): void => {
    if (tableName === WORKFLOW_TABLE_NAMES.scheme) {
      void reload();
    }
  };

  /**
   * 实例推进：刷新待办/我的/已办/实例管理页
   * @param _payload 推进事件
   */
  const handleInstanceProgressed = (_payload: unknown): void => {
    if (
      tableName === WORKFLOW_TABLE_NAMES.todo
      || tableName === WORKFLOW_TABLE_NAMES.my
      || tableName === WORKFLOW_TABLE_NAMES.processed
      || tableName === WORKFLOW_TABLE_NAMES.instance
    ) {
      void reload();
    }
  };

  /**
   * 待办数量变更：待办页同步刷新列表
   */
  const handleTodoCountUpdated = (): void => {
    if (tableName === WORKFLOW_TABLE_NAMES.todo) {
      void reload();
    }
  };

  onMounted(() => {
    on('table:refresh', handleTableRefresh);
    on('workflow:scheme:changed', handleSchemeChanged);
    on('workflow:instance:progressed', handleInstanceProgressed);
    on('workflow:todo:count-updated', handleTodoCountUpdated);
  });

  onUnmounted(() => {
    off('table:refresh', handleTableRefresh);
    off('workflow:scheme:changed', handleSchemeChanged);
    off('workflow:instance:progressed', handleInstanceProgressed);
    off('workflow:todo:count-updated', handleTodoCountUpdated);
  });
}
