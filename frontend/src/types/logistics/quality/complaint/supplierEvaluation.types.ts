// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：supplierEvaluation.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 供应商评价考核主表实体
 * 对应前端 TaktSupplierEvaluationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SupplierEvaluation
 * @description 对应后端 TaktSupplierEvaluationDto
 */
export interface SupplierEvaluation extends CompanyDtoBase {

  /**
   * 整改跟进状态（字典 logistics_quality_rectification_status）
   */
  rectificationStatus?: number;

  /**
   * 评价项目明细列表（主子表关系）（子表，级联保存）
   */
  items?: SupplierEvaluationItemCreate[];

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
 * SupplierEvaluation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierEvaluationExport
 * @description 对应后端 TaktSupplierEvaluationExportDto
 */
export interface SupplierEvaluationExport {
  /**
   * SupplierEvaluationID
   */
  supplierEvaluationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 评价表编码（组合唯一索引）
   */
  supplierEvaluationCode: string;

  /**
   * 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
   */
  supplierId: string;

  /**
   * 供应商名称
   */
  supplierName1: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 评价日期
   */
  evaluationDate: string;

  /**
   * 评价周期（字典 logistics_quality_period）
   */
  evaluationPeriod: number;

  /**
   * 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
   */
  evaluationType: number;

  /**
   * 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
   */
  evaluatorByGroupCode?: string;

  /**
   * 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  evaluatorByGroupName?: string;

  /**
   * 评价部门（选项 TaktDepts/tree-options；DictValue=DeptCode）
   */
  evaluationDept?: string;

  /**
   * 总体评级（字典 logistics_quality_supplier_rating）
   */
  overallRating: number;

  /**
   * 综合评分（0-100分）
   */
  totalScore?: number;

  /**
   * 质量评分（0-100分）
   */
  qualityScore?: number;

  /**
   * 交付评分（0-100分）
   */
  deliveryScore?: number;

  /**
   * 价格评分（0-100分）
   */
  priceScore?: number;

  /**
   * 服务评分（0-100分）
   */
  serviceScore?: number;

  /**
   * 技术能力评分（0-100分）
   */
  technicalScore?: number;

  /**
   * 主要优点
   */
  mainStrengths?: string;

  /**
   * 主要问题/不足
   */
  mainIssues?: string;

  /**
   * 改进要求/建议
   */
  improvementRequirements?: string;

  /**
   * 考核结论（字典 logistics_quality_evaluation_conclusion）
   */
  evaluationConclusion: number;

  /**
   * 整改期限（要求完成日期）
   */
  rectificationDeadline?: string;

  /**
   * 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
   */
  attachments?: string;

  /**
   * 评价状态（字典 logistics_quality_evaluation_status）
   */
  evaluationStatus: number;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 整改跟进状态（字典 logistics_quality_rectification_status）
   */
  rectificationStatus: number;

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

