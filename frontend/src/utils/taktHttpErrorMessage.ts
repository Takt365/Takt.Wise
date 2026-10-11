// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktHttpErrorMessage.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：HTTP 层错误文案（vue-i18n，供 request 拦截器与 Store 复用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import axios from 'axios';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import type { AxiosError } from 'axios';

/**
 * 是否为 TaktApiError（避免 utils ↔ request 循环依赖）
 * @param error 捕获的异常
 */
function isTaktApiError(error: unknown): error is Error {
  return error instanceof Error && error.name === 'TaktApiError';
}

/** 已映射 i18n 的 HTTP 状态码（键路径：error.page.http.status.*） */
const HTTP_STATUS_I18N_KEYS: Readonly<Record<number, string>> = {
  400: 'error.page.http.status.400',
  401: 'error.page.http.status.401',
  403: 'error.page.http.status.403',
  404: 'error.page.http.status.404',
  405: 'error.page.http.status.405',
  408: 'error.page.http.status.408',
  409: 'error.page.http.status.409',
  413: 'error.page.http.status.413',
  415: 'error.page.http.status.415',
  422: 'error.page.http.status.422',
  429: 'error.page.http.status.429',
  500: 'error.page.http.status.500',
  501: 'error.page.http.status.501',
  502: 'error.page.http.status.502',
  503: 'error.page.http.status.503',
  504: 'error.page.http.status.504',
};

/**
 * 翻译 HTTP 错误文案（跟随 vue-i18n 当前 locale）
 * @param key i18n 键（如 error.page.http.network）
 * @param params 插值参数
 * @returns 本地化文案
 */
export function translateHttpErrorMessage(
  key: string,
  params?: Record<string, string | number>
): string {
  return translateLocaleMessage(key, params);
}

/**
 * @deprecated 请使用 translateHttpErrorMessage('error.page.http.network')
 */
export function getHttpNetworkErrorMessage(): string {
  return translateHttpErrorMessage('error.page.http.network');
}

/**
 * @deprecated 请使用 translateHttpErrorMessage('error.page.http.status.502')
 */
export function getHttpBadGatewayErrorMessage(): string {
  return translateHttpErrorMessage('error.page.http.status.502');
}

/**
 * 按 HTTP 状态码解析用户可见错误文案
 * @param status HTTP 状态码；undefined 表示无响应
 * @returns 本地化文案
 */
export function resolveHttpStatusMessage(status: number | undefined): string {
  if (status === undefined) {
    return translateHttpErrorMessage('error.page.http.network');
  }

  const statusKey = HTTP_STATUS_I18N_KEYS[status];
  if (statusKey) {
    return translateHttpErrorMessage(statusKey);
  }

  return translateHttpErrorMessage('error.page.http.defaultWithStatus', { status });
}

/**
 * 将 Axios / 业务异常解析为面向用户的错误文案
 * @param error 捕获的异常
 * @returns 本地化文案
 */
export function resolveHttpErrorMessage(error: unknown): string {
  if (isTaktApiError(error) && error.message) {
    return error.message;
  }

  if (axios.isAxiosError(error)) {
    const axiosError = error as AxiosError;
    if (axiosError.code === 'ECONNABORTED') {
      return translateHttpErrorMessage('error.page.http.timeout');
    }

    if (!axiosError.response && axiosError.request) {
      return translateHttpErrorMessage('error.page.http.network');
    }

    const responseData = axiosError.response?.data;
    if (
      typeof responseData === 'object'
      && responseData !== null
      && 'message' in responseData
      && typeof (responseData as { message?: unknown }).message === 'string'
    ) {
      const apiMessage = String((responseData as { message: string }).message).trim();
      if (apiMessage) {
        return apiMessage;
      }
    }

    // ASP.NET [ApiController] ValidationProblemDetails：errors 字段汇总
    if (typeof responseData === 'object' && responseData !== null && 'errors' in responseData) {
      const errors = (responseData as { errors?: unknown }).errors;
      if (errors && typeof errors === 'object') {
        const parts: string[] = [];
        for (const value of Object.values(errors as Record<string, unknown>)) {
          if (Array.isArray(value)) {
            for (const item of value) {
              if (typeof item === 'string' && item.trim()) {
                parts.push(item.trim());
              }
            }
          } else if (typeof value === 'string' && value.trim()) {
            parts.push(value.trim());
          }
        }
        if (parts.length > 0) {
          return parts.join('; ');
        }
      }
      const title = (responseData as { title?: unknown }).title;
      if (typeof title === 'string' && title.trim()) {
        return title.trim();
      }
    }

    return resolveHttpStatusMessage(axiosError.response?.status);
  }

  if (error instanceof Error && error.message) {
    const statusMatch = /status code (\d+)/i.exec(error.message);
    if (statusMatch) {
      return resolveHttpStatusMessage(Number(statusMatch[1]));
    }

    return error.message;
  }

  return translateHttpErrorMessage('error.page.http.default');
}

/**
 * HTTP 请求失败时的默认兜底文案（非业务 TaktApiResult.message）
 * @returns 本地化文案
 */
export function getHttpDefaultErrorMessage(): string {
  return translateHttpErrorMessage('error.page.http.default');
}

/**
 * 登录页 401 且无业务 message 时的兜底文案
 * @returns 本地化文案
 */
export function getHttpLoginFailedMessage(): string {
  return translateHttpErrorMessage('error.page.http.loginFailed');
}

/**
 * 请求配置错误文案
 * @returns 本地化文案
 */
export function getHttpConfigErrorMessage(): string {
  return translateHttpErrorMessage('error.page.http.config');
}
