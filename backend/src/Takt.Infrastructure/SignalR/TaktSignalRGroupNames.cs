// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRGroupNames.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR Hub 组名约定（租户+公司隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR Hub 组名工具；推送路由：ConnectHub=在线/强退/在线统计，NotificationHub=私信/广播/消息统计（每类单 Hub 一次）
/// </summary>
internal static class TaktSignalRGroupNames
{
    /// <summary>
    /// 公司内用户组（ConnectHub 强退/在线统计；NotificationHub 私信/消息统计/工作流推送）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名</param>
    /// <returns>组名</returns>
    public static string UserGroup(string companyCode, string UserName) =>
        $"Company_{companyCode.Trim()}_User_{UserName.Trim()}";

    /// <summary>
    /// 公司内广播通知组
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <returns>组名</returns>
    public static string NotificationsGroup(string companyCode) =>
        $"Company_{companyCode}_Notifications";
}
