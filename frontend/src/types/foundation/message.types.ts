// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：message.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成 CRUD + 批量发送类型；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 在线消息实体 公司级实体：消息按租户+公司双重隔离
 * 对应前端 TaktMessageDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Message
 * @description 对应后端 TaktMessageDto
 */
export interface Message extends CompanyDtoBase {
  /**
   * MessageID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  messageId: string;

  /**
   * 发送者用户 ID
   */
  fromUserId: string;

  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 发送者昵称（由用户表 NickName 解析，非消息表持久化字段）
   */
  fromUserNickName?: string;

  /**
   * 接收者用户 ID
   */
  toUserId: string;

  /**
   * 接收者用户名
   */
  toUserName: string;

  /**
   * 消息标题
   */
  messageTitle: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 消息类型（字典 sys_message_type DictValue：text、system、multimedia）
   */
  messageType: string;

  /**
   * 消息分组（字典 sys_message_group DictValue）
   */
  messageGroup: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 抄送（0=否，1=是）
   */
  isCc: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 消息扩展数据（JSON）
   */
  messageExtData?: string;

  /**
   * 读取状态（0=未读 1=已读）
   */
  readStatus: number;
}

/**
 * Message 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MessageExport
 * @description 对应后端 TaktMessageExportDto
 */
export interface MessageExport {
  /**
   * MessageID
   */
  messageId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 接收者用户名
   */
  toUserName: string;

  /**
   * 消息标题
   */
  messageTitle?: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 消息类型（字典 sys_message_type DictValue：text、system、multimedia）
   */
  messageType: string;

  /**
   * 消息分组（字典 sys_message_group DictValue）
   */
  messageGroup: string;

  /**
   * 读取状态（0=未读 1=已读）
   */
  readStatus: number;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 发送时间
   */
  sendTime: string;

  /**
   * 抄送（0=否，1=是）
   */
  isCc: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 消息扩展数据（JSON）
   */
  messageExtData?: string;

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
 * 创建 Message DTO
 * @description 对应后端 TaktMessageCreateDto
 */
export interface MessageCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 发送者用户 ID
   */
  fromUserId?: string;

  /**
   * 发送者用户名
   */
  fromUserName: string;

  /**
   * 接收者用户 ID
   */
  toUserId?: string;

  /**
   * 接收者用户名
   */
  toUserName?: string;

  /**
   * 消息标题
   */
  messageTitle: string;

  /**
   * 消息内容
   */
  messageContent: string;

  /**
   * 消息类型（字典 sys_message_type DictValue：text、system、multimedia）
   */
  messageType?: string;

  /**
   * 消息分组（字典 sys_message_group DictValue）
   */
  messageGroup?: string;

  /**
   * 读取时间
   */
  readTime?: string;

  /**
   * 发送时间
   */
  sendTime?: string;

  /**
   * 抄送（0=否，1=是）
   */
  isCc?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 消息扩展数据（JSON）
   */
  messageExtData?: string;

  /**
   * 读取状态（0=未读 1=已读）
   */
  readStatus?: number;

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
 * 更新 Message DTO
 * @description 对应后端 TaktMessageUpdateDto
 */
export interface MessageUpdate extends MessageCreate {
  /**
   * MessageID（标识要更新的实体）
   */
  messageId: string;
}

/**
 * 批量创建并发送消息 DTO
 * @description 对应后端 TaktMessageBatchCreateDto / POST TaktMessages/batch-send
 */
export interface MessageBatchCreate extends MessageCreate {
  /** 是否发送给当前公司全部可访问用户 */
  sendToAll: boolean;
  /** 指定接收者用户 ID 列表 */
  toUserIds?: string[];
}

/**
 * 当前用户在线消息统计
 * @description 对应后端 TaktMessageStatisticsDto / GET TaktMessages/statistics
 */
export interface MessageStatistics {
  /** 用户 ID */
  userId?: string;
  /** 用户名（接收者） */
  userName: string;
  /** 收件箱消息总数 */
  totalCount: number;
  /** 已读数量 */
  readCount: number;
  /** 未读数量 */
  unreadCount: number;
}

