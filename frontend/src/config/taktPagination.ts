// ========================================

// 项目名称：节拍智控·TaktWise

// 命名空间：frontend/src/config

// 文件名称：taktPagination.ts

// 创建时间：2026-06-14

// 创建人：Takt365(Cursor AI)

// 功能描述：分页运行时网关（租户上下文就绪后从后端 appsettings Paged 拉取）

//

// 版权信息：Copyright (c) 2025 Takt  All rights reserved.

// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。

// ========================================



import { getPlatformPaginationConfig } from '@/api/foundation/platform.api';

import { useTenantStore } from '@/stores/identity/useTenantStore';

import type { TaktPagedQuery, TaktPaginationConfig } from '@/types/common.types';



/** 是否已完成 bootstrap 拉取 */

let configured = false;



/** 运行时分页配置（与 appsettings Paged 一致） */

let runtimeConfig: TaktPaginationConfig | null = null;



/** 进行中的拉取 Promise（防并发重复请求） */

let loadPromise: Promise<void> | null = null;



/**

 * 断言分页配置已加载（须先 ensureTaktPaginationConfigAsync）

 */

function assertPaginationConfigured(): void {

  if (!configured || runtimeConfig == null) {

    throw new Error('分页配置未加载：请先选择租户并 await ensureTaktPaginationConfigAsync()');

  }

}



/**

 * 拉取分页配置（须 tenantCode 已写入 Pinia / 请求头）

 * @returns {Promise<void>}

 */

export async function loadTaktPaginationConfig(): Promise<void> {

  const tenantCode = useTenantStore().tenantCode?.trim();

  if (!tenantCode) {

    return;

  }



  if (configured) {

    return;

  }



  const config = await getPlatformPaginationConfig();

  runtimeConfig = {

    defaultPageIndex: config.defaultPageIndex,

    defaultPageSize: config.defaultPageSize,

    maxPageSize: config.maxPageSize,

    pageSizeOptions: config.pageSizeOptions.map(String),

  };

  configured = true;

}



/**

 * 租户就绪后确保分页配置已加载（登录页选租户 / 已登录恢复上下文后调用）

 * @returns {Promise<void>}

 */

export async function ensureTaktPaginationConfigAsync(): Promise<void> {

  const tenantCode = useTenantStore().tenantCode?.trim();

  if (!tenantCode) {

    return;

  }



  if (configured) {

    return;

  }



  if (!loadPromise) {

    loadPromise = loadTaktPaginationConfig().finally(() => {

      loadPromise = null;

    });

  }



  await loadPromise;

}



/**

 * 登出或租户清空时重置（下次选租户后重新拉取）

 */

export function resetTaktPaginationConfig(): void {

  configured = false;

  runtimeConfig = null;

  loadPromise = null;

}



/**

 * 默认页码（从 1 开始）

 * @returns {number}

 */

export function getTaktDefaultPageIndex(): number {

  assertPaginationConfigured();

  return runtimeConfig!.defaultPageIndex;

}



/**

 * 默认每页条数

 * @returns {number}

 */

export function getTaktDefaultPageSize(): number {

  assertPaginationConfigured();

  return runtimeConfig!.defaultPageSize;

}



/**

 * 列表 pageSize 上限

 * @returns {number}

 */

export function getTaktMaxPageSize(): number {

  assertPaginationConfigured();

  return runtimeConfig!.maxPageSize;

}



/**

 * TaktPagination 可选每页条数

 * @returns {readonly string[]}

 */

export function getTaktPageSizeOptions(): readonly string[] {

  assertPaginationConfigured();

  return runtimeConfig!.pageSizeOptions;

}



/**

 * 构造默认分页查询 DTO（pageIndex + pageSize）

 * @param overrides 覆盖字段

 * @returns {TaktPagedQuery} 与后端 [FromQuery] 扁平参数一致

 */

export function createDefaultPagedQuery(overrides?: Partial<TaktPagedQuery>): TaktPagedQuery {

  return {

    pageIndex: getTaktDefaultPageIndex(),

    pageSize: getTaktDefaultPageSize(),

    ...overrides,

  };

}

