// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本推移页静态文案；引用键 logistics.quality.cost.cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Quality Cost Trend',
    periodRange: 'Period',
    costCategory: 'Cost Category',
    currencyCode: 'Currency',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period range',
    summary: '{count} cost category row(s) (monthly totals from assurance, issue, and incident)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    export: 'Export',
    exportSuccess: 'Export succeeded',
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
    costCategoryOptions: {
      assurance: 'Quality Assurance',
      issue: 'Quality Issue',
      incident: 'Quality Incident',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'Variance',
      variancePercent: 'Variance %',
    },
  },
};
