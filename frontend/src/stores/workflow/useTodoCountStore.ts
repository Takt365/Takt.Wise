// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/workflow
// 文件名称：useTodoCountStore.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工作流待办数量 Pinia Store（HTTP 首拉 + SignalR 增量）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref } from 'vue';
import { getFlowEngineTodoCount } from '@/api/workflow/flowEngine.api';
import type { FlowTodoCountUpdatedEvent } from '@/types/workflow/signalR.types';
import { createLogger } from '@/utils/logger';

const workflowTodoCountLogger = createLogger('workflow-todo-count');

/**
 * 工作流待办数量 Store
 */
export const useWorkflowTodoCountStore = defineStore('workflowTodoCount', () => {
  /** 当前用户待办数量 */
  const todoCount = ref(0);
  /** 是否正在拉取 */
  const loading = ref(false);

  /**
   * 通过 HTTP 刷新待办数量（首屏 / 重连补拉）
   * @returns {Promise<void>}
   */
  async function refreshTodoCountAsync(): Promise<void> {
    loading.value = true;
    try {
      const result = await getFlowEngineTodoCount();
      todoCount.value = result.todoCount ?? 0;
    } catch (error: unknown) {
      workflowTodoCountLogger.warn('拉取待办数量失败', { action: 'refreshTodoCount' }, error);
    } finally {
      loading.value = false;
    }
  }

  /**
   * 应用 SignalR 推送的待办数量
   * @param {FlowTodoCountUpdatedEvent} event Hub 事件
   * @returns {void}
   */
  function applyTodoCountFromSignalR(event: FlowTodoCountUpdatedEvent): void {
    todoCount.value = event.todoCount ?? 0;
  }

  /**
   * 重置待办数量
   * @returns {void}
   */
  function resetTodoCount(): void {
    todoCount.value = 0;
    loading.value = false;
  }

  return {
    todoCount,
    loading,
    refreshTodoCountAsync,
    applyTodoCountFromSignalR,
    resetTodoCount,
  };
});
