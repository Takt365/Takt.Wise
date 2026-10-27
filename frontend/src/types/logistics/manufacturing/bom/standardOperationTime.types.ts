// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：standardOperationTime.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 标准工时实体
 * 对应前端 TaktStandardOperationTimeDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 StandardOperationTime
 * @description 对应后端 TaktStandardOperationTimeDto
 */
export interface StandardOperationTime extends ApprovalDtoBase {

  /**
   * 物料编码（选项 TaktGeneralMaterials/options）
   */
  materialCode?: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter?: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit，默认 MIN）
   */
  timeUnit?: string;

  /**
   * 标准点数
   */
  standardShorts?: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit，默认 SHORT）
   */
  pointsUnit?: string;

  /**
   * 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate?: number;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

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
 * StandardOperationTime 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardOperationTimeExport
 * @description 对应后端 TaktStandardOperationTimeExportDto
 */
export interface StandardOperationTimeExport {
  /**
   * StandardOperationTimeID
   */
  standardOperationTimeId: string;

  /**
   * 工厂代码（选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options）
   */
  materialCode: string;

  /**
   * 工作中心（选项 TaktWorkCenters/options，按工厂 ExtValue 过滤）
   */
  workCenter: string;

  /**
   * 工序描述
   */
  operationDesc?: string;

  /**
   * 标准工时（分钟）
   */
  standardMinutes: number;

  /**
   * 工时单位（字典 logistics_manufacturing_time_unit，默认 MIN）
   */
  timeUnit: string;

  /**
   * 标准点数
   */
  standardShorts: number;

  /**
   * 点数单位（字典 logistics_manufacturing_points_unit，默认 SHORT）
   */
  pointsUnit: string;

  /**
   * 点数转分钟汇率（decimal，精度 3 位小数；可选值参见字典 logistics_manufacturing_points_to_minutes_rate：普通=1，AI=0.028，SMT=0.045）
   */
  pointsToMinutesRate: number;

  /**
   * 转换后标准工时（分钟）
   */
  convertedMinutes: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

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

