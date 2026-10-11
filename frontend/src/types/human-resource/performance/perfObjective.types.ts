// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perfObjective.types.ts
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
 * 员工绩效目标
 * 对应前端 TaktPerfObjectiveDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 PerfObjective
 * @description 对应后端 TaktPerfObjectiveDto
 */
export interface PerfObjective extends ApprovalDtoBase {

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
 * PerfObjective 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfObjectiveExport
 * @description 对应后端 TaktPerfObjectiveExportDto
 */
export interface PerfObjectiveExport {
  /**
   * PerfObjectiveID
   */
  perfObjectiveId: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 方案指标 ID
   */
  schemeMetricId: string;

  /**
   * 目标周期（如 2026-Q1、2026-Annual）
   */
  objectivePeriod: string;

  /**
   * 目标描述
   */
  objectiveDescription: string;

  /**
   * 目标值
   */
  targetValue: number;

  /**
   * 实际完成值
   */
  actualValue: number;

  /**
   * 完成百分比（%）
   */
  completionPercentage: number;

  /**
   * 目标权重（%）
   */
  objectiveWeight: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 截止日期
   */
  dueDate: string;

  /**
   * 目标达成说明
   */
  achievementNotes: string;

  /**
   * 业务状态（0=待确认 1=进行中 2=已完成）
   */
  objectiveStatus: number;

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

