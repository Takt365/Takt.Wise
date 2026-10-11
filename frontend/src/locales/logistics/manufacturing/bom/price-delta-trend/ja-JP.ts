// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/price-delta-trend
// 文件名称：ja-JP.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：引用键 logistics.manufacturing.bom.price-delta-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'コスト差異推移',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '原価期間を選択してください',
    selectMaterialTypeRequired: '物料タイプを選択してください',
    selectBasePeriodRequired: '基準月を選択してください',
    selectComparePeriodRequired: '比較月を選択してください',
    selectDeltaMonthInRange: '基準月と比較月は原価期間内である必要があります',
    queryFailed: '製品コスト差異推移の照会に失敗しました',
    exportSuccess: '製品コスト差異推移をエクスポートしました',
    exportFailed: '製品コスト差異推移のエクスポートに失敗しました',
    periodRange: '原価期間',
    basePeriod: '基準月',
    comparePeriod: '比較月',
    priceDeltaOption: '差異',
    priceDeltaOptionAll: 'すべて',
    modelCodeOptional: '機種（任意）',
    productCodeOptional: '製品（任意）',
    compareHint: '比較月 {compare} → 基準月 {base}；差異=基準月−比較月；差異 {delta}',
    priceDelta: '差異',
    zeroPriceGroup: '0価格グループ',
    replaceComponentGroup: '推奨代替価格',
    priceDeltaTrend: '価格差異グループ',
    componentDeltaGroup: '部品差異',
    reworkOrderGroup: '改修',
  },
}
