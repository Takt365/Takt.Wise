// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecMonthlyTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月设变推移转置分析类型（设变号×部门×月份）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 月设变推移查询
 * @description 对应后端 TaktEcMonthlyTrendQueryDto
 */
export interface EcMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 设变单号 */
  ecCode?: string;
  /** 责任部门编码 */
  deptCode?: string;
  /** 实施范围（字典 logistics_manufacturing_ec_scope_category） */
  ecScope?: number;
  /** 变更状态（字典 logistics_manufacturing_ec_status） */
  changeStatus?: number;
  /** 设变状态（字典 logistics_manufacturing_ec_gijutsu_status） */
  ecStatus?: number;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 月设变推移转置行
 * @description 对应后端 TaktEcMonthlyTrendDto（工厂×设变号×部门）
 */
export interface EcMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 设变单号 */
  ecCode: string;
  /** 责任部门编码 */
  deptCode: string;
  /** 各期间完成件数 */
  periodValues?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比差额（件数） */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 月设变推移分析结果
 * @description 对应后端 TaktEcMonthlyTrendResultDto
 */
export interface EcMonthlyTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<EcMonthlyTrend>;
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

/**
 * 月实施推移查询
 * @description 对应后端 TaktEcImplementationMonthlyTrendQueryDto
 */
export interface EcImplementationMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 责任部门编码 */
  deptCode?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 月实施推移转置行
 * @description 对应后端 TaktEcImplementationMonthlyTrendDto
 */
export interface EcImplementationMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 责任部门编码 */
  deptCode: string;
  /** 各期间实施件数 */
  periodValues?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比差额（件数） */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 月实施推移分析结果
 * @description 对应后端 TaktEcImplementationMonthlyTrendResultDto
 */
export interface EcImplementationMonthlyTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<EcImplementationMonthlyTrend>;
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
