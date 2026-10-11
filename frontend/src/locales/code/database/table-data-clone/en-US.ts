// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/table-data-clone
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/table-data-clone page static copy; keys code.database.table-data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "Cross-Tenant Table Clone",
    subtitle: "Cross-tenant only; 1–5 tables per request; target tables are fully backed up then TRUNCATE cleared before clone",
    section: {
      scope: "Tenants and Databases",
      tables: "Table Mapping (1–5)",
      options: "Clone Options",
      result: "Execution Result",
    },
    field: {
      sourcetenant: "Source Tenant",
      targettenant: "Target Tenant",
      sourcedatabase: "Source Database",
      targetdatabase: "Target Database",
      sourcetable: "Source Table",
      targettable: "Target Table",
      preserveidentity: "Preserve identity column values (IDENTITY_INSERT)",
    },
    tablemapping: {
      addrow: "Add Table",
      removerow: "Remove",
      maxhint: "Up to 5 tables per request",
      actioncolumn: "Action",
    },
    crosstenantrequired: "Table clone requires different source and target tenants",
    tablerequired: "Configure at least one table mapping",
    action: {
      startclone: "Start Clone",
      confirmexecute: "Confirm and Execute",
    },
    backupmodaltitle: "Backup Window — Confirm Before Execute",
    clonesuccess: "Clone succeeded",
    clonefailed: "Clone failed",
    previewrequired: "Open the backup window and confirm before executing",
    preview: {
      confirmhint: "I have read the backup window notice and confirm target tables will be fully backed up then TRUNCATE cleared",
      summary: "{count} target table(s) will be fully backed up before clone, then TRUNCATE cleared.",
      warning: "Warning: Before clone, all data in target table {tableName} will be backed up then cleared. This cannot be undone.",
      backupwithrows: "Step 1: Back up all {rowCount} rows from target table {tableName} to {backupTable}",
      backupempty: "Step 1: Target table {tableName} has no data; create empty-structure backup table {backupTable}",
      cleartruncate: "Step 2: TRUNCATE all data in target table {tableName} ({rowCount} rows)",
    },
    result: {
      backuptable: "Backup Table",
      backeduprows: "Backed Up Rows",
      clearedrows: "Cleared Rows",
      sourcerows: "Source Rows",
      clonedrows: "Cloned Rows",
      commoncolumns: "Common Columns",
      summary: "Backup and Clear Summary",
      targetrows: "Target Existing Rows",
      plannedbackuptable: "Planned Backup Table",
      totalsourcerows: "Total Source Rows",
      totalclonedrows: "Total Cloned Rows",
      tablecount: "Table Count",
    },
  },
};
