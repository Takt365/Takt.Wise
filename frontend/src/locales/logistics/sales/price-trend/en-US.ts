// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/price-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：Sales price trend page static copy; keys logistics.sales.price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Sales Price Trend',
    periodRange: 'Period (month)',
    materialCode: 'Material code',
    customerCode: 'Customer code',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period range',
    selectPriceTypeRequired: 'Please select a condition type',
    selectCustomerRequired: 'Please select a customer',
    summary: '{count} material×customer rows (columns = ValidFrom; same-month records stay separate)',
    summaryModel: '{count} material×customer rows (model/product groups from BOM; gap fill same as left)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    trendSummaryValidFrom: 'Each row compares its last two ValidFrom dates: up {up} · down {down} · flat {flat}',
    carriedFrom: 'Latest price date {period} (no valid price this month; carried forward)',
    export: 'Export list',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      price: 'Sales price trend',
      model: 'Model price trend',
    },
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
      varianceAmount: 'MoM delta',
      variancePercent: 'MoM %',
      modelGroup: 'Model group',
      productGroup: 'Product group',
      materialText: 'Material text',
    },
  },
};
