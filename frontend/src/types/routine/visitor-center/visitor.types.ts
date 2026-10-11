// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/visitor-center
// 文件名称：visitor.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/visitor-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 来访接待主实体（来访公司及参访起止时间）
 * 对应前端 TaktVisitorDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Visitor
 * @description 对应后端 TaktVisitorDto
 */
export interface Visitor extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 来访公司名称
   */
  visitorCompanyName?: string;

  /**
   * 参访开始时间
   */
  visitStartTime?: string;

  /**
   * 参访结束时间
   */
  visitEndTime?: string;

  /**
   * 来访人员列表（子表，级联保存）
   */
  companions?: VisitorCompanionCreate[];

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
 * Visitor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VisitorExport
 * @description 对应后端 TaktVisitorExportDto
 */
export interface VisitorExport {
  /**
   * VisitorID
   */
  visitorId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来访公司名称
   */
  visitorCompanyName: string;

  /**
   * 参访开始时间
   */
  visitStartTime: string;

  /**
   * 参访结束时间
   */
  visitEndTime: string;

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

