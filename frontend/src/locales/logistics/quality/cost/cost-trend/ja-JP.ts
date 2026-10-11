// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本推移页静态文案；引用键 logistics.quality.cost.cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '品質コスト推移',
    periodRange: '期間年月',
    costCategory: 'コスト区分',
    currencyCode: '通貨',
    selectPlantRequired: '工場コードを選択してください',
    selectPeriodRequired: '期間年月を選択してください',
    summary: 'コスト区分行 {count} 件（品質保証/問題/事故を月次集計）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下降 {down} · 横ばい {flat}',
    export: '一覧出力',
    exportSuccess: '一覧出力に成功しました',
    exportFailed: '一覧出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    filter: {
      all: 'すべて',
      changed: '変動のみ',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下降',
      flat: '横ばい',
    },
    costCategoryOptions: {
      assurance: '品質保証',
      issue: '品質問題',
      incident: '品質事故',
    },
    columns: {
      trend: '推移',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
    },
  },
};
