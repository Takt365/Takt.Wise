// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-monthly
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生產不良推移頁靜態文案；引用鍵 logistics.manufacturing.defect.defect-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '月生產不良推移',
    periodRange: '期間年月',
    defectCategory: '不良類別',
    modelCode: '機種',
    selectPlantRequired: '請選擇工廠代碼',
    selectPeriodRequired: '請選擇期間年月',
    summary: '機種×不良類別行 {count} 條（按月匯總不良率）',
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無資料可匯出，請先查詢',
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
    defectCategoryOptions: {
      assy: '組立',
      pcba: 'PCBA',
    },
    columns: {
      trend: '漲跌',
      varianceAmount: '環比率差',
      variancePercent: '環比%',
    },
  },
};
