// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-stat
// 文件名称：en-US.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计页静态文案；引用键 logistics.manufacturing.engineering-change.ec-stat.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'EC statistics',
    issuePeriodRange: 'Issue period',
    entryPeriodRange: 'Entry period',
    ecCode: 'EC No.',
    selectPlantRequired: 'Select a plant',
    selectPeriodRequired: 'Select an issue period or an entry period',
    exportSuccess: 'Export succeeded',
    exportFailed: 'Export failed',
    exportEmpty: 'Query first. There is nothing to export',
    tabs: {
      pur: 'Purchasing',
      pc: 'PC',
      iqc: 'Incoming',
      mc: 'Material control',
      mtl: 'Material',
      mfg2: 'MFG 2',
      mfg1: 'MFG 1',
      qc: 'QA',
      me: 'Manufacturing engineering',
    },
    status: {
      all: 'All',
      done: 'Implemented',
      open: 'Not implemented',
      me: {
        done: 'SOP updated',
        open: 'SOP not updated',
      },
    },
    columns: {
      plantCode: 'Plant',
      ecCode: 'EC No.',
      lineCount: 'Lines',
      implementedCount: 'Implemented',
      pendingCount: 'Not implemented',
      ratePercent: 'Implemented %',
      meImplementedCount: 'Updated',
      tePendingCount: 'Not updated',
      teRatePercent: 'Updated %',
    },
  },
};
