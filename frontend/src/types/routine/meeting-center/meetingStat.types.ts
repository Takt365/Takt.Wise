// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/meeting-center
// 文件名称：meetingStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：会议件数统计（数据看板 meeting-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 会议件数统计（按开始时间与状态）
 * @description 对应后端 TaktMeetingStatDto
 */
export interface MeetingStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 未开始件数 */
  monthNotStartedCount: number
  /** 已完成件数 */
  monthCompletedCount: number
}
