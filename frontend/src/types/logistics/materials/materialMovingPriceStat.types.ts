// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialMovingPriceStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格在库金额统计（数据看板 stock-stat；按评估类别分项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 在库金额按评估类别分项
 * @description 对应后端 TaktMaterialMovingPriceValuationAmountDto
 */
export interface MaterialMovingPriceValuationAmount {
  /** 评估类别（Z792/Z790/Z300/OTHER） */
  valuation: string
  /** 该类别库存金额（元） */
  stockAmount: number
}

/**
 * 在库金额统计（按评估期间汇总 + 评估类别分项）
 * @description 对应后端 TaktMaterialMovingPriceStatDto
 */
export interface MaterialMovingPriceStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 在库金额合计（元） */
  monthStockAmount: number
  /** 物料行数 */
  monthRowCount: number
  /** 按评估类别分项 */
  byValuation: MaterialMovingPriceValuationAmount[]
}
