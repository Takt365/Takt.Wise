// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/complaint/customer-complaint-trend
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉推移页静态文案；引用键 logistics.quality.complaint.customer-complaint-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '顾客投诉推移',
    periodRange: '期间年月',
    customerCode: '客户编码',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '客户行 {count} 条（按月统计投诉件数）',
    trendSummary: '环比 {base} → {compare}：增 {up} · 减 {down} · 平 {flat}',
    export: '清单导出',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    filter: {
      all: '全部',
      changed: '仅增减',
    },
    trend: {
      none: '—',
      up: '增',
      down: '减',
      flat: '平',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
  },
};
