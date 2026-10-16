// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/procurement/purchase-price-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移页静态文案；引用键 logistics.procurement.purchase-price-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '采购价格推移',
    periodRange: '期间年月',
    materialCode: '物料编码',
    supplierCode: '供应商编码',
    selectPlantRequired: '请选择工厂代码',
    selectMaterialTypeRequired: '请选择物料类型',
    selectPeriodRequired: '请选择期间年月',
    selectPriceTypeRequired: '请选择条件类型',
    selectSupplierRequired: '请选择供应商',
    summary: '定价记录 {count} 行（与导出条数一致；仅所选工厂；同一物料相邻比较）',
    summaryModel: '物料×供应商行 {count} 条（机种/产品组来自 BOM；列与涨跌规则同左）',
    trendSummary: '{base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
    trendSummaryValidFrom: '各行第 1 笔对末笔：涨 {up} · 跌 {down} · 平 {flat}',
    adjacentVs: '相对 {period}',
    pointColumn: '第{n}笔',
    carriedFrom: '最近价格日期 {period}（该月无有效价，沿用回填）',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      price: '采购价格推移',
      model: '机种价格推移',
    },
    filter: {
      all: '全部',
      changed: '仅涨跌',
      leading: '领涨领跌前50',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    columns: {
      unitPrice: '单价',
      validFrom: '有效起始日',
      adjacent: '涨跌',
      trend: '期间涨跌',
      varianceAmount: '期间差额',
      variancePercent: '期间%',
      modelGroup: '机种组',
      productGroup: '产品组',
      materialText: '物料描述',
    },
  },
};
