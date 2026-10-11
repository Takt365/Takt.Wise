// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：masterDemandSchedule.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 主需求计划 MDS 头表（公司级；承接销售订单与预测，下推 MPS）
 * 对应前端 TaktMasterDemandScheduleDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MasterDemandSchedule
 * @description 对应后端 TaktMasterDemandScheduleDto
 */
export interface MasterDemandSchedule extends ApprovalDtoBase {

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
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
   * MDS 明细行（按物料与时间桶）（子表，级联保存）
   */
  lines?: MasterDemandScheduleLineCreate[];

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
 * MasterDemandSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterDemandScheduleExport
 * @description 对应后端 TaktMasterDemandScheduleExportDto
 */
export interface MasterDemandScheduleExport {
  /**
   * MasterDemandScheduleID
   */
  masterDemandScheduleId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * MDS 编码（租户+公司+工厂内业务唯一）
   */
  mdsCode: string;

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

