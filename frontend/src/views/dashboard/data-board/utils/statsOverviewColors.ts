// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsOverviewColors.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：统计概览六项指标与 color-base.css 著名色彩（11种）映射
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktThemeColorPreset } from '@/utils/theme';

/** 概览指标键 */
export type StatsOverviewMetricKey =
  | 'todo'
  | 'message'
  | 'meeting'
  | 'notification'
  | 'helpdesk'
  | 'online';

/**
 * 六项概览指标 → 著名色预设（与 color-base.css --takt-* 一致）
 * @see frontend/src/styles/color-base.css
 */
export const STATS_OVERVIEW_METRIC_COLOR_PRESET: Record<StatsOverviewMetricKey, TaktThemeColorPreset> = {
  todo: 'klein-blue',
  message: 'tiffany-blue',
  meeting: 'mars-green',
  notification: 'chinese-red',
  helpdesk: 'sennelier-yellow',
  online: 'memorial-gray',
};

/**
 * 取概览指标强调色 CSS 变量
 * @param key 指标键
 * @returns var(--takt-*) 字符串
 */
export function resolveOverviewMetricAccentColor(key: string): string {
  if (key === 'online') {
    return 'var(--takt-cn-feicui)'
  }
  const preset = STATS_OVERVIEW_METRIC_COLOR_PRESET[key as StatsOverviewMetricKey];
  if (!preset) {
    return 'var(--ant-color-primary)';
  }
  return `var(--takt-${preset})`;
}
