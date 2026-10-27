// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：productionMonthlyTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 月生产推移查询
 * @description 对应后端 TaktProductionMonthlyTrendQueryDto
 */
export interface ProductionMonthlyTrendQuery extends TaktPagedQuery {
  /** 页签：assy-team / assy-model / pcba-team / pcba-model */
  statTab: string;
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 班组 */
  teamCode?: string;
  /** 机种 */
  modelCode?: string;
  /** 物料编码 */
  materialCode?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 月生产推移转置行
 * @description 对应后端 TaktProductionMonthlyTrendDto
 */
export interface ProductionMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 班组 */
  teamCode?: string;
  /** 机种 */
  modelCode: string;
  /** 产出类别 */
  outputCategory: string;
  /** 产出类别显示名 */
  outputCategoryName?: string | null;
  /** 各期间产量合计 */
  periodValues?: Record<string, number>;
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
 * 月生产推移分析结果
 * @description 对应后端 TaktProductionMonthlyTrendResultDto
 */
export interface ProductionMonthlyTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<ProductionMonthlyTrend>;
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
