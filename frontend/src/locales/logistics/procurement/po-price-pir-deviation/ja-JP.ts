// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/po-price-pir-deviation
// 文件名称：ja-JP.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO単価とPIR乖離ページ文案；参照キー logistics.procurement.po-price-pir-deviation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'PO単価とPIR乖離',
    periodRange: '発注期間',
    selectPlantRequired: 'プラントを選択してください',
    selectPeriodRequired: '発注期間を選択してください',
    summary: 'ヒット {count} 行（PIRなし {noPir} · 適用価乖離 {absent}）',
    export: '一覧出力',
    exportSuccess: '出力成功',
    exportFailed: '出力失敗',
    exportEmpty: '出力データがありません。先に照会してください',
    columns: {
      plantCode: 'プラント',
      purchaseOrderCode: '発注',
      lineNumber: '明細',
      orderDate: '発注日',
      supplierCode: '仕入先',
      materialCode: '品目',
      materialDescription: '品目記述',
      orderQuantity: '数量',
      poUnitPrice: 'PO単価',
      pirValidPeriod: 'PIR有効期間',
      pirPricesText: 'PIR価格',
      deviationAmount: '価格差',
      deviationPercent: '価格差比',
    },
    reason: {
      noPir: '期間内PIRなし',
      priceAbsent: '適用PIR価から乖離',
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
