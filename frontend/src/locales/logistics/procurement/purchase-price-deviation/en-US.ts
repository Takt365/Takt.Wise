// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-deviation
// 文件名称：en-US.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO/MIRO price deviation page copy; keys logistics.procurement.purchase-price-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Purchase Price Deviation',
    tabs: {
      po: 'PO deviation',
      miro: 'MIRO deviation',
    },
    periodRange: 'Order period',
    periodRangeMiro: 'Posting period',
    selectPlantRequired: 'Please select plant',
    selectPeriodRequired: 'Please select period',
    summary: '{count} hits (no PCR {noPir} · off applicable {absent})',
    export: 'Export',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export; query first',
    columns: {
      plantCode: 'Plant',
      purchaseOrderCode: 'PO',
      materialDocumentCode: 'Material doc',
      lineNumber: 'Line',
      orderDate: 'Order date',
      postingDate: 'Posting date',
      supplierCode: 'Supplier',
      materialCode: 'Material',
      materialDescription: 'Description',
      orderQuantity: 'Qty',
      quantity: 'Qty',
      poUnitPrice: 'PO price',
      miroUnitPrice: 'MIRO price',
      pirValidPeriod: 'PCR validity',
      pirPricesText: 'PCR price',
      deviationAmount: 'Variance',
      deviationPercent: 'Variance %',
    },
    reason: {
      noPir: 'No PCR in period',
      priceAbsent: 'Off applicable PCR price',
    },
    filter: {
      all: 'All',
      changed: 'Changed',
    },
    trend: {
      up: 'Higher',
      down: 'Lower',
    },
  },
};
