// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktGsap.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：GSAP 应用级初始化（插件注册一次；组件内动画用 composables/useGsap）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import gsap from 'gsap';
import { DrawSVGPlugin } from 'gsap/DrawSVGPlugin';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
import { SplitText } from 'gsap/SplitText';

/** 是否已完成 GSAP 初始化 */
let initialized = false;

/**
 * 注册 GSAP 插件（仅客户端、仅一次）
 * @description 须在 main.ts 挂载前调用；插件勿在组件内重复 registerPlugin
 */
export function initTaktGsap(): void {
  if (typeof window === 'undefined' || initialized) {
    return;
  }
  gsap.registerPlugin(ScrollTrigger, SplitText, DrawSVGPlugin);
  initialized = true;
}

export { gsap, ScrollTrigger, SplitText, DrawSVGPlugin };
