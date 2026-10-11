// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：priceTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 销售价格推移查询
 * @description 对应后端 TaktSalesPriceTrendQueryDto
 */
export interface SalesPriceTrendQuery extends TaktPagedQuery {
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
  /** 客户编码 */
  customerCode?: string;
  /** 价格类型（字典 logistics_procurement_price_type，如 PR00） */
  priceType?: string;
  /** 仅启用主表 */
  onlyEnabled?: boolean;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 销售价格推移转置行
 * @description 对应后端 TaktSalesPriceTrendDto
 */
export interface SalesPriceTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料描述 */
  materialDescription?: string;
  /** 客户编码（空串表示通用价） */
  customerCode: string;
  /** 客户名称 */
  customerName?: string;
  /** 币种 */
  currencyCode?: string;
  /** 销售单位 */
  unit?: string;
  /** 各期间单价（键 yyyy-MM-dd，对应 ValidFrom） */
  periodUnitPrices?: Record<string, number>;
  /** 各期间单价来源（与列键相同的有效起始日） */
  periodPriceSourcePeriods?: Record<string, string>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比差额 */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 销售价格推移分析结果
 * @description 对应后端 TaktSalesPriceTrendResultDto
 */
export interface SalesPriceTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<SalesPriceTrend>;
  /** 期间列顺序（yyyy-MM-dd，ValidFrom） */
  periodOrder: string[];
  /** 行总数 */
  materialCount: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 涨价行数 */
  upCount: number;
  /** 跌价行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
}

