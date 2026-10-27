// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：event.types.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：全局事件总线类型定义（采集、格式化、上报）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogContext } from '@/types/logger.types';

/**
 * 全局通知类型
 */
export type NotificationType = 'success' | 'error' | 'warning' | 'info';

/**
 * 事件阶段（发布 / 处理）
 */
export type EventPhase = 'emit' | 'handle';

/**
 * 事件总线契约（所有事件必须在此声明）
 */
export interface Events {
  /**
   * 会话失效（401 / 后端 Unauthorized）
   */
  'auth:session-expired': {
    message?: string;
  };

  /**
   * 空闲超时自动登出
   */
  'auth:idle-timeout': {
    message?: string;
  };

  /**
   * 主动登出（含强退）
   */
  'user:logout': {
    /** 登出后提示（hardRedirect 时写入 sessionStorage 供登录页展示） */
    message?: string;
    /** 整页跳转登录页（强退/会话失效场景，避免动态路由清掉后 SPA 内导航失败） */
    hardRedirect?: boolean;
  } | undefined;

  /**
   * 登录成功
   */
  'user:login': {
    userId: string;
    userName: string;
  };

  /**
   * 用户信息已更新
   */
  'user:update': Record<string, unknown>;

  /**
   * 租户切换
   */
  'tenant:change': {
    tenantCode: string;
    companyCode?: string;
  };

  /**
   * 公司切换
   */
  'company:change': {
    companyCode: string;
  };

  /**
   * 全局 Toast / 通知中心入列
   */
  'notification:show': {
    type: NotificationType;
    message: string;
    description?: string;
    /**
     * 为 true 时仅写入通知中心，不弹出 Message（由 @/utils/notification 弹出 Notification）
     */
    silent?: boolean;
  };

  /**
   * 在线消息 SignalR 实时送达（接收方客户端）
   */
  'foundation:message:received': import('@/types/foundation/signalR.types').SignalRMessage;

  /**
   * 流程定义变更（SignalR）
   */
  'workflow:scheme:changed': import('@/types/workflow/signalR.types').FlowSchemeChangedEvent;

  /**
   * 流程实例推进（SignalR）
   */
  'workflow:instance:progressed': import('@/types/workflow/signalR.types').FlowInstanceProgressedEvent;

  /**
   * 待办数量更新（SignalR）
   */
  'workflow:todo:count-updated': import('@/types/workflow/signalR.types').FlowTodoCountUpdatedEvent;

  /**
   * 定时任务定义变更（SignalR）
   */
  'foundation:quartz-task:changed': import('@/types/foundation/quartzSignalR.types').QuartzTaskChangedEvent;

  /**
   * 定时任务执行完成（SignalR）
   */
  'foundation:quartz-task:executed': import('@/types/foundation/quartzSignalR.types').QuartzTaskExecutedEvent;

  /**
   * BOM 物料成本机种月平均重算完成（SignalR）
   */
  'logistics:bom-material-cost-item:recalculate-completed': import('@/types/logistics/manufacturing/bom/materialCostItemSignalR.types').BomMaterialCostItemRecalculateCompletedEvent;

  /**
   * 设变技术课主表后台保存完成（SignalR）
   */
  'logistics:ec-te:persist-completed': import('@/types/logistics/manufacturing/engineering-change/ecTePersistSignalR.types').EcTePersistCompletedEvent;

  /**
   * 刷新菜单
   */
  'menu:refresh': undefined;

  /**
   * 菜单折叠
   */
  'menu:collapse': {
    collapsed: boolean;
  };

  /**
   * 刷新表格
   */
  'table:refresh': {
    tableName?: string;
  };

  /**
   * 表格查询
   */
  'table:query': {
    tableName: string;
    query: Record<string, unknown>;
  };

  /**
   * 重置表单
   */
  'form:reset': {
    formName?: string;
  };

  /**
   * 提交表单
   */
  'form:submit': {
    formName: string;
    data: Record<string, unknown>;
  };

  /**
   * 打开弹窗
   */
  'modal:open': {
    modalName: string;
    props?: Record<string, unknown>;
  };

  /**
   * 关闭弹窗
   */
  'modal:close': {
    modalName: string;
  };

  /**
   * 主题切换
   */
  'theme:change': {
    theme: 'light' | 'dark';
  };

  /**
   * 主题色预设切换
   */
  'theme-color:change': {
    preset: string;
    color: string;
  };

  /**
   * 语言切换
   */
  'locale:change': {
    locale: string;
  };

  /**
   * 导入进度
   */
  'import:progress': {
    progress: number;
    message: string;
  };

  /**
   * 导入完成
   */
  'import:complete': {
    success: number;
    fail: number;
    errors: string[];
  };
}

/**
 * 标准事件条目（采集与上报结构）
 */
export interface EventEntry {
  /**
   * 事件名称
   */
  eventName: string;

  /**
   * 事件阶段
   */
  phase: EventPhase;

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
   * 运行环境
   */
  environment: string;

  /**
   * 页面 URL
   */
  url: string;

  /**
   * 事件载荷
   */
  payload?: unknown;

  /**
   * 运行时上下文
   */
  context?: LogContext;
}

/**
 * 事件批量上报载荷
 */
export interface EventReportPayload {
  /**
   * 批次 ID
   */
  batchId: string;

  /**
   * 上报时间（ISO8601）
   */
  reportedAt: string;

  /**
   * 事件条目
   */
  entries: EventEntry[];
}

/**
 * 事件总线配置
 */
export interface EventBusConfig {
  /**
   * 是否输出到控制台
   */
  enableConsole: boolean;

  /**
   * 是否上报远端
   */
  enableReport: boolean;

  /**
   * 上报地址（POST JSON）
   */
  reportUrl?: string;

  /**
   * 批量上报条数阈值
   */
  batchSize: number;

  /**
   * flush 间隔（毫秒）
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
