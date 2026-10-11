// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.MeetingCenter
// 文件名称：TaktMeetingNotification.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知投递记录（按参会人持久化；邮件发送与回执确认）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.MeetingCenter;

/// <summary>
/// 会议通知投递记录
/// 按会议参会人持久化通知类型、发送状态与回执确认；邮件内携带 ConfirmReceiptToken 供收件人点击确认
/// </summary>
[SugarTable("takt_routine_meeting_center_notification", "会议通知表")]
[SugarIndex("ix_meeting_notification_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_notification_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_notification_meeting_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MeetingId), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_notification_attendee_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MeetingAttendeeId), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_notification_confirm_token_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfirmReceiptToken), OrderByType.Asc, true)]
[SugarIndex("ix_meeting_notification_delivery_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeliveryStatus), OrderByType.Asc, false)]
public class TaktMeetingNotification : TaktCompanyEntityBase
{
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id；主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_id", ColumnDescription = "会议ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }
    /// <summary>
    /// 参会人员 ID（选项 TaktMeetingAttendees/options；DictValue=Id；主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_attendee_id", ColumnDescription = "参会人员ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }
    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_title", ColumnDescription = "会议标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string MeetingTitle { get; set; } = string.Empty;
    /// <summary>
    /// 会议编码（冗余字段，便于查询；与主表 TaktMeeting.MeetingCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_code", ColumnDescription = "会议编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MeetingCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
    /// <summary>
    /// 用户姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户姓名", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 收件邮箱（员工档案 Email）
    /// </summary>
    [SugarColumn(ColumnName = "recipient_email", ColumnDescription = "收件邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string RecipientEmail { get; set; } = string.Empty;
    /// <summary>
    /// 通知类型（字典 routine_meeting_center_notification_type；0=邀请 1=变更 2=取消 3=提醒）
    /// </summary>
    [SugarColumn(ColumnName = "notification_type", ColumnDescription = "通知类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NotificationType { get; set; } = 0;
    /// <summary>
    /// 通知渠道（字典 routine_meeting_center_notification_channel；0=邮件）
    /// </summary>
    [SugarColumn(ColumnName = "notification_channel", ColumnDescription = "通知渠道", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int NotificationChannel { get; set; } = 0;
    /// <summary>
    /// 投递状态（字典 routine_meeting_center_notification_status；0=待发送 1=已发送 2=已确认 3=发送失败）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_status", ColumnDescription = "投递状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeliveryStatus { get; set; } = 0;
    /// <summary>
    /// 邮件主题
    /// </summary>
    [SugarColumn(ColumnName = "notification_subject", ColumnDescription = "邮件主题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string NotificationSubject { get; set; } = string.Empty;
    /// <summary>
    /// 回执确认令牌（邮件链接参数；租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "confirm_receipt_token", ColumnDescription = "回执确认令牌", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
    public string ConfirmReceiptToken { get; set; } = string.Empty;
    /// <summary>
    /// 发送时间
    /// </summary>
    [SugarColumn(ColumnName = "sent_at", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? SentAt { get; set; }
    /// <summary>
    /// 回执确认时间
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_at", ColumnDescription = "回执确认时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ConfirmedAt { get; set; }
    /// <summary>
    /// 确认人用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by_user_id", ColumnDescription = "确认人用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedByUserId { get; set; }
    /// <summary>
    /// 确认人用户名
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by_user_name", ColumnDescription = "确认人用户名", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? ConfirmedByUserName { get; set; }
    /// <summary>
    /// 发送失败原因（SMTP 或校验错误摘要）
    /// </summary>
    [SugarColumn(ColumnName = "send_error_message", ColumnDescription = "发送失败原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? SendErrorMessage { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 会议（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MeetingId))]
    public TaktMeeting? Meeting { get; set; }
    /// <summary>
    /// 参会人员（主子表关系）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MeetingAttendeeId))]
    public TaktMeetingAttendee? MeetingAttendee { get; set; }
}
