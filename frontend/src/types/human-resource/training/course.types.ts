// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/training
// 文件名称：course.types.ts
// 创建时间：2026-06-23
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
 * 培训课程定义
 * 对应前端 TaktTrainingCourseDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TrainingCourse
 * @description 对应后端 TaktTrainingCourseDto
 */
export interface TrainingCourse extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode?: string;

  /**
   * 课程名称
   */
  courseName?: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType?: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel?: string;

  /**
   * 课程描述
   */
  courseDescription?: string;

  /**
   * 课程目标
   */
  courseObjectives?: string;

  /**
   * 培训时长（小时）
   */
  trainingHours?: number;

  /**
   * 主讲讲师
   */
  mainInstructor?: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod?: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod?: string;

  /**
   * 及格分数线
   */
  passingScore?: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus?: number;

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

}

/**
 * TrainingCourse 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingCourseExport
 * @description 对应后端 TaktTrainingCourseExportDto
 */
export interface TrainingCourseExport {
  /**
   * TrainingCourseID
   */
  trainingCourseId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 课程编码（租户+公司内唯一）
   */
  courseCode: string;

  /**
   * 课程名称
   */
  courseName: string;

  /**
   * 课程类型（入职培训/技能培训/管理培训/安全培训/专业培训）
   */
  courseType: string;

  /**
   * 课程级别（初级/中级/高级/专家）
   */
  courseLevel: string;

  /**
   * 课程描述
   */
  courseDescription: string;

  /**
   * 课程目标
   */
  courseObjectives: string;

  /**
   * 培训时长（小时）
   */
  trainingHours: number;

  /**
   * 主讲讲师
   */
  mainInstructor: string;

  /**
   * 培训方式（线下/线上/混合）
   */
  trainingMethod: string;

  /**
   * 考核方式（考试/实操/作业/无）
   */
  assessmentMethod: string;

  /**
   * 及格分数线
   */
  passingScore: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（1=启用 0=禁用）
   */
  trainingCourseStatus: number;

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

