// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticketReply.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 工单回复实体（用户与客服会话）
 * 对应前端 TaktTicketReplyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketReply
 * @description 对应后端 TaktTicketReplyDto
 */
export interface TicketReply extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工单 ID
   */
  ticketId?: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType?: number;

  /**
   * 作者用户 ID
   */
  authorId?: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent?: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal?: number;

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
 * TicketReply 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketReplyExport
 * @description 对应后端 TaktTicketReplyExportDto
 */
export interface TicketReplyExport {
  /**
   * TicketReplyID
   */
  ticketReplyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 作者类型（0=客服，1=用户，2=系统）
   */
  authorType: number;

  /**
   * 作者用户 ID
   */
  authorId: string;

  /**
   * 作者姓名
   */
  authorName?: string;

  /**
   * 回复内容
   */
  ticketReplyContent: string;

  /**
   * 附件列表 JSON
   */
  attachments?: string;

  /**
   * 是否内部备注（仅客服可见）
   */
  isInternal: number;

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

