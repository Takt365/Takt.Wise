// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：masterProductionScheduleLine.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 主生产计划 MPS 行（MDS 明细 + 时间桶 + ATP；下推 MRP）
 * 对应前端 TaktMasterProductionScheduleLineDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MasterProductionScheduleLine
 * @description 对应后端 TaktMasterProductionScheduleLineDto
 */
export interface MasterProductionScheduleLine extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId?: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode?: string;

  /**
   * 来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）
   */
  masterDemandScheduleLineId?: string;

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
   * 毛需求数量
   */
  grossRequirement?: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts?: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand?: number;

  /**
   * 净需求数量
   */
  netRequirement?: number;

  /**
   * 计划订单数量（MPS 产出，供 MRP 展开）
   */
  plannedOrderQuantity?: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity?: number;

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
 * MasterProductionScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MasterProductionScheduleLineExport
 * @description 对应后端 TaktMasterProductionScheduleLineExportDto
 */
export interface MasterProductionScheduleLineExport {
  /**
   * MasterProductionScheduleLineID
   */
  masterProductionScheduleLineId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * MPS 头表 ID（主子表关系）
   */
  masterProductionScheduleId: string;

  /**
   * MPS 编码（冗余）
   */
  mpsCode: string;

  /**
   * 来源 MDS 行 ID（关联 TaktMasterDemandScheduleLine.Id）
   */
  masterDemandScheduleLineId?: string;

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
   * 毛需求数量
   */
  grossRequirement: number;

  /**
   * 预计入库（计划接收）
   */
  scheduledReceipts: number;

  /**
   * 预计可用库存（期初预计库存）
   */
  projectedOnHand: number;

  /**
   * 净需求数量
   */
  netRequirement: number;

  /**
   * 计划订单数量（MPS 产出，供 MRP 展开）
   */
  plannedOrderQuantity: number;

  /**
   * 可承诺量 ATP
   */
  atpQuantity: number;

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

