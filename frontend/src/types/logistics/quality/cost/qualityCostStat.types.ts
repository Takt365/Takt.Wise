// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：qualityCostStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本金额统计（数据看板 cost-stat：业务/事故/应对）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 品质成本金额统计（业务/事故/应对共用）
 * @description 对应后端 TaktQualityCostStatDto
 */
export interface QualityCostStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 金额合计（元） */
  monthTotalAmount: number
  /** 单据行数 */
  monthRowCount: number
}
