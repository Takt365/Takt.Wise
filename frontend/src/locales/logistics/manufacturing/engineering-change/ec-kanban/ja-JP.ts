// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：ja-JP.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設変看板のページ専有文案（entity/dict 種子なし）。列見出しは entity.ecte.* / entity.ecexec.* / org.dept.* を優先。参照键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tabs: {
      board: '看板',
      /** 生管 → 製二 → 製一 → 品管（行数一致） */
      production: 'バッチ',
      pur: '購買',
      iqc: '受入検査',
      mc: '部管',
      mtl: '物料',
    },
    kpi: {
      ecTotal: '設変数',
    },
    charts: {
      statusTitle: '部門別 実施済 / 未実施',
      sunburstTitle: 'サンバースト：主表 → 明細 → 部門 → 実施/未実施',
      monthTitle: '月別登録設変',
      implemented: '実施済',
      notImplemented: '未実施',
      masterRing: '主表',
      detailRing: '明細',
      deptRing: '部門実行',
      tooltipMaster: '主表件数',
      tooltipDetail: '明細行数',
      tooltipDept: '部門',
      tooltipDeptRows: '部門行数',
      tooltipImplemented: '実施済',
      tooltipNotImplemented: '未実施',
    },
    filter: {
      implementationStatus: '実施',
      dept: '部門',
    },
    column: {
      issueDate: '発行日',
      leader: '技術担当',
      ecCode: '設変番号',
      model: '機種',
      finishedProduct: '製品',
      tech: '技術課',
      pur: '購買課',
      pc: '生管課',
      iqc: '受検課',
      mc: '部管課',
      mtl: '物料',
      mfg2: '製二課',
      mfg1: '製一課',
      qc: '品管課',
      me: '製技',
    },
    implemented: '実施',
    /** 購買/受検/部管/物料：行数集計（未命中もラベル表示・数値は空） */
    deptCount: {
      summary: '{rows}:{done}:{pending}',
      empty: '',
      tooltip: '明細:{rows},実施:{done},未実施:{pending}',
      tooltipEmpty: '',
    },
    pathLabel: {
      pc: '生管',
      pur: '購買',
      iqc: '受検',
      mc: '部管',
      mtl: '物料',
      mfg2: '製二課',
      mfg1: '製一課',
      qc: '品管課',
    },
    implLabel: {
      pc: '生管課',
      pur: '購買課',
      iqc: '受検課',
      mc: '部管課',
      mtl: '物料',
      mfg2: '製二課',
      mfg1: '製一課',
      qc: '品管課',
    },
    implementationStatus: {
      notStarted: '未着手',
      inProgress: '実施中',
      officiallyCompleted: '正式完了',
      fullyCompleted: '全完了',
      deptCompleted: '完了',
    },
    batch: {
      group: {
        ecInfo: '設変情報',
        pur: '購買課',
        pc: '生管課',
        iqc: '受検課',
        mc: '部管課',
        mtl: '物料',
        mfg2: '製二課',
        mfg1: '製一課',
        qc: '品管課',
        me: '製技',
      },
      column: {
        ecCode: '設変番号',
        liaisonNo: '接取No.',
        pNo: 'P番No.',
        tcjDate: 'TCJ接取日',
        issueDate: '発行日',
        model: '機種',
        product: '製品',
        entryDate: '登録日',
        orderDate: '注文日',
        orderCode: '注文番号',
        inboundDate: '入荷日',
        scheduledInput: '予定投入',
        productionSchedule: '生産予定',
        inspectionDate: '検査日',
        outboundDate: '出庫日',
        outboundBatch: '出庫ロット',
        productionDate: '生産日',
        productionBatch: '生産ロット',
        sampleDate: '検様日',
        sampleBatch: '検校ロット',
        sopDate: 'SOP日',
      },
    },
  },
};
