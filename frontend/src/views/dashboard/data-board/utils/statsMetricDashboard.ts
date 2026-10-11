// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsMetricDashboard.ts
// 功能描述：数据看板 KPI 卡 dashboard 布局：著名色循环 + 图标 + 项 enrichment
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Component } from 'vue'
import {
  RiAlertLine,
  RiArrowDownLine,
  RiArrowUpLine,
  RiBarChartBoxLine,
  RiCalendarEventLine,
  RiCalendarLine,
  RiCheckboxCircleLine,
  RiCloseCircleLine,
  RiCustomerService2Line,
  RiFileList3Line,
  RiFilePaper2Line,
  RiGroupLine,
  RiLineChartLine,
  RiLoader4Line,
  RiMailLine,
  RiMoneyCnyCircleLine,
  RiPercentLine,
  RiShoppingCart2Line,
  RiStackLine,
  RiSurveyLine,
  RiTaskLine,
  RiTimeLine,
  RiUserHeartLine,
  RiUserVoiceLine,
} from '@remixicon/vue'
import type { StatsMetricItem } from '../components/StatsMetricGrid.vue'
import { resolveOverviewMetricAccentColor } from './statsOverviewColors'
import { resolveOverviewMetricIcon } from './statsOverviewIcons'

/** dashboard KPI 卡著名色循环（与 color-base.css 对齐） */
export const STATS_DASHBOARD_ACCENT_PALETTE: readonly string[] = [
  'var(--takt-tiffany-blue)',
  'var(--takt-klein-blue)',
  'var(--takt-chinese-red)',
  'var(--takt-cn-feicui)',
  'var(--takt-sennelier-yellow)',
  'var(--takt-mars-green)',
  'var(--takt-bordeaux-red)',
  'var(--takt-prussian-blue)',
  'var(--takt-titian-red)',
  'var(--takt-burgundy-red)',
  'var(--takt-memorial-gray)',
]

/** 指标键 → Remix Icon（各业务模块共用） */
const STATS_METRIC_ICON_BY_KEY: Record<string, Component> = {
  total: RiStackLine,
  notimplemented: RiCloseCircleLine,
  implemented: RiCheckboxCircleLine,
  inprogress: RiLoader4Line,
  notofficial: RiAlertLine,
  requests: RiFileList3Line,
  orders: RiShoppingCart2Line,
  tickets: RiCustomerService2Line,
  openTickets: RiMailLine,
  closedTickets: RiCheckboxCircleLine,
  contracts: RiFilePaper2Line,
  defectQty: RiAlertLine,
  defectRate: RiPercentLine,
  yieldRate: RiLineChartLine,
  trendUp: RiArrowUpLine,
  users: RiUserHeartLine,
  todayvisits: RiGroupLine,
  ticketCount: RiCustomerService2Line,
  pendingCount: RiCloseCircleLine,
  inProgressCount: RiLoader4Line,
  processedCount: RiCheckboxCircleLine,
  completedCount: RiCheckboxCircleLine,
  meetingNotStarted: RiCalendarEventLine,
  meetingCompleted: RiCheckboxCircleLine,
  announcementCount: RiMailLine,
  complaintCount: RiUserVoiceLine,
  customerCount: RiGroupLine,
  upCount: RiArrowUpLine,
  downCount: RiArrowDownLine,
  totalAmount: RiMoneyCnyCircleLine,
  rowCount: RiStackLine,
  iqcOrders: RiSurveyLine,
  iqcPassRate: RiPercentLine,
  ipqcOrders: RiSurveyLine,
  ipqcPassRate: RiPercentLine,
  downtime: RiTimeLine,
  input: RiTimeLine,
  prod: RiTimeLine,
  actual: RiTimeLine,
  todo: RiTaskLine,
  message: RiMailLine,
  meeting: RiCalendarEventLine,
}

/**
 * 按索引取 dashboard KPI 强调色
 * @param index 序号（0 起）
 * @returns CSS 颜色变量
 */
export function resolveDashboardMetricAccentColorByIndex(index: number): string {
  const palette = STATS_DASHBOARD_ACCENT_PALETTE
  if (palette.length === 0) {
    return 'var(--ant-color-primary)'
  }
  return palette[((index % palette.length) + palette.length) % palette.length] ?? palette[0]
}

/**
 * 解析指标图标（概览六项优先专用映射；yyyy-MM 月份键用日历；其余查表）
 * @param key 指标键
 * @returns 图标组件
 */
export function resolveStatsMetricIcon(key: string): Component {
  const overviewIcon = resolveOverviewMetricIcon(key)
  if (overviewIcon) {
    return overviewIcon
  }
  if (/^\d{4}-\d{2}$/.test(key)) {
    return RiCalendarLine
  }
  return STATS_METRIC_ICON_BY_KEY[key] ?? RiBarChartBoxLine
}

/** 概览六项指标键 */
const OVERVIEW_METRIC_KEYS = ['todo', 'message', 'meeting', 'notification', 'helpdesk', 'online'] as const

/**
 * 解析指标强调色（概览六项专用色；其余按序号循环著名色）
 * @param key 指标键
 * @param index 序号
 * @returns CSS 颜色变量
 */
export function resolveStatsMetricAccentColor(key: string, index: number): string {
  if ((OVERVIEW_METRIC_KEYS as readonly string[]).includes(key)) {
    return resolveOverviewMetricAccentColor(key)
  }
  return resolveDashboardMetricAccentColorByIndex(index)
}

/**
 * 补齐 dashboard KPI 卡所需的 accentColor / icon（保留已有值）
 * @param items 原始指标项
 * @returns 完整 StatsMetricItem 列表
 */
export function enrichDashboardMetricItems(
  items: Array<Omit<StatsMetricItem, 'accentColor' | 'icon'> & Partial<Pick<StatsMetricItem, 'accentColor' | 'icon'>>>,
): StatsMetricItem[] {
  return items.map((item, index) => ({
    ...item,
    accentColor: item.accentColor ?? resolveStatsMetricAccentColor(item.key, index),
    icon: item.icon ?? resolveStatsMetricIcon(item.key),
  }))
}
