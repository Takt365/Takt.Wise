// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：productionDispatch.types.ts
// 创建时间：2026-07-13
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
 * 生产派工单（Prod_Order → Dispatch → MES 报工）
 * 对应前端 TaktProductionDispatchDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionDispatch
 * @description 对应后端 TaktProductionDispatchDto
 */
export interface ProductionDispatch extends CompanyDtoBase {

  /**
   * 派工单编码
   */
  dispatchCode?: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder.Id，选项 TaktProductionOrders/options）
   */
  productionOrderId?: string;

  /**
   * 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
   */
  prodOrderCode?: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity?: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus?: number;

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
 * ProductionDispatch 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionDispatchExport
 * @description 对应后端 TaktProductionDispatchExportDto
 */
export interface ProductionDispatchExport {
  /**
   * ProductionDispatchID
   */
  productionDispatchId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 派工单编码
   */
  dispatchCode: string;

  /**
   * 生产工单 ID（关联 TaktProductionOrder.Id，选项 TaktProductionOrders/options）
   */
  productionOrderId: string;

  /**
   * 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
   */
  prodOrderCode: string;

  /**
   * APS 工序排程 ID（关联 TaktApsOperation.Id，选项 TaktApsOperations/options）
   */
  apsOperationId?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 派工数量
   */
  dispatchQuantity: number;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
   */
  dispatchStatus: number;

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

