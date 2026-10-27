// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-stat
// 文件名称：zh-HK.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良統計頁靜態文案；引用鍵 logistics.manufacturing.defect.defect-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '不良統計',
    periodRange: '期間',
    prodCategory: '生產類別',
    prodOrderCode: '工單',
    batchCode: '批次',
    modelCode: '機種',
    materialCode: '物料',
    selectPlantRequired: '請選擇工廠',
    selectPeriodRequired: '請選擇期間',
    summary: '{count} 行',
    exportSuccess: '清單匯出成功',
    exportFailed: '清單匯出失敗',
    exportEmpty: '暫無數據可匯出，請先查詢',
    tabs: {
      assyOrder: '組立按工單',
      assyBatch: '組立按批次',
      smtInspection: 'SMT檢查',
      pcbaRepair: 'PCBA修理',
    },
    smt: {
      inspectionDate: '檢查日期',
      modelCode: '機種名稱',
      pcbaBoardType: '板別',
      visualInspectionLine: '目視線別',
      aoiLine: 'AOI線別',
      bSideAssemblyDate: 'B面實裝',
      tSideAssemblyDate: 'T面實裝',
      shiftNo: '生產班別',
      inspectorName: '檢查員',
      prodOrderCode: '工單號',
      batchCode: '批次',
      prodOrderQty: 'LOT台數',
      dailyCompletedQty: '當日完成數',
      inspectionQty: '檢查台數',
      inspectionStatus: '檢查狀態',
      teamCode: '生產線別',
      inspectionWorkHours: '檢查工數',
      aoiWorkHours: 'AOI工數',
      defectQty: '不良數量',
      handPlacement: '手貼部品',
      serialNumber: '序列號',
      content: '內容',
      defectLocation: '個所區分',
    },
    repair: {
      prodDate: '生產日期',
      modelCode: '生產機種',
      prodOrderCode: '生產訂單',
      batchCode: '生產LOT',
      prodOrderQty: '訂單台數',
      pcbaBoardType: '板別',
      prodActualQty: '生產實績',
      teamCode: '生產組別',
      cardCode: '卡號',
      defectSymptom: '不良症狀',
      defectEngineering: '檢出工程',
      defectReason: '不良原因',
      defectQty: '不良數量',
      defectResponsibility: '責任歸屬',
      defectNature: '不良性質',
      repairOperatorName: '修理',
    },
    columns: {
      plantCode: '工廠',
      prodOrderCode: '工單',
      batchCode: '批次',
      modelCode: '機種',
      materialCode: '物料',
      actualQty: '實績',
      defectQty: '不良數量',
      defectRatePercent: '不良率%',
      yieldRatePercent: '直行率%',
      lastProdDate: '最近生產日期',
    },
  },
};
