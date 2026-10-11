// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/cost/cost-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本推移页静态文案；引用键 logistics.quality.cost.cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '質量成本推移',
    periodRange: '期間年月',
    costCategory: '成本類別',
    currencyCode: '成本幣種',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '成本類別行 {count} 條（按月匯總品質保證/問題/事故成本）',
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    export: '清單導出',
    exportSuccess: '清單導出成功',
    exportFailed: '清單導出失敗',
    exportEmpty: '暫無數據可導出，請先查詢',
    filter: {
      all: '全部',
      changed: '僅漲跌',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
    },
    costCategoryOptions: {
      assurance: '品質保證',
      issue: '品質問題',
      incident: '品質事故',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
    },
  },
};
