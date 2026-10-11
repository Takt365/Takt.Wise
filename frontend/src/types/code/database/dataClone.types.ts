// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：dataClone.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 公司级数据克隆请求 DTO（一次仅一个源公司、一张源表 → 一个目标公司、一张目标表）
 * 对应前端 DataClone
 * @description 对应后端 TaktDataCloneDto
 */
export interface DataClone {
  /**
   * 源租户编码（3 位）
   */
  sourceTenantCode: string;

  /**
   * 源数据库展示名
   */
  sourceDatabaseName: string;

  /**
   * 源物理表名
   */
  sourceTableName: string;

  /**
   * 源公司编码（4 位）
   */
  sourceCompanyCode: string;

  /**
   * 目标租户（3 位）
   */
  targetTenantCode: string;

  /**
   * 目标数据库展示名
   */
  targetDatabaseName: string;

  /**
   * 目标物理表名
   */
  targetTableName: string;

  /**
   * 目标公司编码（4 位）
   */
  targetCompanyCode: string;

  /**
   * 是否保留自增列原值（IDENTITY_INSERT）
   */
  preserveIdentityValues: boolean;

  /**
   * 已在备份窗口确认：目标公司将先备份再清空（执行克隆时必须为 true）
   */
  confirmTargetBackupAndClear: boolean;

}

/**
 * 公司级数据克隆备份预览 DTO（备份窗口）
 * 对应前端 DataClonePreview
 * @description 对应后端 TaktDataClonePreviewDto
 */
export interface DataClonePreview {
  /**
   * 目标物理表名
   */
  targetTableName: string;

  /**
   * 目标公司编码
   */
  targetCompanyCode: string;

  /**
   * 目标公司现有行数
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

  /**
   * 确认提示
   */
  confirmHint: string;

}

/**
 * 公司级数据克隆结果 DTO
 * 对应前端 DataCloneResult
 * @description 对应后端 TaktDataCloneResultDto
 */
export interface DataCloneResult {
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
   * 源公司匹配行数
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

