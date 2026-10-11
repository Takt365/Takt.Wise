// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：operation.types.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * APS 工序排程（APS_Order → Operation，关联 RoutingItem 与 WC/Resource）
 * 对应前端 TaktApsOperationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsOperation
 * @description 对应后端 TaktApsOperationDto
 */
export interface ApsOperation extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId?: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode?: string;

  /**
   * 行号（工序序号）
   */
  lineNumber?: number;

  /**
   * 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes?: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes?: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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
 * ApsOperation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsOperationExport
 * @description 对应后端 TaktApsOperationExportDto
 */
export interface ApsOperationExport {
  /**
   * ApsOperationID
   */
  apsOperationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * APS 订单 ID（主子表关系）
   */
  apsOrderId: string;

  /**
   * APS 订单编码（冗余）
   */
  apsOrderCode: string;

  /**
   * 行号（工序序号）
   */
  lineNumber: number;

  /**
   * 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
   */
  workCenterResourceId?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 计划工时（分钟）
   */
  plannedDurationMinutes: number;

  /**
   * 换型时间（分钟）
   */
  changeoverMinutes: number;

  /**
   * 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
   */
  operationStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
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

