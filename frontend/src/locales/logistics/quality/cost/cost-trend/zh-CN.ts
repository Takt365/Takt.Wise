// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本推移页静态文案；引用键 logistics.quality.cost.cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '质量成本推移',
    periodRange: '期间年月',
    costCategory: '成本类别',
    currencyCode: '成本币种',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '成本类别行 {count} 条（按月汇总品质保证/问题/事故成本）',
    trendSummary: '环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    filter: {
      all: '全部',
      changed: '仅涨跌',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    costCategoryOptions: {
      assurance: '品质保证',
      issue: '品质问题',
      incident: '品质事故',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
  },
};
