// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-CN.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板页面专有文案（无对应 entity/dict 种子）；列标题优先 entity.ecgijutsu.* / entity.ecexec.* / org.dept.*；引用键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    filter: {
      implementationStatus: '实施状态',
    },
    column: {
      detailCount: '明细数',
      pendingCount: '待实施数',
      implementationStatus: '实施状态',
      path: '路径',
      implementation: '实施',
    },
    implemented: '实施',
    pathLabel: {
      pmc: '生管',
      mp: '采购',
      iqc: '受检',
      mc: '部管',
      smt: '物料',
      pcba: '制二课',
      assy: '制一课',
      qa: '品管课',
    },
    implLabel: {
      pmc: '生管课',
      mp: '采购课',
      iqc: '受检课',
      mc: '部管课',
      smt: '物料',
      pcba: '制二课',
      assy: '制一课',
      qa: '品管课',
    },
    implementationStatus: {
      notStarted: '未开始',
      inProgress: '实施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
    },
  },
};
