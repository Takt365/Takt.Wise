// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api
// 文件名称：health.api.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：后端根路径健康检查（预热 Cookie，不走 /api 业务包装）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { requireViteEnv } from '@/config/viteEnv';

/**
 * 解析健康检查完整 URL（后端 Program.cs MapGet("/health")，不在 /api 下）
 * @returns {string} 可 fetch 的地址
 */
function resolveHealthCheckUrl(): string {
  const apiBase = requireViteEnv('VITE_API_BASE_URL').replace(/\/$/, '');

  if (apiBase.startsWith('http://') || apiBase.startsWith('https://')) {
    const root = apiBase.replace(/\/api$/i, '');
    return `${root}/health`;
  }

  return '/health';
}

/**
 * 预热会话 Cookie（匿名 GET /health；失败由调用方静默处理）
 * @returns {Promise<void>}
 */
export async function probeHealthAsync(): Promise<void> {
  const response = await fetch(resolveHealthCheckUrl(), {
    method: 'GET',
    credentials: 'include',
  });

  if (!response.ok) {
    throw new Error(`Health check failed: ${response.status}`);
  }
}
