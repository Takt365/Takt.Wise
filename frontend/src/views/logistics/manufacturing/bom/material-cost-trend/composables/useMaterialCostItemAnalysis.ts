// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/composables
// 文件名称：useMaterialCostItemAnalysis.ts
// 功能描述：BOM 物料成本分析页静态文案前缀、金额/涨跌格式化、环比列客户端排序比较
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  BomMaterialCostItemComponentMovingPriceQuery,
  BomMaterialCostItemMonthlyTrendLine,
  BomMaterialCostItemTransposedQuery,
} from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types';
import { formatAnalysisVariancePercent } from '@/utils/analysisPercent';
import { useI18n } from 'vue-i18n';

/** 静态 locales 引用键前缀 logistics.manufacturing.bom.material-cost-trend.page */
export const MATERIAL_COST_ANALYSIS_LOCALE_PREFIX =
  'logistics.manufacturing.bom.material-cost-trend.page';

/** 分析视图：单物料涨跌 / 机种物料清单转置 */
export type BomMaterialCostItemAnalysisViewMode = 'trend' | 'transposed';

/**
 * 涨跌码排序秩（与用户列表列 sorter 同模式：表头客户端比较）
 * @param trend 涨跌码
 * @returns 秩
 */
export function bomMomTrendSortRank(trend: string | null | undefined): number {
  if (trend === 'new') return 0;
  if (trend === 'removed') return 1;
  if (trend === 'up') return 2;
  if (trend === 'down') return 3;
  if (trend === 'flat') return 4;
  return 5;
}

/**
 * 可空数值比较（null/undefined 靠后）
 * @param a 左值
 * @param b 右值
 * @returns 比较结果
 */
export function compareBomMomNullableNumber(
  a: number | null | undefined,
  b: number | null | undefined,
): number {
  if (a == null && b == null) return 0;
  if (a == null) return 1;
  if (b == null) return -1;
  return a - b;
}

/**
 * 年月区间转为转置查询的核算日期起止（yyyy-MM → yyyy-MM-dd）
 * @param range 年月区间
 * @returns costingDateStart / costingDateEnd
 */
export function periodRangeToCostingDateQuery(
  range: [string, string] | null | undefined,
): Pick<BomMaterialCostItemTransposedQuery, 'costingDateStart' | 'costingDateEnd'> {
  if (!range?.[0]) {
    return {};
  }
  const costingDateStart = `${range[0]}-01`;
  if (!range[1]) {
    return { costingDateStart };
  }
  const parts = range[1].split('-').map(Number);
  const year = parts[0];
  const month = parts[1];
  if (!year || !month) {
    return { costingDateStart };
  }
  const lastDay = new Date(year, month, 0).getDate();
  const costingDateEnd = `${range[1]}-${String(lastDay).padStart(2, '0')}`;
  return { costingDateStart, costingDateEnd };
}

/**
 * 年月区间转为移动价格期间起止（yyyy-MM → 当月首日 yyyy-MM-dd）
 * @param range 年月区间
 * @returns periodDateStart / periodDateEnd
 */
export function periodRangeToMovingPricePeriodQuery(
  range: [string, string] | null | undefined,
): Pick<BomMaterialCostItemComponentMovingPriceQuery, 'periodDateStart' | 'periodDateEnd'> {
  if (!range?.[0]) {
    return {};
  }
  const periodDateStart = `${range[0]}-01`;
  if (!range[1]) {
    return { periodDateStart };
  }
  return {
    periodDateStart,
    periodDateEnd: `${range[1]}-01`,
  };
}

/** 两期间组件差异下钻查询上下文 */
export interface BomMaterialCostItemVarianceQueryContext {
  plantCode: string;
  productCode: string;
  basePeriod: string;
  comparePeriod: string;
}

/**
 * BOM 物料成本分析页格式化与涨跌展示
 * @returns 格式化函数与 locale 前缀
 */
export function useMaterialCostAnalysis() {
  const { t } = useI18n();
  const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX;

  /**
   * 格式化金额
   * @param value 数值
   * @returns 展示文本
   */
  function formatCost(value?: number | null): string {
    if (value == null || Number.isNaN(value)) return '—';
    return value.toFixed(5);
  }

  /**
   * 格式化百分比（0.00% 不显示）
   * @param value 数值
   * @returns 展示文本
   */
  function formatPercent(value?: number | null): string {
    return formatAnalysisVariancePercent(value, 'points');
  }

  /**
   * 涨跌文案
   * @param trend 趋势码
   * @returns 展示文本
   */
  function trendLabel(trend: string): string {
    const key = `${localePrefix}.trend.${trend}`;
    const text = t(key);
    return text === key ? trend : text;
  }

  /**
   * 涨跌样式
   * @param trend 趋势码
   * @returns CSS 类
   */
  function trendClass(trend: string): string {
    if (trend === 'up' || trend === 'new') return 'text-red-600 font-medium';
    if (trend === 'down' || trend === 'removed') return 'text-green-600 font-medium';
    return '';
  }

  /**
   * 期间单元格变动码文案（有无物料 / 价格）
   * @param changeType present / absent / new / removed / up / down / flat
   * @returns 展示文本
   */
  function periodChangeTypeLabel(changeType: string): string {
    const key = `${localePrefix}.periodChange.${changeType}`;
    const text = t(key);
    return text === key ? changeType : text;
  }

  /**
   * 期间单元格变动码样式
   * @param changeType 变动码
   * @returns CSS 类
   */
  function periodChangeTypeClass(changeType: string): string {
    if (changeType === 'new') return 'text-blue-600 font-medium';
    if (changeType === 'removed') return 'text-orange-600 font-medium';
    if (changeType === 'up') return 'text-red-600';
    if (changeType === 'down') return 'text-green-600';
    if (changeType === 'absent') return 'text-text-secondary';
    return '';
  }

  /**
   * 差异金额样式
   * @param value 差异额
   * @returns CSS 类
   */
  function varianceClass(value?: number | null): string {
    if (value == null) return '';
    if (value > 0) return 'text-red-600';
    if (value < 0) return 'text-green-600';
    return '';
  }

  /**
   * 是否可下钻组件差异
   * @param record 月度行
   * @returns 是否可下钻
   */
  function canDrill(record: BomMaterialCostItemMonthlyTrendLine): boolean {
    return record.trend !== 'none' && !!record.basePeriod;
  }

  /**
   * 变动类型文案
   * @param changeType 变动类型码
   * @returns 展示文本
   */
  function changeTypeLabel(changeType: string): string {
    const key = `${localePrefix}.changeType.${changeType}`;
    const text = t(key);
    return text === key ? changeType : text;
  }

  return {
    localePrefix,
    formatCost,
    formatPercent,
    trendLabel,
    changeTypeLabel,
    periodChangeTypeLabel,
    periodChangeTypeClass,
    trendClass,
    varianceClass,
    canDrill,
  };
}
