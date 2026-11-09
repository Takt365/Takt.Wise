// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：en-US.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：Kanban page-only copy (no entity/dict seed); column titles prefer entity.ecte.* / entity.ecexec.* / org.dept.*. Keys logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tabs: {
      board: 'Board',
      /** PC → Mfg2 → Mfg1 → Qc (same row grain) */
      production: 'Batch',
      pur: 'Purchasing',
      iqc: 'IQC',
      mc: 'MC',
      mtl: 'Material',
    },
    kpi: {
      ecTotal: 'EC count',
    },
    charts: {
      statusTitle: 'Dept implemented vs not',
      sunburstTitle: 'Sunburst: master → detail → depts → done/open',
      monthTitle: 'EC entries by month',
      implemented: 'Implemented',
      notImplemented: 'Not implemented',
      masterRing: 'Master',
      detailRing: 'Detail',
      deptRing: 'Dept exec',
      tooltipMaster: 'Master rows',
      tooltipDetail: 'Detail rows',
      tooltipDept: 'Dept',
      tooltipDeptRows: 'Dept rows',
      tooltipImplemented: 'Implemented',
      tooltipNotImplemented: 'Not implemented',
    },
    filter: {
      implementationStatus: 'Implemented',
      dept: 'Department',
    },
    column: {
      issueDate: 'Issue date',
      leader: 'Owner',
      ecCode: 'EC No.',
      model: 'Model',
      finishedProduct: 'Product',
      tech: 'Engineering',
      pur: 'Purchasing',
      pc: 'Production control',
      iqc: 'Incoming',
      mc: 'Material control',
      mtl: 'Material',
      mfg2: 'MFG2',
      mfg1: 'MFG1',
      qc: 'Quality',
      me: 'MFG technology',
    },
    implemented: 'done',
    /** Purchasing / IQC / MC / Material row summary (empty hits keep labels, blank numbers) */
    deptCount: {
      summary: '{rows}:{done}:{pending}',
      empty: '',
      tooltip: 'Detail:{rows},Done:{done},Open:{pending}',
      tooltipEmpty: '',
    },
    pathLabel: {
      pc: 'PC',
      pur: 'Purchasing',
      iqc: 'IQC',
      mc: 'Material control',
      mtl: 'Materials',
      mfg2: 'MFG2',
      mfg1: 'MFG1',
      qc: 'QA',
    },
    implLabel: {
      pc: 'PC',
      pur: 'Purchasing',
      iqc: 'IQC',
      mc: 'Material control',
      mtl: 'Materials',
      mfg2: 'MFG2',
      mfg1: 'MFG1',
      qc: 'QA',
    },
    implementationStatus: {
      notStarted: 'Not started',
      inProgress: 'In progress',
      officiallyCompleted: 'Officially completed',
      fullyCompleted: 'Fully completed',
      deptCompleted: 'Completed',
    },
    batch: {
      group: {
        ecInfo: 'EC',
        pur: 'Purchasing',
        pc: 'Production control',
        iqc: 'Incoming inspection',
        mc: 'Material control',
        mtl: 'Material',
        mfg2: 'MFG2',
        mfg1: 'MFG1',
        qc: 'Quality',
        me: 'MFG technology',
      },
      column: {
        ecCode: 'EC No.',
        liaisonNo: 'Liaison No.',
        pNo: 'P No.',
        tcjDate: 'TCJ date',
        issueDate: 'Issue date',
        model: 'Model',
        product: 'Product',
        entryDate: 'Entry date',
        orderDate: 'Order date',
        orderCode: 'Order No.',
        inboundDate: 'Inbound date',
        scheduledInput: 'Planned input',
        productionSchedule: 'Production plan',
        inspectionDate: 'Inspection date',
        outboundDate: 'Outbound date',
        outboundBatch: 'Issue batch',
        productionDate: 'Production date',
        productionBatch: 'Production batch',
        sampleDate: 'Sample date',
        sampleBatch: 'Sample batch',
        sopDate: 'SOP date',
      },
    },
  },
};
