// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktPaged.ts
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：分页参数访问门面（转发 config/taktPagination；运维只改 appsettings Paged）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export {
  loadTaktPaginationConfig,
  ensureTaktPaginationConfigAsync,
  resetTaktPaginationConfig,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
  getTaktMaxPageSize,
  getTaktPageSizeOptions,
  createDefaultPagedQuery,
} from '@/config/taktPagination';
