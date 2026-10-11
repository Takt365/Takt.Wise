// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktQuartzJobSignalRPushService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz Job 执行完成 SignalR 推送接口（Infrastructure 调度层）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;

namespace Takt.Domain.Interfaces;

/// <summary>
/// Quartz Job 执行完成 SignalR 推送接口
/// </summary>
public interface ITaktQuartzJobSignalRPushService
{
    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="task">定时任务实体（含最新统计字段）</param>
    /// <param name="log">执行日志</param>
    /// <param name="triggerUserName">触发用户名（手动触发时有值）</param>
    /// <returns>任务</returns>
    Task PushTaskExecutedAsync(
        TaktQuartzTask task,
        TaktQuartzLog log,
        string? triggerUserName);
}
