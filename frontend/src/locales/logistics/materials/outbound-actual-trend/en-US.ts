// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/outbound-actual-trend
// 文件名称：en-US.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：Sales actual trend page static copy; keys logistics.materials.outbound-actual-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Material Outbound Trend',
    periodRange: 'Posting month',
    materialCode: 'Material code',
    customerCode: 'Customer code',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select posting month range',
    monthColumn: 'Month',
    amountColumn: 'Purchase share (万CNY)',
    impactColumn: 'Price impact (万CNY; Δunit × qty)',
    summary: '{count} months (share in 万CNY; impact = (unit now − unit prev) × qty now; up/down not netted)',
    footerTotals: 'Total {amount}    Up {up}    Down {down}',
    adjacentVs: 'vs {period}: unit delta × current qty',
    unitPrice: 'Unit price',
    qty: 'Qty',
    month: '{month}',
    monthYear: '{year}/{month}',
    total: 'Total',
    upTotal: 'Price-up total',
    downTotal: 'Price-down total',
    detailSummary: '{count} material rows (one row per month: amount/unit/qty/month/change; adjacent = (unit now − unit prev) × qty now; period columns toggleable)',
    chartSummary: '{count} months (positive=up, negative=down in 万CNY; no offset; subtitle = selecmed period)',
    tabs: {
      chart: 'Bar chart',
      trend: 'Trend',
      detail: 'Detail',
    },
    chart: {
      title: 'Monthly up/down',
      subtext: 'Period: {period}, base month {baseMonth}',
      seriesUp: 'Up',
      seriesDown: 'Down',
      seriesFlat: 'Flat',
      unitWanYuan: '万CNY',
      net: 'Net',
      markMax: 'Max',
      markMin: 'Min',
    },
    columns: {
      amount: 'Amount',
      unitPrice: 'Unit price',
      quantity: 'Qty',
      month: 'Month',
      adjacent: 'Unit price Δ%',
      trend: 'Period trend',
      varianceAmount: 'Period price impact',
    },
    export: 'Export list',
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
  },
};
