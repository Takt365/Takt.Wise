// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perfAssessment.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工绩效考核
 * 对应前端 TaktPerfAssessmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfAssessment
 * @description 对应后端 TaktPerfAssessmentDto
 */
export interface PerfAssessment extends CompanyDtoBase {

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
 * PerfAssessment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfAssessmentExport
 * @description 对应后端 TaktPerfAssessmentExportDto
 */
export interface PerfAssessmentExport {
  /**
   * PerfAssessmentID
   */
  perfAssessmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 考核周期（如 2026-Q1、2026-Annual）
   */
  assessmentPeriod: string;

  /**
   * 考核日期
   */
  assessmentDate: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 自评分数
   */
  selfScore: number;

  /**
   * 自评说明
   */
  selfEvaluationNotes: string;

  /**
   * 主管评分
   */
  supervisorScore: number;

  /**
   * 主管评语
   */
  supervisorComments: string;

  /**
   * 综合得分
   */
  finalScore: number;

  /**
   * 绩效等级（A/B/C/D/E）
   */
  performanceGrade: string;

  /**
   * 评审人 ID
   */
  reviewerId: string;

  /**
   * 面谈日期
   */
  interviewDate: string;

  /**
   * 面谈记录
   */
  interviewNotes: string;

  /**
   * 状态（0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
   */
  assessmentStatus: number;

  /**
   * 关联工厂
   */
  plantCode?: string;

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

