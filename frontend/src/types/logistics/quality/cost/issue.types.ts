// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issue.types.ts
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
 * 品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)及汇总数据
 * 对应前端 TaktQualityIssueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssue
 * @description 对应后端 TaktQualityIssueDto
 */
export interface QualityIssue extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode?: string;

  /**
   * 问题日期
   */
  issueDate?: string;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 批次号/Lot No
   */
  lot?: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes?: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost?: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  currencyCode?: string;

  /**
   * 会议/调查/试验费用明细列表（子表，级联保存）
   */
  meetingItems?: QualityIssueMeetingCreate[];

  /**
   * 组装不良改修应对明细列表（子表，级联保存）
   */
  assyReworkItems?: QualityIssueAssyReworkCreate[];

  /**
   * PCBA不良改修应对明细列表（子表，级联保存）
   */
  pcbaReworkItems?: QualityIssuePcbaReworkCreate[];

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
 * QualityIssue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssueExport
 * @description 对应后端 TaktQualityIssueExportDto
 */
export interface QualityIssueExport {
  /**
   * QualityIssueID
   */
  qualityIssueId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质问题编码（唯一，如：QF-2026-0001）
   */
  qualityIssueCode: string;

  /**
   * 问题日期
   */
  issueDate: string;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 批次号/Lot No
   */
  lot: string;

  /**
   * 品质问题应对摘要(汇总说明)
   */
  qualityProblemsResponse?: string;

  /**
   * 不良改修应对摘要(汇总说明)
   */
  reworkDueToDefects?: string;

  /**
   * 是否需要不良改修应对(Y/N)
   */
  needRework?: string;

  /**
   * 总时间(分钟,自动计算 = 各子表时间合计)
   */
  totalTimeMinutes: number;

  /**
   * 总费用(元,自动计算 = 各子表费用合计)
   */
  totalCost: number;

  /**
   * 成本币种（CNY/USD/JPY等）
   */
  currencyCode: string;

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

