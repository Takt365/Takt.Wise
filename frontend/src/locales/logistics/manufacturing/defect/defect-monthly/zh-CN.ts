// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-monthly
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移页静态文案；引用键 logistics.manufacturing.defect.defect-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '不良推移',
    tabs: {
      assy: '组立',
      pcba: 'PCBA',
    },
    periodRange: '期间年月',
    defectCategory: '不良类别',
    modelCode: '机种',
    selectPlantRequired: '请选择工厂代码',
    selectPeriodRequired: '请选择期间年月',
    summary: '机种×不良类别行 {count} 条（按月汇总不良率）',
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
    defectCategoryOptions: {
      assy: '组立',
      pcba: 'PCBA',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比率差',
      variancePercent: '环比%',
    },
  },
};
