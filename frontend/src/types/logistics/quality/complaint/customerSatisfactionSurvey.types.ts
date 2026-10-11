// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerSatisfactionSurvey.types.ts
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
 * 客户满意度调查表主表实体
 * 对应前端 TaktCustomerSatisfactionSurveyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerSatisfactionSurvey
 * @description 对应后端 TaktCustomerSatisfactionSurveyDto
 */
export interface CustomerSatisfactionSurvey extends CompanyDtoBase {

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus?: number;

  /**
   * 调查项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: CustomerSatisfactionSurveyItemCreate[];

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
 * CustomerSatisfactionSurvey 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerSatisfactionSurveyExport
 * @description 对应后端 TaktCustomerSatisfactionSurveyExportDto
 */
export interface CustomerSatisfactionSurveyExport {
  /**
   * CustomerSatisfactionSurveyID
   */
  customerSatisfactionSurveyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 调查表编码（组合唯一索引）
   */
  customerSatisfactionSurveyCode: string;

  /**
   * 客户 ID（选项 TaktCustomers/options；DictValue=Id）
   */
  customerId: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 调查日期
   */
  surveyDate: string;

  /**
   * 调查方式（字典 logistics_quality_survey_method）
   */
  surveyMethod: number;

  /**
   * 调查类型（字典 logistics_quality_survey_type）
   */
  surveyType: number;

  /**
   * 调查周期（字典 logistics_quality_period）
   */
  surveyPeriod: number;

  /**
   * 调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）
   */
  surveyorByGroupCode?: string;

  /**
   * 调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  surveyorByGroupName?: string;

  /**
   * 客户联系人
   */
  customerContact?: string;

  /**
   * 客户联系电话
   */
  customerPhone?: string;

  /**
   * 整体满意度（字典 logistics_quality_satisfaction_level）
   */
  overallSatisfaction: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 产品质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付准时率评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 服务质量评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 价格竞争力评分（0-100分）
   */
  priceScore?: number;

  /**
   * 技术支持评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 客户主要表扬
   */
  customerPraise?: string;

  /**
   * 客户主要意见/建议
   */
  customerFeedback?: string;

  /**
   * 改进计划/措施
   */
  improvementPlan?: string;

  /**
   * 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  relatedComplaintId?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 调查状态（字典 logistics_quality_survey_status）
   */
  surveyStatus: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 跟进状态（字典 logistics_quality_follow_up_status）
   */
  followUpStatus: number;

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

