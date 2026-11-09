// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-deviation
// 文件名称：ja-JP.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO/MIRO価格乖離ページ文案；参照キー logistics.procurement.purchase-price-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '調達価格乖離',
    tabs: {
      po: 'PO価格乖離',
      miro: 'MIRO価格乖離',
    },
    periodRange: '発注期間',
    periodRangeMiro: '転記期間',
    selectPlantRequired: 'プラントを選択してください',
    selectPeriodRequired: '期間を選択してください',
    summary: 'ヒット {count} 行（PCRなし {noPir} · 適用価乖離 {absent}）',
    export: '一覧出力',
    exportSuccess: '出力成功',
    exportFailed: '出力失敗',
    exportEmpty: '出力データがありません。先に照会してください',
    columns: {
      plantCode: 'プラント',
      purchaseOrderCode: '発注',
      materialDocumentCode: '品目伝票',
      lineNumber: '明細',
      orderDate: '発注日',
      postingDate: '転記日',
      supplierCode: '仕入先',
      materialCode: '品目',
      materialDescription: '品目記述',
      orderQuantity: '数量',
      quantity: '数量',
      poUnitPrice: 'PO単価',
      miroUnitPrice: 'MIRO単価',
      pirValidPeriod: 'PCR有効期間',
      pirPricesText: 'PCR価格',
      deviationAmount: '価格差',
      deviationPercent: '価格差比',
    },
    reason: {
      noPir: '期間内PCRなし',
      priceAbsent: '適用PCR価から乖離',
    },
    filter: {
      all: 'すべて',
      changed: '騰落あり',
    },
    trend: {
      up: '高い',
      down: '低い',
    },
  },
};
