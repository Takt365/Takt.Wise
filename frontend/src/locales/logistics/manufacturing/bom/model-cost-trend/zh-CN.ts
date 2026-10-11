// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/model-cost-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：机种成本推移页静态文案；引用键 logistics.manufacturing.bom.model-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '机种成本推移',
    periodRange: '核算年月',
    modelOptional: '机种（可选，空=全部）',
    modelAll: '全部机种',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择核算期间',
    selectMaterialTypeRequired: '请选择物料类型',
    summary:
      '{plant} / {materialType} / {period} / 机种 {model} — 成品 {productCount} 个（组件差异取 {compare}）',
    export: '导出机种成本推移',
    exportSuccess: '机种成本推移导出成功',
    exportFailed: '机种成本推移导出失败',
    queryFailed: '机种成本推移查询失败',
    columns: {
      model: '机种',
      product: '产品',
      productDescription: '产品描述',
      variance: '差异',
      commonPrice: '共通价格',
      variancePrice: '差异价格',
      componentVariance: '组件差异',
    },
  },
};
