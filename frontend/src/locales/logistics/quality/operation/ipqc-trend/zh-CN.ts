// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/ipqc-trend
// 文件名称：zh-CN.ts
// 功能描述：过程质量推移页静态文案；引用键 logistics.quality.operation.ipqc-trend.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    title: '过程质量推移',
    periodRange: '期间年月',
    processCode: '工序编码',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '工序行 {count} 条（按月展示不良率；无抽样月份留空）',
    trendSummary: '环比 {base} → {compare}：升 {up} · 降 {down} · 平 {flat}',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    filter: { all: '全部', changed: '仅涨跌' },
    trend: { none: '—', up: '升', down: '降', flat: '平' },
    columns: { trend: '涨跌', varianceAmount: '环比差额', variancePercent: '环比%', defectRate: '不良率' },
  },
};
