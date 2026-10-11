// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：tableDataClone.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆类型（对应 TaktTableClone DTO）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 单张表克隆项
 * @description 对应后端 TaktTableCloneItemDto
 */
export interface TableCloneItem {
  sourceTableName: string;
  targetTableName: string;
}

/**
 * 跨租户整表克隆请求
 * @description 对应后端 TaktTableCloneDto
 */
export interface TableCloneRequest {
  sourceTenantCode: string;
  sourceDatabaseName: string;
  targetTenantCode: string;
  targetDatabaseName: string;
  tables: TableCloneItem[];
  preserveIdentityValues?: boolean;
  confirmTargetBackupAndClear?: boolean;
}

/**
 * 单张目标表备份预览项
 */
export interface TableCloneTargetPreviewItem {
  targetTableName: string;
  targetRowCount: number;
  plannedBackupTableName: string;
  backupDescription: string;
  clearDescription: string;
  warningMessage: string;
}

/**
 * 跨租户整表克隆备份预览
 */
export interface TableClonePreview {
  summaryMessage: string;
  confirmHint: string;
  targets: TableCloneTargetPreviewItem[];
}

/**
 * 单张表克隆结果
 */
export interface TableCloneTableResult {
  sourceTableName: string;
  targetTableName: string;
  backupTableName: string;
  backedUpRowCount: number;
  clearedRowCount: number;
  backupSummaryMessage: string;
  sourceRowCount: number;
  clonedRowCount: number;
  commonColumnCount: number;
  commonColumns: string[];
  skippedSourceColumns: string[];
  skippedTargetColumns: string[];
}

/**
 * 跨租户整表克隆批量结果
 */
export interface TableCloneResult {
  tableCount: number;
  totalSourceRowCount: number;
  totalClonedRowCount: number;
  tables: TableCloneTableResult[];
}
