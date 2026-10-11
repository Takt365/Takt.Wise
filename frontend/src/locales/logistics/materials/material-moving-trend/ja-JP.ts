// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/material-moving-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：移動平均価格推移ページ文案；引用键 logistics.materials.material-moving-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '移動平均価格推移',
    periodRange: '期間年月',
    materialCode: '品目コード',
    selectPlantRequired: '工場を選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectValuationRequired: '評価クラスを選択してください',
    selectPeriodRequired: '期間年月を選択してください',
    summary: '品目行 {count} 件（欠月/無価は直近有価月を継承；* にマウスオーバーで出典月）',
    summaryModel: '品目行 {count} 件（機種/製品グループは BOM：構成→製品→機種）',
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下落 {down} · 横ばい {flat}',
    carriedFrom: '{period} から回填（当月は価格なし）',
    export: '一覧エクスポート',
    exportSuccess: '一覧のエクスポートに成功しました',
    exportFailed: '一覧のエクスポートに失敗しました',
    exportEmpty: 'エクスポートするデータがありません。先に照会してください',
    tabs: {
      price: '品目価格推移',
      model: '品目-機種-価格推移',
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
      trend: '増減',
      varianceAmount: '差額',
      variancePercent: '増減率%',
      modelGroup: '機種グループ',
      productGroup: '製品グループ',
      materialText: '品目テキスト',
    },
  },
};
