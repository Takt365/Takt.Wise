// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：fqcOrderItem.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * FQC出货检验单明细实体
 * 对应前端 TaktFqcOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FqcOrderItem
 * @description 对应后端 TaktFqcOrderItemDto
 */
export interface FqcOrderItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * FQC检验单 ID（选项 TaktFqcOrders/options，DictValue=Id）
   */
  fqcOrderId?: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode?: string;

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
   * 批次号
   */
  batchCode?: string;

  /**
   * 入库数量
   */
  warehouseQuantity?: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
   */
  standardCode?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  inspectorBy?: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: FqcDefectHandlingCreate[];

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
 * FqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FqcOrderItemExport
 * @description 对应后端 TaktFqcOrderItemExportDto
 */
export interface FqcOrderItemExport {
  /**
   * FqcOrderItemID
   */
  fqcOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * FQC检验单 ID（选项 TaktFqcOrders/options，DictValue=Id）
   */
  fqcOrderId: string;

  /**
   * FQC检验单编码（冗余字段，便于查询）
   */
  fqcOrderCode: string;

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
   * 批次号
   */
  batchCode?: string;

  /**
   * 入库数量
   */
  warehouseQuantity: number;

  /**
   * 检验标准编码（选项 TaktInspectionStandards/options；DictValue=StandardCode）
   */
  standardCode: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（字典 logistics_quality_inspection_method）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 抽检序列号
   */
  sampleSerialCode?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（选项 TaktEmployees/options；DictValue=EmployeeCode）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 判定状态（字典 logistics_quality_judge_status）
   */
  judgeStatus: number;

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

