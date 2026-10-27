// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/common-material
// 文件名称：en-US.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：Common material page copy; keys logistics.manufacturing.bom.common-material.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Common Materials',
    costingMonth: 'Costing month',
    costingMonthPlaceholder: 'yyyy-MM',
    material: 'Material',
    materialOptional: 'Material (optional)',
    modelOptional: 'Model (optional)',
    hint: 'Loads materials with purchase type F, production-related X, and an empty PCB SECT mark, then reads the model from the cost header by product code. A material used on two or more models is common.',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a costing month',
    summary: '{plant} / {period} / model {model} / material {material} — {total} common materials',
    exportSuccess: 'Common materials exported',
    exportFailed: 'Failed to export common materials',
    queryFailed: 'Common material query failed',
    columns: {
      period: 'Period',
      model: 'Model',
      component: 'Material',
      description: 'Material description',
      productCount: 'Products',
      usage: 'Usage',
      lineCost: 'Line cost',
    },
  },
}
