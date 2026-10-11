// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktContentModalWidth.ts
// 创建时间：2026-09-01
// 创建人：Takt365(Cursor AI)
// 功能描述：弹出窗体宽度 =（当前视口宽度 − 左侧菜单栏宽度）× 80%；纯计算 + 实测侧栏
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 内容区弹窗宽度占「视口 − 左侧菜单」的比例 */
export const TAKT_CONTENT_MODAL_WIDTH_RATIO = 0.8

/** 弹窗宽度下限（px），避免侧栏过宽或视口过小时不可用 */
export const TAKT_CONTENT_MODAL_WIDTH_MIN_PX = 480

/**
 * 实测左侧菜单栏宽度（.layout-sider；无侧栏布局为 0）
 * @param root 查询根；缺省为 document；SSR/无 document 时返回 0
 * @returns 侧栏像素宽度
 */
export function measureTaktLeftSiderWidth(root?: ParentNode | null): number {
  const scope = root ?? (typeof document !== 'undefined' ? document : null)
  if (scope == null) {
    return 0
  }
  const sider = scope.querySelector('.layout-sider') as HTMLElement | null
  if (sider == null) {
    return 0
  }
  const width = sider.getBoundingClientRect().width
  if (!Number.isFinite(width) || width <= 0) {
    return 0
  }
  return Math.round(width)
}

/**
 * 计算内容区弹窗宽度：（视口宽度 − 左侧菜单宽度）× 比例
 * @param viewportWidth 当前视口宽度（window.innerWidth）
 * @param siderWidth 左侧菜单栏宽度（px）
 * @param ratio 占比，默认 0.8
 * @returns 弹窗宽度（px），不低于 TAKT_CONTENT_MODAL_WIDTH_MIN_PX
 */
export function computeTaktContentModalWidthPx(
  viewportWidth: number,
  siderWidth: number,
  ratio: number = TAKT_CONTENT_MODAL_WIDTH_RATIO,
): number {
  const vw = Number.isFinite(viewportWidth) && viewportWidth > 0 ? viewportWidth : 0
  const sw = Number.isFinite(siderWidth) && siderWidth > 0 ? siderWidth : 0
  const usable = Math.max(0, vw - sw)
  const r = Number.isFinite(ratio) && ratio > 0 ? ratio : TAKT_CONTENT_MODAL_WIDTH_RATIO
  return Math.max(TAKT_CONTENT_MODAL_WIDTH_MIN_PX, Math.floor(usable * r))
}
