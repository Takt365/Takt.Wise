// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：supplierEvaluationItem.types.ts
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
 * 供应商评价考核项目明细实体
 * 对应前端 TaktSupplierEvaluationItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SupplierEvaluationItem
 * @description 对应后端 TaktSupplierEvaluationItemDto
 */
export interface SupplierEvaluationItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
   */
  evaluationId?: string;

  /**
   * 评价表编码（冗余字段，便于查询）
   */
  supplierEvaluationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 评价类别类型（字典 logistics_quality_evaluation_category）
   */
  categoryType?: number;

  /**
   * 评价项目名称
   */
  itemName?: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（字典 logistics_quality_supplier_rating）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired?: number;

  /**
   * 整改期限
   */
  rectificationDeadline?: string;

  /**
   * 整改状态（字典 logistics_quality_rectification_status）
   */
  rectificationStatus?: number;

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
 * SupplierEvaluationItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierEvaluationItemExport
 * @description 对应后端 TaktSupplierEvaluationItemExportDto
 */
export interface SupplierEvaluationItemExport {
  /**
   * SupplierEvaluationItemID
   */
  supplierEvaluationItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
   */
  evaluationId: string;

  /**
   * 评价表编码（冗余字段，便于查询）
   */
  supplierEvaluationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 评价类别类型（字典 logistics_quality_evaluation_category）
   */
  categoryType: number;

  /**
   * 评价项目名称
   */
  itemName: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（字典 logistics_quality_supplier_rating）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired: number;

  /**
   * 整改期限
   */
  rectificationDeadline?: string;

  /**
   * 整改状态（字典 logistics_quality_rectification_status）
   */
  rectificationStatus: number;

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

