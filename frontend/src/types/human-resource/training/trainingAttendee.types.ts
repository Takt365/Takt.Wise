// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/training
// 文件名称：trainingAttendee.types.ts
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工培训结果记录
 * 对应前端 TaktTrainingAttendeeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TrainingAttendee
 * @description 对应后端 TaktTrainingAttendeeDto
 */
export interface TrainingAttendee extends CompanyDtoBase {

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * TrainingAttendee 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingAttendeeExport
 * @description 对应后端 TaktTrainingAttendeeExportDto
 */
export interface TrainingAttendeeExport {
  /**
   * TrainingAttendeeID
   */
  trainingAttendeeId: string;

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
   * 培训课程 ID
   */
  trainingCourseId: string;

  /**
   * 培训课程名称
   */
  courseName: string;

  /**
   * 培训类型
   */
  trainingType: string;

  /**
   * 培训讲师
   */
  instructor: string;

  /**
   * 培训开始日期
   */
  trainingStartDate: string;

  /**
   * 培训结束日期
   */
  trainingEndDate: string;

  /**
   * 培训日期
   */
  trainingDate: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 培训成绩
   */
  trainingScore: number;

  /**
   * 是否通过（0=否 1=是）
   */
  isPassed: number;

  /**
   * 证书编码
   */
  CertificateCode: string;

  /**
   * 培训评价
   */
  trainingEvaluation: string;

  /**
   * 状态（1=有效 0=无效）
   */
  trainingResultStatus: number;

  /**
   * 关联工厂
   */
  plantCode?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

