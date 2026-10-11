// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：plannedOrder.types.ts
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
 * 计划订单（MRP 自制件净需求固化为可排程计划订单，下推 APS）
 * 对应前端 TaktPlannedOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PlannedOrder
 * @description 对应后端 TaktPlannedOrderDto
 */
export interface PlannedOrder extends CompanyDtoBase {

  /**
   * 计划订单编码
   */
  plannedOrderCode?: string;

  /**
   * 来源 MRP 头表 ID
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MRP 明细行 ID
   */
  materialRequirementsPlanningItemId?: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode?: string;

  /**
   * 计划数量
   */
  plannedQuantity?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode，选项 TaktRoutings/options，DictValue=RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
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
 * PlannedOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PlannedOrderExport
 * @description 对应后端 TaktPlannedOrderExportDto
 */
export interface PlannedOrderExport {
  /**
   * PlannedOrderID
   */
  plannedOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 计划订单编码
   */
  plannedOrderCode: string;

  /**
   * 来源 MRP 头表 ID
   */
  materialRequirementsPlanningId?: string;

  /**
   * 来源 MRP 编码（冗余）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 来源 MRP 明细行 ID
   */
  materialRequirementsPlanningItemId?: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode: string;

  /**
   * 计划数量
   */
  plannedQuantity: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * 工艺路线编码（关联 TaktRouting.RoutingCode，选项 TaktRoutings/options，DictValue=RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划订单状态（字典 planned_order_status；0=计划，1=确认，2=已释放，3=已关闭）
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

