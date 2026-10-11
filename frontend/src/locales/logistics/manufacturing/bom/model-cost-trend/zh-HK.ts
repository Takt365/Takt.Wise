// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：機種成本推移頁靜態文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '機種成本推移',
    periodRange: '核算年月',
    modelOptional: '機種（可選，空=全部）',
    modelAll: '全部機種',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇核算期間',
    selectMaterialTypeRequired: '請選擇物料類型',
    summary:
      '{plant} / {materialType} / {period} / 機種 {model} — 成品 {productCount} 個（組件差異取 {compare}）',
    export: '導出機種成本推移',
    exportSuccess: '機種成本推移導出成功',
    exportFailed: '機種成本推移導出失敗',
    queryFailed: '機種成本推移查詢失敗',
    columns: {
      model: '機種',
      product: '產品',
      productDescription: '產品描述',
      variance: '差異',
      commonPrice: '共通價格',
      variancePrice: '差異價格',
      componentVariance: '組件差異',
    },
  },
};
