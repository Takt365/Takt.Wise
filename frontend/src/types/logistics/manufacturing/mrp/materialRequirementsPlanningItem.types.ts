// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：materialRequirementsPlanningItem.types.ts
// 创建时间：2026-07-23
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
 * 物料需求计划 MRP 明细行（物料 + 需求日期 + 净需求数量）
 * 对应前端 TaktMaterialRequirementsPlanningItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialRequirementsPlanningItem
 * @description 对应后端 TaktMaterialRequirementsPlanningItemDto
 */
export interface MaterialRequirementsPlanningItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId?: string;

  /**
   * MRP 编码（冗余字段，便于查询）
   */
  materialRequirementsPlanningCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode，可选）
   */
  modelCode?: string;

  /**
   * 机种名称（冗余）
   */
  modelName?: string;

  /**
   * 父项物料编码（BOM 展开上级，可选）
   */
  parentMaterialCode?: string;

  /**
   * BOM 层级（1=顶层成品）
   */
  bomLevel?: number;

  /**
   * 需求日期
   */
  requirementDate?: string;

  /**
   * 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  planUnit?: string;

  /**
   * 毛需求数量（基本单位数量）
   */
  grossRequirement?: number;

  /**
   * 计划接收数量（在途/已订未收等，运算快照）
   */
  scheduledReceipts?: number;

  /**
   * 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
   */
  onHandQuantity?: number;

  /**
   * 预计可用库存（运算后 POH 快照）
   */
  projectedOnHand?: number;

  /**
   * 净需求数量（基本单位数量）
   */
  netRequirement?: number;

  /**
   * 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
   */
  procurementType?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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
 * MaterialRequirementsPlanningItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialRequirementsPlanningItemExport
 * @description 对应后端 TaktMaterialRequirementsPlanningItemExportDto
 */
export interface MaterialRequirementsPlanningItemExport {
  /**
   * MaterialRequirementsPlanningItemID
   */
  materialRequirementsPlanningItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  materialRequirementsPlanningId: string;

  /**
   * MRP 编码（冗余字段，便于查询）
   */
  materialRequirementsPlanningCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 物料规格（回填：随物料）
   */
  materialSpecification?: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode，可选）
   */
  modelCode?: string;

  /**
   * 机种名称（冗余）
   */
  modelName?: string;

  /**
   * 父项物料编码（BOM 展开上级，可选）
   */
  parentMaterialCode?: string;

  /**
   * BOM 层级（1=顶层成品）
   */
  bomLevel: number;

  /**
   * 需求日期
   */
  requirementDate: string;

  /**
   * 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
   */
  planUnit: string;

  /**
   * 毛需求数量（基本单位数量）
   */
  grossRequirement: number;

  /**
   * 计划接收数量（在途/已订未收等，运算快照）
   */
  scheduledReceipts: number;

  /**
   * 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
   */
  onHandQuantity: number;

  /**
   * 预计可用库存（运算后 POH 快照）
   */
  projectedOnHand: number;

  /**
   * 净需求数量（基本单位数量）
   */
  netRequirement: number;

  /**
   * 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
   */
  procurementType: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是）
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

