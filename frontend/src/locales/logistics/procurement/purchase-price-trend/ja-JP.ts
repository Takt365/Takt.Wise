// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：購買価格推移ページ静的文案；引用キー logistics.procurement.purchase-price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '購買価格推移',
    periodRange: '期間（年月）',
    materialCode: '品目コード',
    supplierCode: '仕入先コード',
    selectPlantRequired: 'プラントを選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectPeriodRequired: '期間（年月）を選択してください',
    selectPriceTypeRequired: '条件タイプを選択してください',
    selectSupplierRequired: '仕入先を選択してください',
    summary: '価格記録 {count} 行（導出件数と同じ；選択工場のみ；同一品目で隣接比較）',
    summaryModel: '品目×仕入先行 {count} 件（機種/製品グループは BOM；列と漲跌ルールは左と同じ）',
    trendSummary: '{base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat}',
    trendSummaryValidFrom: '各行は第1件と最終件で比較：上昇 {up} · 下落 {down} · 横ばい {flat}',
    adjacentVs: '{period} 比',
    pointColumn: '第{n}件',
    carriedFrom: '直近価格日付 {period}（当月は有効価格なし・回填）',
    export: '一覧エクスポート',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません。先に検索してください',
    tabs: {
      price: '購買価格推移',
      model: '機種価格推移',
    },
    filter: {
      all: 'すべて',
      changed: '変動のみ',
      leading: '上昇・下落各上位50',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
    },
    columns: {
      unitPrice: '単価',
      validFrom: '有効開始日',
      adjacent: '変動',
      trend: '期間推移',
      varianceAmount: '期間差額',
      variancePercent: '期間%',
      modelGroup: '機種グループ',
      productGroup: '製品グループ',
      materialText: '品目テキスト',
    },
  },
};
