// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：expenseDetail.types.ts
// 创建时间：2026-07-23
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
 * 费用单明细实体
 * 对应前端 TaktExpenseDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ExpenseDetail
 * @description 对应后端 TaktExpenseDetailDto
 */
export interface ExpenseDetail extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId?: string;

  /**
   * 费用单编码（冗余，便于查询）
   */
  expenseCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory?: string;

  /**
   * 明细项名称
   */
  itemName?: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity?: number;

  /**
   * 金额
   */
  itemAmount?: number;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceCode?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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
 * ExpenseDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ExpenseDetailExport
 * @description 对应后端 TaktExpenseDetailExportDto
 */
export interface ExpenseDetailExport {
  /**
   * ExpenseDetailID
   */
  expenseDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 费用单 ID（主子表关系）
   */
  expenseId: string;

  /**
   * 费用单编码（冗余，便于查询）
   */
  expenseCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 分配类别（字典 logistics_sales_allocation_category：A=资产，K=成本中心，F=订单；会签明细、采购申请明细、费用单明细共用）
   */
  allocationCategory: string;

  /**
   * 明细项名称
   */
  itemName: string;

  /**
   * 明细项说明
   */
  itemDescription?: string;

  /**
   * 数量
   */
  itemQuantity: number;

  /**
   * 金额
   */
  itemAmount: number;

  /**
   * 会计科目（选项 TaktAccountTitles/options；DictValue=Id）
   */
  accountTitle?: string;

  /**
   * 发票号码
   */
  invoiceCode?: string;

  /**
   * 费用发生日期
   */
  expenseDetailDate?: string;

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

