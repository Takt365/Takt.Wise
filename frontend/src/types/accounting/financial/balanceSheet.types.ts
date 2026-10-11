// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：balanceSheet.types.ts
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
 * 资产负债表行实体（CAS 财务报表列报 / IAS 1 Statement of Financial Position） 列报原则：资产与负债按流动/非流动分类；所有者权益单独列示；期末列报金额参与「资产=负债+权益」勾稽。 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
 * 对应前端 TaktBalanceSheetDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BalanceSheet
 * @description 对应后端 TaktBalanceSheetDto
 */
export interface BalanceSheet extends CompanyDtoBase {

  /**
   * 会计期间编码（YYYYMM；资产负债表日所属报告期）
   */
  periodCode?: string;

  /**
   * 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
   */
  statementLineCode?: string;

  /**
   * 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
   */
  statementLineName?: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
   */
  lineCategory?: number;

  /**
   * 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
   */
  balanceDirection?: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
   */
  isTotalLine?: number;

  /**
   * 期初余额（总账口径）
   */
  openingBalance?: number;

  /**
   * 本期借方发生额
   */
  debitAmount?: number;

  /**
   * 本期贷方发生额
   */
  creditAmount?: number;

  /**
   * 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
   */
  closingBalance?: number;

  /**
   * 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
   */
  presentationAmount?: number;

  /**
   * 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
   */
  priorPeriodAmount?: number;

  /**
   * 币种（字典 accounting_financial_currency_code；报告货币）
   */
  currencyCode?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  balanceSheetStatus?: number;

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
 * BalanceSheet 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BalanceSheetExport
 * @description 对应后端 TaktBalanceSheetExportDto
 */
export interface BalanceSheetExport {
  /**
   * BalanceSheetID
   */
  balanceSheetId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options，DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 会计期间编码（YYYYMM；资产负债表日所属报告期）
   */
  periodCode: string;

  /**
   * 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
   */
  statementLineCode: string;

  /**
   * 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
   */
  statementLineName: string;

  /**
   * 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
   */
  accountTitleCode?: string;

  /**
   * 会计科目名称（冗余）
   */
  accountTitleName?: string;

  /**
   * 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
   */
  lineCategory: number;

  /**
   * 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
   */
  balanceDirection: number;

  /**
   * 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
   */
  isTotalLine: number;

  /**
   * 期初余额（总账口径）
   */
  openingBalance: number;

  /**
   * 本期借方发生额
   */
  debitAmount: number;

  /**
   * 本期贷方发生额
   */
  creditAmount: number;

  /**
   * 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
   */
  closingBalance: number;

  /**
   * 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
   */
  presentationAmount: number;

  /**
   * 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
   */
  priorPeriodAmount: number;

  /**
   * 币种（字典 accounting_financial_currency_code；报告货币）
   */
  currencyCode: string;

  /**
   * 排序号（越小越靠前；应与报表印刷顺序一致）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=停用）
   */
  balanceSheetStatus: number;

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

