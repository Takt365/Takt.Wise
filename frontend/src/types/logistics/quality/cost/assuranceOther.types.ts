// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assuranceOther.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 品质业务明细 - 其他通常业务费用
 * 对应前端 TaktQualityAssuranceOtherDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceOther
 * @description 对应后端 TaktQualityAssuranceOtherDto
 */
export interface QualityAssuranceOther extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId?: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode?: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber?: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost?: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * QualityAssuranceOther 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceOtherExport
 * @description 对应后端 TaktQualityAssuranceOtherExportDto
 */
export interface QualityAssuranceOtherExport {
  /**
   * QualityAssuranceOtherID
   */
  qualityAssuranceOtherId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
   */
  qualityAssuranceId: string;

  /**
   * 品质业务编码（冗余字段,便于查询）
   */
  qualityAssuranceCode: string;

  /**
   * 项号（如10, 20, 30，步长严格为10）
   */
  lineNumber: number;

  /**
   * 其他通常业务费用(元)
   */
  operationsCost: number;

  /**
   * 通常业务作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 通常业务其他费用(元)
   */
  otherExpenses: number;

  /**
   * 通常业务其他备注
   */
  otherNote?: string;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

