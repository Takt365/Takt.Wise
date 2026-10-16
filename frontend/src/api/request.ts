// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api
// 文件名称：request.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：Axios 统一请求实例；自动携带鉴权与租户头；解析后端 TaktApiResult 并返回 data 载荷
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import axios from 'axios';
import type {
  AxiosError,
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from 'axios';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { resolveRequestLocale } from '@/stores/foundation/useLocaleStore';
import type { TaktApiResult, TaktBinaryDownload } from '@/types/common.types';
import { TaktResultCode } from '@/utils/common';
import { createLogger } from '@/utils/logger';
import { savePayloadContainsFullWidth } from '@/utils/regex';
import {
  consumeReadonlySaveFieldNames,
  installReadonlySaveFieldCapture,
  TAKT_READONLY_FIELDS_HEADER,
} from '@/utils/taktReadonlySaveFields';
import { resolveHttpErrorMessage } from '@/utils/taktHttpErrorMessage';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { EventBus, emitNotification } from '@/utils/eventBus';
import {
  attachApiPerformanceStart,
  observeApiPerformanceError,
  observeApiPerformanceResponse,
} from '@/utils/taktApiPerformanceTracker';
import { ensureValidAccessToken, refreshOAuthTokens } from '@/utils/oauth';
import { isLogoutInProgress } from '@/bootstrap/taktLogoutFlow';
import { buildTaktClientProfileHeaders } from '@/utils/taktClientProfile';
import {
  isDemoReadonly,
  isDemoWriteExemptUrl,
  isMutatingHttpMethod,
} from '@/utils/taktDemoMode';

const requestLogger = createLogger('request');

installReadonlySaveFieldCapture();

/** Axios 配置扩展：无感刷新重试标记 */
declare module 'axios' {
  export interface InternalAxiosRequestConfig {
    /** 已为 401 / 业务未授权尝试过 refresh 并重试 */
    _retryAuth?: boolean;
    /** 跳过请求前主动刷新（登录等接口） */
    skipTokenRefresh?: boolean;
    /** 请求开始 performance.now()（API 性能监控） */
    _taktPerfStartMs?: number;
    /** 跳过 API 性能监控 */
    skipApiPerformanceTrack?: boolean;
  }
}

/**
 * API 业务错误
 * @description HTTP 失败或响应体 code !== TaktResultCode.Success 时由拦截器抛出，供业务层 catch 区分处理
 */
export class TaktApiError extends Error {
  /**
   * 后端结果代码（对应 Takt.Shared.Enums.TaktResultCode）
   */
  code: TaktResultCode;

  /**
   * 后端 TaktApiResult.data 字段（失败时可能为 null）
   */
  data: unknown;

  /**
   * @param {string} message 错误提示（通常与后端 message 一致）
   * @param {TaktResultCode} code 结果代码
   * @param {unknown} [data=null] 业务 data 载荷
   */
  constructor(message: string, code: TaktResultCode, data: unknown = null) {
    super(message);
    this.name = 'TaktApiError';
    this.code = code;
    this.data = data;
  }
}

/**
 * 判断响应体是否为后端统一包装 TaktApiResult
 * @param {unknown} data 原始响应体
 * @returns {boolean} 是否为 TaktApiResult 结构
 */
function isTaktApiResult(data: unknown): data is TaktApiResult<unknown> {
  if (!data || typeof data !== 'object') {
    return false;
  }
  const body = data as Record<string, unknown>;
  return typeof body.code === 'number' && typeof body.message === 'string';
}

/**
 * 是否为二进制下载响应（跳过 JSON 解包，直接返回 Blob / ArrayBuffer）
 * @param {AxiosResponse} response Axios 原始响应
 * @returns {boolean} 是否按二进制处理
 */
function isBinaryResponse(response: AxiosResponse): boolean {
  const responseType = response.config.responseType;
  if (responseType === 'blob' || responseType === 'arraybuffer') {
    return true;
  }
  return response.data instanceof Blob || response.data instanceof ArrayBuffer;
}

/**
 * 解包二进制下载响应（可选携带 Content-Disposition / Content-Type）
 * @template T Blob 或 TaktBinaryDownload
 * @param {AxiosResponse} response Axios 原始响应
 * @returns {T} 二进制载荷
 */
function unwrapBinaryResponse<T>(response: AxiosResponse): T {
  const data = response.data;
  if (response.config.returnBinaryMeta && data instanceof Blob) {
    const headers = response.headers as Record<string, string | undefined>;
    const binary: TaktBinaryDownload = {
      blob: data,
      contentDisposition: headers['content-disposition'] ?? null,
      contentType: headers['content-type'] ?? null,
    };
    return binary as T;
  }
  return data as T;
}

/**
 * 解包成功响应：返回 TaktApiResult.data；业务失败时提示并抛出 TaktApiError
 * @template T 业务数据类型
 * @param {AxiosResponse} response Axios 原始响应
 * @returns {T} 解包后的业务 data
 * @throws {TaktApiError} code !== TaktResultCode.Success
 */
function unwrapTaktApiResult<T>(response: AxiosResponse): T {
  if (isBinaryResponse(response)) {
    return unwrapBinaryResponse<T>(response);
  }

  const body = response.data;

  if (!isTaktApiResult(body)) {
    return body as T;
  }

  if (body.code !== TaktResultCode.Success) {
    const errMsg = body.message || '请求失败';
    if (body.code !== TaktResultCode.Unauthorized) {
      emitNotification('error', errMsg);
    }
    throw new TaktApiError(errMsg, body.code, body.data);
  }

  return body.data as T;
}

/**
 * 将 unknown 异常收窄为 AxiosError
 * @param {unknown} error 捕获的异常
 * @returns {AxiosError | null} AxiosError 或 null
 */
function toAxiosError(error: unknown): AxiosError | null {
  if (!axios.isAxiosError(error)) {
    return null;
  }
  return error as AxiosError;
}

/**
 * 解析会话过期提示（缺省 layouts.page.session.expired）
 * @param message 业务或 HTTP 附带文案
 */
function resolveSessionExpiredMessage(message?: string): string {
  return message?.trim() || translateLocaleMessage('layouts.page.session.expired');
}

/**
 * 未授权：仅广播事件，由 bootstrap 订阅方清状态并跳转
 */
function emitSessionExpired(message?: string): void {
  EventBus.emit('auth:session-expired', { message: resolveSessionExpiredMessage(message) });
}

/**
 * 是否为登录预检/会话接口（401 不触发全局会话过期）
 * @param config Axios 请求配置
 */
function isSkipLoginAuthError(config?: InternalAxiosRequestConfig): boolean {
  return Boolean(config?.skipLoginAuthError);
}

/**
 * 是否为 Cookie 会话注销（登出进行中仍须放行，否则服务端无法回填 LogoutAt / 关闭 Online）
 * @param config Axios 请求配置
 */
function isSignOutSessionRequest(config?: InternalAxiosRequestConfig): boolean {
  const url = config?.url ?? '';
  return url.includes('session/signout');
}

/**
 * 从裸 HTTP 响应体提取 message 字段
 * @param data 响应体
 * @param fallback 缺省文案
 */
function extractResponseMessage(data: unknown, fallback: string): string {
  if (typeof data === 'object' && data !== null && 'message' in data) {
    const message = (data as { message?: unknown }).message;
    if (typeof message === 'string' && message.trim()) {
      return message.trim();
    }
  }
  return fallback;
}

/**
 * 401 / 业务未授权：尝试 refresh_token 后重试原请求
 * @param {InternalAxiosRequestConfig} config 原请求配置
 * @param {string} [message] 失败时提示文案
 * @returns {Promise<AxiosResponse>} 重试响应或 reject
 */
async function retryRequestAfterTokenRefresh(
  config: InternalAxiosRequestConfig,
  message?: string
): Promise<AxiosResponse> {
  if (isSkipLoginAuthError(config)) {
    const errMsg = message || '登录失败';
    return Promise.reject(new TaktApiError(errMsg, TaktResultCode.Unauthorized));
  }

  if (config._retryAuth) {
    emitSessionExpired(message);
    return Promise.reject(new TaktApiError(resolveSessionExpiredMessage(message), TaktResultCode.Unauthorized));
  }

  const refreshed = await refreshOAuthTokens();
  if (!refreshed) {
    emitSessionExpired(message);
    return Promise.reject(new TaktApiError(resolveSessionExpiredMessage(message), TaktResultCode.Unauthorized));
  }

  const userStore = useUserStore();
  config._retryAuth = true;
  config.headers.Authorization = `Bearer ${userStore.token}`;
  return axiosInstance.request(config);
}

/**
 * 判断是否为租户业务库不可用类错误文案（后端 error.tenant.database.* 默认中文）
 * @param message 错误提示
 */
function isTenantDatabaseErrorMessage(message: string): boolean {
  const text = message.trim();
  if (!text) {
    return false;
  }
  return (
    text.includes('业务数据库不存在')
    || text.includes('业务数据库无法连接')
    || text.includes('缺少业务数据表')
    || text.includes('无法登录 SQL Server')
    || text.includes('database does not exist')
    || text.includes('business database is unreachable')
    || text.includes('business tables are missing')
    || text.includes('Cannot log in to SQL Server')
  );
}

/**
 * 统一处理 Axios 错误响应
 * @param {unknown} error 拦截器捕获的异常
 * @returns {Promise<unknown>} reject 或刷新成功后重试的响应
 */
function rejectFromAxiosError(error: unknown): Promise<unknown> {
  if (isLogoutInProgress()) {
    return Promise.reject(error);
  }

  const axiosError = toAxiosError(error);
  if (!axiosError) {
    if (error instanceof Error) {
      if (error.message === 'logout in progress') {
        return Promise.reject(error);
      }
      requestLogger.warn('请求配置错误', { action: 'config' }, error);
      emitNotification('error', resolveHttpErrorMessage(error));
    }
    return Promise.reject(error);
  }

  if (!axiosError.response) {
    if (axiosError.request) {
      requestLogger.warn('网络连接失败', { action: 'network' }, axiosError);
      emitNotification('error', resolveHttpErrorMessage(axiosError));
    }
    return Promise.reject(axiosError);
  }

  const { status, data, config: axiosConfig } = axiosError.response;
  const resolvedMessage = resolveHttpErrorMessage(axiosError);

  if (isTaktApiResult(data)) {
    if (data.code === TaktResultCode.Unauthorized && axiosError.config) {
      if (isSkipLoginAuthError(axiosError.config)) {
        return Promise.reject(new TaktApiError(data.message, data.code, data.data));
      }
      return retryRequestAfterTokenRefresh(axiosError.config, data.message || undefined);
    }
    if (data.code !== TaktResultCode.Unauthorized && !axiosConfig?.skipErrorNotification) {
      const errMsg = data.message?.trim() || resolvedMessage;
      emitNotification('error', errMsg);
      if (isTenantDatabaseErrorMessage(errMsg)) {
        requestLogger.warn('租户业务库不可用', { action: 'tenantDatabase', url: axiosConfig?.url }, axiosError);
      }
    }
    return Promise.reject(new TaktApiError(data.message || resolvedMessage, data.code, data.data));
  }

  if (!axiosConfig?.skipErrorNotification) {
    switch (status) {
      case 401:
        if (!isSkipLoginAuthError(axiosConfig)) {
          if (axiosError.config) {
            return retryRequestAfterTokenRefresh(axiosError.config, resolvedMessage);
          }
          emitSessionExpired(resolvedMessage);
        }
        break;
      case 403:
        emitNotification('error', resolvedMessage);
        break;
      case 400:
      case 404:
      case 500:
      default:
        if (status === 500) {
          requestLogger.error(
            '请求失败',
            { action: 'http', status, url: axiosConfig?.url, message: resolvedMessage },
            axiosError
          );
        } else if (status !== 404) {
          requestLogger.warn(
            '请求失败',
            { action: 'http', status, url: axiosConfig?.url, message: resolvedMessage },
            axiosError
          );
        }
        emitNotification('error', resolvedMessage);
        if (isTenantDatabaseErrorMessage(resolvedMessage)) {
          requestLogger.warn('租户业务库不可用', { action: 'tenantDatabase', url: axiosConfig?.url }, axiosError);
        }
        break;
    }
  }

  if (status === 401 && isSkipLoginAuthError(axiosConfig)) {
    return Promise.reject(new TaktApiError(resolvedMessage, TaktResultCode.Unauthorized));
  }

  if (status === 400 && isSkipLoginAuthError(axiosConfig)) {
    return Promise.reject(new TaktApiError(resolvedMessage, TaktResultCode.BadRequest));
  }

  return Promise.reject(axiosError);
}

/**
 * 业务 HTTP 客户端
 * @description 所有方法返回值均为拦截器解包后的业务 data，而非完整 TaktApiResult 包装
 */
export interface TaktHttpClient {
  /**
   * 配置式请求（推荐，与业务 API 生成模板一致）
   * @template T 业务 data 类型
   * @param {AxiosRequestConfig} config Axios 请求配置
   * @returns {Promise<T>} 解包后的业务 data
   */
  <T = unknown>(config: AxiosRequestConfig): Promise<T>;

  /**
   * GET 请求
   * @template T 业务 data 类型
   * @param {string} url 相对路径（相对 baseURL）
   * @param {AxiosRequestConfig} [config] 附加配置
   * @returns {Promise<T>} 解包后的业务 data
   */
  get<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T>;

  /**
   * POST 请求
   * @template T 业务 data 类型
   */
  post<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;

  /**
   * PUT 请求
   * @template T 业务 data 类型
   */
  put<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;

  /**
   * DELETE 请求
   * @template T 业务 data 类型
   */
  delete<T = unknown>(url: string, config?: AxiosRequestConfig): Promise<T>;

  /**
   * PATCH 请求
   * @template T 业务 data 类型
   */
  patch<T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T>;
}

/**
 * 是否为创建或更新：POST 资源集合、POST import、PUT 主键、PUT status/sort/built-in/public
 * @param method HTTP 方法
 * @param url 请求地址
 * @returns {boolean} 是创建或更新时为 true
 */
function isCreateOrUpdateRequest(method?: string, url?: string): boolean {
  const httpMethod = (method || '').toLowerCase()
  const parts = String(url ?? '')
    .split('?')[0]
    .split('/')
    .filter((segment) => segment && segment.toLowerCase() !== 'api')
  const action = (parts[parts.length - 1] || '').toLowerCase()
  if (httpMethod === 'post') {
    return parts.length === 1 || (parts.length === 2 && action === 'import')
  }
  if (httpMethod !== 'put' || parts.length !== 2) {
    return false
  }
  return /^\d+$/.test(action) || action === 'status' || action === 'sort' || action === 'built-in' || action === 'public'
}

/**
 * Axios 底层实例（挂载请求/响应拦截器）
 */
const axiosInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
  timeout: 30000,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
  },
});

