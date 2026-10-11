// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/ipqc-trend
// 文件名称：ja-JP.ts
// 功能描述：工程品質推移ページ；参照キー logistics.quality.operation.ipqc-trend.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    title: '工程品質推移',
    periodRange: '期間年月',
    processCode: '工程',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    summary: '工程行 {count} 件（月別不良率）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下降 {down} · 横ばい {flat}',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません',
    filter: { all: 'すべて', changed: '変動のみ' },
    trend: { none: '—', up: '上昇', down: '下降', flat: '横ばい' },
    columns: { trend: '推移', varianceAmount: '前月差', variancePercent: '前月比%', defectRate: '不良率' },
  },
};
