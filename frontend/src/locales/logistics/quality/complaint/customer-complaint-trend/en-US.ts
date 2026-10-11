// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/complaint/customer-complaint-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉推移页静态文案；引用键 logistics.quality.complaint.customer-complaint-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Customer Complaint Trend',
    periodRange: 'Period',
    customerCode: 'Customer Code',
    selectPlantRequired: 'Please select plant code',
    selectPeriodRequired: 'Please select period',
    summary: '{count} customer row(s) (monthly complaint counts)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    export: 'Export',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Please query first.',
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
