// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/defect/defect-stat
// 文件名称：zh-CN.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计页静态文案；引用键 logistics.manufacturing.defect.defect-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '不良统计',
    periodRange: '期间',
    prodCategory: '生产类别',
    prodOrderCode: '工单',
    batchCode: '批次',
    modelCode: '机种',
    materialCode: '物料',
    selectPlantRequired: '请选择工厂',
    selectPeriodRequired: '请选择期间',
    summary: '{count} 行',
    exportSuccess: '清单导出成功',
    exportFailed: '清单导出失败',
    exportEmpty: '暂无数据可导出，请先查询',
    tabs: {
      assyOrder: '组立按工单',
      assyBatch: '组立按批次',
      smtInspection: 'SMT检查',
      pcbaRepair: 'PCBA修理',
    },
    smt: {
      inspectionDate: '检查日期',
      modelCode: '机种名称',
      pcbaBoardType: '板别',
      visualInspectionLine: '目视线别',
      aoiLine: 'AOI线别',
      bSideAssemblyDate: 'B面实装',
      tSideAssemblyDate: 'T面实装',
      shiftNo: '生产班别',
      inspectorName: '检查员',
      prodOrderCode: '工单号',
      batchCode: '批次',
      prodOrderQty: 'LOT台数',
      dailyCompletedQty: '当日完成数',
      inspectionQty: '检查台数',
      inspectionStatus: '检查状态',
      teamCode: '生产线别',
      inspectionWorkHours: '检查工数',
      aoiWorkHours: 'AOI工数',
      defectQty: '不良数量',
      handPlacement: '手贴部品',
      serialNumber: '序列号',
      content: '内容',
      defectLocation: '个所区分',
    },
    repair: {
      prodDate: '生产日期',
      modelCode: '生产机种',
      prodOrderCode: '生产订单',
      batchCode: '生产LOT',
      prodOrderQty: '订单台数',
      pcbaBoardType: '板别',
      prodActualQty: '生产实绩',
      teamCode: '生产组别',
      cardCode: '卡号',
      defectSymptom: '不良症状',
      defectEngineering: '检出工程',
      defectReason: '不良原因',
      defectQty: '不良数量',
      defectResponsibility: '责任归属',
      defectNature: '不良性质',
      repairOperatorName: '修理',
    },
    columns: {
      plantCode: '工厂',
      prodOrderCode: '工单',
      batchCode: '批次',
      modelCode: '机种',
      materialCode: '物料',
      actualQty: '实绩',
      defectQty: '不良数量',
      defectRatePercent: '不良率%',
      yieldRatePercent: '直行率%',
      lastProdDate: '最近生产日期',
    },
  },
};
