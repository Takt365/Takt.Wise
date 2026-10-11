// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/fqc-trend
// 文件名称：ja-JP.ts
// 功能描述：完成品検査推移ページ；参照キー logistics.quality.operation.fqc-trend.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    title: '完成品検査推移',
    periodRange: '期間年月',
    customerCode: '顧客',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    summary: '顧客行 {count} 件（月別不良率）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下降 {down} · 横ばい {flat}',
    exportSuccess: 'エクスポート成功',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません',
    filter: { all: 'すべて', changed: '変動のみ' },
    trend: { none: '—', up: '上昇', down: '下降', flat: '横ばい' },
    columns: { trend: '推移', varianceAmount: '前月差', variancePercent: '前月比%', defectRate: '不良率' },
  },
};
