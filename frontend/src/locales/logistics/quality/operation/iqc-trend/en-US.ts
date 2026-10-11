// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/iqc-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC inspection trend page locales; keys logistics.quality.operation.iqc-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'IQC Inspection Trend',
    periodRange: 'Period',
    supplierCode: 'Supplier',
    selectPlantRequired: 'Please select plant',
    selectPeriodRequired: 'Please select period',
    summary: '{count} supplier row(s) (defect rate by month)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export',
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
      varianceAmount: 'Variance',
      variancePercent: 'MoM %',
      defectRate: 'Defect rate',
    },
  },
};
