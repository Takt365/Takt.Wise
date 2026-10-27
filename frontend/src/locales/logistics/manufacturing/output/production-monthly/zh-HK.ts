// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：zh-HK.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生產推移頁靜態文案；引用鍵 logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '生產統計',
    periodRange: '期間',
    teamCode: '班組',
    modelCode: '機種',
    materialCode: '物料',
    prodCategory: '生產類別',
    selectPlantRequired: '請選擇工廠',
    selectPeriodRequired: '請選擇期間',
    summary: '{count} 行',
    tabs: {
      assyTeam: '組立按班組',
      assyModel: '組立按機種',
      pcbaTeam: 'PCBA按班組',
      pcbaModel: 'PCBA按機種',
    },
    trendSummary: '環比 {base} → {compare}：漲 {up} · 跌 {down} · 平 {flat}',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
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
      teamCode: '生產班組',
      pcbaTeamCode: '班組',
      prodDate: '生產日期',
      batchCode: '生產批次',
      materialCode: '物料',
      modelCode: '機種',
      modelCodeOnly: '機種編碼',
      stdMinutes: 'ST',
      planQty: '計劃台數',
      directLabor: '直接',
      directLaborCount: '直接人數',
      indirectLabor: '間接',
      laborMinutes: '工數',
      laborMinutesFull: '生產工數',
      actualQty: '實績台數',
      actualStdMinutes: '實績ST',
      stdDiff: 'ST差異',
      qtyDiff: '台數差異',
      achievementRate: '達成率',
      lotQty: 'LOT數量',
      pcbBoardType: 'Pcb類別',
      panelSide: '板面',
      pcbaActualQty: '生產實績',
      cumulativeQty: '累計生產',
      completedStatus: '完成狀態',
      defectQty: '不良數量',
      defectUnits: '不良台數',
    },
  },
};
