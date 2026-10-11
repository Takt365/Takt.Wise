// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-analysis
// 文件名称：ja-JP.ts
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：BOMコスト分析ページ静的文言；参照キー logistics.manufacturing.bom.material-cost-analysis.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'BOMコスト分析',
    periodRange: '原価期間',
    selectPlantRequired: '工場コードを選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    selectPeriodRequired: '原価期間を選択してください',
    queryFailed: 'BOMコスト分析の照会に失敗しました',
    exportSuccess: 'BOMコスト分析のエクスポートに成功しました',
    exportFailed: 'BOMコスト分析のエクスポートに失敗しました',
    filter: {
      all: 'すべて',
      changed: '騰落のみ',
    },
    sort: {
      productCode: '製品コード（全件）',
      trend: '騰落優先（全件）',
      varianceDesc: '差額絶対値降順（全件）',
    },
    columns: {
      trend: '騰落',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
    },
    trend: {
      none: '—',
      up: '騰',
      down: '落',
      flat: '横',
    },
  },
};
