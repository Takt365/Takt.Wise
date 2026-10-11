// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFlowApprovalTable.ts
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：审批业务物理表名常量（与实体 SugarTable 及表单 RelatedTableName 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 已接入通用 submit-by-table 的审批实体物理表名
 * 新增审批实体：继承 TaktApprovalEntityBase 后表名自动进入后端白名单，此处登记便于业务页引用
 */
export const TaktFlowApprovalTable = {
  /** 请假 */
  leave: 'takt_human_resource_attendance_leave',
  /** 加班 */
  overtime: 'takt_human_resource_attendance_overtime',
  /** 公告 */
  announcement: 'takt_routine_announcement',
  /** 会议 */
  meeting: 'takt_routine_meeting_center',
} as const

/** 审批表名联合类型 */
export type TaktFlowApprovalTableName = typeof TaktFlowApprovalTable[keyof typeof TaktFlowApprovalTable]
