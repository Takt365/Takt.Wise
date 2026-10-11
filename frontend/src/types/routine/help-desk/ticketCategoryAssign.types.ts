// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticketCategoryAssign.types.ts
// 创建时间：2026-06-23
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
 * 工单分类默认处理人（按 CategoryCode 自动分配处理人）
 * 对应前端 TaktTicketCategoryAssignDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketCategoryAssign
 * @description 对应后端 TaktTicketCategoryAssignDto
 */
export interface TicketCategoryAssign extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode?: string;

  /**
   * 默认处理人 ID
   */
  assigneeId?: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

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
 * TicketCategoryAssign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketCategoryAssignExport
 * @description 对应后端 TaktTicketCategoryAssignExportDto
 */
export interface TicketCategoryAssignExport {
  /**
   * TicketCategoryAssignID
   */
  ticketCategoryAssignId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 分类编码（与 TaktTicket.CategoryCode 对应）
   */
  categoryCode: string;

  /**
   * 默认处理人 ID
   */
  assigneeId: string;

  /**
   * 默认处理人姓名
   */
  assigneeName?: string;

  /**
   * 排序号
   */
  sortOrder: number;

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

