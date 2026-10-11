// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：ja-JP.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設変看板のページ専有文案（entity/dict 種子なし）。列見出しは entity.ecgijutsu.* / entity.ecexec.* / org.dept.* を優先。参照键 logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    filter: {
      implementationStatus: '実施状態',
    },
    column: {
      detailCount: '明細数',
      pendingCount: '未実施数',
      implementationStatus: '実施状態',
      path: '実施パス',
    },
    implementationStatus: {
      notStarted: '未着手',
      inProgress: '実施中',
      officiallyCompleted: '正式完了',
      fullyCompleted: '全完了',
    },
  },
};
