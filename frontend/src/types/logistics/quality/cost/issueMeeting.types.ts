// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issueMeeting.types.ts
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
 * 品质问题应对明细 - 会议/调查/试验费用
 * 对应前端 TaktQualityIssueMeetingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssueMeeting
 * @description 对应后端 TaktQualityIssueMeetingDto
 */
export interface QualityIssueMeeting extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute?: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute?: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost?: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes?: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount?: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount?: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes?: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost?: number;

  /**
   * 其他费用（元）
   */
  otherExpenses?: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes?: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost?: number;

  /**
   * 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
   */
  responseGroupCode?: string;

  /**
   * 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  responseGroupName?: string;

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
 * QualityIssueMeeting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssueMeetingExport
 * @description 对应后端 TaktQualityIssueMeetingExportDto
 */
export interface QualityIssueMeetingExport {
  /**
   * QualityIssueMeetingID
   */
  qualityIssueMeetingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 直接人员费率（元/分钟）
   */
  directManpowerCostPerMinute: number;

  /**
   * 间接人员费率（元/分钟）
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 讨论调查试验内容(会议记录)
   */
  meetingInvestigationContent?: string;

  /**
   * 讨论调查试验费用(元)
   */
  meetingInvestigationCost: number;

  /**
   * 讨论会使用时间(分钟)
   */
  meetingTimeMinutes: number;

  /**
   * 直接人员参加人数
   */
  directParticipantCount: number;

  /**
   * 间接人员参加人数
   */
  indirectParticipantCount: number;

  /**
   * 调查评价试验工作时间（分钟）
   */
  investigationWorkTimeMinutes: number;

  /**
   * 交通费、旅费（元）
   */
  travelCost: number;

  /**
   * 其他费用（元）
   */
  otherExpenses: number;

  /**
   * 其他作业時間（分钟）
   */
  otherWorkTimeMinutes: number;

  /**
   * 其他设备购入费、工程费、搬运费等（元）
   */
  otherApparatusCost: number;

  /**
   * 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
   */
  responseGroupCode?: string;

  /**
   * 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  responseGroupName?: string;

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

