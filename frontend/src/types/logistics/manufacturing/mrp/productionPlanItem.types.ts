// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：productionPlanItem.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mrp 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt生产计划明细实体
 * 对应前端 TaktProductionPlanItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionPlanItem
 * @description 对应后端 TaktProductionPlanItemDto
 */
export interface ProductionPlanItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId?: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售计划编码
   */
  salesForecastCode?: string;

  /**
   * 来源销售计划行号
   */
  salesForecastLineNumber?: number;

  /**
   * 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
   */
  materialRequirementsPlanningItemId?: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
   */
  modelCode?: string;

  /**
   * 机种名称（冗余字段，便于查询展示）
   */
  modelName?: string;

  /**
   * 计划单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity?: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * ProductionPlanItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionPlanItemExport
 * @description 对应后端 TaktProductionPlanItemExportDto
 */
export interface ProductionPlanItemExport {
  /**
   * ProductionPlanItemID
   */
  productionPlanItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  productionPlanId: string;

  /**
   * 生产计划编码（冗余字段，便于查询）
   */
  productionPlanCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
   */
  salesForecastId?: string;

  /**
   * 来源销售计划编码
   */
  salesForecastCode?: string;

  /**
   * 来源销售计划行号
   */
  salesForecastLineNumber?: number;

  /**
   * 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
   */
  materialRequirementsPlanningItemId?: string;

  /**
   * 物料编码（关联 TaktGeneralMaterial.MaterialCode，选项 TaktGeneralMaterials/options）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
   */
  modelCode?: string;

  /**
   * 机种名称（冗余字段，便于查询展示）
   */
  modelName?: string;

  /**
   * 计划单位（字典 logistics_materials_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 计划数量（基本单位数量）
   */
  planQuantity: number;

  /**
   * 计划开工日期
   */
  plannedStartDate?: string;

  /**
   * 计划完工日期
   */
  plannedEndDate?: string;

  /**
   * 已转工单/采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单位成本
   */
  estimatedUnitCost: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

