// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/complaint/customer-complaint-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉推移页静态文案；引用键 logistics.quality.complaint.customer-complaint-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '顧客クレーム推移',
    periodRange: '期間年月',
    customerCode: '得意先コード',
    selectPlantRequired: '工場コードを選択してください',
    selectPeriodRequired: '期間年月を選択してください',
    summary: '得意先行 {count} 件（月別クレーム件数）',
    trendSummary: '前月比 {base} → {compare}：増 {up} · 減 {down} · 平 {flat}',
    export: '一覧出力',
    exportSuccess: '出力に成功しました',
    exportFailed: '出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    filter: {
      all: 'すべて',
      changed: '増減のみ',
    },
    trend: {
      none: '—',
      up: '増',
      down: '減',
      flat: '平',
    },
    columns: {
      trend: '増減',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
    },
  },
};
