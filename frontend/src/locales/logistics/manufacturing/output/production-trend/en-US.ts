// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-trend
// 文件名称：en-US.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Production trend page copy; keys logistics.manufacturing.output.production-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Production trend',
    periodRange: 'Period',
    teamCode: 'Team',
    modelCode: 'Model',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    summary: '{count} rows',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      assy: 'Assembly',
      pcba: 'PCBA',
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
      varianceAmount: 'MoM Δ',
      variancePercent: 'MoM %',
    },
  },
};
