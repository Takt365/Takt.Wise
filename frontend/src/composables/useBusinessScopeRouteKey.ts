// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useBusinessScopeRouteKey.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：租户/公司切换时 RouterView :key，强制重建当前业务页并重新 loadData
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed, type ComputedRef } from 'vue';
import { useTenantStore } from '@/stores/identity/useTenantStore';

/**
 * 业务隔离域路由键（租户 + 公司）
 * @description 绑定到布局 RouterView :key；company:change / tenant:change 后子页面 remount
 * @returns {ComputedRef<string>} 路由视图 key
 */
export function useBusinessScopeRouteKey(): ComputedRef<string> {
  const tenantStore = useTenantStore();
  return computed(
    () => `${tenantStore.tenantCode}:${tenantStore.companyCode}`,
  );
}
