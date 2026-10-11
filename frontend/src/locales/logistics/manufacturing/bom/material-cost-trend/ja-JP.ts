// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：製品コスト分析ページ；キー logistics.manufacturing.bom.material-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '製品コスト分析',
    periodRange: '原価年月',
    selectPlantRequired: '工場コードを選択してください',
    selectModelRequired: '機種コードを選択してください',
    selectProductRequired: '製品コードを選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectPeriodRequired: '原価期間を選択してください',
    summary: '{plant} / {model} / {product} — 単一製品の明細 {componentCount} 行（コスト分析）',
    trendSummary:
      '明細前月比 {base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat} · 追加 {added} · 削除 {removed}',
    queryFailed: '製品コスト分析の照会に失敗しました',
    exportSuccess: '製品コスト分析の出力に成功',
    exportFailed: '製品コスト分析の出力に失敗',
    filter: {
      all: 'すべて',
      changed: '変動のみ',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
      new: '追加',
      removed: '削除',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '追加',
      removed: '削除',
      up: '上昇',
      down: '下落',
      flat: '横ばい',
    },
    columns: {
      trend: '騰落',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
    },
  },
};
