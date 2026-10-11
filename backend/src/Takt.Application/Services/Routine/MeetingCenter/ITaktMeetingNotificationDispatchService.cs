// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：ITaktMeetingNotificationDispatchService.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知派发服务（落库 TaktMeetingNotification 后发送邮件）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议通知派发服务（按参会人落库并发送邮件）
/// </summary>
public interface ITaktMeetingNotificationDispatchService
{
    /// <summary>
    /// 向会议参会人员（未作废行）创建通知记录并发送邮件
    /// </summary>
    /// <param name="meetingId">会议 ID</param>
    /// <param name="kind">通知类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task NotifyMeetingAttendeesAsync(
        long meetingId,
        TaktMeetingNotificationKind kind,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 扫描并发送到达提醒时间的会议邮件（Quartz 定时任务入口）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task SendDueMeetingRemindersAsync(
        string tenantCode,
        string companyCode,
        CancellationToken cancellationToken = default);
}
