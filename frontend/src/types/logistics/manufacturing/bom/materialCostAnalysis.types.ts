// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCostAnalysis.types.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析类型（转置 / 差异 / 月度涨跌；合计/重算/机种月均；对应 TaktBomMaterialCostAnalyses）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/** BOM 成本分析转置查询（行=产品，列=月份总成本） */
export interface BomMaterialCostItemTransposedQuery extends TaktPagedQuery {
  plantCode?: string;
  /** 物料类型（字典 logistics_materials_material_type；空=默认 FERT） */
  materialType?: string;
  modelCode?: string;
  productCode?: string;
  costingDateStart?: string;
  costingDateEnd?: string;
  focusPeriod?: string;
  trendFilter?: string;
  /** 全量排序（分页前）：productCode / trend / varianceDesc */
  sortBy?: string;
}

/** BOM 成本分析转置行 */
export interface BomMaterialCostItemTransposed {
  plantCode: string;
  modelCode?: string;
  productCode: string;
  productDescription: string;
  currencyCode?: string;
  periodCosts: Record<string, number>;
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** BOM 成本分析转置分页结果 */
export interface BomMaterialCostItemTransposedResult {
  paged: TaktPagedResult<BomMaterialCostItemTransposed>;
  periodOrder: string[];
  modelSummary?: BomMaterialCostItemModelSummary | null;
  periodCostTotals?: Record<string, number>;
  varianceAmountTotal?: number | null;
}

/** 机种材料成本汇总 */
export interface BomMaterialCostItemModelSummary {
  modelCode: string;
  modelName: string;
  productCount: number;
  averagePeriodCosts: Record<string, number>;
}

/** BOM 成本分析差异查询 */
export interface BomMaterialCostItemVarianceQuery {
  plantCode: string;
  productCode: string;
  basePeriod: string;
  comparePeriod: string;
}

/** BOM 成本分析差异行 */
export interface BomMaterialCostItemVarianceLine {
  bomItemCode: string;
  componentCode: string;
  componentDescription: string;
  purchaseType: string;
  currencyCode: string;
  baseCost: number;
  compareCost: number;
  varianceAmount: number;
  variancePercent?: number | null;
  baseUnitPrice: number;
  compareUnitPrice: number;
  unitPriceVariance: number;
  baseQuantity: number;
  compareQuantity: number;
  quantityVariance: number;
  priceEffectAmount: number;
  quantityEffectAmount: number;
  changeType: string;
}

/** BOM 成本分析差异结果 */
export interface BomMaterialCostItemVarianceResult {
  plantCode: string;
  productCode: string;
  productDescription: string;
  basePeriod: string;
  comparePeriod: string;
  baseTotalCost: number;
  compareTotalCost: number;
  totalVariance: number;
  lines: BomMaterialCostItemVarianceLine[];
}

/** BOM 成本分析月度涨跌查询 */
export interface BomMaterialCostItemMonthlyTrendQuery {
  plantCode: string;
  modelCode: string;
  productCode: string;
  periodStart?: string;
  periodEnd?: string;
}

/** BOM 成本分析月度涨跌行 */
export interface BomMaterialCostItemMonthlyTrendLine {
  period: string;
  totalCost: number;
  basePeriod?: string | null;
  baseTotalCost?: number | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
  trend: string;
}

/** BOM 成本分析月度涨跌结果 */
export interface BomMaterialCostItemMonthlyTrendResult {
  plantCode: string;
  modelCode: string;
  productCode: string;
  productDescription: string;
  allMaterialsUnderModel: boolean;
  lines: BomMaterialCostItemMonthlyTrendLine[];
}
