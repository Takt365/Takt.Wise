// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktClientProfile.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端浏览器/系统/设备画像（与 backend TaktUserAgentHelper、TaktHttpHeaderNames 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  TaktBrowserType,
  TaktDeviceType,
  TaktOperatingSystem,
} from '@/constants/takt.constants';

/** HTTP 客户端画像请求头（与 backend TaktHttpHeaderNames 一致） */
export const TaktClientProfileHeaderNames = {
  userAgent: 'X-Takt-User-Agent',
  browser: 'X-Takt-Client-Browser',
  operatingSystem: 'X-Takt-Client-Os',
  deviceType: 'X-Takt-Client-Device',
} as const;

/** SignalR WebSocket 查询参数（与 backend TaktHttpQueryNames 一致） */
export const TaktClientProfileQueryNames = {
  userAgent: 'takt_user_agent',
  browser: 'takt_client_browser',
  operatingSystem: 'takt_client_os',
  deviceType: 'takt_client_device',
} as const;

/** 客户端画像 */
export type TaktClientProfile = {
  userAgent: string;
  browser: string;
  operatingSystem: string;
  deviceType: string;
};

const MAX_USER_AGENT_LENGTH = 500;

/**
 * 解析 User-Agent 为浏览器、操作系统、登录设备（与 backend TaktUserAgentHelper.Parse 规则一致）
 * @param {string | null | undefined} userAgent User-Agent 原文
 * @returns {TaktClientProfile} 客户端画像
 */
export function parseTaktClientProfile(userAgent?: string | null): TaktClientProfile {
  const raw = (userAgent ?? '').trim();
  if (!raw) {
    return {
      userAgent: '',
      browser: TaktBrowserType.Unknown,
      operatingSystem: TaktOperatingSystem.Unknown,
      deviceType: TaktDeviceType.Unknown,
    };
  }

  const ua = raw.length > MAX_USER_AGENT_LENGTH ? raw.slice(0, MAX_USER_AGENT_LENGTH) : raw;
  return {
    userAgent: ua,
    browser: resolveBrowser(ua),
    operatingSystem: resolveOperatingSystem(ua),
    deviceType: resolveDeviceType(ua),
  };
}

/**
 * 读取当前浏览器 navigator.userAgent 并解析
 * @returns {TaktClientProfile} 客户端画像
 */
export function resolveTaktClientProfile(): TaktClientProfile {
  const userAgent = typeof navigator !== 'undefined' ? navigator.userAgent : '';
  return parseTaktClientProfile(userAgent);
}

/**
 * 构建 HTTP 请求头（Axios / fetch）
 * @returns {Record<string, string>} 客户端画像请求头
 */
export function buildTaktClientProfileHeaders(): Record<string, string> {
  const profile = resolveTaktClientProfile();
  const headers: Record<string, string> = {};
  if (profile.userAgent) {
    headers[TaktClientProfileHeaderNames.userAgent] = profile.userAgent;
  }
  if (profile.browser !== TaktBrowserType.Unknown) {
    headers[TaktClientProfileHeaderNames.browser] = profile.browser;
  }
  if (profile.operatingSystem !== TaktOperatingSystem.Unknown) {
    headers[TaktClientProfileHeaderNames.operatingSystem] = profile.operatingSystem;
  }
  if (profile.deviceType !== TaktDeviceType.Unknown) {
    headers[TaktClientProfileHeaderNames.deviceType] = profile.deviceType;
  }
  return headers;
}

/**
 * 构建 SignalR WebSocket 查询参数（无法携带自定义 Header 时使用）
 * @returns {Record<string, string>} 查询参数键值
 */
export function buildTaktClientProfileQueryParams(): Record<string, string> {
  const profile = resolveTaktClientProfile();
  const params: Record<string, string> = {};
  if (profile.userAgent) {
    params[TaktClientProfileQueryNames.userAgent] = profile.userAgent;
  }
  if (profile.browser !== TaktBrowserType.Unknown) {
    params[TaktClientProfileQueryNames.browser] = profile.browser;
  }
  if (profile.operatingSystem !== TaktOperatingSystem.Unknown) {
    params[TaktClientProfileQueryNames.operatingSystem] = profile.operatingSystem;
  }
  if (profile.deviceType !== TaktDeviceType.Unknown) {
    params[TaktClientProfileQueryNames.deviceType] = profile.deviceType;
  }
  return params;
}

/**
 * 解析浏览器
 * @param {string} userAgent User-Agent 原文
 * @returns {string} TaktBrowserType 常量值
 */
function resolveBrowser(userAgent: string): string {
  if (/Edg\/|Edge\//i.test(userAgent)) {
    return TaktBrowserType.Edge;
  }
  if (/Firefox\//i.test(userAgent)) {
    return TaktBrowserType.Firefox;
  }
  if (/Chrome\/|CriOS\//i.test(userAgent)) {
    return TaktBrowserType.Chrome;
  }
  if (/Safari\//i.test(userAgent)) {
    return TaktBrowserType.Safari;
  }
  return TaktBrowserType.Unknown;
}

/**
 * 解析操作系统
 * @param {string} userAgent User-Agent 原文
 * @returns {string} TaktOperatingSystem 常量值
 */
function resolveOperatingSystem(userAgent: string): string {
  if (/iPhone|iPad|iPod/i.test(userAgent)) {
    return TaktOperatingSystem.Ios;
  }
  if (/Android/i.test(userAgent)) {
    return TaktOperatingSystem.Android;
  }
  if (/Windows/i.test(userAgent)) {
    return TaktOperatingSystem.Windows;
  }
  if (/Mac OS X|Macintosh/i.test(userAgent)) {
    return TaktOperatingSystem.MacOs;
  }
  if (/Linux/i.test(userAgent)) {
    return TaktOperatingSystem.Linux;
  }
  return TaktOperatingSystem.Unknown;
}

/**
 * 解析登录设备
 * @param {string} userAgent User-Agent 原文
 * @returns {string} TaktDeviceType 常量值
 */
function resolveDeviceType(userAgent: string): string {
  if (/iPad|Tablet/i.test(userAgent)) {
    return TaktDeviceType.Tablet;
  }
  if (/Mobile|iPhone|iPod|Android/i.test(userAgent)) {
    return TaktDeviceType.Mobile;
  }
  return TaktDeviceType.Pc;
}
