// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerSatisfactionSurveyItem.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 客户满意度调查项目明细实体
 * 对应前端 TaktCustomerSatisfactionSurveyItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerSatisfactionSurveyItem
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemDto
 */
export interface CustomerSatisfactionSurveyItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
   */
  surveyId?: string;

  /**
   * 调查表编码（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 调查类别类型（字典 logistics_quality_satisfaction_category）
   */
  categoryType?: number;

  /**
   * 调查项目名称
   */
  itemName?: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（字典 logistics_quality_satisfaction_level）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus?: number;

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
 * CustomerSatisfactionSurveyItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerSatisfactionSurveyItemExport
 * @description 对应后端 TaktCustomerSatisfactionSurveyItemExportDto
 */
export interface CustomerSatisfactionSurveyItemExport {
  /**
   * CustomerSatisfactionSurveyItemID
   */
  customerSatisfactionSurveyItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
   */
  surveyId: string;

  /**
   * 调查表编码（冗余字段，便于查询）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 调查类别类型（字典 logistics_quality_satisfaction_category）
   */
  categoryType: number;

  /**
   * 调查项目名称
   */
  itemName: string;

  /**
   * 调查项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 满意度等级（字典 logistics_quality_satisfaction_level）
   */
  satisfactionLevel?: number;

  /**
   * 客户反馈/意见
   */
  customerFeedback?: string;

  /**
   * 改进建议
   */
  improvementSuggestion?: string;

  /**
   * 跟进措施
   */
  followUpAction?: string;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus: number;

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

