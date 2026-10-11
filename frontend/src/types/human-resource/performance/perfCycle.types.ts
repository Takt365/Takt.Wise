// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/performance
// 文件名称：perfCycle.types.ts
// 创建时间：2026-06-24
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
 * 绩效考核周期日程安排
 * 对应前端 TaktPerfCycleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PerfCycle
 * @description 对应后端 TaktPerfCycleDto
 */
export interface PerfCycle extends CompanyDtoBase {

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
 * PerfCycle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PerfCycleExport
 * @description 对应后端 TaktPerfCycleExportDto
 */
export interface PerfCycleExport {
  /**
   * PerfCycleID
   */
  perfCycleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 周期编码（租户+公司内唯一）
   */
  cycleCode: string;

  /**
   * 周期名称
   */
  cycleName: string;

  /**
   * 周期类型（月度/季度/半年度/年度）
   */
  cycleType: string;

  /**
   * 周期年度
   */
  cycleYear: number;

  /**
   * 周期序号
   */
  cycleSequence: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 目标设定截止日期
   */
  goalSettingDueDate: string;

  /**
   * 自评截止日期
   */
  selfEvaluationDueDate: string;

  /**
   * 主管评审截止日期
   */
  supervisorReviewDueDate: string;

  /**
   * 面谈截止日期
   */
  interviewDueDate: string;

  /**
   * 结果确认截止日期
   */
  resultConfirmationDueDate: string;

  /**
   * 适用部门
   */
  applicableDepartment: string;

  /**
   * 周期说明
   */
  perfCycleDescription: string;

  /**
   * 状态（0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
   */
  cycleScheduleStatus: number;

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

