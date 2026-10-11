// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：Product cost analysis page; keys logistics.manufacturing.bom.material-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Product cost analysis',
    periodRange: 'Costing months',
    selectPlantRequired: 'Please select a plant',
    selectModelRequired: 'Please select a model',
    selectProductRequired: 'Please select a product',
    selectMaterialTypeRequired: 'Please select a material type',
    selectPeriodRequired: 'Please select a costing period',
    summary: '{plant} / {model} / {product} — {componentCount} component rows (single-product analysis)',
    trendSummary:
      'Detail MoM {base} → {compare}: up {up} · down {down} · flat {flat} · new {added} · removed {removed}',
    queryFailed: 'Product cost analysis query failed',
    exportSuccess: 'Product cost analysis exported',
    exportFailed: 'Failed to export product cost analysis',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
      new: 'New',
      removed: 'Removed',
    },
    periodChange: {
      present: 'Present',
      absent: 'Absent',
      new: 'New',
      removed: 'Removed',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'MoM amount',
      variancePercent: 'MoM %',
    },
  },
};
