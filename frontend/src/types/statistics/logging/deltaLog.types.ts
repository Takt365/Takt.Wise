// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：deltaLog.types.ts
// 创建时间：2026-06-12
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
 * 差异日志实体（AOP 审计）
 * 对应前端 TaktDeltaLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DeltaLog
 * @description 对应后端 TaktDeltaLogDto
 */
export interface DeltaLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 操作类型（INSERT、UPDATE、DELETE）
   */
  operType: string;

  /**
   * 数据库表名（SugarTable 物理表名）
   */
  tableName: string;

  /**
   * 业务主键 ID
   */
  primaryKeyId?: string;

  /**
   * 修改前数据 JSON（旧值快照）
   */
  beforeData: string;

  /**
   * 修改后数据 JSON（新值快照）
   */
  afterData: string;

  /**
   * 差异内容 JSON（变更字段及旧/新值明细）
   */
  diffData: string;

  /**
   * 执行的 SQL 语句（AOP 捕获）
   */
  sqlStatement: string;

  /**
   * 操作 IP
   */
  operIp: string;

  /**
   * 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
   */
  operLocation: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 浏览器（TaktConstants.BrowserType）
   */
  browser: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  os: string;

  /**
   * 登录设备（TaktConstants.DeviceType）
   */
  deviceType: string;

  /**
   * 操作时间（数据变更发生时刻）
   */
  operTime: string;

  /**
   * 执行耗时（毫秒）
   */
  elapsedTime: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * 更新DeltaLog DTO
 * 继承 TaktDeltaLogCreateDto，添加 DeltaLogId 字段
 * 对应前端 DeltaLogUpdate
 * @description 对应后端 TaktDeltaLogUpdateDto
 */
export interface DeltaLogUpdate extends DeltaLogCreate {
  /**
   * DeltaLogID（标识要更新的实体）
   */
  deltaLogId: string;

}

/**
 * DeltaLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DeltaLogExport
 * @description 对应后端 TaktDeltaLogExportDto
 */
export interface DeltaLogExport {
  /**
   * DeltaLogID
   */
  deltaLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号）
   */
  userName: string;

  /**
   * 操作类型（INSERT、UPDATE、DELETE）
   */
  operType: string;

  /**
   * 数据库表名（SugarTable 物理表名）
   */
  tableName: string;

  /**
   * 业务主键 ID
   */
  primaryKeyId?: string;

  /**
   * 修改前数据 JSON（旧值快照）
   */
  beforeData: string;

  /**
   * 修改后数据 JSON（新值快照）
   */
  afterData: string;

  /**
   * 差异内容 JSON（变更字段及旧/新值明细）
   */
  diffData: string;

  /**
   * 执行的 SQL 语句（AOP 捕获）
   */
  sqlStatement: string;

  /**
   * 操作 IP
   */
  operIp: string;

  /**
   * 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
   */
  operLocation: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 浏览器（TaktConstants.BrowserType）
   */
  browser: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  os: string;

  /**
   * 登录设备（TaktConstants.DeviceType）
   */
  deviceType: string;

  /**
   * 操作时间（数据变更发生时刻）
   */
  operTime: string;

  /**
   * 执行耗时（毫秒）
   */
  elapsedTime: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

