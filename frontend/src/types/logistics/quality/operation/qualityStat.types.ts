// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：qualityStat.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计查询与行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 质量统计查询
 */
export interface QualityStatQuery {
  /** 页签 */
  statTab?: string
  /** 工厂 */
  plantCode?: string
  /** 期间起 */
  periodDateStart?: string
  /** 期间止 */
  periodDateEnd?: string
  /** 供应商 / 工序 / 客户 */
  dimensionCode?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 质量统计行
 */
export interface QualityStatRow {
  /** 工厂 */
  plantCode: string
  /** 维度编码 */
  dimensionCode: string
  /** 维度名称 */
  dimensionName: string
  /** 检验单数 */
  orderCount: number
  /** 抽样数量 */
  sampleQty: number
  /** 合格数量 */
  qualifiedQty: number
  /** 不合格数量 */
  unqualifiedQty: number
  /** 验退数量 */
  returnQty: number
  /** 不良率 */
  defectRatePercent: number
  /** 合格率 */
  qualifiedRatePercent: number
}

/**
 * 质量统计结果
 */
export interface QualityStatResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: QualityStatRow[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
