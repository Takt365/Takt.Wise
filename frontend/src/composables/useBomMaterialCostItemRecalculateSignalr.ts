// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useBomMaterialCostItemRecalculateSignalr.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：订阅 BOM 物料成本机种月平均重算 SignalR 完成事件
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted } from 'vue';
import type { BomMaterialCostItemRecalculateCompletedEvent } from '@/types/logistics/manufacturing/bom/materialCostItemSignalR.types';
import { useEventBus } from '@/utils/eventBus';

/** BOM 物料成本列表 tableName 常量 */
export const BOM_MATERIAL_COST_ITEM_TABLE_NAME = 'logistics-manufacturing-bom-material-cost-item';

/**
 * 格式化执行耗时（毫秒 → 可读文案）
 * @param {number} ms 毫秒
 * @returns {string} 可读耗时
 */
export function formatBomMaterialCostItemRecalculateDuration(ms: number): string {
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
 * 订阅 BOM 物料成本重算 SignalR 完成事件
 * @param {(event: BomMaterialCostItemRecalculateCompletedEvent) => void | Promise<void>} onCompleted 完成回调
 * @returns {void}
 */
export function useBomMaterialCostItemRecalculateSignalR(
  onCompleted: (event: BomMaterialCostItemRecalculateCompletedEvent) => void | Promise<void>,
): void {
  const { on, off } = useEventBus();

  const handleRecalculateCompleted = (event: BomMaterialCostItemRecalculateCompletedEvent): void => {
    void onCompleted(event);
  };

  onMounted(() => {
    on('logistics:bom-material-cost-item:recalculate-completed', handleRecalculateCompleted);
  });

  onUnmounted(() => {
    off('logistics:bom-material-cost-item:recalculate-completed', handleRecalculateCompleted);
  });
}
