// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/outbound-actual-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：販売実績推移ページ静的文案；引用キー logistics.materials.outbound-actual-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '出庫推移',
    periodRange: '転記年月',
    materialCode: '品目コード',
    customerCode: '得意先コード',
    selectPlantRequired: 'プラントを選択してください',
    selectPeriodRequired: '転記年月を選択してください',
    monthColumn: '月',
    amountColumn: '購買シェア（万CNY）',
    impactColumn: '増減金額（万CNY，単価差×数量）',
    summary: '{count} か月（購買シェアは万CNY；増減=(当月単価−前月単価)×当月数量、上昇と下落は相殺しない）',
    footerTotals: '合計 {amount}    上昇合計 {up}    下落合計 {down}',
    adjacentVs: '{period}比：単価差 × 当月数量',
    unitPrice: '単価',
    qty: '数量',
    month: '{month}月',
    monthYear: '{year}年{month}月',
    total: '合計',
    upTotal: '上昇合計',
    downTotal: '下落合計',
    detailSummary: '品目 {count} 行（月ごとに1行：金額/単価/数量/月/増減；増減=(当月単価−前月単価)×当月数量；期間列は列設定で表示切替）',
    chartSummary: '{count} か月（正軸=上昇、負軸=下落、単位万CNY；相殺なし；副題は選択期間）',
    tabs: {
      chart: '棒グラフ',
      trend: '推移表',
      detail: '明細',
    },
    chart: {
      title: '月次増減',
      subtext: '期間：{period}，基準月{baseMonth}',
      seriesUp: '上昇',
      seriesDown: '下落',
      seriesFlat: '同額',
      unitWanYuan: '万CNY',
      net: '純増減',
      markMax: '最大',
      markMin: '最小',
    },
    columns: {
      amount: '金額',
      unitPrice: '単価',
      quantity: '数量',
      month: '月',
      adjacent: '単価増減%',
      trend: '期間増減',
      varianceAmount: '期間単価影響金額',
    },
    export: '一覧エクスポート',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません。先に検索してください',
    filter: {
      all: 'すべて',
      changed: '変動のみ',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
    },
  },
};
