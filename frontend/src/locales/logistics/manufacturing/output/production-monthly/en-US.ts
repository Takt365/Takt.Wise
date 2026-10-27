// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：en-US.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：Monthly production trend page copy; keys logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Production statistics',
    periodRange: 'Period',
    teamCode: 'Team',
    modelCode: 'Model',
    materialCode: 'Material',
    prodCategory: 'Production category',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    summary: '{count} rows',
    tabs: {
      assyTeam: 'Assembly by team',
      assyModel: 'Assembly by model',
      pcbaTeam: 'PCBA by team',
      pcbaModel: 'PCBA by model',
    },
    trendSummary: 'MoM {base} → {compare}: up {up} · down {down} · flat {flat}',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    filter: {
      all: 'All',
      changed: 'Changed only',
    },
    trend: {
      none: '—',
      up: 'Up',
      down: 'Down',
      flat: 'Flat',
    },
    columns: {
      trend: 'Trend',
      varianceAmount: 'MoM Δ',
      variancePercent: 'MoM %',
      teamCode: 'Team',
      pcbaTeamCode: 'Team',
      prodDate: 'Production date',
      batchCode: 'Batch',
      materialCode: 'Material',
      modelCode: 'Model',
      modelCodeOnly: 'Model',
      stdMinutes: 'ST',
      planQty: 'Plan qty',
      directLabor: 'Direct',
      directLaborCount: 'Direct headcount',
      indirectLabor: 'Indirect',
      laborMinutes: 'Labor minutes',
      laborMinutesFull: 'Labor minutes',
      actualQty: 'Actual qty',
      actualStdMinutes: 'Actual ST',
      stdDiff: 'ST variance',
      qtyDiff: 'Qty variance',
      achievementRate: 'Achievement',
      lotQty: 'Lot qty',
      pcbBoardType: 'PCB type',
      panelSide: 'Side',
      pcbaActualQty: 'Output',
      cumulativeQty: 'Cumulative',
      completedStatus: 'Completion',
      defectQty: 'Defect qty',
      defectUnits: 'Defect units',
    },
  },
};
