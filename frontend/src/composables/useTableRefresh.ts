// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTableRefresh.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅 table:refresh（租户/公司切换时由全局事件发出），刷新列表/树数据
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import { useEventBus } from '@/utils/eventBus';

/**
 * 订阅全局 table:refresh，在租户/公司切换后重新加载页面数据
 * @param {() => void | Promise<void>} reload 刷新回调（通常为 loadData）
 * @param {string} [tableName] 可选表名；与事件 payload.tableName 一致时才刷新
 * @returns {void}
 */
export function useTableRefresh(
  reload: () => void | Promise<void>,
  tableName?: string,
): void {
  const { on, off } = useEventBus();

  /**
   * 处理 table:refresh 事件
   * @param payload 事件载荷
   */
  const handleTableRefresh = (payload?: { tableName?: string }): void => {
    if (tableName && payload?.tableName && payload.tableName !== tableName) {
      return;
    }
    void reload();
  };

  onMounted(() => {
    on('table:refresh', handleTableRefresh);
  });

  onUnmounted(() => {
    off('table:refresh', handleTableRefresh);
  });
}
