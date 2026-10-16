// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：Purchase price trend page static copy; keys logistics.procurement.purchase-price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Purchase Price Trend',
    periodRange: 'Period (month)',
    materialCode: 'Material code',
    supplierCode: 'Supplier code',
    selectPlantRequired: 'Please select a plant',
    selectMaterialTypeRequired: 'Please select a material type',
    selectPeriodRequired: 'Please select a period range',
    selectPriceTypeRequired: 'Please select a condition type',
    selectSupplierRequired: 'Please select a supplier',
    summary: '{count} pricing records (same count as export; selected plant only; adjacent compare within material)',
    summaryModel: '{count} material×supplier rows (model/product groups from BOM; same columns and trend rules as the left tab)',
    trendSummary: '{base} → {compare}: up {up} · down {down} · flat {flat}',
    trendSummaryValidFrom: 'Each row compares first vs last point: up {up} · down {down} · flat {flat}',
    adjacentVs: 'vs {period}',
    pointColumn: 'Point {n}',
    carriedFrom: 'Latest price date {period} (no valid price this month; carried forward)',
    export: 'Export list',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      price: 'Purchase price trend',
      model: 'Model price trend',
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
      unitPrice: 'Unit price',
      validFrom: 'Valid from',
      adjacent: 'Change',
      trend: 'Period trend',
      varianceAmount: 'Period delta',
      variancePercent: 'Period %',
      modelGroup: 'Model group',
      productGroup: 'Product group',
      materialText: 'Material text',
    },
  },
};
