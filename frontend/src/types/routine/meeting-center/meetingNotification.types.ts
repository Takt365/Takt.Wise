// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/meeting-center
// 文件名称：meetingNotification.types.ts
// 创建时间：2026-08-26
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
 * 会议通知投递记录 按会议参会人持久化通知类型、发送状态与回执确认；邮件内携带 ConfirmReceiptToken 供收件人点击确认
 * 对应前端 TaktMeetingNotificationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MeetingNotification
 * @description 对应后端 TaktMeetingNotificationDto
 */
export interface MeetingNotification extends CompanyDtoBase {
  /**
   * MeetingNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  meetingNotificationId: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId: string;

  /**
   * 会议 名称（填充字段）
   */
  meetingName?: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId: string;

  /**
   * 参会人员 名称（填充字段）
   */
  meetingAttendeeName?: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus: number;

  /**
   * 邮件主题
   */
  notificationSubject: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken: string;

  /**
   * 发送时间
   */
  sentAt?: string;

  /**
   * 回执确认时间
   */
  confirmedAt?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

  /**
   * 会议（主表） （主表：TaktMeeting）
   */
  meeting?: Meeting;

  /**
   * 参会人员（主子表关系） （主表：TaktMeetingAttendee）
   */
  meetingAttendee?: MeetingAttendee;

}


/**
 * MeetingNotification 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MeetingNotificationQuery
 * @description 对应后端 TaktMeetingNotificationQueryDto
 */
export interface MeetingNotificationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId?: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId?: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle?: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode?: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail?: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType?: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel?: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus?: number;

  /**
   * 邮件主题
   */
  notificationSubject?: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken?: string;

  /**
   * 发送时间（范围查询-开始）
   */
  sentAtStart?: string;

  /**
   * 发送时间（范围查询-结束）
   */
  sentAtEnd?: string;

  /**
   * 回执确认时间（范围查询-开始）
   */
  confirmedAtStart?: string;

  /**
   * 回执确认时间（范围查询-结束）
   */
  confirmedAtEnd?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建MeetingNotification DTO
 * 对应前端 MeetingNotificationCreate
 * @description 对应后端 TaktMeetingNotificationCreateDto
 */
export interface MeetingNotificationCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus: number;

  /**
   * 邮件主题
   */
  notificationSubject: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken: string;

  /**
   * 发送时间
   */
  sentAt?: string;

  /**
   * 回执确认时间
   */
  confirmedAt?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

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
 * 更新MeetingNotification DTO
 * 继承 TaktMeetingNotificationCreateDto，添加 MeetingNotificationId 字段
 * 对应前端 MeetingNotificationUpdate
 * @description 对应后端 TaktMeetingNotificationUpdateDto
 */
export interface MeetingNotificationUpdate extends MeetingNotificationCreate {
  /**
   * MeetingNotificationID（标识要更新的实体）
   */
  meetingNotificationId: string;

}


/**
 * MeetingNotification 状态更新 DTO
 * 对应前端 MeetingNotificationStatus
 * @description 对应后端 TaktMeetingNotificationStatusDto
 */
export interface MeetingNotificationStatus {
  /**
   * MeetingNotificationID
   */
  meetingNotificationId: string;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus: number;

}


/**
 * MeetingNotification 导入模板行 DTO
 * 对应前端 MeetingNotificationTemplate
 * @description 对应后端 TaktMeetingNotificationTemplateDto
 */
export interface MeetingNotificationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId?: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId?: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle?: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode?: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail?: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType?: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel?: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus?: number;

  /**
   * 邮件主题
   */
  notificationSubject?: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken?: string;

  /**
   * 发送时间
   */
  sentAt?: string;

  /**
   * 回执确认时间
   */
  confirmedAt?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

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
 * MeetingNotification 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MeetingNotificationImport
 * @description 对应后端 TaktMeetingNotificationImportDto
 */
export interface MeetingNotificationImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId?: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId?: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle?: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode?: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName?: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail?: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType?: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel?: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus?: number;

  /**
   * 邮件主题
   */
  notificationSubject?: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken?: string;

  /**
   * 发送时间
   */
  sentAt?: string;

  /**
   * 回执确认时间
   */
  confirmedAt?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

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
 * MeetingNotification 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MeetingNotificationExport
 * @description 对应后端 TaktMeetingNotificationExportDto
 */
export interface MeetingNotificationExport {
  /**
   * MeetingNotificationID
   */
  meetingNotificationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
   */
  meetingId: string;

  /**
   * 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
   */
  meetingAttendeeId: string;

  /**
   * 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
   */
  meetingTitle: string;

  /**
   * 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
   */
  meetingCode: string;

  /**
   * 用户 ID（选项 TaktUsers/options；DictValue=Id）
   */
  userId: string;

  /**
   * 用户姓名（冗余字段，便于查询）
   */
  userName: string;

  /**
   * 收件邮箱（员工档案 Email）
   */
  recipientEmail: string;

  /**
   * 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
   */
  notificationType: number;

  /**
   * 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
   */
  notificationChannel: number;

  /**
   * 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
   */
  deliveryStatus: number;

  /**
   * 邮件主题
   */
  notificationSubject: string;

  /**
   * 回执确认令牌（邮件链接参数；租户+公司内唯一）
   */
  confirmReceiptToken: string;

  /**
   * 发送时间
   */
  sentAt?: string;

  /**
   * 回执确认时间
   */
  confirmedAt?: string;

  /**
   * 确认人用户 ID
   */
  confirmedByUserId?: string;

  /**
   * 确认人用户名
   */
  confirmedByUserName?: string;

  /**
   * 发送失败原因（SMTP 或校验错误摘要）
   */
  sendErrorMessage?: string;

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

