// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：tableArchive.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 数据表归档（按表登记归档键与热库保留年数）
 * 对应前端 TaktTableArchiveDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TableArchive
 * @description 对应后端 TaktTableArchiveDto
 */
export interface TableArchive extends CompanyDtoBase {
  /** 主键 */
  tableArchiveId: string;
  /** 目标租户 */
  targetTenantCode: string;
  /** 目标数据库展示名 */
  targetDatabaseName: string;
  /** 物理表名 */
  tableName: string;
  /** 归档键列 */
  archiveKeyColumn: string;
  /** 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等） */
  archiveKeyKind: number;
  /** 热库保留年数（固定为 1） */
  retainHotYears: number;
  /** 归档显示名 */
  archiveName: string;
  /** 排序号 */
  sortOrder: number;
  /** 状态（字典 sys_normal_disable；1=启用 0=禁用） */
  archiveStatus: number;
  /** 创建时间 */
  createdAt: string;
}

/** 分页查询 */
export interface TableArchiveQuery extends TaktPagedQuery {
  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  targetTenantCode?: string;
  targetDatabaseName?: string;
  tableName?: string;
  archiveKeyColumn?: string;
  archiveKeyKind?: number;
  retainHotYears?: number;
  archiveName?: string;
  sortOrder?: number;
  archiveStatus?: number;
  createdAtStart?: string;
  createdAtEnd?: string;
  extField?: string;
  remark?: string;
}

/** 创建 */
export interface TableArchiveCreate {
  tenantCode?: string;
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string
  targetTenantCode: string;
  targetDatabaseName: string;
  tableName: string;
  archiveKeyColumn: string;
  archiveKeyKind?: number;
  retainHotYears?: number;
  archiveName?: string;
  archiveStatus?: number;
  extField?: string;
  remark?: string;
}

/** 更新 */
export interface TableArchiveUpdate extends TableArchiveCreate {
  tableArchiveId: string;
}

/** 状态 */
export interface TableArchiveStatus {
  tableArchiveId: string;
  archiveStatus: number;
}

/** 排序 */
export interface TableArchiveSort {
  tableArchiveId: string;
  sortOrder: number;
}

/**
 * 按年归档预览/执行请求
 */
export interface TableArchiveExecuteDto {
  policyIds: string[];
  archiveYear: number;
}

/**
 * 单策略归档预览项
 */
export interface TableArchivePreviewItem {
  policyId: string;
  archiveName: string;
  tableName: string;
  archiveTableName: string;
  archiveYear: number;
  sourceRowCount: number;
}

/**
 * 归档预览聚合结果
 */
export interface TableArchivePreviewResult {
  items: TableArchivePreviewItem[];
  totalRowCount: number;
}

/**
 * 单策略归档执行项
 */
export interface TableArchiveExecuteItem {
  policyId: string;
  tableName: string;
  archiveTableName: string;
  archiveYear: number;
  sourceRowCount: number;
  archivedRowCount: number;
  deletedRowCount: number;
  success: boolean;
  errorMessage?: string | null;
}

/**
 * 归档执行聚合结果
 */
export interface TableArchiveExecuteResult {
  items: TableArchiveExecuteItem[];
}

/**
 * 预建年分表请求
 */
export interface TableEnsureYearTablesDto {
  policyId: string;
  years: number[];
}

/**
 * 预建年分表结果
 */
export interface TableEnsureYearTablesResult {
  policyId: string;
  tableName: string;
  yearTableNames: string[];
}

/**
 * 按年归档调度请求（立即/后台）
 */
export interface TableArchiveScheduleDto {
  policyIds: string[];
  archiveYear: number;
  /** 后台执行必填；立即执行可省略 */
  scheduledAt?: string | null;
}

/**
 * 按年归档调度结果
 */
export interface TableArchiveScheduleResult {
  quartzTaskId: string;
  taskCode: string;
  executeMode: number;
  scheduledAt: string;
  archiveYear: number;
  policyIds: string[];
}

