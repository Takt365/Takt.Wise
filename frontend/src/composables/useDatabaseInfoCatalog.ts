// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useDatabaseInfoCatalog.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：租户业务库与物理表目录加载（供 database 克隆页选库选表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { ref } from 'vue';
import { getDatabaseInfoList, getDatabaseTableInfoList } from '@/api/code/database/databaseInfo.api';
import type { DatabaseInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types';

/**
 * 加载可 introspect 的租户库列表与按租户缓存的物理表列表
 * @returns 目录状态与加载方法
 */
export function useDatabaseInfoCatalog() {
  /** 租户业务库列表 */
  const databaseInfoList = ref<DatabaseInfo[]>([]);
  /** 库列表加载中 */
  const databaseInfoLoading = ref(false);
  /** 按 tenantCode 缓存的物理表列表 */
  const tablesByTenant = ref<Record<string, DatabaseTableInfo[]>>({});
  /** 物理表加载中的 tenantCode 集合 */
  const tablesLoadingTenants = ref<string[]>([]);

  /**
   * 加载租户业务库列表
   * @returns {Promise<void>}
   */
  async function loadDatabaseInfoList(): Promise<void> {
    if (databaseInfoLoading.value) {
      return;
    }
    databaseInfoLoading.value = true;
    try {
      const list = await getDatabaseInfoList();
      databaseInfoList.value = list ?? [];
    } catch (error) {
      logger.error('[DatabaseInfoCatalog] 加载业务库失败', { error });
      databaseInfoList.value = [];
      throw error;
    } finally {
      databaseInfoLoading.value = false;
    }
  }

  /**
   * 按租户编码加载物理表列表（带缓存）
   * @param tenantCode 租户编码（3 位）
   * @returns {Promise<DatabaseTableInfo[]>} 物理表列表
   */
  async function loadTablesForTenant(tenantCode: string): Promise<DatabaseTableInfo[]> {
    const code = tenantCode?.trim();
    if (!code) {
      return [];
    }
    const cached = tablesByTenant.value[code];
    if (cached) {
      return cached;
    }
    if (tablesLoadingTenants.value.includes(code)) {
      return tablesByTenant.value[code] ?? [];
    }
    tablesLoadingTenants.value = [...tablesLoadingTenants.value, code];
    try {
      const list = await getDatabaseTableInfoList(code);
      const tables = [...(list ?? [])].sort((a, b) =>
        String(a.tableName ?? '').localeCompare(String(b.tableName ?? ''), undefined, { sensitivity: 'base' }),
      );
      tablesByTenant.value = { ...tablesByTenant.value, [code]: tables };
      return tables;
    } catch (error) {
      logger.error('[DatabaseInfoCatalog] 加载物理表失败', { tenantCode: code, error });
      tablesByTenant.value = { ...tablesByTenant.value, [code]: [] };
      throw error;
    } finally {
      tablesLoadingTenants.value = tablesLoadingTenants.value.filter((item) => item !== code);
    }
  }

  /**
   * 根据租户编码解析数据库展示名
   * @param tenantCode 租户编码
   * @returns 连接串 Database= 段展示名
   */
  function resolveDatabaseDisplayName(tenantCode: string): string {
    const code = tenantCode?.trim();
    if (!code) {
      return '';
    }
    return databaseInfoList.value.find((item) => item.tenantCode === code)?.displayName ?? '';
  }

  /**
   * 判断指定租户物理表是否加载中
   * @param tenantCode 租户编码
   * @returns 是否 loading
   */
  function isTablesLoading(tenantCode: string): boolean {
    const code = tenantCode?.trim();
    return !!code && tablesLoadingTenants.value.includes(code);
  }

  return {
    databaseInfoList,
    databaseInfoLoading,
    tablesByTenant,
    loadDatabaseInfoList,
    loadTablesForTenant,
    resolveDatabaseDisplayName,
    isTablesLoading,
  };
}
