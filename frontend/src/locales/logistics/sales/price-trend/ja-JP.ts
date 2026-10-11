// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/sales/price-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：販売価格推移ページ静的文案；引用キー logistics.sales.price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '販売価格推移',
    periodRange: '期間（年月）',
    materialCode: '品目コード',
    customerCode: '得意先コード',
    selectPlantRequired: 'プラントを選択してください',
    selectPeriodRequired: '期間（年月）を選択してください',
    selectPriceTypeRequired: '条件タイプを選択してください',
    selectCustomerRequired: '得意先を選択してください',
    summary: '品目×得意先行 {count} 件（列=有効開始日；同月の複数件は結合しない）',
    summaryModel: '品目×得意先行 {count} 件（機種/製品グループは BOM；欠月回填は左と同じ）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat}',
    trendSummaryValidFrom: '各行は直近2件の有効開始日で比較：上昇 {up} · 下落 {down} · 横ばい {flat}',
    carriedFrom: '直近価格日付 {period}（当月は有効価格なし・回填）',
    export: '一覧エクスポート',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません。先に検索してください',
    tabs: {
      price: '販売価格推移',
      model: '機種価格推移',
    },
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
    columns: {
      trend: '推移',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
      modelGroup: '機種グループ',
      productGroup: '製品グループ',
      materialText: '品目テキスト',
    },
  },
};
