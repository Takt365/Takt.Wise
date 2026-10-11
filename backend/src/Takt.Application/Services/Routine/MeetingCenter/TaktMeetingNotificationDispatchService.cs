// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationDispatchService.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知派发：按参会人落库 TaktMeetingNotification 后发送含回执链接的邮件
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Routine.MeetingCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议通知派发服务
/// </summary>
public class TaktMeetingNotificationDispatchService : TaktServiceBase, ITaktMeetingNotificationDispatchService
{
    private readonly ITaktApprovalRepository<TaktMeeting> _meetingRepository;
    private readonly ITaktCompanyRepository<TaktMeetingAttendee> _meetingAttendeeRepository;
    private readonly ITaktCompanyRepository<TaktMeetingNotification> _meetingNotificationRepository;
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingRepository">会议仓储</param>
    /// <param name="meetingAttendeeRepository">参会人员仓储</param>
    /// <param name="meetingNotificationRepository">会议通知仓储</param>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="configuration">应用配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingNotificationDispatchService(
        ITaktApprovalRepository<TaktMeeting> meetingRepository,
        ITaktCompanyRepository<TaktMeetingAttendee> meetingAttendeeRepository,
        ITaktCompanyRepository<TaktMeetingNotification> meetingNotificationRepository,
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        IConfiguration configuration,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingRepository = meetingRepository;
        _meetingAttendeeRepository = meetingAttendeeRepository;
        _meetingNotificationRepository = meetingNotificationRepository;
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public async Task NotifyMeetingAttendeesAsync(
        long meetingId,
        TaktMeetingNotificationKind kind,
        CancellationToken cancellationToken = default)
    {
        if (meetingId <= 0)
        {
            return;
        }
        var meeting = await _meetingRepository.GetByIdAsync(meetingId);
        if (meeting == null)
        {
            LogWarning($"会议通知跳过：会议不存在 MeetingId={meetingId}");
            return;
        }
        if (!ShouldNotifyForStatus(meeting.MeetingStatus, kind))
        {
            return;
        }
        var notificationType = MapNotificationType(kind);
        var recipients = await ResolveAttendeeRecipientsAsync(meeting);
        if (recipients.Count == 0)
        {
            LogWarning($"会议通知跳过：无有效参会人邮箱 MeetingId={meetingId}, Kind={kind}");
            return;
        }
        var subject = BuildSubject(meeting, kind);
        foreach (var recipient in recipients)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (kind == TaktMeetingNotificationKind.Reminder
                && await HasReminderNotificationAsync(meeting, recipient.UserId, notificationType))
            {
                continue;
            }
            var token = GenerateConfirmReceiptToken();
            var entity = new TaktMeetingNotification
            {
                TenantCode = meeting.TenantCode,
                CompanyCode = meeting.CompanyCode,
                CultureCode = meeting.CultureCode,
                PlantCode = meeting.PlantCode,
                MeetingId = meeting.Id,
                MeetingAttendeeId = recipient.MeetingAttendeeId,
                MeetingTitle = meeting.MeetingTitle,
                MeetingCode = meeting.MeetingCode,
                UserId = recipient.UserId,
                UserName = recipient.DisplayName,
                RecipientEmail = recipient.Email,
                NotificationType = notificationType,
                NotificationChannel = TaktMeetingConstants.NotificationChannelEmail,
                DeliveryStatus = TaktMeetingConstants.NotificationStatusPending,
                NotificationSubject = subject,
                ConfirmReceiptToken = token,
            };
            entity = await _meetingNotificationRepository.CreateAsync(entity);
            var confirmUrl = BuildConfirmReceiptUrl(token);
            var body = BuildHtmlBody(meeting, kind, recipient.DisplayName, confirmUrl);
            try
            {
                await TaktMailHelper.SendEmailAsync(_configuration, recipient.Email, subject, body);
                entity.DeliveryStatus = TaktMeetingConstants.NotificationStatusSent;
                entity.SentAt = DateTime.Now;
                entity.SendErrorMessage = null;
                await _meetingNotificationRepository.UpdateAsync(entity);
                LogInformation(
                    "会议通知已发送: NotificationId={NotificationId}, MeetingId={MeetingId}, Kind={Kind}, UserId={UserId}",
                    entity.Id,
                    meetingId,
                    kind,
                    recipient.UserId);
            }
            catch (Exception ex)
            {
                entity.DeliveryStatus = TaktMeetingConstants.NotificationStatusFailed;
                entity.SendErrorMessage = TaktStringHelper.Truncate(ex.Message, 500);
                await _meetingNotificationRepository.UpdateAsync(entity);
                LogWarning(
                    $"会议通知发送失败: NotificationId={entity.Id}, MeetingId={meetingId}, Kind={kind}, Error={ex.Message}");
            }
        }
    }

    /// <inheritdoc />
    public async Task SendDueMeetingRemindersAsync(
        string tenantCode,
        string companyCode,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var now = DateTime.Now;
        var meetings = await _meetingRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.MeetingStatus == TaktMeetingConstants.StatusScheduled
            && x.ReminderMinutes > 0
            && x.StartTime > now);
        if (meetings.Count == 0)
        {
            return;
        }
        foreach (var meeting in meetings)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var remindAt = meeting.StartTime.AddMinutes(-meeting.ReminderMinutes);
            if (now < remindAt)
            {
                continue;
            }
            await NotifyMeetingAttendeesAsync(meeting.Id, TaktMeetingNotificationKind.Reminder, cancellationToken);
        }
    }

    /// <summary>
    /// 是否已存在有效提醒通知（已发送或已确认）
    /// </summary>
    /// <param name="meeting">会议</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="notificationType">通知类型</param>
    /// <returns>是否已存在</returns>
    private async Task<bool> HasReminderNotificationAsync(TaktMeeting meeting, long userId, int notificationType)
    {
        var existing = await _meetingNotificationRepository.FirstAsync(x =>
            x.MeetingId == meeting.Id
            && x.UserId == userId
            && x.NotificationType == notificationType
            && (x.DeliveryStatus == TaktMeetingConstants.NotificationStatusSent
                || x.DeliveryStatus == TaktMeetingConstants.NotificationStatusConfirmed));
        return existing != null;
    }

    /// <summary>
    /// 按参会人员解析收件信息
    /// </summary>
    /// <param name="meeting">会议主表</param>
    /// <returns>收件人列表</returns>
    private async Task<List<TaktMeetingMailRecipient>> ResolveAttendeeRecipientsAsync(TaktMeeting meeting)
    {
        var attendees = await _meetingAttendeeRepository.GetListAsync(x =>
            x.MeetingId == meeting.Id && x.IsObsolete == 0);
        if (attendees.Count == 0)
        {
            return new List<TaktMeetingMailRecipient>();
        }
        var userIds = attendees.Where(a => a.UserId > 0).Select(a => a.UserId).Distinct().ToList();
        if (userIds.Count == 0)
        {
            return new List<TaktMeetingMailRecipient>();
        }
        var users = await _userRepository.GetListAsync(u =>
            u.TenantCode == meeting.TenantCode && userIds.Contains(u.Id));
        var userMap = users.ToDictionary(u => u.Id);
        var employeeIds = users.Where(u => u.EmployeeId > 0).Select(u => u.EmployeeId).Distinct().ToList();
        var employees = employeeIds.Count > 0
            ? await _employeeRepository.GetListAsync(e =>
                e.TenantCode == meeting.TenantCode
                && e.CompanyCode == meeting.CompanyCode
                && employeeIds.Contains(e.Id))
            : new List<TaktEmployee>();
        var employeeEmailMap = employees
            .Where(e => !string.IsNullOrWhiteSpace(e.Email))
            .ToDictionary(e => e.Id, e => e.Email!.Trim());
        var result = new List<TaktMeetingMailRecipient>();
        foreach (var attendee in attendees)
        {
            if (!userMap.TryGetValue(attendee.UserId, out var user) || user.EmployeeId <= 0)
            {
                continue;
            }
            if (!employeeEmailMap.TryGetValue(user.EmployeeId, out var email)
                || !TaktRegexHelper.IsValidEmail(email))
            {
                continue;
            }
            var displayName = !string.IsNullOrWhiteSpace(attendee.UserName)
                ? attendee.UserName
                : user.NickName;
            result.Add(new TaktMeetingMailRecipient(attendee.Id, attendee.UserId, displayName, email));
        }
        return result;
    }

    /// <summary>
    /// 生成回执确认令牌
    /// </summary>
    /// <returns>URL 安全令牌</returns>
    private static string GenerateConfirmReceiptToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    /// <summary>
    /// 构建回执确认页 URL（基于 OpenIddict FrontendLoginUrl 推导前端根地址）
    /// </summary>
    /// <param name="token">回执令牌</param>
    /// <returns>确认链接</returns>
    private string BuildConfirmReceiptUrl(string token)
    {
        var openIddict = _configuration.BindOptions<TaktOpenIddictOptions>(TaktOpenIddictOptions.SectionName);
        var loginUrl = openIddict.FrontendLoginUrl?.Trim() ?? string.Empty;
        var baseUrl = loginUrl.EndsWith("/login", StringComparison.OrdinalIgnoreCase)
            ? loginUrl[..^"/login".Length]
            : loginUrl.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            baseUrl = "https://localhost:60081";
        }
        return $"{baseUrl}/meeting-notification/confirm?token={Uri.EscapeDataString(token)}";
    }

    /// <summary>
    /// 映射通知类型枚举到字典 int
    /// </summary>
    /// <param name="kind">派发类型</param>
    /// <returns>字典值</returns>
    private static int MapNotificationType(TaktMeetingNotificationKind kind) =>
        kind switch
        {
            TaktMeetingNotificationKind.Invitation => TaktMeetingConstants.NotificationTypeInvitation,
            TaktMeetingNotificationKind.Update => TaktMeetingConstants.NotificationTypeUpdate,
            TaktMeetingNotificationKind.Cancellation => TaktMeetingConstants.NotificationTypeCancellation,
            TaktMeetingNotificationKind.Reminder => TaktMeetingConstants.NotificationTypeReminder,
            _ => TaktMeetingConstants.NotificationTypeInvitation,
        };

    /// <summary>
    /// 当前会议状态是否允许发送指定类型通知
    /// </summary>
    /// <param name="meetingStatus">会议状态</param>
    /// <param name="kind">通知类型</param>
    /// <returns>是否发送</returns>
    private static bool ShouldNotifyForStatus(int meetingStatus, TaktMeetingNotificationKind kind) =>
        kind switch
        {
            TaktMeetingNotificationKind.Cancellation => meetingStatus == TaktMeetingConstants.StatusCancelled,
            TaktMeetingNotificationKind.Reminder => meetingStatus == TaktMeetingConstants.StatusScheduled,
            _ => meetingStatus == TaktMeetingConstants.StatusScheduled
                || meetingStatus == TaktMeetingConstants.StatusInProgress,
        };

    /// <summary>
    /// 构建邮件主题
    /// </summary>
    /// <param name="meeting">会议</param>
    /// <param name="kind">通知类型</param>
    /// <returns>主题</returns>
    private static string BuildSubject(TaktMeeting meeting, TaktMeetingNotificationKind kind)
    {
        var title = string.IsNullOrWhiteSpace(meeting.MeetingTitle) ? meeting.MeetingCode : meeting.MeetingTitle.Trim();
        var prefix = kind switch
        {
            TaktMeetingNotificationKind.Invitation => "【会议邀请】",
            TaktMeetingNotificationKind.Update => "【会议变更】",
            TaktMeetingNotificationKind.Cancellation => "【会议取消】",
            TaktMeetingNotificationKind.Reminder => "【会议提醒】",
            _ => "【会议通知】",
        };
        return $"{prefix}{title}";
    }

    /// <summary>
    /// 构建 HTML 邮件正文（含回执确认按钮）
    /// </summary>
    /// <param name="meeting">会议</param>
    /// <param name="kind">通知类型</param>
    /// <param name="recipientName">收件人</param>
    /// <param name="confirmUrl">回执确认链接</param>
    /// <returns>HTML 正文</returns>
    private static string BuildHtmlBody(
        TaktMeeting meeting,
        TaktMeetingNotificationKind kind,
        string recipientName,
        string confirmUrl)
    {
        var sb = new StringBuilder();
        sb.Append("<div style=\"font-family:Microsoft YaHei,Arial,sans-serif;font-size:14px;color:#333;\">");
        sb.Append("<p>").Append(WebUtility.HtmlEncode(recipientName)).Append("，您好：</p>");
        sb.Append("<p>");
        sb.Append(kind switch
        {
            TaktMeetingNotificationKind.Invitation => "您被邀请参加以下会议，请准时出席。",
            TaktMeetingNotificationKind.Update => "以下会议信息已更新，请留意最新安排。",
            TaktMeetingNotificationKind.Cancellation => "以下会议已取消，请知悉。",
            TaktMeetingNotificationKind.Reminder => "以下会议即将开始，请做好准备。",
            _ => "会议通知如下。",
        });
        sb.Append("</p>");
        sb.Append("<table style=\"border-collapse:collapse;width:100%;max-width:640px;\">");
        AppendRow(sb, "会议标题", meeting.MeetingTitle);
        AppendRow(sb, "会议编码", meeting.MeetingCode);
        AppendRow(sb, "开始时间", meeting.StartTime.ToString("yyyy-MM-dd HH:mm"));
        AppendRow(sb, "结束时间", meeting.EndTime.ToString("yyyy-MM-dd HH:mm"));
        AppendRow(sb, "会议室", meeting.MeetingRoomName);
        AppendRow(sb, "地点", meeting.Location);
        AppendRow(sb, "会议链接", meeting.MeetingLink);
        AppendRow(sb, "组织人", meeting.OrganizerName);
        AppendRow(sb, "主办部门", meeting.DeptName);
        sb.Append("</table>");
        if (!string.IsNullOrWhiteSpace(meeting.MeetingAgenda)
            && kind != TaktMeetingNotificationKind.Cancellation)
        {
            sb.Append("<p style=\"margin-top:16px;font-weight:600;\">会议议程</p>");
            sb.Append("<div>").Append(meeting.MeetingAgenda).Append("</div>");
        }
        if (kind != TaktMeetingNotificationKind.Cancellation)
        {
            sb.Append("<p style=\"margin-top:20px;\">");
            sb.Append("<a href=\"").Append(WebUtility.HtmlEncode(confirmUrl));
            sb.Append("\" style=\"display:inline-block;padding:10px 20px;background:#1677ff;color:#fff;text-decoration:none;border-radius:4px;\">");
            sb.Append("确认收到会议通知</a></p>");
        }
        sb.Append("<p style=\"margin-top:24px;color:#888;font-size:12px;\">本邮件由 Takt 会议中心自动发送，请勿直接回复。</p>");
        sb.Append("</div>");
        return sb.ToString();
    }

    /// <summary>
    /// 追加表格行
    /// </summary>
    /// <param name="sb">StringBuilder</param>
    /// <param name="label">标签</param>
    /// <param name="value">值</param>
    private static void AppendRow(StringBuilder sb, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }
        sb.Append("<tr><td style=\"padding:6px 12px 6px 0;color:#666;white-space:nowrap;vertical-align:top;\">")
            .Append(WebUtility.HtmlEncode(label))
            .Append("</td><td style=\"padding:6px 0;\">")
            .Append(WebUtility.HtmlEncode(value.Trim()))
            .Append("</td></tr>");
    }

    /// <summary>
    /// 邮件收件人
    /// </summary>
    /// <param name="MeetingAttendeeId">参会人 ID</param>
    /// <param name="UserId">用户 ID</param>
    /// <param name="DisplayName">显示名</param>
    /// <param name="Email">邮箱</param>
    private sealed record TaktMeetingMailRecipient(
        long MeetingAttendeeId,
        long UserId,
        string DisplayName,
        string Email);
}
