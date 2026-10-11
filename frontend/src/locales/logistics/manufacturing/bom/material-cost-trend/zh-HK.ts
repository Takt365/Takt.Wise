// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-cost-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：產品成本分析頁；引用鍵 logistics.manufacturing.bom.material-cost-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '產品成本分析',
    periodRange: '核算年月',
    selectPlantRequired: '請選擇工廠代碼',
    selectModelRequired: '請選擇機種編碼',
    selectProductRequired: '請選擇產品編碼',
    selectMaterialTypeRequired: '請選擇物料類型',
    selectPeriodRequired: '請選擇核算期間',
    summary: '{plant} / {model} / {product} — 單個產品明細組件 {componentCount} 行（成本分析報表）',
    trendSummary:
      '明細環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat} · 新增 {added} · 剔除 {removed}',
    queryFailed: '產品成本分析查詢失敗',
    exportSuccess: '產品成本分析匯出成功',
    exportFailed: '產品成本分析匯出失敗',
    filter: {
      all: '全部',
      changed: '僅變動',
    },
    trend: {
      none: '—',
      up: '漲',
      down: '跌',
      flat: '平',
      new: '新增',
      removed: '剔除',
    },
    periodChange: {
      present: '有',
      absent: '無',
      new: '新增',
      removed: '剔除',
      up: '漲',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
    },
  },
};
