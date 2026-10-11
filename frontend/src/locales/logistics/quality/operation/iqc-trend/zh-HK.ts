// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/iqc-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：進貨檢驗推移頁；引用鍵 logistics.quality.operation.iqc-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '進貨檢驗推移',
    periodRange: '期間年月',
    supplierCode: '供應商編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '供應商行 {count} 條（按月展示不良率）',
    trendSummary: '環比 {base} → {compare}：升 {up} · 降 {down} · 平 {flat}',
    exportSuccess: '清單導出成功',
    exportFailed: '清單導出失敗',
    exportEmpty: '暫無數據可導出，請先查詢',
    filter: {
      all: '全部',
      changed: '僅漲跌',
    },
    trend: {
      none: '—',
      up: '升',
      down: '降',
      flat: '平',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
      defectRate: '不良率',
    },
  },
};
