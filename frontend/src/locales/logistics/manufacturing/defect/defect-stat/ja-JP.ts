// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-stat
// 文件名称：ja-JP.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良統計ページ文言；キー logistics.manufacturing.defect.defect-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '不良統計',
    periodRange: '期間',
    prodCategory: '生産区分',
    prodOrderCode: '工単',
    batchCode: 'ロット',
    modelCode: '機種',
    materialCode: '品目',
    selectPlantRequired: '工場を選択してください',
    selectPeriodRequired: '期間を選択してください',
    summary: '{count} 行',
    exportSuccess: '一覧出力に成功しました',
    exportFailed: '一覧出力に失敗しました',
    exportEmpty: '出力するデータがありません。先に検索してください',
    tabs: {
      assyOrder: '組立・工単',
      assyBatch: '組立・ロット',
      smtInspection: 'SMT検査',
      pcbaRepair: 'PCBA修理',
    },
    smt: {
      inspectionDate: '検査日',
      modelCode: '機種名',
      pcbaBoardType: '板別',
      visualInspectionLine: '目視線',
      aoiLine: 'AOI線',
      bSideAssemblyDate: 'B面実装',
      tSideAssemblyDate: 'T面実装',
      shiftNo: '生産班',
      inspectorName: '検査員',
      prodOrderCode: '工単',
      batchCode: 'ロット',
      prodOrderQty: 'LOT台数',
      dailyCompletedQty: '当日完成',
      inspectionQty: '検査台数',
      inspectionStatus: '検査状態',
      teamCode: '生産ライン',
      inspectionWorkHours: '検査工数',
      aoiWorkHours: 'AOI工数',
      defectQty: '不良数',
      handPlacement: '手貼部品',
      serialNumber: 'シリアル',
      content: '内容',
      defectLocation: '個所区分',
    },
    repair: {
      prodDate: '生産日',
      modelCode: '生産機種',
      prodOrderCode: '生産注文',
      batchCode: '生産LOT',
      prodOrderQty: '注文台数',
      pcbaBoardType: '板別',
      prodActualQty: '生産実績',
      teamCode: '生産組',
      cardCode: 'カード',
      defectSymptom: '不良症状',
      defectEngineering: '検出工程',
      defectReason: '不良原因',
      defectQty: '不良数',
      defectResponsibility: '責任帰属',
      defectNature: '不良性質',
      repairOperatorName: '修理',
    },
    columns: {
      plantCode: '工場',
      prodOrderCode: '工単',
      batchCode: 'ロット',
      modelCode: '機種',
      materialCode: '品目',
      actualQty: '実績',
      defectQty: '不良数',
      defectRatePercent: '不良率%',
      yieldRatePercent: '直行率%',
      lastProdDate: '最終生産日',
    },
  },
};
