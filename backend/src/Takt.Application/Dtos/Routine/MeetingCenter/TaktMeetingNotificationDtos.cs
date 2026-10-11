// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationDtos.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingNotification 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMeetingNotification 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.MeetingCenter;

// ========================================
// MeetingNotification 响应 DTO
// ========================================

/// <summary>
/// 会议通知投递记录 按会议参会人持久化通知类型、发送状态与回执确认；邮件内携带 ConfirmReceiptToken 供收件人点击确认
/// 对应前端 TaktMeetingNotificationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMeetingNotificationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MeetingNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingNotificationId { get; set; }

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 会议 名称（填充字段）
    /// </summary>
    public string? MeetingName { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

    /// <summary>
    /// 参会人员 名称（填充字段）
    /// </summary>
    public string? MeetingAttendeeName { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int NotificationType { get; set; } = 0;

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int NotificationChannel { get; set; } = 0;

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 邮件主题
    /// </summary>
    public string NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    public string ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 会议（主表）
    /// （主表：TaktMeeting）
    /// </summary>
    public TaktMeetingDto? Meeting { get; set; }

    /// <summary>
    /// 参会人员（主子表关系）
    /// （主表：TaktMeetingAttendee）
    /// </summary>
    public TaktMeetingAttendeeDto? MeetingAttendee { get; set; }

}

// ========================================
// MeetingNotification 查询 DTO
// ========================================

/// <summary>
/// MeetingNotification 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMeetingNotificationQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingAttendeeId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string? MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string? MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    public string? RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int? NotificationType { get; set; }

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int? NotificationChannel { get; set; }

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 邮件主题
    /// </summary>
    public string? NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    public string? ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间（范围查询-开始）
    /// </summary>
    public DateTime? SentAtStart { get; set; }

    /// <summary>
    /// 发送时间（范围查询-结束）
    /// </summary>
    public DateTime? SentAtEnd { get; set; }

    /// <summary>
    /// 回执确认时间（范围查询-开始）
    /// </summary>
    public DateTime? ConfirmedAtStart { get; set; }

    /// <summary>
    /// 回执确认时间（范围查询-结束）
    /// </summary>
    public DateTime? ConfirmedAtEnd { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建MeetingNotification DTO
// ========================================

/// <summary>
/// 创建MeetingNotification DTO
/// </summary>
public class TaktMeetingNotificationCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    [Required(ErrorMessage = "会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）不能为空")]
    public string MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    [Required(ErrorMessage = "收件邮箱（员工档案 Email）不能为空")]
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int NotificationType { get; set; } = 0;

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int NotificationChannel { get; set; } = 0;

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 邮件主题
    /// </summary>
    [Required(ErrorMessage = "邮件主题不能为空")]
    public string NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "回执确认令牌（邮件链接参数；租户+公司内唯一）不能为空")]
    public string ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新MeetingNotification DTO
// ========================================

/// <summary>
/// 更新MeetingNotification DTO
/// 继承 TaktMeetingNotificationCreateDto，添加 MeetingNotificationId 字段
/// </summary>
public class TaktMeetingNotificationUpdateDto : TaktMeetingNotificationCreateDto
{
    /// <summary>
    /// MeetingNotificationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingNotificationId { get; set; }

}

// ========================================
// MeetingNotification 状态 DTO
// ========================================

/// <summary>
/// MeetingNotification 状态更新 DTO
/// </summary>
public class TaktMeetingNotificationStatusDto
{
    /// <summary>
    /// MeetingNotificationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingNotificationId { get; set; }

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    [Required(ErrorMessage = "投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）不能为空")]
    public int DeliveryStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MeetingNotification 导入模板行 DTO
/// </summary>
public class TaktMeetingNotificationTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingAttendeeId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string? MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string? MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    public string? RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int? NotificationType { get; set; }

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int? NotificationChannel { get; set; }

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 邮件主题
    /// </summary>
    public string? NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    public string? ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// MeetingNotification 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMeetingNotificationImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingAttendeeId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string? MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string? MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    public string? RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int? NotificationType { get; set; }

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int? NotificationChannel { get; set; }

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int? DeliveryStatus { get; set; }

    /// <summary>
    /// 邮件主题
    /// </summary>
    public string? NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    public string? ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// MeetingNotification 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMeetingNotificationExportDto
{
    /// <summary>
    /// MeetingNotificationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingNotificationId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    public string MeetingCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    public int NotificationType { get; set; } = 0;

    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    public int NotificationChannel { get; set; } = 0;

    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    public int DeliveryStatus { get; set; } = 0;

    /// <summary>
    /// 邮件主题
    /// </summary>
    public string NotificationSubject { get; set; } = string.Empty;

    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    public string ConfirmReceiptToken { get; set; } = string.Empty;

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }

    /// <summary>
    /// 确认人用户名
    /// </summary>
    public string? ConfirmedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    public string? SendErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 邮件链接回执确认请求（令牌）
/// </summary>
public class TaktMeetingNotificationConfirmReceiptByTokenDto
{
    /// <summary>
    /// 回执确认令牌（邮件链接 query 参数 token）
    /// </summary>
    public string ConfirmReceiptToken { get; set; } = string.Empty;
}

/// <summary>
/// 回执确认结果
/// </summary>
public class TaktMeetingNotificationConfirmReceiptResultDto
{
    /// <summary>
    /// 会议通知 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingNotificationId { get; set; }

    /// <summary>
    /// 会议标题（展示用）
    /// </summary>
    public string MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 是否此前已确认（幂等）
    /// </summary>
    public bool AlreadyConfirmed { get; set; }

    /// <summary>
    /// 回执确认时间
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }
}
