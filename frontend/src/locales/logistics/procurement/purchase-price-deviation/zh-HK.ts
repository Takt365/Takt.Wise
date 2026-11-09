// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-deviation
// 文件名称：zh-HK.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO/MIRO價偏離頁靜態文案；引用鍵 logistics.procurement.purchase-price-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '採購價偏離',
    tabs: {
      po: 'PO價偏離',
      miro: 'MIRO價偏離',
    },
    periodRange: '訂單期間',
    periodRangeMiro: '過賬期間',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間',
    summary: '命中 {count} 行（無PCR {noPir} · 偏離適用價 {absent}）',
    export: '清單匯出',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無資料可匯出，請先查詢',
    columns: {
      plantCode: '工廠',
      purchaseOrderCode: '採購訂單',
      materialDocumentCode: '物料憑證',
      lineNumber: '行號',
      orderDate: '訂單日期',
      postingDate: '過賬日期',
      supplierCode: '供應商',
      materialCode: '物料編碼',
      materialDescription: '物料描述',
      orderQuantity: '數量',
      quantity: '數量',
      poUnitPrice: 'PO單價',
      miroUnitPrice: 'MIRO單價',
      pirValidPeriod: 'PCR生效週期',
      pirPricesText: 'PCR價格',
      deviationAmount: '價差',
      deviationPercent: '價差比',
    },
    reason: {
      noPir: '同週期無PCR',
      priceAbsent: '偏離適用PCR價',
    },
    filter: {
      all: '全部',
      changed: '有漲跌',
    },
    trend: {
      up: '偏高',
      down: '偏低',
    },
  },
};
