// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-monthly-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月设变推移页静态文案；引用键 logistics.manufacturing.engineering-change.ec-monthly-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '月次設変推移',
    periodRange: '期間（月）',
    selectPlantRequired: '工場コードを選択してください',
    selectPeriodRequired: '期間（月）を選択してください',
    tabs: {
      issue: '月次設変推移',
      implement: '月次実施推移',
    },
    summary: '設変番号×部門行 {count} 件（完了日時で月次集計）',
    summaryImplement: '部門行 {count} 件（完了日時で月次実施件数集計）',
    deptCode: '部門コード',
    ecCode: '設変番号',
    trendSummary: '前月比 {base} → {compare}：増 {up} · 減 {down} · 横ばい {flat}',
    exportSuccess: 'エクスポート完了',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません。先に検索してください',
    filter: {
      all: 'すべて',
      changed: '増減のみ',
    },
    trend: {
      none: '—',
      up: '増',
      down: '減',
      flat: '横ばい',
    },
    columns: {
      trend: '増減',
      varianceAmount: '前月比差',
      variancePercent: '前月比%',
    },
  },
};
