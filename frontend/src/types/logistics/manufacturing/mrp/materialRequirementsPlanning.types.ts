// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：materialRequirementsPlanning.types.ts
// 创建时间：2026-07-23
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
 * 物料需求计划 MRP 头表（公司级；MPS 下推，产出 TaktPlannedOrder / TaktProductionPlan / TaktPurchasePlan）
 * 对应前端 TaktMaterialRequirementsPlanningDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MaterialRequirementsPlanning
 * @description 对应后端 TaktMaterialRequirementsPlanningDto
 */
export interface MaterialRequirementsPlanning extends ApprovalDtoBase {

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy?: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus?: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

  /**
   * 计划说明
   */
  planDescription?: string;

  /**
   * MRP 需求明细列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialRequirementsPlanningItemCreate[];

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
 * MaterialRequirementsPlanning 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialRequirementsPlanningExport
 * @description 对应后端 TaktMaterialRequirementsPlanningExportDto
 */
export interface MaterialRequirementsPlanningExport {
  /**
   * MaterialRequirementsPlanningID
   */
  materialRequirementsPlanningId: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MRP 编码（租户+公司+工厂内业务唯一）
   */
  materialRequirementsPlanningCode: string;

  /**
   * 来源 MPS 头表 ID（Scheduling 层上游，关联 TaktMasterProductionSchedule.Id）
   */
  masterProductionScheduleId?: string;

  /**
   * 来源 MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层追溯，可选）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

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
   * 计划人员工ID（选项 TaktEmployees/options；DictValue=Id）
   */
  plannerGroupCode?: string;

  /**
   * 计划人（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  planBy: string;

  /**
   * 运算状态（0=草稿，1=运算中，2=已运算，3=已发布，4=失败）
   */
  runStatus: number;

  /**
   * 产出生产计划 ID（运算完成后回写）
   */
  productionPlanId?: string;

  /**
   * 产出生产计划编码（冗余）
   */
  productionPlanCode?: string;

  /**
   * 产出采购计划 ID（运算完成后回写）
   */
  purchasePlanId?: string;

  /**
   * 产出采购计划编码（冗余）
   */
  purchasePlanCode?: string;

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

