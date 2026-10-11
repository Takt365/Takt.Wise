// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/materials/material-moving-trend
// 文件名称：zh-HK.ts
// 创建时间：2026-07-17
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移動價格推移頁靜態文案；引用鍵 logistics.materials.material-moving-trend.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '移動價格推移',
    periodRange: '期間年月',
    materialCode: '物料編碼',
    selectPlantRequired: '請選擇工廠代碼',
    selectMaterialTypeRequired: '請選擇物料類型',
    selectValuationRequired: '請選擇評估類別',
    selectPeriodRequired: '請選擇期間年月',
    summary: '物料行 {count} 條（缺月/無價依次向前取最近有價月；* 懸停可查看來源月）',
    summaryModel: '物料行 {count} 條（機種/產品組來自 BOM：組件→產品→機種；缺月回填同左）',
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    carriedFrom: '回填自 {period}（該月無價格，沿用最近有價期間）',
    export: '清單匯出',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無資料可匯出，請先查詢',
    tabs: {
      price: '物料價格推移',
      model: '物料-機種-價格推移',
    },
    filter: {
      all: '全部',
      changed: '僅漲跌',
      leading: '領漲領跌前50',
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
      modelGroup: '機種組',
      productGroup: '產品組',
      materialText: '物料描述',
    },
  },
};
