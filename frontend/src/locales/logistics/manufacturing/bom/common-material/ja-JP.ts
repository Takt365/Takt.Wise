// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/common-material
// 文件名称：ja-JP.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：汎用部品ページ文案；引用键 logistics.manufacturing.bom.common-material.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '汎用部品',
    costingMonth: '原価計算年月',
    costingMonthPlaceholder: 'yyyy-MM',
    material: '物料',
    materialOptional: '物料（任意）',
    modelOptional: '機種（任意）',
    hint: '指定年月の明細（購買タイプ F、生産関連 X、PCB SECT 標識が空）を先に取り、原価ヘッダの製品コードで機種を対応します。2機種以上の物料を汎用とします。',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '原価計算年月を選択してください',
    summary: '{plant} / {period} / 機種 {model} / 物料 {material} — 汎用部品 {total} 行',
    exportSuccess: '汎用部品をエクスポートしました',
    exportFailed: '汎用部品のエクスポートに失敗しました',
    queryFailed: '汎用部品の照会に失敗しました',
    columns: {
      period: '期間',
      model: '機種',
      component: '物料',
      description: '物料説明',
      productCount: '製品数',
      usage: '使用一覧',
      lineCost: '行原価',
    },
  },
}
