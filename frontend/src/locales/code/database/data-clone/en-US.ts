// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/data-clone
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：code/database/data-clone page static copy; keys code.database.data-clone.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "Company Data Clone",
    subtitle: "One source company and one table to one target company and one table; target company data is backed up then cleared before clone",
    section: {
      source: "Source Scope",
      target: "Target Scope",
      options: "Clone Options",
      result: "Execution Result",
    },
    field: {
      tenant: "Tenant",
      database: "Database",
      table: "Table",
      company: "Company Code",
      preserveidentity: "Preserve identity column values (IDENTITY_INSERT)",
    },
    action: {
      startclone: "Start Clone",
      confirmexecute: "Confirm and Execute",
    },
    backupmodaltitle: "Backup Window — Confirm Before Execute",
    backupsummary: "Backup and Clear Steps",
    samescopeerror: "Source and target tenant, database, table, and company code cannot be identical",
    previewrequired: "Open the backup window and confirm before executing",
    clonesuccess: "Clone succeeded",
    clonefailed: "Clone failed",
    preview: {
      confirmhint: "I have read the backup window notice and confirm the target company data will be backed up then cleared",
      warning: "Warning: Before clone, data for company {companyCode} in target table {tableName} will be backed up then cleared. This cannot be undone.",
      backupwithrows: "Step 1: Back up all {rowCount} rows for company {companyCode} in target table {tableName} to {backupTable}",
      backupempty: "Step 1: No data for company {companyCode} in target table {tableName}; create empty-structure backup table {backupTable}",
      cleardelete: "Step 2: Delete all data for company {companyCode} in target table {tableName} ({rowCount} rows)",
      resultbackupwithrows: "Backed up {rowCount} rows for company {companyCode} in target table {tableName} to {backupTable}",
      resultbackupempty: "No data for company {companyCode} in target table {tableName}; created empty-structure backup table {backupTable}",
      resultdeleted: "Deleted all {rowCount} rows for company {companyCode} in target table {tableName}",
      resultsummary: "{backupPart}; {clearPart}",
    },
    result: {
      backuptable: "Backup Table",
      backeduprows: "Backed Up Rows",
      clearedrows: "Cleared Rows",
      sourcerows: "Source Company Rows",
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
