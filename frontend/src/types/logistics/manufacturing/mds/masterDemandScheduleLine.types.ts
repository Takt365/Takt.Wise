// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mds
// 文件名称：masterDemandScheduleLine.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 主需求计划 MDS 行（物料 + 时间桶 + 需求来源）
 * 对应前端 TaktMasterDemandScheduleLineDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MasterDemandScheduleLine
 * @description 对应后端 TaktMasterDemandScheduleLineDto
 */
export interface MasterDemandScheduleLine extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId?: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode?: string;

  /**
   * 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
   */
  demandSourceType?: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode?: string;

  /**
   * 时间桶开始
   */
  bucketStart?: string;

  /**
   * 时间桶结束
   */
  bucketEnd?: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity?: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure?: string;

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
 * MasterDemandScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterDemandScheduleLineExport
 * @description 对应后端 TaktMasterDemandScheduleLineExportDto
 */
export interface MasterDemandScheduleLineExport {
  /**
   * MasterDemandScheduleLineID
   */
  masterDemandScheduleLineId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * MDS 头表 ID（主子表关系）
   */
  masterDemandScheduleId: string;

  /**
   * MDS 编码（冗余）
   */
  mdsCode: string;

  /**
   * 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
   */
  demandSourceType: number;

  /**
   * 来源销售订单 ID（可选）
   */
  salesOrderId?: string;

  /**
   * 来源销售订单行号（可选；与 SalesOrderId 成对）
   */
  salesOrderLineNumber?: number;

  /**
   * 来源销售预测 ID（可选；预测/计划类需求）
   */
  salesForecastId?: string;

  /**
   * 来源销售预测行号（可选；与 SalesForecastId 成对）
   */
  salesForecastLineNumber?: number;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode: string;

  /**
   * 时间桶开始
   */
  bucketStart: string;

  /**
   * 时间桶结束
   */
  bucketEnd: string;

  /**
   * 需求数量（基本单位）
   */
  demandQuantity: number;

  /**
   * 计量单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  unitOfMeasure: string;

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

