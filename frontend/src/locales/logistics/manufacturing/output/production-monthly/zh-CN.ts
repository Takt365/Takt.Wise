// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：zh-CN.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移页静态文案；引用键 logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '生产统计',
    periodRange: '期间',
    teamCode: '班组',
    modelCode: '机种',
    materialCode: '物料',
    prodCategory: '生产类别',
    selectPlantRequired: '请选择工厂',
    selectPeriodRequired: '请选择期间',
    summary: '{count} 行',
    tabs: {
      assyTeam: '组立按班组',
      assyModel: '组立按机种',
      pcbaTeam: 'PCBA按班组',
      pcbaModel: 'PCBA按机种',
    },
    trendSummary: '环比 {base} → {compare}：涨 {up} · 跌 {down} · 平 {flat}',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    filter: {
      all: '全部',
      changed: '仅涨跌',
    },
    trend: {
      none: '—',
      up: '涨',
      down: '跌',
      flat: '平',
    },
    columns: {
      trend: '涨跌',
      varianceAmount: '环比差额',
      variancePercent: '环比%',
      teamCode: '生产班组',
      pcbaTeamCode: '班组',
      prodDate: '生产日期',
      batchCode: '生产批次',
      materialCode: '物料',
      modelCode: '机种',
      modelCodeOnly: '机种编码',
      stdMinutes: 'ST',
      planQty: '计划台数',
      directLabor: '直接',
      directLaborCount: '直接人数',
      indirectLabor: '间接',
      laborMinutes: '工数',
      laborMinutesFull: '生产工数',
      actualQty: '实绩台数',
      actualStdMinutes: '实绩ST',
      stdDiff: 'ST差异',
      qtyDiff: '台数差异',
      achievementRate: '达成率',
      lotQty: 'LOT数量',
      pcbBoardType: 'Pcb类别',
      panelSide: '板面',
      pcbaActualQty: '生产实绩',
      cumulativeQty: '累计生产',
      completedStatus: '完成状态',
      defectQty: '不良数量',
      defectUnits: '不良台数',
    },
  },
};
