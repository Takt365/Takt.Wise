// ========================================
// 项目名称：节拍智控·TaktWise
// 文件名称：en-US.ts
// 功能描述：Manufacturing MRP page static copy; keys logistics.manufacturing.mrp.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

export default {
  page: {
    wizard: {
      title: 'MRP Run Wizard',
      step: {
        mps: 'Select MPS',
        options: 'Run Options',
        run: 'Execute',
        preview: 'Preview',
        publish: 'Publish',
      },
      mpsHint: 'Confirm the MRP header is linked to a master production schedule (MPS).',
      bomType: 'BOM Type',
      maxBomLevel: 'Max BOM Level',
      includePo: 'Include Open POs',
      includePlanned: 'Include Planned Orders',
      runReady: 'Click Run to start MRP (BOM explosion + netting).',
      publishHint: 'Publishing creates planned orders and purchase plans.',
      runSuccess: 'MRP run completed',
      publishSuccess: 'MRP published',
    },
    flow: {
      wizard: 'MRP Wizard',
      trace: 'Supply/Demand Trace',
    },
    mpsFromMds: {
      title: 'Generate MPS from MDS',
      mds: 'Master Demand Schedule',
      success: 'MPS generated',
    },
    apsSchedule: {
      pickOrders: 'Select APS Orders',
      scheduleSuccess: 'APS scheduling completed',
      releaseSuccess: 'Work orders released',
    },
    purchasePlan: {
      convertSuccess: 'Converted to purchase request',
    },
    plannedOrder: {
      releaseSuccess: 'Released to APS',
    },
  },
};
