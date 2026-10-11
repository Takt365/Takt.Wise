// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine
// 文件名称：announcementStat.api.ts
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知看板统计 API（与 announcement CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  AnnouncementStat,
  AnnouncementStatQuery,
} from '@/types/routine/announcementStat.types'

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAnnouncementStats
 */
const ANNOUNCEMENT_STAT_API_BASE = 'TaktAnnouncementStats'

/**
 * 获取公告通知件数统计（数据看板；按 PublishTime）
 * @param {AnnouncementStatQuery} queryDto 查询参数
 * @returns {Promise<AnnouncementStat>} 公告通知件数统计
 */
export function getAnnouncementStat(queryDto: AnnouncementStatQuery): Promise<AnnouncementStat> {
  return request<AnnouncementStat>({
    url: `${ANNOUNCEMENT_STAT_API_BASE}/announcement-stat`,
    method: 'get',
    params: queryDto,
  })
}
