// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/so-price-scr-deviation
// 文件名称：en-US.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO vs SCR deviation page copy; keys logistics.sales.so-price-scr-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'SO vs SCR Price Deviation',
    periodRange: 'Order period',
    selectPlantRequired: 'Please select plant',
    selectPeriodRequired: 'Please select order period',
    summary: '{count} hits (no SCR {noScr} · price not in list {absent}; item + qty/value scales in same validity)',
    export: 'Export',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export; query first',
    columns: {
      plantCode: 'Plant',
      salesOrderCode: 'SO',
      lineNumber: 'Line',
      orderDate: 'Order date',
      pricingDate: 'Pricing date',
      customerCode: 'Customer',
      materialCode: 'Material',
      currencyCode: 'Currency',
      orderQuantity: 'Qty',
      soNormalizedUnitPrice: 'SO unit price',
      matchReason: 'Reason',
      scrValidFrom: 'SCR from',
      scrValidTo: 'SCR to',
      scrPricesText: 'SCR prices in period',
      closestScrNormalizedUnitPrice: 'Closest SCR unit',
      deviationAmount: 'Deviation',
      deviationTrend: 'Direction',
    },
    reason: {
      noScr: 'No SCR in period',
      priceAbsent: 'Price not in SCR list',
    },
    trend: {
      up: 'Higher',
      down: 'Lower',
    },
  },
};
