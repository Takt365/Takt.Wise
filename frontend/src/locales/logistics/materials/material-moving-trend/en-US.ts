// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/material-moving-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：Material monthly moving price trend page copy; keys logistics.materials.material-moving-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Moving Price Trend',
    periodRange: 'Period',
    materialCode: 'Material code',
    selectPlantRequired: 'Please select a plant',
    selectMaterialTypeRequired: 'Please select a material type',
    selectValuationRequired: 'Please select a valuation class',
    selectPeriodRequired: 'Please select a period range',
    summary: '{count} material rows (gaps use last positive price; hover * for source month)',
    summaryModel: '{count} material rows (model/product groups from BOM: component→product→model)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    carriedFrom: 'Carried from {period} (no price this month)',
    export: 'Export list',
    exportSuccess: 'List exported',
    exportFailed: 'Failed to export list',
    exportEmpty: 'No rows to export. Run a query first.',
    tabs: {
      price: 'Material price trend',
      model: 'Material–model price trend',
    },
    filter: {
      all: 'All',
      changed: 'Changed only',
      leading: 'Top 50 up & down',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'Variance',
      variancePercent: 'Variance %',
      modelGroup: 'Models',
      productGroup: 'Products',
      materialText: 'Material text',
    },
  },
};
