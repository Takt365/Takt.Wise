// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine
// 文件名称：announcementStat.types.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知件数统计类型（数据看板；独立于 CRUD）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 公告通知件数统计查询
 * @description 对应后端 TaktAnnouncementStatQueryDto
 */
export interface AnnouncementStatQuery {
  /** 统计月份 yyyy-MM */
  statMonth?: string
  /** 发布时间-起 */
  publishTimeStart?: string
  /** 发布时间-止 */
  publishTimeEnd?: string
}

/**
 * 公告通知件数统计
 * @description 对应后端 TaktAnnouncementStatDto
 */
export interface AnnouncementStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 通知件数 */
  monthAnnouncementCount: number
}
