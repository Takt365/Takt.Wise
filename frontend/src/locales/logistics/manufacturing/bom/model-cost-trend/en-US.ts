// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：en-US.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：Model cost trend page copy; keys logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Model Cost Trend',
    periodRange: 'Costing months',
    modelOptional: 'Model (optional, empty = all)',
    modelAll: 'All models',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a costing period',
    selectMaterialTypeRequired: 'Please select a material type',
    summary:
      '{plant} / {materialType} / {period} / model {model} — {productCount} products (component diff at {compare})',
    export: 'Export model cost trend',
    exportSuccess: 'Model cost trend exported',
    exportFailed: 'Failed to export model cost trend',
    queryFailed: 'Model cost trend query failed',
    columns: {
      model: 'Model',
      product: 'Product',
      productDescription: 'Product description',
      variance: 'Variance',
      commonPrice: 'Common price',
      variancePrice: 'Variance price',
      componentVariance: 'Component variance',
    },
  },
};
