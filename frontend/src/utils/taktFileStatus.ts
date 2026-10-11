// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils
// 文件名称：taktFileStatus.ts
// 创建时间：2026-06-13
// 创建人：Takt
// 功能描述：文件状态常量与判定（字典 sys_normal_disable；与后端 TaktFileHelper 对齐）
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 禁用（sys_normal_disable=0） */
export const TAKT_FILE_STATUS_DISABLED = 0;

/** 启用（sys_normal_disable=1） */
export const TAKT_FILE_STATUS_ENABLED = 1;

/** 锁定（sys_normal_disable=2） */
export const TAKT_FILE_STATUS_LOCKED = 2;

/**
 * 是否为启用态（仅 1 可下载）
 * @param value 文件状态
 * @returns 启用返回 true
 */
export function isFileStatusEnabled(value: unknown): boolean {
  return normalizeFileStatus(value) === TAKT_FILE_STATUS_ENABLED;
}

/**
 * 规范化文件状态；非法值回退为启用（1）
 * @param value 表单或 API 值
 * @returns 0 / 1 / 2
 */
export function normalizeFileStatus(value: unknown): number {
  const num = typeof value === 'number' ? value : Number(value);
  if (num === TAKT_FILE_STATUS_DISABLED || num === TAKT_FILE_STATUS_ENABLED || num === TAKT_FILE_STATUS_LOCKED) {
    return num;
  }
  return TAKT_FILE_STATUS_ENABLED;
}
