// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/logging
// 文件名称：trackingLog.types.ts
// 创建时间：2026-06-25
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
 * 前端交互日志实体
 * 对应前端 TaktTrackingLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TrackingLog
 * @description 对应后端 TaktTrackingLogDto
 */
export interface TrackingLog extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory: string;

  /**
   * 事件发生时间（客户端 UTC）
   */
  eventTime: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs: number;

  /**
   * PerformanceEntry.name
   */
  entryName: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel: number;

  /**
   * SPA 路由路径
   */
  routePath: string;

  /**
   * 页面完整 URL
   */
  pageUrl: string;

  /**
   * TaskAttribution.containerType
   */
  containerType: string;

  /**
   * TaskAttribution.containerName
   */
  containerName: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc: string;

  /**
   * TaskAttribution.containerId
   */
  containerId: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 客户端 IP
   */
  clientIp: string;

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
 * 更新TrackingLog DTO
 * 继承 TaktTrackingLogCreateDto，添加 TrackingLogId 字段
 * 对应前端 TrackingLogUpdate
 * @description 对应后端 TaktTrackingLogUpdateDto
 */
export interface TrackingLogUpdate extends TrackingLogCreate {
  /**
   * TrackingLogID（标识要更新的实体）
   */
  trackingLogId: string;

}

/**
 * TrackingLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrackingLogExport
 * @description 对应后端 TaktTrackingLogExportDto
 */
export interface TrackingLogExport {
  /**
   * TrackingLogID
   */
  trackingLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 事件类型（如 longtask）
   */
  eventTrackingType: string;

  /**
   * 事件分类（如 performance）
   */
  eventTrackingCategory: string;

  /**
   * 事件发生时间（客户端 UTC）
   */
  eventTime: string;

  /**
   * 长任务阻塞时长（毫秒）
   */
  durationMs: number;

  /**
   * PerformanceEntry.startTime（毫秒，相对页面导航起点）
   */
  performanceStartMs: number;

  /**
   * PerformanceEntry.name
   */
  entryName: string;

  /**
   * 追踪级别（1=warn 2=error，前端阈值映射）
   */
  trackingLevel: number;

  /**
   * SPA 路由路径
   */
  routePath: string;

  /**
   * 页面完整 URL
   */
  pageUrl: string;

  /**
   * TaskAttribution.containerType
   */
  containerType: string;

  /**
   * TaskAttribution.containerName
   */
  containerName: string;

  /**
   * TaskAttribution.containerSrc
   */
  containerSrc: string;

  /**
   * TaskAttribution.containerId
   */
  containerId: string;

  /**
   * 完整 attribution JSON 数组
   */
  attributionJson: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 客户端 IP
   */
  clientIp: string;

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

/**
 * Long Task 单条上报 DTO
 */
export interface TrackingLogTrackItem {
  eventTrackingType: string;
  eventTrackingCategory: string;
  eventTime: string;
  durationMs: number;
  performanceStartMs: number;
  entryName: string;
  trackingLevel: number;
  routePath: string;
  pageUrl: string;
  containerType: string;
  containerName: string;
  containerSrc: string;
  containerId: string;
  attributionJson?: string;
  userAgent: string;
}

/**
 * Long Task 批量上报 DTO
 */
export interface TrackingLogBatchTrack {
  items: TrackingLogTrackItem[];
}

/**
 * Long Task 批量上报结果
 */
export interface TrackingLogTrackResult {
  count: number;
}

