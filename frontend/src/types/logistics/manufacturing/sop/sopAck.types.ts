// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopAck.types.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 确认实体
 * 对应前端 TaktSopAckDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopAck
 * @description 对应后端 TaktSopAckDto
 */
export interface SopAck extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * SOP 主档 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  sopId?: string;

  /**
   * SOP 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId?: string;

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 确认人 ID（班组长，序列化为 string 以避免 Javascript 精度问题）
   */
  acknowledgedBy?: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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
 * SopAck 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopAckExport
 * @description 对应后端 TaktSopAckExportDto
 */
export interface SopAckExport {
  /**
   * SopAckID
   */
  sopAckId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * SOP 主档 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  sopId: string;

  /**
   * SOP 版本 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  revisionId: string;

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 确认人 ID（班组长，序列化为 string 以避免 Javascript 精度问题）
   */
  acknowledgedBy: string;

  /**
   * 确认时间
   */
  acknowledgedAt: string;

  /**
   * 确认意见
   */
  ackComment?: string;

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

