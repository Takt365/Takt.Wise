// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-analysis
// 文件名称：en-US.ts
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM cost analysis page; keys logistics.manufacturing.bom.material-cost-analysis.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'BOM cost analysis',
    periodRange: 'Costing period',
    selectPlantRequired: 'Please select a plant',
    selectMaterialTypeRequired: 'Please select a material type',
    selectPeriodRequired: 'Please select a costing period',
    queryFailed: 'BOM cost analysis query failed',
    exportSuccess: 'BOM cost analysis exported',
    exportFailed: 'Failed to export BOM cost analysis',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    sort: {
      productCode: 'Product code (all rows)',
      trend: 'Trend first (all rows)',
      varianceDesc: 'Abs. variance ↓ (all rows)',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'MoM amount',
      variancePercent: 'MoM %',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
  },
};
