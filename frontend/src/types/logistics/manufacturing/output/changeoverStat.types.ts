// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：changeoverStat.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计清单类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 切换统计查询
 */
export interface ChangeoverStatQuery {
  /** 工厂 */
  plantCode?: string
  /** 期间起 */
  periodDateStart?: string
  /** 期间止 */
  periodDateEnd?: string
  /** 生产类别 */
  prodCategory?: string
  /** 切换类别 */
  changeoverCategory?: string
  /** 班组 */
  teamCode?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 切换统计行
 */
export interface ChangeoverStatRow {
  /** 行键 */
  rowKey: string
  /** 工厂 */
  plantCode: string
  /** 生产类别 */
  prodCategory: string
  /** 切换类别 */
  changeoverCategory: string
  /** 生产日期 */
  prodDate: string
  /** 班组 */
  teamCode: string
  /** 当前工单 */
  currentProdOrderCode: string
  /** 当前机种 */
  currentModelCode: string
  /** 切换后工单 */
  changeoverProdOrderCode: string
  /** 切换后机种 */
  changeoverModelCode: string
  /** 切换次数 */
  changeoverCount: number
  /** 切换时间 */
  changeoverTime: number
  /** 仪设时间 */
  instrumentSetupTime: number
  /** 切换总时间 */
  totalChangeoverTime: number
  /** 读取 SOP 时间 */
  readSopTime: number
  /** 学习时间 */
  learningTime: number
  /** 人数 */
  personCount: number
  /** 学习总时间 */
  totalLearningTime: number
  /** SOP 总时间 */
  totalSopTime: number
}

/**
 * 切换统计结果
 */
export interface ChangeoverStatResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: ChangeoverStatRow[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
