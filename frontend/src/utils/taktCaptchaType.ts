// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktCaptchaType.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：验证码类型常量（与后端 TaktCaptchaTypeNames、Captcha:Type 一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktCaptchaTypeValue } from '@/types/identity/captcha.types';

/**
 * 验证码类型名称（运行时常量，用于与 API 返回 captchaType 比较）
 */
export const TaktCaptchaType = {
  /** 滑块拼图验证码 */
  Slider: 'Slider',
  /** 行为拖动验证码 */
  Behavior: 'Behavior',
} as const satisfies Record<string, TaktCaptchaTypeValue>;

/**
 * 判断挑战是否为滑块拼图类型
 * @param {string | undefined} captchaType API 返回的类型字段
 * @returns {boolean} 是否为 Slider
 */
export function isTaktCaptchaSliderType(captchaType: string | undefined): boolean {
  // 与后端 TaktCaptchaTypeNames.Slider 字符串比较
  return captchaType === TaktCaptchaType.Slider;
}

/**
 * 判断挑战是否为行为验证码类型
 * @param {string | undefined} captchaType API 返回的类型字段
 * @returns {boolean} 是否为 Behavior
 */
export function isTaktCaptchaBehaviorType(captchaType: string | undefined): boolean {
  // 与后端 TaktCaptchaTypeNames.Behavior 字符串比较
  return captchaType === TaktCaptchaType.Behavior;
}
