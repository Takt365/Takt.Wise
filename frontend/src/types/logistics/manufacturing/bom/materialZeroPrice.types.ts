// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialZeroPrice.types.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单 DTO（独立菜单；对齐 TaktBomMaterialZeroPrices）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types'

/**
 * 组件零价格合并查询（工厂+核算月；机种可选多选，空=全部）
 */
export interface BomMaterialZeroPriceQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string
  /** 机种编码（兼容单值） */
  modelCode?: string
  /** 机种编码多选（逗号分隔；空=全部机种） */
  modelCodes?: string
  /** 核算日起 */
  costingDateStart?: string
  /** 核算日止 */
  costingDateEnd?: string
}

/**
 * 组件零价格合并行
 */
export interface BomMaterialZeroPrice {
  plantCode: string
  modelCode: string
  componentCode: string
  componentDescription: string
  productCodes: string
  productCount: number
  movingAveragePrice: number
  suggestedComponentCode?: string | null
  suggestedMovingPrice?: number | null
  costingPeriod: string
}

/**
 * 组件零价格合并结果
 */
export interface BomMaterialZeroPriceResult {
  paged: TaktPagedResult<BomMaterialZeroPrice>
  productCodes: string[]
  componentCount: number
  costingPeriod: string
}

/**
 * 回填移动平均价请求（当前查询条件 + 行组件）
 */
export interface BomMaterialZeroPriceMovingBackfillDto {
  plantCode: string
  componentCode?: string
  modelCode?: string
  modelCodes?: string
  costingDateStart?: string
  costingDateEnd?: string
}

/**
 * 手工替换更新移动平均价请求（原组件 ← 新组件价/单位/币种）
 */
export interface BomMaterialZeroPriceManualMovingDto {
  plantCode: string
  /** 原零价组件 */
  componentCode: string
  /** 替换新组件 */
  sourceComponentCode: string
  modelCode?: string
  modelCodes?: string
  costingDateStart?: string
  costingDateEnd?: string
  /** 新组件移动平均价（原值，须 > 0） */
  movingAveragePrice: number
  /** 价格单位（默认 1000） */
  movingPriceUnit?: number
  /** 币种（默认 CNY） */
  movingPriceCurrencyCode?: string
}

/**
 * 回填移动平均价结果
 */
export interface BomMaterialZeroPriceMovingBackfillResult {
  scannedRowCount: number
  updatedRowCount: number
  skippedNoPriceCount: number
  unchangedRowCount: number
  componentProcessedCount: number
  sourceComponentCode?: string | null
  valuationPeriod?: string | null
  priceInfo?: string | null
  productMonthlyCostUpdatedCount: number
  modelMonthlyAverageUpdatedCount: number
  processedMonth: string
}

/**
 * PCB SECT 整树 ExtField 打标请求（工厂+核算月；机种可选）
 */
export interface BomMaterialZeroPricePcbSectMarkDto {
  plantCode: string
  modelCode?: string
  modelCodes?: string
  costingDateStart?: string
  costingDateEnd?: string
}

/**
 * PCB SECT 整树 ExtField 打标结果
 */
export interface BomMaterialZeroPricePcbSectMarkResult {
  scannedRowCount: number
  pcbSectRowCount: number
  updatedRowCount: number
  unchangedRowCount: number
  skippedOverflowCount: number
  processedMonth: string
}

/**
 * 去前导 0（当前工厂；纯数字产品/组件编码）
 */
export interface BomMaterialZeroPriceStripLeadingZeroDto {
  plantCode: string
}

/**
 * 去前导 0 结果
 */
export interface BomMaterialZeroPriceStripLeadingZeroResult {
  headerProductUpdatedCount: number
  itemProductUpdatedCount: number
  itemComponentUpdatedCount: number
  skippedConflictCount: number
}
