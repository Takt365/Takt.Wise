// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：productionPlan.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt生产计划实体（公司级；MRP 运算产出自制件计划，非 MPS 上游）
 * 对应前端 TaktProductionPlanDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 ProductionPlan
 * @description 对应后端 TaktProductionPlanDto
 */
export interface ProductionPlan extends ApprovalDtoBase {

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode?: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate?: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd?: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeCode，选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 计划总金额
   */
  totalAmount?: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus?: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus?: number;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * 生产计划明细列表（主子表关系）（子表，级联保存）
   */
  items?: ProductionPlanItemCreate[];

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
 * ProductionPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionPlanExport
 * @description 对应后端 TaktProductionPlanExportDto
 */
export interface ProductionPlanExport {
  /**
   * ProductionPlanID
   */
  productionPlanId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产计划编码（租户+公司+工厂内业务唯一）
   */
  productionPlanCode: string;

  /**
   * 来源物料需求计划 ID（Planning 层 MRP 上游，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源销售预测ID（Demand 层追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测编码（冗余字段，便于查询）
   */
  salesForecastCode?: string;

  /**
   * 计划编制日期
   */
  planDate: string;

  /**
   * 计划周期开始日期
   */
  planPeriodStart: string;

  /**
   * 计划周期结束日期
   */
  planPeriodEnd: string;

  /**
   * 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（关联 TaktEmployee.EmployeeCode，选项 TaktEmployees/options，DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 计划总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 计划总金额
   */
  totalAmount: number;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 已转工单/采购金额
   */
  convertedAmount: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  planStatus: number;

  /**
   * 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
   */
  convertedStatus: number;

  /**
   * 计划说明
   */
  planDescription?: string;

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

