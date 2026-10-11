// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：masterProductionSchedule.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 主生产计划 MPS 头表（公司级；MDS 下推，成品级何时做多少、粗产能校验）
 * 对应前端 TaktMasterProductionScheduleDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MasterProductionSchedule
 * @description 对应后端 TaktMasterProductionScheduleDto
 */
export interface MasterProductionSchedule extends ApprovalDtoBase {

  /**
   * MPS 编码
   */
  mpsCode?: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart?: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd?: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType?: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus?: number;

  /**
   * MPS 明细行（子表，级联保存）
   */
  lines?: MasterProductionScheduleLineCreate[];

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
 * MasterProductionSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterProductionScheduleExport
 * @description 对应后端 TaktMasterProductionScheduleExportDto
 */
export interface MasterProductionScheduleExport {
  /**
   * MasterProductionScheduleID
   */
  masterProductionScheduleId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MPS 编码
   */
  mpsCode: string;

  /**
   * 来源 MDS 头表 ID（Demand 层上游，关联 TaktMasterDemandSchedule.Id）
   */
  masterDemandScheduleId?: string;

  /**
   * 来源 MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 计划周期开始
   */
  planPeriodStart: string;

  /**
   * 计划周期结束
   */
  planPeriodEnd: string;

  /**
   * 时间桶粒度（字典 mps_time_bucket_type；0=日，1=周，2=月）
   */
  bucketType: number;

  /**
   * 计划状态（字典 sys_normal_disable；1=启用，0=禁用，2=锁定）
   */
  scheduleStatus: number;

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

