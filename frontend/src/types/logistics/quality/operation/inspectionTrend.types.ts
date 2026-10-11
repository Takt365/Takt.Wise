// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：inspectionTrend.types.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC/IPQC/FQC 检验月推移转置分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * 检验月推移查询基类
 * @description 对应后端 TaktQualityInspectionMonthlyTrendQueryDto
 */
export interface QualityInspectionMonthlyTrendQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 期间起（当月首日） */
  periodDateStart?: string;
  /** 期间止（当月首日） */
  periodDateEnd?: string;
  /** 关注期间 yyyy-MM */
  focusPeriod?: string;
  /** 涨跌筛选 */
  trendFilter?: string;
}

/**
 * 检验月推移转置行基类
 * @description 对应后端 TaktQualityInspectionMonthlyTrendDto
 */
export interface QualityInspectionMonthlyTrend {
  /** 工厂代码 */
  plantCode: string;
  /** 各期间不良率（0~1） */
  periodDefectRates?: Record<string, number | null>;
  /** 各期间检验单数 */
  periodOrderCounts?: Record<string, number>;
  /** 各期间抽样数量 */
  periodSampleQuantities?: Record<string, number>;
  /** 各期间不合格数量 */
  periodUnqualifiedQuantities?: Record<string, number>;
  /** 环比涨跌 */
  trend?: string;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 环比不良率差额 */
  varianceAmount?: number | null;
  /** 环比变动率（小数比率） */
  variancePercent?: number | null;
}

/**
 * 检验月推移分析结果
 * @description 对应后端 TaktQualityInspectionMonthlyTrendResultDto
 */
export interface QualityInspectionMonthlyTrendResult<TRow extends QualityInspectionMonthlyTrend> {
  /** 分页行 */
  paged: TaktPagedResult<TRow>;
  /** 期间列顺序 */
  periodOrder: string[];
  /** 行总数 */
  rowCount: number;
  /** 环比基准期间 */
  basePeriod?: string;
  /** 环比对比期间 */
  comparePeriod?: string;
  /** 不良率上升行数 */
  upCount: number;
  /** 不良率下降行数 */
  downCount: number;
  /** 持平行数 */
  flatCount: number;
  /** 无法比较行数 */
  noneCount: number;
}

/** IQC 检验月推移查询 */
export interface IqcOrderMonthlyTrendQuery extends QualityInspectionMonthlyTrendQuery {
  /** 供应商编码 */
  supplierCode?: string;
}

/** IQC 检验月推移行 */
export interface IqcOrderMonthlyTrend extends QualityInspectionMonthlyTrend {
  /** 供应商编码 */
  supplierCode: string;
  /** 供应商名称 */
  supplierName?: string;
}

/** IPQC 检验月推移查询 */
export interface IpqcOrderMonthlyTrendQuery extends QualityInspectionMonthlyTrendQuery {
  /** 工序编码 */
  processCode?: string;
}

/** IPQC 检验月推移行 */
export interface IpqcOrderMonthlyTrend extends QualityInspectionMonthlyTrend {
  /** 工序编码 */
  processCode: string;
  /** 工序名称 */
  processName?: string;
}

/** FQC 检验月推移查询 */
export interface FqcOrderMonthlyTrendQuery extends QualityInspectionMonthlyTrendQuery {
  /** 客户编码 */
  customerCode?: string;
}

/** FQC 检验月推移行 */
export interface FqcOrderMonthlyTrend extends QualityInspectionMonthlyTrend {
  /** 客户编码 */
  customerCode: string;
  /** 客户名称 */
  customerName?: string;
}
