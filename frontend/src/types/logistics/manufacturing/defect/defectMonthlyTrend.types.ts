// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：defectMonthlyTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 月生产不良推移查询
 * @description 对应后端 TaktDefectMonthlyTrendQueryDto
 */
export interface DefectMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 机种编码 */
  modelCode?: string;
  /** 不良类别：assy / pcba */
  defectCategory?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 月生产不良推移转置行
 * @description 对应后端 TaktDefectMonthlyTrendDto
 */
export interface DefectMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 机种编码 */
  modelCode: string;
  /** 不良类别 */
  defectCategory: string;
  /** 不良类别显示名 */
  defectCategoryName?: string | null;
  /** 各期间不良率（小数 0~1） */
  periodValues?: Record<string, number>;
  /** 各期间生实/检查数量 */
  periodActualQuantities?: Record<string, number>;
  /** 各期间不良数量 */
  periodDefectQuantities?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比不良率差额（小数 0~1） */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 月生产不良推移分析结果
 * @description 对应后端 TaktDefectMonthlyTrendResultDto
 */
export interface DefectMonthlyTrendResult {
  /** 分页行 */
  paged?: TaktPagedResult<DefectMonthlyTrend>;
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
