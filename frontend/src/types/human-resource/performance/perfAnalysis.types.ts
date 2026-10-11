// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perfAnalysis.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/performance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 分析改进
 * 对应前端 TaktPerfAnalysisDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PerfAnalysis
 * @description 对应后端 TaktPerfAnalysisDto
 */
export interface PerfAnalysis extends ApprovalDtoBase {

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
 * PerfAnalysis 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfAnalysisExport
 * @description 对应后端 TaktPerfAnalysisExportDto
 */
export interface PerfAnalysisExport {
  /**
   * PerfAnalysisID
   */
  perfAnalysisId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联考核评估 ID
   */
  assessmentId: string;

  /**
   * 改进计划标题
   */
  planTitle: string;

  /**
   * 改进领域
   */
  improvementArea: string;

  /**
   * 当前状况描述
   */
  currentSituation: string;

  /**
   * 改进目标
   */
  improvementGoal: string;

  /**
   * 改进措施
   */
  improvementActions: string;

  /**
   * 计划制定日期
   */
  planDate: string;

  /**
   * 目标完成日期
   */
  targetCompletionDate: string;

  /**
   * 进度百分比（%）
   */
  progressPercentage: number;

  /**
   * 改进结果说明
   */
  resultDescription: string;

  /**
   * 指导老师 ID
   */
  mentorId: string;

  /**
   * 业务状态（0=待审批 1=进行中 2=已完成 3=已关闭）
   */
  improvementStatus: number;

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

