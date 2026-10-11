// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：产品成本推移页静态文案；引用键 logistics.manufacturing.bom.material-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '产品成本推移',
    periodRange: '核算年月',
    selectPlantRequired: '请选择工厂代码',
    selectModelRequired: '请选择机种编码',
    selectProductRequired: '请选择产品编码',
    selectMaterialTypeRequired: '请选择物料类型',
    selectPeriodRequired: '请选择核算期间',
    summary: '{plant} / {model} / {product} — 单个产品明细组件 {componentCount} 行（成本分析报表）',
    trendSummary:
      '明细环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat} · 新增 {added} · 剔除 {removed}',
    queryFailed: '产品成本推移查询失败',
    exportSuccess: '产品成本推移导出成功',
    exportFailed: '产品成本推移导出失败',
    filter: {
      all: '全部',
      changed: '仅变动',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
      new: '新增',
      removed: '剔除',
    },
    periodChange: {
      present: '有',
      absent: '无',
      new: '新增',
      removed: '剔除',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
  },
};
