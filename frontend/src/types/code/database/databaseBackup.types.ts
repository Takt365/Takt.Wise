// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/code/database
// 文件名称：databaseBackup.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktCompanyDtoBase, TaktPagedQuery } from '@/types/common.types'

/** 数据库备份记录 */
export interface DatabaseBackup extends TaktCompanyDtoBase {
  databaseBackupId: string
  backupCode: string
  targetTenantCode: string
  targetDatabaseName: string
  /** 1=Full 2=Delta */
  backupType: number
  /** 1=立即 2=后台 */
  executeMode: number
  backupPath: string
  backupFileName?: string
  scheduledAt?: string
  startedAt?: string
  finishedAt?: string
  fileSizeBytes?: string
  quartzTaskId?: string
  errorMessage?: string
  /** 0待执行 1执行中 2成功 3失败 4已调度 */
  backupStatus: number
}

/** 列表查询 */
export interface DatabaseBackupQuery extends TaktPagedQuery {
  targetTenantCode?: string
  targetDatabaseName?: string
  backupType?: number
  backupStatus?: number
  backupCode?: string
}

/** 立即 / 后台备份请求 */
export interface DatabaseBackupRunDto {
  targetTenantCode: string
  targetDatabaseName: string
  backupType: number
  backupPath: string
  remark?: string
  scheduledAt?: string
}

/** 备份路径选项 */
export interface DatabaseBackupPathOptions {
  defaultRoot: string
  allowedRoots: string[]
}
