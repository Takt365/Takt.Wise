// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：workOrderLabor.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 维护工单报工明细实体（主子表：挂载于维护工单）
 * 对应前端 TaktMaintenanceWorkOrderLaborDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceWorkOrderLabor
 * @description 对应后端 TaktMaintenanceWorkOrderLaborDto
 */
export interface MaintenanceWorkOrderLabor extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours?: number;

  /**
   * 小时费率
   */
  hourlyRate?: number;

  /**
   * 人工成本
   */
  laborCost?: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus?: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * MaintenanceWorkOrderLabor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceWorkOrderLaborExport
 * @description 对应后端 TaktMaintenanceWorkOrderLaborExportDto
 */
export interface MaintenanceWorkOrderLaborExport {
  /**
   * MaintenanceWorkOrderLaborID
   */
  maintenanceWorkOrderLaborId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 员工ID（序列化为string以避免Javascript精度问题）
   */
  employeeId?: string;

  /**
   * 员工编码
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余）
   */
  employeeName?: string;

  /**
   * 报工日期
   */
  workDate: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 工时（小时）
   */
  workHours: number;

  /**
   * 小时费率
   */
  hourlyRate: number;

  /**
   * 人工成本
   */
  laborCost: number;

  /**
   * 作业描述
   */
  operationDescription?: string;

  /**
   * 报工确认状态（0=待确认，1=已确认）
   */
  confirmationStatus: number;

  /**
   * 确认时间
   */
  confirmedAt?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

