// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCostTrend.types.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移类型（对应 TaktBomMaterialCostTrends）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/** BOM 成本推移：单个产品 × 月材料成本查询 */
export interface BomMaterialCostItemComponentMovingPriceQuery extends TaktPagedQuery {
  plantCode: string;
  /** 物料类型（字典 logistics_materials_material_type；空=默认 FERT） */
  materialType?: string;
  /** 可选；仅缩小产品范围 */
  modelCode?: string;
  /** 产品编码（必填；单个产品） */
  productCode: string;
  /** 核算期间起 yyyy-MM-dd（CostingDate 月初） */
  periodDateStart?: string;
  /** 核算期间止 yyyy-MM-dd（CostingDate 月初） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 涨跌筛选：up/down/flat/none/changed */
  trendFilter?: string;
}

/**
 * BOM 成本推移明细行：单个产品下 BOM 明细行月材料成本
 * @description 按明细表业务键跨月对齐；缺月无键；含涨跌环比
 */
export interface BomMaterialCostItemComponentMovingPrice {
  plantCode: string;
  modelCode: string;
  productCode: string;
  productDescription: string;
  /** 行号（项号/序号，固定步长=10） */
  lineNumber?: number;
  /** BOM 层级 */
  bomLevel?: string;
  /** BOM 项目号 */
  bomItemCode?: string;
  /** 组件编码（明细表） */
  componentCode: string;
  /** 组件描述（明细表） */
  componentDescription: string;
  /** 组件数量 */
  componentQuantity?: number;
  productionRelated?: string | null;
  /** PCB SECT 标识（空参与；X 不参与） */
  pcbSectIndicator?: string | null;
  purchaseType: string;
  currencyCode: string;
  /** 各核算月材料成本，键 yyyy-MM；缺月无键 */
  periodMaterialCosts: Record<string, number>;
  /**
   * 各月相对上一展示月：present / absent / new / removed / up / down / flat
   * （先区分有无物料，再对比价格）
   */
  periodChangeTypes?: Record<string, string>;
  trend?: string;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  varianceAmount?: number | null;
  variancePercent?: number | null;
}

/** BOM 成本推移（产品明细组件×月材料成本）分析结果 */
export interface BomMaterialCostItemComponentMovingPriceResult {
  paged: TaktPagedResult<BomMaterialCostItemComponentMovingPrice>;
  periodOrder: string[];
  productCodes: string[];
  /** 明细组件行总数 */
  componentCount: number;
  basePeriod?: string | null;
  comparePeriod?: string | null;
  upCount: number;
  downCount: number;
  flatCount: number;
  /** 关注月新增组件数 */
  newCount?: number;
  /** 关注月剔除组件数 */
  removedCount?: number;
  noneCount: number;
  /** 全量行各期间材料成本合计（分页前、已应用涨跌筛选） */
  periodCostTotals?: Record<string, number>;
  /** 全量行环比差额合计（分页前、已应用涨跌筛选） */
  varianceAmountTotal?: number | null;
}
