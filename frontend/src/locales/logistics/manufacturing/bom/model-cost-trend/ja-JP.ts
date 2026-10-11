// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：機種コスト推移ページ文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '機種コスト推移',
    periodRange: '原価年月',
    modelOptional: '機種（任意、空＝すべて）',
    modelAll: '全機種',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '原価期間を選択してください',
    selectMaterialTypeRequired: '品目タイプを選択してください',
    summary:
      '{plant} / {materialType} / {period} / 機種 {model} — 製品 {productCount}（部品差異は {compare}）',
    export: '機種コスト推移を出力',
    exportSuccess: '機種コスト推移の出力に成功',
    exportFailed: '機種コスト推移の出力に失敗',
    queryFailed: '機種コスト推移の照会に失敗しました',
    columns: {
      model: '機種',
      product: '製品',
      productDescription: '製品説明',
      variance: '差異',
      commonPrice: '共通価格',
      variancePrice: '差異価格',
      componentVariance: '部品差異',
    },
  },
};
