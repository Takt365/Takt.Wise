// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-stat
// 文件名称：en-US.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Defect statistics page copy; keys logistics.manufacturing.defect.defect-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：This software uses MIT License; the author assumes no liability for use.
// ========================================

export default {
  page: {
    title: 'Defect statistics',
    periodRange: 'Period',
    prodCategory: 'Production category',
    prodOrderCode: 'Order',
    batchCode: 'Batch',
    modelCode: 'Model',
    materialCode: 'Material',
    selectPlantRequired: 'Please select a plant',
    selectPeriodRequired: 'Please select a period',
    summary: '{count} rows',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'No data to export. Run a query first.',
    tabs: {
      assyOrder: 'Assembly by order',
      assyBatch: 'Assembly by batch',
      smtInspection: 'SMT inspection',
      pcbaRepair: 'PCBA repair',
    },
    smt: {
      inspectionDate: 'Inspection date',
      modelCode: 'Model name',
      pcbaBoardType: 'Board',
      visualInspectionLine: 'Visual line',
      aoiLine: 'AOI line',
      bSideAssemblyDate: 'B-side assembly',
      tSideAssemblyDate: 'T-side assembly',
      shiftNo: 'Shift',
      inspectorName: 'Inspector',
      prodOrderCode: 'Order',
      batchCode: 'Batch',
      prodOrderQty: 'LOT qty',
      dailyCompletedQty: 'Completed today',
      inspectionQty: 'Inspected qty',
      inspectionStatus: 'Inspection status',
      teamCode: 'Line',
      inspectionWorkHours: 'Inspection hours',
      aoiWorkHours: 'AOI hours',
      defectQty: 'Defect qty',
      handPlacement: 'Hand placement',
      serialNumber: 'Serial',
      content: 'Content',
      defectLocation: 'Location',
    },
    repair: {
      prodDate: 'Production date',
      modelCode: 'Model',
      prodOrderCode: 'Order',
      batchCode: 'LOT',
      prodOrderQty: 'Order qty',
      pcbaBoardType: 'Board',
      prodActualQty: 'Actual qty',
      teamCode: 'Team',
      cardCode: 'Card',
      defectSymptom: 'Symptom',
      defectEngineering: 'Detected process',
      defectReason: 'Cause',
      defectQty: 'Defect qty',
      defectResponsibility: 'Responsibility',
      defectNature: 'Nature',
      repairOperatorName: 'Repair',
    },
    columns: {
      plantCode: 'Plant',
      prodOrderCode: 'Order',
      batchCode: 'Batch',
      modelCode: 'Model',
      materialCode: 'Material',
      actualQty: 'Actual',
      defectQty: 'Defect qty',
      defectRatePercent: 'Defect %',
      yieldRatePercent: 'Yield %',
      lastProdDate: 'Last production date',
    },
  },
};
