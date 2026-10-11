// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：archiveLog.types.ts
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
 * 归档日志（完整审计）
 * 对应前端 TaktArchiveLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ArchiveLog
 * @description 对应后端 TaktArchiveLogDto
 */
export interface ArchiveLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 归档种类（小写点号分段，如 table.year / file / attachment）
   */
  archiveKind: string;

  /**
   * 来源业务键（策略 Id、单据号等，统一字符串）
   */
  sourceId: string;

  /**
   * 来源名称（表名、路径、资源名等）
   */
  sourceName: string;

  /**
   * 归档目标名称（年分表名、归档路径等）
   */
  targetName: string;

  /**
   * 归档年份（按年归档时填写；其它场景可空）
   */
  archiveYear?: number;

  /**
   * 归档前匹配数量（行/文件/对象）
   */
  sourceCount: number;

  /**
   * 实际归档数量
   */
  archivedCount: number;

  /**
   * 源侧删除数量（热区清理等；无删除则为 0）
   */
  deletedCount: number;

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
 * 更新ArchiveLog DTO
 * 继承 TaktArchiveLogCreateDto，添加 ArchiveLogId 字段
 * 对应前端 ArchiveLogUpdate
 * @description 对应后端 TaktArchiveLogUpdateDto
 */
export interface ArchiveLogUpdate extends ArchiveLogCreate {
  /**
   * ArchiveLogID（标识要更新的实体）
   */
  archiveLogId: string;

}

/**
 * ArchiveLog 状态更新 DTO
 * 对应前端 ArchiveLogStatus
 * @description 对应后端 TaktArchiveLogStatusDto
 */
export interface ArchiveLogStatus {
  /**
   * ArchiveLogID
   */
  archiveLogId: string;

  /**
   * 运行状态（0=进行中 1=成功 2=失败）
   */
  runStatus: number;

}

/**
 * ArchiveLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ArchiveLogExport
 * @description 对应后端 TaktArchiveLogExportDto
 */
export interface ArchiveLogExport {
  /**
   * ArchiveLogID
   */
  archiveLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 归档种类（小写点号分段，如 table.year / file / attachment）
   */
  archiveKind: string;

  /**
   * 来源业务键（策略 Id、单据号等，统一字符串）
   */
  sourceId: string;

  /**
   * 来源名称（表名、路径、资源名等）
   */
  sourceName: string;

  /**
   * 归档目标名称（年分表名、归档路径等）
   */
  targetName: string;

  /**
   * 归档年份（按年归档时填写；其它场景可空）
   */
  archiveYear?: number;

  /**
   * 归档前匹配数量（行/文件/对象）
   */
  sourceCount: number;

  /**
   * 实际归档数量
   */
  archivedCount: number;

  /**
   * 源侧删除数量（热区清理等；无删除则为 0）
   */
  deletedCount: number;

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

