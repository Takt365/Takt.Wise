// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFps.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：FPS 采样纯工具（rAF 窗口计算帧率）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 根据帧数与采样窗口计算 FPS
 * @param frameCount 窗口内帧数
 * @param elapsedMs 采样窗口毫秒
 * @returns {number} 帧/秒
 */
export function calculateFps(frameCount: number, elapsedMs: number): number {
  if (frameCount <= 0 || elapsedMs <= 0) {
    return 0;
  }
  return (frameCount * 1000) / elapsedMs;
}

/**
 * FPS 是否低于阈值（画面掉帧）
 * @param fps 当前帧率
 * @param threshold 阈值（默认 30）
 * @returns {boolean} 低于阈值为 true
 */
export function isFpsBelowThreshold(fps: number, threshold: number): boolean {
  if (Number.isNaN(fps) || fps <= 0) {
    return true;
  }
  return fps < threshold;
}
