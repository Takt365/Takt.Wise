// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/quality/operation/fqc-trend
// 文件名称：zh-HK.ts
// 功能描述：成品檢驗推移頁；引用鍵 logistics.quality.operation.fqc-trend.page.*
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    title: '成品檢驗推移',
    periodRange: '期間年月',
    customerCode: '客戶編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '客戶行 {count} 條（按月展示不良率）',
    trendSummary: '環比 {base} → {compare}：升 {up} · 降 {down} · 平 {flat}',
    exportSuccess: '清單導出成功',
    exportFailed: '清單導出失敗',
    exportEmpty: '暫無數據可導出，請先查詢',
    filter: { all: '全部', changed: '僅漲跌' },
    trend: { none: '—', up: '升', down: '降', flat: '平' },
    columns: { trend: '漲跌', varianceAmount: '環比差額', variancePercent: '環比%', defectRate: '不良率' },
  },
};
