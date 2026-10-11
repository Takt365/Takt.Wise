// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsOverviewPopover.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：统计概览 Popover 预览数据加载与行映射
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import type { StatsMetricPopoverRow } from '../components/StatsMetricPopoverPanel.vue';
import type { Message } from '@/types/foundation/message.types';
import type { Online } from '@/types/foundation/online.types';
import type { Announcement } from '@/types/routine/announcement.types';
import type { Ticket } from '@/types/routine/help-desk/ticket.types';
import type { Meeting } from '@/types/routine/meeting-center/meeting.types';
import type { MeetingNotification } from '@/types/routine/meeting-center/meetingNotification.types';
import type { FlowTodoItem } from '@/types/workflow/flowEngine.types';
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardListPreview,
} from './statsQuery';

/** sys_publish_status：已发布 */
const ANNOUNCEMENT_STATUS_PUBLISHED = 1;

/** 在线状态：在线 */
const ONLINE_STATUS_ONLINE = 0;

/** Popover 预览结果 */
export interface StatsOverviewPopoverPreview {
  rows: StatsMetricPopoverRow[];
  total: number;
}

/**
 * 格式化日期时间
 * @param value ISO 时间字符串
 * @returns {string} 展示文本
 */
function formatDateTime(value?: string): string {
  if (!value?.trim()) {
    return '';
  }
  const parsed = dayjs(value);
  return parsed.isValid() ? parsed.format('YYYY-MM-DD HH:mm') : value;
}

/**
 * 公告摘要
 * @param item 公告 DTO
 * @returns {string} 摘要
 */
function resolveAnnouncementSummary(item: Announcement): string {
  const summary = item.summary?.trim();
  if (summary) {
    return summary;
  }
  const plain = item.content?.replace(/<[^>]+>/g, ' ').replace(/\s+/g, ' ').trim();
  if (!plain) {
    return '';
  }
  return plain.length > 60 ? `${plain.slice(0, 60)}…` : plain;
}

/**
 * 加载统计概览 Popover 预览
 * @param key 指标键
 * @param hasPermission 权限判断
 * @param hasAny 任意权限判断
 * @returns {Promise<StatsOverviewPopoverPreview>} 预览数据
 */
export async function loadOverviewPopoverPreview(
  key: string,
  hasPermission: (permission: string) => boolean,
  hasAny: (permissions: string[]) => boolean,
): Promise<StatsOverviewPopoverPreview> {
  switch (key) {
    case 'todo': {
      if (!hasPermission(DASHBOARD_STATS_PERMISSION.flowTodoList)) {
        return { rows: [], total: 0 };
      }
      const res = await fetchDashboardListPreview<FlowTodoItem>(DASHBOARD_STATS_API.flowTodoList);
      return {
        total: res.total,
        rows: res.rows.map((item) => ({
          title: item.processTitle || item.processName,
          description: [
            item.taskName,
            item.startUserName,
            formatDateTime(item.startTime),
          ].filter(Boolean).join(' · '),
        })),
      };
    }
    case 'message': {
      if (!hasAny([
        DASHBOARD_STATS_PERMISSION.messageStatistics,
        DASHBOARD_STATS_PERMISSION.messageUnreadList,
      ])) {
        return { rows: [], total: 0 };
      }
      const res = await fetchDashboardListPreview<Message>(DASHBOARD_STATS_API.messageUnreadList);
      return {
        total: res.total,
        rows: res.rows.map((item) => ({
          title: item.messageTitle,
          description: [
            item.fromUserNickName || item.fromUserName,
            formatDateTime(item.sendTime),
          ].filter(Boolean).join(' · '),
        })),
      };
    }
    case 'meeting': {
      if (!hasPermission(DASHBOARD_STATS_PERMISSION.meetingList)) {
        return { rows: [], total: 0 };
      }
      const res = await fetchDashboardListPreview<Meeting>(DASHBOARD_STATS_API.meetingList);
      return {
        total: res.total,
        rows: res.rows.map((item) => ({
          title: item.meetingTitle || item.meetingCode || '',
          description: [
            item.organizerName,
            formatDateTime(item.startTime),
          ].filter(Boolean).join(' · '),
        })),
      };
    }
    case 'notification': {
      if (hasPermission(DASHBOARD_STATS_PERMISSION.announcementList)) {
        const res = await fetchDashboardListPreview<Announcement>(DASHBOARD_STATS_API.announcementList, {
          announcementStatus: ANNOUNCEMENT_STATUS_PUBLISHED,
        });
        return {
          total: res.total,
          rows: res.rows.map((item) => ({
            title: item.announcementTitle,
            description: [
              resolveAnnouncementSummary(item),
              formatDateTime(item.publishTime),
            ].filter(Boolean).join(' · '),
          })),
        };
      }
      if (hasPermission(DASHBOARD_STATS_PERMISSION.meetingNotificationList)) {
        const res = await fetchDashboardListPreview<MeetingNotification>(
          DASHBOARD_STATS_API.meetingNotificationList,
        );
        return {
          total: res.total,
          rows: res.rows.map((item) => ({
            title: item.notificationSubject || item.meetingTitle,
            description: [
              item.userName,
              formatDateTime(item.sentAt),
            ].filter(Boolean).join(' · '),
          })),
        };
      }
      return { rows: [], total: 0 };
    }
    case 'helpdesk': {
      if (!hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskTicketList)) {
        return { rows: [], total: 0 };
      }
      const res = await fetchDashboardListPreview<Ticket>(DASHBOARD_STATS_API.helpDeskTicketList);
      return {
        total: res.total,
        rows: res.rows.map((item) => ({
          title: item.ticketTitle || item.ticketCode,
          description: item.submitterName || item.ticketCode,
        })),
      };
    }
    case 'online': {
      if (!hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard)) {
        return { rows: [], total: 0 };
      }
      const onlineRes = await fetchDashboardListPreview<Online>(DASHBOARD_STATS_API.onlineList, {
        onlineStatus: ONLINE_STATUS_ONLINE,
      });
      return {
        total: onlineRes.total,
        rows: onlineRes.rows.map((item) => ({
          title: item.userName,
          description: [
            item.deviceType,
            item.connectIp,
          ].filter(Boolean).join(' · '),
        })),
      };
    }
    default:
      return { rows: [], total: 0 };
  }
}
