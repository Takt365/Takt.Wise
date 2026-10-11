// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：databaseInfo.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common.types';

/**
 * 数据库摘要（可连接租户业务库，与 appsettings Database:TenantCodes / ConnectionStrings:Tenant_* 对齐）
 * 对应前端 DatabaseInfo
 * @description 对应后端 TaktDatabaseInfoDto
 */
export interface DatabaseInfo {
  /**
   * 租户编码（3 位，如 000、100）
   */
  tenantCode: string;

  /**
   * 数据库展示名称（连接串 Database= 段，如 Takt_000_Dev）
   */
  displayName: string;

}

/**
 * 数据库表摘要（指定租户库下物理表 introspect 结果，用于选表导入）
 * 对应前端 DatabaseTableInfo
 * @description 对应后端 TaktDatabaseTableInfoDto
 */
export interface DatabaseTableInfo {
  /**
   * 数据表名称
   */
  tableName: string;

  /**
   * 表描述（表注释）
   */
  tableComment?: string;

}

/**
 * 数据库表摘要分页查询（租户取自当前登录上下文）
 * @description 对应后端 TaktDatabaseTableInfoQueryDto
 */
export interface DatabaseTableInfoQuery extends TaktPagedQuery {
}

/**
 * 数据库表列摘要（指定物理表列 introspect 结果）
 * 对应前端 DatabaseTableColumnInfo
 * @description 对应后端 TaktDatabaseTableColumnInfoDto
 */
export interface DatabaseTableColumnInfo {
  /**
   * 数据库列名称（snake_case，如 user_name）
   */
  databaseColumnName: string;

  /**
   * 列描述（列注释）
   */
  columnComment?: string;

  /**
   * 数据库数据类型（如 nvarchar、int、datetime）
   */
  databaseDataType: string;

  /**
   * 长度（字符串长度或数值整数位）
   */
  length: number;

  /**
   * 小数位数
   */
  decimalDigits: number;

  /**
   * 是否主键
   */
  isPrimaryKey: boolean;

  /**
   * 是否自增
   */
  isIdentity: boolean;

  /**
   * 是否可空
   */
  isNullable: boolean;

}

