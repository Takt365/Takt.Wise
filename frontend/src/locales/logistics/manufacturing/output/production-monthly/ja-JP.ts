// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/output/production-monthly
// 文件名称：ja-JP.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月次生産推移ページ文言；キー logistics.manufacturing.output.production-monthly.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '生産統計',
    periodRange: '期間',
    teamCode: '班',
    modelCode: '機種',
    materialCode: '品目',
    prodCategory: '生産区分',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    summary: '{count} 行',
    tabs: {
      assyTeam: '組立・班別',
      assyModel: '組立・機種別',
      pcbaTeam: 'PCBA・班別',
      pcbaModel: 'PCBA・機種別',
    },
    trendSummary: '前月比 {base} → {compare}：上昇 {up} · 下降 {down} · 横ばい {flat}',
    exportSuccess: '一覧出力に成功しました',
    exportFailed: '一覧出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    filter: {
      all: 'すべて',
      changed: '変動のみ',
    },
    trend: {
      none: '—',
      up: '上昇',
      down: '下降',
      flat: '横ばい',
    },
    columns: {
      trend: '推移',
      varianceAmount: '前月比差額',
      variancePercent: '前月比%',
      teamCode: '生産班',
      pcbaTeamCode: '班',
      prodDate: '生産日',
      batchCode: 'ロット',
      materialCode: '品目',
      modelCode: '機種',
      modelCodeOnly: '機種',
      stdMinutes: 'ST',
      planQty: '計画台数',
      directLabor: '直接',
      directLaborCount: '直接人員',
      indirectLabor: '間接',
      laborMinutes: '工数',
      laborMinutesFull: '生産工数',
      actualQty: '実績台数',
      actualStdMinutes: '実績ST',
      stdDiff: 'ST差異',
      qtyDiff: '台数差異',
      achievementRate: '達成率',
      lotQty: 'LOT数量',
      pcbBoardType: 'PCB区分',
      panelSide: '面',
      pcbaActualQty: '生産実績',
      cumulativeQty: '累計生産',
      completedStatus: '完了状態',
      defectQty: '不良数量',
      defectUnits: '不良台数',
    },
  },
};
