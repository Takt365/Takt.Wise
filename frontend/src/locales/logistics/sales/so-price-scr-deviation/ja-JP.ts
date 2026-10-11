// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/so-price-scr-deviation
// 文件名称：ja-JP.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO単価とSCR乖離ページ文案；参照キー logistics.sales.so-price-scr-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'SO単価とSCR乖離',
    periodRange: '受注期間',
    selectPlantRequired: 'プラントを選択してください',
    selectPeriodRequired: '受注期間を選択してください',
    summary: '該当中 {count} 行（SCRなし {noScr} · 単価不在 {absent}；同一有効期間の条件行/数量・価値スケール）',
    export: '一覧出力',
    exportSuccess: '出力成功',
    exportFailed: '出力失敗',
    exportEmpty: '出力データがありません。先に照会してください',
    columns: {
      plantCode: 'プラント',
      salesOrderCode: '受注',
      lineNumber: '明細',
      orderDate: '受注日',
      pricingDate: '価格日付',
      customerCode: '得意先',
      materialCode: '品目',
      currencyCode: '通貨',
      orderQuantity: '数量',
      soNormalizedUnitPrice: 'SO換算単価',
      matchReason: '理由',
      scrValidFrom: 'SCR開始',
      scrValidTo: 'SCR終了',
      scrPricesText: '期間内販売条件',
      closestScrNormalizedUnitPrice: '最近SCR換算単価',
      deviationAmount: '乖離額',
      deviationTrend: '方向',
    },
    reason: {
      noScr: '期間内SCRなし',
      priceAbsent: '単価が価目にない',
    },
    trend: {
      up: '高い',
      down: '低い',
    },
  },
};
