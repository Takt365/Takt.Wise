// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issueAssyRework.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 品质问题应对明细 - 组装不良改修应对(组装选别・改修费用)
 * 对应前端 TaktQualityIssueAssyReworkDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssueAssyRework
 * @description 对应后端 TaktQualityIssueAssyReworkDto
 */
export interface QualityIssueAssyRework extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost?: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost?: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost?: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost?: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteCode?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2?: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
   */
  responseGroupCode?: string;

  /**
   * 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  responseGroupName?: string;

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
 * QualityIssueAssyRework 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssueAssyReworkExport
 * @description 对应后端 TaktQualityIssueAssyReworkExportDto
 */
export interface QualityIssueAssyReworkExport {
  /**
   * QualityIssueAssyReworkID
   */
  qualityIssueAssyReworkId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteCode?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
   */
  responseGroupCode?: string;

  /**
   * 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  responseGroupName?: string;

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

