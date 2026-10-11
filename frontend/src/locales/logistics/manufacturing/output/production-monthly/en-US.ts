// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：Monthly production trend page copy; keys logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Monthly Production Trend',
    periodRange: 'Period',
    modelCode: 'Model',
    outputCategory: 'Output Category',
    selectPlantRequired: 'Please select plant code',
    selectPeriodRequired: 'Please select period',
    summary: '{count} model rows (monthly assy/PCBA output totals)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
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
    outputCategoryOptions: {
      assy: 'Assembly',
      pcba: 'PCBA',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'MoM Δ',
      variancePercent: 'MoM %',
    },
  },
};
