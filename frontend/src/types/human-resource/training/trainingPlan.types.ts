// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/training
// 文件名称：trainingPlan.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/training 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 培训计划（年度/季度/专项）
 * 对应前端 TaktTrainingPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TrainingPlan
 * @description 对应后端 TaktTrainingPlanDto
 */
export interface TrainingPlan extends ApprovalDtoBase {

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
 * TrainingPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TrainingPlanExport
 * @description 对应后端 TaktTrainingPlanExportDto
 */
export interface TrainingPlanExport {
  /**
   * TrainingPlanID
   */
  trainingPlanId: string;

  /**
   * 计划编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 计划名称
   */
  planName: string;

  /**
   * 计划年度
   */
  planYear: number;

  /**
   * 计划类型（年度/季度/月度/专项）
   */
  planType: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 计划开始日期
   */
  startDate: string;

  /**
   * 计划结束日期
   */
  endDate: string;

  /**
   * 培训目标
   */
  trainingObjectives: string;

  /**
   * 计划培训人数
   */
  plannedHeadcount: number;

  /**
   * 培训预算（元）
   */
  trainingBudget: number;

  /**
   * 计划说明
   */
  description: string;

  /**
   * 业务状态（1=启用 0=禁用）
   */
  trainingPlanStatus: number;

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

