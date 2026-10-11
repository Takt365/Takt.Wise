// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：inspectionStandard.types.ts
// 创建时间：2026-06-30
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
 * 检验标准实体（IQC/IPQC/FQC通用）
 * 对应前端 TaktInspectionStandardDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 InspectionStandard
 * @description 对应后端 TaktInspectionStandardDto
 */
export interface InspectionStandard extends CompanyDtoBase {

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode?: string;

  /**
   * 检验标准名称
   */
  standardName?: string;

  /**
   * 检验类型（字典 logistics_quality_inspection_type）
   */
  inspectionType?: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode?: string;

  /**
   * 物料类别名称
   */
  materialCategoryName?: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

  /**
   * 检验标准状态（字典 logistics_quality_standard_status）
   */
  standardStatus?: number;

  /**
   * 检验标准明细列表（主子表关系）（子表，级联保存）
   */
  items?: InspectionStandardItemCreate[];

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
 * InspectionStandard 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 InspectionStandardExport
 * @description 对应后端 TaktInspectionStandardExportDto
 */
export interface InspectionStandardExport {
  /**
   * InspectionStandardID
   */
  inspectionStandardId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 检验标准编码（唯一索引）
   */
  standardCode: string;

  /**
   * 检验标准名称
   */
  standardName: string;

  /**
   * 检验类型（字典 logistics_quality_inspection_type）
   */
  inspectionType: number;

  /**
   * 物料类别编码
   */
  materialCategoryCode: string;

  /**
   * 物料类别名称
   */
  materialCategoryName: string;

  /**
   * 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
   */
  samplingSchemeCode?: string;

  /**
   * 抽样方案名称
   */
  samplingSchemeName?: string;

  /**
   * 检验标准描述
   */
  standardDescription?: string;

  /**
   * 检验标准状态（字典 logistics_quality_standard_status）
   */
  standardStatus: number;

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

