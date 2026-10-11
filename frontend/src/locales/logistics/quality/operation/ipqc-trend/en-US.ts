// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/ipqc-trend
// 文件名称：en-US.ts
// 功能描述：IPQC quality trend page locales; keys logistics.quality.operation.ipqc-trend.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    title: 'IPQC Quality Trend',
    periodRange: 'Period',
    processCode: 'Process',
    selectPlantRequired: 'Please select plant',
    selectPeriodRequired: 'Please select period',
    summary: '{count} process row(s) (defect rate by month)',
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export',
    filter: { all: 'All', changed: 'Changed only' },
    trend: { none: '—', up: 'Up', down: 'Down', flat: 'Flat' },
    columns: { trend: 'Trend', varianceAmount: 'Variance', variancePercent: 'MoM %', defectRate: 'Defect rate' },
  },
};
