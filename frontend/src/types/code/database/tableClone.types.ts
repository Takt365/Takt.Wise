// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：tableClone.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 跨租户整表数据克隆请求 DTO（同租户内禁止；一次 1~5 张表）
 * 对应前端 TableClone
 * @description 对应后端 TaktTableCloneDto
 */
export interface TableClone {
  /**
   * 源租户编码（3 位）
   */
  sourceTenantCode: string;

  /**
   * 源数据库展示名
   */
  sourceDatabaseName: string;

  /**
   * 目标租户（3 位）
   */
  targetTenantCode: string;

  /**
   * 目标数据库展示名
   */
  targetDatabaseName: string;

  /**
   * 待克隆表清单（1~5 张）
   */
  tables: TableCloneItem[];

  /**
   * 是否保留自增列原值（IDENTITY_INSERT）
   */
  preserveIdentityValues: boolean;

  /**
   * 已在备份窗口确认：目标表将先全量备份再 TRUNCATE 清空（执行克隆时必须为 true）
   */
  confirmTargetBackupAndClear: boolean;

}

/**
 * 单张表的跨租户克隆项
 * 对应前端 TableCloneItem
 * @description 对应后端 TaktTableCloneItemDto
 */
export interface TableCloneItem {
  /**
   * 源物理表名
   */
  sourceTableName: string;

  /**
   * 目标物理表名
   */
  targetTableName: string;

}

/**
 * 跨租户整表克隆备份预览 DTO（备份窗口）
 * 对应前端 TableClonePreview
 * @description 对应后端 TaktTableClonePreviewDto
 */
export interface TableClonePreview {
  /**
   * 总体提示
   */
  summaryMessage: string;

  /**
   * 确认提示（执行克隆前须阅读并勾选 ConfirmTargetBackupAndClear）
   */
  confirmHint: string;

  /**
   * 各目标表备份/清空预览
   */
  targets: TableCloneTargetPreviewItem[];

}

/**
 * 单张目标表备份预览项
 * 对应前端 TableCloneTargetPreviewItem
 * @description 对应后端 TaktTableCloneTargetPreviewItemDto
 */
export interface TableCloneTargetPreviewItem {
  /**
   * 目标物理表名
   */
  targetTableName: string;

  /**
   * 目标表现有行数
   */
  targetRowCount: number;

  /**
   * 计划备份表名
   */
  plannedBackupTableName: string;

  /**
   * 备份步骤说明
   */
  backupDescription: string;

  /**
   * 清空步骤说明
   */
  clearDescription: string;

  /**
   * 风险提示
   */
  warningMessage: string;

}

/**
 * 跨租户整表克隆批量结果 DTO
 * 对应前端 TableCloneResult
 * @description 对应后端 TaktTableCloneResultDto
 */
export interface TableCloneResult {
  /**
   * 本次克隆表数量
   */
  tableCount: number;

  /**
   * 源表行数合计
   */
  totalSourceRowCount: number;

  /**
   * 写入目标表行数合计
   */
  totalClonedRowCount: number;

  /**
   * 各表克隆明细
   */
  tables: TableCloneTableResult[];

}

/**
 * 单张表克隆结果 DTO
 * 对应前端 TableCloneTableResult
 * @description 对应后端 TaktTableCloneTableResultDto
 */
export interface TableCloneTableResult {
  /**
   * 源物理表名
   */
  sourceTableName: string;

  /**
   * 目标物理表名
   */
  targetTableName: string;

  /**
   * 备份表名
   */
  backupTableName: string;

  /**
   * 备份行数
   */
  backedUpRowCount: number;

  /**
   * 清空行数
   */
  clearedRowCount: number;

  /**
   * 备份与清空摘要
   */
  backupSummaryMessage: string;

  /**
   * 源表行数
   */
  sourceRowCount: number;

  /**
   * 实际写入目标表行数
   */
  clonedRowCount: number;

  /**
   * 参与映射的同名列数量
   */
  commonColumnCount: number;

  /**
   * 参与 INSERT 的同名列
   */
  commonColumns: string[];

  /**
   * 源表存在但目标表未映射的列
   */
  skippedSourceColumns: string[];

  /**
   * 目标表存在但源表未映射的列
   */
  skippedTargetColumns: string[];

}

