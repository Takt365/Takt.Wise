// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：productionStatLine.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产统计明细行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 生产统计明细查询
 */
export interface ProductionStatLineQuery {
  /** 页签 */
  statTab?: string
  /** 工厂 */
  plantCode?: string
  /** 期间起 */
  periodDateStart?: string
  /** 期间止 */
  periodDateEnd?: string
  /** 班组 */
  teamCode?: string
  /** 机种 */
  modelCode?: string
  /** 物料 */
  materialCode?: string
  /** 生产类别 */
  prodCategory?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 生产统计明细行
 */
export interface ProductionStatLine {
  /** 行键 */
  rowKey: string
  /** 工厂 */
  plantCode: string
  /** 班组 */
  teamCode: string
  /** 生产日期 */
  prodDate: string
  /** 批次 */
  batchCode: string
  /** 物料 */
  materialCode: string
  /** 机种 */
  modelCode: string
  /** ST */
  stdMinutes: number
  /** 计划台数 */
  planQty: number
  /** 直接 */
  directLabor: number
  /** 间接 */
  indirectLabor: number
  /** 工数 */
  laborMinutes: number
  /** 实绩台数 / 生产实绩 */
  actualQty: number
  /** 实绩 ST */
  actualStdMinutes: number
  /** ST 差异 */
  stdDiff: number
  /** 台数差异 */
  qtyDiff: number
  /** 达成率 */
  achievementRate: number
  /** LOT 数量 */
  lotQty: number
  /** PCB 类别 */
  pcbBoardType: string
  /** 板面 */
  panelSide: string
  /** 累计生产 */
  cumulativeQty: number
  /** 完成状态 */
  completedStatus: number
  /** 不良 */
  defectQty: number
}

/**
 * 生产统计明细结果
 */
export interface ProductionStatLineResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: ProductionStatLine[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
