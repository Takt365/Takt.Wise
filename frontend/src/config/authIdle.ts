// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：authIdle.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：认证空闲超时配置（VITE_AUTH_IDLE_TIMEOUT_MINUTES / VITE_AUTH_IDLE_WARNING_MINUTES）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  TAKT_AUTH_IDLE_DEFAULT_TIMEOUT_MINUTES,
  TAKT_AUTH_IDLE_DEFAULT_WARNING_MINUTES,
} from '@/utils/common';

/**
 * 解析空闲自动登出超时时长（毫秒）
 * @description VITE_AUTH_IDLE_TIMEOUT_MINUTES：0 或未配置有效正数则禁用
 * @returns {number} 超时时长（毫秒），0 表示禁用
 */
export function getAuthIdleTimeoutMs(): number {
  /** 环境变量原始字符串（去首尾空白） */
  const raw = import.meta.env.VITE_AUTH_IDLE_TIMEOUT_MINUTES?.trim();

  // 未配置时使用仓库默认分钟数
  if (!raw) {
    return TAKT_AUTH_IDLE_DEFAULT_TIMEOUT_MINUTES * 60 * 1000;
  }

  // 显式 0 表示禁用空闲登出
  if (raw === '0') {
    return 0;
  }

  /** 解析后的分钟数 */
  const minutes = Number(raw);

  // 非正数或 NaN 视为禁用
  if (Number.isNaN(minutes) || minutes <= 0) {
    return 0;
  }

  // 分钟转毫秒
  return minutes * 60 * 1000;
}

/**
 * 解析空闲登出预警时长（分钟）
 * @description VITE_AUTH_IDLE_WARNING_MINUTES：0 禁用预警；须小于总超时时长
 * @returns {number} 预警分钟数，0 表示到期前不弹窗
 */
export function getAuthIdleWarningMinutes(): number {
  const totalMs = getAuthIdleTimeoutMs();
  if (totalMs <= 0) {
    return 0;
  }

  const totalMinutes = totalMs / 60_000;
  const raw = import.meta.env.VITE_AUTH_IDLE_WARNING_MINUTES?.trim();

  if (raw === '0') {
    return 0;
  }

  const minutes = raw ? Number(raw) : TAKT_AUTH_IDLE_DEFAULT_WARNING_MINUTES;
  if (Number.isNaN(minutes) || minutes <= 0) {
    return 0;
  }

  if (minutes >= totalMinutes) {
    return 0;
  }

  return minutes;
}

/**
 * 空闲登出预警时长（毫秒）
 * @returns {number} 毫秒，0 表示不预警
 */
export function getAuthIdleWarningMs(): number {
  return getAuthIdleWarningMinutes() * 60_000;
}

/**
 * 是否启用空闲自动登出
 * @returns {boolean} 超时时长大于 0 时为 true
 */
export function isAuthIdleLogoutEnabled(): boolean {
  // 毫秒阈值为 0 即功能关闭
  return getAuthIdleTimeoutMs() > 0;
}
