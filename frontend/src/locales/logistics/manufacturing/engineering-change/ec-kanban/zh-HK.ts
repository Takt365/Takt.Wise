// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-HK.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設變看板頁面專有文案（無對應 entity/dict 種子）；欄標題優先 entity.ecte.* / entity.ecexec.* / org.dept.*；引用鍵 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tabs: {
      board: '看板',
      impl: '實施',
      batch: '批次',
    },
    kpi: {
      ecTotal: '設變單數',
    },
    charts: {
      statusTitle: '各部門已實施 / 未實施對照',
      sunburstTitle: '旭日圖：主表 → 明細 → 各部門 → 實施/未實施',
      monthTitle: '按月錄入設變',
      implemented: '已實施',
      notImplemented: '未實施',
      masterRing: '主表',
      detailRing: '明細',
      deptRing: '部門執行',
      tooltipMaster: '主表記錄數',
      tooltipDetail: '明細行數',
      tooltipDept: '部門',
      tooltipDeptRows: '部門行數',
      tooltipImplemented: '已實施',
      tooltipNotImplemented: '未實施',
    },
    filter: {
      implementationStatus: '實施',
      dept: '部門',
    },
    column: {
      issueDate: '發行日期',
      leader: '技術擔當',
      ecCode: '設變號碼',
      model: '機種',
      finishedProduct: '成品',
      tech: '技術課',
      pur: '採購課',
      pc: '生管課',
      iqc: '受檢課',
      mc: '部管課',
      mtl: '物料',
      mfg2: '制二課',
      mfg1: '制一課',
      qc: '品管課',
      me: '制技',
    },
    implemented: '實施',
    /** 採購/受檢/部管/物料：行數匯總（空命中仍顯示標籤、數字留空） */
    deptCount: {
      summary: '{rows}:{done}:{pending}',
      empty: '',
      tooltip: '明細:{rows},實施:{done},未實施:{pending}',
      tooltipEmpty: '',
    },
    pathLabel: {
      pc: '生管',
      pur: '採購',
      iqc: '受檢',
      mc: '部管',
      mtl: '物料',
      mfg2: '製二課',
      mfg1: '製一課',
      qc: '品管課',
    },
    implLabel: {
      pc: '生管課',
      pur: '採購課',
      iqc: '受檢課',
      mc: '部管課',
      mtl: '物料',
      mfg2: '製二課',
      mfg1: '製一課',
      qc: '品管課',
    },
    implementationStatus: {
      notStarted: '未開始',
      inProgress: '實施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
      deptCompleted: '已完成',
    },
    batch: {
      group: {
        ecInfo: '設變信息',
        pur: '採購課',
        pc: '生管課',
        iqc: '受檢課',
        mc: '部管課',
        mtl: '物料',
        mfg2: '制二課',
        mfg1: '制一課',
        qc: '品管課',
        me: '制技',
      },
      column: {
        ecCode: '設變號碼',
        liaisonNo: '接取No.',
        pNo: 'P番No.',
        tcjDate: 'TCJ接取日期',
        issueDate: '發行日期',
        model: '機種',
        product: '成品',
        entryDate: '登入日期',
        orderDate: '訂單日期',
        orderCode: '訂單號碼',
        inboundDate: '進貨日期',
        scheduledInput: '預定投入',
        productionSchedule: '生產預定',
        inspectionDate: '檢驗日期',
        outboundDate: '出庫日期',
        outboundBatch: '出庫批次',
        productionDate: '生產日期',
        productionBatch: '生產批次',
        sampleDate: '檢樣日期',
        sampleBatch: '檢校批次',
        sopDate: 'SOP日期',
      },
    },
  },
};
