// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-CN.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板页面专有文案（无对应 entity/dict 种子）；列标题优先 entity.ecte.* / entity.ecexec.* / org.dept.*；引用键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tabs: {
      board: '看板',
      impl: '实施',
      batch: '批次',
    },
    kpi: {
      ecTotal: '设变单数',
    },
    charts: {
      statusTitle: '实施状态分布',
      deptTitle: '各部门执行行（已实施 / 未实施）',
      monthTitle: '按月录入设变',
      implemented: '已实施',
      notImplemented: '未实施',
    },
    filter: {
      implementationStatus: '实施',
      dept: '部门',
    },
    column: {
      issueDate: '发行日期',
      leader: '技术担当',
      ecCode: '设变号码',
      model: '机种',
      finishedProduct: '成品',
      tech: '技术课',
      pur: '采购课',
      pc: '生管课',
      iqc: '受检课',
      mc: '部管课',
      mtl: '物料',
      mfg2: '制二课',
      mfg1: '制一课',
      qc: '品管课',
      me: '制技',
    },
    implemented: '实施',
    pathLabel: {
      pc: '生管',
      pur: '采购',
      iqc: '受检',
      mc: '部管',
      mtl: '物料',
      mfg2: '制二课',
      mfg1: '制一课',
      qc: '品管课',
    },
    implLabel: {
      pc: '生管课',
      pur: '采购课',
      iqc: '受检课',
      mc: '部管课',
      mtl: '物料',
      mfg2: '制二课',
      mfg1: '制一课',
      qc: '品管课',
    },
    implementationStatus: {
      notStarted: '未开始',
      inProgress: '实施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
    },
    batch: {
      group: {
        ecInfo: '设变信息',
        pur: '采购课',
        pc: '生管课',
        iqc: '受检课',
        mc: '部管课',
        mtl: '物料',
        mfg2: '制二课',
        mfg1: '制一课',
        qc: '品管课',
        me: '制技',
      },
      column: {
        ecCode: '设变号码',
        liaisonNo: '接取No.',
        pNo: 'P番No.',
        tcjDate: 'TCJ接取日期',
        issueDate: '发行日期',
        model: '机种',
        product: '成品',
        entryDate: '登入日期',
        orderDate: '订单日期',
        orderCode: '订单号码',
        scheduledInput: '预定投入',
        productionSchedule: '生产预定',
        inspectionDate: '检验日期',
        iqcOrderCode: '受检单号',
        outboundDate: '出库日期',
        outboundBatch: '出库批次',
        productionDate: '生产日期',
        productionBatch: '生产批次',
        sampleDate: '检样日期',
        sopDate: 'SOP日期',
      },
    },
  },
};
