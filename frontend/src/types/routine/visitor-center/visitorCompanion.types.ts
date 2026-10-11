// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/visitor-center
// 文件名称：visitorCompanion.types.ts
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
 * 来访人员子实体（部门、职称、姓名）
 * 对应前端 TaktVisitorCompanionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 VisitorCompanion
 * @description 对应后端 TaktVisitorCompanionDto
 */
export interface VisitorCompanion extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 来访记录 ID
   */
  visitorId?: string;

  /**
   * 部门
   */
  department?: string;

  /**
   * 职称
   */
  jobTitle?: string;

  /**
   * 来访人员姓名
   */
  companionName?: string;

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
 * VisitorCompanion 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VisitorCompanionExport
 * @description 对应后端 TaktVisitorCompanionExportDto
 */
export interface VisitorCompanionExport {
  /**
   * VisitorCompanionID
   */
  visitorCompanionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来访记录 ID
   */
  visitorId: string;

  /**
   * 部门
   */
  department: string;

  /**
   * 职称
   */
  jobTitle: string;

  /**
   * 来访人员姓名
   */
  companionName: string;

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

