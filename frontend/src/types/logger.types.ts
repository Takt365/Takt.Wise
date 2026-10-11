// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：logger.types.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端通用日志类型定义（采集、格式化、上报）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogLevel } from '@/utils/logFormatter';

/**
 * 单条日志上下文（业务扩展字段）
 */
export interface LogContext {
  /**
   * 模块名（如 request、router、login）
   */
  module?: string;

  /**
   * 动作名（如 submit、navigate、refresh）
   */
  action?: string;

  /**
   * 当前用户 ID
   */
  userId?: string;

  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司编码
   */
  companyCode?: string;

  /**
   * 当前路由路径
   */
  route?: string;

  /**
   * 请求追踪 ID
   */
  requestId?: string;

  /**
   * 扩展字段
   */
  [key: string]: unknown;
}

/**
 * 序列化后的错误信息
 */
export interface LogErrorInfo {
  /**
   * 错误类型名
   */
  name: string;

  /**
   * 错误消息
   */
  message: string;

  /**
   * 堆栈（可选）
   */
  stack?: string;
}

/**
 * 统一日志条目（采集与上报的标准结构）
 */
export interface LogEntry {
  /**
   * 日志级别
   */
  level: LogLevel;

  /**
   * 主消息
   */
  message: string;

  /**
   * ISO8601 时间戳
   */
  timestamp: string;

  /**
   * 应用名称
   */
  appName: string;

  /**
   * 应用版本
   */
  appVersion: string;

  /**
   * 运行环境（development / production）
   */
  environment: string;

  /**
   * 页面 URL
   */
  url: string;

  /**
   * 用户代理
   */
  userAgent: string;

  /**
   * 业务上下文
   */
  context?: LogContext;

  /**
   * 关联错误
   */
  error?: LogErrorInfo;

  /**
   * 标签（便于检索）
   */
  tags?: string[];
}

/**
 * 批量上报载荷
 */
export interface LogReportPayload {
  /**
   * 批次 ID
   */
  batchId: string;

  /**
   * 客户端上报时间（ISO8601）
   */
  reportedAt: string;

  /**
   * 日志条目列表
   */
  entries: LogEntry[];
}

/**
 * 日志器配置
 */
export interface LoggerConfig {
  /**
   * 最低输出级别
   */
  minLevel: LogLevel;

  /**
   * 是否输出到控制台
   */
  enableConsole: boolean;

  /**
   * 是否写入本地 logs 目录（开发环境经 Vite 落盘）
   */
  enableFile: boolean;

  /**
   * 本地落盘端点（开发默认 /__takt/client-logs）
   */
  fileUrl?: string;

  /**
   * 是否上报远端
   */
  enableReport: boolean;

  /**
   * 上报地址（POST JSON）
   */
  reportUrl?: string;

  /**
   * 队列达到该条数时立即 flush
   */
  batchSize: number;

  /**
   * 定时 flush 间隔（毫秒）
   */
  flushIntervalMs: number;

  /**
   * 应用名称
   */
  appName: string;

  /**
   * 应用版本
   */
  appVersion: string;

  /**
   * 运行环境
   */
  environment: string;
}

/**
 * 日志器实例接口
 */
export interface Logger {
  /**
   * 调试日志
   * @param message 消息
   * @param context 上下文
   */
  debug(message: string, context?: LogContext): void;

  /**
   * 信息日志
   * @param message 消息
   * @param context 上下文
   */
  info(message: string, context?: LogContext): void;

  /**
   * 警告日志
   * @param message 消息
   * @param context 上下文
   * @param error 关联错误
   */
  warn(message: string, context?: LogContext, error?: unknown): void;

  /**
   * 错误日志
   * @param message 消息
   * @param context 上下文
   * @param error 关联错误
   */
  error(message: string, context?: LogContext, error?: unknown): void;

  /**
   * 致命错误日志（立即 flush 上报队列）
   * @param message 消息
   * @param context 上下文
   * @param error 关联错误
   */
  fatal(message: string, context?: LogContext, error?: unknown): void;

  /**
   * 立即 flush 上报队列
   */
  flush(): void;
}