axiosInstance.interceptors.request.use(
  async (config: InternalAxiosRequestConfig) => {
    if (isLogoutInProgress() && !isSignOutSessionRequest(config)) {
      return Promise.reject(new Error('logout in progress'));
    }

    if (isCreateOrUpdateRequest(config.method, config.url)) {
      const readonlyFieldNames = consumeReadonlySaveFieldNames();
      if (readonlyFieldNames.size > 0) {
        config.headers.set(TAKT_READONLY_FIELDS_HEADER, [...readonlyFieldNames].join(','));
      }
      if (savePayloadContainsFullWidth(config.data, undefined, readonlyFieldNames)) {
        const translated = translateLocaleMessage('common.validation.fullwidth.forbidden');
        const msg = translated === 'common.validation.fullwidth.forbidden' || translated.includes('{field}')
          ? '不能包含全角字符'
          : translated;
        emitNotification('warning', msg);
        return Promise.reject(new TaktApiError(msg, TaktResultCode.BadRequest));
      }
    }

    // Demo 只读：拦截写请求（鉴权/SignalR 等豁免）
    if (
      isDemoReadonly()
      && isMutatingHttpMethod(config.method)
      && !isDemoWriteExemptUrl(config.url)
    ) {
      const msg = translateLocaleMessage('components.navigation.page.systemsetting.demohint');
      emitNotification('warning', msg);
      return Promise.reject(new TaktApiError(msg, TaktResultCode.Forbidden));
    }

    const userStore = useUserStore();
    const tenantStore = useTenantStore();

    if (userStore.token && !tenantStore.tenantCode) {
      tenantStore.restoreTenantCodeFromStorage();
    }

    if (!config.skipTokenRefresh && userStore.token && userStore.refreshToken) {
      await ensureValidAccessToken();
    }

    if (userStore.token) {
      config.headers.Authorization = `Bearer ${userStore.token}`;
    }
    if (tenantStore.tenantCode) {
      config.headers['X-Tenant-Code'] = tenantStore.tenantCode;
    }
    if (tenantStore.companyCode) {
      config.headers['X-Company-Code'] = tenantStore.companyCode;
    }

    config.headers['Accept-Language'] = resolveRequestLocale();

    const clientProfileHeaders = buildTaktClientProfileHeaders();
    for (const [key, value] of Object.entries(clientProfileHeaders)) {
      config.headers[key] = value;
    }

    if (config.data instanceof FormData) {
      delete config.headers['Content-Type'];
    }

    return attachApiPerformanceStart(config);
  },
  (error: unknown) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => observeApiPerformanceResponse(response),
  (error: unknown) => observeApiPerformanceError(error)
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    try {
      return unwrapTaktApiResult(response);
    } catch (err: unknown) {
      if (
        err instanceof TaktApiError &&
        err.code === TaktResultCode.Unauthorized &&
        response.config
      ) {
        if (isSkipLoginAuthError(response.config)) {
          return Promise.reject(err);
        }
        return retryRequestAfterTokenRefresh(response.config, err.message).then((retried) => {
          try {
            return unwrapTaktApiResult(retried);
          } catch (retryErr: unknown) {
            return Promise.reject(retryErr);
          }
        });
      }
      return Promise.reject(err);
    }
  },
  (error: unknown) => rejectFromAxiosError(error)
);

/**
 * 配置式请求入口
 * @template T 业务 data 类型
 */
function taktRequest<T = unknown>(config: AxiosRequestConfig): Promise<T> {
  return axiosInstance.request(config) as Promise<T>;
}

const request = Object.assign(taktRequest, {
  get: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    axiosInstance.get(url, config) as Promise<T>,
  post: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.post(url, data, config) as Promise<T>,
  put: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.put(url, data, config) as Promise<T>,
  delete: <T = unknown>(url: string, config?: AxiosRequestConfig) =>
    axiosInstance.delete(url, config) as Promise<T>,
  patch: <T = unknown>(url: string, data?: unknown, config?: AxiosRequestConfig) =>
    axiosInstance.patch(url, data, config) as Promise<T>,
}) as TaktHttpClient;

export default request;
