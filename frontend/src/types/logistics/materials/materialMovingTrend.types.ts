// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialMovingTrend.types.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 物料移动价格推移查询
 * @description 对应后端 TaktMaterialMovingTrendQueryDto
 */
export interface MaterialMovingTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日 yyyy-MM-dd） */
  periodDateStart?: string;
  /** 期间止（当月首日 yyyy-MM-dd） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 评估类别 */
  valuation?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 物料移动价格推移转置行
 * @description 对应后端 TaktMaterialMovingTrendDto
 */
export interface MaterialMovingTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料名称 */
  materialName?: string;
  /** 评估类别 */
  valuation: string;
  /** 币种 */
  currencyCode?: string;
  /** 各期间单价 */
  periodUnitPrices?: Record<string, number>;
  /** 各期间单价来源月 */
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
 * 物料移动价格推移分析结果
 * @description 对应后端 TaktMaterialMovingTrendResultDto
 */
export interface MaterialMovingTrendResult {
  /** 分页行 */
  paged: TaktPagedResult<MaterialMovingTrend>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 物料行总数 */
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
