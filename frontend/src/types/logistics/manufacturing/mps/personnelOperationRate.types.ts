// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：personnelOperationRate.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 人员稼动率实体（生产线人员作业效率记录） 人员稼动率(%) = 在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
 * 对应前端 TaktPersonnelOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PersonnelOperationRate
 * @description 对应后端 TaktPersonnelOperationRateDto
 */
export interface PersonnelOperationRate extends CompanyDtoBase {

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 生产班组名称
   */
  TeamCodeName?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount?: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount?: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount?: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount?: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime?: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime?: number;

  /**
   * 休息时间（分钟）
   */
  breakTime?: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime?: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate?: number;

  /**
   * 计划产量
   */
  plannedOutput?: number;

  /**
   * 实际产量
   */
  actualOutput?: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity?: number;

  /**
   * 不良品数量
   */
  defectiveQuantity?: number;

  /**
   * 良品率（%）
   */
  yieldRate?: number;

  /**
   * 工作效率（%）
   */
  workEfficiency?: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours?: number;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 主管（选项 TaktEmployees/options，存员工姓名或工号）
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus?: number;

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
 * PersonnelOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PersonnelOperationRateExport
 * @description 对应后端 TaktPersonnelOperationRateExportDto
 */
export interface PersonnelOperationRateExport {
  /**
   * PersonnelOperationRateID
   */
  personnelOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode: string;

  /**
   * 生产班组名称
   */
  TeamCodeName?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 计划直接人员数量
   */
  plannedDirectPersonnelCount: number;

  /**
   * 实际直接人员数量
   */
  actualDirectPersonnelCount: number;

  /**
   * 计划间接人员数量
   */
  plannedIndirectPersonnelCount: number;

  /**
   * 实际间接人员数量
   */
  actualIndirectPersonnelCount: number;

  /**
   * 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
   */
  plannedWorkTime: number;

  /**
   * 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
   */
  actualWorkTime: number;

  /**
   * 休息时间（分钟）
   */
  breakTime: number;

  /**
   * 空闲时间（分钟）。等料、设备调试等非作业时间。
   */
  idleTime: number;

  /**
   * 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
   */
  personnelOperationRate: number;

  /**
   * 计划产量
   */
  plannedOutput: number;

  /**
   * 实际产量
   */
  actualOutput: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity: number;

  /**
   * 不良品数量
   */
  defectiveQuantity: number;

  /**
   * 良品率（%）
   */
  yieldRate: number;

  /**
   * 工作效率（%）
   */
  workEfficiency: number;

  /**
   * 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
   */
  idleReasonType?: number;

  /**
   * 空闲原因描述
   */
  idleReason?: string;

  /**
   * 加班时间（分钟）
   */
  overtimeHours: number;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 主管（选项 TaktEmployees/options，存员工姓名或工号）
   */
  supervisor?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

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

