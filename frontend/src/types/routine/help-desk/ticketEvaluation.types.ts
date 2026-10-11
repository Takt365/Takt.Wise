// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticketEvaluation.types.ts
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
 * 工单服务评价（一个工单对应一条评价）
 * 对应前端 TaktTicketEvaluationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TicketEvaluation
 * @description 对应后端 TaktTicketEvaluationDto
 */
export interface TicketEvaluation extends CompanyDtoBase {
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
   * 综合评分
   */
  score?: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId?: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间
   */
  evaluatedAt?: string;

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
 * TicketEvaluation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TicketEvaluationExport
 * @description 对应后端 TaktTicketEvaluationExportDto
 */
export interface TicketEvaluationExport {
  /**
   * TicketEvaluationID
   */
  ticketEvaluationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工单 ID
   */
  ticketId: string;

  /**
   * 综合评分
   */
  score: number;

  /**
   * 评价内容
   */
  comment?: string;

  /**
   * 评价人 ID
   */
  evaluatorId: string;

  /**
   * 评价人姓名
   */
  evaluatorName?: string;

  /**
   * 评价时间
   */
  evaluatedAt: string;

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

