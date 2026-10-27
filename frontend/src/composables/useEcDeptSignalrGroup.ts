// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useEcDeptSignalrGroup.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门页挂载时加入 TaktEcChangeHub 部门组，离开时退出
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onMounted, onUnmounted, watch } from 'vue';
import * as signalR from '@microsoft/signalr';
import { storeToRefs } from 'pinia';
import { useSignalRStore } from '@/stores/foundation/useSignalrStore';
import { taktSignalRManager } from '@/utils/taktSignalr';
import { createLogger } from '@/utils/logger';
import type { TaktEcExecCode } from '@/constants/logistics/ecExecCodes.constants';

const ecDeptSignalrLogger = createLogger('ec-dept-signalr');

/**
 * 设变部门页订阅本部门 SignalR 通知组（JoinDeptGroup / LeaveDeptGroup）
 * @param deptCode 部门编码（Te/Mp 等，与后端 TaktEcExecCodes 一致）
 */
export function useEcExecSignalRGroup(deptCode: TaktEcExecCode): void {
  const { ecChangeHubState } = storeToRefs(useSignalRStore());
  const normalizedCode = deptCode.trim();

  /**
   * 加入部门通知组
   */
  async function joinDeptGroupAsync(): Promise<void> {
    if (!normalizedCode) {
      return;
    }
    if (ecChangeHubState.value !== signalR.HubConnectionState.Connected) {
      return;
    }
    try {
      await taktSignalRManager.joinEcExecGroupAsync(normalizedCode);
      ecDeptSignalrLogger.info('已加入设变部门 SignalR 组', { deptCode: normalizedCode });
    } catch (error: unknown) {
      ecDeptSignalrLogger.warn('加入设变部门 SignalR 组失败', { deptCode: normalizedCode }, error);
    }
  }

  /**
   * 离开部门通知组
   */
  async function leaveDeptGroupAsync(): Promise<void> {
    if (!normalizedCode) {
      return;
    }
    try {
      await taktSignalRManager.leaveEcExecGroupAsync(normalizedCode);
    } catch (error: unknown) {
      ecDeptSignalrLogger.warn('离开设变部门 SignalR 组失败', { deptCode: normalizedCode }, error);
    }
  }

  watch(ecChangeHubState, (state) => {
    if (state === signalR.HubConnectionState.Connected) {
      void joinDeptGroupAsync();
    }
  });

  onMounted(() => {
    void joinDeptGroupAsync();
  });

  onUnmounted(() => {
    void leaveDeptGroupAsync();
  });
}
