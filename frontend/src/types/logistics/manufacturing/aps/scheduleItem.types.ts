// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：scheduleItem.types.ts
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
 * APS排程明细（排程的具体工序任务）
 * 对应前端 TaktApsScheduleItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsScheduleItem
 * @description 对应后端 TaktApsScheduleItemDto
 */
export interface ApsScheduleItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId?: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode?: string;

  /**
   * APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
   */
  workOrderCode?: string;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心描述
   */
  workCenterDescription?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 工序序号
   */
  processSequence?: number;

  /**
   * 工序标准ST值
   */
  processStandardST?: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit?: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes?: number;

  /**
   * 计划数量
   */
  planQuantity?: number;

  /**
   * 计划开始时间
   */
  planStartTime?: string;

  /**
   * 计划结束时间
   */
  planEndTime?: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus?: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority?: number;

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
 * ApsScheduleItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsScheduleItemExport
 * @description 对应后端 TaktApsScheduleItemExportDto
 */
export interface ApsScheduleItemExport {
  /**
   * ApsScheduleItemID
   */
  apsScheduleItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  apsScheduleId: string;

  /**
   * APS排程编码（冗余字段，便于查询）
   */
  apsScheduleCode: string;

  /**
   * APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
   */
  apsOrderId?: string;

  /**
   * APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
   */
  apsOperationId?: string;

  /**
   * 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
   */
  routingItemId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
   */
  workOrderCode: string;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode: string;

  /**
   * 产品名称
   */
  productName: string;

  /**
   * 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 工作中心描述
   */
  workCenterDescription?: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 工序序号
   */
  processSequence: number;

  /**
   * 工序标准ST值
   */
  processStandardST: number;

  /**
   * 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
   */
  processStandardSTUnit: number;

  /**
   * 额外时间（分钟），如换模、调试、清洁等准备时间
   */
  extraMinutes: number;

  /**
   * 计划数量
   */
  planQuantity: number;

  /**
   * 计划开始时间
   */
  planStartTime: string;

  /**
   * 计划结束时间
   */
  planEndTime: string;

  /**
   * 实际开始时间
   */
  actualStartTime?: string;

  /**
   * 实际结束时间
   */
  actualEndTime?: string;

  /**
   * 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
   */
  processStatus: number;

  /**
   * 优先级（0=普通，1=紧急，2=特急）
   */
  priority: number;

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

