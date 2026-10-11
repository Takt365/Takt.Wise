// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：signalR.types.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR Hub 事件相关类型定义（Foundation 域；类型名去 Takt 前缀与末尾 Dto）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * Hub 返回的在线用户摘要
 */
export interface OnlineUser {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 连接时间
   */
  connectTime: string;

  /**
   * 最后活动时间
   */
  lastActiveTime?: string;
}

/**
 * 用户连接事件
 */
export interface UserConnectedEvent {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 连接时间
   */
  connectTime: string;
}

/**
 * 用户断开连接事件
 */
export interface UserDisconnectedEvent {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 断开时间
   */
  disconnectTime: string;
}

/**
 * SignalR 私信
 */
export interface SignalRMessage {
  /**
   * 消息 ID
   */
  messageId?: string;

  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 发送者昵称
   */
  fromUserNickName?: string;

  /**
   * 接收者用户名
   */
  toUserName: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 文件名称
   */
  fileName?: string;

  /**
   * 访问地址
   */
  accessUrl?: string;

  /**
   * 消息类型（字典 sys_message_type DictValue）
   */
  messageType: string;

  /**
   * 消息分组（字典 sys_message_group DictValue）
   */
  messageGroup: string;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 读取状态（0=未读，1=已读）
   */
  readStatus: number;
}

/**
 * 广播消息
 */
export interface BroadcastMessage {
  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 消息类型（字典 sys_message_type DictValue）
   */
  messageType: string;

  /**
   * 消息分组（字典 sys_message_group DictValue）
   */
  messageGroup: string;

  /**
   * 发送时间
   */
  sendTime: string;
}

/**
 * 消息已发送事件
 */
export interface MessageSentEvent {
  /**
   * 接收者用户名
   */
  toUserName: string;

  /**
   * 消息 ID
   */
  messageId?: string;

  /**
   * 发送时间
   */
  sendTime: string;
}

/**
 * 消息已读事件
 */
export interface MessageReadEvent {
  /**
   * 消息 ID
   */
  messageId: number;

  /**
   * 读取时间
   */
  readTime: string;
}

/**
 * SignalR 错误事件
 */
export interface SignalRErrorEvent {
  /**
   * 错误消息
   */
  message: string;
}

/**
 * 上线通知事件
 */
export interface OnlineMessageEvent {
  /**
   * 用户名
   */
  userName: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 连接时间
   */
  connectTime: string;

  /**
   * 消息内容
   */
  message: string;

  /**
   * 消息内容类型（可选，上线欢迎消息不含此字段）
   */
  messageType?: number;
}

/**
 * 强退事件
 */
export interface ForceLogoutEvent {
  /**
   * 强退提示
   */
  message: string;

  /**
   * 在线用户记录 ID
   */
  onlineId?: string;

  /**
   * SignalR 连接 ID
   */
  connectionId?: string;

  /**
   * 用户名
   */
  userName?: string;

  /**
   * 强退时间
   */
  forceKickTime?: string;
}

/**
 * 延迟强退预告事件
 */
export interface ForceLogoutScheduledEvent {
  /**
   * 预告提示
   */
  message: string;

  /**
   * 延迟秒数
   */
  delaySeconds: number;

  /**
   * 计划强退时间（ISO 8601）
   */
  kickAt: string;

  /**
   * 在线用户记录 ID
   */
  onlineId?: string;

  /**
   * SignalR 连接 ID
   */
  connectionId?: string;

  /**
   * 用户名
   */
  userName?: string;
}
