// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsDashboardPlant.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：数据看板工厂维度月推移查询辅助（plantCode + 当月 focusPeriod）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant';
import { getTaktDefaultPageIndex, getTaktMaxPageSize } from '@/utils/taktPaged';

/**
 * 看板关注月 yyyy-MM（自然月；默认上月）
 * @param monthsAgo 距今月数（0=当月，1=上月）
 * @returns {string} 月份键
 */
export function getDashboardFocusMonth(monthsAgo = 1): string {
  return dayjs().subtract(monthsAgo, 'month').format('YYYY-MM');
}

/**
 * 检验/生产统计区间（默认上月；与后端 TaktQualityStatQueryDto 对齐）
 * @param monthsAgo 距今月数（0=当月，1=上月）
 * @returns {{ start: string; end: string }} YYYY-MM-DD HH:mm:ss
 */
export function getDashboardInspectionMonthRange(monthsAgo = 1): { start: string; end: string } {
  const anchor = dayjs().subtract(monthsAgo, 'month');
  return {
    start: anchor.startOf('month').format('YYYY-MM-DD 00:00:00'),
    end: anchor.endOf('month').format('YYYY-MM-DD 23:59:59'),
  };
}

/**
 * 月推移分析通用查询参数（当月 focus + 有界 pageSize）
 * @param focusMonth 关注月 yyyy-MM
 * @returns 查询片段
 */
export function buildDashboardMonthTrendQuery(focusMonth: string): Record<string, unknown> {
  return {
    periodDateStart: `${focusMonth}-01`,
    periodDateEnd: `${focusMonth}-01`,
    focusPeriod: focusMonth,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: getTaktMaxPageSize(),
  };
}

/**
 * 解析当前公司关联工厂（看板静默加载）
 * @returns {Promise<string>} 工厂代码；未配置时为空串
 */
export async function resolveDashboardPlantCode(): Promise<string> {
  return resolveCurrentCompanyRelatedPlantCode();
}

/** 近三个月自然月区间（含当月，从最早到最新） */
export interface DashboardCalendarMonthRange {
  /** yyyy-MM */
  key: string;
  /** 月份数字（1–12，用于文案） */
  month: number;
  /** YYYY-MM-DD HH:mm:ss */
  start: string;
  /** YYYY-MM-DD HH:mm:ss */
  end: string;
}

/**
 * 前三个自然月区间（不含当月）
 * @returns {DashboardCalendarMonthRange[]} 三个月区间
 */
export function getLastThreeCalendarMonthRanges(): DashboardCalendarMonthRange[] {
  const offsets = [3, 2, 1];
  return offsets.map((offset) => {
    const anchor = dayjs().subtract(offset, 'month');
    return {
      key: anchor.format('YYYY-MM'),
      month: anchor.month() + 1,
      start: anchor.startOf('month').format('YYYY-MM-DD 00:00:00'),
      end: anchor.endOf('month').format('YYYY-MM-DD 23:59:59'),
    };
  });
}
