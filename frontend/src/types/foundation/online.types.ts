// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：online.types.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktOnlineDto → Online）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common.types';

/**
 * 在线用户实体 公司级实体：在线用户按租户+公司双重隔离
 * 对应前端 Online
 * 继承 CompanyDtoBase
 * @description 对应后端 TaktOnlineDto
 */
export interface Online extends CompanyDtoBase {
  /**
   * OnlineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  onlineId: string;

  /**
   * SignalR 连接 ID（租户+公司内唯一）
   */
  connectionId: string;

  /**
   * SignalR 连接 名称（填充字段）
   */
  connectionName?: string;

  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 在线状态（0=在线，1=离线，2=离开）
   */
  onlineStatus: number;

  /**
   * 连接 IP 地址
   */
  connectIp: string;

  /**
   * 连接地点
   */
  connectLocation: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 登录设备
   */
  deviceType: string;

  /**
   * 浏览器（TaktConstants.BrowserType）
   */
  browserType: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  operatingSystem: string;

  /**
   * 连接时间
   */
  connectTime: string;

  /**
   * 最后活动时间
   */
  lastActiveTime: string;

  /**
   * 断开时间（未断开时为 null）
   */
  disconnectTime?: string;

  /**
   * 连接时长（秒）
   */
  connectionDuration: number;

}

/**
 * Online 分页查询 DTO
 * 继承 TaktPagedQuery
 * @description 对应后端 TaktOnlineQueryDto
 */
export interface OnlineQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * SignalR 连接 ID（租户+公司内唯一）
   */
  connectionId?: string;

  /**
   * 用户名
   */
  userName?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 在线状态（0=在线，1=离线，2=离开）
   */
  onlineStatus?: number;

  /**
   * 连接 IP 地址
   */
  connectIp?: string;

  /**
   * 连接地点
   */
  connectLocation?: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent?: string;

  /**
   * 登录设备
   */
  deviceType?: string;

  /**
   * 浏览器（TaktConstants.BrowserType）
   */
  browserType?: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  operatingSystem?: string;

  /**
   * 连接时间（范围查询-开始）
   */
  connectTimeStart?: string;

  /**
   * 连接时间（范围查询-结束）
   */
  connectTimeEnd?: string;

  /**
   * 最后活动时间（范围查询-开始）
   */
  lastActiveTimeStart?: string;

  /**
   * 最后活动时间（范围查询-结束）
   */
  lastActiveTimeEnd?: string;

  /**
   * 断开时间（范围查询-开始）
   */
  disconnectTimeStart?: string;

  /**
   * 断开时间（范围查询-结束）
   */
  disconnectTimeEnd?: string;

  /**
   * 连接时长（秒）
   */
  connectionDuration?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}

/**
 * Online 状态更新 DTO
 * @description 对应后端 TaktOnlineStatusDto
 */
export interface OnlineStatus {
  /**
   * OnlineID
   */
  onlineId: string;

  /**
   * 在线状态（0=在线，1=离线，2=离开）
   */
  onlineStatus: number;

}

/**
 * Online 导出 DTO（独立实现，不继承响应 Dto）
 * @description 对应后端 TaktOnlineExportDto
 */
export interface OnlineExport {
  /**
   * OnlineID
   */
  onlineId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * SignalR 连接 ID（租户+公司内唯一）
   */
  connectionId: string;

  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 在线状态（0=在线，1=离线，2=离开）
   */
  onlineStatus: number;

  /**
   * 连接 IP 地址
   */
  connectIp: string;

  /**
   * 连接地点
   */
  connectLocation: string;

  /**
   * 用户代理（User-Agent）
   */
  userAgent: string;

  /**
   * 登录设备
   */
  deviceType: string;

  /**
   * 浏览器（TaktConstants.BrowserType）
   */
  browserType: string;

  /**
   * 操作系统（TaktConstants.OperatingSystem）
   */
  operatingSystem: string;

  /**
   * 连接时间
   */
  connectTime: string;

  /**
   * 最后活动时间
   */
  lastActiveTime: string;

  /**
   * 断开时间（未断开时为 null）
   */
  disconnectTime?: string;

  /**
   * 连接时长（秒）
   */
  connectionDuration: number;

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

/**
 * 当前登录用户在线统计 DTO
 * @description 对应后端 TaktOnlineStatisticsDto
 */
export interface OnlineStatistics {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 当前用户活跃在线连接数（多终端/多标签页分别计数）
   */
  onlineCount: number;

  /**
   * 当前在线总时长（秒）：当前用户所有活跃会话从连接至今累计
   */
  currentDurationSeconds: number;

  /**
   * 当天累计在线时长（秒）：当前用户当日各会话有效时长之和
   */
  todayDurationSeconds: number;

  /**
   * 当月累计在线时长（秒）
   */
  monthDurationSeconds: number;

  /**
   * 本周累计在线时长（秒，周一至今日）
   */
  weekTotalDurationSeconds: number;

  /**
   * 本周日均在线时长（秒）
   */
  weekAverageDurationSeconds: number;

  /**
   * 本月日均在线时长（秒）
   */
  monthAverageDurationSeconds: number;
}

/**
 * 在线看板统计 DTO（公司维度）
 * @description 对应后端 TaktOnlineDashboardStatisticsDto
 */
export interface OnlineDashboardStatistics {
  /**
   * 当前在线用户数
   */
  onlineUserCount: number;

  /**
   * 当日总访问量（与在线时长无关）
   */
  todayVisitCount: number;
}

/**
 * 在线用户强退参数
 * @description 对应后端 TaktOnlineForceKickDto
 */
export interface OnlineForceKick {
  /** SignalR 连接 ID（主键查无记录时回退定位） */
  connectionId?: string;
  /** 强退原因（可选） */
  reason?: string;
  /** 延迟强退秒数（0 表示立即强退） */
  delaySeconds?: number;
}

/**
 * 批量强退在线用户参数
 * @description 对应后端 TaktOnlineForceKickBatchDto
 */
export interface OnlineForceKickBatch {
  /** 在线用户 ID 列表 */
  onlineIds: string[];
  /** 强退原因（可选） */
  reason?: string;
  /** 延迟强退秒数（0 表示立即强退） */
  delaySeconds?: number;
}

/**
 * SignalR 统计推送目标用户
 * @description 对应后端 TaktSignalRPushStatisticsRequestDto
 */
export interface OnlinePushStatisticsRequest {
  /** 目标用户名 */
  userName: string;
  /** 目标用户 ID（可选） */
  userId?: string;
}

/**
 * 在线消息广播推送参数
 * @description 对应后端 TaktMessageBroadcastDto（TaktOnlinesController 广播端点）
 */
export interface OnlineBroadcastPush {
  /** 公司代码 */
  companyCode?: string;
  /** 发送者用户名 */
  fromUserName?: string;
  /** 消息标题 */
  messageTitle?: string;
  /** 消息内容 */
  messageContent: string;
  /** 消息类型 */
  messageType?: string;
  /** 消息分组 */
  messageGroup?: string;
  /** 发送时间 */
  sendTime?: string;
}