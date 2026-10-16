// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：zh-HK.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設變看板頁面專有文案（無對應 entity/dict 種子）；欄標題優先 entity.ecgijutsu.* / entity.ecexec.* / org.dept.*；引用鍵 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    filter: {
      implementationStatus: '實施狀態',
    },
    column: {
      detailCount: '明細數',
      pendingCount: '待實施數',
      implementationStatus: '實施狀態',
      path: '路徑',
      implementation: '實施',
    },
    implemented: '實施',
    pathLabel: {
      pmc: '生管',
      mp: '採購',
      iqc: '受檢',
      mc: '部管',
      smt: '物料',
      pcba: '製二課',
      assy: '製一課',
      qa: '品管課',
    },
    implLabel: {
      pmc: '生管課',
      mp: '採購課',
      iqc: '受檢課',
      mc: '部管課',
      smt: '物料',
      pcba: '製二課',
      assy: '製一課',
      qa: '品管課',
    },
    implementationStatus: {
      notStarted: '未開始',
      inProgress: '實施中',
      officiallyCompleted: '正式完成',
      fullyCompleted: '全部完成',
    },
  },
};
