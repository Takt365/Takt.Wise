// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：modelCostTrend.types.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移类型（一行一个成品）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/** 机种成本推移查询 */
export interface BomMaterialCostItemModelCostTrendQuery extends TaktPagedQuery {
  plantCode: string;
  /** 物料类型（字典 logistics_materials_material_type） */
  materialType?: string;
  /** 机种（空=全部机种，各机种单独对比成品） */
  modelCode?: string;
  periodDateStart?: string;
  periodDateEnd?: string;
}

/**
 * 机种成本推移行（一个成品）
 * @description 各月成品成本、相邻月差额、组件差异（编码-用量:价格）
 */
export interface BomMaterialCostItemModelCostTrend {
  plantCode: string;
  modelCode: string;
  modelName: string;
  productCode: string;
  productDescription: string;
  currencyCode: string;
  /** 各月成品成本，键 yyyy-MM */
  periodCosts: Record<string, number>;
  /** 最后月 − 前一月 */
  varianceAmount?: number | null;
  /** 止月共通组件行成本合计 */
  commonPrice?: number | null;
  /** 止月差异组件行成本合计 */
  variancePrice?: number | null;
  /** 组件编码-用量:价格，分号分隔 */
  componentVarianceText: string;
}

/** 机种成本推移分析结果 */
export interface BomMaterialCostItemModelCostTrendResult {
  paged: TaktPagedResult<BomMaterialCostItemModelCostTrend>;
  periodOrder: string[];
  comparePeriod?: string | null;
  modelCount: number;
  productCount: number;
  periodCostTotals?: Record<string, number>;
  varianceAmountTotal?: number | null;
  commonPriceTotal?: number | null;
  variancePriceTotal?: number | null;
}
