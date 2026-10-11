// ========================================

// 项目名称：节拍智控·TaktWise

// 命名空间：frontend/src

// 文件名称：vite-env.d.ts

// 创建时间：2026-05-23

// 创建人：Takt365(Cursor AI)

// 功能描述：Vite 环境变量与 import.meta.env 类型声明（与 frontend/.env* 中 VITE_ 前缀变量一致）

// 

// 版权信息：Copyright (c) 2025 Takt  All rights reserved.

// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。

// ========================================



/// <reference types="vite/client" />

/// <reference types="vite-plugin-pwa/client" />



/**

 * 客户端可访问的环境变量（必须以 VITE_ 开头）

 * @description 服务端专用变量（如 VITE_API_PROXY_TARGET）仅在 vite.config.ts 中通过 loadEnv 使用，不在此声明

 */

interface ImportMetaEnv {

  readonly VITE_APP_TITLE: string;



  /** 浏览器访问根地址（OAuth、SignalR、开发代理同源） */

  readonly VITE_APP_ORIGIN: string;



  readonly VITE_API_BASE_URL: string;



  readonly VITE_DEV_SERVER_PORT: string;



  /** esbuild 构建目标（如 es2022） */

  readonly VITE_BUILD_TARGET: string;



  readonly VITE_BUILD_SOURCEMAP: string;



  readonly VITE_DEV_SERVER_HTTPS: string;



  readonly VITE_DEV_SERVER_HOST: string;



  readonly VITE_PWA_ENABLED: string;



  readonly VITE_PWA_DEV_ENABLED: string;



  readonly VITE_PWA_SHORT_NAME: string;



  readonly VITE_PWA_THEME_COLOR: string;



  readonly VITE_PWA_BACKGROUND_COLOR: string;



  readonly VITE_PWA_DESCRIPTION: string;



  /** 空闲自动登出超时时长（分钟）；0 表示禁用 */
  readonly VITE_AUTH_IDLE_TIMEOUT_MINUTES?: string;

  /** 空闲登出预警时长（分钟）；到期前弹窗，须小于总超时；0 表示不预警 */
  readonly VITE_AUTH_IDLE_WARNING_MINUTES?: string;

  readonly VITE_OAUTH_CLIENT_ID: string;



  readonly VITE_OAUTH_REDIRECT_URI: string;



  readonly VITE_OAUTH_SCOPE: string;



  readonly VITE_OAUTH_AUTHORIZE_PATH: string;



  readonly VITE_OAUTH_TOKEN_PATH: string;



  readonly VITE_SIGNALR_HUB_CONNECT_PATH: string;



  readonly VITE_SIGNALR_HUB_NOTIFICATION_PATH: string;

  readonly VITE_SIGNALR_HUB_EC_CHANGE_PATH: string;



  readonly VITE_LOG_MIN_LEVEL?: string;



  readonly VITE_LOG_ENABLE_CONSOLE?: string;



  readonly VITE_LOG_ENABLE_FILE?: string;



  readonly VITE_LOG_FILE_URL?: string;



  readonly VITE_LOG_ENABLE_REPORT?: string;



  readonly VITE_LOG_REPORT_URL?: string;



  readonly VITE_LOG_BATCH_SIZE?: string;



  readonly VITE_LOG_FLUSH_INTERVAL_MS?: string;

  readonly VITE_LONG_TASK_MONITOR_ENABLED?: string;

  readonly VITE_LONG_TASK_WARN_MS?: string;

  readonly VITE_LONG_TASK_ERROR_MS?: string;

  /** @deprecated 使用 VITE_EVENT_TRACKING_REPORT_ENABLED */
  readonly VITE_LONG_TASK_REPORT_ENABLED?: string;

  /** @deprecated 使用 VITE_EVENT_TRACKING_BATCH_SIZE */
  readonly VITE_LONG_TASK_REPORT_BATCH_SIZE?: string;

  /** @deprecated 使用 VITE_EVENT_TRACKING_FLUSH_MS */
  readonly VITE_LONG_TASK_REPORT_FLUSH_MS?: string;

  readonly VITE_EVENT_TRACKING_ENABLED?: string;

  readonly VITE_EVENT_TRACKING_REPORT_ENABLED?: string;

  readonly VITE_EVENT_TRACKING_BATCH_SIZE?: string;

  readonly VITE_EVENT_TRACKING_FLUSH_MS?: string;

  readonly VITE_FPS_MONITOR_ENABLED?: string;

  readonly VITE_FPS_WARN_THRESHOLD?: string;

  readonly VITE_FPS_SAMPLE_MS?: string;

  readonly VITE_FPS_REPORT_COOLDOWN_MS?: string;

  readonly VITE_FPS_DWELL_MIN_MS?: string;

  readonly VITE_FPS_DROP_ALERT_ENABLED?: string;

  readonly VITE_API_PERF_TRACK_ENABLED?: string;

  readonly VITE_API_SLOW_MS?: string;

  readonly VITE_API_ERROR_MS?: string;

  readonly VITE_WEB_VITALS_MONITOR_ENABLED?: string;

  readonly VITE_FCP_WARN_MS?: string;

  readonly VITE_LCP_WARN_MS?: string;

  readonly VITE_LCP_ERROR_MS?: string;

  readonly VITE_INP_WARN_MS?: string;

  readonly VITE_INP_ERROR_MS?: string;

  readonly VITE_CLS_WARN?: string;

  readonly VITE_CLS_ERROR?: string;

  readonly VITE_CORRELATION_WINDOW_MS?: string;

  readonly VITE_APP_VERSION?: string;



  readonly VITE_EVENT_ENABLE_CONSOLE?: string;



  readonly VITE_EVENT_ENABLE_REPORT?: string;



  readonly VITE_EVENT_REPORT_URL?: string;



  readonly VITE_EVENT_BATCH_SIZE?: string;



  readonly VITE_EVENT_FLUSH_INTERVAL_MS?: string;



  readonly MODE: string;



  readonly DEV: boolean;



  readonly PROD: boolean;



  readonly SSR: boolean;

}



interface ImportMeta {

  readonly env: ImportMetaEnv;

}



/** ECharts 官方 i18n 语言包（与 registerLocale 的 LocaleOption 一致） */

declare module 'echarts/i18n/langEN-obj' {

  import type { LocaleOption } from 'echarts';

  const locale: LocaleOption;

  export default locale;

}



declare module 'echarts/i18n/langZH-obj' {

  import type { LocaleOption } from 'echarts';

  const locale: LocaleOption;

  export default locale;

}



declare module 'echarts/i18n/langJA-obj' {

  import type { LocaleOption } from 'echarts';

  const locale: LocaleOption;

  export default locale;

}

