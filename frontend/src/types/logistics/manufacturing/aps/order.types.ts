// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：order.types.ts
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
 * APS 排程订单（Planned Order 释放后进入 APS 排程）
 * 对应前端 TaktApsOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsOrder
 * @description 对应后端 TaktApsOrderDto
 */
export interface ApsOrder extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * APS 订单编码
   */
  apsOrderCode?: string;

  /**
   * 来源计划订单 ID
   */
  plannedOrderId?: string;

  /**
   * 来源计划订单编码（冗余）
   */
  plannedOrderCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 订单数量
   */
  orderQuantity?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

  /**
   * 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
   */
  orderStatus?: number;

  /**
   * 关联 APS 排程批次 ID（可选）
   */
  apsScheduleId?: string;

  /**
   * APS 工序排程列表（子表，级联保存）
   */
  operations?: ApsOperationCreate[];

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
 * ApsOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsOrderExport
 * @description 对应后端 TaktApsOrderExportDto
 */
export interface ApsOrderExport {
  /**
   * ApsOrderID
   */
  apsOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * APS 订单编码
   */
  apsOrderCode: string;

  /**
   * 来源计划订单 ID
   */
  plannedOrderId?: string;

  /**
   * 来源计划订单编码（冗余）
   */
  plannedOrderCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 订单数量
   */
  orderQuantity: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

  /**
   * 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
   */
  routingCode?: string;

  /**
   * 计划开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划结束时间
   */
  plannedEndTime?: string;

  /**
   * APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
   */
  orderStatus: number;

  /**
   * 关联 APS 排程批次 ID（可选）
   */
  apsScheduleId?: string;

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

