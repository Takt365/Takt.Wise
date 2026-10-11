// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-kanban
// 文件名称：en-US.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：Kanban page-only copy (no entity/dict seed); column titles prefer entity.ecgijutsu.* / entity.ecexec.* / org.dept.*. Keys logistics.manufacturing.engineering-change.ec-kanban.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    filter: {
      implementationStatus: 'Implementation status',
    },
    column: {
      detailCount: 'Detail lines',
      pendingCount: 'Pending',
      implementationStatus: 'Status',
      path: 'Implementation path',
    },
    implementationStatus: {
      notStarted: 'Not started',
      inProgress: 'In progress',
      officiallyCompleted: 'Officially completed',
      fullyCompleted: 'Fully completed',
    },
  },
};
