// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/complaint/customer-complaint-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉推移页静态文案；引用键 logistics.quality.complaint.customer-complaint-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '顧客投訴推移',
    periodRange: '期間年月',
    customerCode: '客戶編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '客戶行 {count} 條（按月統計投訴件數）',
    trendSummary: '環比 {base} → {compare}：增 {up} · 減 {down} · 平 {flat}',
    export: '清單導出',
    exportSuccess: '清單導出成功',
    exportFailed: '清單導出失敗',
    exportEmpty: '暫無數據可導出，請先查詢',
    filter: {
      all: '全部',
      changed: '僅增減',
    },
    trend: {
      none: '—',
      up: '增',
      down: '減',
      flat: '平',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
    },
  },
};
