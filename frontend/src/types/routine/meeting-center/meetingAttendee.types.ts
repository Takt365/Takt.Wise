// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/meeting-center
// 文件名称：meetingAttendee.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/meeting-center 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 参会人员子实体
 * 对应前端 TaktMeetingAttendeeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MeetingAttendee
 * @description 对应后端 TaktMeetingAttendeeDto
 */
export interface MeetingAttendee extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 会议 ID
   */
  meetingId?: string;

  /**
   * 用户 ID
   */
  userId?: string;

  /**
   * 用户姓名
   */
  userName?: string;

  /**
   * 参与角色
   */
  attendeeRole?: number;

  /**
   * 出席状态
   */
  attendanceStatus?: number;

  /**
   * 签到时间
   */
  checkInTime?: string;

  /**
   * 签退时间
   */
  checkOutTime?: string;

  /**
   * 签到方式（0=手动，1=扫码，2=人脸等）
   */
  checkInMethod?: number;

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
 * MeetingAttendee 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MeetingAttendeeExport
 * @description 对应后端 TaktMeetingAttendeeExportDto
 */
export interface MeetingAttendeeExport {
  /**
   * MeetingAttendeeID
   */
  meetingAttendeeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 会议 ID
   */
  meetingId: string;

  /**
   * 用户 ID
   */
  userId: string;

  /**
   * 用户姓名
   */
  userName: string;

  /**
   * 参与角色
   */
  attendeeRole: number;

  /**
   * 出席状态
   */
  attendanceStatus: number;

  /**
   * 签到时间
   */
  checkInTime?: string;

  /**
   * 签退时间
   */
  checkOutTime?: string;

  /**
   * 签到方式（0=手动，1=扫码，2=人脸等）
   */
  checkInMethod: number;

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

