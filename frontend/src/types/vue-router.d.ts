// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：vue-router.d.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：vue-router RouteMeta 模块扩展（须 import 原模块后再 augment）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import 'vue-router';

declare module 'vue-router' {
  interface RouteMeta {
    /**
     * 页面标题 i18n 键
     */
    titleKey?: string;
    /**
     * 是否需要登录
     */
    requiresAuth?: boolean;
    /**
     * 功能权限码
     */
    permission?: string;
  }
}
