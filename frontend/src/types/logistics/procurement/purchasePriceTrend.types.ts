// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchasePriceTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 采购价格推移查询
 * @description 对应后端 TaktPurchasePriceTrendQueryDto
 */
export interface PurchasePriceTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** 供应商编码 */
  supplierCode?: string;
  /** 价格类型（字典 logistics_procurement_price_type，如 PB00） */
  priceType?: string;
  /** 仅启用主表 */
  onlyEnabled?: boolean;
  /** 涨跌筛选 */
  trendFilter?: string;
  /** 明细可见/导出列键（逗号分隔；与表格列设置一致） */
  detailColumnKeys?: string;
}

/**
 * 采购价格推移单笔
 * @description 对应后端 TaktPurchasePriceTrendPointDto
 */
export interface PurchasePriceTrendPoint {
  /** 有效起始日 yyyy-MM-dd */
  validFrom: string;
  /** 折算单价 */
  unitPrice: number;
  /** 当月采购数量 */
  quantity?: number;
  /** 当月采购金额 */
  amount?: number;
  /** 当月是否有采购 */
  hasPurchase?: boolean;
  /** 相对前一笔涨跌 */
  adjacentTrend?: string;
  /** 相对前一笔差额（采购订单价格历史推移为单价差×当月数量） */
  adjacentVarianceAmount?: number | null;
  /** 相对前一笔变动率 */
  adjacentVariancePercent?: number | null;
  /** 前一笔有效起始日 */
  adjacentBasePeriod?: string | null;
}

/**
 * 相邻两次有效起始日比较
 * @description 对应后端 TaktPurchasePriceTrendAdjacentDto
 */
export interface PurchasePriceTrendAdjacent {
  /** 前一次有效起始日 */
  basePeriod: string;
  /** 本次有效起始日 */
  comparePeriod: string;
  /** 相邻涨跌 */
  trend: string;
  /** 相邻差额 */
  varianceAmount: number;
  /** 相邻变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 采购价格推移转置行
 * @description 对应后端 TaktPurchasePriceTrendDto
 */
export interface PurchasePriceTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料描述 */
  materialDescription?: string;
  /** 供应商编码 */
  supplierCode: string;
  /** 供应商名称 */
  supplierName?: string;
  /** 币种 */
  currencyCode?: string;
  /** 采购单位 */
  unit?: string;
  /** 各期间单价（键 yyyy-MM-dd） */
  periodUnitPrices?: Record<string, number>;
  /** 各期间单价来源 */
  periodPriceSourcePeriods?: Record<string, string>;
  /** 该行点序列（采购价格推移=ValidFrom 笔；采购订单价格历史推移=期间各月） */
  periodPoints?: PurchasePriceTrendPoint[];
  /** 相邻比较（键=后一次有效起始日） */
  periodAdjacents?: Record<string, PurchasePriceTrendAdjacent>;
  /** 期间涨跌（第 1 笔 vs 末笔） */
  trend?: string;
  /** 期间基准有效起始日 */
  basePeriod?: string;
  /** 期间对比有效起始日 */
  comparePeriod?: string;
  /** 期间差额 */
  varianceAmount?: number | null;
  /** 期间变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 采购价格推移分析结果
 * @description 对应后端 TaktPurchasePriceTrendResultDto
 */
export interface PurchasePriceTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<PurchasePriceTrend>;
  /** 列序：采购价格推移为第 n 笔；采购订单价格历史推移为期间年月 yyyy-MM */
  periodOrder: string[];
  /** 行总数 */
  materialCount: number;
  /** 第 1 笔序号 */
  basePeriod?: string;
  /** 末笔序号 */
  comparePeriod?: string;
  /** 涨价行数 */
  upCount: number;
  /** 跌价行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
  /** 各月金额 / 涨价合计 / 跌价合计 */
  monthSummaries?: PurchasePriceTrendMonthSummary[];
  /** 期间采购金额合计 */
  totalAmount?: number;
  /** 期间涨价合计 */
  totalUpAmount?: number;
  /** 期间跌价合计 */
  totalDownAmount?: number;
}

/**
 * 价格推移单月汇总
 * @description 对应后端 TaktPurchasePriceTrendMonthSummaryDto
 */
export interface PurchasePriceTrendMonthSummary {
  /** 年月 yyyy-MM */
  period: string;
  /** 当月金额合计 */
  amount: number;
  /** 当月涨价合计 */
  upAmount: number;
  /** 当月跌价合计 */
  downAmount: number;
}
