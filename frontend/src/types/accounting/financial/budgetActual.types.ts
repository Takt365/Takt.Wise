// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：budgetActual.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 预算实绩实体（管理会计 Budget vs Actual；CAS 全面预算管理实务 / 国际通用管理会计） 差异约定：差异金额 = 实绩 − 预算；差异率 = 差异 / |预算|（预算为 0 时为 0）。 唯一键：租户 + 公司 + 工厂 + 期间 + 成本中心 + 预算项
 * 对应前端 TaktBudgetActualDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BudgetActual
 * @description 对应后端 TaktBudgetActualDto
 */
export interface BudgetActual extends CompanyDtoBase {

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode?: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode?: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode?: string;

  /**
   * 预算项名称
   */
  budgetItemName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType?: number;

  /**
   * 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
   */
  measureType?: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount?: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount?: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount?: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent?: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual?: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount?: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount?: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus?: number;

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
 * BudgetActual 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BudgetActualExport
 * @description 对应后端 TaktBudgetActualExportDto
 */
export interface BudgetActualExport {
  /**
   * BudgetActualID
   */
  budgetActualId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 会计期间编码（YYYYMM）
   */
  periodCode: string;

  /**
   * 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
   */
  costCenterCode: string;

  /**
   * 成本中心名称（冗余）
   */
  costCenterName?: string;

  /**
   * 预算项编码
   */
  budgetItemCode: string;

  /**
   * 预算项名称
   */
  budgetItemName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
   */
  budgetType: number;

  /**
   * 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
   */
  measureType: number;

  /**
   * 本期预算金额（或数量，视 MeasureType）
   */
  budgetAmount: number;

  /**
   * 本期实绩金额（或数量）
   */
  actualAmount: number;

  /**
   * 本期差异金额（= 实绩 − 预算）
   */
  varianceAmount: number;

  /**
   * 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
   */
  variancePercent: number;

  /**
   * 上年同期实绩（比较分析）
   */
  priorPeriodActual: number;

  /**
   * 本年累计预算
   */
  ytdBudgetAmount: number;

  /**
   * 本年累计实绩
   */
  ytdActualAmount: number;

  /**
   * 本年累计差异（= 本年累计实绩 − 本年累计预算）
   */
  ytdVarianceAmount: number;

  /**
   * 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  budgetActualStatus: number;

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

