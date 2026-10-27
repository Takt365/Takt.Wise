// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生產推移頁靜態文案；引用鍵 logistics.manufacturing.output.production-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '生產推移',
    periodRange: '期間',
    teamCode: '班組',
    modelCode: '機種',
    selectPlantRequired: '請選擇工廠',
    selectPeriodRequired: '請選擇期間',
    summary: '{count} 行',
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
    tabs: {
      assy: '組立',
      pcba: 'PCBA',
    },
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
    columns: {
      trend: '漲跌',
      varianceAmount: '環比差額',
      variancePercent: '環比%',
    },
  },
};
