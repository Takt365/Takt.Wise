// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/so-price-scr-deviation
// 文件名称：zh-HK.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO價偏離SCR頁靜態文案；引用鍵 logistics.sales.so-price-scr-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'SO價偏離SCR',
    periodRange: '訂單期間',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇訂單期間',
    summary: '命中 {count} 行（無SCR {noScr} · 單價不在價目 {absent}；同生效週期含條件行/數量等級/價值等級）',
    export: '清單匯出',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無資料可匯出，請先查詢',
    columns: {
      plantCode: '工廠',
      salesOrderCode: '銷售訂單',
      lineNumber: '行號',
      orderDate: '訂單日期',
      pricingDate: '價格日期',
      customerCode: '客戶',
      materialCode: '物料',
      currencyCode: '幣種',
      orderQuantity: '數量',
      soNormalizedUnitPrice: 'SO折算單價',
      matchReason: '原因',
      scrValidFrom: 'SCR有效起',
      scrValidTo: 'SCR有效止',
      scrPricesText: '週期內銷售條件',
      closestScrNormalizedUnitPrice: '最近SCR折算單價',
      deviationAmount: '偏離金額',
      deviationTrend: '方向',
    },
    reason: {
      noScr: '同週期無SCR',
      priceAbsent: '單價不在價目中',
    },
    trend: {
      up: '偏高',
      down: '偏低',
    },
  },
};
