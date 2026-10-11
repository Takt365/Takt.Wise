// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src
// 文件名称：main.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：应用入口；Pinia/路由/i18n/日志/性能监控 bootstrap
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import Antd from 'ant-design-vue';
import FcDesigner from '@form-create/antd-designer';
import { useUmoEditor } from '@umoteam/editor';
import '@umoteam/editor/style';
import App from './App.vue';
import router from './router';
import i18n from './locales';
import { initTaktGsap } from '@/bootstrap/taktGsap';
import { registerTaktEventHandlers } from '@/bootstrap/taktEventHandlers';
import { initTaktIdleSession } from '@/bootstrap/taktIdleSession';
import { initTaktTokenSession } from '@/bootstrap/taktTokenSession';
import { initTaktClientPerformanceMonitor } from '@/bootstrap/taktClientPerformanceMonitor';
import { ensureTaktPaginationConfigAsync } from '@/config/taktPagination';
import { registerPermissionDirective } from '@/directives/permission';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';
import { useSettingStore } from '@/stores/common/useSettingStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { applySettings } from '@/utils/applySettings';
import { initEventBus } from '@/utils/eventBus';
import { initLogger } from '@/utils/logger';
import { parseEnvBoolean, setRuntimeRouter } from '@/utils/runtimeContext';
import { initTaktThemeDom } from '@/utils/theme';
import 'ant-design-vue/dist/reset.css';
import 'flag-icons/css/flag-icons.min.css';
import './styles/global.css';

setRuntimeRouter(router);
initTaktThemeDom();
initTaktGsap();

/**
 * 注册 PWA Service Worker（仅当 VITE_PWA_ENABLED 且构建已启用 vite-plugin-pwa）
 */
async function registerPwaServiceWorker(): Promise<void> {
  if (!parseEnvBoolean(import.meta.env.VITE_PWA_ENABLED, true)) {
    return;
  }
  const { registerSW } = await import('virtual:pwa-register');
  registerSW({
    immediate: true,
  });
}

void registerPwaServiceWorker();

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);

useSettingStore();
applySettings();

const userStore = useUserStore();
const tenantStore = useTenantStore();
if (userStore.isLoggedIn) {
  tenantStore.restoreTenantCodeFromStorage();
}
if (tenantStore.tenantCode?.trim()) {
  await ensureTaktPaginationConfigAsync();
}

const isLoginEntryPath =
  typeof window !== 'undefined'
  && (window.location.pathname === '/login' || window.location.pathname.startsWith('/login/'));
if (!isLoginEntryPath) {
  useLocaleStore().initLocaleFromStorage();
}

app.use(router);
app.use(i18n);
app.use(Antd);
app.use(FcDesigner);
app.use(FcDesigner.formCreate);
app.use(useUmoEditor, {});

/**
 * ant-design-vue Table 内部 MeasureCell 会 emit columnResize，组件未声明 emits，路由切换时刷屏。
 * 过滤该已知误报，其它 Vue warn 仍输出。
 */
app.config.warnHandler = (msg, _instance, trace) => {
  if (typeof msg === 'string' && msg.includes('emitted event "columnResize"')) {
    return;
  }
  console.warn(`[Vue warn]: ${msg}${trace ? `\n${trace}` : ''}`);
};

registerTaktEventHandlers();
initTaktIdleSession();
initTaktTokenSession();
registerPermissionDirective(app);
initEventBus();
initLogger(app, undefined, router);
initTaktClientPerformanceMonitor();

app.mount('#app');
