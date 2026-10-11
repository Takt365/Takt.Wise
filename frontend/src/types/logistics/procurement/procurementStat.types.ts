// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：procurementStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：采购统计 DTO（数据看板 order-stat / invoice-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 采购统计查询 DTO（订单按订单日期；发票按过帐日期）
 * @description 对应后端 TaktProcurementStatQueryDto
 */
export interface ProcurementStatQuery {
  /** 订单日期（范围-开始） */
  orderDateStart?: string;
  /** 订单日期（范围-结束） */
  orderDateEnd?: string;
  /** 过帐日期（范围-开始；invoice-stat） */
  postingDateStart?: string;
  /** 过帐日期（范围-结束；invoice-stat） */
  postingDateEnd?: string;
  /** 申请日期（范围-开始；request-stat） */
  requestDateStart?: string;
  /** 申请日期（范围-结束；request-stat） */
  requestDateEnd?: string;
  /** 同比基期订单日期（范围-开始；可选） */
  compareOrderDateStart?: string;
  /** 同比基期订单日期（范围-结束；可选） */
  compareOrderDateEnd?: string;
}

/**
 * 采购订单统计 DTO
 * @description 对应后端 TaktPurchaseOrderStatDto
 */
export interface PurchaseOrderStat {
  /** 统计月份（yyyy-MM） */
  statMonth: string;
  /** 月订单数量 */
  monthOrderCount: number;
  /** 月采购金额合计（元） */
  monthTotalAmount: number;
  /** 同比基期订单数量 */
  compareOrderCount: number;
  /** 订单数同比增长率（%） */
  orderCountYoYPercent: number;
}

/**
 * 采购发票统计 DTO（数据看板）
 * @description 对应后端 TaktPurchaseInvoiceStatDto
 */
export interface PurchaseInvoiceStat {
  /** 统计月份（yyyy-MM） */
  statMonth: string;
  /** 月发票数量（按过帐日期） */
  monthInvoiceCount: number;
  /** 月采购金额合计（元） */
  monthTotalAmount: number;
}

/**
 * 采购申请统计 DTO（数据看板）
 * @description 对应后端 TaktPurchaseRequestStatDto
 */
export interface PurchaseRequestStat {
  /** 统计月份（yyyy-MM） */
  statMonth: string;
  /** 月申请数量（按申请日期） */
  monthRequestCount: number;
  /** 月申请金额合计（元） */
  monthTotalAmount: number;
}
