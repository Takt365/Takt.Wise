// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：productionOrder.types.ts
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
 * 生产工单实体
 * 对应前端 TaktProductionOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionOrder
 * @description 对应后端 TaktProductionOrderDto
 */
export interface ProductionOrder extends CompanyDtoBase {

  /**
   * 工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
   */
  prodOrderType?: string;

  /**
   * 工单号
   */
  prodOrderCode?: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode?: string;

  /**
   * 工单数量
   */
  prodOrderQty?: number;

  /**
   * 已生产数量
   */
  producedQty?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）
   */
  unitOfMeasure?: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialCode?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime?: string;

  /**
   * 计划完工时间
   */
  plannedEndTime?: string;

  /**
   * 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
   */
  orderStatus?: number;

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
 * ProductionOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionOrderExport
 * @description 对应后端 TaktProductionOrderExportDto
 */
export interface ProductionOrderExport {
  /**
   * ProductionOrderID
   */
  productionOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 工单类别（字典 logistics_manufacturing_prod_order_type，存 DictValue，如 ZDTA/ZDTB/ZDTC/ZDTD/ZDTE/ZDTF）
   */
  prodOrderType: string;

  /**
   * 工单号
   */
  prodOrderCode: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode: string;

  /**
   * 工单数量
   */
  prodOrderQty: number;

  /**
   * 已生产数量
   */
  producedQty: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，存 DictValue）
   */
  unitOfMeasure: string;

  /**
   * 实际开始日期
   */
  actualStartDate?: string;

  /**
   * 实际完成日期
   */
  actualEndDate?: string;

  /**
   * 优先级（字典 sys_priority_level；1=最高 2=高 3=普通 4=低）
   */
  priority: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options，存 WorkCenterCode，ExtValue=PlantCode 过滤）
   */
  workCenter?: string;

  /**
   * 生产批次
   */
  prodBatch?: string;

  /**
   * 序列号
   */
  serialCode?: string;

  /**
   * 工艺路线编码
   */
  routingCode?: string;

  /**
   * 来源计划订单 ID（关联 TaktPlannedOrder.Id，选项 TaktPlannedOrders/options，ExtValue=PlantCode 过滤）
   */
  plannedOrderId?: string;

  /**
   * 来源 APS 订单 ID（关联 TaktApsOrder.Id，选项 TaktApsOrders/options，ExtValue=PlantCode 过滤）
   */
  apsOrderId?: string;

  /**
   * 计划开工时间
   */
  plannedStartTime?: string;

  /**
   * 计划完工时间
   */
  plannedEndTime?: string;

  /**
   * 状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成）
   */
  orderStatus: number;

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

