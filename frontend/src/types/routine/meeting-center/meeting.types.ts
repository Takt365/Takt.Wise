// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/meeting-center
// 文件名称：meeting.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/meeting-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理
 * 对应前端 TaktMeetingDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Meeting
 * @description 对应后端 TaktMeetingDto
 */
export interface Meeting extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 会议编码（租户+公司内唯一）
   */
  meetingCode?: string;

  /**
   * 会议标题
   */
  meetingTitle?: string;

  /**
   * 会议类型
   */
  meetingType?: number;

  /**
   * 会议状态
   */
  meetingStatus?: number;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程（会前）
   */
  meetingAgenda?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  meetingTags?: string;

  /**
   * 组织人 ID
   */
  organizerId?: string;

  /**
   * 组织人姓名
   */
  organizerName?: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxAttendees?: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes?: number;

  /**
   * 会议室 ID
   */
  meetingRoomId?: string;

  /**
   * 会议室名称（冗余快照）
   */
  meetingRoomName?: string;

  /**
   * 参与人列表（主子表关系）（子表，级联保存）
   */
  attendees?: MeetingAttendeeCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * Meeting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MeetingExport
 * @description 对应后端 TaktMeetingExportDto
 */
export interface MeetingExport {
  /**
   * MeetingID
   */
  meetingId: string;

  /**
   * 会议编码（租户+公司内唯一）
   */
  meetingCode: string;

  /**
   * 会议标题
   */
  meetingTitle: string;

  /**
   * 会议类型
   */
  meetingType: number;

  /**
   * 会议状态
   */
  meetingStatus: number;

  /**
   * 开始时间
   */
  startTime: string;

  /**
   * 结束时间
   */
  endTime: string;

  /**
   * 会议地点（线下会议室名称或地址）
   */
  location?: string;

  /**
   * 会议链接（线上会议 URL）
   */
  meetingLink?: string;

  /**
   * 会议议程（会前）
   */
  meetingAgenda?: string;

  /**
   * 标签（逗号分隔或 JSON 数组存储）
   */
  meetingTags?: string;

  /**
   * 组织人 ID
   */
  organizerId: string;

  /**
   * 组织人姓名
   */
  organizerName: string;

  /**
   * 主办部门 ID
   */
  deptId?: string;

  /**
   * 主办部门名称
   */
  deptName?: string;

  /**
   * 最大参会人数（0 表示不限）
   */
  maxAttendees: number;

  /**
   * 提前提醒分钟数（0 表示不提醒）
   */
  reminderMinutes: number;

  /**
   * 会议室 ID
   */
  meetingRoomId?: string;

  /**
   * 会议室名称（冗余快照）
   */
  meetingRoomName?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

