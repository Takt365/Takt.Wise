// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：qualityCostReport.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告查询与行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 品质成本报告查询
 */
export interface QualityCostReportQuery {
  /** 页签 */
  statTab?: string
  /** 工厂 */
  plantCode?: string
  /** 期间起 */
  periodDateStart?: string
  /** 期间止 */
  periodDateEnd?: string
  /** 记录组 / 机种 / 废弃组 */
  dimensionCode?: string
  /** 币种 */
  currencyCode?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 品质成本报告行
 */
export interface QualityCostReportRow {
  /** 工厂 */
  plantCode: string
  /** 维度编码 */
  dimensionCode: string
  /** 维度名称 */
  dimensionName: string
  /** 币种 */
  currencyCode: string
  /** 单据数 */
  orderCount: number
  /** 金额 */
  amount: number
  /** 时间或废弃数量 */
  quantity: number
}

/**
 * 品质成本报告结果
 */
export interface QualityCostReportResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: QualityCostReportRow[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
