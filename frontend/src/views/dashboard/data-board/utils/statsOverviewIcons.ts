// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsOverviewIcons.ts
// 功能描述：统计概览六项指标 Remix Icon 映射
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Component } from 'vue'
import {
  RiCalendarEventLine,
  RiCustomerService2Line,
  RiMailLine,
  RiNotification3Line,
  RiTaskLine,
  RiUserHeartLine,
} from '@remixicon/vue'
import type { StatsOverviewMetricKey } from './statsOverviewColors'

/**
 * 概览六项指标 → 图标组件
 */
export const STATS_OVERVIEW_METRIC_ICON: Record<StatsOverviewMetricKey, Component> = {
  todo: RiTaskLine,
  message: RiMailLine,
  meeting: RiCalendarEventLine,
  notification: RiNotification3Line,
  helpdesk: RiCustomerService2Line,
  online: RiUserHeartLine,
}

/**
 * 取概览指标图标组件
 * @param key 指标键
 * @returns 图标组件或 undefined
 */
export function resolveOverviewMetricIcon(key: string): Component | undefined {
  return STATS_OVERVIEW_METRIC_ICON[key as StatsOverviewMetricKey]
}
