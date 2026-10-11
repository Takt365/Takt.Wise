// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：runtimeContext.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：日志与事件共享的运行时上下文合并（运行时网关：Pinia / Router 注入；非纯工具）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Router } from 'vue-router';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import type { LogContext } from '@/types/logger.types';

export { createAppMeta } from '@/utils/appMeta';
export { readViteEnv } from '@/config/viteEnv';

/** 运行时注入的路由实例（避免 runtime-context ↔ router ↔ logger 循环依赖） */
let runtimeRouter: Router | null = null;

/**
 * 解析布尔型环境变量
 * @param {string | undefined} value 环境变量
 * @param {boolean} defaultValue 默认值
 * @returns {boolean} 解析结果
 */
export function parseEnvBoolean(value: string | undefined, defaultValue: boolean): boolean {
  if (value === undefined || value === '') {
    return defaultValue;
  }
  return value === 'true' || value === '1';
}

/**
 * 注入路由实例（在 main.ts 创建 router 后调用）
 * @param {Router} router Vue Router 实例
 */
export function setRuntimeRouter(router: Router): void {
  runtimeRouter = router;
}

/**
 * 合并 Pinia / Router 运行时上下文
 * @param {LogContext} [context] 调用方上下文
 * @returns {LogContext} 合并后的上下文
 */
export function mergeRuntimeContext(context?: LogContext): LogContext {
  const merged: LogContext = { ...context };

  try {
    const userStore = useUserStore();
    if (userStore.userId) {
      merged.userId = merged.userId ?? userStore.userId;
    }
    if (userStore.userName) {
      merged.userName = merged.userName ?? userStore.userName;
    }
  } catch {
    // Pinia 尚未挂载时忽略
  }

  try {
    const tenantStore = useTenantStore();
    if (tenantStore.tenantCode) {
      merged.tenantCode = merged.tenantCode ?? tenantStore.tenantCode;
    }
    if (tenantStore.companyCode) {
      merged.companyCode = merged.companyCode ?? tenantStore.companyCode;
    }
  } catch {
    // Pinia 尚未挂载时忽略
  }

  if (runtimeRouter) {
    try {
      merged.route = merged.route ?? runtimeRouter.currentRoute.value.fullPath;
    } catch {
      // Router 尚未就绪时忽略
    }
  }

  return merged;
}
