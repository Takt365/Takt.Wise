// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-monthly
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月次生産不良推移ページ文言；キー logistics.manufacturing.defect.defect-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '不良推移',
    tabs: {
      assy: '組立',
      pcba: 'PCBA',
    },
    periodRange: '期間年月',
    defectCategory: '不良区分',
    modelCode: '機種',
    selectPlantRequired: '工場コードを選択してください',
    selectPeriodRequired: '期間年月を選択してください',
    summary: '機種×不良区分 {count} 行（月次不良率集計）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下降 {down} · 横ばい {flat}',
    exportSuccess: 'エクスポート完了',
    exportFailed: 'エクスポート失敗',
    exportEmpty: 'エクスポートするデータがありません。先に検索してください',
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
    defectCategoryOptions: {
      assy: '組立',
      pcba: 'PCBA',
    },
    columns: {
      trend: '推移',
      varianceAmount: '率差',
      variancePercent: '前月比%',
    },
  },
};
