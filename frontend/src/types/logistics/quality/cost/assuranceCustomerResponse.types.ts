// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assuranceCustomerResponse.types.ts
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
 * 品质业务明细 - 顾客品质要求对应业务费用
 * 对应前端 TaktQualityAssuranceCustomerResponseDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssuranceCustomerResponse
 * @description 对应后端 TaktQualityAssuranceCustomerResponseDto
 */
export interface QualityAssuranceCustomerResponse extends CompanyDtoBase {
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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 顾客品质要求对应业务费用(元)
   */
  responseCost?: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes?: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 顾客应对备注
   */
  customerResponseNote?: string;

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
 * QualityAssuranceCustomerResponse 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceCustomerResponseExport
 * @description 对应后端 TaktQualityAssuranceCustomerResponseExportDto
 */
export interface QualityAssuranceCustomerResponseExport {
  /**
   * QualityAssuranceCustomerResponseID
   */
  qualityAssuranceCustomerResponseId: string;

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
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 顾客品质要求对应业务费用(元)
   */
  responseCost: number;

  /**
   * 评价作业时间(分钟)
   */
  workTimeMinutes: number;

  /**
   * 评价其他费用(元)
   */
  otherExpenses: number;

  /**
   * 顾客应对备注
   */
  customerResponseNote?: string;

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

