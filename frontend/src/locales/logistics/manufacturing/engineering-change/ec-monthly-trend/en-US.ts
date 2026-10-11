// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-monthly-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月设变推移页静态文案；引用键 logistics.manufacturing.engineering-change.ec-monthly-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Monthly EC Trend',
    periodRange: 'Period (month)',
    selectPlantRequired: 'Please select plant code',
    selectPeriodRequired: 'Please select period range',
    tabs: {
      issue: 'Monthly EC Trend',
      implement: 'Monthly Implementation Trend',
    },
    summary: '{count} EC×dept row(s) (completed tasks by month)',
    summaryImplement: '{count} department row(s) (completed task count by completion month)',
    deptCode: 'Department code',
    ecCode: 'EC No.',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    exportSuccess: 'Export completed',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'MoM Δ',
      variancePercent: 'MoM %',
    },
  },
};
