// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：backup.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份执行配置类型（结果明细在备份日志 BackupLog，本类型仅配置 + 最近状态摘要）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

/** 数据库备份执行配置（对应 TaktDatabaseBackupDto） */
export interface DatabaseBackup extends CompanyDtoBase {
  /** 主键 */
  databaseBackupId: string;
  /** 备份编码 */
  backupCode: string;
  /** 目标租户 */
  targetTenantCode: string;
  /** 目标数据库 */
  targetDatabaseName: string;
  /** 备份类型 1=Full 2=Delta */
  backupType: number;
  /** 执行方式 1=立即 2=后台 */
  executeMode: number;
  /** 路径类型 1=本地 2=网络 3=FTP */
  backupPathType: number;
  /** 备份目录 */
  backupPath: string;
  /** 主机 */
  backupHost?: string;
  /** 端口 */
  backupPort?: number;
  /** 用户名 */
  backupUserName?: string;
  /** 是否已保存密码 */
  hasBackupPassword?: boolean;
  /** 备份文件名 */
  backupFileName: string;
  /** 计划执行时间 */
  scheduledAt?: string;
  /** 最近一次执行时间（摘要；明细见备份日志） */
  lastRunAt?: string;
  /** Quartz 任务 Id */
  quartzTaskId?: string;
  /** Quartz 任务名 */
  quartzTaskName?: string;
  /** 备份状态 0~4 */
  backupStatus: number;
}

/** 分页查询 */
export interface DatabaseBackupQuery extends TaktPagedQuery {
  backupCode?: string;
  targetTenantCode?: string;
  targetDatabaseName?: string;
  backupType?: number;
  backupPathType?: number;
  executeMode?: number;
  backupStatus?: number;
  backupPath?: string;
  backupFileName?: string;
  scheduledAtStart?: string;
  scheduledAtEnd?: string;
  lastRunAtStart?: string;
  lastRunAtEnd?: string;
  quartzTaskId?: string;
  createdAtStart?: string;
  createdAtEnd?: string;
  extField?: string;
  remark?: string;
}

/** 创建 */
export interface DatabaseBackupCreate {
  backupCode?: string;
  targetTenantCode: string;
  targetDatabaseName: string;
  backupType?: number;
  executeMode?: number;
  backupPathType?: number;
  backupPath: string;
  backupHost?: string;
  backupPort?: number;
  backupUserName?: string;
  backupPassword?: string;
  backupFileName?: string;
  scheduledAt?: string;
  backupStatus?: number;
  extField?: string;
  remark?: string;
}

/** 更新 */
export interface DatabaseBackupUpdate extends DatabaseBackupCreate {
  databaseBackupId: string;
}

/** 状态 */
export interface DatabaseBackupStatus {
  databaseBackupId: string;
  backupStatus: number;
}

/** 立即/调度执行请求（与 RunDto 对齐的前端载荷） */
export interface DatabaseBackupRun {
  targetTenantCode: string;
  targetDatabaseName: string;
  backupType: number;
  backupPathType: number;
  backupPath: string;
  backupHost?: string;
  backupPort?: number;
  backupUserName?: string;
  backupPassword?: string;
  backupFileName?: string;
  scheduledAt?: string;
  remark?: string;
}

/** 备份路径选项 */
export interface DatabaseBackupPathOptions {
  defaultRoot: string;
  allowedRoots: string[];
}

/** 目录浏览条目（同通用 TaktFolderExplorerItem） */
export type DatabaseBackupBrowseItem = import('@/types/components/folderExplorer.types').TaktFolderExplorerItem

/** 目录浏览结果（同通用 TaktFolderExplorerBrowseResult） */
export type DatabaseBackupBrowseResult = import('@/types/components/folderExplorer.types').TaktFolderExplorerBrowseResult

/** 按 Id 调度 */
export interface DatabaseBackupScheduleByIdDto {
  scheduledAt: string;
}
