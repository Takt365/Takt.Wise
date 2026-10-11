// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：platform.api.ts
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：平台公开配置 API（分页等，对应 appsettings Paged）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktPaginationConfig } from '@/types/common.types';

/** API 路径前缀（与 TaktWiseController 一致） */
const PLATFORM_API_BASE = 'TaktWise';

/**
 * 获取分页全局配置（来源 appsettings Paged）
 * @returns {Promise<TaktPaginationConfig>} 分页配置
 */
export function getPlatformPaginationConfig(): Promise<TaktPaginationConfig> {
  return request<TaktPaginationConfig>({
    url: `${PLATFORM_API_BASE}/pagination`,
    method: 'get',
    skipTokenRefresh: true,
  });
}
