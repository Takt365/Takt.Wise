// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：salesStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：销售统计 DTO（数据看板 order-stat / invoice-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 销售订单统计查询 DTO
 * @description 对应后端 TaktSalesStatQueryDto
 */
export interface SalesOrderStatQuery {
  /** 订单日期（范围-开始） */
  orderDateStart?: string;
  /** 订单日期（范围-结束） */
  orderDateEnd?: string;
  /** 同比基期订单日期（范围-开始；可选） */
  compareOrderDateStart?: string;
  /** 同比基期订单日期（范围-结束；可选） */
  compareOrderDateEnd?: string;
}

/**
 * 销售订单统计 DTO
 * @description 对应后端 TaktSalesOrderStatDto
 */
export interface SalesOrderStat {
  /** 统计月份（yyyy-MM） */
  statMonth: string;
  /** 月订单数量 */
  monthOrderCount: number;
  /** 月订单金额合计（元） */
  monthTotalAmount: number;
  /** 同比基期订单数量 */
  compareOrderCount: number;
  /** 订单数同比增长率（%） */
  orderCountYoYPercent: number;
}

/**
 * 销售发票统计查询 DTO
 * @description 对应后端 TaktSalesInvoiceStatQueryDto
 */
export interface SalesInvoiceStatQuery {
  /** 年度期间 yyyyMM */
  yearMonth?: string;
  /** 明细过帐日期（范围-开始） */
  postingDateStart?: string;
  /** 明细过帐日期（范围-结束） */
  postingDateEnd?: string;
  /** 同比基期年度期间 yyyyMM（可选） */
  compareYearMonth?: string;
}

/**
 * 销售发票统计 DTO
 * @description 对应后端 TaktSalesInvoiceStatDto
 */
export interface SalesInvoiceStat {
  /** 统计月份（yyyy-MM） */
  statMonth: string;
  /** 年度期间（yyyyMM） */
  yearMonth: string;
  /** 月发票数量 */
  monthInvoiceCount: number;
  /** 月销售额合计（本位币元） */
  monthSalesAmount: number;
  /** 同比基期发票数量 */
  compareInvoiceCount: number;
  /** 同比基期销售额合计（本位币元） */
  compareSalesAmount: number;
  /** 发票数同比增长率（%） */
  invoiceCountYoYPercent: number;
  /** 销售额同比增长率（%） */
  salesAmountYoYPercent: number;
}
