// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/meeting-center
// 文件名称：meetingMinutes.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/meeting-center 会后纪要类型（对齐 TaktMeetingMinutes 实体/DTO）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 会后纪要
 * @description 对应后端 TaktMeetingMinutesDto
 */
export interface MeetingMinutes extends CompanyDtoBase {
  /** MeetingMinutesID */
  meetingMinutesId: string;
  /** 会议 ID（选项 TaktMeetings/options） */
  meetingId: string;
  /** 会议标题（冗余字段，与主表 TaktMeeting.MeetingTitle 一致） */
  meetingTitle: string;
  /** 行号（纪要分项序号，固定步长=10；纪要通常为 10） */
  lineNumber: number;
  /** 会议纪要（会后纪要富文本 HTML） */
  meetingMinutes?: string;
  /** 摘要（纪要列表展示用） */
  meetingSummary?: string;
  /** 记录 ID（选项 TaktUsers/options） */
  recorderId?: string;
  /** 记录员（冗余字段，与 TaktUser.UserName 一致） */
  recorderName?: string;
  /** 文件名称（原始文件名，上传回填） */
  fileName?: string;
  /** 访问地址（文件访问 URL，上传回填） */
  accessUrl?: string;
  /** 是否作废（字典 sys_yes_no；0=否 1=是） */
  isObsolete: number;
  /** 会议（主表） */
  meeting?: Meeting;
}

/**
 * MeetingMinutes 分页查询
 * @description 对应后端 TaktMeetingMinutesQueryDto
 */
export interface MeetingMinutesQuery extends TaktPagedQuery {
  tenantCode?: string;
  companyCode?: string;
  plantCode?: string;
  meetingId?: string;
  meetingTitle?: string;
  lineNumber?: number;
  meetingMinutes?: string;
  meetingSummary?: string;
  recorderId?: string;
  recorderName?: string;
  fileName?: string;
  accessUrl?: string;
  isObsolete?: number;
  createdAtStart?: string;
  createdAtEnd?: string;
  extField?: string;
  remark?: string;
}

/**
 * 创建 MeetingMinutes
 * @description 对应后端 TaktMeetingMinutesCreateDto
 */
export interface MeetingMinutesCreate {
  tenantCode: string;
  companyCode: string;
  plantCode: string;
  cultureCode: string;
  meetingId: string;
  lineNumber: number;
  meetingMinutes?: string;
  meetingSummary?: string;
  recorderId?: string;
  recorderName?: string;
  fileName?: string;
  accessUrl?: string;
  isObsolete: number;
  extField?: string;
  remark?: string;
}

/**
 * 更新 MeetingMinutes
 * @description 对应后端 TaktMeetingMinutesUpdateDto
 */
export interface MeetingMinutesUpdate extends MeetingMinutesCreate {
  meetingMinutesId: string;
}

/**
 * MeetingMinutes 作废/撤销作废
 * @description 对应后端 TaktMeetingMinutesObsoleteDto
 */
export interface MeetingMinutesObsolete {
  meetingMinutesId: string;
  isObsolete: number;
}

/**
 * MeetingMinutes 导入模板行
 * @description 对应后端 TaktMeetingMinutesTemplateDto
 */
export interface MeetingMinutesTemplate {
  tenantCode?: string;
  companyCode?: string;
  plantCode?: string;
  meetingId?: string;
  lineNumber?: number;
  meetingMinutes?: string;
  meetingSummary?: string;
  recorderId?: string;
  recorderName?: string;
  fileName?: string;
  accessUrl?: string;
  isObsolete?: number;
  extField?: string;
  remark?: string;
}

/**
 * MeetingMinutes 导入
 * @description 对应后端 TaktMeetingMinutesImportDto
 */
export interface MeetingMinutesImport {
  tenantCode?: string;
  companyCode?: string;
  plantCode?: string;
  cultureCode?: string;
  meetingId?: string;
  lineNumber?: number;
  meetingMinutes?: string;
  meetingSummary?: string;
  recorderId?: string;
  recorderName?: string;
  fileName?: string;
  accessUrl?: string;
  isObsolete?: number;
  extField?: string;
  remark?: string;
}

/**
 * MeetingMinutes 导出
 * @description 对应后端 TaktMeetingMinutesExportDto
 */
export interface MeetingMinutesExport {
  meetingMinutesId: string;
  companyCode: string;
  meetingId: string;
  lineNumber: number;
  meetingMinutes?: string;
  meetingSummary?: string;
  recorderId?: string;
  recorderName?: string;
  fileName?: string;
  accessUrl?: string;
  isObsolete: number;
  extField?: string;
  remark?: string;
  createdAt: string;
}
