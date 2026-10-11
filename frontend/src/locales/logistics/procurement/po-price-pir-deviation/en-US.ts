// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/po-price-pir-deviation
// 文件名称：en-US.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO vs PIR deviation page copy; keys logistics.procurement.po-price-pir-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'PO vs PIR Price Deviation',
    periodRange: 'Order period',
    selectPlantRequired: 'Please select plant',
    selectPeriodRequired: 'Please select order period',
    summary: '{count} hits (no PIR {noPir} · off applicable {absent})',
    export: 'Export',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export; query first',
    columns: {
      plantCode: 'Plant',
      purchaseOrderCode: 'PO',
      lineNumber: 'Line',
      orderDate: 'Order date',
      supplierCode: 'Supplier',
      materialCode: 'Material',
      materialDescription: 'Description',
      orderQuantity: 'Qty',
      poUnitPrice: 'PO price',
      pirValidPeriod: 'PIR validity',
      pirPricesText: 'PIR price',
      deviationAmount: 'Variance',
      deviationPercent: 'Variance %',
    },
    reason: {
      noPir: 'No PIR in period',
      priceAbsent: 'Off applicable PIR price',
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
