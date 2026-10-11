// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issuePcbaRework.types.ts
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
 * 品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
 * 对应前端 TaktQualityIssuePcbaReworkDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssuePcbaRework
 * @description 对应后端 TaktQualityIssuePcbaReworkDto
 */
export interface QualityIssuePcbaRework extends CompanyDtoBase {
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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost?: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes?: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes?: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost?: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost?: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses?: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost?: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName1?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteCode?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2?: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

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
 * QualityIssuePcbaRework 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssuePcbaReworkExport
 * @description 对应后端 TaktQualityIssuePcbaReworkExportDto
 */
export interface QualityIssuePcbaReworkExport {
  /**
   * QualityIssuePcbaReworkID
   */
  qualityIssuePcbaReworkId: string;

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
   * PCBA不良内容(Parts/Components)
   */
  pcbaDefectParts?: string;

  /**
   * PCBA选别・改修费用（元）
   */
  pcbaReworkCost: number;

  /**
   * PCBA选别・改修时间（分钟）
   */
  pcbaReworkTimeMinutes: number;

  /**
   * PCBA再检查时间（分钟）
   */
  pcbaReinspectionTimeMinutes: number;

  /**
   * PCBA交通费、旅费（元）
   */
  pcbaTravelCost: number;

  /**
   * PCBA仓库管理费（元）
   */
  pcbaWarehouseCost: number;

  /**
   * PCBA选别・改修其他费用（元）
   */
  pcbaOtherExpenses: number;

  /**
   * PCBA选别・改修备注
   */
  pcbaReworkNote?: string;

  /**
   * PCBA向顾客的费用请求（元）
   */
  pcbaScrapCost: number;

  /**
   * PCBA顾客名
   */
  pcbaCustomerName1?: string;

  /**
   * PCBA Debit Note No
   */
  pcbaDebitNoteCode?: string;

  /**
   * PCBA其他费用（元）
   */
  pcbaOtherExpenses2: number;

  /**
   * PCBA备注
   */
  pcbaNote?: string;

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

