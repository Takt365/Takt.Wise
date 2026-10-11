// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/statistics/quick-query/configurable/composables
// 文件名称：useConfigurableSchemaCatalog.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表选库选表与列目录（TaktConfigurables schema API）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getConfigurableSchemaColumns,
  getConfigurableSchemaDatabases,
  getConfigurableSchemaTables,
} from '@/api/statistics/quick-query/configurable.api';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import type { DatabaseInfo, DatabaseTableColumnInfo, DatabaseTableInfo } from '@/types/code/database/databaseInfo.types';

/**
 * 定制报表 schema 目录：租户库、物理表、列
 * @returns 目录状态与解析方法
 */
export function useConfigurableSchemaCatalog() {
  const tenantStore = useTenantStore();
  /** 租户业务库列表 */
  const databaseInfoList = ref<DatabaseInfo[]>([]);
  /** 库列表加载中 */
  const databaseInfoLoading = ref(false);
  /** 按 tenantCode 缓存的物理表列表 */
  const tablesByTenant = ref<Record<string, DatabaseTableInfo[]>>({});
  /** 物理表加载中的 tenantCode 集合 */
  const tablesLoadingTenants = ref<string[]>([]);
  /** 按 tenantCode:tableName 缓存的列列表 */
  const columnsByTableKey = ref<Record<string, DatabaseTableColumnInfo[]>>({});
  /** 列加载中的 tableKey 列表 */
  const columnsLoadingKeys = ref<string[]>([]);

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
      const list = await getConfigurableSchemaDatabases();
      databaseInfoList.value = list ?? [];
    } catch (error) {
      logger.error('[ConfigurableSchema] 加载业务库失败', { error });
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
      const list = await getConfigurableSchemaTables(code);
      const tables = list ?? [];
      tablesByTenant.value = { ...tablesByTenant.value, [code]: tables };
      return tables;
    } catch (error) {
      logger.error('[ConfigurableSchema] 加载物理表失败', { tenantCode: code, error });
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

  /**
   * 生成表缓存键
   * @param tenantCode 租户编码
   * @param tableName 表名
   * @returns 缓存键
   */
  function buildTableKey(tenantCode: string, tableName: string): string {
    const code = tenantCode?.trim();
    const table = tableName?.trim();
    if (!code || !table) {
      return '';
    }
    return `${code}:${table}`;
  }

  /**
   * 加载物理表列（带缓存）
   * @param tenantCode 租户编码
   * @param tableName 表名
   * @returns 列列表
   */
  async function loadColumnsForTable(tenantCode: string, tableName: string): Promise<DatabaseTableColumnInfo[]> {
    const key = buildTableKey(tenantCode, tableName);
    if (!key) {
      return [];
    }
    const cached = columnsByTableKey.value[key];
    if (cached) {
      return cached;
    }
    if (columnsLoadingKeys.value.includes(key)) {
      return columnsByTableKey.value[key] ?? [];
    }
    columnsLoadingKeys.value = [...columnsLoadingKeys.value, key];
    try {
      const list = await getConfigurableSchemaColumns(tenantCode.trim(), tableName.trim());
      const columns = list ?? [];
      columnsByTableKey.value = { ...columnsByTableKey.value, [key]: columns };
      return columns;
    } catch (error) {
      logger.error('[ConfigurableSchema] 加载列失败', { tenantCode, tableName, error });
      columnsByTableKey.value = { ...columnsByTableKey.value, [key]: [] };
      throw error;
    } finally {
      columnsLoadingKeys.value = columnsLoadingKeys.value.filter((item) => item !== key);
    }
  }

  /**
   * 获取已缓存的列列表
   * @param tenantCode 租户编码
   * @param tableName 表名
   * @returns 列列表
   */
  function getCachedColumns(tenantCode: string, tableName: string): DatabaseTableColumnInfo[] {
    const key = buildTableKey(tenantCode, tableName);
    if (!key) {
      return [];
    }
    return columnsByTableKey.value[key] ?? [];
  }

  /**
   * 列是否加载中
   * @param tenantCode 租户编码
   * @param tableName 表名
   * @returns 是否 loading
   */
  function isColumnsLoading(tenantCode: string, tableName: string): boolean {
    const key = buildTableKey(tenantCode, tableName);
    return !!key && columnsLoadingKeys.value.includes(key);
  }

  /**
   * 按数据源别名解析租户与物理表
   * @param sourceAlias 数据源别名
   * @param sourceRows 数据源子表行
   * @returns 租户与表名，无法解析时 null
   */
  function resolveSourceTable(
    sourceAlias: string,
    sourceRows: readonly Record<string, unknown>[]
  ): { tenantCode: string; tableName: string } | null {
    const alias = sourceAlias?.trim();
    if (!alias) {
      return null;
    }
    const matched = sourceRows.find((row) => String(row.sourceAlias ?? '').trim() === alias);
    if (!matched?.tableName) {
      return null;
    }
    const tenantCode = String(matched.__catalogTenantCode ?? tenantStore.tenantCode ?? '').trim();
    const tableName = String(matched.tableName ?? '').trim();
    if (!tenantCode || !tableName) {
      return null;
    }
    return { tenantCode, tableName };
  }

  /**
   * 按数据源别名加载列
   * @param sourceAlias 数据源别名
   * @param sourceRows 数据源子表行
   * @returns 列列表
   */
  async function loadColumnsForSourceAlias(
    sourceAlias: string,
    sourceRows: readonly Record<string, unknown>[]
  ): Promise<DatabaseTableColumnInfo[]> {
    const resolved = resolveSourceTable(sourceAlias, sourceRows);
    if (!resolved) {
      return [];
    }
    return loadColumnsForTable(resolved.tenantCode, resolved.tableName);
  }

  return {
    databaseInfoList,
    databaseInfoLoading,
    tablesByTenant,
    columnsByTableKey,
    loadDatabaseInfoList,
    loadTablesForTenant,
    resolveDatabaseDisplayName,
    isTablesLoading,
    buildTableKey,
    loadColumnsForTable,
    getCachedColumns,
    isColumnsLoading,
    resolveSourceTable,
    loadColumnsForSourceAlias,
  };
}
