// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：profitLoss.types.ts
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
 * 利润表（及综合收益）行实体（CAS 利润表列报 / IAS 1 Statement of Profit or Loss and OCI） 列报层次：收入→成本费用→营业利润→利润总额→所得税→净利润→其他综合收益→综合收益总额。 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
 * 对应前端 TaktProfitLossDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProfitLoss
 * @description 对应后端 TaktProfitLossDto
 */
export interface ProfitLoss extends CompanyDtoBase {

  /**
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode?: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode?: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory?: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine?: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount?: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount?: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount?: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense?: number;

  /**
   * 币种（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus?: number;

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
 * ProfitLoss 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProfitLossExport
 * @description 对应后端 TaktProfitLossExportDto
 */
export interface ProfitLossExport {
  /**
   * ProfitLossID
   */
  profitLossId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 会计期间编码（YYYYMM；利润表报告期）
   */
  periodCode: string;

  /**
   * 报表项目编码（利润表/综合收益表行项目）
   */
  statementLineCode: string;

  /**
   * 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
   */
  statementLineName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
   */
  lineCategory: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
   */
  isTotalLine: number;

  /**
   * 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
   */
  periodAmount: number;

  /**
   * 上期金额（比较信息；CAS/IAS 1）
   */
  priorPeriodAmount: number;

  /**
   * 本年累计金额（中国利润表常见列；自财年期初至本期末）
   */
  yearToDateAmount: number;

  /**
   * 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
   */
  isExpense: number;

  /**
   * 币种（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  profitLossStatus: number;

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

