// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationKind.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议邮件通知类型（邀请/变更/取消/提醒）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议邮件通知类型
/// </summary>
public enum TaktMeetingNotificationKind
{
    /// <summary>
    /// 新建或首次排期邀请
    /// </summary>
    Invitation = 0,
    /// <summary>
    /// 会议信息或参会人变更
    /// </summary>
    Update = 1,
    /// <summary>
    /// 会议取消
    /// </summary>
    Cancellation = 2,
    /// <summary>
    /// 开始前提醒（ReminderMinutes）
    /// </summary>
    Reminder = 3,
}
