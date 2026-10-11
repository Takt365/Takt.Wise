// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移页静态文案；引用键 logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '月生产推移',
    periodRange: '期间年月',
    modelCode: '机种',
    outputCategory: '产出类别',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '机种行 {count} 条（按月汇总组立/PCBA 产量）',
    trendSummary: '环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
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
    outputCategoryOptions: {
      assy: '组立',
      pcba: 'PCBA',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
    },
  },
};
