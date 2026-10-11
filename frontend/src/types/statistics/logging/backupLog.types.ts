// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：backupLog.types.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 备份日志（完整审计）
 * 对应前端 TaktBackupLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BackupLog
 * @description 对应后端 TaktBackupLogDto
 */
export interface BackupLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 备份种类（小写，如 database / file / config）
   */
  backupKind: string;

  /**
   * 来源业务键（备份配置 Id、任务号等，统一字符串）
   */
  sourceId: string;

  /**
   * 来源编码快照（配置编码、任务编码等）
   */
  sourceCode: string;

  /**
   * 目标名称（库展示名、目标标签等）
   */
  targetName: string;

  /**
   * 目标范围（可选；如租户码、公司码、路径根等）
   */
  targetScope: string;

  /**
   * 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
   */
  syncMode: number;

  /**
   * 执行方式快照（1=立即 2=后台）
   */
  executeMode: number;

  /**
   * 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
   */
  pathType: number;

  /**
   * 执行后结果路径
   */
  resultPath?: string;

  /**
   * 结果大小（字节）
   */
  fileSizeBytes: string;

  /**
   * 运行状态（0=进行中 1=成功 2=失败）
   */
  runStatus: number;

  /**
   * 失败错误信息
   */
  errorMessage?: string;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  finishedAt?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * 更新BackupLog DTO
 * 继承 TaktBackupLogCreateDto，添加 BackupLogId 字段
 * 对应前端 BackupLogUpdate
 * @description 对应后端 TaktBackupLogUpdateDto
 */
export interface BackupLogUpdate extends BackupLogCreate {
  /**
   * BackupLogID（标识要更新的实体）
   */
  backupLogId: string;

}

/**
 * BackupLog 状态更新 DTO
 * 对应前端 BackupLogStatus
 * @description 对应后端 TaktBackupLogStatusDto
 */
export interface BackupLogStatus {
  /**
   * BackupLogID
   */
  backupLogId: string;

  /**
   * 运行状态（0=进行中 1=成功 2=失败）
   */
  runStatus: number;

}

/**
 * BackupLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BackupLogExport
 * @description 对应后端 TaktBackupLogExportDto
 */
export interface BackupLogExport {
  /**
   * BackupLogID
   */
  backupLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 备份种类（小写，如 database / file / config）
   */
  backupKind: string;

  /**
   * 来源业务键（备份配置 Id、任务号等，统一字符串）
   */
  sourceId: string;

  /**
   * 来源编码快照（配置编码、任务编码等）
   */
  sourceCode: string;

  /**
   * 目标名称（库展示名、目标标签等）
   */
  targetName: string;

  /**
   * 目标范围（可选；如租户码、公司码、路径根等）
   */
  targetScope: string;

  /**
   * 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
   */
  syncMode: number;

  /**
   * 执行方式快照（1=立即 2=后台）
   */
  executeMode: number;

  /**
   * 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
   */
  pathType: number;

  /**
   * 执行后结果路径
   */
  resultPath?: string;

  /**
   * 结果大小（字节）
   */
  fileSizeBytes: string;

  /**
   * 运行状态（0=进行中 1=成功 2=失败）
   */
  runStatus: number;

  /**
   * 失败错误信息
   */
  errorMessage?: string;

  /**
   * 开始时间
   */
  startedAt: string;

  /**
   * 结束时间
   */
  finishedAt?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

