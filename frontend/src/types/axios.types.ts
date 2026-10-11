// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：axios.types.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Axios 请求配置扩展（登录跳过刷新、登录错误处理）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import 'axios';

declare module 'axios' {
  export interface AxiosRequestConfig {
    /** 跳过请求前主动刷新（登录等接口） */
    skipTokenRefresh?: boolean;
    /** 登录预检/会话接口：401 仅返回业务 message，不触发全局会话过期 */
    skipLoginAuthError?: boolean;
    /** 为 true 时不通过 EventBus 弹出全局错误（由调用方自行 toast） */
    skipErrorNotification?: boolean;
    /** 二进制下载：返回 blob + Content-Disposition/Content-Type，而非仅 Blob */
    returnBinaryMeta?: boolean;
  }

  export interface InternalAxiosRequestConfig {
    /** 已为 401 / 业务未授权尝试过 refresh 并重试 */
    _retryAuth?: boolean;
    /** 跳过请求前主动刷新（登录等接口） */
    skipTokenRefresh?: boolean;
    /** 登录预检/会话接口：401 仅返回业务 message，不触发全局会话过期 */
    skipLoginAuthError?: boolean;
    /** 为 true 时不通过 EventBus 弹出全局错误（由调用方自行 toast） */
    skipErrorNotification?: boolean;
    /** 二进制下载：返回 blob + Content-Disposition/Content-Type，而非仅 Blob */
    returnBinaryMeta?: boolean;
  }
}
