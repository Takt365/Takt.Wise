// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：costTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本月推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 质量成本月推移查询
 * @description 对应后端 TaktQualityCostTrendQueryDto
 */
export interface QualityCostTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 成本类别：assurance / issue / incident */
  costCategory?: string;
  /** 成本币种 */
  currencyCode?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 质量成本月推移转置行
 * @description 对应后端 TaktQualityCostTrendDto
 */
export interface QualityCostTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 成本类别 */
  costCategory: string;
  /** 成本类别显示名 */
  costCategoryName?: string | null;
  /** 成本币种 */
  currencyCode: string;
  /** 各期间汇总金额 */
  periodAmounts?: Record<string, number>;
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
 * 质量成本月推移分析结果
 * @description 对应后端 TaktQualityCostTrendResultDto
 */
export interface QualityCostTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<QualityCostTrend>;
  /** 期间列顺序 */
  periodOrder?: string[];
  /** 行总数 */
  rowCount?: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 上涨行数 */
  upCount?: number;
  /** 下跌行数 */
  downCount?: number;
  /** 持平行数 */
  flatCount?: number;
  /** 无法比较行数 */
  noneCount?: number;
}
