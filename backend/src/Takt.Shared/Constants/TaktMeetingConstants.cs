// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktMeetingConstants.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心业务常量（状态、Quartz 处理器键）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 会议中心业务常量
/// </summary>
public static class TaktMeetingConstants
{
    /// <summary>
    /// 会议状态：草稿
    /// </summary>
    public const int StatusDraft = 0;
    /// <summary>
    /// 会议状态：已排期
    /// </summary>
    public const int StatusScheduled = 1;
    /// <summary>
    /// 会议状态：进行中
    /// </summary>
    public const int StatusInProgress = 2;
    /// <summary>
    /// 会议状态：已结束
    /// </summary>
    public const int StatusEnded = 3;
    /// <summary>
    /// 会议状态：已取消
    /// </summary>
    public const int StatusCancelled = 4;
    /// <summary>
    /// Quartz 会议开始前提醒扫描处理器键（与 TaktQuartzTask.ClassName 一致）
    /// </summary>
    public const string QuartzHandlerMeetingReminderScan = "TaktMeetingReminderScanJobHandler";
    /// <summary>
    /// 通知投递状态：待发送
    /// </summary>
    public const int NotificationStatusPending = 0;
    /// <summary>
    /// 通知投递状态：已发送
    /// </summary>
    public const int NotificationStatusSent = 1;
    /// <summary>
    /// 通知投递状态：已确认
    /// </summary>
    public const int NotificationStatusConfirmed = 2;
    /// <summary>
    /// 通知投递状态：发送失败
    /// </summary>
    public const int NotificationStatusFailed = 3;
    /// <summary>
    /// 通知渠道：邮件
    /// </summary>
    public const int NotificationChannelEmail = 0;
    /// <summary>
    /// 通知类型：邀请
    /// </summary>
    public const int NotificationTypeInvitation = 0;
    /// <summary>
    /// 通知类型：变更
    /// </summary>
    public const int NotificationTypeUpdate = 1;
    /// <summary>
    /// 通知类型：取消
    /// </summary>
    public const int NotificationTypeCancellation = 2;
    /// <summary>
    /// 通知类型：提醒
    /// </summary>
    public const int NotificationTypeReminder = 3;
}
