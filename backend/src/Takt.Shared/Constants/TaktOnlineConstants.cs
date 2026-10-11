// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktOnlineSessionActivityDefaults.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 在线会话活跃累计间隔（与 frontend takt-signalr heartbeatIntervalMs 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// SignalR 在线会话 Heartbeat 间隔（由 TaktOnlineService 引用）
/// </summary>
public static class TaktOnlineConstants
{
    /// <summary>
    /// Hub Heartbeat 周期（秒，与 frontend 30_000ms 一致）
    /// </summary>
    public const int ReportingIntervalSeconds = 30;

    /// <summary>
    /// 服务端去重：距上次累计不足该秒数则跳过（略小于上报周期）
    /// </summary>
    public const int MinReportingGapSeconds = 25;

    /// <summary>
    /// 延迟强退默认等待秒数（3 分钟）
    /// </summary>
    public const int DelayedKickSeconds = 180;

    /// <summary>
    /// 延迟强退允许的最大秒数（10 分钟）
    /// </summary>
    public const int MaxDelayedKickSeconds = 600;

    /// <summary>
    /// 强退落库消息类型（字典 sys_message_type DictValue）
    /// </summary>
    public const string KickMessageType = "system";

    /// <summary>
    /// 延迟强退预告消息分组（字典 sys_message_group DictValue）
    /// </summary>
    public const string KickScheduleMessageGroup = "reminder";

    /// <summary>
    /// 立即/到期强退执行消息分组（字典 sys_message_group DictValue）
    /// </summary>
    public const string KickExecuteMessageGroup = "message";
}
