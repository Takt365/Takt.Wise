// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter.Quartz
// 文件名称：TaktMeetingReminderScanJobHandler.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 扫描已排期会议并按 ReminderMinutes 向参会人发送开始前提醒邮件
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Routine.MeetingCenter.Quartz;

/// <summary>
/// 会议开始前提醒邮件扫描处理器
/// </summary>
public sealed class TaktMeetingReminderScanJobHandler : ITaktQuartzJobHandler
{
    private readonly ITaktMeetingNotificationDispatchService _meetingNotificationDispatchService;

    /// <summary>
    /// 初始化扫描处理器
    /// </summary>
    /// <param name="meetingNotificationDispatchService">会议通知派发服务</param>
    public TaktMeetingReminderScanJobHandler(ITaktMeetingNotificationDispatchService meetingNotificationDispatchService)
    {
        _meetingNotificationDispatchService = meetingNotificationDispatchService;
    }

    /// <inheritdoc />
    public string HandlerKey => TaktMeetingConstants.QuartzHandlerMeetingReminderScan;

    /// <inheritdoc />
    public async Task ExecuteAsync(TaktQuartzJobContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var task = context.Task;
        await _meetingNotificationDispatchService.SendDueMeetingRemindersAsync(
            task.TenantCode ?? string.Empty,
            task.CompanyCode ?? string.Empty,
            cancellationToken);
    }
}
