// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktLoginLayoutDom.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：登录表单位置 localStorage 读写
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TAKT_LOGIN_LAYOUT_STORAGE_KEY } from '@/utils/common';
import type { TaktLoginLayoutPosition } from '@/types/common.types';

export { TAKT_LOGIN_LAYOUT_STORAGE_KEY };
export type { TaktLoginLayoutPosition };

/**
 * 读取已保存的登录表单位置
 * @param {TaktLoginLayoutPosition} [fallback='center'] 无有效缓存时的默认值
 * @returns {TaktLoginLayoutPosition} 左/中/右布局位置
 */
export function readStoredLoginLayoutPosition(
  fallback: TaktLoginLayoutPosition = 'center'
): TaktLoginLayoutPosition {
  /** localStorage 中持久化的布局值 */
  const stored = localStorage.getItem(TAKT_LOGIN_LAYOUT_STORAGE_KEY);

  // 仅接受约定的三种枚举值
  if (stored === 'left' || stored === 'center' || stored === 'right') {
    return stored;
  }

  // 非法或缺失时回退默认
  return fallback;
}

/**
 * 持久化登录表单位置
 * @param {TaktLoginLayoutPosition} position 左/中/右布局位置
 * @returns {void}
 */
export function saveLoginLayoutPosition(position: TaktLoginLayoutPosition): void {
  // 写入 localStorage，下次进入登录页恢复
  localStorage.setItem(TAKT_LOGIN_LAYOUT_STORAGE_KEY, position);
}
